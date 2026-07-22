// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>Provides extension methods for easing animations using observables.</summary>
public static class EasesExtensions
{
    /// <summary>Defines the Zero value.</summary>
    private const double Zero = 0.0;

    /// <summary>Defines the One value.</summary>
    private const double One = 1.0;

    /// <summary>Defines the Two value.</summary>
    private const double Two = 2.0;

    /// <summary>Defines the Half value.</summary>
    private const double Half = 0.5;

    /// <summary>Defines the BackOvershoot value.</summary>
    private const double BackOvershoot = 1.70158;

    /// <summary>Defines the BackInOutOvershootScale value.</summary>
    private const double BackInOutOvershootScale = 1.525;

    /// <summary>Defines the BounceFirstThreshold value.</summary>
    private const double BounceFirstThreshold = 4.0 / 11.0;

    /// <summary>Defines the BounceSecondThreshold value.</summary>
    private const double BounceSecondThreshold = 8.0 / 11.0;

    /// <summary>Defines the BounceThirdThreshold value.</summary>
    private const double BounceThirdThreshold = 9.0 / 10.0;

    /// <summary>Defines the BounceFirstCoefficient value.</summary>
    private const double BounceFirstCoefficient = 7.5625;

    /// <summary>Defines the BounceSecondQuadratic value.</summary>
    private const double BounceSecondQuadratic = 9.075;

    /// <summary>Defines the BounceSecondLinear value.</summary>
    private const double BounceSecondLinear = 9.9;

    /// <summary>Defines the BounceSecondOffset value.</summary>
    private const double BounceSecondOffset = 3.4;

    /// <summary>Defines the BounceThirdQuadratic value.</summary>
    private const double BounceThirdQuadratic = 4356.0 / 361.0;

    /// <summary>Defines the BounceThirdLinear value.</summary>
    private const double BounceThirdLinear = 35442.0 / 1805.0;

    /// <summary>Defines the BounceThirdOffset value.</summary>
    private const double BounceThirdOffset = 16061.0 / 1805.0;

    /// <summary>Defines the BounceFourthQuadratic value.</summary>
    private const double BounceFourthQuadratic = 10.8;

    /// <summary>Defines the BounceFourthLinear value.</summary>
    private const double BounceFourthLinear = 20.52;

    /// <summary>Defines the BounceFourthOffset value.</summary>
    private const double BounceFourthOffset = 10.72;

    /// <summary>Defines the ElasticOscillations value.</summary>
    private const double ElasticOscillations = 13.0;

    /// <summary>Defines the ElasticExponent value.</summary>
    private const double ElasticExponent = 10.0;

    /// <summary>Defines the CubicInOutFactor value.</summary>
    private const double CubicInOutFactor = 4.0;

    /// <summary>Defines the QuarticInOutFactor value.</summary>
    private const double QuarticInOutFactor = 8.0;

    /// <summary>Defines the ExpoInOutExponent value.</summary>
    private const double ExpoInOutExponent = 20.0;

    /// <summary>Defines the SineTolerance value.</summary>
    private const double SineTolerance = 1e-14;

    /// <summary>Defines the ComparisonTolerance value.</summary>
    private const double ComparisonTolerance = 1e-12;

    /// <summary>Defines the SingleCompletionSignal value.</summary>
    private const int SingleCompletionSignal = 1;

    /// <summary>Gets the half-pi value.</summary>
    private static double HalfPi => Math.PI / Two;

    /// <summary>Calculates the EaseWith value.</summary>
    /// <param name="source">The source value.</param>
    /// <param name="ease">The ease value.</param>
    /// <returns>The calculated value.</returns>
    private static IObservable<Duration> EaseWith(
        IObservable<Duration> source,
        Func<double, double> ease) =>
        Observable.Defer(() => source.Select(duration => Duration.Create(ease(duration.Percent))));

    /// <summary>Calculates the ToDurationStream value.</summary>
    /// <param name="source">The source value.</param>
    /// <returns>The calculated value.</returns>
    private static IObservable<Duration> ToDurationStream(IObservable<double> source) =>
        Observable.Defer(() => source.Select(Duration.Create));

