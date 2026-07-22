// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Avalonia.Controls;

namespace AnimationRx.Avalonia.TestApp;

/// <summary>Main window for the Avalonia test application.</summary>
public partial class MainWindow : Window, IDisposable
{
    /// <summary>Stores the game controller.</summary>
    private GameWindow? _gameWindow;

    /// <summary>Stores whether this instance has been disposed.</summary>
    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="MainWindow"/> class.</summary>
    public MainWindow() => InitializeComponent();

    /// <summary>Releases resources held by this window.</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Creates the game controller after construction is complete.</summary>
    /// <param name="e">The event data.</param>
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        _gameWindow = new(this);
    }

    /// <summary>Releases the game controller when the window closes.</summary>
    /// <param name="e">The event data.</param>
    protected override void OnClosed(EventArgs e)
    {
        Dispose();
        base.OnClosed(e);
    }

    /// <summary>Releases resources held by this window.</summary>
    /// <param name="disposing">True to dispose managed resources, otherwise false.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _gameWindow?.Dispose();
            _gameWindow = null;
        }

        _disposed = true;
    }
}
