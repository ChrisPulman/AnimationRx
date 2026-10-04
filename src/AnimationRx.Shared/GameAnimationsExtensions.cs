// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_SHIM
using ReactiveUI.Primitives.Reactive;
#else
using ReactiveUI.Primitives;
#endif

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides composable game animation trajectories and sprite frame selection.</summary>
public static class GameAnimationsExtensions
{
    /// <summary>Defines the quadratic control weight.</summary>
    private const double QuadraticWeight = 2;

    /// <summary>Defines the cubic control weight.</summary>
    private const double CubicWeight = 3;

    /// <summary>Defines the acceleration integration factor.</summary>
    private const double AccelerationFactor = 0.5;

    /// <summary>Provides animation mappings for numeric progress or time.</summary>
    /// <param name="progress">The progress or elapsed seconds stream.</param>
    extension(IObservable<double> progress)
    {
        /// <summary>Maps normalized progress to a quadratic Bezier trajectory, allowing easing overshoot.</summary>
        /// <param name="start">The starting position.</param>
        /// <param name="control">The control position.</param>
        /// <param name="end">The ending position.</param>
        /// <returns>The trajectory position stream.</returns>
        public IObservable<(double X, double Y)> QuadraticBezier((double X, double Y) start, (double X, double Y) control, (double X, double Y) end)
        {
            ValidateSource(progress);
            return progress.Select(t =>
            {
                var inverse = 1 - t;
                return (
                    (inverse * inverse * start.X) + (QuadraticWeight * inverse * t * control.X) + (t * t * end.X),
                    (inverse * inverse * start.Y) + (QuadraticWeight * inverse * t * control.Y) + (t * t * end.Y));
            });
        }

        /// <summary>Maps normalized progress to a cubic Bezier trajectory, allowing easing overshoot.</summary>
        /// <param name="start">The starting position.</param>
        /// <param name="control1">The first control position.</param>
        /// <param name="control2">The second control position.</param>
        /// <param name="end">The ending position.</param>
        /// <returns>The trajectory position stream.</returns>
        public IObservable<(double X, double Y)> CubicBezier(
            (double X, double Y) start,
            (double X, double Y) control1,
            (double X, double Y) control2,
            (double X, double Y) end)
        {
            ValidateSource(progress);
            return progress.Select(t =>
            {
                var inverse = 1 - t;
                var a = inverse * inverse * inverse;
                var b = CubicWeight * inverse * inverse * t;
                var c = CubicWeight * inverse * t * t;
                var d = t * t * t;
                return ((a * start.X) + (b * control1.X) + (c * control2.X) + (d * end.X), (a * start.Y) + (b * control1.Y) + (c * control2.Y) + (d * end.Y));
            });
        }

        /// <summary>Maps normalized progress to an elliptical orbit using angles in radians.</summary>
        /// <param name="center">The orbit center.</param>
        /// <param name="radii">The horizontal and vertical radii.</param>
        /// <param name="startAngle">The starting angle in radians.</param>
        /// <param name="sweepAngle">The total angle in radians; negative values reverse direction.</param>
        /// <returns>The orbit position stream.</returns>
        public IObservable<(double X, double Y)> Orbit((double X, double Y) center, (double X, double Y) radii, double startAngle, double sweepAngle)
        {
            ValidateSource(progress);
            return progress.Select(t =>
            {
                var angle = startAngle + (t * sweepAngle);
                return (center.X + (radii.X * Math.Cos(angle)), center.Y + (radii.Y * Math.Sin(angle)));
            });
        }

        /// <summary>Maps elapsed seconds to motion under constant acceleration.</summary>
        /// <param name="origin">The initial position.</param>
        /// <param name="velocity">The initial velocity in position units per second.</param>
        /// <param name="acceleration">The acceleration in position units per second squared.</param>
        /// <returns>The ballistic position stream.</returns>
        public IObservable<(double X, double Y)> Ballistic((double X, double Y) origin, (double X, double Y) velocity, (double X, double Y) acceleration)
        {
            ValidateSource(progress);
            return progress.Select(t => (origin.X + (velocity.X * t) + (AccelerationFactor * acceleration.X * t * t), origin.Y + (velocity.Y * t) + (AccelerationFactor * acceleration.Y * t * t)));
        }

        /// <summary>Maps normalized progress to zero-based sprite indices, clamping easing overshoot.</summary>
        /// <param name="frameCount">The positive number of sprite frames.</param>
        /// <returns>The frame index stream, emitting only changes.</returns>
        public IObservable<int> SpriteFrames(int frameCount)
        {
            ValidateSource(progress);
            if (frameCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(frameCount), frameCount, "Frame count must be positive.");
            }

            return progress.Select(t =>
            {
                if (t <= 0 || double.IsNaN(t))
                {
                    return 0;
                }

                return t >= 1 ? frameCount - 1 : (int)(t * frameCount);
            }).DistinctUntilChanged();
        }

    }

    /// <summary>Validates a source stream.</summary>
    /// <param name="source">The source stream.</param>
    private static void ValidateSource(IObservable<double> source)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
    }
}
