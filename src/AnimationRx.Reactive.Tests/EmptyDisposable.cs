// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;

namespace AnimationRx.Tests;

/// <summary>Provides an empty disposable matching the lean test helper name.</summary>
internal static class EmptyDisposable
{
    /// <summary>Gets an empty disposable instance.</summary>
    internal static IDisposable Instance => Disposable.Empty;
}
