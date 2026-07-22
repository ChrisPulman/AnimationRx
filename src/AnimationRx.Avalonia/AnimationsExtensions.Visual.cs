// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Media;

namespace CP.AnimationRx;

/// <summary>Provides Avalonia visual animation extension helpers.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides animation extension methods for visual elements.</summary>
    /// <param name="element">The visual element to animate.</param>
    extension(Visual element)
    {
        /// <summary>Animates element opacity to a target value.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> OpacityTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => element.Opacity, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Opacity = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Animates element opacity to fully visible.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> FadeIn(double milliSeconds, Ease ease, IScheduler? scheduler) =>
            element.OpacityTo(milliSeconds, 1, ease, scheduler);

        /// <summary>Animates element opacity to fully transparent.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> FadeOut(double milliSeconds, Ease ease, IScheduler? scheduler) =>
            element.OpacityTo(milliSeconds, 0, ease, scheduler);

        /// <summary>Execute TranslateTransform.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="targetX">The target X position.</param>
        /// <param name="targetY">The target Y position.</param>
        /// <param name="horizontalEase">The horizontal easing value.</param>
        /// <param name="verticalEase">The vertical easing value.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> TranslateTransform(
            double milliSeconds,
            double targetX,
            double targetY,
            Ease horizontalEase,
            Ease verticalEase,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new TranslateTransform());
                        var fromX = tr.X;
                        var fromY = tr.Y;

                        var anim = DurationPercentage(milliSeconds, scheduler);
                        var horizontalMove = anim.EaseAnimation(horizontalEase).Distance(targetX - fromX);
                        var verticalMove = anim.EaseAnimation(verticalEase).Distance(targetY - fromY);

                        return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                            .ObserveOn(GetUiScheduler())
                            .Do(p =>
                            {
                                tr.X = fromX + p.X;
                                tr.Y = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Animates TranslateTransform by relative X/Y values.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="deltaX">The relative X movement.</param>
        /// <param name="deltaY">The relative Y movement.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> TranslateBy(
            double milliSeconds,
            double deltaX,
            double deltaY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new TranslateTransform());
                        return element.TranslateTransform(
                            milliSeconds,
                            tr.X + deltaX,
                            tr.Y + deltaY,
                            easeX,
                            easeY,
                            scheduler);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Animates TranslateTransform X/Y to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> TranslateTo(
            double milliSeconds,
            double toX,
            double toY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            element.TranslateTransform(milliSeconds, toX, toY, easeX, easeY, scheduler);

        /// <summary>Rotates the transform.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> RotateTransform(double milliSeconds, double angle, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new RotateTransform());
                        var from = tr.Angle;

                        return AnimateValue(milliSeconds, from, angle, ease, scheduler)
                            .ObserveOn(GetUiScheduler())
                            .Do(v => tr.Angle = v)
                            .Select(_ => Unit.Default);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Animates rotation angle to a target value (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toAngle">To angle.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> RotateTo(double milliSeconds, double toAngle, Ease ease, IScheduler? scheduler) =>
            element.RotateTransform(milliSeconds, toAngle, ease, scheduler);

        /// <summary>Animates rotation by a relative angle.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="deltaAngle">The relative angle.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> RotateBy(double milliSeconds, double deltaAngle, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new RotateTransform());
                        return element.RotateTransform(milliSeconds, tr.Angle + deltaAngle, ease, scheduler);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Scale transform animation.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="scaleX">The scale x.</param>
        /// <param name="scaleY">The scale y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ScaleTransform(
            double milliSeconds,
            double scaleX,
            double scaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new ScaleTransform(1, 1));
                        var fromX = tr.ScaleX;
                        var fromY = tr.ScaleY;

                        var anim = DurationPercentage(milliSeconds, scheduler);
                        var horizontalMove = anim.EaseAnimation(easeX).Distance(scaleX - fromX);
                        var verticalMove = anim.EaseAnimation(easeY).Distance(scaleY - fromY);

                        return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                            .ObserveOn(GetUiScheduler())
                            .Do(p =>
                            {
                                tr.ScaleX = fromX + p.X;
                                tr.ScaleY = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Animates ScaleTransform to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toScaleX">To scale x.</param>
        /// <param name="toScaleY">To scale y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ScaleTo(
            double milliSeconds,
            double toScaleX,
            double toScaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            element.ScaleTransform(milliSeconds, toScaleX, toScaleY, easeX, easeY, scheduler);

        /// <summary>Animates ScaleTransform by relative scale values.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="deltaScaleX">The relative scale x.</param>
        /// <param name="deltaScaleY">The relative scale y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ScaleBy(
            double milliSeconds,
            double deltaScaleX,
            double deltaScaleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new ScaleTransform(1, 1));
                        return element.ScaleTransform(
                            milliSeconds,
                            tr.ScaleX + deltaScaleX,
                            tr.ScaleY + deltaScaleY,
                            easeX,
                            easeY,
                            scheduler);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Skew transform animation.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="angleX">The angle x.</param>
        /// <param name="angleY">The angle y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> SkewTransform(
            double milliSeconds,
            double angleX,
            double angleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new SkewTransform());
                        var fromX = tr.AngleX;
                        var fromY = tr.AngleY;

                        var anim = DurationPercentage(milliSeconds, scheduler);
                        var horizontalMove = anim.EaseAnimation(easeX).Distance(angleX - fromX);
                        var verticalMove = anim.EaseAnimation(easeY).Distance(angleY - fromY);

                        return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                            .ObserveOn(GetUiScheduler())
                            .Do(p =>
                            {
                                tr.AngleX = fromX + p.X;
                                tr.AngleY = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Animates skew angles to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toAngleX">To angle x.</param>
        /// <param name="toAngleY">To angle y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> SkewTo(
            double milliSeconds,
            double toAngleX,
            double toAngleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            element.SkewTransform(milliSeconds, toAngleX, toAngleY, easeX, easeY, scheduler);

        /// <summary>Animates SkewTransform by relative angles.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="deltaAngleX">The relative angle x.</param>
        /// <param name="deltaAngleY">The relative angle y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> SkewBy(
            double milliSeconds,
            double deltaAngleX,
            double deltaAngleY,
            Ease easeX,
            Ease easeY,
            IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new SkewTransform());
                        return element.SkewTransform(
                            milliSeconds,
                            tr.AngleX + deltaAngleX,
                            tr.AngleY + deltaAngleY,
                            easeX,
                            easeY,
                            scheduler);
                    },
                    GetUiScheduler()).SelectMany(x => x));

        /// <summary>Shakes horizontally using TranslateTransform and returns to the original position.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="amplitude">The amplitude.</param>
        /// <param name="shakes">The shakes.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the shaking is done.</returns>
        public IObservable<Unit> ShakeTranslate(
            double milliSeconds,
            double amplitude,
            int shakes,
            Ease ease,
            IScheduler? scheduler)
        {
            if (shakes <= 0)
            {
                shakes = 1;
            }

            return Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(element, () => new TranslateTransform());
                    var fromX = tr.X;
                    var fromY = tr.Y;

                    return AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .Select(p =>
                        {
                            var t = p * Math.PI * FullCircleRadiansMultiplier * shakes;
                            return Math.Sin(t) * amplitude * (1.0 - p);
                        })
                        .ObserveOn(GetUiScheduler())
                        .Do(dx =>
                        {
                            tr.X = fromX + dx;
                            tr.Y = fromY;
                        })
                        .Finally(() =>
                        {
                            tr.X = fromX;
                            tr.Y = fromY;
                        })
                        .Select(_ => Unit.Default);
                },
                GetUiScheduler()).SelectMany(x => x));
        }

        /// <summary>Pulses opacity between two values for a specified number of pulses.</summary>
        /// <param name="milliSecondsPerHalf">The milli seconds per half.</param>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <param name="pulses">The pulses.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the pulsing is done.</returns>
        public IObservable<Unit> PulseOpacity(
            double milliSecondsPerHalf,
            double low,
            double high,
            int pulses,
            Ease ease,
            IScheduler? scheduler)
        {
            if (pulses <= 0)
            {
                pulses = 1;
            }

            var seq = new[]
            {
            element.OpacityTo(milliSecondsPerHalf, low, ease, scheduler),
            element.OpacityTo(milliSecondsPerHalf, high, ease, scheduler),
        }.Sequence();

            return seq.RepeatAnimation(pulses);
        }
    }
}
