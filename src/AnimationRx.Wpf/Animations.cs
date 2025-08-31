/*
Copyright (c) Chris Pulman. All rights reserved.
Licensed under the MIT license. See LICENSE file in the project root for full license information.
*/

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI;

namespace CP.AnimationRx;

/// <summary>
/// Animations.
/// </summary>
public static class Animations
{
    /// <summary>
    /// Animates the frame using an interval based on frames-per-second.
    /// </summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable that ticks every frame.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
        {
            var fps = framesPerSecond <= 0 ? 60.0 : framesPerSecond;
            var period = TimeSpan.FromMilliseconds(1000.0 / fps);
            return Observable.Interval(period, scheduler ?? RxApp.TaskpoolScheduler);
        });

    /// <summary>
    /// Animates a frame stream synchronized with WPF CompositionTarget.Rendering (UI refresh rate, usually monitor Hz).
    /// </summary>
    /// <returns>An observable that produces a tick per WPF render frame on the main thread.</returns>
    public static IObservable<long> RenderFrames() =>
        Observable.Create<long>(observer =>
        {
            long i = 0;
            void Handler(object? s, EventArgs e) => observer.OnNext(i++);
            CompositionTarget.Rendering += Handler;
            return Disposable.Create(() => CompositionTarget.Rendering -= Handler);
        }).ObserveOn(RxApp.MainThreadScheduler);

    /// <summary>
    /// Convenience overload to drive frames by a fixed period.
    /// </summary>
    /// <param name="period">The frame period.</param>
    /// <param name="scheduler">Optional scheduler to schedule the frames.</param>
    /// <returns>An observable sequence of frame indices emitted on the given period.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler = null) =>
        Observable.Interval(period, scheduler ?? RxApp.TaskpoolScheduler);

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
        Observable.Defer(() => Observable.Start(() => element.Margin.Bottom, RxApp.MainThreadScheduler)
                    .SelectMany(initialValue =>
                        DurationPercentage(milliSeconds, scheduler)
                            .EaseAnimation(ease)
                            .Distance(position - initialValue)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Do(t =>
                            {
                                var mar = element.Margin;
                                mar.Bottom = initialValue + t;
                                element.Margin = mar;
                            })
                            .Select(_ => Unit.Default)));

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
    /// Produces a percentage of the duration from a changing time span observable.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The animation duration in milliseconds, provided as an observable (to allow dynamic duration).</param>
    /// <param name="scheduler">The scheduler to drive the time source; defaults to RxApp.TaskpoolScheduler.</param>
    /// <returns>An observable of Duration representing the percentage (0..1) over time.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            MilliSecondsElapsed(scheduler ?? RxApp.TaskpoolScheduler)
                .CombineLatest(milliSeconds, (ems, ms) => ms <= 0 ? 1.0 : ems / ms)
                .ToDuration()
                .TakeWhile(t => t.Percent < 1)
                .Concat(Observable.Return(Duration.Create(1))));

    /// <summary>
    /// Produces a percentage of the duration for a fixed time span in milliseconds.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The animation duration in milliseconds.</param>
    /// <param name="scheduler">The scheduler to drive the time source; defaults to RxApp.TaskpoolScheduler.</param>
    /// <returns>An observable of Duration representing the percentage (0..1) over time.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            MilliSecondsElapsed(scheduler ?? RxApp.TaskpoolScheduler)
                .Select(ems => milliSeconds <= 0 ? 1.0 : ems / milliSeconds)
                .ToDuration()
                .TakeWhile(t => t.Percent < 1)
                .Concat(Observable.Return(Duration.Create(1))));

    /// <summary>
    /// Lefts the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milliseconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler used for ObserveOn.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> LeftMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, (ms, p) => new { ms, p })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await LeftMarginMoveCore(element, v.ms, v.p, ease, scheduler))));

    /// <summary>
    /// Lefts the margin move.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="position">The position.</param>
    /// <param name="ease">The ease stream.</param>
    /// <param name="scheduler">The scheduler used for ObserveOn.</param>
    /// <returns>A Value.</returns>
    public static IObservable<Unit> LeftMarginMove(this FrameworkElement element, IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease, IScheduler? scheduler = null) =>
        Observable.Create<Unit>(obs =>
            milliSeconds
            .CombineLatest(position, ease, (ms, p, e) => new { ms, p, e })
            .ObserveOn(scheduler ?? RxApp.MainThreadScheduler)
            .Subscribe(async v => obs.OnNext(await LeftMarginMoveCore(element, v.ms, v.p, v.e, scheduler))));

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
        LeftMarginMoveCore(element, milliSeconds, position, ease, scheduler);

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
        Observable.Defer(() => Observable.Start(() => element.Margin.Right, RxApp.MainThreadScheduler)
                    .SelectMany(initialValue =>
                        DurationPercentage(milliSeconds, scheduler)
                            .EaseAnimation(ease)
                            .Distance(position - initialValue)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Do(t =>
                            {
                                var mar = element.Margin;
                                mar.Right = initialValue + t;
                                element.Margin = mar;
                            })
                            .Select(_ => Unit.Default)));

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
        Observable.Defer(() => Observable.Start(
                 () =>
             {
                 var trRot = GetOrAddTransform(element, () => new RotateTransform(0, element.ActualWidth / 2, element.ActualHeight / 2));

                 var initialValue = trRot.Angle;

                 return DurationPercentage(milliSeconds, scheduler)
                     .EaseAnimation(ease)
                     .Distance(angle - initialValue)
                     .ObserveOn(RxApp.MainThreadScheduler)
                     .Do(t => trRot.Angle = initialValue + t)
                     .Select(_ => Unit.Default);
             },
                 RxApp.MainThreadScheduler).SelectMany(x => x));

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
                        async v =>
                        {
                            await Task.Delay(interval);
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
        Observable.Defer(() => Observable.Start(() => element.Margin.Top, RxApp.MainThreadScheduler)
                    .SelectMany(initialValue =>
                        DurationPercentage(milliSeconds, scheduler)
                    .EaseAnimation(ease)
                    .Distance(position - initialValue)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(t =>
                    {
                        var mar = element.Margin;
                        mar.Top = initialValue + t;
                        element.Margin = mar;
                    })
                    .Select(_ => Unit.Default)));

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
        Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var trTns = GetOrAddTransform(element, () => new TranslateTransform(element.RenderTransform.Value.OffsetX, element.RenderTransform.Value.OffsetY));

                        var xinitialValue = trTns.X;
                        var yinitialValue = trTns.Y;

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
                    },
                    RxApp.MainThreadScheduler).SelectMany(x => x));

    /// <summary>
    /// Animates element opacity to a target value.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target opacity (0..1).</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> OpacityTo(this UIElement element, double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => element.Opacity, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(v => element.Opacity = v)
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates FrameworkElement width to a target value.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target width.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> WidthTo(this FrameworkElement element, double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => double.IsNaN(element.Width) ? element.ActualWidth : element.Width, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(v => element.Width = v)
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates FrameworkElement height to a target value.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target height.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> HeightTo(this FrameworkElement element, double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => double.IsNaN(element.Height) ? element.ActualHeight : element.Height, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(v => element.Height = v)
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates Margin to a target thickness.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target thickness.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> MarginTo(this FrameworkElement element, double milliSeconds, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => element.Margin, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(p => element.Margin = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates Padding to a target thickness (for Controls).
    /// </summary>
    /// <param name="element">The control to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target padding.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> PaddingTo(this Control element, double milliSeconds, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => element.Padding, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(p => element.Padding = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates Canvas.Left attached property.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target left position.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> CanvasLeftTo(this FrameworkElement element, double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => double.IsNaN(Canvas.GetLeft(element)) ? 0 : Canvas.GetLeft(element), RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(v => Canvas.SetLeft(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates Canvas.Top attached property.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target top position.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> CanvasTopTo(this FrameworkElement element, double milliSeconds, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => double.IsNaN(Canvas.GetTop(element)) ? 0 : Canvas.GetTop(element), RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(v => Canvas.SetTop(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Animates SolidColorBrush Color for Background-like properties.
    /// </summary>
    /// <param name="brush">The brush to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="to">The target color.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> BrushColorTo(this SolidColorBrush brush, double milliSeconds, Color to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(() => brush.Color, RxApp.MainThreadScheduler)
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(p => brush.Color = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>
    /// Scale transform animation.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="scaleX">Target scale on X.</param>
    /// <param name="scaleY">Target scale on Y.</param>
    /// <param name="easeX">Easing for X.</param>
    /// <param name="easeY">Easing for Y.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> ScaleTransform(this FrameworkElement element, double milliSeconds, double scaleX, double scaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(element, () => new ScaleTransform(1, 1, element.ActualWidth / 2, element.ActualHeight / 2));
                    var sx = tr.ScaleX;
                    var sy = tr.ScaleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var xMove = anim.EaseAnimation(easeX).Distance(scaleX - sx);
                    var yMove = anim.EaseAnimation(easeY).Distance(scaleY - sy);

                    return xMove.CombineLatest(yMove, (x, y) => new Point(x, y))
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(p =>
                        {
                            tr.ScaleX = sx + p.X;
                            tr.ScaleY = sy + p.Y;
                        })
                        .Select(_ => Unit.Default);
                },
                RxApp.MainThreadScheduler).SelectMany(x => x));

    /// <summary>
    /// Skew transform animation.
    /// </summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="angleX">Target angle X.</param>
    /// <param name="angleY">Target angle Y.</param>
    /// <param name="easeX">Easing for X.</param>
    /// <param name="easeY">Easing for Y.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A Unit observable that completes when the animation ends.</returns>
    public static IObservable<Unit> SkewTransform(this FrameworkElement element, double milliSeconds, double angleX, double angleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(element, () => new SkewTransform(0, 0, element.ActualWidth / 2, element.ActualHeight / 2));
                    var ax = tr.AngleX;
                    var ay = tr.AngleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var xMove = anim.EaseAnimation(easeX).Distance(angleX - ax);
                    var yMove = anim.EaseAnimation(easeY).Distance(angleY - ay);

                    return xMove.CombineLatest(yMove, (x, y) => new Point(x, y))
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(p =>
                        {
                            tr.AngleX = ax + p.X;
                            tr.AngleY = ay + p.Y;
                        })
                        .Select(_ => Unit.Default);
                },
                RxApp.MainThreadScheduler).SelectMany(x => x));

    /// <summary>
    /// Sequences animations in order (Concat) and completes when the last completes.
    /// </summary>
    /// <param name="animations">The animations to play sequentially.</param>
    /// <returns>An observable that completes when all animations finish.</returns>
    public static IObservable<Unit> Sequence(this IEnumerable<IObservable<Unit>> animations)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        return animations.Aggregate(Observable.Return(Unit.Default), (acc, next) => acc.Concat(next));
    }

    /// <summary>
    /// Runs animations in parallel and completes when all complete.
    /// </summary>
    /// <param name="animations">The animations to play in parallel.</param>
    /// <returns>An observable that completes when all animations finish.</returns>
    public static IObservable<Unit> Parallel(this IEnumerable<IObservable<Unit>> animations)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        return animations.Any() ? animations.Merge().LastOrDefaultAsync().Select(_ => Unit.Default) : Observable.Return(Unit.Default);
    }

    /// <summary>
    /// Repeats an animation a specific number of times. If count is null, repeats forever.
    /// </summary>
    /// <param name="animation">The animation to repeat.</param>
    /// <param name="count">Number of repetitions; null for infinite.</param>
    /// <returns>The repeated animation sequence.</returns>
    public static IObservable<Unit> Repeat(this IObservable<Unit> animation, int? count = null)
    {
        if (animation is null)
        {
            throw new ArgumentNullException(nameof(animation));
        }

        return count is null ? animation.Repeat() : animation.Repeat(count.Value);
    }

    /// <summary>
    /// Adds a delay between each animation in a sequence.
    /// </summary>
    /// <param name="animations">The animations.</param>
    /// <param name="delay">The delay to insert between animations.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A sequence of animations separated by the specified delay.</returns>
    public static IObservable<Unit> DelayBetween(this IEnumerable<IObservable<Unit>> animations, TimeSpan delay, IScheduler? scheduler = null)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        var sched = scheduler ?? RxApp.TaskpoolScheduler;
        return animations.SelectMany(a => new[] { Observable.Timer(delay, sched).Select(_ => Unit.Default), a }.AsEnumerable())
                         .Sequence();
    }

    /// <summary>
    /// Applies a stagger to a set of animations (delay incrementally).
    /// </summary>
    /// <param name="animations">The animations to stagger.</param>
    /// <param name="staggerBy">The incremental delay between each animation.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>A new enumerable of animations each delayed by the stagger.</returns>
    public static IEnumerable<IObservable<Unit>> Stagger(this IEnumerable<IObservable<Unit>> animations, TimeSpan staggerBy, IScheduler? scheduler = null)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        var sched = scheduler ?? RxApp.TaskpoolScheduler;
        var delay = TimeSpan.Zero;
        foreach (var anim in animations)
        {
            var start = Observable.Timer(delay, sched).Select(_ => Unit.Default);
            yield return start.Concat(anim);
            delay += staggerBy;
        }
    }

    /// <summary>
    /// Helper to create a value animation from a start value to an end value using easing.
    /// </summary>
    /// <param name="milliSeconds">The duration in milliseconds.</param>
    /// <param name="from">The start value.</param>
    /// <param name="to">The end value.</param>
    /// <param name="ease">The easing to use.</param>
    /// <param name="scheduler">Optional scheduler to drive the animation.</param>
    /// <returns>An observable that produces the interpolated value over time.</returns>
    public static IObservable<double> AnimateValue(double milliSeconds, double from, double to, Ease ease = Ease.None, IScheduler? scheduler = null) =>
        Observable.Defer(() =>
            DurationPercentage(milliSeconds, scheduler)
                .EaseAnimation(ease)
                .Distance(to - from)
                .Select(delta => from + delta));

    private static IObservable<Unit> LeftMarginMoveCore(FrameworkElement element, double milliSeconds, double position, Ease ease, IScheduler? scheduler)
        => Observable.Defer(() => Observable.Start(() => element.Margin.Left, RxApp.MainThreadScheduler)
                .SelectMany(initialValue =>
                    DurationPercentage(milliSeconds, scheduler)
                        .EaseAnimation(ease)
                        .Distance(position - initialValue)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(t =>
                        {
                            var mar = element.Margin;
                            mar.Left = initialValue + t;
                            element.Margin = mar;
                        })
                        .Select(_ => Unit.Default)));

    private static T GetOrAddTransform<T>(FrameworkElement element, Func<T> factory)
        where T : Transform
    {
        if (element.RenderTransform is TransformGroup existingGroup)
        {
            var found = existingGroup.Children.OfType<T>().FirstOrDefault();
            if (found != null)
            {
                return found;
            }

            var newTr = factory();
            existingGroup.Children.Add(newTr);
            return newTr;
        }

        if (element.RenderTransform is T existing)
        {
            // Wrap existing in a TransformGroup to safely add others later
            var group = new TransformGroup();
            group.Children.Add(existing);
            element.RenderTransform = group;
            return existing;
        }
        else if (element.RenderTransform is Transform single)
        {
            // Preserve the existing single transform by moving it into a group
            var group = new TransformGroup();
            group.Children.Add(single);
            var newTr = factory();
            group.Children.Add(newTr);
            element.RenderTransform = group;
            return newTr;
        }
        else
        {
            var group = new TransformGroup();
            var newTr = factory();
            group.Children.Add(newTr);
            element.RenderTransform = group;
            return newTr;
        }
    }

    /// <summary>
    /// Clamps the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>The clamped value.</returns>
    private static double Clamp(double value, double min, double max) => value < min ? min : (value > max ? max : value);

    /// <summary>
    /// Linear Interpolation between two thicknesses.
    /// </summary>
    private static Thickness Lerp(Thickness a, Thickness b, double t)
    {
        var p = Clamp(t, 0, 1);
        return new Thickness(
            a.Left + ((b.Left - a.Left) * p),
            a.Top + ((b.Top - a.Top) * p),
            a.Right + ((b.Right - a.Right) * p),
            a.Bottom + ((b.Bottom - a.Bottom) * p));
    }

    /// <summary>
    /// Linear Interpolation between two colours.
    /// </summary>
    private static Color Lerp(Color a, Color b, double t)
    {
        var p = Clamp(t, 0, 1);
        byte LerpByte(byte x, byte y) => (byte)(x + ((y - x) * p));
        return Color.FromArgb(LerpByte(a.A, b.A), LerpByte(a.R, b.R), LerpByte(a.G, b.G), LerpByte(a.B, b.B));
    }
}
