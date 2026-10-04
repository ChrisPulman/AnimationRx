// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias WpfReactiveRx;
#else
extern alias WpfRx;
#endif

#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
#else
using ReactiveUI.Primitives;
#endif
using TUnit.Core;
#if REACTIVE_TESTS
using RxEases = WpfReactiveRx::CP.AnimationRx.Reactive.Eases;
using RxEasesExtensions = WpfReactiveRx::CP.AnimationRx.Reactive.EasesExtensions;
#else
using RxEases = WpfRx::CP.AnimationRx.Eases;
using RxEasesExtensions = WpfRx::CP.AnimationRx.EasesExtensions;
#endif

namespace AnimationRx.Tests;

/// <summary>Verifies equivalent raw duration conversion facades.</summary>
public sealed partial class WpfCoreTests
{
    /// <summary>Verifies both easing facades preserve raw duration percentages.</summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task ToDuration_EasingFacades_PreservePercentages()
    {
        var values = Percentages.Take(SampleCount).ToArray();
        var facade = await ReadPercentsAsync(RxEases.ToDuration(values.ToObservable()));
        var extensions = await ReadPercentsAsync(RxEasesExtensions.ToDuration(values.ToObservable()));
        await AssertSequencesAreEqualAsync(facade, values);
        await AssertSequencesAreEqualAsync(extensions, values);
    }
}
