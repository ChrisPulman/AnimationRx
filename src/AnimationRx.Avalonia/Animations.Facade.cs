// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides static facades for layout, transform, and observable composition animations.</summary>
public static partial class Animations
{
    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(Control element, IObservable<double> milliSeconds, IObservable<double> position) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(Control element, double milliSeconds, double position) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease) =>
        AnimationsExtensions.BottomMarginMove(element, milliSeconds, position, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source duration observable.</param>
    /// <param name="distance">The distance value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> Distance(IObservable<Duration> @this, double distance) =>
        AnimationsExtensions.Distance(@this, distance);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source duration observable.</param>
    /// <param name="distance">The distance value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> Distance(IObservable<Duration> @this, IObservable<double> distance) =>
        AnimationsExtensions.Distance(@this, distance);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(Control element, IObservable<double> milliSeconds, IObservable<double> position) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(Control element, double milliSeconds, double position) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease) =>
        AnimationsExtensions.LeftMarginMove(element, milliSeconds, position, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.RightMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.RightMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(Control element, IObservable<double> milliSeconds, IObservable<double> position) =>
        AnimationsExtensions.RightMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        AnimationsExtensions.RightMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        AnimationsExtensions.RightMarginMove(element, milliSeconds, position, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        Visual element,
        IObservable<double> milliSeconds,
        IObservable<double> angle,
        Ease ease) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        Visual element,
        IObservable<double> milliSeconds,
        IObservable<double> angle,
        IObservable<Ease> ease) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        Visual element,
        double milliSeconds,
        double angle,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(Visual element, IObservable<double> milliSeconds, IObservable<double> angle) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(Visual element, double milliSeconds, double angle) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        Visual element,
        double milliSeconds,
        double angle,
        Ease ease) =>
        AnimationsExtensions.RotateTransform(element, milliSeconds, angle, ease);

    /// <summary>
    /// Takes one value of T every interval.
    /// CAUTION: Do not use on streams producing values at a higher rate then the interval. Use
    /// Sample before this to filter out higher speed streams.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="this">The source.</param>
    /// <param name="interval">The interval to produce values at.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>
    /// A Value.
    /// </returns>
    public static IObservable<T> TakeOneEvery<T>(IObservable<T> @this, TimeSpan interval, IScheduler? scheduler) =>
        AnimationsExtensions.TakeOneEvery(@this, interval, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="this">The this value.</param>
    /// <param name="interval">The interval value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<T> TakeOneEvery<T>(IObservable<T> @this, TimeSpan interval) =>
        AnimationsExtensions.TakeOneEvery(@this, interval);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source observable.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ToDuration(IObservable<double> @this) =>
        AnimationsExtensions.ToDuration(@this);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(Control element, IObservable<double> milliSeconds, IObservable<double> position) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(Control element, double milliSeconds, double position) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        Control element,
        double milliSeconds,
        double position,
        Ease ease) =>
        AnimationsExtensions.TopMarginMove(element, milliSeconds, position, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        Visual element,
        double milliSeconds,
        double toX,
        double toY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.TranslateTo(element, milliSeconds, toX, toY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        Visual element,
        double milliSeconds,
        double toX,
        double toY) =>
        AnimationsExtensions.TranslateTo(element, milliSeconds, toX, toY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        Visual element,
        double milliSeconds,
        double toX,
        double toY,
        Ease easeX) =>
        AnimationsExtensions.TranslateTo(element, milliSeconds, toX, toY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        Visual element,
        double milliSeconds,
        double toX,
        double toY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.TranslateTo(element, milliSeconds, toX, toY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        Visual element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        Visual element,
        double milliSeconds,
        double toScaleX,
        double toScaleY) =>
        AnimationsExtensions.ScaleTo(element, milliSeconds, toScaleX, toScaleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        Visual element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
        Ease easeX) =>
        AnimationsExtensions.ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        Visual element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(
        Visual element,
        double milliSeconds,
        double toAngle,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.RotateTo(element, milliSeconds, toAngle, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(Visual element, double milliSeconds, double toAngle) =>
        AnimationsExtensions.RotateTo(element, milliSeconds, toAngle);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(
        Visual element,
        double milliSeconds,
        double toAngle,
        Ease ease) =>
        AnimationsExtensions.RotateTo(element, milliSeconds, toAngle, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(
        Visual element,
        double milliSeconds,
        double deltaAngle,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.RotateBy(element, milliSeconds, deltaAngle, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(Visual element, double milliSeconds, double deltaAngle) =>
        AnimationsExtensions.RotateBy(element, milliSeconds, deltaAngle);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(
        Visual element,
        double milliSeconds,
        double deltaAngle,
        Ease ease) =>
        AnimationsExtensions.RotateBy(element, milliSeconds, deltaAngle, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        Visual element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        Visual element,
        double milliSeconds,
        double toAngleX,
        double toAngleY) =>
        AnimationsExtensions.SkewTo(element, milliSeconds, toAngleX, toAngleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        Visual element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
        Ease easeX) =>
        AnimationsExtensions.SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        Visual element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        Visual element,
        double milliSeconds,
        double amplitude,
        int shakes,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.ShakeTranslate(element, milliSeconds, amplitude, shakes, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(Visual element, double milliSeconds, double amplitude) =>
        AnimationsExtensions.ShakeTranslate(element, milliSeconds, amplitude);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        Visual element,
        double milliSeconds,
        double amplitude,
        int shakes) =>
        AnimationsExtensions.ShakeTranslate(element, milliSeconds, amplitude, shakes);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        Visual element,
        double milliSeconds,
        double amplitude,
        int shakes,
        Ease ease) =>
        AnimationsExtensions.ShakeTranslate(element, milliSeconds, amplitude, shakes, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        Visual element,
        double milliSecondsPerHalf,
        double low,
        double high,
        int pulses,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(Visual element, double milliSecondsPerHalf) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(Visual element, double milliSecondsPerHalf, double low) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf, low);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        Visual element,
        double milliSecondsPerHalf,
        double low,
        double high) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf, low, high);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        Visual element,
        double milliSecondsPerHalf,
        double low,
        double high,
        int pulses) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf, low, high, pulses);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        Visual element,
        double milliSecondsPerHalf,
        double low,
        double high,
        int pulses,
        Ease ease) =>
        AnimationsExtensions.PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="horizontalEase">The horizontal easing value.</param>
    /// <param name="verticalEase">The vertical easing value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        IObservable<double> milliSeconds,
        IObservable<Point> position,
        Ease horizontalEase,
        Ease verticalEase) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, position, horizontalEase, verticalEase);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontal position.</param>
    /// <param name="verticalPosition">The vertical position.</param>
    /// <param name="horizontalEase">The horizontal easing value.</param>
    /// <param name="verticalEase">The vertical easing value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase,
        Ease verticalEase,
        IScheduler? scheduler) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, horizontalPosition, verticalPosition, horizontalEase, verticalEase, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(Visual element, IObservable<double> milliSeconds, IObservable<Point> position) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, position);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        IObservable<double> milliSeconds,
        IObservable<Point> position,
        Ease horizontalEase) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, position, horizontalEase);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, horizontalPosition, verticalPosition);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, horizontalPosition, verticalPosition, horizontalEase);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <param name="verticalEase">The verticalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        Visual element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase,
        Ease verticalEase) =>
        AnimationsExtensions.TranslateTransform(element, milliSeconds, horizontalPosition, verticalPosition, horizontalEase, verticalEase);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        Visual element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        Visual element,
        double milliSeconds,
        double deltaX,
        double deltaY) =>
        AnimationsExtensions.TranslateBy(element, milliSeconds, deltaX, deltaY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        Visual element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX) =>
        AnimationsExtensions.TranslateBy(element, milliSeconds, deltaX, deltaY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        Visual element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(
        Visual element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.OpacityTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(Visual element, double milliSeconds, double to) =>
        AnimationsExtensions.OpacityTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(
        Visual element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.OpacityTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(
        Visual element,
        double milliSeconds,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.FadeIn(element, milliSeconds, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(Visual element, double milliSeconds) =>
        AnimationsExtensions.FadeIn(element, milliSeconds);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(Visual element, double milliSeconds, Ease ease) =>
        AnimationsExtensions.FadeIn(element, milliSeconds, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(
        Visual element,
        double milliSeconds,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.FadeOut(element, milliSeconds, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(Visual element, double milliSeconds) =>
        AnimationsExtensions.FadeOut(element, milliSeconds);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(Visual element, double milliSeconds, Ease ease) =>
        AnimationsExtensions.FadeOut(element, milliSeconds, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.WidthTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.WidthTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.WidthTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.HeightTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.HeightTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.HeightTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(
        Control element,
        double milliSeconds,
        Thickness to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.MarginTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(Control element, double milliSeconds, Thickness to) =>
        AnimationsExtensions.MarginTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(
        Control element,
        double milliSeconds,
        Thickness to,
        Ease ease) =>
        AnimationsExtensions.MarginTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(
        TemplatedControl element,
        double milliSeconds,
        Thickness to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.PaddingTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(TemplatedControl element, double milliSeconds, Thickness to) =>
        AnimationsExtensions.PaddingTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(
        TemplatedControl element,
        double milliSeconds,
        Thickness to,
        Ease ease) =>
        AnimationsExtensions.PaddingTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.CanvasLeftTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.CanvasLeftTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.CanvasLeftTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.CanvasTopTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.CanvasTopTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.CanvasTopTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.CanvasRightTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.CanvasRightTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.CanvasRightTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.CanvasBottomTo(element, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(Control element, double milliSeconds, double to) =>
        AnimationsExtensions.CanvasBottomTo(element, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(
        Control element,
        double milliSeconds,
        double to,
        Ease ease) =>
        AnimationsExtensions.CanvasBottomTo(element, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.BrushColorTo(brush, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(SolidColorBrush brush, double milliSeconds, Color to) =>
        AnimationsExtensions.BrushColorTo(brush, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease) =>
        AnimationsExtensions.BrushColorTo(brush, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease,
        IScheduler? scheduler) =>
        AnimationsExtensions.ColorTo(brush, milliSeconds, to, ease, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(SolidColorBrush brush, double milliSeconds, Color to) =>
        AnimationsExtensions.ColorTo(brush, milliSeconds, to);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease) =>
        AnimationsExtensions.ColorTo(brush, milliSeconds, to, ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        Visual element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        Visual element,
        double milliSeconds,
        double scaleX,
        double scaleY) =>
        AnimationsExtensions.ScaleTransform(element, milliSeconds, scaleX, scaleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        Visual element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX) =>
        AnimationsExtensions.ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        Visual element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        Visual element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        Visual element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY) =>
        AnimationsExtensions.ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        Visual element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX) =>
        AnimationsExtensions.ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        Visual element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        Visual element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.SkewTransform(element, milliSeconds, angleX, angleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        Visual element,
        double milliSeconds,
        double angleX,
        double angleY) =>
        AnimationsExtensions.SkewTransform(element, milliSeconds, angleX, angleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        Visual element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX) =>
        AnimationsExtensions.SkewTransform(element, milliSeconds, angleX, angleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        Visual element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.SkewTransform(element, milliSeconds, angleX, angleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        Visual element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        AnimationsExtensions.SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, easeY, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        Visual element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY) =>
        AnimationsExtensions.SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        Visual element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX) =>
        AnimationsExtensions.SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        Visual element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX,
        Ease easeY) =>
        AnimationsExtensions.SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, easeY);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> Sequence(IEnumerable<IObservable<Unit>> animations) =>
        AnimationsExtensions.Sequence(animations);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> Parallel(IEnumerable<IObservable<Unit>> animations) =>
        AnimationsExtensions.Parallel(animations);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="delay">The delay value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> DelayBetween(IEnumerable<IObservable<Unit>> animations, TimeSpan delay, IScheduler? scheduler) =>
        AnimationsExtensions.DelayBetween(animations, delay, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="delay">The delay value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> DelayBetween(IEnumerable<IObservable<Unit>> animations, TimeSpan delay) =>
        AnimationsExtensions.DelayBetween(animations, delay);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animation">The animation value.</param>
    /// <param name="count">The count value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RepeatAnimation(IObservable<Unit> animation, int count) =>
        AnimationsExtensions.RepeatAnimation(animation, count);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animation">The animation value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RepeatAnimation(IObservable<Unit> animation) =>
        AnimationsExtensions.RepeatAnimation(animation);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="staggerBy">The staggerBy value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IEnumerable<IObservable<Unit>> Stagger(IEnumerable<IObservable<Unit>> animations, TimeSpan staggerBy, IScheduler? scheduler) =>
        AnimationsExtensions.Stagger(animations, staggerBy, scheduler);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="staggerBy">The staggerBy value.</param>
    /// <returns>The resulting observable.</returns>
    public static IEnumerable<IObservable<Unit>> Stagger(IEnumerable<IObservable<Unit>> animations, TimeSpan staggerBy) =>
        AnimationsExtensions.Stagger(animations, staggerBy);
}
