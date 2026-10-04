// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias WpfReactiveRx;
#else
extern alias WpfRx;
#endif

using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
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
using RxAnimations = WpfReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxEase = WpfReactiveRx::CP.AnimationRx.Reactive.Ease;
using Unit = System.Reactive.Unit;
#else
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies native transform and layout parity behavior on initially unconfigured elements.</summary>
public sealed partial class WpfUiAnimationTests
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
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
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

    /// <summary>Verifies subscribed layout and color animations reach their native property endpoints.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task LayoutAndColor_Subscriptions_UpdateNativeProperties()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var button = new Button();
        await RunAnimationAsync(scheduler => RxAnimations.MarginTo(
            button,
            InstantDuration,
            TargetMargin,
            RxEase.None,
            scheduler));
        await Assert.That(button.Margin).IsEqualTo(TargetMargin);
        await RunAnimationAsync(scheduler => RxAnimations.PaddingTo(
            button,
            InstantDuration,
            TargetPadding,
            RxEase.None,
            scheduler));
        await Assert.That(button.Padding).IsEqualTo(TargetPadding);
        var brush = new SolidColorBrush();
        await RunAnimationAsync(scheduler => RxAnimations.BrushColorTo(
            brush,
            InstantDuration,
            TargetColor,
            RxEase.None,
            scheduler));
        await Assert.That(brush.Color).IsEqualTo(TargetColor);
    }

    /// <summary>Verifies canvas animations resolve unset positions and replace existing positions.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task CanvasPositions_UnsetAndExisting_ReachEndpoints()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var button = new Button();
        foreach (var initial in new[] { double.NaN, ParityTransformTarget })
        {
            Canvas.SetLeft(button, initial);
            Canvas.SetTop(button, initial);
            Canvas.SetRight(button, initial);
            Canvas.SetBottom(button, initial);
            await RunAnimationAsync(scheduler => RxAnimations.CanvasLeftTo(
                button,
            InstantDuration,
            TargetCanvasLeft,
            RxEase.None,
            scheduler));
            await RunAnimationAsync(scheduler => RxAnimations.CanvasTopTo(
                button,
            InstantDuration,
            TargetCanvasTop,
            RxEase.None,
            scheduler));
            await RunAnimationAsync(scheduler => RxAnimations.CanvasRightTo(
                button,
            InstantDuration,
            TargetCanvasRight,
            RxEase.None,
            scheduler));
            await RunAnimationAsync(scheduler => RxAnimations.CanvasBottomTo(
                button,
            InstantDuration,
            TargetCanvasBottom,
            RxEase.None,
            scheduler));
            await Assert.That(Canvas.GetLeft(button)).IsEqualTo(TargetCanvasLeft);
            await Assert.That(Canvas.GetTop(button)).IsEqualTo(TargetCanvasTop);
            await Assert.That(Canvas.GetRight(button)).IsEqualTo(TargetCanvasRight);
            await Assert.That(Canvas.GetBottom(button)).IsEqualTo(TargetCanvasBottom);
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
