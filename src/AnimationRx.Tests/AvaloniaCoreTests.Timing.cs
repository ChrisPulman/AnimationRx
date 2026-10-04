// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias AvaloniaReactiveRx;
#else
extern alias AvaloniaRx;
#endif

#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
using Observable = ReactiveUI.Primitives.Reactive.Signals.Signal;
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using Unit = System.Reactive.Unit;
#else
using ReactiveUI.Primitives;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using Unit = ReactiveUI.Primitives.RxVoid;
#endif
using TUnit.Assertions;
using TUnit.Core;

namespace AnimationRx.Tests;

/// <summary>Tests default scheduling of timing and composition helpers.</summary>
public sealed partial class AvaloniaCoreTests
{
    /// <summary>Defines a positive animation frame rate.</summary>
    private const double PositiveFrameRate = 60;

    /// <summary>Defines the upper bound for default scheduler tests.</summary>
    private const int DefaultSchedulerTimeout = 10_000;

    /// <summary>Defines the sequence seed, stagger tick, and animation emission count.</summary>
    private const int StaggeredSequenceEmissionCount = 3;

    /// <summary>Verifies frame, delay, and stagger helpers run with their default scheduler.</summary>
    /// <param name="cancellationToken">Cancels the pending timing subscriptions.</param>
    /// <returns>The asynchronous test task.</returns>
    [Test]
    [Timeout(DefaultSchedulerTimeout)]
    public async Task Timing_DefaultSchedulers_EmitExpectedValues(CancellationToken cancellationToken)
    {
        var frames = await RxAnimations.AnimateFrame(PositiveFrameRate, scheduler: null).Take(1).ToArray().ToTask(cancellationToken);
        await Assert.That(frames).IsEquivalentTo(new long[] { 0 });
        var durations = await RxAnimations.DurationPercentage(DurationMilliseconds, scheduler: null).ToArray().ToTask(cancellationToken);
        await Assert.That(durations[0].Percent).IsEqualTo(0.0);
        await Assert.That(durations[^1].Percent).IsEqualTo(1.0);
        var values = await RxAnimations.TakeOneEvery(Observable.Return(1), TimeSpan.Zero, scheduler: null).ToArray().ToTask(cancellationToken);
        await Assert.That(values).IsEquivalentTo(new[] { 1 });
        var staggered = RxAnimations.Stagger([Observable.Return(Unit.Default)], TimeSpan.Zero, scheduler: null).ToArray();
        var completions = await RxAnimations.Sequence(staggered).ToArray().ToTask(cancellationToken);
        await Assert.That(completions.Length).IsEqualTo(StaggeredSequenceEmissionCount);
    }
}
