// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Provides render-synchronized frame sources.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Provides fluent callback frame sources.</summary>
    /// <param name="requestFrame">Registers the next rendering callback, such as TopLevel.RequestAnimationFrame.</param>
    extension(Action<Action<TimeSpan>> requestFrame)
    {
        /// <summary>Creates a cold frame stream; disposal stops emissions and callback renewal.</summary>
        /// <returns>The frame stream.</returns>
        public IObservable<long> RenderFrames() =>
            RenderFrameSource.Create(requestFrame).SubscribeOn(GetUiScheduler()).ObserveOn(GetUiScheduler());
    }
}
