// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Media;
using ReactiveUI.Avalonia;

namespace CP.AnimationRx;

/// <summary>Provides shared Avalonia animation helper methods.</summary>
public static partial class AnimationsExtensions
{
    /// <summary>Gets or sets the UI scheduler factory.</summary>
    private static Func<IScheduler> UiSchedulerFactory { get; set; } = CreateUiScheduler;

    /// <summary>Linear interpolation between two thicknesses.</summary>
    /// <param name="a">The starting thickness.</param>
    /// <param name="b">The target thickness.</param>
    /// <param name="t">The interpolation percentage.</param>
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

    /// <summary>Linear interpolation between two colors.</summary>
    /// <param name="a">The starting color.</param>
    /// <param name="b">The target color.</param>
    /// <param name="t">The interpolation percentage.</param>
    /// <returns>The interpolated color.</returns>
    private static Color Lerp(Color a, Color b, double t)
    {
        var p = Clamp(t, 0, 1);
        byte LerpByte(byte x, byte y) => (byte)(x + ((y - x) * p));
        return Color.FromArgb(LerpByte(a.A, b.A), LerpByte(a.R, b.R), LerpByte(a.G, b.G), LerpByte(a.B, b.B));
    }

    /// <summary>Clamps the specified value.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <returns>The clamped value.</returns>
    private static double Clamp(double value, double min, double max)
    {
        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    /// <summary>Gets an existing transform of the requested type or adds a new one.</summary>
    /// <typeparam name="T">The transform type.</typeparam>
    /// <param name="element">The element that owns the transform.</param>
    /// <param name="factory">The factory used when a transform must be created.</param>
    /// <returns>The existing or newly added transform.</returns>
    private static T GetOrAddTransform<T>(Visual element, Func<T> factory)
        where T : Transform
    {
        var current = element.RenderTransform;

        if (current is TransformGroup group)
        {
            foreach (var child in group.Children)
            {
                if (child is T found)
                {
                    return found;
                }
            }

            var newTr = factory();
            group.Children.Add(newTr);
            element.RenderTransform = group;
            return newTr;
        }

        if (current is T existing)
        {
            var g = new TransformGroup();
            g.Children.Add(existing);
            element.RenderTransform = g;
            return existing;
        }

        if (current is Transform single)
        {
            var g = new TransformGroup();
            g.Children.Add(single);
            var newTr = factory();
            g.Children.Add(newTr);
            element.RenderTransform = g;
            return newTr;
        }

        var groupNew = new TransformGroup();
        var newTransform = factory();
        groupNew.Children.Add(newTransform);
        element.RenderTransform = groupNew;
        return newTransform;
    }

    /// <summary>Gets the scheduler used for background animation timing.</summary>
    /// <returns>The background scheduler.</returns>
    private static TaskPoolScheduler GetBackgroundScheduler() => TaskPoolScheduler.Default;

    /// <summary>Gets the scheduler used for UI updates.</summary>
    /// <returns>The UI scheduler.</returns>
    private static IScheduler GetUiScheduler() => UiSchedulerFactory();

    /// <summary>Creates the Avalonia UI scheduler.</summary>
    /// <returns>The Avalonia UI scheduler.</returns>
    private static AvaloniaScheduler CreateUiScheduler() => AvaloniaScheduler.Instance;
}
