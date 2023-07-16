// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using ReactiveUI;

namespace CP.AnimationRx;

/// <summary>
/// Animations.
/// </summary>
public static class Animations
{
    /// <summary>
    /// Animates the frame.
    /// </summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <returns>A Value.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond) =>
        Observable.Defer(() => Observable.Interval(TimeSpan.FromMilliseconds(1000 / framesPerSecond)));

    /// <summary>
    /// Bottoms the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> BottomMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, Ease ease = Ease.None) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await BottomMarginMove(element, v.ms, v.p, ease))));

    /// <summary>
    /// Bottoms the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> BottomMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await BottomMarginMove(element, v.ms, v.p, v.e))));

    /// <summary>
    /// Bottoms the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An Observabel Unit.</returns>
    public static IObservable<Unit> BottomMarginMove(this FrameworkElement element, double milliSeconds, double position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            {
                var initialValue = element.Margin.Bottom;
                return DurationPercentage(milliSeconds, scheduler)
                    .EaseAnimation(ease)
                    .Distance(position - initialValue)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(t =>
                    {
                        var mar = element.Margin;
                        mar.Bottom = initialValue + t;
                        element.Margin = mar;
                    })
                    .Select(_ => Unit.Default);
            });

    /// <summary>
    /// Distances the specified distance.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <param name="distance">The distance.</param>
    /// <returns>A Value.</returns>
    public static IObservable<double> Distance(this IObservable<Duration> @this, double distance) =>
        Observable.Create((IObserver<double> obs) => @this.Select(t => t.Percent * distance).Subscribe(obs));

    /// <summary>
    /// Distances the specified distance.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <param name="distance">The distance.</param>
    /// <returns>A Value.</returns>
    public static IObservable<double> Distance(this IObservable<Duration> @this, IObservable<double> distance) =>
        Observable.Create((IObserver<double> obs) => @this.CombineLatest(distance, (du, d) => du.Percent * d).Subscribe(obs));

    /// <summary>
    /// Produces a percentage of the duration.
    /// </summary>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() => MilliSecondsElapsed(scheduler ?? RxApp.TaskpoolScheduler).CombineLatest(milliSeconds, (ems, ms) => ems / ms).ToDuration().TakeWhile(t => t.Percent <= 1));

    /// <summary>
    /// Produces a percentage of the duration.
    /// </summary>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() => MilliSecondsElapsed(scheduler ?? RxApp.TaskpoolScheduler).Select(ems => ems / milliSeconds).ToDuration().TakeWhile(t => t.Percent <= 1));

    /// <summary>
    /// Lefts the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> LeftMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await LeftMarginMove(element, v.ms, v.p, ease))));

    /// <summary>
    /// Lefts the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> LeftMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await LeftMarginMove(element, v.ms, v.p, v.e))));

