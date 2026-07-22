// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Primitives;

namespace CP.AnimationRx;

/// <summary>Provides shared animation repetition support.</summary>
internal static class AnimationRepeat
{
    /// <summary>Repeats an animation sequence.</summary>
    /// <param name="animation">The animation sequence.</param>
    /// <param name="count">The number of repetitions, or <see langword="null"/> to repeat indefinitely.</param>
    /// <returns>An observable that repeats the supplied animation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="animation"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
    internal static IObservable<Unit> Repeat(IObservable<Unit> animation, int? count)
    {
        if (animation is null)
        {
            throw new ArgumentNullException(nameof(animation));
        }

        if (count is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Repeat count cannot be negative.");
        }

        if (count == 0)
        {
            return Observable.Empty<Unit>();
        }

        if (count is null)
        {
            return Observable.Defer(() => animation.Concat(Repeat(animation, null)));
        }

        var sequence = animation;
        for (var index = 1; index < count.Value; index++)
        {
            sequence = sequence.Concat(animation);
        }

        return sequence;
    }
}
