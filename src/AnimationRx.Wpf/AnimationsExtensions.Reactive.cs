// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Windows;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides observable driven layout animation overloads.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides bottom margin animation extensions.</summary>
    /// <param name="element">The element to animate.</param>
    extension(FrameworkElement element)
    {
        /// <summary>Animates the bottom margin using the latest duration, position, and easing.</summary>
        /// <param name="milliSeconds">The duration stream.</param>
        /// <param name="position">The target margin stream.</param>
        /// <param name="ease">The easing stream.</param>
        /// <param name="scheduler">The animation scheduler.</param>
        /// <returns>A stream of animation updates.</returns>
        public IObservable<Unit> BottomMarginMove(
            IObservable<double> milliSeconds,
            IObservable<double> position,
            IObservable<Ease> ease,
            IScheduler? scheduler) =>
            Animations.BottomMarginMove(
                element,
            milliSeconds,
            position,
            ease,
            scheduler);
    }
}
