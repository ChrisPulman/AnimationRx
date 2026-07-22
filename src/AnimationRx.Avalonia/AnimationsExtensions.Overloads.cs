// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace CP.AnimationRx;

/// <summary>Provides overloads for AnimationsExtensions.</summary>
public static partial class AnimationsExtensions
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
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease) =>
        AnimateValue(milliSeconds, from, to, ease, null);

    /// <summary>Provides extension overloads.</summary>
    /// <param name="element">The extended element value.</param>
    extension(Control element)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> WidthTo(double milliSeconds, double to) =>
            WidthTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> WidthTo(double milliSeconds, double to, Ease ease) =>
            WidthTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> HeightTo(double milliSeconds, double to) =>
            HeightTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> HeightTo(double milliSeconds, double to, Ease ease) =>
            HeightTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> MarginTo(double milliSeconds, Thickness to) =>
            MarginTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> MarginTo(double milliSeconds, Thickness to, Ease ease) =>
            MarginTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasLeftTo(double milliSeconds, double to) =>
            CanvasLeftTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasLeftTo(double milliSeconds, double to, Ease ease) =>
            CanvasLeftTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasTopTo(double milliSeconds, double to) =>
            CanvasTopTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasTopTo(double milliSeconds, double to, Ease ease) =>
            CanvasTopTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasRightTo(double milliSeconds, double to) =>
            CanvasRightTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasRightTo(double milliSeconds, double to, Ease ease) =>
            CanvasRightTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasBottomTo(double milliSeconds, double to) =>
            CanvasBottomTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasBottomTo(double milliSeconds, double to, Ease ease) =>
            CanvasBottomTo(element, milliSeconds, to, ease, null);
    }

    /// <summary>Provides extension overloads.</summary>
    /// <param name="animations">The extended animations value.</param>
    extension(IEnumerable<IObservable<Unit>> animations)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="delay">The delay value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> DelayBetween(TimeSpan delay) =>
            DelayBetween(animations, delay, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="staggerBy">The staggerBy value.</param>
        /// <returns>The resulting observable.</returns>
        public IEnumerable<IObservable<Unit>> Stagger(TimeSpan staggerBy) =>
            Stagger(animations, staggerBy, null);
    }

    /// <summary>Provides extension overloads.</summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="this">The extended this value.</param>
    extension<T>(IObservable<T> @this)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="interval">The interval value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<T> TakeOneEvery(TimeSpan interval) =>
            TakeOneEvery(@this, interval, null);
    }

    /// <summary>Provides extension overloads.</summary>
    /// <param name="brush">The extended brush value.</param>
    extension(SolidColorBrush brush)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BrushColorTo(double milliSeconds, Color to) =>
            BrushColorTo(brush, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BrushColorTo(double milliSeconds, Color to, Ease ease) =>
            BrushColorTo(brush, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ColorTo(double milliSeconds, Color to) =>
            ColorTo(brush, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ColorTo(double milliSeconds, Color to, Ease ease) =>
            ColorTo(brush, milliSeconds, to, ease, null);
    }

    /// <summary>Provides extension overloads.</summary>
    /// <param name="element">The extended element value.</param>
    extension(TemplatedControl element)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PaddingTo(double milliSeconds, Thickness to) =>
            PaddingTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PaddingTo(double milliSeconds, Thickness to, Ease ease) =>
            PaddingTo(element, milliSeconds, to, ease, null);
    }

    /// <summary>Provides extension overloads.</summary>
    /// <param name="element">The extended element value.</param>
    extension(Visual element)
    {
        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> OpacityTo(double milliSeconds, double to) =>
            OpacityTo(element, milliSeconds, to, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> OpacityTo(double milliSeconds, double to, Ease ease) =>
            OpacityTo(element, milliSeconds, to, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeIn(double milliSeconds) =>
            FadeIn(element, milliSeconds, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeIn(double milliSeconds, Ease ease) =>
            FadeIn(element, milliSeconds, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeOut(double milliSeconds) =>
            FadeOut(element, milliSeconds, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeOut(double milliSeconds, Ease ease) =>
            FadeOut(element, milliSeconds, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="targetX">The targetX value.</param>
        /// <param name="targetY">The targetY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(double milliSeconds, double targetX, double targetY) =>
            TranslateTransform(element, milliSeconds, targetX, targetY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="targetX">The targetX value.</param>
        /// <param name="targetY">The targetY value.</param>
        /// <param name="horizontalEase">The horizontalEase value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(
            double milliSeconds,
            double targetX,
            double targetY,
            Ease horizontalEase) =>
            TranslateTransform(element, milliSeconds, targetX, targetY, horizontalEase, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="targetX">The targetX value.</param>
        /// <param name="targetY">The targetY value.</param>
        /// <param name="horizontalEase">The horizontalEase value.</param>
        /// <param name="verticalEase">The verticalEase value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(
            double milliSeconds,
            double targetX,
            double targetY,
            Ease horizontalEase,
            Ease verticalEase) =>
            TranslateTransform(element, milliSeconds, targetX, targetY, horizontalEase, verticalEase, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaX">The deltaX value.</param>
        /// <param name="deltaY">The deltaY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateBy(double milliSeconds, double deltaX, double deltaY) =>
            TranslateBy(element, milliSeconds, deltaX, deltaY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaX">The deltaX value.</param>
        /// <param name="deltaY">The deltaY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateBy(double milliSeconds, double deltaX, double deltaY, Ease easeX) =>
            TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaX">The deltaX value.</param>
        /// <param name="deltaY">The deltaY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateBy(
            double milliSeconds,
            double deltaX,
            double deltaY,
            Ease easeX,
            Ease easeY) =>
            TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toX">The toX value.</param>
        /// <param name="toY">The toY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTo(double milliSeconds, double toX, double toY) =>
            TranslateTo(element, milliSeconds, toX, toY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toX">The toX value.</param>
        /// <param name="toY">The toY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTo(double milliSeconds, double toX, double toY, Ease easeX) =>
            TranslateTo(element, milliSeconds, toX, toY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toX">The toX value.</param>
        /// <param name="toY">The toY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTo(double milliSeconds, double toX, double toY, Ease easeX, Ease easeY) =>
            TranslateTo(element, milliSeconds, toX, toY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(double milliSeconds, double angle) =>
            RotateTransform(element, milliSeconds, angle, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(double milliSeconds, double angle, Ease ease) =>
            RotateTransform(element, milliSeconds, angle, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngle">The toAngle value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTo(double milliSeconds, double toAngle) =>
            RotateTo(element, milliSeconds, toAngle, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngle">The toAngle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTo(double milliSeconds, double toAngle, Ease ease) =>
            RotateTo(element, milliSeconds, toAngle, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngle">The deltaAngle value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateBy(double milliSeconds, double deltaAngle) =>
            RotateBy(element, milliSeconds, deltaAngle, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngle">The deltaAngle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateBy(double milliSeconds, double deltaAngle, Ease ease) =>
            RotateBy(element, milliSeconds, deltaAngle, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="scaleX">The scaleX value.</param>
        /// <param name="scaleY">The scaleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTransform(double milliSeconds, double scaleX, double scaleY) =>
            ScaleTransform(element, milliSeconds, scaleX, scaleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="scaleX">The scaleX value.</param>
        /// <param name="scaleY">The scaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTransform(double milliSeconds, double scaleX, double scaleY, Ease easeX) =>
            ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="scaleX">The scaleX value.</param>
        /// <param name="scaleY">The scaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTransform(
            double milliSeconds,
            double scaleX,
            double scaleY,
            Ease easeX,
            Ease easeY) =>
            ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toScaleX">The toScaleX value.</param>
        /// <param name="toScaleY">The toScaleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTo(double milliSeconds, double toScaleX, double toScaleY) =>
            ScaleTo(element, milliSeconds, toScaleX, toScaleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toScaleX">The toScaleX value.</param>
        /// <param name="toScaleY">The toScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTo(double milliSeconds, double toScaleX, double toScaleY, Ease easeX) =>
            ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toScaleX">The toScaleX value.</param>
        /// <param name="toScaleY">The toScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTo(
            double milliSeconds,
            double toScaleX,
            double toScaleY,
            Ease easeX,
            Ease easeY) =>
            ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaScaleX">The deltaScaleX value.</param>
        /// <param name="deltaScaleY">The deltaScaleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleBy(double milliSeconds, double deltaScaleX, double deltaScaleY) =>
            ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaScaleX">The deltaScaleX value.</param>
        /// <param name="deltaScaleY">The deltaScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleBy(double milliSeconds, double deltaScaleX, double deltaScaleY, Ease easeX) =>
            ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaScaleX">The deltaScaleX value.</param>
        /// <param name="deltaScaleY">The deltaScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleBy(
            double milliSeconds,
            double deltaScaleX,
            double deltaScaleY,
            Ease easeX,
            Ease easeY) =>
            ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angleX">The angleX value.</param>
        /// <param name="angleY">The angleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTransform(double milliSeconds, double angleX, double angleY) =>
            SkewTransform(element, milliSeconds, angleX, angleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angleX">The angleX value.</param>
        /// <param name="angleY">The angleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTransform(double milliSeconds, double angleX, double angleY, Ease easeX) =>
            SkewTransform(element, milliSeconds, angleX, angleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angleX">The angleX value.</param>
        /// <param name="angleY">The angleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTransform(
            double milliSeconds,
            double angleX,
            double angleY,
            Ease easeX,
            Ease easeY) =>
            SkewTransform(element, milliSeconds, angleX, angleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngleX">The toAngleX value.</param>
        /// <param name="toAngleY">The toAngleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTo(double milliSeconds, double toAngleX, double toAngleY) =>
            SkewTo(element, milliSeconds, toAngleX, toAngleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngleX">The toAngleX value.</param>
        /// <param name="toAngleY">The toAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTo(double milliSeconds, double toAngleX, double toAngleY, Ease easeX) =>
            SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngleX">The toAngleX value.</param>
        /// <param name="toAngleY">The toAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTo(
            double milliSeconds,
            double toAngleX,
            double toAngleY,
            Ease easeX,
            Ease easeY) =>
            SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngleX">The deltaAngleX value.</param>
        /// <param name="deltaAngleY">The deltaAngleY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewBy(double milliSeconds, double deltaAngleX, double deltaAngleY) =>
            SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, Ease.None, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngleX">The deltaAngleX value.</param>
        /// <param name="deltaAngleY">The deltaAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewBy(double milliSeconds, double deltaAngleX, double deltaAngleY, Ease easeX) =>
            SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngleX">The deltaAngleX value.</param>
        /// <param name="deltaAngleY">The deltaAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewBy(
            double milliSeconds,
            double deltaAngleX,
            double deltaAngleY,
            Ease easeX,
            Ease easeY) =>
            SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, easeY, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="amplitude">The amplitude value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ShakeTranslate(double milliSeconds, double amplitude) =>
            ShakeTranslate(element, milliSeconds, amplitude, DefaultShakeCount, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="amplitude">The amplitude value.</param>
        /// <param name="shakes">The shakes value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ShakeTranslate(double milliSeconds, double amplitude, int shakes) =>
            ShakeTranslate(element, milliSeconds, amplitude, shakes, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="amplitude">The amplitude value.</param>
        /// <param name="shakes">The shakes value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ShakeTranslate(double milliSeconds, double amplitude, int shakes, Ease ease) =>
            ShakeTranslate(element, milliSeconds, amplitude, shakes, ease, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(double milliSecondsPerHalf) =>
            PulseOpacity(
                element,
                milliSecondsPerHalf,
                DefaultLowOpacity,
                DefaultHighOpacity,
                DefaultPulseCount,
                Ease.None,
                null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <param name="low">The low value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(double milliSecondsPerHalf, double low) =>
            PulseOpacity(element, milliSecondsPerHalf, low, DefaultHighOpacity, DefaultPulseCount, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(double milliSecondsPerHalf, double low, double high) =>
            PulseOpacity(element, milliSecondsPerHalf, low, high, DefaultPulseCount, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <param name="pulses">The pulses value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(double milliSecondsPerHalf, double low, double high, int pulses) =>
            PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, Ease.None, null);

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <param name="pulses">The pulses value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(
            double milliSecondsPerHalf,
            double low,
            double high,
            int pulses,
            Ease ease) =>
            PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, ease, null);
    }
}
