// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias AvaloniaReactiveRx;
#else
extern alias AvaloniaRx;
#endif

using System.Reflection;
using Avalonia.Controls;
using Avalonia.Media;
#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
#else
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
#endif
using TUnit.Assertions;
using TUnit.Core;
using TUnit.Core.Executors;
#if REACTIVE_TESTS
using ISequencer = System.Reactive.Concurrency.IScheduler;
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxEase = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Ease;
using Unit = System.Reactive.Unit;
#else
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxEase = AvaloniaRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies native transform and layout parity behavior on initially unconfigured elements.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Defines the transform endpoint.</summary>
    private const double ParityTransformTarget = 2;

    /// <summary>Defines the relative scale endpoint.</summary>
    private const double ParityRelativeScaleTarget = 3;

    /// <summary>Verifies every scalar transform creates and updates its transform when none exists.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task Transforms_FreshElements_CreateAndUpdateNativeTransforms()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var names = new[]
        {
            "TranslateTransform", "TranslateTo", "TranslateBy", "ScaleTransform", "ScaleTo", "ScaleBy",
            "RotateTransform", "RotateTo", "RotateBy", "SkewTransform", "SkewTo", "SkewBy", "ShakeTranslate"
        };
        foreach (var method in typeof(RxAnimations).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => names.Contains(method.Name) &&
                method.GetParameters().Last().ParameterType == typeof(ISequencer) &&
                method.GetParameters()[1].ParameterType == typeof(double)))
        {
            var button = new Button { Width = InitialWidth, Height = InitialHeight };
            await RunAnimationAsync(scheduler =>
            {
                var arguments = CreateTransformArguments(method, button, scheduler);
                return (IObservable<Unit>)method.Invoke(null, arguments)!;
            });
            if (method.Name.StartsWith("Scale", StringComparison.Ordinal))
            {
                var expected = method.Name == "ScaleBy" ? ParityRelativeScaleTarget : ParityTransformTarget;
                await Assert.That(GetTransform<ScaleTransform>(button).ScaleX).IsEqualTo(expected);
                await Assert.That(GetTransform<ScaleTransform>(button).ScaleY).IsEqualTo(expected);
            }
            else if (method.Name.StartsWith("Rotate", StringComparison.Ordinal))
            {
                await Assert.That(GetTransform<RotateTransform>(button).Angle).IsEqualTo(ParityTransformTarget);
            }
            else if (method.Name.StartsWith("Skew", StringComparison.Ordinal))
            {
                await Assert.That(GetTransform<SkewTransform>(button).AngleX).IsEqualTo(ParityTransformTarget);
                await Assert.That(GetTransform<SkewTransform>(button).AngleY).IsEqualTo(ParityTransformTarget);
            }
            else
            {
                var expected = method.Name == "ShakeTranslate" ? 0 : ParityTransformTarget;
                await Assert.That(GetTransform<TranslateTransform>(button).X).IsEqualTo(expected);
                await Assert.That(GetTransform<TranslateTransform>(button).Y).IsEqualTo(expected);
            }
        }
    }

    /// <summary>Creates deterministic arguments for an unconfigured transform animation.</summary>
    /// <param name="method">The animation method.</param>
    /// <param name="button">The target button.</param>
    /// <param name="scheduler">The timing scheduler.</param>
    /// <returns>The native animation arguments.</returns>
    private static object[] CreateTransformArguments(MethodInfo method, Button button, ISequencer scheduler)
    {
        return method.GetParameters().Select(parameter =>
        {
            if (parameter.Name == "element")
            {
        return (object)button;
            }

            if (parameter.ParameterType == typeof(ISequencer))
            {
        return scheduler;
            }

            if (parameter.ParameterType == typeof(RxEase))
            {
        return RxEase.None;
            }

            if (parameter.ParameterType == typeof(int))
            {
        return 1;
            }

            return parameter.Name == "milliSeconds" ? InstantDuration : ParityTransformTarget;
        }).ToArray();
    }
}
