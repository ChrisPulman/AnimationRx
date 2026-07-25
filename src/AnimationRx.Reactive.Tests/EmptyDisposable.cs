// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;

namespace AnimationRx.Tests;

/// <summary>Provides an empty disposable matching the lean test helper name.</summary>
internal static class EmptyDisposable
{
    /// <summary>Gets an empty disposable instance.</summary>
    internal static IDisposable Instance => Disposable.Empty;
}
