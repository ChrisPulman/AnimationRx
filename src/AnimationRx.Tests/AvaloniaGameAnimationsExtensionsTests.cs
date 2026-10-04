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
using RxGame = AvaloniaReactiveRx::CP.AnimationRx.Reactive.GameAnimationsExtensions;
#else
using ReactiveUI.Primitives;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxGame = AvaloniaRx::CP.AnimationRx.GameAnimationsExtensions;
#endif
using TUnit.Assertions;
using TUnit.Core;

namespace AnimationRx.Tests;

/// <summary>Tests game animation trajectory and sprite operators.</summary>
public sealed class AvaloniaGameAnimationsExtensionsTests
{
    /// <summary>Defines midpoint progress.</summary>
    private const double Half = 0.5;

    /// <summary>Defines the curve endpoint.</summary>
    private const double End = 4;

    /// <summary>Defines the quadratic control height.</summary>
    private const double Height = 2;

    /// <summary>Defines sprite count.</summary>
    private const int Frames = 4;

    /// <summary>Defines the midpoint sprite divisor and subscription count.</summary>
    private const int Two = 2;

    /// <summary>Defines floating-point assertion tolerance.</summary>
    private const double Tolerance = 0.000001;

    /// <summary>Verifies independent trajectory formulas.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task TrajectoriesCalculateMidpoints()
    {
        (double X, double Y) point = default;
        using var quadratic = RxGame.QuadraticBezier(Observable.Return(Half), (0, 0), (Height, Height), (End, 0)).Subscribe(value => point = value);
        await Assert.That(point).IsEqualTo((Height, 1.0));
        using var cubic = RxGame.CubicBezier(Observable.Return(Half), (0, 0), (0, End), (End, End), (End, 0)).Subscribe(value => point = value);
        await Assert.That(point).IsEqualTo((Height, End - 1));
        using var orbit = RxGame.Orbit(Observable.Return(Half), (1, 1), (Height, End), 0, Math.PI).Subscribe(value => point = value);
        await Assert.That(point.X).IsEqualTo(1.0).Within(Tolerance);
        await Assert.That(point.Y).IsEqualTo(End + 1);
        using var ballistic = RxGame.Ballistic(Observable.Return(Height), (1, 1), (Height, End), (0, -Height)).Subscribe(value => point = value);
        await Assert.That(point).IsEqualTo((End + 1, End + 1));
    }

    /// <summary>Verifies sprite boundaries, overshoot and repeated index filtering.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task SpriteFramesClampAndDeduplicate()
    {
        var indices = new List<int>();
        var values = new[] { double.NaN, -1.0, 0, Half, Half, 1, Height };
        using var subscription = RxGame.SpriteFrames(values.ToObservable(), Frames).Subscribe(indices.Add);
        await Assert.That(indices.ToArray()).IsEquivalentTo(new[] { 0, Frames / Two, Frames - 1 });
    }

    /// <summary>Verifies argument guards across all operators.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task OperatorsRejectInvalidArguments()
    {
        await Assert.That(() => RxGame.QuadraticBezier(null!, default, default, default)).Throws<ArgumentNullException>();
        await Assert.That(() => RxGame.CubicBezier(null!, default, default, default, default)).Throws<ArgumentNullException>();
        await Assert.That(() => RxGame.Orbit(null!, default, default, 0, 1)).Throws<ArgumentNullException>();
        await Assert.That(() => RxGame.Ballistic(null!, default, default, default)).Throws<ArgumentNullException>();
        await Assert.That(() => RxGame.SpriteFrames(null!, 1)).Throws<ArgumentNullException>();
        await Assert.That(() => RxGame.SpriteFrames(Observable.Return(0.0), 0)).Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>Verifies subscriptions remain lazy and errors propagate.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task OperatorsPreserveColdSubscriptionsAndErrors()
    {
        var subscriptions = 0;
        var source = Observable.Defer(() =>
        {
            subscriptions++;
            return Observable.Return(1.0);
        });
        var path = RxGame.QuadraticBezier(source, default, default, (End, End));
        await Assert.That(subscriptions).IsEqualTo(0);
        using var first = path.Subscribe(_ => { });
        using var second = path.Subscribe(_ => { });
        await Assert.That(subscriptions).IsEqualTo(Two);
        var expected = new InvalidOperationException("trajectory source failed");
        Exception? error = null;
        using var failed = RxGame.Ballistic(Observable.Throw<double>(expected), default, default, default).Subscribe(_ => { }, value => error = value);
        await Assert.That(error).IsSameReferenceAs(expected);
    }
}
