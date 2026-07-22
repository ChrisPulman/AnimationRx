// Copyright (c) 2022-2026 Chris Pulman. All rights reserved.
// Chris Pulman licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using CP.AnimationRx;

namespace CP.Animation.TestApp;

/// <summary>Interaction logic for MainWindow.xaml.</summary>
public partial class MainWindow : IDisposable
{
    /// <summary>Defines the player movement speed in pixels per second.</summary>
    private const double PlayerSpeed = 260.0;

    /// <summary>Defines the bullet movement speed in pixels per second.</summary>
    private const double BulletSpeed = 480.0;

    /// <summary>Defines the enemy movement speed in pixels per second.</summary>
    private const double EnemySpeed = 120.0;

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

    /// <summary>Defines the life pulse duration in milliseconds.</summary>
    private const double LifePulseMilliseconds = 80.0;

    /// <summary>Defines the low opacity used by the life pulse.</summary>
    private const double LifePulseLowOpacity = 0.3;

    /// <summary>Defines the number of life pulses.</summary>
    private const int LifePulseCount = 3;

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

    /// <summary>Stores whether this instance has been disposed.</summary>
    private bool _disposedValue;

    /// <summary>Stores whether a game is running.</summary>
    private bool _isRunning;

    /// <summary>Initializes a new instance of the <see cref="MainWindow"/> class.</summary>
    public MainWindow()
    {
        InitializeComponent();

        // Player
        _player = new Rectangle
        {
            Width = PlayerWidth,
            Height = PlayerHeight,
            Fill = _playerBrush
        };

        _ = Playfield.Children.Add(_player);
        Canvas.SetLeft(_player, PlayerInitialLeft);
        Canvas.SetTop(_player, PlayerInitialTop);

        // Global key handling: start and fire (lifetime)
        _ = PreviewKeyDownEvents()
            .Subscribe(HandleOnPreviewKeyDown, HandleError)
            .DisposeWith(_lifetime);

        // Show animated title overlay on load (lifetime)
        _ = LoadedEvents()
            .Do(_ => ShowTitleOverlay())
            .Subscribe(_ => Playfield.Focus(), HandleError)
            .DisposeWith(_lifetime);
    }

    /// <summary>Gets the scheduler used for UI updates.</summary>
    private static SynchronizationContextScheduler UiScheduler => CreateUiScheduler();