    /// <summary>Calculates the Square value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static double Square(double value) => value * value;

    /// <summary>Calculates the Cube value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static double Cube(double value) => value * value * value;

    /// <summary>Calculates the FourthPower value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static double FourthPower(double value) => Square(Square(value));

    /// <summary>Calculates the FifthPower value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static double FifthPower(double value) => FourthPower(value) * value;

    /// <summary>Calculates the IsCloseTo value.</summary>
    /// <param name="value">The input value.</param>
    /// <param name="expected">The expected value.</param>
    /// <returns>The calculated value.</returns>
    private static bool IsCloseTo(double value, double expected) =>
        Math.Abs(value - expected) <= ComparisonTolerance;

    /// <summary>Calculates the IsCloseTo value.</summary>
    /// <param name="value">The input value.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="tolerance">The tolerance value.</param>
    /// <returns>The calculated value.</returns>
    private static bool IsCloseTo(double value, double expected, double tolerance) =>
        Math.Abs(value - expected) <= tolerance;

    /// <summary>Calculates the IsFirstHalf value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static bool IsFirstHalf(double value) => value < Half;

    /// <summary>Calculates the IsSecondHalf value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static bool IsSecondHalf(double value) => value >= Half;

    /// <summary>Calculates the IsOne value.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The calculated value.</returns>
    private static bool IsOne(double value) => IsCloseTo(value, One);

    /// <summary>Calculates the BackInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double BackInPercent(double percent) =>
        Square(percent) * (((BackOvershoot + One) * percent) - BackOvershoot);

    /// <summary>Calculates the BackInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double BackInOutPercent(double percent)
    {
        const double overshoot = BackOvershoot * BackInOutOvershootScale;
        var scaled = percent * Two;

        if (scaled < One)
        {
            return Half * Square(scaled) * (((overshoot + One) * scaled) - overshoot);
        }

        scaled -= Two;
        return Half * ((Square(scaled) * (((overshoot + One) * scaled) + overshoot)) + Two);
    }

    /// <summary>Calculates the BackOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double BackOutPercent(double percent)
    {
        var shifted = percent - One;
        return (Square(shifted) * (((BackOvershoot + One) * shifted) + BackOvershoot)) + One;
    }

    /// <summary>Calculates the BounceOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double BounceOutPercent(double percent)
    {
        var squared = Square(percent);
        if (percent < BounceFirstThreshold)
        {
            return BounceFirstCoefficient * squared;
        }

        if (percent < BounceSecondThreshold)
        {
            return (BounceSecondQuadratic * squared)
                - (BounceSecondLinear * percent)
                + BounceSecondOffset;
        }

        return percent < BounceThirdThreshold
            ? (BounceThirdQuadratic * squared)
                - (BounceThirdLinear * percent)
                + BounceThirdOffset
            : (BounceFourthQuadratic * squared)
                - (BounceFourthLinear * percent)
                + BounceFourthOffset;
    }

    /// <summary>Calculates the CircInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CircInPercent(double percent) =>
        One - Math.Sqrt(One - Square(percent));

    /// <summary>Calculates the CircInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CircInOutPercent(double percent)
    {
        var scaled = percent * Two;
        if (scaled < One)
        {
            return -Half * (Math.Sqrt(One - Square(scaled)) - One);
        }

        scaled -= Two;
        return Half * (Math.Sqrt(One - Square(scaled)) + One);
    }

    /// <summary>Calculates the CircOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CircOutPercent(double percent)
    {
        var shifted = percent - One;
        return Math.Sqrt(One - Square(shifted));
    }

    /// <summary>Calculates the CubicInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CubicInPercent(double percent) => Cube(percent);

    /// <summary>Calculates the CubicInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CubicInOutPercent(double percent) =>
        percent < Half
            ? CubicInOutFactor * Cube(percent)
            : (Half * Math.Pow((Two * percent) - Two, Two + One)) + One;

    /// <summary>Calculates the CubicOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double CubicOutPercent(double percent) =>
        Cube(percent - One) + One;

    /// <summary>Calculates the ElasticInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ElasticInPercent(double percent) =>
        Math.Sin(ElasticOscillations * percent * HalfPi)
        * Math.Pow(Two, ElasticExponent * (percent - One));

