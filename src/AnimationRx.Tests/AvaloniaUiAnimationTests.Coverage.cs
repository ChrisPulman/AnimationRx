// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias AvaloniaRx;

using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI.Primitives.Concurrency;
using ReactiveUI.Primitives.Disposables;
using TUnit.Assertions;
using TUnit.Core;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
using RxEase = AvaloniaRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains Avalonia coverage-focused animation tests.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Defines a progress value below the interpolation minimum.</summary>
    private const double BelowMinimumProgress = -1.0;

    /// <summary>Defines a progress value above the interpolation maximum.</summary>
    private const double AboveMaximumProgress = 2.0;

    /// <summary>Verifies Avalonia transform helper branches are exercised.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AvaloniaTransformHelperBranchesAreCovered()
    {
        using var schedulerOverride = OverrideUiScheduler();

        var groupButton = CreateButton();
        var group = new TransformGroup();
        group.Children.Add(new RotateTransform());
        groupButton.RenderTransform = group;
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateTransform(
            groupButton,
            InstantDuration,
            TargetTranslateX,
            TargetTranslateY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(group.Children.OfType<TranslateTransform>().Any()).IsTrue();

        var matchingTransformButton = CreateButton();
        matchingTransformButton.RenderTransform = new TranslateTransform();
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateTransform(
            matchingTransformButton,
            InstantDuration,
            TargetTranslateX,
            TargetTranslateY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(((TransformGroup)matchingTransformButton.RenderTransform!).Children
            .OfType<TranslateTransform>()
            .Any()).IsTrue();

        var singleTransformButton = CreateButton();
        singleTransformButton.RenderTransform = new RotateTransform();
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateTransform(
            singleTransformButton,
            InstantDuration,
            TargetTranslateX,
            TargetTranslateY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(((TransformGroup)singleTransformButton.RenderTransform!).Children
            .OfType<TranslateTransform>()
            .Any()).IsTrue();
    }

    /// <summary>Verifies Avalonia private math and scheduler helper branches are exercised.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AvaloniaPrivateHelperBranchesAreCovered()
    {
        var lowThickness = InvokeAvaloniaLerp(default, TargetThickness, BelowMinimumProgress);
        var highColor = InvokeAvaloniaLerp(Colors.Transparent, TargetColor, AboveMaximumProgress);
        var scheduler = InvokeAvaloniaCreateUiScheduler();

        await Assert.That(lowThickness).IsEqualTo(default(Thickness));
        await Assert.That(highColor).IsEqualTo(TargetColor);
        await Assert.That(scheduler).IsNotNull();
    }

    /// <summary>Verifies Avalonia wrapper overloads are exercised directly.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AvaloniaWrapperOverloadsAreCovered()
    {
        var source = Observable.Return(1);
        var never = Observable.Create<Unit>(_ => EmptyDisposable.Instance);

        await Assert.That(RxAnimationsExtensions.TakeOneEvery(source, TimeSpan.Zero)).IsNotNull();
        await Assert.That(RxAnimationsExtensions.RepeatAnimation(never)).IsNotNull();
    }

    /// <summary>Invokes the private Avalonia thickness interpolation helper.</summary>
    /// <param name="start">The starting thickness.</param>
    /// <param name="end">The ending thickness.</param>
    /// <param name="progress">The interpolation progress.</param>
    /// <returns>The interpolated thickness.</returns>
    private static Thickness InvokeAvaloniaLerp(Thickness start, Thickness end, double progress) =>
        InvokeAvaloniaLerpCore<Thickness>(
            [typeof(Thickness), typeof(Thickness), typeof(double)],
            start,
            end,
            progress);

    /// <summary>Invokes the private Avalonia color interpolation helper.</summary>
    /// <param name="start">The starting color.</param>
    /// <param name="end">The ending color.</param>
    /// <param name="progress">The interpolation progress.</param>
    /// <returns>The interpolated color.</returns>
    private static Color InvokeAvaloniaLerp(Color start, Color end, double progress) =>
        InvokeAvaloniaLerpCore<Color>([typeof(Color), typeof(Color), typeof(double)], start, end, progress);

    /// <summary>Invokes the private Avalonia interpolation helper.</summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="parameterTypes">The interpolation method parameter types.</param>
    /// <param name="arguments">The interpolation method arguments.</param>
    /// <returns>The reflected interpolation result.</returns>
    private static T InvokeAvaloniaLerpCore<T>(Type[] parameterTypes, params object[] arguments)
    {
        var method = typeof(RxAnimationsExtensions).GetMethod(
            "Lerp",
            BindingFlags.Static | BindingFlags.NonPublic,
            parameterTypes)!;
        return (T)method.Invoke(null, arguments)!;
    }

    /// <summary>Invokes the private Avalonia UI scheduler factory.</summary>
    /// <returns>The reflected UI scheduler.</returns>
    private static ISequencer InvokeAvaloniaCreateUiScheduler()
    {
        var method = typeof(RxAnimationsExtensions).GetMethod(
            "CreateUiScheduler",
            BindingFlags.Static | BindingFlags.NonPublic)!;
        return (ISequencer)method.Invoke(null, null)!;
    }
}
