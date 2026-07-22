// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>Provides overloads for Animations.</summary>
public static partial class Animations
{
    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="framesPerSecond">The framesPerSecond value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond) =>
        AnimateFrame(framesPerSecond, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="period">The period value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period) =>
        AnimateFrame(period, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds) =>
        DurationPercentage(milliSeconds, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds) =>
        DurationPercentage(milliSeconds, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to) =>
        AnimateValue(milliSeconds, from, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(
        double milliSeconds,
        double from,
        double to,
        IScheduler? scheduler) =>
        AnimateValue(milliSeconds, from, to, Ease.None, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease) =>
        AnimateValue(milliSeconds, from, to, ease, null);
}
