// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides the matching render frame facade.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides fluent callback frame sources.</summary>
    /// <param name="requestFrame">Registers the next rendering callback.</param>
    extension(Action<Action<TimeSpan>> requestFrame)
    {
        /// <summary>Creates a cancellable stream synchronized with the native rendering source.</summary>
        /// <returns>The frame stream.</returns>
        public IObservable<long> RenderFrames() => Animations.RenderFrames(requestFrame);
    }
}
