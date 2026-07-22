// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace AnimationRx.Avalonia.TestApp;

/// <summary>Defines constants for the Avalonia test app game controller.</summary>
public sealed partial class GameWindow
{
    /// <summary>Defines the player movement speed in pixels per second.</summary>
    private const double PlayerSpeed = 260.0;

    /// <summary>Defines the bullet movement speed in pixels per second.</summary>
    private const double BulletSpeed = 480.0;

    /// <summary>Defines the enemy movement speed in pixels per second.</summary>
    private const double EnemySpeed = 120.0;

    /// <summary>Defines the animation frame rate.</summary>
    private const int FramesPerSecond = 60;

    /// <summary>Defines the player width.</summary>
    private const double PlayerWidth = 40.0;

    /// <summary>Defines the player height.</summary>
    private const double PlayerHeight = 14.0;

    /// <summary>Defines the player's initial horizontal position.</summary>
    private const double PlayerInitialLeft = 100.0;

    /// <summary>Defines the player's initial vertical position.</summary>
    private const double PlayerInitialTop = 420.0;

    /// <summary>Defines the starting life count.</summary>
    private const int InitialLives = 3;

    /// <summary>Defines the initial frame delta in seconds.</summary>
    private const double InitialFrameDeltaSeconds = 0.016;

    /// <summary>Defines the maximum accepted frame delta in seconds.</summary>
    private const double MaxFrameDeltaSeconds = 0.05;

    /// <summary>Defines the inactive input value.</summary>
    private const double NoInput = 0.0;

    /// <summary>Defines the negative input value.</summary>
    private const double NegativeInput = -0.5;

    /// <summary>Defines the positive input value.</summary>
    private const double PositiveInput = 0.5;

    /// <summary>Defines the title fade duration in milliseconds.</summary>
    private const double TitleFadeMilliseconds = 600.0;

    /// <summary>Defines the overlay text fade duration in milliseconds.</summary>
    private const double OverlayTextFadeMilliseconds = 400.0;

    /// <summary>Defines the hint fade duration in milliseconds.</summary>
    private const double HintFadeMilliseconds = 250.0;

    /// <summary>Defines the press-start pulse interval in milliseconds.</summary>
    private const double PressStartPulseIntervalMilliseconds = 900.0;

    /// <summary>Defines the press-start pulse duration in milliseconds.</summary>
    private const double PressStartPulseMilliseconds = 200.0;

    /// <summary>Defines the dimmed press-start opacity.</summary>
    private const double PressStartDimOpacity = 0.3;

    /// <summary>Defines the overlay hide duration in milliseconds.</summary>
    private const double OverlayHideMilliseconds = 250.0;

    /// <summary>Defines the enemy spawn interval in milliseconds.</summary>
    private const double EnemySpawnIntervalMilliseconds = 900.0;

    /// <summary>Defines the idle breathing interval in seconds.</summary>
    private const double IdleBreathingIntervalSeconds = 2.0;

    /// <summary>Defines the idle breathing duration in milliseconds.</summary>
    private const double IdleBreathingMilliseconds = 800.0;

    /// <summary>Defines the idle breathing scale.</summary>
    private const double IdleBreathingScale = 1.15;

    /// <summary>Defines the bullet width.</summary>
    private const double BulletWidth = 6.0;

    /// <summary>Defines the bullet height.</summary>
    private const double BulletHeight = 2.0;

    /// <summary>Defines the divisor used to find the player center.</summary>
    private const double CenterDivisor = 2.0;

    /// <summary>Defines the bullet vertical offset.</summary>
    private const double BulletVerticalOffset = 1.0;

    /// <summary>Defines the minimum playfield width for spawning enemies.</summary>
    private const double MinimumEnemyPlayfieldWidth = 40.0;

    /// <summary>Defines the minimum playfield height for spawning enemies.</summary>
    private const double MinimumEnemyPlayfieldHeight = 20.0;

    /// <summary>Defines the enemy width.</summary>
    private const double EnemyWidth = 30.0;

    /// <summary>Defines the enemy height.</summary>
    private const double EnemyHeight = 12.0;

    /// <summary>Defines the right padding applied to spawned enemies.</summary>
    private const double EnemyRightPadding = 4.0;

    /// <summary>Defines the score awarded for an enemy hit.</summary>
    private const int EnemyHitScore = 10;

    /// <summary>Defines the hit flash duration in milliseconds.</summary>
    private const double HitFlashMilliseconds = 150.0;

    /// <summary>Defines the player shake duration in milliseconds.</summary>
    private const double PlayerShakeMilliseconds = 180.0;

    /// <summary>Defines the player shake distance.</summary>
    private const double PlayerShakeDistance = 3.0;

    /// <summary>Defines the number of player shakes.</summary>
    private const int PlayerShakeCount = 3;

    /// <summary>Defines the life pulse duration in milliseconds.</summary>
    private const double LifePulseMilliseconds = 80.0;

    /// <summary>Defines the low opacity used by the life pulse.</summary>
    private const double LifePulseLowOpacity = 0.3;

    /// <summary>Defines the number of life pulses.</summary>
    private const int LifePulseCount = 3;
}
