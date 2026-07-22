// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace CP.AnimationRx;

/// <summary>Provides shared WPF animation helper methods.</summary>
public static partial class Animations
{
    /// <summary>Gets or sets the UI scheduler factory.</summary>
    private static Func<IScheduler> UiSchedulerFactory { get; set; } = CreateUiScheduler;

    /// <summary>Animates the left margin for a single resolved value set.</summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The animation duration in milliseconds.</param>
    /// <param name="position">The target margin position.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">The optional scheduler.</param>
    /// <returns>The animation completion observable.</returns>
    private static IObservable<Unit> LeftMarginMoveCore(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler)
        => Observable.Defer(() => Observable.Start(() => element.Margin.Left, GetUiScheduler())
                .SelectMany(initialValue =>
                    DurationPercentage(milliSeconds, scheduler)
                        .EaseAnimation(ease)
                        .Distance(position - initialValue)
                        .ObserveOn(GetUiScheduler())
                        .Do(t =>
                        {
                            var mar = element.Margin;
                            mar.Left = initialValue + t;
                            element.Margin = mar;
                        })
                        .Select(_ => Unit.Default)));

    /// <summary>Animates the right margin for a single resolved value set.</summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The animation duration in milliseconds.</param>
    /// <param name="position">The target margin position.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">The optional scheduler.</param>
    /// <returns>The animation completion observable.</returns>
    private static IObservable<Unit> RightMarginMoveCore(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Margin.Right, GetUiScheduler())
            .SelectMany(initialValue =>
                DurationPercentage(milliSeconds, scheduler)
                    .EaseAnimation(ease)
                    .Distance(position - initialValue)
                    .ObserveOn(GetUiScheduler())
                    .Do(t =>
                    {
                        var mar = element.Margin;
                        mar.Right = initialValue + t;
                        element.Margin = mar;
                    })
                    .Select(_ => Unit.Default)));

    /// <summary>Animates the top margin for a single resolved value set.</summary>
    /// <param name="element">The element to animate.</param>
    /// <param name="milliSeconds">The animation duration in milliseconds.</param>
    /// <param name="position">The target margin position.</param>
    /// <param name="ease">The easing function.</param>
    /// <param name="scheduler">The optional scheduler.</param>
    /// <returns>The animation completion observable.</returns>
    private static IObservable<Unit> TopMarginMoveCore(
        FrameworkElement element,
        double milliSeconds,
        double position,
        Ease ease,
        IScheduler? scheduler) =>
        Observable.Defer(() => Observable.Start(() => element.Margin.Top, GetUiScheduler())
            .SelectMany(initialValue =>
                DurationPercentage(milliSeconds, scheduler)
                    .EaseAnimation(ease)
                    .Distance(position - initialValue)
                    .ObserveOn(GetUiScheduler())
                    .Do(t =>
                    {
                        var mar = element.Margin;
                        mar.Top = initialValue + t;
                        element.Margin = mar;
                    })
                    .Select(_ => Unit.Default)));

    /// <summary>Gets an existing transform or adds a new one to the element.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The element that owns the transform.</param>
    /// <param name="factory">The transform factory.</param>
    /// <returns>The existing or newly created transform.</returns>
    private static T GetOrAddTransform<T>(FrameworkElement element, Func<T> factory)
        where T : Transform
    {
        if (element.RenderTransform is TransformGroup existingGroup)
        {
            foreach (var child in existingGroup.Children)
            {
                if (child is T found)
                {
                    return found;
                }
            }

            var newTr = factory();
            existingGroup.Children.Add(newTr);
            return newTr;
        }

        if (element.RenderTransform is T existing)
        {
            var group = new TransformGroup();
            group.Children.Add(existing);
            element.RenderTransform = group;
            return existing;
        }

        if (element.RenderTransform is Transform single)
        {
            var group = new TransformGroup();
            group.Children.Add(single);
            var newTr = factory();
            group.Children.Add(newTr);
            element.RenderTransform = group;
            return newTr;
        }

        var newGroup = new TransformGroup();
        var newTransform = factory();
        newGroup.Children.Add(newTransform);
        element.RenderTransform = newGroup;
        return newTransform;
    }

    /// <summary>Gets the scheduler used for background animation timing.</summary>
    /// <returns>The background scheduler.</returns>
    private static TaskPoolScheduler GetBackgroundScheduler() => TaskPoolScheduler.Default;

    /// <summary>Gets the scheduler used for UI updates.</summary>
    /// <returns>The UI scheduler.</returns>
    private static IScheduler GetUiScheduler() => UiSchedulerFactory();

    /// <summary>Creates a scheduler for the current WPF dispatcher.</summary>
    /// <returns>The dispatcher-backed scheduler.</returns>
    private static SynchronizationContextScheduler CreateUiScheduler()
    {
        var dispatcher = Application.Current?.Dispatcher
            ?? Dispatcher.CurrentDispatcher;

        return new(new DispatcherSynchronizationContext(dispatcher));
    }

    /// <summary>Clamps the specified value.</summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>The clamped value.</returns>
    private static double Clamp(double value, double min, double max)
    {
        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    /// <summary>Linear Interpolation between two thicknesses.</summary>
    /// <param name="a">The starting thickness.</param>
    /// <param name="b">The ending thickness.</param>
    /// <param name="t">The interpolation progress.</param>
    /// <returns>The interpolated thickness.</returns>
    private static Thickness Lerp(Thickness a, Thickness b, double t)
    {
        var p = Clamp(t, 0, 1);
        return new(
            a.Left + ((b.Left - a.Left) * p),
            a.Top + ((b.Top - a.Top) * p),
            a.Right + ((b.Right - a.Right) * p),
            a.Bottom + ((b.Bottom - a.Bottom) * p));
    }

    /// <summary>Linear Interpolation between two colours.</summary>
    /// <param name="a">The starting colour.</param>
    /// <param name="b">The ending colour.</param>
    /// <param name="t">The interpolation progress.</param>
    /// <returns>The interpolated colour.</returns>
    private static Color Lerp(Color a, Color b, double t)
    {
        var p = Clamp(t, 0, 1);
        byte LerpByte(byte x, byte y) => (byte)(x + ((y - x) * p));
        return Color.FromArgb(LerpByte(a.A, b.A), LerpByte(a.R, b.R), LerpByte(a.G, b.G), LerpByte(a.B, b.B));
    }
}
