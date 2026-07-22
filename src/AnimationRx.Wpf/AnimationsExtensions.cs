// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CP.AnimationRx;

/// <summary>Provides extension methods for WPF animations.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides WPF control animation extension methods.</summary>
    /// <param name="element">The control to animate.</param>
    extension(Control element)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PaddingTo(double milliSeconds, Thickness to, Ease ease, IScheduler? scheduler) =>
            Animations.PaddingTo(element, milliSeconds, to, ease, scheduler);
    }

    /// <summary>Provides WPF framework element animation extension methods.</summary>
    /// <param name="element">The element to animate.</param>
    extension(FrameworkElement element)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.BottomMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            IObservable<Ease> ease) =>
            Animations.BottomMarginMove(element, milliSeconds, position, ease);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(
            double milliSeconds,
            double position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.BottomMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasBottomTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.CanvasBottomTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasLeftTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.CanvasLeftTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasRightTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.CanvasRightTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> CanvasTopTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.CanvasTopTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> HeightTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.HeightTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            IObservable<Ease> ease,
            IScheduler? scheduler) =>
            Animations.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(
            double milliSeconds,
            double position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.LeftMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> MarginTo(double milliSeconds, Thickness to, Ease ease, IScheduler? scheduler) =>
            Animations.MarginTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RightMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.RightMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RightMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            IObservable<Ease> ease,
            IScheduler? scheduler) =>
            Animations.RightMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngle">The deltaAngle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateBy(double milliSeconds, double deltaAngle, Ease ease, IScheduler? scheduler) =>
            Animations.RotateBy(element, milliSeconds, deltaAngle, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngle">The toAngle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTo(double milliSeconds, double toAngle, Ease ease, IScheduler? scheduler) =>
            Animations.RotateTo(element, milliSeconds, toAngle, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(
            IObservable<double> milliSeconds,
            IObservable<double> angle,
            Ease ease) =>
            Animations.RotateTransform(element, milliSeconds, angle, ease);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(
            IObservable<double> milliSeconds,
            IObservable<double> angle,
            IObservable<Ease> ease) =>
            Animations.RotateTransform(element, milliSeconds, angle, ease);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(double milliSeconds, double angle, Ease ease, IScheduler? scheduler) =>
            Animations.RotateTransform(element, milliSeconds, angle, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaScaleX">The deltaScaleX value.</param>
        /// <param name="deltaScaleY">The deltaScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleBy(
            double milliSeconds,
            double deltaScaleX,
            double deltaScaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.ScaleBy(element, milliSeconds, deltaScaleX, deltaScaleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toScaleX">The toScaleX value.</param>
        /// <param name="toScaleY">The toScaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTo(
            double milliSeconds,
            double toScaleX,
            double toScaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.ScaleTo(element, milliSeconds, toScaleX, toScaleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="scaleX">The scaleX value.</param>
        /// <param name="scaleY">The scaleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ScaleTransform(
            double milliSeconds,
            double scaleX,
            double scaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.ScaleTransform(element, milliSeconds, scaleX, scaleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="amplitude">The amplitude value.</param>
        /// <param name="shakes">The shakes value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ShakeTranslate(
            double milliSeconds,
            double amplitude,
            int shakes,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.ShakeTranslate(element, milliSeconds, amplitude, shakes, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaAngleX">The deltaAngleX value.</param>
        /// <param name="deltaAngleY">The deltaAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewBy(
            double milliSeconds,
            double deltaAngleX,
            double deltaAngleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.SkewBy(element, milliSeconds, deltaAngleX, deltaAngleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toAngleX">The toAngleX value.</param>
        /// <param name="toAngleY">The toAngleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTo(
            double milliSeconds,
            double toAngleX,
            double toAngleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.SkewTo(element, milliSeconds, toAngleX, toAngleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angleX">The angleX value.</param>
        /// <param name="angleY">The angleY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> SkewTransform(
            double milliSeconds,
            double angleX,
            double angleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.SkewTransform(element, milliSeconds, angleX, angleY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.TopMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            IObservable<Ease> ease,
            IScheduler? scheduler) =>
            Animations.TopMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(
            double milliSeconds,
            double position,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.TopMarginMove(element, milliSeconds, position, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="deltaX">The deltaX value.</param>
        /// <param name="deltaY">The deltaY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateBy(
            double milliSeconds,
            double deltaX,
            double deltaY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.TranslateBy(element, milliSeconds, deltaX, deltaY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="toX">The toX value.</param>
        /// <param name="toY">The toY value.</param>
        /// <param name="easeX">The easeX value.</param>
        /// <param name="easeY">The easeY value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTo(
            double milliSeconds,
            double toX,
            double toY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Animations.TranslateTo(element, milliSeconds, toX, toY, easeX, easeY, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="horizontalEase">The horizontal easing value.</param>
        /// <param name="verticalEase">The vertical easing value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(
            IObservable<double> milliSeconds,
            IObservable<Point> position,
            Ease horizontalEase,
            Ease verticalEase) =>
            Animations.TranslateTransform(element, milliSeconds, position, horizontalEase, verticalEase);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="horizontalPosition">The horizontal position.</param>
        /// <param name="verticalPosition">The vertical position.</param>
        /// <param name="horizontalEase">The horizontal easing value.</param>
        /// <param name="verticalEase">The vertical easing value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(
            double milliSeconds,
            double horizontalPosition,
            double verticalPosition,
            Ease horizontalEase,
            Ease verticalEase,
            IScheduler? scheduler) =>
            Animations.TranslateTransform(
                element,
                milliSeconds,
                horizontalPosition,
                verticalPosition,
                horizontalEase,
                verticalEase,
                scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> WidthTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.WidthTo(element, milliSeconds, to, ease, scheduler);
    }

    /// <summary>Provides animation composition extension methods.</summary>
    /// <param name="animations">The animations to compose.</param>
    extension(IEnumerable<IObservable<Unit>> animations)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="delay">The delay value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> DelayBetween(TimeSpan delay, IScheduler? scheduler) =>
            Animations.DelayBetween(animations, delay, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> Parallel() =>
            Animations.Parallel(animations);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> Sequence() =>
            Animations.Sequence(animations);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="staggerBy">The staggerBy value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IEnumerable<IObservable<Unit>> Stagger(TimeSpan staggerBy, IScheduler? scheduler) =>
            Animations.Stagger(animations, staggerBy, scheduler);
    }

    /// <summary>Provides duration stream extension methods.</summary>
    /// <param name="source">The duration stream.</param>
    extension(IObservable<Duration> source)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="distance">The distance value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<double> Distance(double distance) =>
            Animations.Distance(source, distance);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="distance">The distance value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<double> Distance(IObservable<double> distance) =>
            Animations.Distance(source, distance);
    }

    /// <summary>Provides generic observable extension methods.</summary>
    /// <typeparam name="T">The observable value type.</typeparam>
    /// <param name="source">The source stream.</param>
    extension<T>(IObservable<T> source)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="interval">The interval value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<T> TakeOneEvery(TimeSpan interval, IScheduler? scheduler) =>
            Animations.TakeOneEvery(source, interval, scheduler);
    }

    /// <summary>Provides unit animation stream extension methods.</summary>
    /// <param name="animation">The animation stream.</param>
    extension(IObservable<Unit> animation)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="count">The count value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RepeatAnimation(int count) =>
            Animations.RepeatAnimation(animation, count);
    }

    /// <summary>Provides double stream extension methods.</summary>
    /// <param name="source">The double stream.</param>
    extension(IObservable<double> source)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="velocity">The velocity value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<double> PixelsPerSecond(double velocity) =>
            Animations.PixelsPerSecond(source, velocity);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <returns>The resulting observable.</returns>
        public IObservable<Duration> ToDuration() =>
            Animations.ToDuration(source);
    }

    /// <summary>Provides brush animation extension methods.</summary>
    /// <param name="brush">The brush to animate.</param>
    extension(SolidColorBrush brush)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BrushColorTo(double milliSeconds, Color to, Ease ease, IScheduler? scheduler) =>
            Animations.BrushColorTo(brush, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> ColorTo(double milliSeconds, Color to, Ease ease, IScheduler? scheduler) =>
            Animations.ColorTo(brush, milliSeconds, to, ease, scheduler);
    }

    /// <summary>Provides WPF element animation extension methods.</summary>
    /// <param name="element">The element to animate.</param>
    extension(UIElement element)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeIn(double milliSeconds, Ease ease, IScheduler? scheduler) =>
            Animations.FadeIn(element, milliSeconds, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> FadeOut(double milliSeconds, Ease ease, IScheduler? scheduler) =>
            Animations.FadeOut(element, milliSeconds, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="to">The to value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> OpacityTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Animations.OpacityTo(element, milliSeconds, to, ease, scheduler);

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="high">The high value.</param>
        /// <param name="pulses">The pulses value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> PulseOpacity(
            double milliSecondsPerHalf,
            double low,
            double high,
            int pulses,
            Ease ease,
            IScheduler? scheduler) =>
            Animations.PulseOpacity(element, milliSecondsPerHalf, low, high, pulses, ease, scheduler);
    }
}
