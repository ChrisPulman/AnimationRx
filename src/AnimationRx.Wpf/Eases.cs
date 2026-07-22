// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>Provides static easing facade methods.</summary>
public static class Eases
{
    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BackIn(IObservable<Duration> source) => source.BackIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BackInOut(IObservable<Duration> source) => source.BackInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BackOut(IObservable<Duration> source) => source.BackOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BounceIn(IObservable<Duration> source) => source.BounceIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BounceInOut(IObservable<Duration> source) => source.BounceInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> BounceOut(IObservable<Duration> source) => source.BounceOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CircIn(IObservable<Duration> source) => source.CircIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CircInOut(IObservable<Duration> source) => source.CircInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CircOut(IObservable<Duration> source) => source.CircOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CubicIn(IObservable<Duration> source) => source.CubicIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CubicInOut(IObservable<Duration> source) => source.CubicInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> CubicOut(IObservable<Duration> source) => source.CubicOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source">The source value.</param>
    /// <param name="ease)">The ease) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> EaseAnimation(
        IObservable<Duration> source,
        Ease ease) =>
        source.EaseAnimation(ease);

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ElasticIn(IObservable<Duration> source) => source.ElasticIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ElasticInOut(IObservable<Duration> source) => source.ElasticInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ElasticOut(IObservable<Duration> source) => source.ElasticOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ExpoIn(IObservable<Duration> source) => source.ExpoIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ExpoInOut(IObservable<Duration> source) => source.ExpoInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> ExpoOut(IObservable<Duration> source) => source.ExpoOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuadIn(IObservable<Duration> source) => source.QuadIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuadInOut(IObservable<Duration> source) => source.QuadInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuadOut(IObservable<Duration> source) => source.QuadOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuarticIn(IObservable<Duration> source) => source.QuarticIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuarticInOut(IObservable<Duration> source) => source.QuarticInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuarticOut(IObservable<Duration> source) => source.QuarticOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuinticIn(IObservable<Duration> source) => source.QuinticIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuinticInOut(IObservable<Duration> source) => source.QuinticInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> QuinticOut(IObservable<Duration> source) => source.QuinticOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> SineIn(IObservable<Duration> source) => source.SineIn();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> SineInOut(IObservable<Duration> source) => source.SineInOut();

    /// <summary>Delegates to the matching animation helper.</summary>
    /// <param name="source)">The source) value.</param>
    /// <returns>The resulting observable.</returns>
    public static IObservable<Duration> SineOut(IObservable<Duration> source) => source.SineOut();
}
