// Copyright (c) 2023-2026 Chris Pulman and Contributors. All rights reserved.
// Chris Pulman and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AnimationRx.Avalonia.TestApp;

/// <summary>App.</summary>
public partial class App : Application
{
    /// <summary>Initializes the application by loading XAML etc.</summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>Called when [framework initialization completed].</summary>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
