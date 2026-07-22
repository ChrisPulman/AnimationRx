// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;

namespace CP.AnimationRx;

/// <summary>Provides WPF timing and margin animation helpers.</summary>
public static partial class Animations
{
    /// <summary>Defines the default frames per second.</summary>
    private const double DefaultFramesPerSecond = 60.0;

    /// <summary>Defines the milliseconds per second.</summary>
    private const double MillisecondsPerSecond = 1000.0;

    /// <summary>Defines the default frame interval in milliseconds.</summary>
    private const double DefaultFrameIntervalMilliseconds = 16.0;

    /// <summary>Defines the completed duration percentage.</summary>
    private const double CompletePercentage = 1.0;

    /// <summary>Defines the transform center divisor.</summary>
    private const double TransformCenterDivisor = 2.0;

    /// <summary>Defines the identity transform scale.</summary>
    private const double IdentityScale = 1.0;

    /// <summary>Defines the default transform angle.</summary>
    private const double DefaultAngle = 0.0;

    /// <summary>Defines the full-circle radians multiplier.</summary>
    private const double FullCircleRadiansMultiplier = 2.0;

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="framesPerSecond">The framesPerSecond value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler) =>
        Observable.Defer(() =>
        {
            var fps = framesPerSecond <= 0 ? DefaultFramesPerSecond : framesPerSecond;
            var period = TimeSpan.FromMilliseconds(MillisecondsPerSecond / fps);
            return Observable.Interval(period, scheduler ?? GetBackgroundScheduler());
        });

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="period">The period value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler) =>
        Observable.Interval(period, scheduler ?? GetBackgroundScheduler());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <returns>The resulting observable.</returns>
    public static IObservable<long> RenderFrames() =>
        Observable.Create<long>(observer =>
        {
            long i = 0;
            void Handler(object? sender, EventArgs args)
            {
                _ = sender;
                _ = args;
                observer.OnNext(i);
                i++;
            }

            CompositionTarget.Rendering += Handler;
            return Disposable.Create(() => CompositionTarget.Rendering -= Handler);
        }).ObserveOn(GetUiScheduler());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, (ms, p) => (ms, p))
                .Select(v => BottomMarginMove(element, v.ms, v.p, ease, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                .Select(v => BottomMarginMove(element, v.ms, v.p, v.e, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BottomMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Margin.Bottom, GetUiScheduler())
                    .SelectMany(initialValue =>
                        DurationPercentage(milliSeconds, scheduler)
                            .EaseAnimation(ease)
                            .Distance(position - initialValue)
                            .ObserveOn(GetUiScheduler())
                            .Do(t =>
                            {
                                var mar = element.Margin;
                                mar.Bottom = initialValue + t;
                                element.Margin = mar;
                            })
                            .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source duration observable.</param>
    /// <param name="distance">The distance value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> Distance(IObservable<Duration> @this, double distance) =>
        Observable.Defer(() => @this.Select(t => t.Percent * distance));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source duration observable.</param>
    /// <param name="distance">The distance value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> Distance(IObservable<Duration> @this, IObservable<double> distance) =>
        Observable.Defer(() => @this.CombineLatest(distance, (du, d) => du.Percent * d));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler) =>
        Observable.Defer(() => milliSeconds
            .Select(ms => DurationPercentage(ms, scheduler))
            .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler) =>
        Observable.Defer(() => milliSeconds <= 0
            ? Observable.Return(Duration.Create(CompletePercentage))
            : MilliSecondsElapsed(scheduler ?? GetBackgroundScheduler())
                .Select(ems => ems / milliSeconds)
                .ToDuration()
                .TakeWhile(t => t.Percent < CompletePercentage)
                .Concat(Observable.Return(Duration.Create(CompletePercentage))));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, (ms, p) => (ms, p))
                .Select(v => LeftMarginMoveCore(element, v.ms, v.p, ease, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                .Select(v => LeftMarginMoveCore(element, v.ms, v.p, v.e, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> LeftMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        LeftMarginMoveCore(element, milliSeconds, position, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) =>
        Observable.Defer(() =>
        {
            var start = scheduler.Now;
            return Observable.Interval(TimeSpan.FromMilliseconds(DefaultFrameIntervalMilliseconds), scheduler)
                .Select(_ => (scheduler.Now - start).TotalMilliseconds)
                .StartWith(0.0);
        });

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source observable.</param>
    /// <param name="velocity">The velocity value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<double> PixelsPerSecond(IObservable<double> @this, double velocity) =>
        Observable.Defer(() => @this.Select(ms => velocity * ms / MillisecondsPerSecond));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, (ms, p) => (ms, p))
                .Select(v => RightMarginMoveCore(element, v.ms, v.p, ease, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RightMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                .Select(v => RightMarginMoveCore(element, v.ms, v.p, v.e, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> angle,
        Ease ease) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(angle, (ms, a) => (ms, a))
                .Select(v => RotateTransform(element, v.ms, v.a, ease))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> angle,
        IObservable<Ease> ease) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(angle, ease, (ms, a, e) => (ms, a, e))
                .Select(v => RotateTransform(element, v.ms, v.a, v.e))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angle">The angle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTransform(
        FrameworkElement element,
        double milliSeconds,
        double angle,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                 () =>
             {
                 var rotateTransform = GetOrAddTransform(
                     element,
                     () => new RotateTransform(
                         DefaultAngle,
                         element.ActualWidth / TransformCenterDivisor,
                         element.ActualHeight / TransformCenterDivisor));

                 var initialValue = rotateTransform.Angle;

                 return DurationPercentage(milliSeconds, scheduler)
                     .EaseAnimation(ease)
                     .Distance(angle - initialValue)
                     .ObserveOn(GetUiScheduler())
                     .Do(t => rotateTransform.Angle = initialValue + t)
                     .Select(_ => Unit.Default);
             },
                 GetUiScheduler()).SelectMany(x => x));

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
        Observable.Defer(() =>
        {
            var sched = scheduler ?? GetBackgroundScheduler();
            return @this.Select(v => Observable.Return(v).Delay(interval, sched)).Concat();
        });

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="this">The source observable.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ToDuration(IObservable<double> @this) =>
        Observable.Defer(() => @this.Select(Duration.Create));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, (ms, p) => (ms, p))
                .Select(v => TopMarginMoveCore(element, v.ms, v.p, ease, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<double> position,
        IObservable<Ease> ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                .Select(v => TopMarginMoveCore(element, v.ms, v.p, v.e, scheduler))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TopMarginMove(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        TopMarginMoveCore(element, milliSeconds, position, ease, scheduler);
}
