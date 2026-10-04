// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias AvaloniaReactiveRx;
extern alias WpfReactiveRx;
#else
extern alias AvaloniaRx;
extern alias WpfRx;
#endif

using System.Reflection;
using TUnit.Assertions;
using TUnit.Core;
#if REACTIVE_TESTS
using RxAvaloniaAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxWpfAnimations = WpfReactiveRx::CP.AnimationRx.Reactive.Animations;
#else
using RxAvaloniaAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxWpfAnimations = WpfRx::CP.AnimationRx.Animations;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies that corresponding platform packages expose matching animation contracts.</summary>
public sealed class ApiParityTests
{
    /// <summary>Verifies every public method signature and return type across corresponding platform types.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task PlatformPackages_PublicContracts_AreEquivalent()
    {
        var wpf = GetContracts(typeof(RxWpfAnimations).Assembly);
        var avalonia = GetContracts(typeof(RxAvaloniaAnimations).Assembly);
        await Assert.That(string.Join(Environment.NewLine, wpf)).IsEqualTo(string.Join(Environment.NewLine, avalonia));
    }

    /// <summary>Creates normalized public API signatures for a platform assembly.</summary>
    /// <param name="assembly">The package assembly.</param>
    /// <returns>The ordered contract signatures.</returns>
    private static IEnumerable<string> GetContracts(Assembly assembly)
    {
        var types = assembly.GetExportedTypes().Where(type => !type.IsNested).ToArray();
        var methods = types.SelectMany(type => type.GetMethods(
            BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(method => $"{type.Name}.{method.Name}`{method.GetGenericArguments().Length}" +
                $"({string.Join(",", method.GetParameters().Select(parameter => Normalize(parameter.ParameterType)))})" +
                $":{Normalize(method.ReturnType)}"));
        var values = types.Where(type => type.IsEnum)
            .SelectMany(type => Enum.GetNames(type)
                .Select(name => $"{type.Name}.{name}={Convert.ToInt64(Enum.Parse(type, name))}"));
        return methods.Concat(values).OrderBy(signature => signature, StringComparer.Ordinal);
    }

    /// <summary>Maps native platform and scheduler types to their equivalent contract names.</summary>
    /// <param name="type">The type to normalize.</param>
    /// <returns>The equivalent contract type name.</returns>
    private static string Normalize(Type type)
    {
        if (type.IsGenericType)
        {
            var name = type.Name.Split('`')[0];
            return $"{name}<{string.Join(",", type.GetGenericArguments().Select(Normalize))}>";
        }

        return type.Name switch
        {
            "FrameworkElement" or "UIElement" or "Visual" or "Control" or "TemplatedControl" => "Element",
            "ISequencer" or "IScheduler" => "Scheduler",
            "RxVoid" or "Unit" => "Unit",
            _ => type.Name
        };
    }
}
