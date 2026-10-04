// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Windows;
using CP.AnimationRx;
using ReactiveUI.Primitives;

namespace CP.Animation.TestApp;

/// <summary>Provides fluent game animation examples that compile with the sample application.</summary>
public static class GameAnimationExamples
{
    /// <summary>Defines milliseconds per second for the ballistic time conversion.</summary>
    private const double MillisecondsPerSecond = 1000;

    /// <summary>Creates a curved jump without starting it until subscribed.</summary>
    /// <param name="sprite">The visual to translate.</param>
    /// <param name="milliseconds">The animation duration.</param>
    /// <param name="start">The initial translation.</param>
    /// <param name="control">The jump control point.</param>
    /// <param name="end">The final translation.</param>
    /// <returns>A cancellable jump animation.</returns>
    public static IObservable<RxVoid> Jump(
        FrameworkElement sprite,
        double milliseconds,
        (double X, double Y) start,
        (double X, double Y) control,
        (double X, double Y) end) =>
        Animations.DurationPercentage(milliseconds)
            .EaseAnimation(Ease.SineInOut)
            .Select(duration => duration.Percent)
            .QuadraticBezier(start, control, end)
            .ApplyTranslation(sprite);

    /// <summary>Creates a cubic movement path from a game progress stream.</summary>
    /// <param name="sprite">The visual to translate.</param>
    /// <param name="progress">The normalized game progress stream.</param>
    /// <param name="start">The initial translation.</param>
    /// <param name="control1">The first control point.</param>
    /// <param name="control2">The second control point.</param>
    /// <param name="end">The final translation.</param>
    /// <returns>A cancellable path animation.</returns>
    public static IObservable<RxVoid> FollowCurve(
        FrameworkElement sprite,
        IObservable<double> progress,
        (double X, double Y) start,
        (double X, double Y) control1,
        (double X, double Y) control2,
        (double X, double Y) end) =>
        progress.CubicBezier(start, control1, control2, end).ApplyTranslation(sprite);

    /// <summary>Creates elliptical movement from a game progress stream.</summary>
    /// <param name="sprite">The visual to translate.</param>
    /// <param name="progress">The normalized game progress stream.</param>
    /// <param name="center">The orbit center.</param>
    /// <param name="radii">The horizontal and vertical radii.</param>
    /// <param name="startAngle">The starting angle in radians.</param>
    /// <param name="sweepAngle">The total angle in radians.</param>
    /// <returns>A cancellable orbit animation.</returns>
    public static IObservable<RxVoid> FollowOrbit(
        FrameworkElement sprite,
        IObservable<double> progress,
        (double X, double Y) center,
        (double X, double Y) radii,
        double startAngle,
        double sweepAngle) =>
        progress.Orbit(center, radii, startAngle, sweepAngle).ApplyTranslation(sprite);

    /// <summary>Creates projectile motion driven by elapsed milliseconds.</summary>
    /// <param name="sprite">The visual to translate.</param>
    /// <param name="milliseconds">The elapsed game time stream.</param>
    /// <param name="origin">The launch position.</param>
    /// <param name="velocity">The launch velocity in units per second.</param>
    /// <param name="acceleration">The acceleration in units per second squared.</param>
    /// <returns>A cancellable projectile animation.</returns>
    public static IObservable<RxVoid> Launch(
        FrameworkElement sprite,
        IObservable<double> milliseconds,
        (double X, double Y) origin,
        (double X, double Y) velocity,
        (double X, double Y) acceleration) =>
        milliseconds.Select(value => value / MillisecondsPerSecond).Ballistic(origin, velocity, acceleration).ApplyTranslation(sprite);

    /// <summary>Creates sprite indices from a game progress stream.</summary>
    /// <param name="progress">The normalized game progress stream.</param>
    /// <param name="frameCount">The number of sprite frames.</param>
    /// <returns>The changing sprite indices.</returns>
    public static IObservable<int> SpriteAnimation(
        IObservable<double> progress,
        int frameCount) =>
        progress.SpriteFrames(frameCount);

    /// <summary>Adapts a one-shot native rendering callback registrar.</summary>
    /// <param name="requestFrame">Registers the next rendering callback.</param>
    /// <returns>The cancellable rendering frame stream.</returns>
    public static IObservable<long> RenderLoop(
        Action<Action<TimeSpan>> requestFrame) =>
        requestFrame.RenderFrames();
}
