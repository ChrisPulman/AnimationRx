// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias WpfRx;

using ReactiveUI.Primitives;
using TUnit.Assertions;
using TUnit.Core;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxDuration = WpfRx::CP.AnimationRx.Duration;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using RxEases = WpfRx::CP.AnimationRx.Eases;
using RxEasesExtensions = WpfRx::CP.AnimationRx.EasesExtensions;
using TestScheduler = ReactiveUI.Primitives.Concurrency.VirtualClock;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains WPF core animation tests.</summary>
public sealed class WpfCoreTests
{
    /// <summary>Defines the round-trip percentage.</summary>
    private const double RoundTripPercent = 0.42;

    /// <summary>Defines an unknown ease value.</summary>
    private const RxEase UnknownEase = (RxEase)999;

    /// <summary>Defines the number of samples used by scale tests.</summary>
    private const int SampleCount = 3;

    /// <summary>Defines the distance scale.</summary>
    private const double DistanceScale = 10.0;

    /// <summary>Defines the latest observable distance.</summary>
    private const double LatestDistance = 12.0;

    /// <summary>Defines the pixel velocity.</summary>
    private const double PixelVelocity = 20.0;

    /// <summary>Defines the test duration in milliseconds.</summary>
    private const double DurationMilliseconds = 32.0;

    /// <summary>Defines the frame advance in milliseconds.</summary>
    private const double FrameAdvanceMilliseconds = 17.0;

    /// <summary>Defines the starting animated value.</summary>
    private const double AnimateFrom = 10.0;

    /// <summary>Defines the ending animated value.</summary>
    private const double AnimateTo = 20.0;

    /// <summary>Defines the step delay in milliseconds.</summary>
    private const double StepDelayMilliseconds = 10.0;

    /// <summary>Defines the expected single emission count.</summary>
    private const int SingleEmissionCount = 1;

    /// <summary>Defines the first integer sequence value.</summary>
    private const int FirstSequenceValue = 1;

    /// <summary>Defines the second integer sequence value.</summary>
    private const int SecondSequenceValue = 2;

    /// <summary>Defines the third integer sequence value.</summary>
    private const int ThirdSequenceValue = 3;

    /// <summary>Defines the repeat count.</summary>
    private const int RepeatCount = 3;

    /// <summary>Defines the assertion tolerance.</summary>
    private const double AssertionTolerance = 0.000000000001;

    /// <summary>Provides percentages.</summary>
    private static readonly double[] Percentages =
    [
        0.0,
        0.2,
        0.25,
        0.5,
        0.75,
        0.8,
        0.95,
        1.0
    ];

    /// <summary>Provides first three distance expected.</summary>
    private static readonly double[] FirstThreeDistanceExpected = [0.0, 2.0, 2.5];

    /// <summary>Provides latest distance input.</summary>
    private static readonly double[] LatestDistanceInput = [0.0, 0.5, 1.0];

    /// <summary>Provides latest distance expected.</summary>
    private static readonly double[] LatestDistanceExpected = [0.0, 6.0, 12.0];

    /// <summary>Provides pixels per second input.</summary>
    private static readonly double[] PixelsPerSecondInput = [0.0, 100.0, 1000.0];

    /// <summary>Provides pixels per second expected.</summary>
    private static readonly double[] PixelsPerSecondExpected = [0.0, 2.0, 20.0];

    /// <summary>Provides duration percentage expected.</summary>
    private static readonly double[] DurationPercentageExpected = [0.0, 0.5, 1.0];

    /// <summary>Provides immediate duration expected.</summary>
    private static readonly double[] ImmediateDurationExpected = [1.0];

    /// <summary>Provides animate value expected.</summary>
    private static readonly double[] AnimateValueExpected = [10.0, 15.0, 20.0];

    /// <summary>Provides integer sequence input.</summary>
    private static readonly int[] IntegerSequenceInput =
    [
        FirstSequenceValue,
        SecondSequenceValue,
        ThirdSequenceValue
    ];

    /// <summary>Provides integer sequence expected.</summary>
    private static readonly int[] IntegerSequenceExpected =
    [
        FirstSequenceValue,
        SecondSequenceValue,
        ThirdSequenceValue
    ];

    /// <summary>Provides ordered pair expected.</summary>
    private static readonly int[] OrderedPairExpected = [FirstSequenceValue, SecondSequenceValue];

    /// <summary>Verifies duration create round trips percent.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DurationCreateRoundTripsPercent()
    {
        var duration = RxDuration.Create(RoundTripPercent);

        await Assert.That(duration.Percent).IsEqualTo(RoundTripPercent);
        await Assert.That(duration).IsEqualTo(new(RoundTripPercent));
    }