    /// <summary>Calculates the ElasticInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ElasticInOutPercent(double percent)
    {
        var scaled = Two * percent;
        return percent < Half
            ? Half
                * Math.Sin(ElasticOscillations * HalfPi * scaled)
                * Math.Pow(Two, ElasticExponent * (scaled - One))
            : (Half
                * Math.Sin(-ElasticOscillations * HalfPi * scaled)
                * Math.Pow(Two, -ElasticExponent * (scaled - One))) + One;
    }

    /// <summary>Calculates the ElasticOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ElasticOutPercent(double percent) =>
        (Math.Sin(-ElasticOscillations * (percent + One) * HalfPi)
            * Math.Pow(Two, -ElasticExponent * percent)) + One;

    /// <summary>Calculates the ExpoInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ExpoInPercent(double percent) =>
        IsCloseTo(percent, Zero)
            ? percent
            : Math.Pow(Two, ElasticExponent * (percent - One));

    /// <summary>Calculates the ExpoInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ExpoInOutPercent(double percent)
    {
        if (IsCloseTo(percent, Zero) || IsCloseTo(percent, One))
        {
            return percent;
        }

        return percent < Half
            ? Half * Math.Pow(Two, (ExpoInOutExponent * percent) - ElasticExponent)
            : (-Half * Math.Pow(Two, ElasticExponent - (percent * ExpoInOutExponent))) + One;
    }

    /// <summary>Calculates the ExpoOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double ExpoOutPercent(double percent) =>
        IsCloseTo(percent, One)
            ? percent
            : One - Math.Pow(Two, -ElasticExponent * percent);

    /// <summary>Calculates the QuadInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double QuadInOutPercent(double percent)
    {
        var scaled = percent / Half;
        if (scaled < One)
        {
            return Half * Square(scaled);
        }

        scaled -= One;
        return -Half * ((scaled * (scaled - Two)) - One);
    }

    /// <summary>Calculates the QuarticInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double QuarticInOutPercent(double percent) =>
        percent < Half
            ? QuarticInOutFactor * FourthPower(percent)
            : (-QuarticInOutFactor * FourthPower(percent - One)) + One;

    /// <summary>Calculates the QuarticOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double QuarticOutPercent(double percent) =>
        (Math.Pow(percent - One, Two + One) * (One - percent)) + One;

    /// <summary>Calculates the QuinticInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double QuinticInOutPercent(double percent)
    {
        var scaled = percent * Two;
        if (scaled < One)
        {
            return Half * FifthPower(scaled);
        }

        scaled -= Two;
        return Half * (FifthPower(scaled) + Two);
    }

    /// <summary>Calculates the QuinticOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double QuinticOutPercent(double percent) =>
        FifthPower(percent - One) + One;

    /// <summary>Calculates the SineInPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double SineInPercent(double percent)
    {
        var value = Math.Cos(percent * Math.PI * Half);
        return IsCloseTo(value, Zero, SineTolerance) ? One : One - value;
    }

    /// <summary>Calculates the SineInOutPercent value.</summary>
    /// <param name="percent">The percent value.</param>
    /// <returns>The calculated value.</returns>
    private static double SineInOutPercent(double percent) =>
        -Half * (Math.Cos(Math.PI * percent) - One);

    /// <summary>Provides easing extension methods for duration observables.</summary>
    /// <param name="this">The duration observable.</param>
    extension(IObservable<Duration> @this)
    {
        /// <summary>Backs animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BackIn() => EaseWith(@this, BackInPercent);

        /// <summary>Backs animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BackInOut() => EaseWith(@this, BackInOutPercent);

        /// <summary>Backs animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BackOut() => EaseWith(@this, BackOutPercent);

        /// <summary>Bounces animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BounceIn() =>
            Observable.Defer(() =>
            {
                var reversed = ToDurationStream(@this.Select(duration => One - duration.Percent));
                return ToDurationStream(reversed.BounceOut().Select(duration => One - duration.Percent));
            });

