// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias AvaloniaRx;

using Avalonia;
using Avalonia.Media;
using ReactiveUI.Primitives;
using TUnit.Assertions;
using TUnit.Core;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
using RxEase = AvaloniaRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains Avalonia UI animation tests.</summary>
[NotInParallel]
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Defines an instant animation duration.</summary>
    private const double InstantDuration = 0.0;

    /// <summary>Defines the initial width.</summary>
    private const double InitialWidth = 12.0;

    /// <summary>Defines the target width.</summary>
    private const double TargetWidth = 48.0;

    /// <summary>Defines the initial height.</summary>
    private const double InitialHeight = 16.0;

    /// <summary>Defines the target height.</summary>
    private const double TargetHeight = 36.0;

    /// <summary>Defines the target opacity.</summary>
    private const double TargetOpacity = 0.35;

    /// <summary>Defines the target canvas left value.</summary>
    private const double TargetCanvasLeft = 10.0;

    /// <summary>Defines the target canvas top value.</summary>
    private const double TargetCanvasTop = 20.0;

    /// <summary>Defines the target canvas right value.</summary>
    private const double TargetCanvasRight = 30.0;

    /// <summary>Defines the target canvas bottom value.</summary>
    private const double TargetCanvasBottom = 40.0;

    /// <summary>Defines the target translate X value.</summary>
    private const double TargetTranslateX = 8.0;

    /// <summary>Defines the target translate Y value.</summary>
    private const double TargetTranslateY = 9.0;

    /// <summary>Defines the delta translate X value.</summary>
    private const double DeltaTranslateX = 2.0;

    /// <summary>Defines the delta translate Y value.</summary>
    private const double DeltaTranslateY = 3.0;

    /// <summary>Defines the target angle.</summary>
    private const double TargetAngle = 45.0;

    /// <summary>Defines the delta angle.</summary>
    private const double DeltaAngle = 15.0;

    /// <summary>Defines the target scale X value.</summary>
    private const double TargetScaleX = 1.25;

    /// <summary>Defines the target scale Y value.</summary>
    private const double TargetScaleY = 1.5;

    /// <summary>Defines the delta scale X value.</summary>
    private const double DeltaScaleX = 0.25;

    /// <summary>Defines the delta scale Y value.</summary>
    private const double DeltaScaleY = 0.5;

    /// <summary>Defines the target skew X value.</summary>
    private const double TargetSkewX = 4.0;

    /// <summary>Defines the target skew Y value.</summary>
    private const double TargetSkewY = 6.0;

    /// <summary>Defines the delta skew X value.</summary>
    private const double DeltaSkewX = 1.0;

    /// <summary>Defines the delta skew Y value.</summary>
    private const double DeltaSkewY = 2.0;

    /// <summary>Defines the shake amplitude.</summary>
    private const double ShakeAmplitude = 6.0;

    /// <summary>Defines a disabled animation count.</summary>
    private const int DisabledCount = 0;

    /// <summary>Defines an enabled animation count.</summary>
    private const int EnabledCount = 2;

    /// <summary>Defines the low pulse opacity.</summary>
    private const double PulseLow = 0.1;

    /// <summary>Defines the high pulse opacity.</summary>
    private const double PulseHigh = 0.9;

    /// <summary>Defines the assertion tolerance.</summary>
    private const double AssertionTolerance = 0.000000000001;

    /// <summary>Defines a negative repeat count.</summary>
    private const int NegativeRepeatCount = -1;

    /// <summary>Defines the target thickness.</summary>
    private static readonly Thickness TargetThickness = new(1.0, 2.0, 3.0, 4.0);

    /// <summary>Defines the target color.</summary>
    private static readonly Color TargetColor = Colors.CornflowerBlue;

    /// <summary>Verifies Avalonia control animations mutate their target properties.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task UiAnimationsMutateTargetProperties()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var button = CreateButton();
        var brush = new SolidColorBrush(Colors.Transparent);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.WidthTo(button, InstantDuration, TargetWidth, RxEase.None, scheduler));
        await Assert.That(button.Width).IsEqualTo(TargetWidth).Within(AssertionTolerance);

        button.Width = double.NaN;
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.WidthTo(button, InstantDuration, InitialWidth, RxEase.None, scheduler));
        await Assert.That(button.Width).IsEqualTo(InitialWidth).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.HeightTo(button, InstantDuration, TargetHeight, RxEase.None, scheduler));
        await Assert.That(button.Height).IsEqualTo(TargetHeight).Within(AssertionTolerance);

        button.Height = double.NaN;
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.HeightTo(button, InstantDuration, InitialHeight, RxEase.None, scheduler));
        await Assert.That(button.Height).IsEqualTo(InitialHeight).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.OpacityTo(button, InstantDuration, TargetOpacity, RxEase.None, scheduler));
        await Assert.That(button.Opacity).IsEqualTo(TargetOpacity).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.FadeIn(button, InstantDuration, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.FadeOut(button, InstantDuration, RxEase.None, scheduler));
        await Assert.That(button.Opacity).IsEqualTo(0.0).Within(AssertionTolerance);

        await ExerciseLayoutAnimationsAsync(button);
        await ExerciseBrushAnimationsAsync(brush);
    }

    /// <summary>Verifies Avalonia transform animations mutate their target transforms.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task TransformAnimationsMutateTargetTransforms()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var button = CreateButton();

        await ExerciseTranslateAnimationsAsync(button);
        await ExerciseRotateAnimationsAsync(button);
        await ExerciseScaleAnimationsAsync(button);
        await ExerciseSkewAnimationsAsync(button);
        await ExerciseEffectAnimationsAsync(button);
    }

    /// <summary>Verifies public Avalonia facade overloads can be invoked with fixture arguments.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task PublicFacadeOverloadsAreInvokable()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var context = new AvaloniaReflectionContext();

        InvokeFacadeMethods(typeof(RxAnimations), context);
        InvokeFacadeMethods(typeof(RxAnimationsExtensions), context);

        await Assert.That(context.InvokedCount).IsGreaterThan(0);
    }

    /// <summary>Verifies repeat animation edge cases through the Avalonia extension facade.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task RepeatAnimationHandlesEdgeCases()
    {
        var emptyValues = await RxAnimationsExtensions.RepeatAnimation(Observable.Return(Unit.Default), 0)
            .ToArray()
            .ToTask();
        await Assert.That(emptyValues).IsEmpty();

        await Assert.That(() => RxAnimationsExtensions.RepeatAnimation(null!, 1)).Throws<ArgumentNullException>();
        await Assert.That(() => RxAnimationsExtensions.RepeatAnimation(
                Observable.Return(Unit.Default),
                NegativeRepeatCount))
            .Throws<ArgumentOutOfRangeException>();
    }
}
