// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Input;

namespace CP.Animation.TestApp;

/// <summary>Provides event-stream helpers for <see cref="MainWindow"/>.</summary>
public partial class MainWindow
{
    /// <summary>Creates preview key-down notifications for this window.</summary>
    /// <returns>The preview key-down event stream.</returns>
    private IObservable<KeyEventArgs> PreviewKeyDownEvents() =>
        Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(
                handler => PreviewKeyDown += handler,
                handler => PreviewKeyDown -= handler)
            .Select(pattern => pattern.EventArgs);

    /// <summary>Creates loaded notifications for this window.</summary>
    /// <returns>The loaded event stream.</returns>
    private IObservable<RoutedEventArgs> LoadedEvents() =>
        Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                handler => Loaded += handler,
                handler => Loaded -= handler)
            .Select(pattern => pattern.EventArgs);

    /// <summary>Creates playfield size-changed notifications.</summary>
    /// <returns>The playfield size-changed event stream.</returns>
    private IObservable<SizeChangedEventArgs> PlayfieldSizeChangedEvents() =>
        Observable.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
                handler => Playfield.SizeChanged += handler,
                handler => Playfield.SizeChanged -= handler)
            .Select(pattern => pattern.EventArgs);

    /// <summary>Creates playfield key-down notifications.</summary>
    /// <returns>The playfield key-down event stream.</returns>
    private IObservable<KeyEventArgs> PlayfieldKeyDownEvents() =>
        Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(
                handler => Playfield.KeyDown += handler,
                handler => Playfield.KeyDown -= handler)
            .Select(pattern => pattern.EventArgs);

    /// <summary>Creates playfield key-up notifications.</summary>
    /// <returns>The playfield key-up event stream.</returns>
    private IObservable<KeyEventArgs> PlayfieldKeyUpEvents() =>
        Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(
                handler => Playfield.KeyUp += handler,
                handler => Playfield.KeyUp -= handler)
            .Select(pattern => pattern.EventArgs);
}
