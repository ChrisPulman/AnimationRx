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
#else
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
#endif
using TUnit.Assertions;
using TUnit.Core;
#if REACTIVE_TESTS
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using TestScheduler = System.Reactive.Concurrency.HistoricalScheduler;
#else
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using TestScheduler = ReactiveUI.Primitives.Concurrency.VirtualClock;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies equivalent observable utility facades.</summary>
public sealed partial class AvaloniaCoreTests
{
    /// <summary>Verifies generic delayed sampling facades preserve values and source ordering.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task TakeOneEvery_StaticFacades_PreserveOrder()
    {
        var scheduler = new TestScheduler();
        var values = new List<int>();
        using var subscription = RxAnimations.TakeOneEvery(
            IntegerSequenceInput.ToObservable(),
            TimeSpan.Zero,
            scheduler).Subscribe(values.Add);
        scheduler.AdvanceBy(TimeSpan.FromMilliseconds(1));
        await Assert.That(string.Join(",", values)).IsEqualTo(string.Join(",", IntegerSequenceInput));
        var defaultValues = await RxAnimations.TakeOneEvery(
            IntegerSequenceInput.ToObservable(),
            TimeSpan.Zero).ToArray().ToTask();
        await Assert.That(string.Join(",", defaultValues)).IsEqualTo(string.Join(",", IntegerSequenceInput));
    }
}
