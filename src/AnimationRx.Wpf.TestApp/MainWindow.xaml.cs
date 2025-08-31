// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CP.AnimationRx;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace CP.Animation.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        // Speeds (pixels per second)
        private const double PlayerSpeed = 260.0;
        private const double BulletSpeed = 480.0;
        private const double EnemySpeed = 120.0;

        // Readonly fields first (SA1214)
        private readonly Rectangle _player;
        private readonly SolidColorBrush _playerBrush = new(Colors.DeepSkyBlue);
        private readonly List<Rectangle> _bullets = [];
        private readonly List<Rectangle> _enemies = [];
        private readonly Random _rng = new();
        private readonly CompositeDisposable _lifetime = []; // window lifetime subscriptions (preview keys, load)

        // Non-readonly fields
        private CompositeDisposable _game = []; // per-game subscriptions
        private CompositeDisposable? _overlay; // title overlay animations
        private int _score;
        private int _lives = 3;
        private bool _disposedValue;
        private bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Player
            _player = new Rectangle
            {
                Width = 40,
                Height = 14,
                Fill = _playerBrush
            };

            Playfield.Children.Add(_player);
            Canvas.SetLeft(_player, 100);
            Canvas.SetTop(_player, 420);

            // Global key handling: start and fire (lifetime)
            this.Events().PreviewKeyDown
                .Subscribe(HandleOnPreviewKeyDown, HandleError)
                .DisposeWith(_lifetime);

            // Show animated title overlay on load (lifetime)
            this.Events().Loaded
                .Do(_ => ShowTitleOverlay())
                .Subscribe(_ => Playfield.Focus(), HandleError)
                .DisposeWith(_lifetime);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">True to dispose managed resources, otherwise false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _overlay?.Dispose();
                    _game.Dispose();
                    _lifetime.Dispose();
                }

                _disposedValue = true;
            }
        }

        private static void HandleError(Exception ex) => Debug.WriteLine(ex);

        private void HandleOnPreviewKeyDown(KeyEventArgs e)
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
        }

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
            TitleOverlay.OpacityTo(600, 1, Ease.SineInOut)
                .Concat(GameTitle.OpacityTo(400, 1, Ease.SineOut))
                .Concat(CreditsText.OpacityTo(400, 1, Ease.SineOut))
                .Concat(PressStartText.OpacityTo(400, 1, Ease.SineOut))
                .Subscribe(_ => { }, HandleError)
                .DisposeWith(_overlay);

            Observable.Interval(TimeSpan.FromMilliseconds(900))
                .SelectMany(_ => PressStartText.OpacityTo(200, 0.3, Ease.SineOut)
                    .Concat(PressStartText.OpacityTo(200, 1.0, Ease.SineIn)))
                .Subscribe(_ => { }, HandleError)
                .DisposeWith(_overlay);
        }

        private void HideTitleOverlay()
        {
            // stop pulsing animations
            _overlay?.Dispose();
            _overlay = null;

            // Fade out overlay quickly then collapse
            TitleOverlay.OpacityTo(250, 0, Ease.SineInOut)
                .Subscribe(_ => TitleOverlay.Visibility = Visibility.Collapsed, HandleError);
        }

        private void StartGame()
        {
            // Dispose any previous game subscriptions and reset
            _game.Dispose();
            _game = [];

            // Clear playfield except HUD and overlay
            foreach (var r in _bullets.ToList())
            {
                Playfield.Children.Remove(r);
            }

            _bullets.Clear();

            foreach (var r in _enemies.ToList())
            {
                Playfield.Children.Remove(r);
            }

            _enemies.Clear();

            // Ensure player exists and is visible
            if (!Playfield.Children.Contains(_player))
            {
                Playfield.Children.Add(_player);
            }

            _player.Opacity = 1;

            // Reset score/lives
            _score = 0;
            _lives = 3;
            ScoreText.Text = $"Score: {_score}";
            LivesText.Text = $"Lives: {_lives}";

            // Hide title
            HideTitleOverlay();

            // Wait until Playfield has a valid size then init game loop
            Playfield.Events().SizeChanged
                .Select(_ => new Size(Playfield.ActualWidth, Playfield.ActualHeight))
                .StartWith(new Size(Playfield.ActualWidth, Playfield.ActualHeight))
                .Where(sz => sz.Width > 0 && sz.Height > 0)
                .Take(1)
                .Subscribe(_ => InitGame(), HandleError)
                .DisposeWith(_game);

            _isRunning = true;
            Playfield.Focus();
        }

        private void InitGame()
        {
            // Input
            var keyDown = Playfield.Events().KeyDown.Select(e => e.Key);
            var keyUp = Playfield.Events().KeyUp.Select(e => e.Key);
            var keys = keyDown.Merge(keyUp).Select(_ => Keyboard.Modifiers);

            var left = Playfield.Events().KeyDown.Where(e => e.Key == Key.Left || e.Key == Key.A).Select(_ => -0.5);
            var right = Playfield.Events().KeyDown.Where(e => e.Key == Key.Right || e.Key == Key.D).Select(_ => 0.5);
            var stop = Playfield.Events().KeyUp.Where(e => e.Key == Key.Left || e.Key == Key.A || e.Key == Key.Right || e.Key == Key.D).Select(_ => 0.0);
            var horiz = left.Merge(right).Merge(stop).StartWith(0).DistinctUntilChanged();

            var up = Playfield.Events().KeyDown.Where(e => e.Key == Key.Up).Select(_ => -0.5);
            var down = Playfield.Events().KeyDown.Where(e => e.Key == Key.Down).Select(_ => 0.5);
            var vstop = Playfield.Events().KeyUp.Where(e => e.Key == Key.Up || e.Key == Key.Down).Select(_ => 0.0);
            var vert = up.Merge(down).Merge(vstop).StartWith(0).DistinctUntilChanged();

            // Game loop ~render rate
            var frame = Animations.RenderFrames();

            // Build a dt stream (seconds) with clamping and optional rate limiting to avoid too-fast updates
            var frameDt = frame
                .Timestamp(RxApp.MainThreadScheduler)
                .Scan(new { last = DateTimeOffset.Now, dt = 0.016 }, (acc, t) =>
                {
                    var now = t.Timestamp;
                    var dt = (now - acc.last).TotalSeconds;
                    if (dt < 0)
                    {
                        dt = 0; // clock skew guard
                    }

                    if (dt > 0.05)
                    {
                        dt = 0.05; // clamp to avoid huge jumps (50ms)
                    }

                    return new { last = now, dt };
                })
                .Select(s => s.dt);

            // Move player horizontally (time-based)
            frame.WithLatestFrom(horiz, (_, h) => h)
                .Timestamp(RxApp.MainThreadScheduler)
                .Scan(new { last = DateTimeOffset.Now, x = 100.0 }, (acc, h) =>
                {
                    var now = h.Timestamp;
                    var dt = (now - acc.last).TotalSeconds;
                    if (dt < 0)
                    {
                        dt = 0;
                    }

                    if (dt > 0.05)
                    {
                        dt = 0.05;
                    }

                    var nx = acc.x + (h.Value * PlayerSpeed * dt);
                    var clamped = Math.Max(0, Math.Min(nx, Math.Max(0, Playfield.ActualWidth - _player.Width)));
                    return new { last = now, x = clamped };
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(state => Canvas.SetLeft(_player, state.x))
                .Subscribe(_ => { }, HandleError)
                .DisposeWith(_game);

            // Move player vertically (time-based via Up/Down keys)
            var initialTop = Canvas.GetTop(_player);
            if (double.IsNaN(initialTop))
            {
                initialTop = 420;
            }

            frame.WithLatestFrom(vert, (_, v) => v)
                .Timestamp(RxApp.MainThreadScheduler)
                .Scan(new { last = DateTimeOffset.Now, y = initialTop }, (acc, v) =>
                {
                    var now = v.Timestamp;
                    var dt = (now - acc.last).TotalSeconds;
                    if (dt < 0)
                    {
                        dt = 0;
                    }

                    if (dt > 0.05)
                    {
                        dt = 0.05;
                    }

                    var ny = acc.y + (v.Value * PlayerSpeed * dt);
                    var clampedY = Math.Max(0, Math.Min(ny, Math.Max(0, Playfield.ActualHeight - _player.Height)));
                    return new { last = now, y = clampedY };
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(state => Canvas.SetTop(_player, state.y))
                .Subscribe(_ => { }, HandleError)
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
                .SelectMany(_ => _player.ScaleTransform(800, 1.15, 1.15, Ease.SineInOut, Ease.SineInOut)
                    .Concat(_player.ScaleTransform(800, 1.0, 1.0, Ease.SineInOut, Ease.SineInOut)))
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
            if (double.IsNaN(px) || double.IsNaN(py))
            {
                return;
            }

            var x = px + _player.Width;
            var y = py + (_player.Height / 2) - 1;
            Playfield.Children.Add(b);
            Canvas.SetLeft(b, x);
            Canvas.SetTop(b, y);
            _bullets.Add(b);
        }

        private void SpawnEnemy()
        {
            var pfw = Playfield.ActualWidth;
            var pfh = Playfield.ActualHeight;
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

            Playfield.Children.Add(e);
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
                if (x > Playfield.ActualWidth)
                {
                    Playfield.Children.Remove(b);
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
                    Playfield.Children.Remove(e);
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
                    if (bb.IntersectsWith(eb))
                    {
                        toRemoveEnemies.Add(e);
                        toRemoveBullets.Add(b);
                        _score += 10;
                        ScoreText.Text = $"Score: {_score}";

                        var baseBrush = e.Fill as SolidColorBrush ?? new SolidColorBrush(Colors.LimeGreen);
                        var modBrush = baseBrush.IsFrozen ? baseBrush.Clone() : baseBrush;
                        e.Fill = modBrush;
                        modBrush.BrushColorTo(150, Colors.Yellow, Ease.ExpoOut)
                             .Concat(modBrush.BrushColorTo(150, Colors.Black, Ease.ExpoOut))
                             .ObserveOn(RxApp.MainThreadScheduler)
                             .Subscribe(
                                 _ => { },
                                 HandleError,
                                 () => Playfield.Children.Remove(e));
                    }
                }
            }

            foreach (var e in toRemoveEnemies.Distinct())
            {
                _enemies.Remove(e);
            }

            foreach (var b in toRemoveBullets.Distinct())
            {
                Playfield.Children.Remove(b);
                _bullets.Remove(b);
            }
        }

        private void LoseLife()
        {
            _lives--;
            LivesText.Text = $"Lives: {_lives}";

            _player.OpacityTo(80, 0.3, Ease.SineOut)
                   .Concat(_player.OpacityTo(80, 1.0, Ease.SineIn))
                   .Repeat(3)
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

            // stop spawns/updates for this run
            _game.Dispose();
            _game = [];

            // show title overlay again with a message
            Title = "Game Over - Press Enter to Restart";
            ShowTitleOverlay();
        }
    }
}
