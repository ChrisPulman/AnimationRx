// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides position stream animation adapters.</summary>
public static partial class Animations
{
    /// <summary>Applies positions while preserving existing visual transforms.</summary>
    /// <param name="positions">The absolute translation position stream.</param>
    /// <param name="element">The visual to animate.</param>
    /// <returns>The UI update stream.</returns>
    public static IObservable<Unit> ApplyTranslation(IObservable<(double X, double Y)> positions, Visual element) =>
        AnimationsExtensions.ApplyTranslation(positions, element);
}
