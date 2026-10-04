// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides static timing helpers alongside fluent animation extensions.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Animates the frame using an interval based on frames-per-second.</summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable that ticks every frame.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler) => Animations.AnimateFrame(framesPerSecond, scheduler);

    /// <summary>Convenience overload to drive frames by a fixed period.</summary>
    /// <param name="period">The frame period.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable sequence of frame indices emitted on the given period.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler) => Animations.AnimateFrame(period, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="framesPerSecond">The framesPerSecond value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond) => Animations.AnimateFrame(framesPerSecond);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="period">The period value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period) => Animations.AnimateFrame(period);

    /// <summary>Produces a UI-scheduled frame stream using the default animation cadence.</summary>
    /// <returns>An observable sequence of frame indices.</returns>
    public static IObservable<long> RenderFrames() => Animations.RenderFrames();

    /// <summary>Milliseconds elapsed.</summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits elapsed milliseconds since subscription.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) => Animations.MilliSecondsElapsed(scheduler);

    /// <summary>
    /// Produces a percentage of the duration from a changing time span observable.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler) => Animations.DurationPercentage(milliSeconds, scheduler);

    /// <summary>
    /// Produces a percentage of the duration for a fixed time span in milliseconds.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler) => Animations.DurationPercentage(milliSeconds, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds) => Animations.DurationPercentage(milliSeconds);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds) => Animations.DurationPercentage(milliSeconds);

    /// <summary>Helper to create a value animation from a start value to an end value using easing.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits animated values.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease, IScheduler? scheduler) => Animations.AnimateValue(milliSeconds, from, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to) => Animations.AnimateValue(milliSeconds, from, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease) => Animations.AnimateValue(milliSeconds, from, to, ease);
}
