// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;

#if REACTIVE_SHIM
namespace CP.AnimationRx.Reactive;
#else
namespace CP.AnimationRx;
#endif

/// <summary>Adapts one-shot rendering callbacks to a cancellable frame source.</summary>
internal static class RenderFrameSource
{
    /// <summary>Creates an independent frame counter for each subscription.</summary>
    /// <param name="requestFrame">Registers a one-shot callback for the next rendering tick.</param>
    /// <returns>A lazy frame stream.</returns>
    internal static IObservable<long> Create(Action<Action<TimeSpan>> requestFrame)
    {
        _ = requestFrame ?? throw new ArgumentNullException(nameof(requestFrame));
        return Observable.Create<long>(observer =>
        {
            long frame = 0;
            var disposed = 0;
            void OnFrame(TimeSpan elapsed)
            {
                _ = elapsed;
                if (Volatile.Read(ref disposed) != 0)
                {
                    return;
                }

                observer.OnNext(frame);
                frame++;
                if (Volatile.Read(ref disposed) != 0)
                {
                    return;
                }

                RequestNextFrame();
            }

            void RequestNextFrame()
            {
                try
                {
                    requestFrame(OnFrame);
                }
                catch (Exception error)
                {
                    _ = Interlocked.Exchange(ref disposed, 1);
                    observer.OnError(error);
                }
            }

            RequestNextFrame();
            return Disposable.Create(() => Interlocked.Exchange(ref disposed, 1));
        });
    }
}
