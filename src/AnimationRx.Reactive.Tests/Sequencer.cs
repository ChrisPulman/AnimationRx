// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;

namespace AnimationRx.Tests;

/// <summary>Provides System.Reactive scheduler compatibility for linked reactive tests.</summary>
internal static class Sequencer
{
    /// <summary>Gets the immediate scheduler.</summary>
    internal static IScheduler Immediate => ImmediateScheduler.Instance;
}