    /// <summary>Verifies ease animation routes every known ease to its extension.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task EaseAnimationRoutesEveryKnownEaseToItsExtension()
    {
        foreach (var ease in Enum.GetValues<RxEase>())
        {
            var source = Percentages.Select(RxDuration.Create).ToObservable();
            var direct = await ReadPercentsAsync(Apply(ease, source));
            var routed = await ReadPercentsAsync(RxEasesExtensions.EaseAnimation(source, ease));

            await AssertSequencesAreEqualAsync(routed, direct);
        }
    }

    /// <summary>Verifies ease animation returns source for unknown ease.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task EaseAnimationReturnsSourceForUnknownEase()
    {
        var source = Percentages.Select(RxDuration.Create).ToObservable();
        var routed = await ReadPercentsAsync(RxEasesExtensions.EaseAnimation(source, UnknownEase));

        await AssertSequencesAreEqualAsync(routed, Percentages);
    }

    /// <summary>Verifies eases facade routes every known ease to its extension.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task EasesFacadeRoutesEveryKnownEaseToItsExtension()
    {
        foreach (var ease in Enum.GetValues<RxEase>())
        {
            var source = Percentages.Select(RxDuration.Create).ToObservable();
            var direct = await ReadPercentsAsync(Apply(ease, source));
            var routed = await ReadPercentsAsync(ApplyFacade(ease, source));
            var routedByEase = await ReadPercentsAsync(RxEases.EaseAnimation(source, ease));

            await AssertSequencesAreEqualAsync(routed, direct);
            await AssertSequencesAreEqualAsync(routedByEase, direct);
        }
    }

    /// <summary>Verifies eases facade returns source for unknown ease.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task EasesFacadeReturnsSourceForUnknownEase()
    {
        var source = Percentages.Select(RxDuration.Create).ToObservable();
        var routed = await ReadPercentsAsync(RxEases.EaseAnimation(source, UnknownEase));

        await AssertSequencesAreEqualAsync(routed, Percentages);
    }

    /// <summary>Verifies distance scales duration percentages.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DistanceScalesDurationPercentages()
    {
        var source = Percentages.Take(SampleCount).Select(RxDuration.Create).ToObservable();
        var values = await RxAnimations.Distance(source, DistanceScale).ToArray().ToTask();

        await AssertSequencesAreEqualAsync(values, FirstThreeDistanceExpected);
    }

    /// <summary>Verifies distance uses latest observable distance.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DistanceUsesLatestObservableDistance()
    {
        var scheduler = new TestScheduler();
        var source = LatestDistanceInput.Select(RxDuration.Create).ToObservable(scheduler);
        var valuesTask = RxAnimations.Distance(source, Observable.Return(LatestDistance)).ToArray().ToTask();

        scheduler.Start();

        var values = await valuesTask;

        await AssertSequencesAreEqualAsync(values, LatestDistanceExpected);
    }

    /// <summary>Verifies pixels per second scales elapsed milliseconds.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task PixelsPerSecondScalesElapsedMilliseconds()
    {
        var source = PixelsPerSecondInput.ToObservable();
        var values = await RxAnimations.PixelsPerSecond(source, PixelVelocity).ToArray().ToTask();

        await AssertSequencesAreEqualAsync(values, PixelsPerSecondExpected);
    }

