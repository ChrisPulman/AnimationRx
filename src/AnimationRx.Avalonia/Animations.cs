// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>Provides static animation facade methods for Avalonia.</summary>
public static partial class Animations
{
    /// <summary>Animates the frame using an interval based on frames-per-second.</summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable that ticks every frame.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler) =>
        AnimationsExtensions.AnimateFrame(framesPerSecond, scheduler);

    /// <summary>Convenience overload to drive frames by a fixed period.</summary>
    /// <param name="period">The frame period.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable sequence of frame indices emitted on the given period.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler) =>
        AnimationsExtensions.AnimateFrame(period, scheduler);

    /// <summary>Produces a UI-scheduled frame stream using the default animation cadence.</summary>
    /// <returns>An observable sequence of frame indices.</returns>
    public static IObservable<long> RenderFrames() =>
        AnimationsExtensions.RenderFrames();

    /// <summary>Milliseconds elapsed.</summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits elapsed milliseconds since subscription.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) =>
        AnimationsExtensions.MilliSecondsElapsed(scheduler);

    /// <summary>Produces a percentage of the duration from a changing time span observable.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler) =>
        AnimationsExtensions.DurationPercentage(milliSeconds, scheduler);

    /// <summary>Produces a percentage of the duration for a fixed time span in milliseconds.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler) =>
        AnimationsExtensions.DurationPercentage(milliSeconds, scheduler);

    /// <summary>Helper to create a value animation from a start value to an end value using easing.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits animated values.</returns>
    public static IObservable<double> AnimateValue(
        double milliSeconds,
        double from,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.AnimateValue(milliSeconds, from, to, ease, scheduler);

    /// <summary>Converts elapsed milliseconds to pixels moved at the supplied velocity.</summary>
    /// <param name="source">The source stream.</param>
    /// <param name="velocity">The velocity in pixels per second.</param>
    /// <returns>An observable that emits pixel distances.</returns>
    public static IObservable<double> PixelsPerSecond(IObservable<double> source, double velocity) =>
        source.PixelsPerSecond(velocity);
}
