// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias WpfRx;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using TUnit.Assertions;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimations = WpfRx::CP.AnimationRx.Animations;
using RxDuration = WpfRx::CP.AnimationRx.Duration;
using RxEase = WpfRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains WPF UI animation tests.</summary>
public sealed partial class WpfUiAnimationTests
{
    /// <summary>Exercises translate animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseTranslateAnimationsAsync(Button button)
    {
        await ExerciseTranslateTransformAnimationAsync(button);
        await ExerciseTranslateByAnimationAsync(button);
        await ExerciseTranslateToAnimationAsync(button);
    }

    /// <summary>Exercises translate transform animation.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseTranslateTransformAnimationAsync(Button button)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimations.TranslateTransform(
                button,
                InstantDuration,
                TargetTranslateX,
                TargetTranslateY,
                RxEase.None,
                RxEase.None,
                scheduler));
        var translate = GetTransform<TranslateTransform>(button);
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);
        await Assert.That(translate.Y).IsEqualTo(TargetTranslateY).Within(AssertionTolerance);
    }

    /// <summary>Exercises translate by animation.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseTranslateByAnimationAsync(Button button)
    {
        await ExerciseTranslateTransformAnimationAsync(button);
        var translate = GetTransform<TranslateTransform>(button);
        await RunAnimationAsync(scheduler =>
            RxAnimations.TranslateBy(
                button,
                InstantDuration,
                DeltaTranslateX,
                DeltaTranslateY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX + DeltaTranslateX).Within(AssertionTolerance);
        await Assert.That(translate.Y).IsEqualTo(TargetTranslateY + DeltaTranslateY).Within(AssertionTolerance);
    }

    /// <summary>Exercises translate to animation.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseTranslateToAnimationAsync(Button button)
    {
        await ExerciseTranslateByAnimationAsync(button);
        var translate = GetTransform<TranslateTransform>(button);
        await RunAnimationAsync(scheduler =>
            RxAnimations.TranslateTo(
                button,
                InstantDuration,
                TargetTranslateX,
                TargetTranslateY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);
        await Assert.That(translate.Y).IsEqualTo(TargetTranslateY).Within(AssertionTolerance);
    }

    /// <summary>Exercises rotate animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseRotateAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimations.RotateTransform(button, InstantDuration, TargetAngle, RxEase.None, scheduler));
        var rotate = GetTransform<RotateTransform>(button);
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.RotateBy(button, InstantDuration, DeltaAngle, RxEase.None, scheduler));
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle + DeltaAngle).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.RotateTo(button, InstantDuration, TargetAngle, RxEase.None, scheduler));
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle).Within(AssertionTolerance);
    }

    /// <summary>Exercises scale animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseScaleAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimations.ScaleTransform(
                button,
                InstantDuration,
                TargetScaleX,
                TargetScaleY,
                RxEase.None,
                RxEase.None,
                scheduler));
        var scale = GetTransform<ScaleTransform>(button);
        await Assert.That(scale.ScaleX).IsEqualTo(TargetScaleX).Within(AssertionTolerance);
        await Assert.That(scale.ScaleY).IsEqualTo(TargetScaleY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.ScaleBy(
                button,
                InstantDuration,
                DeltaScaleX,
                DeltaScaleY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(scale.ScaleX).IsEqualTo(TargetScaleX + DeltaScaleX).Within(AssertionTolerance);
        await Assert.That(scale.ScaleY).IsEqualTo(TargetScaleY + DeltaScaleY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.ScaleTo(
                button,
                InstantDuration,
                TargetScaleX,
                TargetScaleY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(scale.ScaleX).IsEqualTo(TargetScaleX).Within(AssertionTolerance);
        await Assert.That(scale.ScaleY).IsEqualTo(TargetScaleY).Within(AssertionTolerance);
    }

    /// <summary>Exercises skew animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseSkewAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimations.SkewTransform(
                button,
                InstantDuration,
                TargetSkewX,
                TargetSkewY,
                RxEase.None,
                RxEase.None,
                scheduler));
        var skew = GetTransform<SkewTransform>(button);
        await Assert.That(skew.AngleX).IsEqualTo(TargetSkewX).Within(AssertionTolerance);
        await Assert.That(skew.AngleY).IsEqualTo(TargetSkewY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.SkewBy(
                button,
                InstantDuration,
                DeltaSkewX,
                DeltaSkewY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(skew.AngleX).IsEqualTo(TargetSkewX + DeltaSkewX).Within(AssertionTolerance);
        await Assert.That(skew.AngleY).IsEqualTo(TargetSkewY + DeltaSkewY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.SkewTo(
                button,
                InstantDuration,
                TargetSkewX,
                TargetSkewY,
                RxEase.None,
                RxEase.None,
                scheduler));
        await Assert.That(skew.AngleX).IsEqualTo(TargetSkewX).Within(AssertionTolerance);
        await Assert.That(skew.AngleY).IsEqualTo(TargetSkewY).Within(AssertionTolerance);
    }

    /// <summary>Exercises shake and pulse animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseEffectAnimationsAsync(Button button)
    {
        var translate = GetTransform<TranslateTransform>(button);
        await RunAnimationAsync(scheduler =>
            RxAnimations.ShakeTranslate(
                button,
                InstantDuration,
                ShakeAmplitude,
                DisabledShakeCount,
                RxEase.None,
                scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.ShakeTranslate(
                button,
                InstantDuration,
                ShakeAmplitude,
                EnabledShakeCount,
                RxEase.None,
                scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.PulseOpacity(
                button,
                InstantDuration,
                PulseLow,
                PulseHigh,
                DisabledPulseCount,
                RxEase.None,
                scheduler));
        await Assert.That(button.Opacity).IsEqualTo(PulseHigh).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimations.PulseOpacity(
                button,
                InstantDuration,
                PulseLow,
                PulseHigh,
                EnabledPulseCount,
                RxEase.None,
                scheduler));
        await Assert.That(button.Opacity).IsEqualTo(PulseHigh).Within(AssertionTolerance);
    }

    /// <summary>Overrides the private UI scheduler factory.</summary>
    /// <param name="owner">The owner type.</param>
    /// <returns>The disposable override.</returns>
    private static SchedulerOverride OverrideUiScheduler(Type owner)
    {
        var property = owner.GetProperty("UiSchedulerFactory", BindingFlags.Static | BindingFlags.NonPublic)!;
        var original = property.GetValue(null)!;
        property.SetValue(null, (Func<ISequencer>)(() => Sequencer.Immediate));
        return new(property, original);
    }

    /// <summary>Runs an animation with a virtual timing scheduler.</summary>
    /// <param name="animationFactory">The animation factory.</param>
    /// <returns>A task that completes when the animation finishes.</returns>
    private static async Task RunAnimationAsync(Func<ISequencer, IObservable<Unit>> animationFactory)
    {
        var scheduler = new VirtualClock();
        Exception? error = null;
        var completed = false;
        using var subscription = animationFactory(scheduler).Subscribe(
            _ => { },
            exception => error = exception,
            () => completed = true);
        for (var index = 0; index < VirtualAnimationAdvanceIterations && !completed && error is null; index++)
        {
            scheduler.AdvanceBy(VirtualAnimationAdvance);
        }

        if (error is not null)
        {
            throw error;
        }

        if (!completed)
        {
            throw new TimeoutException("The animation did not complete within the virtual scheduler advance limit.");
        }

        await Task.CompletedTask;
    }

    /// <summary>Gets a transform from an element render transform group.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The element.</param>
    /// <returns>The transform.</returns>
    private static T GetTransform<T>(FrameworkElement element)
        where T : Transform
    {
        var group = (TransformGroup)element.RenderTransform;
        return group.Children.OfType<T>().First();
    }

    /// <summary>Invokes facade methods.</summary>
    /// <param name="type">The facade type.</param>
    /// <param name="context">The invocation context.</param>
    private static void InvokeFacadeMethods(Type type, WpfReflectionContext context)
    {
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (method.ContainsGenericParameters)
            {
                continue;
            }

            var arguments = method.GetParameters()
                .Select(parameter => context.GetArgument(parameter.ParameterType))
                .ToArray();
            _ = method.Invoke(null, arguments);
            context.InvokedCount++;
        }
    }

    /// <summary>Restores a scheduler override.</summary>
    private sealed class SchedulerOverride : IDisposable
    {
        /// <summary>The reflected property.</summary>
        private readonly PropertyInfo _property;

        /// <summary>The original property value.</summary>
        private readonly object _original;

        /// <summary>Initializes a new instance of the <see cref="SchedulerOverride"/> class.</summary>
        /// <param name="property">The reflected property.</param>
        /// <param name="original">The original value.</param>
        internal SchedulerOverride(PropertyInfo property, object original)
        {
            _property = property;
            _original = original;
        }

        /// <inheritdoc/>
        public void Dispose() => _property.SetValue(null, _original);
    }

    /// <summary>Provides reflection invocation fixtures.</summary>
    private sealed class WpfReflectionContext
    {
        /// <summary>The reusable button.</summary>
        private readonly Button _button = new();

        /// <summary>The reusable brush.</summary>
        private readonly SolidColorBrush _brush = new(Colors.Transparent);

        /// <summary>The argument fixtures.</summary>
        private readonly Dictionary<Type, object> _arguments;

        /// <summary>Initializes a new instance of the <see cref="WpfReflectionContext"/> class.</summary>
        internal WpfReflectionContext()
        {
            _arguments = new Dictionary<Type, object>
            {
                [typeof(Button)] = _button,
                [typeof(Control)] = _button,
                [typeof(FrameworkElement)] = _button,
                [typeof(UIElement)] = _button,
                [typeof(SolidColorBrush)] = _brush,
                [typeof(double)] = InstantDuration,
                [typeof(int)] = EnabledPulseCount,
                [typeof(int?)] = EnabledPulseCount,
                [typeof(TimeSpan)] = TimeSpan.Zero,
                [typeof(RxEase)] = RxEase.None,
                [typeof(ISequencer)] = Sequencer.Immediate,
                [typeof(Thickness)] = TargetMargin,
                [typeof(Point)] = new Point(TargetTranslateX, TargetTranslateY),
                [typeof(Color)] = TargetColor,
                [typeof(IEnumerable<IObservable<Unit>>)] = new[] { Observable.Return(Unit.Default) }
            };
        }

        /// <summary>Gets or sets the invocation count.</summary>
        internal int InvokedCount { get; set; }

        /// <summary>Gets an argument for a reflected parameter type.</summary>
        /// <param name="type">The parameter type.</param>
        /// <returns>The argument.</returns>
        internal object GetArgument(Type type)
        {
            if (_arguments.TryGetValue(type, out var argument))
            {
                return argument;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IObservable<>))
            {
                return GetObservableArgument(type.GetGenericArguments()[0]);
            }

            throw new NotSupportedException(type.FullName);
        }

        /// <summary>Gets an observable argument for a reflected parameter type.</summary>
        /// <param name="type">The observable value type.</param>
        /// <returns>The observable argument.</returns>
        private static object GetObservableArgument(Type type)
        {
            if (type == typeof(double))
            {
                return Observable.Return(InstantDuration);
            }

            if (type == typeof(RxEase))
            {
                return Observable.Return(RxEase.None);
            }

            if (type == typeof(Point))
            {
                return Observable.Return(new Point(TargetTranslateX, TargetTranslateY));
            }

            if (type == typeof(RxDuration))
            {
                return Observable.Return(RxDuration.Create(1.0));
            }

            if (type == typeof(Unit))
            {
                return Observable.Return(Unit.Default);
            }

            throw new NotSupportedException(type.FullName);
        }
    }
}