        /// <summary>Bounces animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BounceInOut() =>
            Observable.Create<Duration>(observer =>
                new CompositeDisposable
                {
                    ToDurationStream(@this.Select(duration => duration.Percent)
                            .Where(IsFirstHalf)
                            .Select(percent => One - (percent * Two)))
                        .BounceOut()
                        .Select(duration => Half * (One - duration.Percent))
                        .ToDuration()
                        .Subscribe(observer.OnNext),
                    ToDurationStream(@this.Select(duration => duration.Percent)
                            .Where(IsSecondHalf)
                            .Select(percent => (percent * Two) - One))
                        .BounceOut()
                        .Select(duration => (Half * duration.Percent) + Half)
                        .ToDuration()
                        .Subscribe(observer.OnNext),
                    @this.Select(duration => duration.Percent)
                        .Where(IsOne)
                        .Take(SingleCompletionSignal)
                        .Subscribe(_ => observer.OnCompleted()),
                });

        /// <summary>Bounces animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> BounceOut() => EaseWith(@this, BounceOutPercent);

        /// <summary>Circles animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CircIn() => EaseWith(@this, CircInPercent);

        /// <summary>Circles animation to animate the object in out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CircInOut() => EaseWith(@this, CircInOutPercent);

        /// <summary>Circles animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CircOut() => EaseWith(@this, CircOutPercent);

        /// <summary>Cubics animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CubicIn() => EaseWith(@this, CubicInPercent);

        /// <summary>Cubics animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CubicInOut() => EaseWith(@this, CubicInOutPercent);

        /// <summary>Cubics animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> CubicOut() => EaseWith(@this, CubicOutPercent);

        /// <summary>Selects the requested ease for the duration stream.</summary>
        /// <param name="ease">The ease.</param>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> EaseAnimation(Ease ease) =>
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

        /// <summary>Elastics animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ElasticIn() => EaseWith(@this, ElasticInPercent);

        /// <summary>Elastics animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ElasticInOut() => EaseWith(@this, ElasticInOutPercent);

        /// <summary>Elastics animation to animate the out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ElasticOut() => EaseWith(@this, ElasticOutPercent);

        /// <summary>Expo animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ExpoIn() => EaseWith(@this, ExpoInPercent);

        /// <summary>Expo animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ExpoInOut() => EaseWith(@this, ExpoInOutPercent);

        /// <summary>Expo animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ExpoOut() => EaseWith(@this, ExpoOutPercent);

        /// <summary>Quads animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuadIn() => EaseWith(@this, Square);

        /// <summary>Quads animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuadInOut() => EaseWith(@this, QuadInOutPercent);

        /// <summary>Quads animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuadOut() =>
            EaseWith(@this, percent => -percent * (percent - Two));

        /// <summary>Quartics animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuarticIn() => EaseWith(@this, FourthPower);

        /// <summary>Quartics animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuarticInOut() => EaseWith(@this, QuarticInOutPercent);

        /// <summary>Quartics animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuarticOut() => EaseWith(@this, QuarticOutPercent);

        /// <summary>Quintic animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuinticIn() => EaseWith(@this, FifthPower);

        /// <summary>Uses a Quintic animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuinticInOut() => EaseWith(@this, QuinticInOutPercent);

        /// <summary>Quintic animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> QuinticOut() => EaseWith(@this, QuinticOutPercent);

        /// <summary>Uses a Sin animation to animate the object in.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> SineIn() => EaseWith(@this, SineInPercent);

        /// <summary>Uses a Sin animation to animate the object in then out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> SineInOut() => EaseWith(@this, SineInOutPercent);

        /// <summary>Uses a Sin animation to animate the object out.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> SineOut() =>
            EaseWith(@this, percent => Math.Sin(percent * HalfPi));
    }

    /// <summary>Provides duration conversion extension methods for double observables.</summary>
    /// <param name="this">The double observable.</param>
    extension(IObservable<double> @this)
    {
        /// <summary>Maps a raw double (0..1) to a <see cref="Duration" />.</summary>
        /// <returns>A duration observable.</returns>
        public IObservable<Duration> ToDuration() => ToDurationStream(@this);
    }
}
