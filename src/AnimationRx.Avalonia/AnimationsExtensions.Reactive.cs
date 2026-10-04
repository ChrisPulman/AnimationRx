// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using Avalonia;
using Avalonia.Controls;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides observable driven layout and transform animations.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Converts raw percentages into animation durations.</summary>
    /// <param name="source">The percentage stream.</param>
    /// <returns>The corresponding durations.</returns>
    public static IObservable<Duration> ToDuration(IObservable<double> source) =>
        EasesExtensions.ToDuration(source)
;

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    private static IObservable<Unit> RightMarginMoveCore(Control element, double milliSeconds, double position, Ease ease, IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Margin.Right, GetUiScheduler())
                .SelectMany(initialValue =>
                    DurationPercentage(milliSeconds, scheduler)
                        .EaseAnimation(ease)
                        .Distance(position - initialValue)
                        .ObserveOn(GetUiScheduler())
                        .Do(t =>
                        {
                            var mar = element.Margin;
                            element.Margin = new(mar.Left, mar.Top, initialValue + t, mar.Bottom);
                        })
                        .Select(_ => Unit.Default)));

/// <summary>Provides reactive BottomMarginMove extensions.</summary>
    /// <param name = "element">The element to animate.</param>
    extension(Control element)
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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, (ms, p) => (ms, p))
                    .Select(v => BottomMarginMove(element, v.ms, v.p, ease, scheduler))
                    .Switch());

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <param name="scheduler">The scheduler value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(
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
            Observable.Defer(() => Observable.Start(() => element.Margin.Bottom, GetUiScheduler())
                        .SelectMany(initialValue =>
                            DurationPercentage(milliSeconds, scheduler)
                                .EaseAnimation(ease)
                                .Distance(position - initialValue)
                                .ObserveOn(GetUiScheduler())
                                .Do(t =>
                                {
                                    var mar = element.Margin;
                                    element.Margin = new(mar.Left, mar.Top, mar.Right, initialValue + t);
                                })
                                .Select(_ => Unit.Default)));

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(IObservable<double> milliSeconds, IObservable<double> position) =>
            BottomMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(IObservable<double> milliSeconds, IObservable<double> position, Ease ease) =>
            BottomMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
            BottomMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(double milliSeconds, double position) =>
            BottomMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> BottomMarginMove(double milliSeconds, double position, Ease ease) =>
            BottomMarginMove(element, milliSeconds, position, ease, null)
;

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, (ms, p) => (ms, p))
                    .Select(v => LeftMarginMove(element, v.ms, v.p, ease, scheduler))
                    .Switch());

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                    .Select(v => LeftMarginMove(element, v.ms, v.p, v.e, scheduler))
                    .Switch());

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
            Observable.Defer(() => Observable.Start(() => element.Margin.Left, GetUiScheduler())
                    .SelectMany(initialValue =>
                        DurationPercentage(milliSeconds, scheduler)
                            .EaseAnimation(ease)
                            .Distance(position - initialValue)
                            .ObserveOn(GetUiScheduler())
                            .Do(t =>
                            {
                                var mar = element.Margin;
                                element.Margin = new(initialValue + t, mar.Top, mar.Right, mar.Bottom);
                            })
                            .Select(_ => Unit.Default)));

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(IObservable<double> milliSeconds, IObservable<double> position) =>
            LeftMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(IObservable<double> milliSeconds, IObservable<double> position, Ease ease) =>
            LeftMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
            LeftMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(double milliSeconds, double position) =>
            LeftMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> LeftMarginMove(double milliSeconds, double position, Ease ease) =>
            LeftMarginMove(element, milliSeconds, position, ease, null)
;

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, (ms, p) => (ms, p))
                    .Select(v => RightMarginMoveCore(element, v.ms, v.p, ease, scheduler))
                    .Switch());

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                    .Select(v => RightMarginMoveCore(element, v.ms, v.p, v.e, scheduler))
                    .Switch());

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RightMarginMove(IObservable<double> milliSeconds, IObservable<double> position) =>
            RightMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RightMarginMove(IObservable<double> milliSeconds, IObservable<double> position, Ease ease) =>
            RightMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RightMarginMove(IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
            RightMarginMove(element, milliSeconds, position, ease, null)
;

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, (ms, p) => (ms, p))
                    .Select(v => TopMarginMove(element, v.ms, v.p, ease, scheduler))
                    .Switch());

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, ease, (ms, p, e) => (ms, p, e))
                    .Select(v => TopMarginMove(element, v.ms, v.p, v.e, scheduler))
                    .Switch());

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
            Observable.Defer(() => Observable.Start(() => element.Margin.Top, GetUiScheduler())
                .SelectMany(initialValue =>
                    DurationPercentage(milliSeconds, scheduler)
                        .EaseAnimation(ease)
                        .Distance(position - initialValue)
                        .ObserveOn(GetUiScheduler())
                        .Do(t =>
                        {
                            var mar = element.Margin;
                            element.Margin = new(mar.Left, initialValue + t, mar.Right, mar.Bottom);
                        })
                        .Select(_ => Unit.Default)));

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(IObservable<double> milliSeconds, IObservable<double> position) =>
            TopMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(IObservable<double> milliSeconds, IObservable<double> position, Ease ease) =>
            TopMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(IObservable<double> milliSeconds, IObservable<double> position, IObservable<Ease> ease) =>
            TopMarginMove(element, milliSeconds, position, ease, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(double milliSeconds, double position) =>
            TopMarginMove(element, milliSeconds, position, Ease.None, null)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TopMarginMove(double milliSeconds, double position, Ease ease) =>
            TopMarginMove(element, milliSeconds, position, ease, null)
;
    }

/// <summary>Provides reactive RotateTransform extensions.</summary>
    /// <param name = "element">The element to animate.</param>
    extension(Visual element)
    {
        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(IObservable<double> milliSeconds, IObservable<double> angle, Ease ease) =>
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(angle, (ms, a) => (ms, a))
                    .Select(v => RotateTransform(element, v.ms, v.a, ease))
                    .Switch());

        /// <summary>Delegates to the matching animation helper.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <param name="ease">The ease value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(IObservable<double> milliSeconds, IObservable<double> angle, IObservable<Ease> ease) =>
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(angle, ease, (ms, a, e) => (ms, a, e))
                    .Select(v => RotateTransform(element, v.ms, v.a, v.e))
                    .Switch());

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="angle">The angle value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> RotateTransform(IObservable<double> milliSeconds, IObservable<double> angle) =>
            RotateTransform(element, milliSeconds, angle, Ease.None)
;

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
            Observable.Defer(() =>
                milliSeconds
                    .CombineLatest(position, (ms, p) => (ms, p))
                    .Select(v => TranslateTransform(element, v.ms, v.p.X, v.p.Y, horizontalEase, verticalEase))
                    .Switch());

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(IObservable<double> milliSeconds, IObservable<Point> position) =>
            TranslateTransform(element, milliSeconds, position, Ease.None, Ease.None)
;

        /// <summary>Calls the overload with default arguments.</summary>
        /// <param name="milliSeconds">The milliSeconds value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="horizontalEase">The horizontalEase value.</param>
        /// <returns>The resulting observable.</returns>
        public IObservable<Unit> TranslateTransform(IObservable<double> milliSeconds, IObservable<Point> position, Ease horizontalEase) =>
            TranslateTransform(element, milliSeconds, position, horizontalEase, Ease.None)
;
    }
}
