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
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxAnimationsExtensions = AvaloniaReactiveRx::CP.AnimationRx.Reactive.AnimationsExtensions;
#else
using ReactiveUI.Primitives;
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
#endif
using TUnit.Assertions;
using TUnit.Core;

namespace AnimationRx.Tests;

/// <summary>Tests render callback lifetime and frame sequencing.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Defines the frame count before cancellation.</summary>
    private const int RenderedFrameCount = 2;

    /// <summary>Verifies lazy registration and disposal with a pending native callback.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task RenderCallbacksStopRenewingAfterDisposal()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var callbacks = new Queue<Action<TimeSpan>>();
        Action<Action<TimeSpan>> request = callbacks.Enqueue;
        var frames = new List<long>();
        var source = RxAnimationsExtensions.RenderFrames(request);
        await Assert.That(callbacks.Count).IsEqualTo(0);
        var subscription = source.Subscribe(frames.Add);
        callbacks.Dequeue()(TimeSpan.Zero);
        callbacks.Dequeue()(TimeSpan.Zero);
        await Assert.That(frames.ToArray()).IsEquivalentTo(new long[] { 0, 1 });
        subscription.Dispose();
        callbacks.Dequeue()(TimeSpan.Zero);
        await Assert.That(callbacks.Count).IsEqualTo(0);
        await Assert.That(frames.Count).IsEqualTo(RenderedFrameCount);

        using var second = RxAnimations.RenderFrames(request).Subscribe(frames.Add);
        callbacks.Dequeue()(TimeSpan.Zero);
        await Assert.That(frames[^1]).IsEqualTo(0L);
    }

    /// <summary>Verifies disposal inside an observer prevents requesting another frame.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task RenderCallbacksAllowDisposalInsideObserver()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var callbacks = new Queue<Action<TimeSpan>>();
        IDisposable? subscription = null;
        subscription = RxAnimations.RenderFrames(callbacks.Enqueue).Subscribe(_ => subscription!.Dispose());
        callbacks.Dequeue()(TimeSpan.Zero);
        await Assert.That(callbacks.Count).IsEqualTo(0);
    }

    /// <summary>Verifies registration failures are emitted as reactive errors.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task RenderCallbacksPropagateRegistrationErrors()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var expected = new InvalidOperationException("render callback registration failed");
        Exception? actual = null;
        using var subscription = RxAnimations.RenderFrames(_ => throw expected).Subscribe(_ => { }, error => actual = error);
        await Assert.That(actual).IsSameReferenceAs(expected);

        var callbacks = new Queue<Action<TimeSpan>>();
        var registrations = 0;
        actual = null;
        using var renewed = RxAnimations.RenderFrames(callback =>
        {
            if (registrations != 0)
            {
                throw expected;
            }

            registrations++;
            callbacks.Enqueue(callback);
        }).Subscribe(_ => { }, error => actual = error);
        callbacks.Dequeue()(TimeSpan.Zero);
        await Assert.That(actual).IsSameReferenceAs(expected);
        await Assert.That(callbacks.Count).IsEqualTo(0);
        await Assert.That(() => RxAnimations.RenderFrames(null!)).Throws<ArgumentNullException>();
    }
}
