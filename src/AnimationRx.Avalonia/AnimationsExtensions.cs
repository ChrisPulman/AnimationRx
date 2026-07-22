// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace CP.AnimationRx;

/// <summary>Provides Avalonia animation timing and core extension helpers.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Defines the default frames per second.</summary>
    private const double DefaultFramesPerSecond = 60.0;

    /// <summary>Defines the milliseconds per second.</summary>
    private const double MillisecondsPerSecond = 1000.0;

    /// <summary>Defines the default frame interval in milliseconds.</summary>
    private const double DefaultFrameIntervalMilliseconds = 16.0;

    /// <summary>Defines the full-circle radians multiplier.</summary>
    private const double FullCircleRadiansMultiplier = 2.0;

    /// <summary>Animates the frame using an interval based on frames-per-second.</summary>
    /// <param name="framesPerSecond">The frames per second.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable that ticks every frame.</returns>
    public static IObservable<long> AnimateFrame(double framesPerSecond, IScheduler? scheduler) =>
        Observable.Defer(() =>
        {
            var fps = framesPerSecond <= 0 ? DefaultFramesPerSecond : framesPerSecond;
            var period = TimeSpan.FromMilliseconds(MillisecondsPerSecond / fps);
            return Observable.Interval(period, scheduler ?? GetBackgroundScheduler());
        });

    /// <summary>Convenience overload to drive frames by a fixed period.</summary>
    /// <param name="period">The frame period.</param>
    /// <param name="scheduler">Optional scheduler.</param>
    /// <returns>An observable sequence of frame indices emitted on the given period.</returns>
    public static IObservable<long> AnimateFrame(TimeSpan period, IScheduler? scheduler) =>
        Observable.Interval(period, scheduler ?? GetBackgroundScheduler());

    /// <summary>Produces a UI-scheduled frame stream using the default animation cadence.</summary>
    /// <returns>An observable sequence of frame indices.</returns>
    public static IObservable<long> RenderFrames() =>
        AnimateFrame(DefaultFramesPerSecond, GetUiScheduler());

    /// <summary>Milliseconds elapsed.</summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits elapsed milliseconds since subscription.</returns>
    public static IObservable<double> MilliSecondsElapsed(IScheduler scheduler) =>
        Observable.Defer(() =>
        {
            var start = scheduler.Now;
            return Observable.Interval(TimeSpan.FromMilliseconds(DefaultFrameIntervalMilliseconds), scheduler)
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
    public static IObservable<Duration> DurationPercentage(IObservable<double> milliSeconds, IScheduler? scheduler) =>
        Observable.Defer(() => milliSeconds
            .Select(ms => DurationPercentage(ms, scheduler))
            .Switch());

    /// <summary>
    /// Produces a percentage of the duration for a fixed time span in milliseconds.
    /// Always emits a final 1.0 tick and then completes.
    /// </summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits duration percentages.</returns>
    public static IObservable<Duration> DurationPercentage(double milliSeconds, IScheduler? scheduler) =>
        Observable.Defer(() => milliSeconds <= 0
            ? Observable.Return(Duration.Create(1))
            : MilliSecondsElapsed(scheduler ?? GetBackgroundScheduler())
                .Select(ems => ems / milliSeconds)
                .ToDuration()
                .TakeWhile(t => t.Percent < 1)
                .Concat(Observable.Return(Duration.Create(1))));

    /// <summary>Helper to create a value animation from a start value to an end value using easing.</summary>
    /// <param name="milliSeconds">The milli seconds.</param>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="ease">The ease.</param>
    /// <param name="scheduler">The scheduler.</param>
    /// <returns>An observable that emits animated values.</returns>
    public static IObservable<double> AnimateValue(
        double milliSeconds,
        double from,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            DurationPercentage(milliSeconds, scheduler)
                .EaseAnimation(ease)
                .Distance(to - from)
                .Select(delta => from + delta));

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
        public IObservable<Unit> WidthTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() =>
                Observable.Start(
                    () => double.IsNaN(element.Width) ? element.Bounds.Width : element.Width,
                    GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Width = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Animates height to a target value.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> HeightTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() =>
                Observable.Start(
                    () => double.IsNaN(element.Height) ? element.Bounds.Height : element.Height,
                    GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Height = v)
                        .Select(_ => Unit.Default)));

        /// <summary>Animates margin to a target thickness.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> MarginTo(double milliSeconds, Thickness to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => element.Margin, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => element.Margin = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Left attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasLeftTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetLeft(element), GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetLeft(element, v))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Top attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasTopTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetTop(element), GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetTop(element, v))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Right attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasRightTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetRight(element), GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetRight(element, v))
                        .Select(_ => Unit.Default)));

        /// <summary>Animates Canvas.Bottom attached property.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> CanvasBottomTo(double milliSeconds, double to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => Canvas.GetBottom(element), GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetBottom(element, v))
                        .Select(_ => Unit.Default)));
    }

    /// <summary>Provides composition extension methods for animation sequences.</summary>
    /// <param name="animations">The animations to compose.</param>
    extension(IEnumerable<IObservable<Unit>> animations)
    {
        /// <summary>Sequences animations in order (Concat) and completes when the last completes.</summary>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> Sequence()
        {
            if (animations is null)
            {
                throw new ArgumentNullException(nameof(animations));
            }

            var sequence = Observable.Return(Unit.Default);
            foreach (var animation in animations)
            {
                sequence = sequence.Concat(animation);
            }

            return sequence;
        }

        /// <summary>Runs animations in parallel and completes when all complete.</summary>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> Parallel()
        {
            if (animations is null)
            {
                throw new ArgumentNullException(nameof(animations));
            }

            return animations.Merge().LastOrDefaultAsync().ToObservable();
        }

        /// <summary>Adds a delay between each animation in a sequence.</summary>
        /// <param name="delay">The delay.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when all animations complete.</returns>
        public IObservable<Unit> DelayBetween(TimeSpan delay, IScheduler? scheduler)
        {
            if (animations is null)
            {
                throw new ArgumentNullException(nameof(animations));
            }

            var sched = scheduler ?? GetBackgroundScheduler();
            var sequence = Observable.Return(Unit.Default);
            foreach (var animation in animations)
            {
                sequence = sequence
                    .Concat(Observable.Timer(delay, sched).Select(_ => Unit.Default))
                    .Concat(animation);
            }

            return sequence;
        }

        /// <summary>Applies a stagger to a set of animations (delay incrementally).</summary>
        /// <param name="staggerBy">The stagger by.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An enumerable of staggered animations.</returns>
        /// <exception cref="ArgumentNullException">animations.</exception>
        public IEnumerable<IObservable<Unit>> Stagger(TimeSpan staggerBy, IScheduler? scheduler)
        {
            if (animations is null)
            {
                throw new ArgumentNullException(nameof(animations));
            }

            return StaggerIterator(staggerBy, scheduler);

            IEnumerable<IObservable<Unit>> StaggerIterator(TimeSpan staggerBy, IScheduler? scheduler)
            {
                var sched = scheduler ?? GetBackgroundScheduler();
                var staggerDelay = TimeSpan.Zero;
                foreach (var animation in animations)
                {
                    var start = Observable.Timer(staggerDelay, sched).Select(_ => Unit.Default);
                    yield return start.Concat(animation);
                    staggerDelay += staggerBy;
                }
            }
        }
    }

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

    /// <summary>Provides throttling extension methods for observable sequences.</summary>
    /// <typeparam name="T">The observable value type.</typeparam>
    /// <param name="this">The source observable.</param>
    extension<T>(IObservable<T> @this)
    {
        /// <summary>Takes one value of T every interval.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that emits one value every interval.</returns>
        public IObservable<T> TakeOneEvery(TimeSpan interval, IScheduler? scheduler) =>
            Observable.Defer(() =>
            {
                var sched = scheduler ?? GetBackgroundScheduler();
                return @this.Select(v => Observable.Return(v).Delay(interval, sched)).Concat();
            });
    }

    /// <summary>Provides repeat extension methods for unit animation observables.</summary>
    /// <param name="animation">The animation to repeat.</param>
    extension(IObservable<Unit> animation)
    {
        /// <summary>Repeats an animation a specific number of times.</summary>
        /// <param name="count">The count.</param>
        /// <returns>
        /// An observable that completes when the animation has repeated the specified number of times.
        /// </returns>
        public IObservable<Unit> RepeatAnimation(int count) =>
            animation.Repeat(count);

        /// <summary>Repeats an animation forever.</summary>
        /// <returns>
        /// An observable that repeats the animation indefinitely until unsubscribed.
        /// </returns>
        public IObservable<Unit> RepeatAnimation() =>
            animation.Repeat();
    }

    /// <summary>Provides elapsed-time projection extension methods.</summary>
    /// <param name="source">The source stream.</param>
    extension(IObservable<double> source)
    {
        /// <summary>Converts elapsed milliseconds to pixels moved at the supplied velocity.</summary>
        /// <param name="velocity">The velocity in pixels per second.</param>
        /// <returns>An observable that emits pixel distances.</returns>
        public IObservable<double> PixelsPerSecond(double velocity) =>
            Observable.Defer(() => source.Select(ms => velocity * ms / MillisecondsPerSecond));
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
        public IObservable<Unit> BrushColorTo(double milliSeconds, Color to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => brush.Color, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => brush.Color = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

        /// <summary>Alias for the brush color animation.</summary>
        /// <param name="milliSeconds">The milli seconds.</param>
        /// <param name="to">To.</param>
        /// <param name="ease">The ease.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>An observable that completes when the animation is done.</returns>
        public IObservable<Unit> ColorTo(double milliSeconds, Color to, Ease ease, IScheduler? scheduler) =>
            brush.BrushColorTo(milliSeconds, to, ease, scheduler);
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
        public IObservable<Unit> PaddingTo(double milliSeconds, Thickness to, Ease ease, IScheduler? scheduler) =>
            Observable.Defer(() => Observable.Start(() => element.Padding, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => element.Padding = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));
    }
}
