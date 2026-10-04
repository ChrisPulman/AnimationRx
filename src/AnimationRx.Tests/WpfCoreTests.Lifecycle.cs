// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias WpfReactiveRx;
#else
extern alias WpfRx;
#endif

#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
#else
using ReactiveUI.Primitives;
#endif
using TUnit.Assertions;
using TUnit.Core;
#if REACTIVE_TESTS
using RxAnimations = WpfReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxEase = WpfReactiveRx::CP.AnimationRx.Reactive.Ease;
using TestScheduler = System.Reactive.Concurrency.HistoricalScheduler;
#else
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using TestScheduler = ReactiveUI.Primitives.Concurrency.VirtualClock;
#endif

namespace AnimationRx.Tests;

/// <summary>Contains WPF animation lifecycle tests.</summary>
public sealed partial class WpfCoreTests
{
    /// <summary>Defines the animation frame interval.</summary>
    private const double FrameIntervalMilliseconds = 16.0;

    /// <summary>Defines when the duration changes between frames.</summary>
    private const double DurationChangeMilliseconds = 20.0;

    /// <summary>Defines the advance from the previous to the replacement animation's endpoint.</summary>
    private const double FinalFrameAdvanceMilliseconds = 4.0;

    /// <summary>Defines an advance well beyond the disposed animation's endpoint.</summary>
    private const double PostDisposalAdvanceMilliseconds = 100.0;

    /// <summary>Defines the halfway percentage.</summary>
    private const double HalfPercentage = 0.5;

    /// <summary>Defines cancellation during the third repetition.</summary>
    private const double RepeatCancellationMilliseconds = 25.0;

    /// <summary>Defines the last intermediate percentage of a twenty-millisecond animation.</summary>
    private const double LastIntermediatePercentage = 0.8;

    /// <summary>Defines the linear animation midpoint.</summary>
    private const double LinearMidpoint = 15.0;

    /// <summary>Verifies eased reverse animations emit calculated midpoints and exact endpoints.</summary>
    /// <param name="ease">The easing curve name.</param>
    /// <param name="midpoint">The expected value halfway through the duration.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    [Arguments(nameof(RxEase.QuadIn), 17.5)]
    [Arguments(nameof(RxEase.QuadOut), 12.5)]
    [Arguments(nameof(RxEase.CubicIn), 18.75)]
    public async Task AnimateValueAppliesEasingToDescendingValues(string ease, double midpoint)
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();
        var completed = false;
        using var subscription = RxAnimations.AnimateValue(
            DurationMilliseconds,
            AnimateTo,
            AnimateFrom,
            Enum.Parse<RxEase>(ease),
            scheduler).Subscribe(values.Add, () => completed = true);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, [AnimateTo, midpoint, AnimateFrom]);
        await Assert.That(completed).IsTrue();
    }

    /// <summary>Verifies durations ending between frames emit one exact final value and complete.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DurationPercentageClampsTheLastFrameAndCompletes()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();
        var completed = false;
        using var subscription = RxAnimations.DurationPercentage(DurationChangeMilliseconds, scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add, () => completed = true);

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(FrameIntervalMilliseconds));
        await AssertSequencesAreEqualAsync(values, [0.0, LastIntermediatePercentage]);
        await Assert.That(completed).IsFalse();

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(FrameIntervalMilliseconds));
        await AssertSequencesAreEqualAsync(values, [0.0, LastIntermediatePercentage, 1.0]);
        await Assert.That(completed).IsTrue();

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(PostDisposalAdvanceMilliseconds));
        await AssertSequencesAreEqualAsync(values, [0.0, LastIntermediatePercentage, 1.0]);
    }

    /// <summary>Verifies disposal stops animation ticks without emitting a final value or completion.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AnimateValueDisposalStopsFurtherTicks()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();
        var completed = false;
        using var subscription = RxAnimations.AnimateValue(
            DurationMilliseconds,
            AnimateFrom,
            AnimateTo,
            scheduler: scheduler).Subscribe(values.Add, () => completed = true);

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(FrameIntervalMilliseconds));
        await AssertSequencesAreEqualAsync(values, [AnimateFrom, LinearMidpoint]);

        subscription.Dispose();
        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(PostDisposalAdvanceMilliseconds));

        await AssertSequencesAreEqualAsync(values, [AnimateFrom, LinearMidpoint]);
        await Assert.That(completed).IsFalse();
    }
}
