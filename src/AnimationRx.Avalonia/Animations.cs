// Copyright (c) 2023-2026 Chris Pulman and Contributors. All rights reserved.
// Chris Pulman and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using ReactiveUI;

namespace CP.AnimationRx;

/// <summary>Animations for Avalonia.</summary>
public static class Animations
{
    /// <summary>Animates the frame using an interval based on frames-per-second.</summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable that ticks every frame.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
        {
            var fps = framesPerSecond <= 0 ? 60.0 : framesPerSecond;
            var period = TimeSpan.FromMilliseconds(1000.0 / fps);
            return Observable.Interval(period, scheduler ?? RxSchedulers.TaskpoolScheduler);
        });

    /// <summary>Convenience overload to drive frames by a fixed period.</summary>
    /// <param name="period">The frame period.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable sequence of frame indices emitted on the given period.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler = null) =>
        Observable.Interval(period, scheduler ?? RxSchedulers.TaskpoolScheduler);

    /// <summary>Milliseconds elapsed.</summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits elapsed milliseconds since subscription.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) =>
        Observable.Defer(() =>
        {
            var start = scheduler.Now;
            return Observable.Interval(TimeSpan.FromMilliseconds(16), scheduler)
                .Select(_ => (scheduler.Now - start).TotalMilliseconds)
                .StartWith(0.0);
        });

    /// <summary>
    /// Produces a percentage of the duration from a changing time span observable.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            MilliSecondsElapsed(scheduler ?? RxSchedulers.TaskpoolScheduler)
                .CombineLatest(milliSeconds, (ems, ms) => ms <= 0 ? 1.0 : ems / ms)
                .ToDuration()
                .TakeWhile(t => t.Percent < 1)
                .Concat(Observable.Return(Duration.Create(1))));

    /// <summary>
    /// Produces a percentage of the duration for a fixed time span in milliseconds.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            MilliSecondsElapsed(scheduler ?? RxSchedulers.TaskpoolScheduler)
                .Select(ems => milliSeconds <= 0 ? 1.0 : ems / milliSeconds)
                .ToDuration()
                .TakeWhile(t => t.Percent < 1)
                .Concat(Observable.Return(Duration.Create(1))));

    /// <summary>Provides distance extension methods for duration observables.</summary>
    /// <param name="this">The duration observable.</param>
    extension(IObservable<Duration> @this)
    {
        /// <summary>Converts duration percentage into a delta over a distance.</summary>
        /// <param name="distance">The distance.</param>
        /// <returns>An observable that emits distance deltas.</returns>
        public IObservable<double> Distance(double distance) =>
            Observable.Defer(() => @this.Select(t => t.Percent * distance));

        /// <summary>Converts duration percentage into a delta over a distance.</summary>
        /// <param name="distance">The distance.</param>
        /// <returns>An observable that emits distance deltas.</returns>
        public IObservable<double> Distance(IObservable<double> distance) =>
            Observable.Defer(() => @this.CombineLatest(distance, (du, d) => du.Percent * d));
    }

    /// <summary>Helper to create a value animation from a start value to an end value using easing.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits animated values.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            DurationPercentage(milliSeconds, scheduler)
                .EaseAnimation(ease)
                .Distance(to - from)
                .Select(delta => from + delta));

    /// <summary>Provides throttling extension methods for observable sequences.</summary>
    /// <typeparam name="T">The observable value type.</typeparam>
    /// <param name="this">The source observable.</param>
    extension<T>(IObservable<T> @this)
    {
        /// <summary>Takes one value of T every interval.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that emits one value every interval.</returns>
        public IObservable<T> TakeOneEvery(TimeSpan interval, IScheduler? scheduler = null) =>
            Observable.Defer(() =>
            {
                var sched = scheduler ?? RxSchedulers.TaskpoolScheduler;
                return @this.Select(v => Observable.Return(v).Delay(interval, sched)).Concat();
            });
    }

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
        public IObservable<Unit> OpacityTo(double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => element.Opacity, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(v => element.Opacity = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Execute TranslateTransform.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="targetX">The target X position.</param>
        /// <param name="targetY">The target Y position.</param>
        /// <param name="xease">The xease.</param>
        /// <param name="yease">The yease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> TranslateTransform(double milliSeconds, double targetX, double targetY, Ease xease = Ease.None, Ease yease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new TranslateTransform());
                        var fromX = tr.X;
                        var fromY = tr.Y;

                        var anim = DurationPercentage(milliSeconds, scheduler);
                        var horizontalMove = anim.EaseAnimation(xease).Distance(targetX - fromX);
                        var verticalMove = anim.EaseAnimation(yease).Distance(targetY - fromY);

                        return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                            .ObserveOn(RxSchedulers.MainThreadScheduler)
                            .Do(p =>
                            {
                                tr.X = fromX + p.X;
                                tr.Y = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    RxSchedulers.MainThreadScheduler).SelectMany(x => x));

        /// <summary>Animates TranslateTransform X/Y to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> TranslateTo(double milliSeconds, double toX, double toY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
            element.TranslateTransform(milliSeconds, toX, toY, easeX, easeY, scheduler);

        /// <summary>Rotates the transform.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> RotateTransform(double milliSeconds, double angle, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(element, () => new RotateTransform());
                        var from = tr.Angle;

                        return AnimateValue(milliSeconds, from, angle, ease, scheduler)
                            .ObserveOn(RxSchedulers.MainThreadScheduler)
                            .Do(v => tr.Angle = v)
                            .Select(_ => Unit.Default);
                    },
                    RxSchedulers.MainThreadScheduler).SelectMany(x => x));

        /// <summary>Animates rotation angle to a target value (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toAngle">To angle.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> RotateTo(double milliSeconds, double toAngle, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            element.RotateTransform(milliSeconds, toAngle, ease, scheduler);

        /// <summary>Scale transform animation.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="scaleX">The scale x.</param>
        /// <param name="scaleY">The scale y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ScaleTransform(double milliSeconds, double scaleX, double scaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
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
                            .ObserveOn(RxSchedulers.MainThreadScheduler)
                            .Do(p =>
                            {
                                tr.ScaleX = fromX + p.X;
                                tr.ScaleY = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    RxSchedulers.MainThreadScheduler).SelectMany(x => x));

        /// <summary>Animates ScaleTransform to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toScaleX">To scale x.</param>
        /// <param name="toScaleY">To scale y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ScaleTo(double milliSeconds, double toScaleX, double toScaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
            element.ScaleTransform(milliSeconds, toScaleX, toScaleY, easeX, easeY, scheduler);

        /// <summary>Skew transform animation.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="angleX">The angle x.</param>
        /// <param name="angleY">The angle y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> SkewTransform(double milliSeconds, double angleX, double angleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
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
                            .ObserveOn(RxSchedulers.MainThreadScheduler)
                            .Do(p =>
                            {
                                tr.AngleX = fromX + p.X;
                                tr.AngleY = fromY + p.Y;
                            })
                            .Select(_ => Unit.Default);
                    },
                    RxSchedulers.MainThreadScheduler).SelectMany(x => x));

        /// <summary>Animates skew angles to target values (absolute).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="toAngleX">To angle x.</param>
        /// <param name="toAngleY">To angle y.</param>
        /// <param name="easeX">The ease x.</param>
        /// <param name="easeY">The ease y.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> SkewTo(double milliSeconds, double toAngleX, double toAngleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
            element.SkewTransform(milliSeconds, toAngleX, toAngleY, easeX, easeY, scheduler);

        /// <summary>Shakes an element horizontally using its TranslateTransform (returns to original position).</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="amplitude">The amplitude.</param>
        /// <param name="shakes">The shakes.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the shaking is done.</returns>
        public IObservable<Unit> ShakeTranslate(double milliSeconds, double amplitude, int shakes = 6, Ease ease = Ease.None, IScheduler? scheduler = null)
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
                            var t = p * Math.PI * 2.0 * shakes;
                            return Math.Sin(t) * amplitude * (1.0 - p);
                        })
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
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
                RxSchedulers.MainThreadScheduler).SelectMany(x => x));
        }

        /// <summary>Pulses opacity between two values for a specified number of pulses.</summary>
        /// <param name="milliSecondsPerHalf">The milli seconds per half.</param>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <param name="pulses">The pulses.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the pulsing is done.</returns>
        public IObservable<Unit> PulseOpacity(double milliSecondsPerHalf, double low = 0.2, double high = 1.0, int pulses = 1, Ease ease = Ease.None, IScheduler? scheduler = null)
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

    /// <summary>Provides animation extension methods for controls.</summary>
    /// <param name="element">The control to animate.</param>
    extension(Control element)
    {
        /// <summary>Animates width to a target value.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> WidthTo(double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => double.IsNaN(element.Width) ? element.Bounds.Width : element.Width, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(v => element.Width = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Animates height to a target value.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> HeightTo(double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => double.IsNaN(element.Height) ? element.Bounds.Height : element.Height, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(v => element.Height = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Animates margin to a target thickness.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> MarginTo(double milliSeconds, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => element.Margin, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(p => element.Margin = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Left attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasLeftTo(double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetLeft(element), RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(v => Canvas.SetLeft(element, v))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Top attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasTopTo(double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetTop(element), RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(v => Canvas.SetTop(element, v))
                        .Select(_ => Unit.Default)));
    }

    /// <summary>Provides animation extension methods for templated controls.</summary>
    /// <param name="element">The templated control to animate.</param>
    extension(TemplatedControl element)
    {
        /// <summary>Animates padding to a target thickness.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> PaddingTo(double milliSeconds, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => element.Padding, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(p => element.Padding = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));
    }

    /// <summary>Provides animation extension methods for solid color brushes.</summary>
    /// <param name="brush">The brush to animate.</param>
    extension(SolidColorBrush brush)
    {
        /// <summary>Animates SolidColorBrush Color.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> BrushColorTo(double milliSeconds, Color to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            Observable.Defer(() => Observable.Start(() => brush.Color, RxSchedulers.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxSchedulers.MainThreadScheduler)
                        .Do(p => brush.Color = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

        /// <summary>Alias for <see cref="BrushColorTo" />.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ColorTo(double milliSeconds, Color to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
            brush.BrushColorTo(milliSeconds, to, ease, scheduler);
    }

    /// <summary>Provides composition extension methods for animation sequences.</summary>
    /// <param name="animations">The animations to compose.</param>
    extension(IEnumerable<IObservable<Unit>> animations)
    {
        /// <summary>Sequences animations in order (Concat) and completes when the last completes.</summary>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> Sequence() =>
            animations.Aggregate(Observable.Return(Unit.Default), (acc, next) => acc.Concat(next));

        /// <summary>Runs animations in parallel and completes when all complete.</summary>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> Parallel() =>
            animations.Any() ? animations.Merge().LastOrDefaultAsync().Select(_ => Unit.Default) : Observable.Return(Unit.Default);

        /// <summary>Adds a delay between each animation in a sequence.</summary>
        /// <param name="delay">The delay.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> DelayBetween(TimeSpan delay, IScheduler? scheduler = null) =>
            animations.SelectMany(a => new[] { Observable.Timer(delay, scheduler ?? RxSchedulers.TaskpoolScheduler).Select(_ => Unit.Default), a }.AsEnumerable()).Sequence();

        /// <summary>Applies a stagger to a set of animations (delay incrementally).</summary>
        /// <param name="staggerBy">The stagger by.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An enumerable of staggered animations.</returns>
        /// <exception cref="ArgumentNullException">animations.</exception>
        public IEnumerable<IObservable<Unit>> Stagger(TimeSpan staggerBy, IScheduler? scheduler = null)
        {
            if (animations is null)
            {
                throw new ArgumentNullException(nameof(animations));
            }

            return StaggerIterator(staggerBy, scheduler);

            IEnumerable<IObservable<Unit>> StaggerIterator(TimeSpan staggerBy, IScheduler? scheduler)
            {
            var sched = scheduler ?? RxSchedulers.TaskpoolScheduler;
            var delay = TimeSpan.Zero;
            foreach (var anim in animations)
            {
                var start = Observable.Timer(delay, sched).Select(_ => Unit.Default);
                yield return start.Concat(anim);
                delay += staggerBy;
            }
            }
        }
    }

    /// <summary>Provides repeat extension methods for unit animation observables.</summary>
    /// <param name="animation">The animation to repeat.</param>
    extension(IObservable<Unit> animation)
    {
        /// <summary>Repeats an animation a specific number of times. If count is null, repeats forever.</summary>
        /// <param name="count">The count.</param>
        /// <returns>An observable that completes when the animation has repeated the specified number of times.</returns>
        public IObservable<Unit> RepeatAnimation(int? count = null) =>
            count is null ? animation.Repeat() : animation.Repeat(count.Value);
    }

    /// <summary>Linear interpolation between two thicknesses.</summary>
    /// <param name="a">The starting thickness.</param>
    /// <param name="b">The target thickness.</param>
    /// <param name="t">The interpolation percentage.</param>
    /// <returns>The interpolated thickness.</returns>
    private static Thickness Lerp(Thickness a, Thickness b, double t)
    {
        var p = Clamp(t, 0, 1);
        return new Thickness(
            a.Left + ((b.Left - a.Left) * p),
            a.Top + ((b.Top - a.Top) * p),
            a.Right + ((b.Right - a.Right) * p),
            a.Bottom + ((b.Bottom - a.Bottom) * p));
    }

    /// <summary>Linear interpolation between two colors.</summary>
    /// <param name="a">The starting color.</param>
    /// <param name="b">The target color.</param>
    /// <param name="t">The interpolation percentage.</param>
    /// <returns>The interpolated color.</returns>
    private static Color Lerp(Color a, Color b, double t)
    {
        var p = Clamp(t, 0, 1);
        byte LerpByte(byte x, byte y) => (byte)(x + ((y - x) * p));
        return Color.FromArgb(LerpByte(a.A, b.A), LerpByte(a.R, b.R), LerpByte(a.G, b.G), LerpByte(a.B, b.B));
    }

    /// <summary>Clamps the specified value.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The clamped value.</returns>
    private static double Clamp(double value, double min, double max)
    {
        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    /// <summary>Gets an existing transform of the requested type or adds a new one.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The element that owns the transform.</param>
    /// <param name="factory">The factory used when a transform must be created.</param>
    /// <returns>The existing or newly added transform.</returns>
    private static T GetOrAddTransform<T>(Visual element, Func<T> factory)
        where T : Transform
    {
        var current = element.RenderTransform;

        if (current is TransformGroup group)
        {
            var found = group.Children.OfType<T>().FirstOrDefault();
            if (found is not null)
            {
                return found;
            }

            var newTr = factory();
            group.Children.Add(newTr);
            element.RenderTransform = group;
            return newTr;
        }

        if (current is T existing)
        {
            var g = new TransformGroup();
            g.Children.Add(existing);
            element.RenderTransform = g;
            return existing;
        }

        if (current is Transform single)
        {
            var g = new TransformGroup();
            g.Children.Add(single);
            var newTr = factory();
            g.Children.Add(newTr);
            element.RenderTransform = g;
            return newTr;
        }

        var groupNew = new TransformGroup();
        var newTransform = factory();
        groupNew.Children.Add(newTransform);
        element.RenderTransform = groupNew;
        return newTransform;
    }
}
