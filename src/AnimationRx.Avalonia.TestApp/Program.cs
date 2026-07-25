// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Avalonia;

namespace AnimationRx.Avalonia.TestApp;

/// <summary>Starts the Avalonia test application.</summary>
internal static class Program
{
    /// <summary>Runs the Avalonia desktop application.</summary>
    /// <param name="args">The command-line arguments.</param>
    [STAThread]
    internal static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    /// <summary>Builds the Avalonia application configuration.</summary>
    /// <returns>The configured application builder.</returns>
    internal static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
