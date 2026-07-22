// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CP.AnimationRx;

/// <summary>Provides WPF transform and layout animation helpers.</summary>
public static partial class Animations
{
    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="position">The position value.</param>
    /// <param name="horizontalEase">The horizontal easing value.</param>
    /// <param name="verticalEase">The vertical easing value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        IObservable<double> milliSeconds,
        IObservable<Point> position,
        Ease horizontalEase,
        Ease verticalEase) =>
        Observable.Defer(() =>
            milliSeconds
                .CombineLatest(position, (ms, p) => (ms, p))
                .Select(v => TranslateTransform(element, v.ms, v.p.X, v.p.Y, horizontalEase, verticalEase))
                .Switch());

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="horizontalPosition">The horizontal position.</param>
    /// <param name="verticalPosition">The vertical position.</param>
    /// <param name="horizontalEase">The horizontal easing value.</param>
    /// <param name="verticalEase">The vertical easing value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTransform(
        FrameworkElement element,
        double milliSeconds,
        double horizontalPosition,
        double verticalPosition,
        Ease horizontalEase,
        Ease verticalEase,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var translateTransform = GetOrAddTransform(
                            element,
                            () => new TranslateTransform(
                                element.RenderTransform.Value.OffsetX,
                                element.RenderTransform.Value.OffsetY));

                        var initialHorizontalPosition = translateTransform.X;
                        var initialVerticalPosition = translateTransform.Y;

                        var anim = DurationPercentage(milliSeconds, scheduler);
                        var horizontalMove = anim
                            .EaseAnimation(horizontalEase)
                            .Distance(horizontalPosition - initialHorizontalPosition);
                        var verticalMove = anim
                            .EaseAnimation(verticalEase)
                            .Distance(verticalPosition - initialVerticalPosition);

                        return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                        .ObserveOn(GetUiScheduler())
                        .Do(t =>
                        {
                            translateTransform.X = initialHorizontalPosition + t.X;
                            translateTransform.Y = initialVerticalPosition + t.Y;
                        })
                        .Select(_ => Unit.Default);
                    },
                    GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaX">The deltaX value.</param>
    /// <param name="deltaY">The deltaY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaX,
        double deltaY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                    () =>
                    {
                        var tr = GetOrAddTransform(
                            element,
                            () => new TranslateTransform(
                                element.RenderTransform.Value.OffsetX,
                                element.RenderTransform.Value.OffsetY));

                        return TranslateTransform(
                            element,
                            milliSeconds,
                            tr.X + deltaX,
                            tr.Y + deltaY,
                            easeX,
                            easeY,
                            scheduler);
                    },
                    GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> OpacityTo(
        UIElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Opacity, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Opacity = v)
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeIn(UIElement element, double milliSeconds, Ease ease, IScheduler? scheduler) =>
        OpacityTo(element, milliSeconds, 1, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> FadeOut(UIElement element, double milliSeconds, Ease ease, IScheduler? scheduler) =>
        OpacityTo(element, milliSeconds, 0, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> WidthTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(element.Width) ? element.ActualWidth : element.Width,
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Width = v)
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> HeightTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(element.Height) ? element.ActualHeight : element.Height,
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => element.Height = v)
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> MarginTo(
        FrameworkElement element,
        double milliSeconds,
        Thickness to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Margin, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => element.Margin = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PaddingTo(
        Control element,
        double milliSeconds,
        Thickness to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Padding, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => element.Padding = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasLeftTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(Canvas.GetLeft(element)) ? 0 : Canvas.GetLeft(element),
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetLeft(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasTopTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(Canvas.GetTop(element)) ? 0 : Canvas.GetTop(element),
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetTop(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasRightTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(Canvas.GetRight(element)) ? 0 : Canvas.GetRight(element),
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetRight(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> CanvasBottomTo(
        FrameworkElement element,
        double milliSeconds,
        double to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() =>
            Observable.Start(
                () => double.IsNaN(Canvas.GetBottom(element)) ? 0 : Canvas.GetBottom(element),
                GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, from, to, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => Canvas.SetBottom(element, v))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> BrushColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => brush.Color, GetUiScheduler())
                .SelectMany(from =>
                    AnimateValue(milliSeconds, 0, 1, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(p => brush.Color = Lerp(from, to, p))
                        .Select(_ => Unit.Default)));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="brush">The brush value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ColorTo(
        SolidColorBrush brush,
        double milliSeconds,
        Color to,
        Ease ease,
        IScheduler? scheduler) =>
        BrushColorTo(brush, milliSeconds, to, ease, scheduler);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="scaleX">The scaleX value.</param>
    /// <param name="scaleY">The scaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTransform(
        FrameworkElement element,
        double milliSeconds,
        double scaleX,
        double scaleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new ScaleTransform(
                            IdentityScale,
                            IdentityScale,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));
                    var sx = tr.ScaleX;
                    var sy = tr.ScaleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var horizontalMove = anim.EaseAnimation(easeX).Distance(scaleX - sx);
                    var verticalMove = anim.EaseAnimation(easeY).Distance(scaleY - sy);

                    return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                        .ObserveOn(GetUiScheduler())
                        .Do(p =>
                        {
                            tr.ScaleX = sx + p.X;
                            tr.ScaleY = sy + p.Y;
                        })
                        .Select(_ => Unit.Default);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaScaleX">The deltaScaleX value.</param>
    /// <param name="deltaScaleY">The deltaScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaScaleX,
        double deltaScaleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new ScaleTransform(
                            IdentityScale,
                            IdentityScale,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));

                    return ScaleTransform(
                        element,
                        milliSeconds,
                        tr.ScaleX + deltaScaleX,
                        tr.ScaleY + deltaScaleY,
                        easeX,
                        easeY,
                        scheduler);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="angleX">The angleX value.</param>
    /// <param name="angleY">The angleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTransform(
        FrameworkElement element,
        double milliSeconds,
        double angleX,
        double angleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new SkewTransform(
                            DefaultAngle,
                            DefaultAngle,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));
                    var ax = tr.AngleX;
                    var ay = tr.AngleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var horizontalMove = anim.EaseAnimation(easeX).Distance(angleX - ax);
                    var verticalMove = anim.EaseAnimation(easeY).Distance(angleY - ay);

                    return horizontalMove.CombineLatest(verticalMove, (x, y) => new Point(x, y))
                        .ObserveOn(GetUiScheduler())
                        .Do(p =>
                        {
                            tr.AngleX = ax + p.X;
                            tr.AngleY = ay + p.Y;
                        })
                        .Select(_ => Unit.Default);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngleX">The deltaAngleX value.</param>
    /// <param name="deltaAngleY">The deltaAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngleX,
        double deltaAngleY,
        Ease easeX,
        Ease easeY,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new SkewTransform(
                            DefaultAngle,
                            DefaultAngle,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));

                    return SkewTransform(
                        element,
                        milliSeconds,
                        tr.AngleX + deltaAngleX,
                        tr.AngleY + deltaAngleY,
                        easeX,
                        easeY,
                        scheduler);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> Sequence(IEnumerable<IObservable<Unit>> animations)
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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> Parallel(IEnumerable<IObservable<Unit>> animations)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        return animations.Merge().LastOrDefaultAsync().ToObservable();
    }

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="delay">The delay value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> DelayBetween(
        IEnumerable<IObservable<Unit>> animations,
        TimeSpan delay,
        IScheduler? scheduler)
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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animation">The animation value.</param>
    /// <param name="count">The count value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RepeatAnimation(IObservable<Unit> animation, int count) =>
        animation.Repeat(count);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="animations">The animations value.</param>
    /// <param name="staggerBy">The staggerBy value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IEnumerable<IObservable<Unit>> Stagger(
        IEnumerable<IObservable<Unit>> animations,
        TimeSpan staggerBy,
        IScheduler? scheduler)
    {
        if (animations is null)
        {
            throw new ArgumentNullException(nameof(animations));
        }

        var sched = scheduler ?? GetBackgroundScheduler();
        var delay = TimeSpan.Zero;
        var staggered = new List<IObservable<Unit>>();
        foreach (var anim in animations)
        {
            var start = Observable.Timer(delay, sched).Select(_ => Unit.Default);
            staggered.Add(start.Concat(anim));
            delay += staggerBy;
        }

        return staggered;
    }
}
