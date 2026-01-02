// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using CP.AnimationRx;
using ReactiveUI;

namespace AnimationRx.Avalonia.TestApp;

/// <summary>
/// Game controller for the Avalonia test app.
/// </summary>
public sealed class GameWindow : IDisposable
{
    private const double PlayerSpeed = 260.0;
    private const double BulletSpeed = 480.0;
    private const double EnemySpeed = 120.0;

    private readonly Window _window;
    private readonly Canvas _playfield;
    private readonly Border _titleOverlay;
    private readonly TextBlock _scoreText;
    private readonly TextBlock _livesText;
    private readonly TextBlock _gameTitle;
    private readonly TextBlock _creditsText;
    private readonly TextBlock _pressStartText;
    private readonly TextBlock _hintText;

    private readonly Rectangle _player;
    private readonly SolidColorBrush _playerBrush = new(Colors.DeepSkyBlue);
    private readonly List<Rectangle> _bullets = [];
    private readonly List<Rectangle> _enemies = [];
    private readonly Random _rng = new();

    private readonly CompositeDisposable _lifetime = [];
    private CompositeDisposable _game = [];
    private CompositeDisposable? _overlay;

    private int _score;
    private int _lives = 3;
    private bool _isRunning;
    private bool _disposed;

    // input state
    private double _h;
    private double _v;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameWindow"/> class.
    /// </summary>
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

        _playfield = window.FindControl<Canvas>("Playfield") ?? throw new InvalidOperationException("Playfield not found");
        _titleOverlay = window.FindControl<Border>("TitleOverlay") ?? throw new InvalidOperationException("TitleOverlay not found");
        _scoreText = window.FindControl<TextBlock>("ScoreText") ?? throw new InvalidOperationException("ScoreText not found");
        _livesText = window.FindControl<TextBlock>("LivesText") ?? throw new InvalidOperationException("LivesText not found");
        _gameTitle = window.FindControl<TextBlock>("GameTitle") ?? throw new InvalidOperationException("GameTitle not found");
        _creditsText = window.FindControl<TextBlock>("CreditsText") ?? throw new InvalidOperationException("CreditsText not found");
        _pressStartText = window.FindControl<TextBlock>("PressStartText") ?? throw new InvalidOperationException("PressStartText not found");
        _hintText = window.FindControl<TextBlock>("HintText") ?? throw new InvalidOperationException("HintText not found");

        _player = new Rectangle
        {
            Width = 40,
            Height = 14,
            Fill = _playerBrush
        };

        _playfield.Children.Add(_player);
        Canvas.SetLeft(_player, 100);
        Canvas.SetTop(_player, 420);

        window.KeyDown += OnKeyDown;
        window.KeyUp += OnKeyUp;
        window.Closed += (_, _) => Dispose();

        window.Opened += (_, _) =>
        {
            ShowTitleOverlay();
            _playfield.Focus();
        };
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
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

