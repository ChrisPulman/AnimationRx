// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

extern alias AvaloniaRx;

using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using TUnit.Assertions;
using Observable = ReactiveUI.Primitives.Signals.Signal;
using RxAnimationsExtensions = AvaloniaRx::CP.AnimationRx.AnimationsExtensions;
using RxDuration = AvaloniaRx::CP.AnimationRx.Duration;
using RxEase = AvaloniaRx::CP.AnimationRx.Ease;
using Unit = ReactiveUI.Primitives.RxVoid;

namespace AnimationRx.Tests;

/// <summary>Contains Avalonia UI animation test helpers.</summary>
public sealed partial class AvaloniaUiAnimationTests
{
    /// <summary>Creates a button with deterministic animation state.</summary>
    /// <returns>The test button.</returns>
    private static Button CreateButton()
    {
        var button = new Button
        {
            Width = InitialWidth,
            Height = InitialHeight,
            Opacity = 1.0,
            Margin = default,
            Padding = default
        };
        Canvas.SetLeft(button, 0.0);
        Canvas.SetTop(button, 0.0);
        Canvas.SetRight(button, 0.0);
        Canvas.SetBottom(button, 0.0);
        return button;
    }

    /// <summary>Exercises layout animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseLayoutAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.MarginTo(button, InstantDuration, TargetThickness, RxEase.None, scheduler));
        await Assert.That(button.Margin).IsEqualTo(TargetThickness);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.PaddingTo(button, InstantDuration, TargetThickness, RxEase.None, scheduler));
        await Assert.That(button.Padding).IsEqualTo(TargetThickness);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.CanvasLeftTo(button, InstantDuration, TargetCanvasLeft, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.CanvasTopTo(button, InstantDuration, TargetCanvasTop, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.CanvasRightTo(button, InstantDuration, TargetCanvasRight, RxEase.None, scheduler));
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.CanvasBottomTo(button, InstantDuration, TargetCanvasBottom, RxEase.None, scheduler));

        await Assert.That(Canvas.GetLeft(button)).IsEqualTo(TargetCanvasLeft).Within(AssertionTolerance);
        await Assert.That(Canvas.GetTop(button)).IsEqualTo(TargetCanvasTop).Within(AssertionTolerance);
        await Assert.That(Canvas.GetRight(button)).IsEqualTo(TargetCanvasRight).Within(AssertionTolerance);
        await Assert.That(Canvas.GetBottom(button)).IsEqualTo(TargetCanvasBottom).Within(AssertionTolerance);
    }

    /// <summary>Exercises brush color animations.</summary>
    /// <param name="brush">The test brush.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseBrushAnimationsAsync(SolidColorBrush brush)
    {
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.BrushColorTo(brush, InstantDuration, TargetColor, RxEase.None, scheduler));
        await Assert.That(brush.Color).IsEqualTo(TargetColor);

        brush.Color = Colors.Transparent;
        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.ColorTo(brush, InstantDuration, TargetColor, RxEase.None, scheduler));
        await Assert.That(brush.Color).IsEqualTo(TargetColor);
    }

    /// <summary>Exercises translate animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseTranslateAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateTransform(
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

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateBy(
            button,
            InstantDuration,
            DeltaTranslateX,
            DeltaTranslateY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX + DeltaTranslateX).Within(AssertionTolerance);
        await Assert.That(translate.Y).IsEqualTo(TargetTranslateY + DeltaTranslateY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.TranslateTo(
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
            RxAnimationsExtensions.RotateTransform(button, InstantDuration, TargetAngle, RxEase.None, scheduler));
        var rotate = GetTransform<RotateTransform>(button);
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.RotateBy(button, InstantDuration, DeltaAngle, RxEase.None, scheduler));
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle + DeltaAngle).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler =>
            RxAnimationsExtensions.RotateTo(button, InstantDuration, TargetAngle, RxEase.None, scheduler));
        await Assert.That(rotate.Angle).IsEqualTo(TargetAngle).Within(AssertionTolerance);
    }

    /// <summary>Exercises scale animations.</summary>
    /// <param name="button">The test button.</param>
    /// <returns>A task that completes when the test finishes.</returns>
    private static async Task ExerciseScaleAnimationsAsync(Button button)
    {
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.ScaleTransform(
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

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.ScaleBy(
            button,
            InstantDuration,
            DeltaScaleX,
            DeltaScaleY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(scale.ScaleX).IsEqualTo(TargetScaleX + DeltaScaleX).Within(AssertionTolerance);
        await Assert.That(scale.ScaleY).IsEqualTo(TargetScaleY + DeltaScaleY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.ScaleTo(
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
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.SkewTransform(
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

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.SkewBy(
            button,
            InstantDuration,
            DeltaSkewX,
            DeltaSkewY,
            RxEase.None,
            RxEase.None,
            scheduler));
        await Assert.That(skew.AngleX).IsEqualTo(TargetSkewX + DeltaSkewX).Within(AssertionTolerance);
        await Assert.That(skew.AngleY).IsEqualTo(TargetSkewY + DeltaSkewY).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.SkewTo(
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
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.ShakeTranslate(
            button,
            InstantDuration,
            ShakeAmplitude,
            DisabledCount,
            RxEase.None,
            scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.ShakeTranslate(
            button,
            InstantDuration,
            ShakeAmplitude,
            EnabledCount,
            RxEase.None,
            scheduler));
        await Assert.That(translate.X).IsEqualTo(TargetTranslateX).Within(AssertionTolerance);

        await RunAnimationAsync(scheduler => RxAnimationsExtensions.PulseOpacity(
            button,
            InstantDuration,
            PulseLow,
            PulseHigh,
            DisabledCount,
            RxEase.None,
            scheduler));
        await RunAnimationAsync(scheduler => RxAnimationsExtensions.PulseOpacity(
            button,
            InstantDuration,
            PulseLow,
            PulseHigh,
            EnabledCount,
            RxEase.None,
            scheduler));
        await Assert.That(button.Opacity).IsEqualTo(PulseHigh).Within(AssertionTolerance);
    }

    /// <summary>Overrides the Avalonia UI scheduler with an immediate sequencer.</summary>
    /// <returns>A disposable scheduler override.</returns>
    private static SchedulerOverride OverrideUiScheduler()
    {
        var property = typeof(RxAnimationsExtensions).GetProperty(
            "UiSchedulerFactory",
            BindingFlags.Static | BindingFlags.NonPublic)!;
        var original = property.GetValue(null)!;
        property.SetValue(null, (Func<ISequencer>)(() => Sequencer.Immediate));
        return new(property, original);
    }

    /// <summary>Runs an animation and asserts that it completes synchronously.</summary>
    /// <param name="animationFactory">The animation factory.</param>
    /// <returns>A task that completes when the assertion finishes.</returns>
    private static async Task RunAnimationAsync(Func<ISequencer, IObservable<Unit>> animationFactory)
    {
        Exception? error = null;
        var completed = false;
        using var subscription = animationFactory(Sequencer.Immediate).Subscribe(
            _ => { },
            exception => error = exception,
            () => completed = true);

        if (error is not null)
        {
            throw error;
        }

        await Assert.That(completed).IsTrue();
    }

    /// <summary>Gets a transform from an element transform group.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The visual element.</param>
    /// <returns>The requested transform.</returns>
    private static T GetTransform<T>(Visual element)
        where T : Transform
    {
        var group = (TransformGroup)element.RenderTransform!;
        return group.Children.OfType<T>().First();
    }

    /// <summary>Invokes public facade methods with deterministic fixture arguments.</summary>
    /// <param name="type">The facade type.</param>
    /// <param name="context">The reflection context.</param>
    private static void InvokeFacadeMethods(Type type, AvaloniaReflectionContext context)
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

    /// <summary>Restores the overridden Avalonia UI scheduler.</summary>
    private sealed class SchedulerOverride : IDisposable
    {
        /// <summary>Stores the scheduler factory property.</summary>
        private readonly PropertyInfo _property;

        /// <summary>Stores the original scheduler factory value.</summary>
        private readonly object _original;

        /// <summary>Initializes a new instance of the <see cref="SchedulerOverride"/> class.</summary>
        /// <param name="property">The scheduler factory property.</param>
        /// <param name="original">The original scheduler factory value.</param>
        internal SchedulerOverride(PropertyInfo property, object original)
        {
            _property = property;
            _original = original;
        }

        /// <inheritdoc />
        public void Dispose() => _property.SetValue(null, _original);
    }

    /// <summary>Supplies deterministic arguments for facade reflection tests.</summary>
    private sealed class AvaloniaReflectionContext
    {
        /// <summary>Stores the test button.</summary>
        private readonly Button _button = CreateButton();

        /// <summary>Stores the test brush.</summary>
        private readonly SolidColorBrush _brush = new(Colors.Transparent);

        /// <summary>Stores reflection arguments keyed by parameter type.</summary>
        private readonly Dictionary<Type, object> _arguments;

        /// <summary>Initializes a new instance of the <see cref="AvaloniaReflectionContext"/> class.</summary>
        internal AvaloniaReflectionContext()
        {
            _arguments = new Dictionary<Type, object>
            {
                [typeof(Button)] = _button,
                [typeof(Control)] = _button,
                [typeof(TemplatedControl)] = _button,
                [typeof(Visual)] = _button,
                [typeof(SolidColorBrush)] = _brush,
                [typeof(double)] = InstantDuration,
                [typeof(int)] = EnabledCount,
                [typeof(int?)] = EnabledCount,
                [typeof(TimeSpan)] = TimeSpan.Zero,
                [typeof(RxEase)] = RxEase.None,
                [typeof(ISequencer)] = Sequencer.Immediate,
                [typeof(Thickness)] = TargetThickness,
                [typeof(Color)] = TargetColor,
                [typeof(IEnumerable<IObservable<Unit>>)] = new[] { Observable.Return(Unit.Default) }
            };
        }

        /// <summary>Gets or sets the invoked method count.</summary>
        internal int InvokedCount { get; set; }

        /// <summary>Gets an argument for a facade parameter type.</summary>
        /// <param name="type">The requested argument type.</param>
        /// <returns>The argument value.</returns>
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

        /// <summary>Gets an observable argument for a facade parameter type.</summary>
        /// <param name="type">The requested observable item type.</param>
        /// <returns>The observable argument value.</returns>
        private static object GetObservableArgument(Type type)
        {
            if (type == typeof(double))
            {
                return Observable.Return(InstantDuration);
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
