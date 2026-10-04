// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias AvaloniaReactiveRx;
#else
extern alias AvaloniaRx;
#endif

using System.Reflection;
using Avalonia;
using Avalonia.Media;
#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
#else
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
#endif
using TUnit.Assertions;
using TUnit.Core;
#if REACTIVE_TESTS
using ISequencer = System.Reactive.Concurrency.IScheduler;
using Observable = ReactiveUI.Primitives.Reactive.Signals.Signal;
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxAnimationsExtensions = AvaloniaReactiveRx::CP.AnimationRx.Reactive.AnimationsExtensions;
using RxEase = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Ease;
using Unit = System.Reactive.Unit;
#else
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
using RxEase = AvaloniaRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies equivalent reactive layout and transform behavior.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Defines the parity fixture ParityOtherAngle value.</summary>
    private const double ParityOtherAngle = 90;

    /// <summary>Defines the parity fixture ParityY value.</summary>
    private const double ParityY = 6;

    /// <summary>Defines the parity fixture ParityTarget value.</summary>
    private const double ParityTarget = 5;

    /// <summary>Defines the parity fixture ParityBottom value.</summary>
    private const double ParityBottom = 4;

    /// <summary>Defines the parity fixture ParityRight value.</summary>
    private const double ParityRight = 3;

    /// <summary>Defines the parity fixture ParityTop value.</summary>
    private const double ParityTop = 2;

    /// <summary>Defines the parity fixture ParityAngle value.</summary>
    private const double ParityAngle = 45;

    /// <summary>Verifies every margin facade and fluent overload preserves the other margin sides.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task MarginMove_AllOverloads_ChangeOnlyRequestedSide()
    {
        using var schedulerOverride = OverrideUiScheduler();
        foreach (var owner in new[] { typeof(RxAnimations), typeof(RxAnimationsExtensions) })
        {
            foreach (var method in owner.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method => method.Name.EndsWith("MarginMove", StringComparison.Ordinal)))
            {
                var button = CreateButton();
                button.Margin = new(1, ParityTop, ParityRight, ParityBottom);
                var arguments = method.GetParameters().Select(parameter =>
                {
                    if (parameter.Name == "element")
                    {
                        return (object)button;
                    }

                    if (parameter.ParameterType == typeof(ISequencer))
                    {
                        return Sequencer.Immediate;
                    }

                    if (parameter.Name == "ease")
                    {
                        return parameter.ParameterType == typeof(RxEase)
                            ? (object)RxEase.None
                            : Observable.Return(RxEase.None);
                    }

                    var value = parameter.Name == "position" ? ParityTarget : 0.0;
                    return parameter.ParameterType == typeof(double) ? (object)value : Observable.Return(value);
                }).ToArray();
                var animation = (IObservable<Unit>)method.Invoke(null, arguments)!;
                await RunAnimationAsync(_ => animation);
                var expected = method.Name switch
                {
                    "LeftMarginMove" => new Thickness(ParityTarget, ParityTop, ParityRight, ParityBottom),
                    "TopMarginMove" => new Thickness(1, ParityTarget, ParityRight, ParityBottom),
                    "RightMarginMove" => new Thickness(1, ParityTop, ParityTarget, ParityBottom),
                    _ => new Thickness(1, ParityTop, ParityRight, ParityTarget)
                };
                await Assert.That(button.Margin).IsEqualTo(expected);
            }
        }
    }

    /// <summary>Verifies observable translation and rotation targets update their existing transforms.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task Transform_ObservableTargets_ApplyAbsolutePositions()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var button = CreateButton();
        await RunAnimationAsync(_ => RxAnimations.TranslateTransform(
            button,
            Observable.Return(0.0),
            Observable.Return(new Point(ParityTarget, ParityY)),
            RxEase.None,
            RxEase.None));
        var translate = GetTransform<TranslateTransform>(button);
        await Assert.That(translate.X).IsEqualTo(ParityTarget);
        await Assert.That(translate.Y).IsEqualTo(ParityY);
        await RunAnimationAsync(_ => RxAnimations.RotateTransform(
            button,
            Observable.Return(0.0),
            Observable.Return(ParityAngle),
            Observable.Return(RxEase.None)));
        await Assert.That(GetTransform<RotateTransform>(button).Angle).IsEqualTo(ParityAngle);
        await RunAnimationAsync(_ => RxAnimations.RotateTransform(
            button,
            Observable.Return(0.0),
            Observable.Return(ParityOtherAngle),
            RxEase.None));
        await Assert.That(GetTransform<RotateTransform>(button).Angle).IsEqualTo(ParityOtherAngle);
    }
}
