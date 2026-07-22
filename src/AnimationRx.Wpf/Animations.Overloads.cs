// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CP.AnimationRx;

/// <summary>Provides overloads for Animations.</summary>
public static partial class Animations
{
    /// <summary>Defines the default shake count.</summary>
    private const int DefaultShakeCount = 6;

    /// <summary>Defines the default low opacity.</summary>
    private const double DefaultLowOpacity = 0.2;

    /// <summary>Defines the default high opacity.</summary>
    private const double DefaultHighOpacity = 1.0;

    /// <summary>Defines the default pulse count.</summary>
    private const int DefaultPulseCount = 1;

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
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position) =>
        BottomMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        BottomMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        BottomMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(FrameworkElement element, double milliSeconds, double position) =>
        BottomMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease) =>
        BottomMarginMove(element, milliSeconds, position, ease, null);

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
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position) =>
        LeftMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        LeftMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        LeftMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(FrameworkElement element, double milliSeconds, double position) =>
        LeftMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease) =>
        LeftMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position) =>
        RightMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        RightMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        RightMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> angle) =>
        RotateTransform(element, milliSeconds, angle, Ease.None);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(FrameworkElement element, double milliSeconds, double angle) =>
        RotateTransform(element, milliSeconds, angle, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        FrameworkElement element,
        double milliSeconds,
        double angle,
        Ease ease) =>
        RotateTransform(element, milliSeconds, angle, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="this">The this value.</param>
    /// <param name="interval">The interval value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<T> TakeOneEvery<T>(IObservable<T> @this, TimeSpan interval) =>
        TakeOneEvery(@this, interval, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position) =>
        TopMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease) =>
        TopMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease) =>
        TopMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(FrameworkElement element, double milliSeconds, double position) =>
        TopMarginMove(element, milliSeconds, position, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease) =>
        TopMarginMove(element, milliSeconds, position, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<Point> position) =>
        TranslateTransform(element, milliSeconds, position, Ease.None, Ease.None);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<Point> position,
        Ease horizontalEase) =>
        TranslateTransform(element, milliSeconds, position, horizontalEase, Ease.None);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition) =>
        TranslateTransform(element, milliSeconds, horizontalPosition, verticalPosition, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase) =>
        TranslateTransform(
            element,
            milliSeconds,
            horizontalPosition,
            verticalPosition,
            horizontalEase,
            Ease.None,
            null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontalPosition value.</param>
    /// <param name="verticalPosition">The verticalPosition value.</param>
    /// <param name="horizontalEase">The horizontalEase value.</param>
    /// <param name="verticalEase">The verticalEase value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase,
        Ease verticalEase) =>
        TranslateTransform(
            element,
            milliSeconds,
            horizontalPosition,
            verticalPosition,
            horizontalEase,
            verticalEase,
            null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaX,
        double deltaY) =>
        TranslateBy(element, milliSeconds, deltaX, deltaY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX) =>
        TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX,
        Ease easeY) =>
        TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(UIElement element, double milliSeconds, double to) =>
        OpacityTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(UIElement element, double milliSeconds, double to, Ease ease) =>
        OpacityTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(UIElement element, double milliSeconds) =>
        FadeIn(element, milliSeconds, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(UIElement element, double milliSeconds, Ease ease) =>
        FadeIn(element, milliSeconds, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(UIElement element, double milliSeconds) =>
        FadeOut(element, milliSeconds, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(UIElement element, double milliSeconds, Ease ease) =>
        FadeOut(element, milliSeconds, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(FrameworkElement element, double milliSeconds, double to) =>
        WidthTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(FrameworkElement element, double milliSeconds, double to, Ease ease) =>
        WidthTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(FrameworkElement element, double milliSeconds, double to) =>
        HeightTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(FrameworkElement element, double milliSeconds, double to, Ease ease) =>
        HeightTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(FrameworkElement element, double milliSeconds, Thickness to) =>
        MarginTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(FrameworkElement element, double milliSeconds, Thickness to, Ease ease) =>
        MarginTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(Control element, double milliSeconds, Thickness to) =>
        PaddingTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(Control element, double milliSeconds, Thickness to, Ease ease) =>
        PaddingTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(FrameworkElement element, double milliSeconds, double to) =>
        CanvasLeftTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(FrameworkElement element, double milliSeconds, double to, Ease ease) =>
        CanvasLeftTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(FrameworkElement element, double milliSeconds, double to) =>
        CanvasTopTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(FrameworkElement element, double milliSeconds, double to, Ease ease) =>
        CanvasTopTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(FrameworkElement element, double milliSeconds, double to) =>
        CanvasRightTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease) =>
        CanvasRightTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(FrameworkElement element, double milliSeconds, double to) =>
        CanvasBottomTo(element, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease) =>
        CanvasBottomTo(element, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(SolidColorBrush brush, double milliSeconds, Color to) =>
        BrushColorTo(brush, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(SolidColorBrush brush, double milliSeconds, Color to, Ease ease) =>
        BrushColorTo(brush, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(SolidColorBrush brush, double milliSeconds, Color to) =>
        ColorTo(brush, milliSeconds, to, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(SolidColorBrush brush, double milliSeconds, Color to, Ease ease) =>
        ColorTo(brush, milliSeconds, to, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        FrameworkElement element,
        double milliSeconds,
        double scaleX,
        double scaleY) =>
        ScaleTransform(element, milliSeconds, scaleX, scaleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        FrameworkElement element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX) =>
        ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        FrameworkElement element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX,
        Ease easeY) =>
        ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY) =>
        ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX) =>
        ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX,
        Ease easeY) =>
        ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        FrameworkElement element,
        double milliSeconds,
        double angleX,
        double angleY) =>
        SkewTransform(element, milliSeconds, angleX, angleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        FrameworkElement element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX) =>
        SkewTransform(element, milliSeconds, angleX, angleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        FrameworkElement element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX,
        Ease easeY) =>
        SkewTransform(element, milliSeconds, angleX, angleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY) =>
        SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX) =>
        SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX,
        Ease easeY) =>
        SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="delay">The delay value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> DelayBetween(IEnumerable<IObservable<Unit>> animations, TimeSpan delay) =>
        DelayBetween(animations, delay, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animation">The animation value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RepeatAnimation(IObservable<Unit> animation) =>
        RepeatAnimation(animation);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="staggerBy">The staggerBy value.</param>
    /// <returns>The resulting observable.</returns>
    public static IEnumerable<IObservable<Unit>> Stagger(
        IEnumerable<IObservable<Unit>> animations,
        TimeSpan staggerBy) =>
        Stagger(animations, staggerBy, null);

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

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        FrameworkElement element,
        double milliSeconds,
        double toX,
        double toY) =>
        TranslateTo(element, milliSeconds, toX, toY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        FrameworkElement element,
        double milliSeconds,
        double toX,
        double toY,
        Ease easeX) =>
        TranslateTo(element, milliSeconds, toX, toY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        FrameworkElement element,
        double milliSeconds,
        double toX,
        double toY,
        Ease easeX,
        Ease easeY) =>
        TranslateTo(element, milliSeconds, toX, toY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        FrameworkElement element,
        double milliSeconds,
        double toScaleX,
        double toScaleY) =>
        ScaleTo(element, milliSeconds, toScaleX, toScaleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        FrameworkElement element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
        Ease easeX) =>
        ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        FrameworkElement element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
        Ease easeX,
        Ease easeY) =>
        ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(FrameworkElement element, double milliSeconds, double toAngle) =>
        RotateTo(element, milliSeconds, toAngle, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngle,
        Ease ease) =>
        RotateTo(element, milliSeconds, toAngle, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(FrameworkElement element, double milliSeconds, double deltaAngle) =>
        RotateBy(element, milliSeconds, deltaAngle, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngle,
        Ease ease) =>
        RotateBy(element, milliSeconds, deltaAngle, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngleX,
        double toAngleY) =>
        SkewTo(element, milliSeconds, toAngleX, toAngleY, Ease.None, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
        Ease easeX) =>
        SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
        Ease easeX,
        Ease easeY) =>
        SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, easeY, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(FrameworkElement element, double milliSeconds, double amplitude) =>
        ShakeTranslate(element, milliSeconds, amplitude, DefaultShakeCount, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        FrameworkElement element,
        double milliSeconds,
        double amplitude,
        int shakes) =>
        ShakeTranslate(element, milliSeconds, amplitude, shakes, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        FrameworkElement element,
        double milliSeconds,
        double amplitude,
        int shakes,
        Ease ease) =>
        ShakeTranslate(element, milliSeconds, amplitude, shakes, ease, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(UIElement element, double milliSecondsPerHalf) =>
        PulseOpacity(
            element,
            milliSecondsPerHalf,
            DefaultLowOpacity,
            DefaultHighOpacity,
            DefaultPulseCount,
            Ease.None,
            null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(UIElement element, double milliSecondsPerHalf, double low) =>
        PulseOpacity(element, milliSecondsPerHalf, low, DefaultHighOpacity, DefaultPulseCount, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        UIElement element,
        double milliSecondsPerHalf,
        double low,
        double high) =>
        PulseOpacity(element, milliSecondsPerHalf, low, high, DefaultPulseCount, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        UIElement element,
        double milliSecondsPerHalf,
        double low,
        double high,
        int pulses) =>
        PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, Ease.None, null);

    /// <summary>Calls the overload with default arguments.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        UIElement element,
        double milliSecondsPerHalf,
        double low,
        double high,
        int pulses,
        Ease ease) =>
        PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, ease, null);
}
