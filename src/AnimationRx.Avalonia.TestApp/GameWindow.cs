// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using CP.AnimationRx;
using ReactiveUI.Avalonia;

namespace AnimationRx.Avalonia.TestApp;

/// <summary>Game controller for the Avalonia test app.</summary>
public sealed partial class GameWindow : IDisposable
{
    /// <summary>Stores the owning window.</summary>
    private readonly Window _window;

    /// <summary>Stores the game playfield canvas.</summary>
    private readonly Canvas _playfield;

    /// <summary>Stores the title overlay panel.</summary>
    private readonly Border _titleOverlay;

    /// <summary>Stores the score display.</summary>
    private readonly TextBlock _scoreText;

    /// <summary>Stores the lives display.</summary>
    private readonly TextBlock _livesText;

    /// <summary>Stores the game title display.</summary>
    private readonly TextBlock _gameTitle;

    /// <summary>Stores the credits display.</summary>
    private readonly TextBlock _creditsText;

    /// <summary>Stores the press-start display.</summary>
    private readonly TextBlock _pressStartText;

    /// <summary>Stores the hint display.</summary>
    private readonly TextBlock _hintText;

    /// <summary>Stores the player shape.</summary>
    private readonly Rectangle _player;

    /// <summary>Stores the player brush.</summary>
    private readonly SolidColorBrush _playerBrush = new(Colors.DeepSkyBlue);

    /// <summary>Stores the active bullet shapes.</summary>
    private readonly List<Rectangle> _bullets = [];

    /// <summary>Stores the active enemy shapes.</summary>
    private readonly List<Rectangle> _enemies = [];

    /// <summary>Stores subscriptions that live for the window.</summary>
    private readonly CompositeDisposable _lifetime = [];

    /// <summary>Stores subscriptions for the current game run.</summary>
    private CompositeDisposable _game = [];

    /// <summary>Stores subscriptions for the title overlay.</summary>
    private CompositeDisposable? _overlay;

    /// <summary>Stores the current score.</summary>
    private int _score;

    /// <summary>Stores the remaining lives.</summary>
    private int _lives = InitialLives;

    /// <summary>Stores whether a game is running.</summary>
    private bool _isRunning;

    /// <summary>Stores whether this instance has been disposed.</summary>
    private bool _disposed;

    /// <summary>Stores the current horizontal input direction.</summary>
    private double _horizontalInput;

    /// <summary>Stores the current vertical input direction.</summary>
    private double _verticalInput;

    /// <summary>Initializes a new instance of the <see cref="GameWindow"/> class.</summary>
    /// <param name="window">The window.</param>
    /// <exception cref="System.InvalidOperationException">
    /// Playfield not found
    /// or
    /// TitleOverlay not found
    /// or
    /// ScoreText not found
    /// or
    /// LivesText not found
    /// or
    /// GameTitle not found
    /// or
    /// CreditsText not found
    /// or
    /// PressStartText not found
    /// or
    /// HintText not found.
    /// </exception>
    public GameWindow(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);
        _window = window;

        _playfield = FindRequiredControl<Canvas>(window, "Playfield");
        _titleOverlay = FindRequiredControl<Border>(window, "TitleOverlay");
        _scoreText = FindRequiredControl<TextBlock>(window, "ScoreText");
        _livesText = FindRequiredControl<TextBlock>(window, "LivesText");
        _gameTitle = FindRequiredControl<TextBlock>(window, "GameTitle");
        _creditsText = FindRequiredControl<TextBlock>(window, "CreditsText");
        _pressStartText = FindRequiredControl<TextBlock>(window, "PressStartText");
        _hintText = FindRequiredControl<TextBlock>(window, "HintText");

        _player = new Rectangle
        {
            Width = PlayerWidth,
            Height = PlayerHeight,
            Fill = _playerBrush
        };

        _playfield.Children.Add(_player);
        Canvas.SetLeft(_player, PlayerInitialLeft);
        Canvas.SetTop(_player, PlayerInitialTop);