    /// <summary>Verifies duration percentage emits initial and final values.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DurationPercentageEmitsInitialAndFinalValues()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        using var subscription = RxAnimations.DurationPercentage(DurationMilliseconds, scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, DurationPercentageExpected);
    }

    /// <summary>Verifies duration percentage completes immediately for non-positive durations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DurationPercentageCompletesImmediatelyForNonPositiveDurations()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        using var subscription = RxAnimations.DurationPercentage(0, scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, ImmediateDurationExpected);
    }

    /// <summary>Verifies observable duration percentage uses the latest duration.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task ObservableDurationPercentageUsesTheLatestDuration()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        using var subscription = RxAnimations.DurationPercentage(Observable.Return(DurationMilliseconds), scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, DurationPercentageExpected);
    }

    /// <summary>Verifies animate frame normalizes non-positive frame rates.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AnimateFrameNormalizesNonPositiveFrameRates()
    {
        var scheduler = new TestScheduler();
        var values = new List<long>();

        using var subscription = RxAnimations.AnimateFrame(0, scheduler).Subscribe(values.Add);

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(FrameAdvanceMilliseconds));

        await Assert.That(values).Count().IsEqualTo(SingleEmissionCount);
        await Assert.That(values[0]).IsEqualTo(0);
    }

    /// <summary>Verifies animate value interpolates from start to end.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task AnimateValueInterpolatesFromStartToEnd()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        using var subscription = RxAnimations.AnimateValue(
            DurationMilliseconds,
            AnimateFrom,
            AnimateTo,
            scheduler: scheduler).Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, AnimateValueExpected);
    }

    /// <summary>Verifies take one every delays each value in sequence.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task TakeOneEveryDelaysEachValueInSequence()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();

        using var subscription = RxAnimations.TakeOneEvery(
                IntegerSequenceInput.ToObservable(),
                TimeSpan.FromMilliseconds(StepDelayMilliseconds),
                scheduler)
            .Subscribe(values.Add);

        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(IntegerSequenceExpected);
    }

    /// <summary>Verifies sequence runs animations in order.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task SequenceRunsAnimationsInOrder()
    {
        var values = new List<int>();
        var animations = new[]
        {
            Observable.Return(Unit.Default).Do(_ => values.Add(FirstSequenceValue)),
            Observable.Return(Unit.Default).Do(_ => values.Add(SecondSequenceValue))
        };

        await RxAnimations.Sequence(animations).ToTask();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    /// <summary>Verifies parallel completes when every animation completes.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task ParallelCompletesWhenEveryAnimationCompletes()
    {
        var values = await RxAnimations.Parallel([
            Observable.Return(Unit.Default),
            Observable.Return(Unit.Default)
        ]).ToArray().ToTask();

        await Assert.That(values).Count().IsEqualTo(SingleEmissionCount);
    }

    /// <summary>Verifies repeat animation uses the requested count.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task RepeatAnimationUsesTheRequestedCount()
    {
        var values = await RxAnimations.RepeatAnimation(
            Observable.Return(Unit.Default),
            RepeatCount).ToArray().ToTask();

        await Assert.That(values).Count().IsEqualTo(RepeatCount);
    }

    /// <summary>Verifies delay between adds the requested delay.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DelayBetweenAddsTheRequestedDelay()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();
        var animations = new[]
        {
            Observable.Return(Unit.Default).Do(_ => values.Add(FirstSequenceValue)),
            Observable.Return(Unit.Default).Do(_ => values.Add(SecondSequenceValue))
        };

        using var subscription = RxAnimations.DelayBetween(
            animations,
            TimeSpan.FromMilliseconds(StepDelayMilliseconds),
            scheduler).Subscribe();
        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    /// <summary>Verifies stagger adds incremental delays.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task StaggerAddsIncrementalDelays()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();

        var staggered = RxAnimations.Stagger(
            [
                Observable.Return(Unit.Default).Do(_ => values.Add(FirstSequenceValue)),
                Observable.Return(Unit.Default).Do(_ => values.Add(SecondSequenceValue))
            ],
            TimeSpan.FromMilliseconds(StepDelayMilliseconds),
            scheduler).ToArray();

        using var subscription = staggered.Merge().Subscribe();
        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    /// <summary>Verifies sequence rejects null animations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task SequenceRejectsNullAnimations()
    {
        await Assert.That(() => RxAnimations.Sequence(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies parallel rejects null animations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task ParallelRejectsNullAnimations()
    {
        await Assert.That(() => RxAnimations.Parallel(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies stagger rejects null animations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task StaggerRejectsNullAnimations()
    {
        await Assert.That(() => RxAnimations.Stagger(null!, TimeSpan.Zero)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies repeat animation rejects null animation.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task RepeatAnimationRejectsNullAnimation()
    {
        await Assert.That(() => RxAnimations.RepeatAnimation(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Verifies delay between rejects null animations.</summary>
    /// <returns>A task that completes when the test finishes.</returns>
    [Test]
    public async Task DelayBetweenRejectsNullAnimations()
    {
        await Assert.That(() => RxAnimations.DelayBetween(null!, TimeSpan.Zero)).Throws<ArgumentNullException>();
    }

    /// <summary>Runs apply.</summary>
    /// <param name="ease">The ease value.</param>
    /// <param name="source">The source value.</param>
    /// <returns>The computed value.</returns>
    private static IObservable<RxDuration> Apply(RxEase ease, IObservable<RxDuration> source) =>
        ease switch
        {
            RxEase.BackIn => RxEasesExtensions.BackIn(source),
            RxEase.BackInOut => RxEasesExtensions.BackInOut(source),
            RxEase.BackOut => RxEasesExtensions.BackOut(source),
            RxEase.BounceIn => RxEasesExtensions.BounceIn(source),
            RxEase.BounceInOut => RxEasesExtensions.BounceInOut(source),
            RxEase.BounceOut => RxEasesExtensions.BounceOut(source),
            RxEase.CircIn => RxEasesExtensions.CircIn(source),
            RxEase.CircInOut => RxEasesExtensions.CircInOut(source),
            RxEase.CircOut => RxEasesExtensions.CircOut(source),
            RxEase.CubicIn => RxEasesExtensions.CubicIn(source),
            RxEase.CubicInOut => RxEasesExtensions.CubicInOut(source),
            RxEase.CubicOut => RxEasesExtensions.CubicOut(source),
            RxEase.ElasticIn => RxEasesExtensions.ElasticIn(source),
            RxEase.ElasticInOut => RxEasesExtensions.ElasticInOut(source),
            RxEase.ElasticOut => RxEasesExtensions.ElasticOut(source),
            RxEase.ExpoIn => RxEasesExtensions.ExpoIn(source),
            RxEase.ExpoInOut => RxEasesExtensions.ExpoInOut(source),
            RxEase.ExpoOut => RxEasesExtensions.ExpoOut(source),
            RxEase.QuinticIn => RxEasesExtensions.QuinticIn(source),
            RxEase.QuinticInOut => RxEasesExtensions.QuinticInOut(source),
            RxEase.QuinticOut => RxEasesExtensions.QuinticOut(source),
            RxEase.QuadIn => RxEasesExtensions.QuadIn(source),
            RxEase.QuadInOut => RxEasesExtensions.QuadInOut(source),
            RxEase.QuadOut => RxEasesExtensions.QuadOut(source),
            RxEase.QuarticIn => RxEasesExtensions.QuarticIn(source),
            RxEase.QuarticInOut => RxEasesExtensions.QuarticInOut(source),
            RxEase.QuarticOut => RxEasesExtensions.QuarticOut(source),
            RxEase.SineIn => RxEasesExtensions.SineIn(source),
            RxEase.SineInOut => RxEasesExtensions.SineInOut(source),
            RxEase.SineOut => RxEasesExtensions.SineOut(source),
            _ => source,
        };

    /// <summary>Runs apply facade.</summary>
    /// <param name="ease">The ease value.</param>
    /// <param name="source">The source value.</param>
    /// <returns>The computed value.</returns>
    private static IObservable<RxDuration> ApplyFacade(RxEase ease, IObservable<RxDuration> source) =>
        ease switch
        {
            RxEase.BackIn => RxEases.BackIn(source),
            RxEase.BackInOut => RxEases.BackInOut(source),
            RxEase.BackOut => RxEases.BackOut(source),
            RxEase.BounceIn => RxEases.BounceIn(source),
            RxEase.BounceInOut => RxEases.BounceInOut(source),
            RxEase.BounceOut => RxEases.BounceOut(source),
            RxEase.CircIn => RxEases.CircIn(source),
            RxEase.CircInOut => RxEases.CircInOut(source),
            RxEase.CircOut => RxEases.CircOut(source),
            RxEase.CubicIn => RxEases.CubicIn(source),
            RxEase.CubicInOut => RxEases.CubicInOut(source),
            RxEase.CubicOut => RxEases.CubicOut(source),
            RxEase.ElasticIn => RxEases.ElasticIn(source),
            RxEase.ElasticInOut => RxEases.ElasticInOut(source),
            RxEase.ElasticOut => RxEases.ElasticOut(source),
            RxEase.ExpoIn => RxEases.ExpoIn(source),
            RxEase.ExpoInOut => RxEases.ExpoInOut(source),
            RxEase.ExpoOut => RxEases.ExpoOut(source),
            RxEase.QuinticIn => RxEases.QuinticIn(source),
            RxEase.QuinticInOut => RxEases.QuinticInOut(source),
            RxEase.QuinticOut => RxEases.QuinticOut(source),
            RxEase.QuadIn => RxEases.QuadIn(source),
            RxEase.QuadInOut => RxEases.QuadInOut(source),
            RxEase.QuadOut => RxEases.QuadOut(source),
            RxEase.QuarticIn => RxEases.QuarticIn(source),
            RxEase.QuarticInOut => RxEases.QuarticInOut(source),
            RxEase.QuarticOut => RxEases.QuarticOut(source),
            RxEase.SineIn => RxEases.SineIn(source),
            RxEase.SineInOut => RxEases.SineInOut(source),
            RxEase.SineOut => RxEases.SineOut(source),
            _ => source,
        };

    /// <summary>Runs read percents async.</summary>
    /// <param name="source">The source value.</param>
    /// <returns>The computed value.</returns>
    private static Task<double[]> ReadPercentsAsync(IObservable<RxDuration> source) =>
        source.Select(duration => duration.Percent).ToArray().ToTask();

    /// <summary>Runs assert sequences are equal async.</summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="expected">The expected value.</param>
    /// <returns>A task that completes when the assertion is done.</returns>
    private static async Task AssertSequencesAreEqualAsync(IReadOnlyList<double> actual, double[] expected)
    {
        await Assert.That(actual).Count().IsEqualTo(expected.Length);

        for (var index = 0; index < expected.Length; index++)
        {
            await Assert.That(actual[index]).IsEqualTo(expected[index]).Within(AssertionTolerance);
        }
    }
}
