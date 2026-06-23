// Copyright (c) 2023-2026 Chris Pulman and Contributors. All rights reserved.
// Chris Pulman and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>Duration.</summary>
public readonly record struct Duration : IEquatable<Duration>
{
    /// <summary>Initializes a new instance of the <see cref="Duration"/> struct.</summary>
    /// <param name="percent">The percent.</param>
    public Duration(double percent) => Percent = percent;

    /// <summary>Gets or sets the percent.</summary>
    /// <value>The percent.</value>
    public double Percent { get; }

    /// <summary>Creates the specified percent.</summary>
    /// <param name="percent">The percent.</param>
    /// <returns>A Value.</returns>
    public static Duration Create(double percent) => new(percent);
}