    private static void HandleError(Exception ex) => Debug.WriteLine(ex);

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.R)
        {
            if (!_isRunning)
            {
                StartGame();
                e.Handled = true;
            }
        }
        else if (e.Key == Key.Space)
        {
            if (_isRunning)
            {
                FireBullet();
                e.Handled = true;
            }
        }

        if (e.Key == Key.Left || e.Key == Key.A)
        {
            _h = -0.5;
        }
        else if (e.Key == Key.Right || e.Key == Key.D)
        {
            _h = 0.5;
        }

        if (e.Key == Key.Up)
        {
            _v = -0.5;
        }
        else if (e.Key == Key.Down)
        {
            _v = 0.5;
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left || e.Key == Key.A || e.Key == Key.Right || e.Key == Key.D)
        {
            _h = 0.0;
        }

        if (e.Key == Key.Up || e.Key == Key.Down)
        {
            _v = 0.0;
        }
    }

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

        _titleOverlay.OpacityTo(600, 1, Ease.SineInOut)
            .Concat(_gameTitle.OpacityTo(400, 1, Ease.SineOut))
            .Concat(_creditsText.OpacityTo(400, 1, Ease.SineOut))
            .Concat(_pressStartText.OpacityTo(400, 1, Ease.SineOut))
            .Concat(_hintText.OpacityTo(250, 1, Ease.SineOut))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);

        Observable.Interval(TimeSpan.FromMilliseconds(900))
            .SelectMany(_ => _pressStartText.OpacityTo(200, 0.3, Ease.SineOut)
                .Concat(_pressStartText.OpacityTo(200, 1.0, Ease.SineIn)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_overlay);
    }

    private void HideTitleOverlay()
    {
        _overlay?.Dispose();
        _overlay = null;

        _titleOverlay.OpacityTo(250, 0, Ease.SineInOut)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
                _ => { },
                HandleError,
                () => _titleOverlay.IsVisible = false);
    }

    private void StartGame()
    {
        _game.Dispose();
        _game = [];

        foreach (var r in _bullets.ToList())
        {
            _playfield.Children.Remove(r);
        }

        _bullets.Clear();

        foreach (var r in _enemies.ToList())
        {
            _playfield.Children.Remove(r);
        }

        _enemies.Clear();

        if (!_playfield.Children.Contains(_player))
        {
            _playfield.Children.Add(_player);
        }

        _player.Opacity = 1;

        _score = 0;
        _lives = 3;
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
            Observable.FromEventPattern<AvaloniaPropertyChangedEventArgs>(_playfield, nameof(_playfield.PropertyChanged))
                .Select(_ => _playfield.Bounds.Size)
                .Where(s => s.Width > 0 && s.Height > 0)
                .Take(1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => InitGame(), HandleError)
                .DisposeWith(_game);
        }

        _isRunning = true;
        _playfield.Focus();
    }

    private void InitGame()
    {
        var frame = Animations.AnimateFrame(60);

        var frameDt = frame
            .Timestamp(RxApp.MainThreadScheduler)
            .Scan(new { last = DateTimeOffset.Now, dt = 0.016 }, (acc, t) =>
            {
                var now = t.Timestamp;
                var dt = (now - acc.last).TotalSeconds;
                if (dt < 0)
                {
                    dt = 0;
                }

                if (dt > 0.05)
                {
                    dt = 0.05;
                }

                return new { last = now, dt };
            })
            .Select(s => s.dt);

        // Move player (time based)
        frameDt
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
            dt =>
            {
                var pfw = _playfield.Bounds.Width;
                var pfh = _playfield.Bounds.Height;

                var x = Canvas.GetLeft(_player) + (_h * PlayerSpeed * dt);
                var y = Canvas.GetTop(_player) + (_v * PlayerSpeed * dt);

                x = Math.Max(0, Math.Min(x, Math.Max(0, pfw - _player.Width)));
                y = Math.Max(0, Math.Min(y, Math.Max(0, pfh - _player.Height)));

                Canvas.SetLeft(_player, x);
                Canvas.SetTop(_player, y);
            },
            HandleError)
            .DisposeWith(_game);

        // Enemy spawn
        Observable.Interval(TimeSpan.FromMilliseconds(900))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Do(_ => SpawnEnemy())
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);

        // Bullets
        frameDt
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(dt => UpdateBullets(dt), HandleError)
            .DisposeWith(_game);

        // Enemies
        frameDt
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(dt => UpdateEnemies(dt), HandleError)
            .DisposeWith(_game);

        // Collisions
        frame
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => CheckCollisions(), HandleError)
            .DisposeWith(_game);

        // Player idle breathing
        Observable.Interval(TimeSpan.FromSeconds(2))
            .SelectMany(_ => _player.ScaleTo(800, 1.15, 1.15, Ease.SineInOut, Ease.SineInOut)
                .Concat(_player.ScaleTo(800, 1.0, 1.0, Ease.SineInOut, Ease.SineInOut)))
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    private void FireBullet()
    {
        var b = new Rectangle
        {
            Width = 6,
            Height = 2,
            Fill = Brushes.OrangeRed
        };

        var px = Canvas.GetLeft(_player);
        var py = Canvas.GetTop(_player);

        var x = px + _player.Width;
        var y = py + (_player.Height / 2) - 1;

        _playfield.Children.Add(b);
        Canvas.SetLeft(b, x);
        Canvas.SetTop(b, y);
        _bullets.Add(b);

        _player.ShakeTranslate(180, 3, shakes: 3, ease: Ease.SineOut)
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);
    }

    private void SpawnEnemy()
    {
        var pfw = _playfield.Bounds.Width;
        var pfh = _playfield.Bounds.Height;
        if (pfw <= 40 || pfh <= 20)
        {
            return;
        }

        var e = new Rectangle
        {
            Width = 30,
            Height = 12,
            Fill = Brushes.LimeGreen
        };

        var left = Math.Max(0, pfw - e.Width - 4);
        var top = _rng.NextDouble() * Math.Max(0, pfh - e.Height);

        _playfield.Children.Add(e);
        Canvas.SetLeft(e, left);
        Canvas.SetTop(e, top);
        _enemies.Add(e);
    }

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
                _playfield.Children.Remove(b);
                _bullets.RemoveAt(i);
            }
        }
    }

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
                _playfield.Children.Remove(e);
                _enemies.RemoveAt(i);
                LoseLife();
            }
        }
    }

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

                    _score += 10;
                    _scoreText.Text = $"Score: {_score}";

                    if (e.Fill is SolidColorBrush b1)
                    {
                        b1.ColorTo(150, Colors.Yellow, Ease.ExpoOut)
                            .Concat(b1.ColorTo(150, Colors.Black, Ease.ExpoOut))
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(
                                _ => { },
                                HandleError,
                                () => _playfield.Children.Remove(e));
                    }
                    else
                    {
                        _playfield.Children.Remove(e);
                    }
                }
            }
        }

        foreach (var e in toRemoveEnemies.Distinct())
        {
            _enemies.Remove(e);
        }

        foreach (var b in toRemoveBullets.Distinct())
        {
            _playfield.Children.Remove(b);
            _bullets.Remove(b);
        }
    }

    private void LoseLife()
    {
        _lives--;
        _livesText.Text = $"Lives: {_lives}";

        _player.PulseOpacity(80, 0.3, 1.0, pulses: 3, ease: Ease.SineInOut)
            .Subscribe(_ => { }, HandleError)
            .DisposeWith(_game);

        if (_lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _isRunning = false;
        _game.Dispose();
        _game = [];

        _window.Title = "Game Over - Press Enter to Restart";
        ShowTitleOverlay();
    }
}
