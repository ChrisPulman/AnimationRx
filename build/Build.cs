using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AnimationRx.Build;
using CP.BuildTools;
using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace AnimationRx.Build;

sealed partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Test);

    private static AbsolutePath SolutionFile => RootDirectory / "src" / "AnimationRx.slnx";

    private static AbsolutePath TestResultsDirectory => RootDirectory / "TestResults";

    readonly Solution Solution = SolutionFile.ReadSolution();
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    static AbsolutePath PackagesDirectory => RootDirectory / "output";

    IEnumerable<Project> TestProjects => Solution.AllProjects
        .Where(project => project.Name.EndsWith(".Tests", StringComparison.Ordinal));

    Target Print => _ => _
        .Executes(() =>
        {
            Log.Information("Configuration = {Configuration}", Configuration);
            Log.Information("MinVerVersionOverride = {Value}", Environment.GetEnvironmentVariable("MinVerVersionOverride") ?? "<auto>");
            Log.Information("Test projects = {Projects}", string.Join(", ", TestProjects.Select(x => x.Name)));
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            if (IsLocalBuild)
            {
                return;
            }

            PackagesDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() => DotNetRestore(s => s.SetProjectFile(Solution)));

    Target Compile => _ => _
        .DependsOn(Restore, Print)
        .Executes(() => DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetNoRestore(true)));

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var project in TestProjects)
            {
                DotNetTest(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .SetNoBuild(true)
                    .SetNoRestore(true)
                    .SetResultsDirectory(TestResultsDirectory / "tests"));
            }
        });

    Target Coverage => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            TestResultsDirectory.CreateOrCleanDirectory();

            foreach (var project in TestProjects)
            {
                DotNetTest(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .SetNoBuild(true)
                    .SetNoRestore(true)
                    .SetResultsDirectory(TestResultsDirectory)
                    .AddProcessAdditionalArguments(
                        "--coverlet",
                        "--coverlet-output-format",
                        "cobertura",
                        "--coverlet-include",
                        "[AnimationRx.Wpf]*",
                        "--coverlet-include",
                        "[AnimationRx.Avalonia]*",
                        "--coverlet-exclude",
                        "[*.Tests]*",
                        "--coverlet-exclude-by-file",
                        "**/obj/**/*.cs",
                        "--coverlet-exclude-by-file",
                        "**/*.g.cs",
                        "--coverlet-exclude-by-file",
                        "**/*.g.i.cs",
                        "--coverlet-exclude-by-file",
                        "**/*AssemblyInfo.cs",
                        "--coverlet-exclude-by-attribute",
                        "GeneratedCodeAttribute",
                        "--coverlet-exclude-by-attribute",
                        "CompilerGeneratedAttribute",
                        "--coverlet-exclude-by-attribute",
                        "ExcludeFromCodeCoverageAttribute"));
            }

            VerifyCoverage();
        });

    static void VerifyCoverage()
    {
        var reports = TestResultsDirectory.GlobFiles("**/*cobertura*.xml").ToList();
        if (reports.Count == 0)
        {
            throw new InvalidOperationException($"No Cobertura coverage reports were produced under {TestResultsDirectory}.");
        }

        var expectedPackages = new[] { "AnimationRx.Avalonia", "AnimationRx.Wpf" };
        var packages = reports
            .SelectMany(report => XDocument.Load(report)
                .Descendants("package")
                .Select(package => new
                {
                    Report = report,
                    Name = package.Attribute("name")?.Value ?? string.Empty,
                    LineRate = ParseRate(package.Attribute("line-rate")?.Value),
                    BranchRate = ParseRate(package.Attribute("branch-rate")?.Value)
                }))
            .ToList();

        var actualPackages = packages.Select(x => x.Name).Distinct(StringComparer.Ordinal).OrderBy(x => x).ToArray();
        if (!expectedPackages.OrderBy(x => x).SequenceEqual(actualPackages, StringComparer.Ordinal))
        {
            throw new InvalidOperationException(
                $"Coverage must contain only {string.Join(", ", expectedPackages)}; found {string.Join(", ", actualPackages)}.");
        }

        foreach (var package in packages)
        {
            if (package.LineRate == 1m && package.BranchRate == 1m)
            {
                continue;
            }

            throw new InvalidOperationException(
                $"{package.Name} coverage is below 100% in {package.Report}: lines={package.LineRate:P2}, branches={package.BranchRate:P2}.");
        }
    }

    static decimal ParseRate(string? value) =>
        decimal.Parse(value ?? "0", NumberStyles.Number, CultureInfo.InvariantCulture);
}