    /// <summary>Releases managed resources.</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases unmanaged and optionally managed resources.</summary>
    /// <param name="disposing">True to dispose managed resources, otherwise false.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _overlay?.Dispose();
            _game.Dispose();
            _lifetime.Dispose();
        }

        _disposedValue = true;
    }

    /// <summary>Creates a scheduler for the current WPF dispatcher.</summary>
    /// <returns>The dispatcher-backed scheduler.</returns>
    private static SynchronizationContextScheduler CreateUiScheduler()
    {
        var dispatcher = Application.Current?.Dispatcher
            ?? Dispatcher.FromThread(Thread.CurrentThread)
            ?? Dispatcher.CurrentDispatcher;

        return new(new DispatcherSynchronizationContext(dispatcher));
    }

    /// <summary>Writes animation errors to diagnostics output.</summary>
    /// <param name="ex">The exception to write.</param>
    private static void HandleError(Exception ex) => Debug.WriteLine(ex);

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

    /// <summary>Returns whether the key moves left.</summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True when the key moves left.</returns>
    private static bool IsLeftMovementKey(Key key) => key is Key.Left or Key.A;

    /// <summary>Returns whether the key moves right.</summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True when the key moves right.</returns>
    private static bool IsRightMovementKey(Key key) => key is Key.Right or Key.D;

    /// <summary>Returns whether the key moves horizontally.</summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True when the key moves horizontally.</returns>
    private static bool IsHorizontalMovementKey(Key key) => IsLeftMovementKey(key) || IsRightMovementKey(key);

    /// <summary>Handles preview key-down input.</summary>
    /// <param name="e">The key event data.</param>
    private void HandleOnPreviewKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter or Key.R when !_isRunning:
            {
                StartGame();
                e.Handled = true;
                break;
            }

            case Key.Space when _isRunning:
            {
                FireBullet();
                e.Handled = true;
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

        TitleOverlay.Visibility = Visibility.Visible;
        TitleOverlay.Opacity = 0;
        GameTitle.Opacity = 0;
        CreditsText.Opacity = 0;
        PressStartText.Opacity = 0;

        // Simple fade-in and pulse for press start
        _ = TitleOverlay.OpacityTo(TitleFadeMilliseconds, 1, Ease.SineInOut)
            .Concat(GameTitle.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Concat(CreditsText.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Concat(PressStartText.OpacityTo(OverlayTextFadeMilliseconds, 1, Ease.SineOut))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);

        _ = Observable.Interval(TimeSpan.FromMilliseconds(PressStartPulseIntervalMilliseconds))
            .SelectMany(_ => PressStartText.OpacityTo(PressStartPulseMilliseconds, PressStartDimOpacity, Ease.SineOut)
                .Concat(PressStartText.OpacityTo(PressStartPulseMilliseconds, 1.0, Ease.SineIn)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);
    }

    /// <summary>Hides the title overlay.</summary>
    private void HideTitleOverlay()
    {
        // stop pulsing animations
        _overlay?.Dispose();
        _overlay = null;

        // Fade out overlay quickly then collapse
        _ = TitleOverlay.OpacityTo(OverlayHideMilliseconds, 0, Ease.SineInOut)
            .Subscribe(_ => TitleOverlay.Visibility = Visibility.Collapsed, HandleError);
    }

    /// <summary>Starts a new game run.</summary>
    private void StartGame()
    {
        // Dispose any previous game subscriptions and reset
        _game.Dispose();
        _game = [];

        // Clear playfield except HUD and overlay
        foreach (var r in _bullets)
        {
            Playfield.Children.Remove(r);
        }

        _bullets.Clear();

        foreach (var r in _enemies)
        {
            Playfield.Children.Remove(r);
        }

        _enemies.Clear();

        // Ensure player exists and is visible
        if (!Playfield.Children.Contains(_player))
        {
            _ = Playfield.Children.Add(_player);
        }

        _player.Opacity = 1;

        // Reset score/lives
        _score = 0;
        _lives = InitialLives;
        ScoreText.Text = "Score: 0";
        LivesText.Text = "Lives: 3";

        // Hide title
        HideTitleOverlay();

        // Wait until Playfield has a valid size then init game loop
        Size initialSize = new(Playfield.ActualWidth, Playfield.ActualHeight);
        _ = PlayfieldSizeChangedEvents()
            .Select(_ => new Size(Playfield.ActualWidth, Playfield.ActualHeight))
            .StartWith(initialSize)
            .Where(sz => sz.Width > 0 && sz.Height > 0)
            .Take(1)
            .Subscribe(_ => InitGame(), HandleError)
            .DisposeWith(_game);

        _isRunning = true;
        _ = Playfield.Focus();
    }

    /// <summary>Initializes the game loop and per-run animations.</summary>
    private void InitGame()
    {
        var frame = Animations.RenderFrames();
        var frameDt = CreateFrameDeltas(frame);

        SubscribeHorizontalMovement(frame, CreateHorizontalInput());
        SubscribeVerticalMovement(frame, CreateVerticalInput());
        SubscribeEnemySpawns();
        SubscribeFrameUpdates(frame, frameDt);
        SubscribeIdleBreathing();
    }

    /// <summary>Creates the horizontal input stream.</summary>
    /// <returns>The horizontal input stream.</returns>
    private IObservable<double> CreateHorizontalInput()
    {
        var left = PlayfieldKeyDownEvents()
            .Where(e => IsLeftMovementKey(e.Key))
            .Select(_ => NegativeInput);

        var right = PlayfieldKeyDownEvents()
            .Where(e => IsRightMovementKey(e.Key))
            .Select(_ => PositiveInput);

        var stop = PlayfieldKeyUpEvents()
            .Where(e => IsHorizontalMovementKey(e.Key))
            .Select(_ => NoInput);

        return left.Merge(right).Merge(stop).StartWith(NoInput).DistinctUntilChanged();
    }

    /// <summary>Creates the vertical input stream.</summary>
    /// <returns>The vertical input stream.</returns>
    private IObservable<double> CreateVerticalInput()
    {
        var up = PlayfieldKeyDownEvents()
            .Where(e => e.Key == Key.Up)
            .Select(_ => NegativeInput);

        var down = PlayfieldKeyDownEvents()
            .Where(e => e.Key == Key.Down)
            .Select(_ => PositiveInput);

        var stop = PlayfieldKeyUpEvents()
            .Where(e => e.Key is Key.Up or Key.Down)
            .Select(_ => NoInput);

        return up.Merge(down).Merge(stop).StartWith(NoInput).DistinctUntilChanged();
    }

    /// <summary>Subscribes the horizontal movement loop.</summary>
    /// <param name="frame">The render frame stream.</param>
    /// <param name="horizontalInput">The horizontal input stream.</param>
    private void SubscribeHorizontalMovement(IObservable<long> frame, IObservable<double> horizontalInput)
    {
        _ = frame.WithLatestFrom(horizontalInput, (_, input) => input)
            .Timestamp(UiScheduler)
            .Scan((last: DateTimeOffset.Now, x: PlayerInitialLeft), (state, sample) =>
            {
                var delta = ClampFrameDelta(sample.Timestamp, state.last);
                var next = state.x + (sample.Value * PlayerSpeed * delta);
                var maximum = Playfield.ActualWidth - _player.Width;
                return (last: sample.Timestamp, x: ClampPosition(next, maximum));
            })
            .ObserveOn(UiScheduler)
            .Do(state => Canvas.SetLeft(_player, state.x))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    /// <summary>Subscribes the vertical movement loop.</summary>
    /// <param name="frame">The render frame stream.</param>
    /// <param name="verticalInput">The vertical input stream.</param>
    private void SubscribeVerticalMovement(IObservable<long> frame, IObservable<double> verticalInput)
    {
        var initialTop = Canvas.GetTop(_player);
        if (double.IsNaN(initialTop))
        {
            initialTop = PlayerInitialTop;
        }

        _ = frame.WithLatestFrom(verticalInput, (_, input) => input)
            .Timestamp(UiScheduler)
            .Scan((last: DateTimeOffset.Now, y: initialTop), (state, sample) =>
            {
                var delta = ClampFrameDelta(sample.Timestamp, state.last);
                var next = state.y + (sample.Value * PlayerSpeed * delta);
                var maximum = Playfield.ActualHeight - _player.Height;
                return (last: sample.Timestamp, y: ClampPosition(next, maximum));
            })
            .ObserveOn(UiScheduler)
            .Do(state => Canvas.SetTop(_player, state.y))
            .Subscribe(_ => { }, HandleError)
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
            .SelectMany(_ => _player.ScaleTransform(
                    IdleBreathingMilliseconds,
                    IdleBreathingScale,
                    IdleBreathingScale,
                    Ease.SineInOut,
                    Ease.SineInOut)
                .Concat(_player.ScaleTransform(
                    IdleBreathingMilliseconds,
                    1.0,
                    1.0,
                    Ease.SineInOut,
                    Ease.SineInOut)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
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
        if (double.IsNaN(px) || double.IsNaN(py))
        {
            return;
        }

        var x = px + _player.Width;
        var y = py + (_player.Height / CenterDivisor) - BulletVerticalOffset;
        _ = Playfield.Children.Add(b);
        Canvas.SetLeft(b, x);
        Canvas.SetTop(b, y);
        _bullets.Add(b);
    }

    /// <summary>Spawns an enemy at the right edge of the playfield.</summary>
    private void SpawnEnemy()
    {
        var pfw = Playfield.ActualWidth;
        var pfh = Playfield.ActualHeight;
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

        _ = Playfield.Children.Add(e);
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
            if (x > Playfield.ActualWidth)
            {
                Playfield.Children.Remove(b);
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
                Playfield.Children.Remove(e);
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
                if (bb.IntersectsWith(eb))
                {
                    toRemoveEnemies.Add(e);
                    toRemoveBullets.Add(b);
                    _score += EnemyHitScore;
                    ScoreText.Text = $"Score: {_score}";

                    var baseBrush = e.Fill as SolidColorBrush ?? new(Colors.LimeGreen);
                    var modBrush = baseBrush.IsFrozen ? baseBrush.Clone() : baseBrush;
                    e.Fill = modBrush;
                    _ = modBrush.BrushColorTo(HitFlashMilliseconds, Colors.Yellow, Ease.ExpoOut)
                         .Concat(modBrush.BrushColorTo(HitFlashMilliseconds, Colors.Black, Ease.ExpoOut))
                         .ObserveOn(UiScheduler)
                         .Subscribe(
                             _ => { },
                             HandleError,
                             () => Playfield.Children.Remove(e));
                }
            }
        }

        foreach (var e in toRemoveEnemies)
        {
            _ = _enemies.Remove(e);
        }

        foreach (var b in toRemoveBullets)
        {
            Playfield.Children.Remove(b);
            _ = _bullets.Remove(b);
        }
    }

    /// <summary>Removes a life and ends the game when none remain.</summary>
    private void LoseLife()
    {
        _lives--;
        LivesText.Text = $"Lives: {_lives}";

        _ = _player.OpacityTo(LifePulseMilliseconds, LifePulseLowOpacity, Ease.SineOut)
               .Concat(_player.OpacityTo(LifePulseMilliseconds, 1.0, Ease.SineIn))
               .RepeatAnimation(LifePulseCount)
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

        // stop spawns/updates for this run
        _game.Dispose();
        _game = [];

        // show title overlay again with a message
        Title = "Game Over - Press Enter to Restart";
        ShowTitleOverlay();
    }
}
