extern alias AvaloniaRx;

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.Reactive.Testing;
using TUnit.Assertions;
using TUnit.Core;
using AvaloniaRx::CP.AnimationRx;

namespace AnimationRx.Tests;

public sealed class AvaloniaCoreTests
{
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

    private static readonly double[] FirstThreeDistanceExpected = [0.0, 2.0, 2.5];
    private static readonly double[] LatestDistanceInput = [0.0, 0.5, 1.0];
    private static readonly double[] LatestDistanceExpected = [0.0, 6.0, 12.0];
    private static readonly double[] DurationPercentageExpected = [0.0, 0.5, 1.0];
    private static readonly double[] ImmediateDurationExpected = [1.0];
    private static readonly double[] AnimateValueExpected = [10.0, 15.0, 20.0];
    private static readonly int[] IntegerSequenceInput = [1, 2, 3];
    private static readonly int[] IntegerSequenceExpected = [1, 2, 3];
    private static readonly int[] OrderedPairExpected = [1, 2];

    [Test]
    public async Task DurationCreateRoundTripsPercent()
    {
        var duration = Duration.Create(0.42);

        await Assert.That(duration.Percent).IsEqualTo(0.42);
        await Assert.That(duration).IsEqualTo(new Duration(0.42));
    }

    [Test]
    public async Task EaseAnimationRoutesEveryKnownEaseToItsExtension()
    {
        foreach (var ease in Enum.GetValues<Ease>())
        {
            var direct = await ReadPercentsAsync(Apply(ease, Percentages.Select(Duration.Create).ToObservable()));
            var routed = await ReadPercentsAsync(Percentages.Select(Duration.Create).ToObservable().EaseAnimation(ease));

            await AssertSequencesAreEqualAsync(routed, direct);
        }
    }

    [Test]
    public async Task EaseAnimationReturnsSourceForUnknownEase()
    {
        var routed = await ReadPercentsAsync(Percentages.Select(Duration.Create).ToObservable().EaseAnimation((Ease)999));

        await AssertSequencesAreEqualAsync(routed, Percentages);
    }

    [Test]
    public async Task DistanceScalesDurationPercentages()
    {
        var values = await Percentages.Take(3).Select(Duration.Create).ToObservable().Distance(10).ToArray().ToTask();

        await AssertSequencesAreEqualAsync(values, FirstThreeDistanceExpected);
    }

    [Test]
    public async Task DistanceUsesLatestObservableDistance()
    {
        var values = await LatestDistanceInput
            .Select(Duration.Create)
            .ToObservable()
            .Distance(Observable.Return(12.0))
            .ToArray()
            .ToTask();

        await AssertSequencesAreEqualAsync(values, LatestDistanceExpected);
    }

