// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias WpfRx;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using TUnit.Assertions;
using TUnit.Core;
using TUnit.Core.Executors;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains WPF UI animation tests.</summary>
[NotInParallel]
public sealed partial class WpfUiAnimationTests
{
    /// <summary>Defines an instant animation duration.</summary>
    private const double InstantDuration = 0.0;

    /// <summary>Defines the virtual animation advance in milliseconds.</summary>
    private const double VirtualAnimationAdvanceMilliseconds = 100.0;

    /// <summary>Defines the maximum virtual animation advance iterations.</summary>
    private const int VirtualAnimationAdvanceIterations = 10;

    /// <summary>Defines the initial element width.</summary>
    private const double InitialWidth = 12.0;

    /// <summary>Defines the target element width.</summary>
    private const double TargetWidth = 48.0;

    /// <summary>Defines the initial element height.</summary>
    private const double InitialHeight = 16.0;

    /// <summary>Defines the target element height.</summary>
    private const double TargetHeight = 36.0;

    /// <summary>Defines the target opacity.</summary>
    private const double TargetOpacity = 0.35;

    /// <summary>Defines the target canvas left position.</summary>
    private const double TargetCanvasLeft = 10.0;

    /// <summary>Defines the target canvas top position.</summary>
    private const double TargetCanvasTop = 20.0;

    /// <summary>Defines the target canvas right position.</summary>
    private const double TargetCanvasRight = 30.0;

    /// <summary>Defines the target canvas bottom position.</summary>
    private const double TargetCanvasBottom = 40.0;

    /// <summary>Defines the target translate X value.</summary>
    private const double TargetTranslateX = 8.0;

    /// <summary>Defines the target translate Y value.</summary>
    private const double TargetTranslateY = 9.0;

    /// <summary>Defines the translate X delta.</summary>
    private const double DeltaTranslateX = 2.0;

    /// <summary>Defines the translate Y delta.</summary>
    private const double DeltaTranslateY = 3.0;

    /// <summary>Defines the target rotation angle.</summary>
    private const double TargetAngle = 45.0;

    /// <summary>Defines the rotation angle delta.</summary>
    private const double DeltaAngle = 15.0;

    /// <summary>Defines the target scale X value.</summary>
    private const double TargetScaleX = 1.25;

    /// <summary>Defines the target scale Y value.</summary>
    private const double TargetScaleY = 1.5;

    /// <summary>Defines the scale X delta.</summary>
    private const double DeltaScaleX = 0.25;

    /// <summary>Defines the scale Y delta.</summary>
    private const double DeltaScaleY = 0.5;

    /// <summary>Defines the target skew X angle.</summary>
    private const double TargetSkewX = 4.0;

    /// <summary>Defines the target skew Y angle.</summary>
    private const double TargetSkewY = 6.0;

    /// <summary>Defines the skew X delta.</summary>
    private const double DeltaSkewX = 1.0;

    /// <summary>Defines the skew Y delta.</summary>
    private const double DeltaSkewY = 2.0;

    /// <summary>Defines the shake amplitude.</summary>
    private const double ShakeAmplitude = 6.0;

    /// <summary>Defines a disabled shake count.</summary>
    private const int DisabledShakeCount = 0;

    /// <summary>Defines an enabled shake count.</summary>
    private const int EnabledShakeCount = 2;

    /// <summary>Defines the pulse low opacity.</summary>
    private const double PulseLow = 0.1;

    /// <summary>Defines the pulse high opacity.</summary>
    private const double PulseHigh = 0.9;

    /// <summary>Defines a disabled pulse count.</summary>
    private const int DisabledPulseCount = 0;

    /// <summary>Defines an enabled pulse count.</summary>
    private const int EnabledPulseCount = 2;

    /// <summary>Defines the finite repeat count.</summary>
    private const int FiniteRepeatCount = 2;

    /// <summary>Defines a negative repeat count.</summary>
    private const int NegativeRepeatCount = -1;

    /// <summary>Defines the assertion tolerance.</summary>
    private const double AssertionTolerance = 0.000000000001;

    /// <summary>Defines the target margin.</summary>
    private static readonly Thickness TargetMargin = new(1.0, 2.0, 3.0, 4.0);

    /// <summary>Defines the target padding.</summary>
    private static readonly Thickness TargetPadding = new(5.0, 6.0, 7.0, 8.0);

    /// <summary>Defines the target brush color.</summary>
    private static readonly Color TargetColor = Colors.CornflowerBlue;

    /// <summary>Defines the virtual animation advance.</summary>
    private static readonly TimeSpan VirtualAnimationAdvance =
        TimeSpan.FromMilliseconds(VirtualAnimationAdvanceMilliseconds);

    /// <summary>Verifies WPF UI animations mutate their target properties.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task UiAnimationsMutateTargetProperties()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var button = new Button
        {
            Width = InitialWidth,
            Height = InitialHeight,
            Opacity = 1.0,
            Margin = default,
            Padding = default
        };

        await RunAnimationAsync(scheduler =>
            RxAnimations.WidthTo(button, InstantDuration, TargetWidth, RxEase.None, scheduler));
        await Assert.That(button.Width).IsEqualTo(TargetWidth).Within(AssertionTolerance);

