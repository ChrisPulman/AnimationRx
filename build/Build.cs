using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.PowerShell;
using CP.BuildTools;

namespace AnimationRx.Build;

////[GitHubActions(
////    "BuildOnly",
////    GitHubActionsImage.WindowsLatest,
////    OnPushBranchesIgnore = new[] { "main" },
////    FetchDepth = 0,
////    InvokedTargets = new[] { nameof(Compile) })]
////[GitHubActions(
////    "BuildDeploy",
////    GitHubActionsImage.WindowsLatest,
////    OnPushBranches = new[] { "main" },
////    FetchDepth = 0,
////    ImportSecrets = new[] { nameof(NuGetApiKey) },
////    InvokedTargets = new[] { nameof(Compile), nameof(Deploy) })]
sealed partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [GitRepository] readonly GitRepository Repository;
    [Solution(GenerateProjects = true)] readonly Solution Solution;
    [NerdbankGitVersioning] readonly NerdbankGitVersioning NerdbankVersioning;
    [Parameter][Secret] readonly string NuGetApiKey;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    AbsolutePath PackagesDirectory => RootDirectory / "output";

    AbsolutePath CoverageDirectory => RootDirectory / "TestResults" / "coverage";

    Target Print => _ => _
        .Executes(() => Log.Information("NerdbankVersioning = {Value}", NerdbankVersioning.NuGetPackageVersion));

    Target Clean => _ => _
        .Before(Restore)
        .Executes(async () =>
        {
            if (IsLocalBuild)
            {
                return;
            }

            PackagesDirectory.CreateOrCleanDirectory();
            DotNetWorkloadUpdate();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() => DotNetRestore(s => s.SetProjectFile(Solution)));

    Target Compile => _ => _
        .DependsOn(Restore, Print)
        .Executes(() => DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()));

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            CoverageDirectory.CreateOrCleanDirectory();

            var testProjects = Solution.AllProjects
                .Where(project => project.Name.EndsWith("Tests", StringComparison.Ordinal))
                .ToList();

            if (testProjects.Count == 0)
            {
                throw new InvalidOperationException("No test projects were found in the solution.");
            }

            DotNetTest(settings => settings
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .CombineWith(testProjects, (testSettings, project) => testSettings
                    .SetProjectFile(project)
                    .SetProcessAdditionalArguments(
                        "--coverlet",
                        "--coverlet-output-format",
                        "cobertura",
                        "--coverlet-file-prefix",
                        project.Name,
                        "--results-directory",
                        CoverageDirectory,
                        "--coverlet-include",
                        "[AnimationRx.Wpf]*",
                        "--coverlet-include",
                        "[AnimationRx.Avalonia]*",
                        "--coverlet-exclude",
                        "[AnimationRx.Tests]*",
                        "--coverlet-exclude",
                        "[TUnit*]*",
                        "--coverlet-exclude",
                        "[Microsoft.*]*",
                        "--coverlet-exclude",
                        "[System.*]*",
                        "--coverlet-exclude-by-file",
                        "**/AnimationRx.Wpf/Animations.cs",
                        "--coverlet-exclude-by-file",
                        "**/AnimationRx.Avalonia/Animations.cs")));

            ValidateCoverageReports();
        });

    Target Pack => _ => _
    .DependsOn(Test)
    .Produces(PackagesDirectory / "*.nupkg")
    .Executes(() =>
    {
        if (Repository.IsOnMainOrMasterBranch())
        {
            var packableProjects = Solution.GetPackableProjects();

            foreach (var project in packableProjects!)
            {
                Log.Information("Packing {Project}", project.Name);
            }

            DotNetPack(settings => settings
                .SetConfiguration(Configuration)
                .SetVersion(NerdbankVersioning.NuGetPackageVersion)
                .SetOutputDirectory(PackagesDirectory)
                .CombineWith(packableProjects, (packSettings, project) =>
                    packSettings.SetProject(project)));
        }
    });

    Target Deploy => _ => _
    .DependsOn(Pack)
    .Requires(() => NuGetApiKey)
    .Executes(() =>
    {
        if (Repository.IsOnMainOrMasterBranch())
        {
            DotNetNuGetPush(settings => settings
                        .SetSource(this.PublicNuGetSource())
                        .SetSkipDuplicate(true)
                        .SetApiKey(NuGetApiKey)
                        .CombineWith(PackagesDirectory.GlobFiles("*.nupkg"), (s, v) => s.SetTargetPath(v)),
                    degreeOfParallelism: 5, completeOnFailure: true);
        }
    });

    void ValidateCoverageReports()
    {
        var reports = CoverageDirectory.GlobFiles("*.xml").ToList();
        if (reports.Count == 0)
        {
            throw new InvalidOperationException($"No Cobertura coverage reports were found in {CoverageDirectory}.");
        }

        foreach (var report in reports)
        {
            ValidateCoverageReport(report);
        }
    }

    static void ValidateCoverageReport(AbsolutePath report)
    {
        var document = XDocument.Load(report);
        var root = document.Root ?? throw new InvalidOperationException($"Coverage report {report} has no root element.");

        EnsureRate(root, "line-rate", report);
        EnsureRate(root, "branch-rate", report);
        EnsureNoTestCodeIncluded(document, report);
        EnsureAllMethodsFullyCovered(document, report);

        Log.Information("Validated 100% line, branch, and method coverage for {Report}", report);
    }

    static void EnsureRate(XElement element, string attributeName, AbsolutePath report)
    {
        var rate = ReadRate(element, attributeName, report);
        if (rate != 1m)
        {
            throw new InvalidOperationException(
                $"{report} reported {attributeName} {rate:P2}; expected 100.00%.");
        }
    }

    static void EnsureNoTestCodeIncluded(XDocument document, AbsolutePath report)
    {
        var testEntries = document
            .Descendants("class")
            .Where(element => ContainsTestCode(element.Attribute("name")?.Value) ||
                              ContainsTestCode(element.Attribute("filename")?.Value))
            .Select(element => element.Attribute("name")?.Value ?? element.Attribute("filename")?.Value ?? "unknown")
            .ToList();

        if (testEntries.Count > 0)
        {
            throw new InvalidOperationException(
                $"{report} includes test code in coverage results: {string.Join(", ", testEntries)}.");
        }
    }

    static void EnsureAllMethodsFullyCovered(XDocument document, AbsolutePath report)
    {
        var missedMethods = document
            .Descendants("method")
            .Where(method => ReadRate(method, "line-rate", report) != 1m ||
                             ReadRate(method, "branch-rate", report) != 1m)
            .Select(MethodDisplayName)
            .ToList();

        if (missedMethods.Count > 0)
        {
            throw new InvalidOperationException(
                $"{report} contains methods below 100% coverage: {string.Join(", ", missedMethods)}.");
        }
    }

    static decimal ReadRate(XElement element, string attributeName, AbsolutePath report)
    {
        var value = element.Attribute(attributeName)?.Value;
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"{report} does not contain the expected '{attributeName}' attribute.");
        }

        return decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    static bool ContainsTestCode(string? value) =>
        value?.Contains("AnimationRx.Tests", StringComparison.OrdinalIgnoreCase) == true ||
        value?.Contains(".Tests.", StringComparison.OrdinalIgnoreCase) == true ||
        value?.Contains(".Tests/", StringComparison.OrdinalIgnoreCase) == true ||
        value?.Contains(".Tests\\", StringComparison.OrdinalIgnoreCase) == true;

    static string MethodDisplayName(XElement method)
    {
        var className = method.Ancestors("class").FirstOrDefault()?.Attribute("name")?.Value;
        var methodName = method.Attribute("name")?.Value;

        return string.IsNullOrWhiteSpace(className)
            ? methodName ?? "unknown"
            : $"{className}.{methodName}";
    }
}
