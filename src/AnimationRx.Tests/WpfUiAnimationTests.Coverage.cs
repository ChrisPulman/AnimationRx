// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias WpfRx;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using ReactiveUI.Primitives.Disposables;
using TUnit.Assertions;
using TUnit.Core;
using TUnit.Core.Executors;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxAnimationsExtensions = WpfRx::CP.AnimationRx.AnimationsExtensions;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains WPF coverage-focused animation tests.</summary>
public sealed partial class WpfUiAnimationTests
{
    /// <summary>Defines a progress value below the interpolation minimum.</summary>
    private const double BelowMinimumProgress = -1.0;

    /// <summary>Defines a progress value above the interpolation maximum.</summary>
    private const double AboveMaximumProgress = 2.0;

    /// <summary>Defines a progress value inside the interpolation range.</summary>
    private const double MidProgress = 0.5;

    /// <summary>Defines the expected midpoint left value.</summary>
    private const double ExpectedMidpointLeft = 3.0;

    /// <summary>Verifies WPF transform helper branches are exercised.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task TransformHelperBranchesAreCovered()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));

        var matchingTransformButton = new Button
        {
            RenderTransform = new TranslateTransform()
        };
        await RunAnimationAsync(scheduler => RxAnimations.TranslateTransform(
            matchingTransformButton,
            InstantDuration,
            TargetTranslateX,
            TargetTranslateY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(((TransformGroup)matchingTransformButton.RenderTransform).Children
            .OfType<TranslateTransform>()
            .Any()).IsTrue();

        var nullTransformButton = new Button
        {
            RenderTransform = null!
        };
        var addedTransform = InvokeWpfGetOrAddTransform(nullTransformButton, static () => new TranslateTransform());

        await Assert.That(addedTransform).IsNotNull();
        await Assert.That(((TransformGroup)nullTransformButton.RenderTransform).Children
            .OfType<TranslateTransform>()
            .Any()).IsTrue();
    }

    /// <summary>Verifies WPF private math and scheduler helper branches are exercised.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task PrivateHelperBranchesAreCovered()
    {
        var lowThickness = InvokeWpfLerp(TargetMargin, TargetPadding, BelowMinimumProgress);
        var midThickness = InvokeWpfLerp(TargetMargin, TargetPadding, MidProgress);
        var highColor = InvokeWpfLerp(Colors.Transparent, TargetColor, AboveMaximumProgress);
        var freshThreadScheduler = InvokeWpfCreateUiSchedulerOnFreshStaThread();
        var scheduler = InvokeWpfCreateUiScheduler();
        var applicationScheduler = InvokeWpfCreateUiSchedulerWithApplication();

        await Assert.That(lowThickness).IsEqualTo(TargetMargin);
        await Assert.That(midThickness.Left).IsEqualTo(ExpectedMidpointLeft);
        await Assert.That(highColor).IsEqualTo(TargetColor);
        await Assert.That(freshThreadScheduler).IsNotNull();
        await Assert.That(scheduler).IsNotNull();
        await Assert.That(applicationScheduler).IsNotNull();
    }

    /// <summary>Verifies WPF wrapper overloads are exercised directly.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [STAThreadExecutor]
    public async Task WrapperOverloadsAreCovered()
    {
        using var schedulerOverride = OverrideUiScheduler(typeof(RxAnimations));
        var source = Observable.Return(1);
        var never = Observable.Create<Unit>(_ => EmptyDisposable.Instance);

        await Assert.That(RxAnimations.TakeOneEvery(source, TimeSpan.Zero)).IsNotNull();
        await Assert.That(RxAnimationsExtensions.TakeOneEvery(source, TimeSpan.Zero, Sequencer.Immediate))
            .IsNotNull();
        await Assert.That(RxAnimationsExtensions.TakeOneEvery(source, TimeSpan.Zero)).IsNotNull();
        await Assert.That(RxAnimations.RepeatAnimation(never)).IsNotNull();
        await Assert.That(RxAnimationsExtensions.RepeatAnimation(never)).IsNotNull();
        await Assert.That(RxAnimations.Stagger([], TimeSpan.Zero, Sequencer.Immediate).ToArray()).IsEmpty();

        using var renderFrames = RxAnimations.RenderFrames().Subscribe();
    }

    /// <summary>Invokes the private WPF thickness interpolation helper.</summary>
    /// <param name="start">The starting thickness.</param>
    /// <param name="end">The ending thickness.</param>
    /// <param name="progress">The interpolation progress.</param>
    /// <returns>The interpolated thickness.</returns>
    private static Thickness InvokeWpfLerp(Thickness start, Thickness end, double progress) =>
        InvokeWpfLerpCore<Thickness>([typeof(Thickness), typeof(Thickness), typeof(double)], start, end, progress);

    /// <summary>Invokes the private WPF color interpolation helper.</summary>
    /// <param name="start">The starting color.</param>
    /// <param name="end">The ending color.</param>
    /// <param name="progress">The interpolation progress.</param>
    /// <returns>The interpolated color.</returns>
    private static Color InvokeWpfLerp(Color start, Color end, double progress) =>
        InvokeWpfLerpCore<Color>([typeof(Color), typeof(Color), typeof(double)], start, end, progress);

    /// <summary>Invokes the private WPF interpolation helper.</summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="parameterTypes">The interpolation method parameter types.</param>
    /// <param name="arguments">The interpolation method arguments.</param>
    /// <returns>The reflected interpolation result.</returns>
    private static T InvokeWpfLerpCore<T>(Type[] parameterTypes, params object[] arguments)
    {
        var method = typeof(RxAnimations).GetMethod(
            "Lerp",
            BindingFlags.Static | BindingFlags.NonPublic,
            parameterTypes)!;
        return (T)method.Invoke(null, arguments)!;
    }

    /// <summary>Invokes the private WPF UI scheduler factory.</summary>
    /// <returns>The reflected UI scheduler.</returns>
    private static ISequencer InvokeWpfCreateUiScheduler()
    {
        var method = typeof(RxAnimations).GetMethod(
            "CreateUiScheduler",
            BindingFlags.Static | BindingFlags.NonPublic)!;
        return (ISequencer)method.Invoke(null, null)!;
    }

    /// <summary>Invokes the private WPF UI scheduler factory after ensuring an application exists.</summary>
    /// <returns>The reflected UI scheduler.</returns>
    private static ISequencer InvokeWpfCreateUiSchedulerWithApplication()
    {
        if (Application.Current is null)
        {
            _ = new Application();
        }

        return InvokeWpfCreateUiScheduler();
    }

    /// <summary>Invokes the private WPF UI scheduler factory on a fresh STA thread.</summary>
    /// <returns>The reflected UI scheduler.</returns>
    private static ISequencer InvokeWpfCreateUiSchedulerOnFreshStaThread()
    {
        ISequencer? scheduler = null;
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                scheduler = InvokeWpfCreateUiScheduler();
            }
            catch (Exception exception)
            {
                error = exception;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (error is not null)
        {
            throw error;
        }

        return scheduler!;
    }

    /// <summary>Invokes the private WPF transform helper.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The target element.</param>
    /// <param name="factory">The transform factory.</param>
    /// <returns>The reflected transform result.</returns>
    private static T InvokeWpfGetOrAddTransform<T>(FrameworkElement element, Func<T> factory)
        where T : Transform
    {
        var method = typeof(RxAnimations)
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(candidate => candidate.Name == "GetOrAddTransform" && candidate.IsGenericMethodDefinition)
            .MakeGenericMethod(typeof(T));
        return (T)method.Invoke(null, [element, factory])!;
    }
}