    /// <summary>
    /// Lefts the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> LeftMarginMove(this FrameworkElement element, double milliSeconds, double position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
        {
            var initialValue = element.Margin.Left;
            return DurationPercentage(milliSeconds, scheduler)
                .EaseAnimation(ease)
                .Distance(position - initialValue)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(t =>
                {
                    var mar = element.Margin;
                    mar.Left = initialValue + t;
                    element.Margin = mar;
                })
                .Select(_ => Unit.Default);
        });

    /// <summary>
    /// Milliseconds elapsed.
    /// </summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) =>
        Observable.Defer(() =>
            {
                var start = scheduler.Now;
                return Observable.Interval(TimeSpan.FromMilliseconds(1), scheduler).Select(_ => (scheduler.Now - start).TotalMilliseconds);
            });

    /// <summary>
    /// Pixels per second.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <param name="velocity">The velocity.</param>
    /// <returns>A Value.</returns>
    public static IObservable<double> PixelsPerSecond(this IObservable<double> @this, double velocity) =>
        Observable.Create((IObserver<double> obs) => @this.Select(ms => velocity * ms / 1000).Subscribe(obs));

    /// <summary>
    /// Rights the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The Milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RightMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await RightMarginMove(element, v.ms, v.p, ease))));

    /// <summary>
    /// Rights the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RightMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await RightMarginMove(element, v.ms, v.p, v.e))));

    /// <summary>
    /// Rights the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The Milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RightMarginMove(this FrameworkElement element, double milliSeconds, double position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            {
                var initialValue = element.Margin.Right;
                return DurationPercentage(milliSeconds, scheduler)
                    .EaseAnimation(ease)
                    .Distance(position - initialValue)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(t =>
                    {
                        var mar = element.Margin;
                        mar.Right = initialValue + t;
                        element.Margin = mar;
                    })
                    .Select(_ => Unit.Default);
            });

    /// <summary>
    /// Rotates the transform.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The Milli seconds.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RotateTransform(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> angle, Ease ease = Ease.None) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(angle, (ms, p) => new { ms, p })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await RotateTransform(element, v.ms, v.p, ease))));

    /// <summary>
    /// Rotates the transform.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RotateTransform(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> angle, IObservable<Ease> ease) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(angle, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await RotateTransform(element, v.ms, v.p, v.e))));

    /// <summary>
    /// Rotates the transform.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The Milli seconds.</param>
    /// <param name="angle">The angle.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> RotateTransform(this FrameworkElement element, double milliSeconds, double angle, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
        {
            var trGrp = new TransformGroup();
            var rt = element.RenderTransform as TransformGroup;
            var lastrt = rt?.Children?[0] as RotateTransform;
            var trRot = lastrt ?? new RotateTransform(0, element.ActualWidth / 2, element.ActualHeight / 2);
            trGrp.Children.Add(trRot);
            var initialValue = trRot.Angle;
            element.RenderTransform = trGrp;

            return DurationPercentage(milliSeconds, scheduler)
                .EaseAnimation(ease)
                .Distance(angle - initialValue)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(t => trRot.Angle = initialValue + t)
            .Select(_ => Unit.Default);
        });

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
    public static IObservable<T> TakeOneEvery<T>(this IObservable<T> @this, TimeSpan interval, IScheduler? scheduler = null) =>
        Observable.Create<T>(obs =>
            @this.ObserveOn(scheduler ?? RxApp.TaskpoolScheduler)
                .Subscribe(
                        v =>
                        {
                            Task.Delay(interval).Wait();
                            obs.OnNext(v);
                        },
                        () => obs.OnCompleted()));

    /// <summary>
    /// double To duration.
    /// </summary>
    /// <param name="this">An IObservable double.</param>
    /// <returns>
    /// An IObservable Duration.
    /// </returns>
    public static IObservable<Duration> ToDuration(this IObservable<double> @this) =>
        Observable.Defer(() => @this.Select(Duration.Create));

    /// <summary>
    /// Tops the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>
    /// A Value.
    /// </returns>
    public static IObservable<Unit> TopMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, Ease ease = Ease.None) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await TopMarginMove(element, v.ms, v.p, ease))));

    /// <summary>
    /// Tops the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> TopMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await TopMarginMove(element, v.ms, v.p, v.e))));

    /// <summary>
    /// Tops the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> TopMarginMove(this FrameworkElement element, double milliSeconds, double position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            {
                var initialValue = element.Margin.Top;
                return DurationPercentage(milliSeconds, scheduler)
            .EaseAnimation(ease)
            .Distance(position - initialValue)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(t =>
            {
                var mar = element.Margin;
                mar.Top = initialValue + t;
                element.Margin = mar;
            })
            .Select(_ => Unit.Default);
            });

    /// <summary>
    /// Execute Translate Transform.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="xease">The x ease.</param>
    /// <param name="yease">The y ease.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> TranslateTransform(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<Point> position, Ease xease = Ease.None, Ease yease = Ease.None) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await TranslateTransform(element, v.ms, v.p.X, v.p.Y, xease, yease))));

    /// <summary>
    /// Execute Translate Transform.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="xPosition">The x position.</param>
    /// <param name="yPosition">The y position.</param>
    /// <param name="xease">The x ease.</param>
    /// <param name="yease">The y ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>
    /// A Value.
    /// </returns>
    public static IObservable<Unit> TranslateTransform(this FrameworkElement element, double milliSeconds, double xPosition, double yPosition, Ease xease = Ease.None, Ease yease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            {
                var trTns = new TranslateTransform(element.RenderTransform.Value.OffsetX, element.RenderTransform.Value.OffsetY);

                var trGrp = new TransformGroup();
                trGrp.Children.Add(trTns);
                var xinitialValue = trTns.X;
                var yinitialValue = trTns.Y;
                element.RenderTransform = trGrp;

                var anim = DurationPercentage(milliSeconds, scheduler);
                var xMove = anim.EaseAnimation(xease).Distance(xPosition - xinitialValue);
                var yMove = anim.EaseAnimation(yease).Distance(yPosition - yinitialValue);

                return xMove.CombineLatest(yMove, (x, y) => new Point(x, y))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(t =>
                {
                    trTns.X = xinitialValue + t.X;
                    trTns.Y = yinitialValue + t.Y;
                })
                .Select(_ => Unit.Default);
            });
}
