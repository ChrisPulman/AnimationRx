// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Media;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides position stream animation adapters.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides fluent position stream translation.</summary>
    /// <param name="positions">The absolute position samples.</param>
    extension(IObservable<(double X, double Y)> positions)
    {
        /// <summary>Applies position samples to translation on the UI scheduler while preserving other transforms.</summary>
        /// <param name="element">The visual to animate.</param>
        /// <returns>The update stream; disposal stops updates.</returns>
        public IObservable<Unit> ApplyTranslation(Visual element)
        {
            if (positions is null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return Observable.Defer(() => positions.ObserveOn(GetUiScheduler())
                .Do(position =>
                {
                    var transform = GetOrAddTransform(element, static () => new TranslateTransform());
                    transform.X = position.X;
                    transform.Y = position.Y;
                })
                .Select(_ => Unit.Default));
        }
    }
}
