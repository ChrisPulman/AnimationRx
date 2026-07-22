// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;

namespace CP.AnimationRx;

/// <summary>Provides WPF animation composition and effect helpers.</summary>
public static partial class Animations
{
    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="from">The from value.</param>
    /// <param name="to">The to value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toX">The toX value.</param>
    /// <param name="toY">The toY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> TranslateTo(
        FrameworkElement element,
        double milliSeconds,
        double toX,
        double toY,
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
                    var fromX = tr.X;
                    var fromY = tr.Y;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var horizontalMove = anim.EaseAnimation(easeX).Distance(toX - fromX);
                    var verticalMove = anim.EaseAnimation(easeY).Distance(toY - fromY);

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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toScaleX">The toScaleX value.</param>
    /// <param name="toScaleY">The toScaleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ScaleTo(
        FrameworkElement element,
        double milliSeconds,
        double toScaleX,
        double toScaleY,
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
                    var fromX = tr.ScaleX;
                    var fromY = tr.ScaleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var horizontalMove = anim.EaseAnimation(easeX).Distance(toScaleX - fromX);
                    var verticalMove = anim.EaseAnimation(easeY).Distance(toScaleY - fromY);

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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngle">The toAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngle,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new RotateTransform(
                            DefaultAngle,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));
                    var from = tr.Angle;

                    return AnimateValue(milliSeconds, from, toAngle, ease, scheduler)
                        .ObserveOn(GetUiScheduler())
                        .Do(v => tr.Angle = v)
                        .Select(_ => Unit.Default);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="deltaAngle">The deltaAngle value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> RotateBy(
        FrameworkElement element,
        double milliSeconds,
        double deltaAngle,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(
                () =>
                {
                    var tr = GetOrAddTransform(
                        element,
                        () => new RotateTransform(
                            DefaultAngle,
                            element.ActualWidth / TransformCenterDivisor,
                            element.ActualHeight / TransformCenterDivisor));
                    return RotateTo(element, milliSeconds, tr.Angle + deltaAngle, ease, scheduler);
                },
                GetUiScheduler()).SelectMany(x => x));

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="toAngleX">The toAngleX value.</param>
    /// <param name="toAngleY">The toAngleY value.</param>
    /// <param name="easeX">The easeX value.</param>
    /// <param name="easeY">The easeY value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> SkewTo(
        FrameworkElement element,
        double milliSeconds,
        double toAngleX,
        double toAngleY,
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
                    var fromX = tr.AngleX;
                    var fromY = tr.AngleY;

                    var anim = DurationPercentage(milliSeconds, scheduler);
                    var horizontalMove = anim.EaseAnimation(easeX).Distance(toAngleX - fromX);
                    var verticalMove = anim.EaseAnimation(easeY).Distance(toAngleY - fromY);

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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSeconds">The milliSeconds value.</param>
    /// <param name="amplitude">The amplitude value.</param>
    /// <param name="shakes">The shakes value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> ShakeTranslate(
        FrameworkElement element,
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
                    var tr = GetOrAddTransform(
                        element,
                        () => new TranslateTransform(
                            element.RenderTransform.Value.OffsetX,
                            element.RenderTransform.Value.OffsetY));
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

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="element">The element value.</param>
    /// <param name="milliSecondsPerHalf">The milliSecondsPerHalf value.</param>
    /// <param name="low">The low value.</param>
    /// <param name="high">The high value.</param>
    /// <param name="pulses">The pulses value.</param>
    /// <param name="ease">The ease value.</param>
    /// <param name="scheduler">The scheduler value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Unit> PulseOpacity(
        UIElement element,
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
            element.OpacityTo(milliSecondsPerHalf, high, ease, scheduler)
        }.Sequence();

        return seq.RepeatAnimation(pulses);
    }
}
