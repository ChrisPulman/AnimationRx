// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using ReactiveUI.Builder;
using Splat;

namespace CP.Animation.TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Use RxUI Builder
            AppLocator.CurrentMutable.CreateReactiveUIBuilder()
                .WithWpf()
                .BuildApp();
        }
    }
}