        window.KeyDown += OnKeyDown;
        window.KeyUp += OnKeyUp;
        window.Closed += (_, _) => Dispose();

        window.Opened += (_, _) =>
        {
            ShowTitleOverlay();
            _ = _playfield.Focus();
        };
    }

    /// <summary>Gets the scheduler used for UI updates.</summary>
    private static AvaloniaScheduler UiScheduler => AvaloniaScheduler.Instance;

    /// <summary>Releases managed resources.</summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _overlay?.Dispose();
        _game.Dispose();
        _lifetime.Dispose();

        _window.KeyDown -= OnKeyDown;
        _window.KeyUp -= OnKeyUp;
    }

    /// <summary>Writes animation errors to diagnostics output.</summary>
    /// <param name="ex">The exception to write.</param>
    private static void HandleError(Exception ex) => Debug.WriteLine(ex);

    /// <summary>Finds a required named control.</summary>
    /// <typeparam name="T">The control type.</typeparam>
    /// <param name="window">The window to search.</param>
    /// <param name="name">The control name.</param>
    /// <returns>The matching control.</returns>
    /// <exception cref="InvalidOperationException">The control was not found.</exception>
    private static T FindRequiredControl<T>(Window window, string name)
        where T : Control =>
        window.FindControl<T>(name) ?? throw new InvalidOperationException($"{name} not found");

    /// <summary>Returns a non-negative cryptographic random integer below the specified maximum.</summary>
    /// <param name="maxExclusive">The exclusive upper bound.</param>
    /// <returns>The generated integer.</returns>
    private static int NextRandomInt32(int maxExclusive)
    {
        if (maxExclusive <= 1)
        {
            return 0;
        }

        var bytes = new byte[sizeof(uint)];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(bytes);
        var value = BitConverter.ToUInt32(bytes, 0);
        return (int)(value % (uint)maxExclusive);
    }

    /// <summary>Creates clamped frame deltas from render frames.</summary>
    /// <param name="frame">The render frame stream.</param>
    /// <returns>The frame delta stream.</returns>
    private static IObservable<double> CreateFrameDeltas(IObservable<long> frame) =>
        frame.Timestamp(UiScheduler)
            .Scan((last: DateTimeOffset.Now, delta: InitialFrameDeltaSeconds), (state, value) =>
            {
                var delta = ClampFrameDelta(value.Timestamp, state.last);
                return (last: value.Timestamp, delta);
            })
            .Select(state => state.delta);

    /// <summary>Clamps a frame delta to the valid movement range.</summary>
    /// <param name="current">The current timestamp.</param>
    /// <param name="previous">The previous timestamp.</param>
    /// <returns>The clamped delta.</returns>
    private static double ClampFrameDelta(DateTimeOffset current, DateTimeOffset previous)
    {
        var delta = (current - previous).TotalSeconds;
        return delta < 0 ? 0 : Math.Min(delta, MaxFrameDeltaSeconds);
    }

    /// <summary>Clamps a position to the playfield.</summary>
    /// <param name="value">The position value.</param>
    /// <param name="maximum">The maximum position value.</param>
    /// <returns>The clamped position.</returns>
    private static double ClampPosition(double value, double maximum) =>
        Math.Max(0, Math.Min(value, Math.Max(0, maximum)));

    /// <summary>Handles key-down input.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The key event data.</param>
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter or Key.R:
            {
                if (!_isRunning)
                {
                    StartGame();
                    e.Handled = true;
                }

                break;
            }

            case Key.Space:
            {
                if (_isRunning)
                {
                    FireBullet();
                    e.Handled = true;
                }

                break;
            }

            case Key.Left or Key.A:
            {
                _horizontalInput = NegativeInput;
                break;
            }

            case Key.Right or Key.D:
            {
                _horizontalInput = PositiveInput;
                break;
            }

            case Key.Up:
            {
                _verticalInput = NegativeInput;
                break;
            }

            case Key.Down:
            {
                _verticalInput = PositiveInput;
                break;
            }

            default:
            {
                break;
            }
        }
    }

    /// <summary>Handles key-up input.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The key event data.</param>
    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left or Key.A or Key.Right or Key.D:
            {
                _horizontalInput = NoInput;
                break;
            }

            case Key.Up or Key.Down:
            {
                _verticalInput = NoInput;
                break;
            }

            default:
            {
                break;
            }
        }
    }

    /// <summary>Shows the title overlay.</summary>
    private void ShowTitleOverlay()
    {
        _overlay?.Dispose();
        _overlay = [];

        _titleOverlay.IsVisible = true;
        _titleOverlay.Opacity = 0;
        _gameTitle.Opacity = 0;
        _creditsText.Opacity = 0;
        _pressStartText.Opacity = 0;
        _hintText.Opacity = 0;

        _ = _titleOverlay.OpacityTo(TitleFadeMilliseconds, 1, Ease.SineInOut)
            .Concat(_gameTitle.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Concat(_creditsText.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Concat(_pressStartText.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Concat(_hintText.OpacityTo(HintFadeMilliseconds, 1, Ease.SineOut))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);

        _ = Observable.Interval(TimeSpan.FromMilliseconds(PressStartPulseIntervalMilliseconds))
            .SelectMany(_ => _pressStartText.OpacityTo(PressStartPulseMilliseconds, PressStartDimOpacity, Ease.SineOut)
                .Concat(_pressStartText.OpacityTo(PressStartPulseMilliseconds, 1.0, Ease.SineIn)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);
    }

    /// <summary>Hides the title overlay.</summary>
    private void HideTitleOverlay()
    {
        _overlay?.Dispose();
        _overlay = null;

        _ = _titleOverlay.OpacityTo(OverlayHideMilliseconds, 0, Ease.SineInOut)
            .ObserveOn(UiScheduler)
            .Subscribe(
                _ => { },
                HandleError,
                () => _titleOverlay.IsVisible = false);
    }

    /// <summary>Starts a new game run.</summary>
    private void StartGame()
    {
        _game.Dispose();
        _game = [];

        foreach (var r in _bullets)
        {
            _ = _playfield.Children.Remove(r);
        }

        _bullets.Clear();

        foreach (var r in _enemies)
        {
            _ = _playfield.Children.Remove(r);
        }

        _enemies.Clear();

        if (!_playfield.Children.Contains(_player))
        {
            _playfield.Children.Add(_player);
        }

        _player.Opacity = 1;

        _score = 0;
        _lives = InitialLives;
        _scoreText.Text = "Score: 0";
        _livesText.Text = "Lives: 3";

        HideTitleOverlay();

        var sz = _playfield.Bounds.Size;
        if (sz.Width > 0 && sz.Height > 0)
        {
            InitGame();
        }
        else
        {
            _ = Observable
                .FromEventPattern<EventHandler<AvaloniaPropertyChangedEventArgs>, AvaloniaPropertyChangedEventArgs>(
                    handler => _playfield.PropertyChanged += handler,
                    handler => _playfield.PropertyChanged -= handler)
                .Select(_ => _playfield.Bounds.Size)
                .Where(s => s.Width > 0 && s.Height > 0)
                .Take(1)
                .ObserveOn(UiScheduler)
                .Subscribe(_ => InitGame(), HandleError)
                .DisposeWith(_game);
        }

        _isRunning = true;
        _ = _playfield.Focus();
    }

    /// <summary>Initializes the game loop and per-run animations.</summary>
    private void InitGame()
    {
        var frame = Animations.AnimateFrame(FramesPerSecond);
        var frameDt = CreateFrameDeltas(frame);

        SubscribePlayerMovement(frameDt);
        SubscribeEnemySpawns();
        SubscribeFrameUpdates(frame, frameDt);
        SubscribeIdleBreathing();
    }

    /// <summary>Subscribes the player movement loop.</summary>
    /// <param name="frameDt">The frame delta stream.</param>
    private void SubscribePlayerMovement(IObservable<double> frameDt)
    {
        _ = frameDt
            .ObserveOn(UiScheduler)
            .Subscribe(
                MovePlayer,
                HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Subscribes enemy spawning.</summary>
    private void SubscribeEnemySpawns()
    {
        _ = Observable.Interval(TimeSpan.FromMilliseconds(EnemySpawnIntervalMilliseconds))
            .ObserveOn(UiScheduler)
            .Do(_ => SpawnEnemy())
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Subscribes per-frame game updates.</summary>
    /// <param name="frame">The render frame stream.</param>
    /// <param name="frameDt">The frame delta stream.</param>
    private void SubscribeFrameUpdates(IObservable<long> frame, IObservable<double> frameDt)
    {
        _ = frameDt
            .ObserveOn(UiScheduler)
            .Subscribe(UpdateBullets, HandleError)
            .DisposeWith(_game);

        _ = frameDt
            .ObserveOn(UiScheduler)
            .Subscribe(UpdateEnemies, HandleError)
            .DisposeWith(_game);

        _ = frame
            .ObserveOn(UiScheduler)
            .Subscribe(_ => CheckCollisions(), HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Subscribes player idle breathing animation.</summary>
    private void SubscribeIdleBreathing()
    {
        _ = Observable.Interval(TimeSpan.FromSeconds(IdleBreathingIntervalSeconds))
            .SelectMany(_ => _player.ScaleTo(
                    IdleBreathingMilliseconds,
                    IdleBreathingScale,
                    IdleBreathingScale,
                    Ease.SineInOut,
                    Ease.SineInOut)
                .Concat(_player.ScaleTo(
                    IdleBreathingMilliseconds,
                    1.0,
                    1.0,
                    Ease.SineInOut,
                    Ease.SineInOut)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Moves the player using the current input state.</summary>
    /// <param name="dt">The frame delta in seconds.</param>
    private void MovePlayer(double dt)
    {
        var width = _playfield.Bounds.Width;
        var height = _playfield.Bounds.Height;
        var x = Canvas.GetLeft(_player) + (_horizontalInput * PlayerSpeed * dt);
        var y = Canvas.GetTop(_player) + (_verticalInput * PlayerSpeed * dt);

        Canvas.SetLeft(_player, ClampPosition(x, width - _player.Width));
        Canvas.SetTop(_player, ClampPosition(y, height - _player.Height));
    }

    /// <summary>Fires a bullet from the player position.</summary>
    private void FireBullet()
    {
        var b = new Rectangle
        {
            Width = BulletWidth,
            Height = BulletHeight,
            Fill = Brushes.OrangeRed
        };

        var px = Canvas.GetLeft(_player);
        var py = Canvas.GetTop(_player);

        var x = px + _player.Width;
        var y = py + (_player.Height / CenterDivisor) - BulletVerticalOffset;

        _playfield.Children.Add(b);
        Canvas.SetLeft(b, x);
        Canvas.SetTop(b, y);
        _bullets.Add(b);

        _ = _player.ShakeTranslate(
                PlayerShakeMilliseconds,
                PlayerShakeDistance,
                shakes: PlayerShakeCount,
                ease: Ease.SineOut)
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Spawns an enemy at the right edge of the playfield.</summary>
    private void SpawnEnemy()
    {
        var pfw = _playfield.Bounds.Width;
        var pfh = _playfield.Bounds.Height;
        if (pfw <= MinimumEnemyPlayfieldWidth || pfh <= MinimumEnemyPlayfieldHeight)
        {
            return;
        }

        var e = new Rectangle
        {
            Width = EnemyWidth,
            Height = EnemyHeight,
            Fill = Brushes.LimeGreen
        };

        var left = Math.Max(0, pfw - e.Width - EnemyRightPadding);
        var maxTop = Math.Max(0, pfh - e.Height);
        var top = maxTop <= 0
            ? 0
            : NextRandomInt32(Math.Max(1, (int)Math.Ceiling(maxTop)));

        _playfield.Children.Add(e);
        Canvas.SetLeft(e, left);
        Canvas.SetTop(e, top);
        _enemies.Add(e);
    }

    /// <summary>Updates active bullet positions.</summary>
    /// <param name="dt">The frame delta in seconds.</param>
    private void UpdateBullets(double dt)
    {
        var speed = BulletSpeed * dt;
        for (var i = _bullets.Count - 1; i >= 0; i--)
        {
            var b = _bullets[i];
            var x = Canvas.GetLeft(b) + speed;
            Canvas.SetLeft(b, x);
            if (x > _playfield.Bounds.Width)
            {
                _ = _playfield.Children.Remove(b);
                _bullets.RemoveAt(i);
            }
        }
    }

    /// <summary>Updates active enemy positions.</summary>
    /// <param name="dt">The frame delta in seconds.</param>
    private void UpdateEnemies(double dt)
    {
        var speed = EnemySpeed * dt;
        for (var i = _enemies.Count - 1; i >= 0; i--)
        {
            var e = _enemies[i];
            var x = Canvas.GetLeft(e) - speed;
            Canvas.SetLeft(e, x);
            if (x < -e.Width)
            {
                _ = _playfield.Children.Remove(e);
                _enemies.RemoveAt(i);
                LoseLife();
            }
        }
    }

    /// <summary>Checks bullet and enemy collisions.</summary>
    private void CheckCollisions()
    {
        var toRemoveEnemies = new List<Rectangle>();
        var toRemoveBullets = new List<Rectangle>();

        foreach (var b in _bullets)
        {
            var bx = Canvas.GetLeft(b);
            var by = Canvas.GetTop(b);
            var bb = new Rect(bx, by, b.Width, b.Height);

            foreach (var e in _enemies)
            {
                var ex = Canvas.GetLeft(e);
                var ey = Canvas.GetTop(e);
                var eb = new Rect(ex, ey, e.Width, e.Height);

                if (bb.Intersects(eb))
                {
                    toRemoveEnemies.Add(e);
                    toRemoveBullets.Add(b);

                    _score += EnemyHitScore;
                    _scoreText.Text = $"Score: {_score}";

                    if (e.Fill is SolidColorBrush b1)
                    {
                        _ = b1.ColorTo(HitFlashMilliseconds, Colors.Yellow, Ease.ExpoOut)
                            .Concat(b1.ColorTo(HitFlashMilliseconds, Colors.Black, Ease.ExpoOut))
                            .ObserveOn(UiScheduler)
                            .Subscribe(
                                _ => { },
                                HandleError,
                                () => _ = _playfield.Children.Remove(e));
                    }
                    else
                    {
                        _ = _playfield.Children.Remove(e);
                    }
                }
            }
        }

        foreach (var e in toRemoveEnemies)
        {
            _ = _enemies.Remove(e);
        }

        foreach (var b in toRemoveBullets)
        {
            _ = _playfield.Children.Remove(b);
            _ = _bullets.Remove(b);
        }
    }

    /// <summary>Removes a life and ends the game when none remain.</summary>
    private void LoseLife()
    {
        _lives--;
        _livesText.Text = $"Lives: {_lives}";

        _ = _player.PulseOpacity(
                LifePulseMilliseconds,
                LifePulseLowOpacity,
                1.0,
                pulses: LifePulseCount,
                ease: Ease.SineInOut)
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);

        if (_lives > 0)
        {
            return;
        }

        GameOver();
    }

    /// <summary>Ends the current game run.</summary>
    private void GameOver()
    {
        _isRunning = false;
        _game.Dispose();
        _game = [];

        _window.Title = "Game Over - Press Enter to Restart";
        ShowTitleOverlay();
    }
}
