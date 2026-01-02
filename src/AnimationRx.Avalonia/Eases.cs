// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CP.AnimationRx;

/// <summary>
/// Eases.
/// </summary>
public static class Eases
{
    /// <summary>
    /// Backs animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BackIn(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                var t = d.Percent;
                const double s = 1.70158;
                return Duration.Create(t * t * (((s + 1) * t) - s));
            }));

    /// <summary>
    /// Backs animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BackInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                const double s = 1.70158 * 1.525;
                var t = d.Percent;

                return (t *= 2) < 1
                    ? Duration.Create(0.5 * (t * t * (((s + 1) * t) - s)))
                    : Duration.Create(0.5 * (((t -= 2) * t * (((s + 1) * t) + s)) + 2));
            }));

    /// <summary>
    /// Backs animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BackOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                var t = d.Percent;
                const double s = 1.70158;
                return Duration.Create((--t * t * (((s + 1) * t) + s)) + 1);
            }));

    /// <summary>
    /// Bounces animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BounceIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => 1.0 - t.Percent).ToDuration().BounceOut().Select(t => 1.0 - t.Percent).ToDuration());

    /// <summary>
    /// Bounces animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BounceInOut(this IObservable<Duration> @this) =>
        Observable.Create<Duration>(obs =>
            new CompositeDisposable
            {
                @this.Select(t => t.Percent).Where(t => t < 0.5).Select(t => 1.0 - (t * 2.0)).ToDuration().BounceOut().Select(t => 0.5 * (1.0 - t.Percent)).ToDuration().Subscribe(t => obs.OnNext(t)),
                @this.Select(t => t.Percent).Where(t => t >= 0.5).Select(t => (t * 2.0) - 1.0).ToDuration().BounceOut().Select(t => (0.5 * t.Percent) + 0.5).ToDuration().Subscribe(t => obs.OnNext(t)),
                @this.Select(t => t.Percent).Where(t => t == 1).Subscribe(_ => obs.OnCompleted())
            });

    /// <summary>
    /// Bounces animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> BounceOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                var t = d.Percent;
                const double a = 4.0 / 11.0;
                const double b = 8.0 / 11.0;
                const double c = 9.0 / 10.0;

                const double ca = 4356.0 / 361.0;
                const double cb = 35442.0 / 1805.0;
                const double cc = 16061.0 / 1805.0;

                var t2 = t * t;

                return Duration.Create(t < a
                    ? 7.5625 * t2
                    : t < b
                        ? (9.075 * t2) - (9.9 * t) + 3.4
                        : t < c
                            ? (ca * t2) - (cb * t) + cc
                            : (10.8 * t * t) - (20.52 * t) + 10.72);
            }));

    /// <summary>
    /// Circles animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CircIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(1.0 - Math.Sqrt(1.0 - (t.Percent * t.Percent)))));

    /// <summary>
    /// Circles animation to animate the object in out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CircInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                var t = d.Percent;
                return (t *= 2) < 1
                    ? Duration.Create(-0.5 * (Math.Sqrt(1 - (t * t)) - 1))
                    : Duration.Create(0.5 * (Math.Sqrt(1 - ((t -= 2) * t)) + 1));
            }));

    /// <summary>
    /// Circles animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CircOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(Math.Sqrt(1 - (--t.Percent * t.Percent)))));

    /// <summary>
    /// Cubics animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CubicIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent * t.Percent * t.Percent)));

    /// <summary>
    /// Cubics animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CubicInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent < 0.5 ? 4.0 * t.Percent * t.Percent * t.Percent : (0.5 * Math.Pow((2.0 * t.Percent) - 2.0, 3.0)) + 1.0)));

    /// <summary>
    /// Cubics animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> CubicOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(d =>
            {
                var t = d.Percent;
                var f = t - 1.0;
                return Duration.Create((f * f * f) + 1.0);
            }));

    /// <summary>
    /// Selects the requested ease for the duration stream.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <param name="ease">The ease.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> EaseAnimation(this IObservable<Duration> @this, Ease ease) =>
        Observable.Defer(() =>
            ease switch
            {
                Ease.BackIn => @this.BackIn(),
                Ease.BackInOut => @this.BackInOut(),
                Ease.BackOut => @this.BackOut(),
                Ease.BounceIn => @this.BounceIn(),
                Ease.BounceInOut => @this.BounceInOut(),
                Ease.BounceOut => @this.BounceOut(),
                Ease.CircIn => @this.CircIn(),
                Ease.CircInOut => @this.CircInOut(),
                Ease.CircOut => @this.CircOut(),
                Ease.CubicIn => @this.CubicIn(),
                Ease.CubicInOut => @this.CubicInOut(),
                Ease.CubicOut => @this.CubicOut(),
                Ease.ElasticIn => @this.ElasticIn(),
                Ease.ElasticInOut => @this.ElasticInOut(),
                Ease.ElasticOut => @this.ElasticOut(),
                Ease.ExpoIn => @this.ExpoIn(),
                Ease.ExpoInOut => @this.ExpoInOut(),
                Ease.ExpoOut => @this.ExpoOut(),
                Ease.QuinticIn => @this.QuinticIn(),
                Ease.QuinticInOut => @this.QuinticInOut(),
                Ease.QuinticOut => @this.QuinticOut(),
                Ease.QuadIn => @this.QuadIn(),
                Ease.QuadInOut => @this.QuadInOut(),
                Ease.QuadOut => @this.QuadOut(),
                Ease.QuarticIn => @this.QuarticIn(),
                Ease.QuarticInOut => @this.QuarticInOut(),
                Ease.QuarticOut => @this.QuarticOut(),
                Ease.SineIn => @this.SineIn(),
                Ease.SineInOut => @this.SineInOut(),
                Ease.SineOut => @this.SineOut(),
                _ => @this,
            });

    /// <summary>
    /// Elastics animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ElasticIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(Math.Sin(13.0 * t.Percent * Math.PI / 2) * Math.Pow(2.0, 10.0 * (t.Percent - 1.0)))));

    /// <summary>
    /// Elastics animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ElasticInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(t =>
                Duration.Create(t.Percent < 0.5
                    ? 0.5 * Math.Sin(+13.0 * Math.PI / 2 * 2.0 * t.Percent) * Math.Pow(2.0, 10.0 * ((2.0 * t.Percent) - 1.0))
                    : (0.5 * Math.Sin(-13.0 * Math.PI / 2 * ((2.0 * t.Percent) - 1.0 + 1.0)) * Math.Pow(2.0, -10.0 * ((2.0 * t.Percent) - 1.0))) + 1.0)));

    /// <summary>
    /// Elastics animation to animate the out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ElasticOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create((Math.Sin(-13.0 * (t.Percent + 1.0) * Math.PI / 2) * Math.Pow(2.0, -10.0 * t.Percent)) + 1.0)));

    /// <summary>
    /// Expo animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ExpoIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent == 0.0 ? t.Percent : Math.Pow(2.0, 10.0 * (t.Percent - 1.0)))));

    /// <summary>
    /// Expo animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ExpoInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(t =>
                Duration.Create((t.Percent == 0.0 || t.Percent == 1.0)
                    ? t.Percent
                    : t.Percent < 0.5
                        ? +0.5 * Math.Pow(2.0, (20.0 * t.Percent) - 10.0)
                        : (-0.5 * Math.Pow(2.0, 10.0 - (t.Percent * 20.0))) + 1.0)));

    /// <summary>
    /// Expo animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ExpoOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent == 1.0 ? t.Percent : 1.0 - Math.Pow(2.0, -10.0 * t.Percent))));

    /// <summary>
    /// Quads animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuadIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent * t.Percent)));

    /// <summary>
    /// Quads animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuadInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(t =>
            {
                t.Percent /= 0.5;

                if (t.Percent < 1)
                {
                    return Duration.Create(0.5 * t.Percent * t.Percent);
                }

                t.Percent--;

                return Duration.Create(-0.5 * ((t.Percent * (t.Percent - 2)) - 1));
            }));

    /// <summary>
    /// Quads animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuadOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(-t.Percent * (t.Percent - 2.0))));

    /// <summary>
    /// Quartics animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuarticIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(Math.Pow(t.Percent, 4.0))));

    /// <summary>
    /// Quartics animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuarticInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent < 0.5 ? +8.0 * Math.Pow(t.Percent, 4.0) : (-8.0 * Math.Pow(t.Percent - 1.0, 4.0)) + 1.0)));

    /// <summary>
    /// Quartics animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuarticOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create((Math.Pow(t.Percent - 1.0, 3.0) * (1.0 - t.Percent)) + 1.0)));

    /// <summary>
    /// Quintic animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuinticIn(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(t.Percent * t.Percent * t.Percent * t.Percent * t.Percent)));

    /// <summary>
    /// Uses a Quintic animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuinticInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(t => (t.Percent *= 2) < 1
                ? Duration.Create(0.5 * t.Percent * t.Percent * t.Percent * t.Percent * t.Percent)
                : Duration.Create(0.5 * (((t.Percent -= 2) * t.Percent * t.Percent * t.Percent * t.Percent) + 2))));

    /// <summary>
    /// Quintic animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> QuinticOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create((--t.Percent * t.Percent * t.Percent * t.Percent * t.Percent) + 1)));

    /// <summary>
    /// Uses a Sin animation to animate the object in.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> SineIn(this IObservable<Duration> @this) =>
        Observable.Defer(() =>
            @this.Select(t =>
            {
                var v = Math.Cos(t.Percent * Math.PI * 0.5);
                return Math.Abs(v) < 1e-14 ? Duration.Create(1) : Duration.Create(1 - v);
            }));

    /// <summary>
    /// Uses a Sin animation to animate the object in then out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> SineInOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(-0.5 * (Math.Cos(Math.PI * t.Percent) - 1))));

    /// <summary>
    /// Uses a Sin animation to animate the object out.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> SineOut(this IObservable<Duration> @this) =>
        Observable.Defer(() => @this.Select(t => Duration.Create(Math.Sin(t.Percent * Math.PI / 2))));

    /// <summary>
    /// Maps a raw double (0..1) to a <see cref="Duration" />.
    /// </summary>
    /// <param name="this">The this.</param>
    /// <returns>A duration observable.</returns>
    public static IObservable<Duration> ToDuration(this IObservable<double> @this) =>
        Observable.Defer(() => @this.Select(Duration.Create));
}
