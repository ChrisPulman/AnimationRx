// Copyright (c) 2019-2026 Chris Pulman and contributors. All rights reserved.
// Chris Pulman and contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if REACTIVE_TESTS
extern alias AvaloniaReactiveRx;
#else
extern alias AvaloniaRx;
#endif

using Avalonia.Controls;
using Avalonia.Media;
#if REACTIVE_TESTS
using ReactiveUI.Primitives.Reactive;
using Observable = ReactiveUI.Primitives.Reactive.Signals.Signal;
using RxAnimations = AvaloniaReactiveRx::CP.AnimationRx.Reactive.Animations;
using RxAnimationsExtensions = AvaloniaReactiveRx::CP.AnimationRx.Reactive.AnimationsExtensions;
#else
using ReactiveUI.Primitives;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = AvaloniaRx::CP.AnimationRx.Animations;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
#endif
using TUnit.Assertions;
using TUnit.Core;

namespace AnimationRx.Tests;

/// <summary>Tests translation stream adapters.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Verifies translation preserves other transforms and remains lazy.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task ApplyTranslationPreservesTransformsAndDefersUpdates()
    {
        using var schedulerOverride = OverrideUiScheduler();
        var rotation = new RotateTransform();
        var button = new Button { RenderTransform = rotation };
        var positions = RxAnimations.DurationPercentage(InstantDuration)
            .Select(duration => duration.Percent)
            .Select(progress => (X: TargetTranslateX * progress, Y: TargetTranslateY * progress));
        var animation = RxAnimations.ApplyTranslation(positions, button);
        await Assert.That(button.RenderTransform).IsSameReferenceAs(rotation);
        await RunAnimationAsync(_ => animation);
        var group = (TransformGroup)button.RenderTransform!;
        await Assert.That(group.Children.Contains(rotation)).IsTrue();
        var translation = GetTransform<TranslateTransform>(button);
        await Assert.That(translation.X).IsEqualTo(TargetTranslateX);
        await Assert.That(translation.Y).IsEqualTo(TargetTranslateY);
        await RunAnimationAsync(_ => RxAnimationsExtensions.ApplyTranslation(positions, button));
        await Assert.That(GetTransform<TranslateTransform>(button)).IsSameReferenceAs(translation);
    }

    /// <summary>Verifies translation adapter argument guards.</summary>
    /// <returns>The test task.</returns>
    [Test]
    public async Task ApplyTranslationRejectsNullInputs()
    {
        var button = new Button();
        await Assert.That(() => RxAnimations.ApplyTranslation(null!, button)).Throws<ArgumentNullException>();
        await Assert.That(() => RxAnimations.ApplyTranslation(Observable.Return((0.0, 0.0)), null!)).Throws<ArgumentNullException>();
    }
}
