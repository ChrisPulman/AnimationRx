// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CP.AnimationRx;

/// <summary>
/// Duration.
/// </summary>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public struct Duration : IEquatable<Duration>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Duration"/> struct.
    /// </summary>
    /// <param name="percent">The percent.</param>
    public Duration(double percent) => Percent = percent;

    /// <summary>
    /// Gets or sets the percent.
    /// </summary>
    /// <value>The percent.</value>
    public double Percent { get; set; }

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Duration left, Duration right) => left.Equals(right);

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Duration left, Duration right) => !(left == right);

    /// <summary>
    /// Creates the specified percent.
    /// </summary>
    /// <param name="percent">The percent.</param>
    /// <returns>A Value.</returns>
    public static Duration Create(double percent) => new(percent);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
    /// parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(Duration other) => Percent == other.Percent;

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///   <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.
    /// </returns>
    public override readonly bool Equals(object? obj) => obj is Duration duration && Equals(duration);
}