        button.Width = double.NaN;
        await RunAnimationAsync(scheduler =>
            RxAnimations.WidthTo(button, InstantDuration, InitialWidth, RxEase.None, scheduler));
        await Assert.That(button.Width).IsEqualTo(InitialWidth).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.HeightTo(button, InstantDuration, TargetHeight, RxEase.None, scheduler));
        await Assert.That(button.Height).IsEqualTo(TargetHeight).Within(AssertionTolerance);

        button.Height = double.NaN;
        await RunAnimationAsync(scheduler =>
            RxAnimations.HeightTo(button, InstantDuration, InitialHeight, RxEase.None, scheduler));
        await Assert.That(button.Height).IsEqualTo(InitialHeight).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.OpacityTo(button, InstantDuration, TargetOpacity, RxEase.None, scheduler));
        await Assert.That(button.Opacity).IsEqualTo(TargetOpacity).Within(AssertionTolerance);

        await Assert.That(RxAnimations.FadeIn(button, InstantDuration, RxEase.None, Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.FadeOut(button, InstantDuration, RxEase.None, Sequencer.Immediate))
            .IsNotNull();

        await Assert.That(RxAnimations.MarginTo(
                button,
                InstantDuration,
                TargetMargin,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.PaddingTo(
                button,
                InstantDuration,
                TargetPadding,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();

        await ExerciseCanvasAnimationsAsync(button);
        await ExerciseBrushAnimationsAsync();
    }

    /// <summary>Verifies WPF transform animations mutate their target transforms.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task TransformAnimationsMutateTargetTransforms()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var button = new Button
        {
            Width = InitialWidth,
            Height = InitialHeight
        };

        await ExerciseTranslateAnimationsAsync(button);
        await ExerciseRotateAnimationsAsync(button);
        await ExerciseScaleAnimationsAsync(button);
        await ExerciseSkewAnimationsAsync(button);
        await ExerciseEffectAnimationsAsync(button);
    }

    /// <summary>Verifies WPF observable margin and transform overloads execute.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task ObservableOverloadsExecute()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var button = new Button();
        var duration = Observable.Return(InstantDuration);
        var position = Observable.Return(TargetCanvasLeft);
        var ease = Observable.Return(RxEase.None);

        await RunAnimationAsync(scheduler =>
            RxAnimations.LeftMarginMove(button, duration, position, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.LeftMarginMove(button, duration, position, ease, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.RightMarginMove(button, duration, position, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.RightMarginMove(button, duration, position, ease, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.TopMarginMove(button, duration, position, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.TopMarginMove(button, duration, position, ease, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.BottomMarginMove(button, duration, position, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimations.BottomMarginMove(button, duration, position, ease, scheduler));

        await RxAnimations.TranslateTransform(
            button,
            duration,
            Observable.Return(new Point(TargetTranslateX, TargetTranslateY)),
            RxEase.None,
            RxEase.None).ToTask();
        await RxAnimations.RotateTransform(button, duration, position, RxEase.None).ToTask();
        await RxAnimations.RotateTransform(button, duration, position, ease).ToTask();

        await Assert.That(button.Margin.Bottom).IsEqualTo(TargetCanvasLeft).Within(AssertionTolerance);
    }

    /// <summary>Verifies public facade overloads can be invoked with valid fixture arguments.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task PublicFacadeOverloadsAreInvokable()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var context = new WpfReflectionContext();

        InvokeFacadeMethods(typeof(RxAnimations), context);
        InvokeFacadeMethods(typeof(WpfRx::CP.AnimationRx.AnimationsExtensions), context);

        await Assert.That(context.InvokedCount).IsGreaterThan(0);
    }

    /// <summary>Verifies repeat animation handles edge cases through the WPF facade.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task RepeatAnimationHandlesEdgeCases()
    {
        var emptyValues = await RxAnimations.RepeatAnimation(Observable.Return(Unit.Default), 0).ToArray().ToTask();
        await Assert.That(emptyValues).IsEmpty();

        var finiteValues = await RxAnimations.RepeatAnimation(
            Observable.Return(Unit.Default),
            FiniteRepeatCount).ToArray().ToTask();
        await Assert.That(finiteValues).Count().IsEqualTo(FiniteRepeatCount);

        await Assert.That(() => RxAnimations.RepeatAnimation(null!, 1)).Throws<ArgumentNullException>();
        await Assert.That(() => RxAnimations.RepeatAnimation(Observable.Return(Unit.Default), NegativeRepeatCount))
            .Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>Exercises canvas animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseCanvasAnimationsAsync(Button button)
    {
        await Assert.That(RxAnimations.CanvasLeftTo(
                button,
                InstantDuration,
                TargetCanvasLeft,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.CanvasTopTo(
                button,
                InstantDuration,
                TargetCanvasTop,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.CanvasRightTo(
                button,
                InstantDuration,
                TargetCanvasRight,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.CanvasBottomTo(
                button,
                InstantDuration,
                TargetCanvasBottom,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
    }

    /// <summary>Exercises brush animations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseBrushAnimationsAsync()
    {
        var brush = new SolidColorBrush(Colors.Transparent);
        await Assert.That(RxAnimations.BrushColorTo(
                brush,
                InstantDuration,
                TargetColor,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimations.ColorTo(
                brush,
                InstantDuration,
                TargetColor,
                RxEase.None,
                Sequencer.Immediate))
            .IsNotNull();
    }
}