    [Test]
    public async Task DurationPercentageEmitsInitialAndFinalValues()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        Animations.DurationPercentage(32, scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, DurationPercentageExpected);
    }

    [Test]
    public async Task DurationPercentageCompletesImmediatelyForNonPositiveDurations()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        Animations.DurationPercentage(0, scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, ImmediateDurationExpected);
    }

    [Test]
    public async Task ObservableDurationPercentageUsesTheLatestDuration()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        Animations.DurationPercentage(Observable.Return(32.0), scheduler)
            .Select(duration => duration.Percent)
            .Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, DurationPercentageExpected);
    }

    [Test]
    public async Task AnimateFrameNormalizesNonPositiveFrameRates()
    {
        var scheduler = new TestScheduler();
        var values = new List<long>();

        using var subscription = Animations.AnimateFrame(0, scheduler).Subscribe(values.Add);

        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(17).Ticks);

        await Assert.That(values).Count().IsEqualTo(1);
        await Assert.That(values[0]).IsEqualTo(0);
    }

    [Test]
    public async Task AnimateValueInterpolatesFromStartToEnd()
    {
        var scheduler = new TestScheduler();
        var values = new List<double>();

        Animations.AnimateValue(32, 10, 20, scheduler: scheduler).Subscribe(values.Add);

        scheduler.Start();

        await AssertSequencesAreEqualAsync(values, AnimateValueExpected);
    }

    [Test]
    public async Task TakeOneEveryDelaysEachValueInSequence()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();

        IntegerSequenceInput.ToObservable()
            .TakeOneEvery(TimeSpan.FromMilliseconds(10), scheduler)
            .Subscribe(values.Add);

        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(IntegerSequenceExpected);
    }

    [Test]
    public async Task SequenceRunsAnimationsInOrder()
    {
        var values = new List<int>();
        var animations = new[]
        {
            Observable.Return(Unit.Default).Do(_ => values.Add(1)),
            Observable.Return(Unit.Default).Do(_ => values.Add(2))
        };

        await animations.Sequence().ToTask();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    [Test]
    public async Task ParallelCompletesWhenEveryAnimationCompletes()
    {
        var values = await new[]
        {
            Observable.Return(Unit.Default),
            Observable.Return(Unit.Default)
        }.Parallel().ToArray().ToTask();

        await Assert.That(values).Count().IsEqualTo(1);
    }

    [Test]
    public async Task RepeatAnimationUsesTheRequestedCount()
    {
        var values = await Observable.Return(Unit.Default).RepeatAnimation(3).ToArray().ToTask();

        await Assert.That(values).Count().IsEqualTo(3);
    }

    [Test]
    public async Task DelayBetweenAddsTheRequestedDelay()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();
        var animations = new[]
        {
            Observable.Return(Unit.Default).Do(_ => values.Add(1)),
            Observable.Return(Unit.Default).Do(_ => values.Add(2))
        };

        animations.DelayBetween(TimeSpan.FromMilliseconds(10), scheduler).Subscribe();
        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    [Test]
    public async Task StaggerAddsIncrementalDelays()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();

        var staggered = new[]
        {
            Observable.Return(Unit.Default).Do(_ => values.Add(1)),
            Observable.Return(Unit.Default).Do(_ => values.Add(2))
        }.Stagger(TimeSpan.FromMilliseconds(10), scheduler).ToArray();

        staggered.Merge().Subscribe();
        scheduler.Start();

        await Assert.That(values).IsEquivalentTo(OrderedPairExpected);
    }

    [Test]
    public async Task StaggerRejectsNullAnimations()
    {
        IEnumerable<IObservable<Unit>> animations = null!;

        await Assert.That(() => animations.Stagger(TimeSpan.Zero)).Throws<ArgumentNullException>();
    }

    private static IObservable<Duration> Apply(Ease ease, IObservable<Duration> source) =>
        ease switch
        {
            Ease.BackIn => source.BackIn(),
            Ease.BackInOut => source.BackInOut(),
            Ease.BackOut => source.BackOut(),
            Ease.BounceIn => source.BounceIn(),
            Ease.BounceInOut => source.BounceInOut(),
            Ease.BounceOut => source.BounceOut(),
            Ease.CircIn => source.CircIn(),
            Ease.CircInOut => source.CircInOut(),
            Ease.CircOut => source.CircOut(),
            Ease.CubicIn => source.CubicIn(),
            Ease.CubicInOut => source.CubicInOut(),
            Ease.CubicOut => source.CubicOut(),
            Ease.ElasticIn => source.ElasticIn(),
            Ease.ElasticInOut => source.ElasticInOut(),
            Ease.ElasticOut => source.ElasticOut(),
            Ease.ExpoIn => source.ExpoIn(),
            Ease.ExpoInOut => source.ExpoInOut(),
            Ease.ExpoOut => source.ExpoOut(),
            Ease.QuinticIn => source.QuinticIn(),
            Ease.QuinticInOut => source.QuinticInOut(),
            Ease.QuinticOut => source.QuinticOut(),
            Ease.QuadIn => source.QuadIn(),
            Ease.QuadInOut => source.QuadInOut(),
            Ease.QuadOut => source.QuadOut(),
            Ease.QuarticIn => source.QuarticIn(),
            Ease.QuarticInOut => source.QuarticInOut(),
            Ease.QuarticOut => source.QuarticOut(),
            Ease.SineIn => source.SineIn(),
            Ease.SineInOut => source.SineInOut(),
            Ease.SineOut => source.SineOut(),
            _ => source,
        };

    private static async Task<double[]> ReadPercentsAsync(IObservable<Duration> source) =>
        await source.Select(duration => duration.Percent).ToArray().ToTask();

    private static async Task AssertSequencesAreEqualAsync(IReadOnlyList<double> actual, double[] expected)
    {
        await Assert.That(actual).Count().IsEqualTo(expected.Length);

        for (var index = 0; index < expected.Length; index++)
        {
            await Assert.That(actual[index]).IsEqualTo(expected[index]).Within(0.000000000001);
        }
    }
}
