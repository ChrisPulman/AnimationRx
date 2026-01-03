﻿![License](https://img.shields.io/github/license/ChrisPulman/AnimationRx.svg) [![Build](https://github.com/ChrisPulman/AnimationRx/actions/workflows/BuildOnly.yml/badge.svg)](https://github.com/ChrisPulman/AnimationRx/actions/workflows/BuildOnly.yml)

### AnimationRx.Wpf
![Nuget](https://img.shields.io/nuget/dt/AnimationRx.Wpf?color=pink&style=plastic) [![Nuget](https://img.shields.io/nuget/v/AnimationRx.Wpf.svg?style=plastic)](https://www.nuget.org/packages/AnimationRx.Wpf)

### AnimationRx.Avalonia
![Nuget](https://img.shields.io/nuget/dt/AnimationRx.Avalonia?color=pink&style=plastic) [![Nuget](https://img.shields.io/nuget/v/AnimationRx.Avalonia.svg?style=plastic)](https://www.nuget.org/packages/AnimationRx.Avalonia)


# AnimationRx

Reactive, composable animation primitives for **WPF** and **Avalonia**. Extends ReactiveUI for Schedulers and Rx conventions.

AnimationRx exposes animations as `IObservable<Unit>` (for effects that complete) and value streams like `IObservable<double>` (for interpolated values). This makes animations easy to **compose**, **sequence**, **run in parallel**, **cancel** (dispose), and **bind** to reactive view-models.

- **AnimationRx.Wpf**: targets .NET Framework `4.6.2`, `4.7.2`, `4.8` and `.NET 8/9/10 (windows)`.
- **AnimationRx.Avalonia**: targets `.NET 8/9/10`.

> Cancellation model: every animation is an `IObservable`. Dispose the subscription returned by `Subscribe()` to cancel mid-flight.

---

## Packages

### AnimationRx.Wpf

NuGet:
- Package: `AnimationRx.Wpf`

Package Manager:
- `Install-Package AnimationRx.Wpf`

### AnimationRx.Avalonia

NuGet:
- Package: `AnimationRx.Avalonia`

Package Manager:
- `Install-Package AnimationRx.Avalonia`

---

## Quick start

### WPF

```csharp
using CP.AnimationRx;
using System.Reactive.Disposables;

// Fade in over 300ms using Sine ease
var d = someElement
    .OpacityTo(300, 1.0, Ease.SineInOut)
    .Subscribe();

// later => cancel if needed
// d.Dispose();
```

### Avalonia

```csharp
using CP.AnimationRx;

// Fade out over 250ms
someVisual
    .OpacityTo(250, 0.0, Ease.SineOut)
    .Subscribe();
```

---

## Core concepts

### `Duration`
Animations are driven by a `Duration` value stream, where:

- `Duration.Percent` is always in `[0..1]`
- `DurationPercentage(...)` produces the progress stream
- `EaseAnimation(...)` reshapes the progress curve
- `Distance(...)` maps progress into a delta (e.g., pixels, degrees)


### Easing
Easing is provided by:

- `Ease` enum (selects common easings)
- `Eases` extension methods (e.g., `BackIn`, `SineOut`) on `IObservable<Duration>`


### Threading & schedulers
Both libraries separate:

- **time source** (defaults to `RxSchedulers.TaskpoolScheduler`)
- **UI updates** (defaults to `RxSchedulers.MainThreadScheduler`)

All built-in UI animations marshal reads/writes to the UI thread.

---

## API reference

In the sections below:

- `rx<T>` means `IObservable<T>`
- methods returning `rx<Unit>` represent animations that complete

---

# AnimationRx.Wpf API

Namespace: `CP.AnimationRx`

## Setup

Add these usings:

```csharp
using CP.AnimationRx;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
```

---

## Frames & timing (WPF)

### `Animations.AnimateFrame(double framesPerSecond, IScheduler? scheduler = null)` -> `rx<long>`
Emits an increasing tick at approximately the requested FPS.

```csharp
var frames = Animations.AnimateFrame(60)
    .Subscribe(i => Debug.WriteLine($"tick {i}"));
```

### `Animations.AnimateFrame(TimeSpan period, IScheduler? scheduler = null)` -> `rx<long>`
Emits ticks on a fixed period.

```csharp
var frames = Animations.AnimateFrame(TimeSpan.FromMilliseconds(33))
    .Subscribe(i => Debug.WriteLine(i));
```

### `Animations.RenderFrames()` -> `rx<long>`
Emits one tick per WPF `CompositionTarget.Rendering` (synchronized to the UI rendering loop).

```csharp
var renderLoop = Animations.RenderFrames()
    .Subscribe(_ =>
    {
        // do per-render work here
    });
```

### `Animations.MilliSecondsElapsed(IScheduler scheduler)` -> `rx<double>`
Emits elapsed milliseconds since subscription (starting with `0.0`).

```csharp
var ms = Animations.MilliSecondsElapsed(RxApp.TaskpoolScheduler)
    .Subscribe(t => Debug.WriteLine($"elapsed {t:0}ms"));
```

### `Animations.DurationPercentage(double milliSeconds, IScheduler? scheduler = null)` -> `rx<Duration>`
Produces progress from `0..1` over the duration, always emitting a final `1.0` and then completing.

```csharp
Animations.DurationPercentage(500)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Animations.DurationPercentage(rx<double> milliSeconds, IScheduler? scheduler = null)` -> `rx<Duration>`
Same as above, but the duration can change dynamically.

```csharp
var durationMs = Observable.Return(750.0);
Animations.DurationPercentage(durationMs)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Animations.ToDuration(this rx<double>)` -> `rx<Duration>`
Maps raw `[0..1]` values to `Duration`.

```csharp
Observable.Return(0.25)
    .Concat(Observable.Return(0.5))
    .Concat(Observable.Return(1.0))
    .ToDuration()
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Animations.TakeOneEvery<T>(this rx<T>, TimeSpan interval, IScheduler? scheduler = null)` -> `rx<T>`
Back-pressure helper that delays each element by `interval` and concatenates (one “in flight” at a time).

```csharp
var throttled = Observable.Range(1, 5)
    .TakeOneEvery(TimeSpan.FromMilliseconds(200));

throttled.Subscribe(Console.WriteLine);
```

---

## Easing (WPF)

### `Ease` enum
Use `Ease.None` for linear or pick from:
`BackIn`, `BackOut`, `BackInOut`, `BounceIn`, `BounceOut`, `BounceInOut`, `CircIn`, `CircOut`, `CircInOut`, `CubicIn`, `CubicOut`, `CubicInOut`, `ElasticIn`, `ElasticOut`, `ElasticInOut`, `ExpoIn`, `ExpoOut`, `ExpoInOut`, `QuadIn`, `QuadOut`, `QuadInOut`, `QuarticIn`, `QuarticOut`, `QuarticInOut`, `QuinticIn`, `QuinticOut`, `QuinticInOut`, `SineIn`, `SineOut`, `SineInOut`.

### `Eases.EaseAnimation(this rx<Duration> progress, Ease ease)` -> `rx<Duration>`
Applies the selected ease to a duration stream.

```csharp
Animations.DurationPercentage(400)
    .EaseAnimation(Ease.ExpoOut)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### Specific easing functions on `rx<Duration>`
Each method below reshapes `Duration.Percent`:

- `BackIn()`, `BackOut()`, `BackInOut()`
- `BounceIn()`, `BounceOut()`, `BounceInOut()`
- `CircIn()`, `CircOut()`, `CircInOut()`
- `CubicIn()`, `CubicOut()`, `CubicInOut()`
- `ElasticIn()`, `ElasticOut()`, `ElasticInOut()`
- `ExpoIn()`, `ExpoOut()`, `ExpoInOut()`
- `QuadIn()`, `QuadOut()`, `QuadInOut()`
- `QuarticIn()`, `QuarticOut()`, `QuarticInOut()`
- `QuinticIn()`, `QuinticOut()`, `QuinticInOut()`
- `SineIn()`, `SineOut()`, `SineInOut()`

Example of using a specific easing method directly:

```csharp
Animations.DurationPercentage(300)
    .SineInOut()
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

---

## Value animation (WPF)

### `Animations.AnimateValue(double ms, double from, double to, Ease ease = Ease.None, IScheduler? scheduler = null)` -> `rx<double>`
Creates a numeric interpolation stream from `from` to `to`.

```csharp
Animations.AnimateValue(600, from: 0, to: 100, Ease.SineInOut)
    .ObserveOn(RxApp.MainThreadScheduler)
    .Subscribe(v => viewModel.Progress = v);
```

### `Animations.Distance(this rx<Duration> progress, double distance)` -> `rx<double>`
Converts progress to `progress.Percent * distance`.

```csharp
Animations.DurationPercentage(300)
    .Distance(200) // 0..200
    .Subscribe(px => Debug.WriteLine(px));
```

### `Animations.Distance(this rx<Duration> progress, rx<double> distance)` -> `rx<double>`
Same as above, but distance is dynamic.

```csharp
var distance = Observable.Return(150.0);
Animations.DurationPercentage(300)
    .Distance(distance)
    .Subscribe(px => Debug.WriteLine(px));
```

### `Animations.PixelsPerSecond(this rx<double> milliSeconds, double velocity)` -> `rx<double>`
Converts milliseconds to pixels at a constant velocity (px/s): `velocity * ms / 1000`.

```csharp
Animations.MilliSecondsElapsed(RxApp.TaskpoolScheduler)
    .PixelsPerSecond(velocity: 200)
    .Subscribe(px => Debug.WriteLine($"{px:0.0}px"));
```

---

## Element property animation (WPF)

All methods below return `rx<Unit>`.

### `OpacityTo(this UIElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `UIElement.Opacity` to `to`.

```csharp
myPanel
    .OpacityTo(250, 0.0, Ease.SineOut)
    .Concat(myPanel.OpacityTo(250, 1.0, Ease.SineIn))
    .Subscribe();
```

### `WidthTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Width` (falls back to `ActualWidth` if `Width` is `NaN`).

```csharp
myControl.WidthTo(400, 320, Ease.ExpoOut).Subscribe();
```

### `HeightTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Height` (falls back to `ActualHeight` if `Height` is `NaN`).

```csharp
myControl.HeightTo(400, 80, Ease.ExpoOut).Subscribe();
```

### `MarginTo(this FrameworkElement element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Margin` to `to`.

```csharp
myControl.MarginTo(300, new Thickness(10), Ease.SineInOut).Subscribe();
```

### `PaddingTo(this Control element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Padding` to `to`.

```csharp
myButton.PaddingTo(300, new Thickness(24, 8, 24, 8), Ease.SineOut).Subscribe();
```

### `CanvasLeftTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates the `Canvas.Left` attached property.

```csharp
myItem.CanvasLeftTo(350, 120, Ease.ExpoOut).Subscribe();
```

### `CanvasTopTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates the `Canvas.Top` attached property.

```csharp
myItem.CanvasTopTo(350, 40, Ease.ExpoOut).Subscribe();
```

### `BrushColorTo(this SolidColorBrush brush, double ms, Color to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates a `SolidColorBrush.Color`.

> If the brush might be frozen, clone it first and assign a non-frozen brush instance to your element.

```csharp
var brush = new SolidColorBrush(Colors.OrangeRed);
myBorder.Background = brush;

brush.BrushColorTo(500, Colors.SteelBlue, Ease.SineInOut).Subscribe();
```

---

## Margin edge animation (WPF)

These helpers animate individual `Thickness` components per-frame.

### `LeftMarginMove(this FrameworkElement element, double ms, double position, Ease ease = Ease.None, IScheduler? scheduler = null)`
Moves `Margin.Left` to an absolute `position`.

```csharp
myControl.LeftMarginMove(400, 100, Ease.ExpoOut).Subscribe();
```

### `RightMarginMove(this FrameworkElement element, rx<double> ms, rx<double> position, Ease ease = Ease.None, IScheduler? scheduler = null)`
Dynamic duration + dynamic position.

```csharp
var ms = Observable.Return(250.0);
var pos = Observable.Return(30.0);

myControl.RightMarginMove(ms, pos, Ease.SineInOut).Subscribe();
```

### `TopMarginMove(this FrameworkElement element, double ms, double position, Ease ease = Ease.None)`
Moves `Margin.Top` to `position`.

```csharp
myControl.TopMarginMove(300, 12, Ease.SineOut).Subscribe();
```

### `BottomMarginMove(this FrameworkElement element, double ms, double position, Ease ease = Ease.None, IScheduler? scheduler = null)`
Moves `Margin.Bottom` to `position`.

```csharp
myControl.BottomMarginMove(300, 12, Ease.SineOut).Subscribe();
```

> Observable overloads also exist for `LeftMarginMove`, `RightMarginMove`, `TopMarginMove`, `BottomMarginMove` where `ms`, `position`, and/or `ease` can be `IObservable`.

---

## Transforms (WPF)

Transform helpers will add the required transform into `RenderTransform` using a `TransformGroup` if needed.

### `TranslateTransform(this FrameworkElement element, rx<double> ms, rx<Point> position, Ease xease = Ease.None, Ease yease = Ease.None)`
Animates translate using dynamic duration and a dynamic target point.

```csharp
var ms = Observable.Return(400.0);
var pos = Observable.Return(new Point(120, 40));

myControl.TranslateTransform(ms, pos, Ease.ExpoOut, Ease.SineIn).Subscribe();
```

### `TranslateTransform(this FrameworkElement element, double ms, double xPosition, double yPosition, Ease xease = Ease.None, Ease yease = Ease.None, IScheduler? scheduler = null)`
Animates translate to (`xPosition`,`yPosition`).

```csharp
myControl.TranslateTransform(400, 120, 40, Ease.ExpoOut, Ease.SineIn).Subscribe();
```

### `RotateTransform(this FrameworkElement element, double ms, double angle, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates rotation by delta (`angle - initialAngle` internally).

```csharp
myControl.RotateTransform(300, 45, Ease.SineOut).Subscribe();
```

### `ScaleTransform(this FrameworkElement element, double ms, double scaleX, double scaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)`
Animates scale by delta.

```csharp
myControl.ScaleTransform(250, 1.2, 1.2, Ease.SineInOut, Ease.SineInOut).Subscribe();
```

### `SkewTransform(this FrameworkElement element, double ms, double angleX, double angleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)`
Animates skew by delta.

```csharp
myControl.SkewTransform(250, 10, 0, Ease.SineOut, Ease.SineOut).Subscribe();
```

### Absolute transform helpers (WPF)
These are convenience helpers that animate to absolute targets:

- `TranslateTo(this FrameworkElement element, double ms, double toX, double toY, ...)`
- `ScaleTo(this FrameworkElement element, double ms, double toScaleX, double toScaleY, ...)`
- `RotateTo(this FrameworkElement element, double ms, double toAngle, ...)`
- `SkewTo(this FrameworkElement element, double ms, double toAngleX, double toAngleY, ...)`

`TranslateTo` example:

```csharp
myControl.TranslateTo(300, toX: 0, toY: 0, Ease.ExpoOut, Ease.ExpoOut).Subscribe();
```

---

## “Effects” helpers (WPF)

### `ShakeTranslate(this FrameworkElement element, double ms, double amplitude, int shakes = 6, Ease ease = Ease.None, IScheduler? scheduler = null)`
Shakes horizontally using `TranslateTransform` and returns to the original position.

```csharp
myControl.ShakeTranslate(600, amplitude: 12, shakes: 8, ease: Ease.SineOut).Subscribe();
```

### `PulseOpacity(this UIElement element, double milliSecondsPerHalf, double low = 0.2, double high = 1.0, int pulses = 1, Ease ease = Ease.None, IScheduler? scheduler = null)`
Pulses opacity between `low` and `high` for `pulses` cycles.

```csharp
myControl.PulseOpacity(150, low: 0.3, high: 1.0, pulses: 3, ease: Ease.SineInOut).Subscribe();
```

---

## Composition helpers (WPF)

### `Sequence(this IEnumerable<rx<Unit>> animations)` -> `rx<Unit>`
Concatenates animations in-order.

```csharp
var anim = new[]
{
    myControl.OpacityTo(150, 0.0),
    myControl.OpacityTo(150, 1.0),
    myControl.ScaleTransform(200, 1.1, 1.1, Ease.SineOut, Ease.SineOut),
    myControl.ScaleTransform(200, 1.0, 1.0, Ease.SineIn, Ease.SineIn)
}.Sequence();

anim.Subscribe();
```

### `Parallel(this IEnumerable<rx<Unit>> animations)` -> `rx<Unit>`
Merges animations and completes when the last one finishes.

```csharp
new[]
{
    myControl.OpacityTo(250, 1.0, Ease.SineOut),
    myControl.TranslateTransform(250, 120, 0, Ease.ExpoOut, Ease.ExpoOut)
}.Parallel().Subscribe();
```

### `RepeatAnimation(this rx<Unit> animation, int? count = null)` -> `rx<Unit>`
Repeats an animation. If `count` is `null`, repeats forever until disposed.

```csharp
var pulseOnce = myControl.PulseOpacity(120, pulses: 1);

pulseOnce.RepeatAnimation(count: 5).Subscribe();
// pulseOnce.RepeatAnimation().Subscribe(); // infinite until disposed
```

### `Stagger(this IEnumerable<rx<Unit>> animations, TimeSpan staggerBy, IScheduler? scheduler = null)` -> `IEnumerable<rx<Unit>>`
Delays each animation by an incremental amount.

```csharp
var items = listBox.Items.Cast<FrameworkElement>();

var anims = items.Select(el => el.TranslateTransform(300, -40, 0, Ease.SineOut, Ease.SineOut)
    .Concat(el.TranslateTransform(250, 0, 0, Ease.ExpoOut, Ease.ExpoOut)));

anims.Stagger(TimeSpan.FromMilliseconds(80))
    .Parallel()
    .Subscribe();
```

---

# AnimationRx.Avalonia API

Namespace: `CP.AnimationRx`

## Setup

Add these usings:

```csharp
using CP.AnimationRx;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
```

---

## Frames & timing (Avalonia)

### `Animations.AnimateFrame(double framesPerSecond, IScheduler? scheduler = null)` -> `rx<long>`
Frames on an interval (default uses taskpool scheduler).

```csharp
Animations.AnimateFrame(60)
    .Subscribe(i => Debug.WriteLine(i));
```

### `Animations.AnimateFrame(TimeSpan period, IScheduler? scheduler = null)` -> `rx<long>`
Fixed-period ticker.

```csharp
Animations.AnimateFrame(TimeSpan.FromMilliseconds(16))
    .Subscribe(_ => { /* timer */ });
```

### `Animations.MilliSecondsElapsed(IScheduler scheduler)` -> `rx<double>`
Elapsed milliseconds since subscription.

```csharp
Animations.MilliSecondsElapsed(RxApp.TaskpoolScheduler)
    .Subscribe(ms => Debug.WriteLine(ms));
```

### `Animations.DurationPercentage(double ms, IScheduler? scheduler = null)` -> `rx<Duration>`
Progress `[0..1]` over a fixed duration.

```csharp
Animations.DurationPercentage(500)
    .EaseAnimation(Ease.SineInOut)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Animations.DurationPercentage(rx<double> ms, IScheduler? scheduler = null)` -> `rx<Duration>`
Progress `[0..1]` over a dynamic duration.

```csharp
var durationMs = Observable.Return(350.0);
Animations.DurationPercentage(durationMs)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Animations.TakeOneEvery<T>(this rx<T>, TimeSpan interval, IScheduler? scheduler = null)` -> `rx<T>`
Delays each element by `interval` and sequences them.

```csharp
Observable.Range(1, 3)
    .TakeOneEvery(TimeSpan.FromMilliseconds(250))
    .Subscribe(Console.WriteLine);
```

---

## Easing (Avalonia)

### `Eases.EaseAnimation(this rx<Duration> progress, Ease ease)` -> `rx<Duration>`
Applies a selected ease.

```csharp
Animations.DurationPercentage(300)
    .EaseAnimation(Ease.ExpoOut)
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### `Eases.ToDuration(this rx<double>)` -> `rx<Duration>`
Maps raw `[0..1]` values to `Duration`.

```csharp
Observable.Return(0.0)
    .Concat(Observable.Return(1.0))
    .ToDuration()
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

### Specific easing functions on `rx<Duration>`
Same list as WPF (`BackIn`, `BounceOut`, `SineInOut`, etc.).

```csharp
Animations.DurationPercentage(300)
    .BounceOut()
    .Subscribe(d => Debug.WriteLine(d.Percent));
```

---

## Value animation (Avalonia)

### `Animations.AnimateValue(double ms, double from, double to, Ease ease = Ease.None, IScheduler? scheduler = null)` -> `rx<double>`
Interpolates numeric values.

```csharp
Animations.AnimateValue(400, 0, 100, Ease.SineInOut)
    .Subscribe(v => Debug.WriteLine(v));
```

### `Animations.Distance(this rx<Duration>, double distance)` -> `rx<double>`
Progress to delta.

```csharp
Animations.DurationPercentage(200)
    .Distance(50)
    .Subscribe(v => Debug.WriteLine(v));
```

### `Animations.Distance(this rx<Duration>, rx<double> distance)` -> `rx<double>`
Dynamic distance.

```csharp
var distance = Observable.Return(180.0);
Animations.DurationPercentage(200)
    .Distance(distance)
    .Subscribe(v => Debug.WriteLine(v));
```

---

## Visual / control property animation (Avalonia)

All methods below return `rx<Unit>`.

### `OpacityTo(this Visual element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Visual.Opacity`.

```csharp
myVisual.OpacityTo(250, 0.0, Ease.SineOut).Subscribe();
```

### `WidthTo(this Control element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Control.Width` (falls back to `Bounds.Width` if `Width` is `NaN`).

```csharp
myControl.WidthTo(300, 240, Ease.ExpoOut).Subscribe();
```

### `HeightTo(this Control element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Control.Height`.

```csharp
myControl.HeightTo(300, 90, Ease.ExpoOut).Subscribe();
```

### `MarginTo(this Control element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `Control.Margin`.

```csharp
myControl.MarginTo(300, new Thickness(12), Ease.SineInOut).Subscribe();
```

### `PaddingTo(this TemplatedControl element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates `TemplatedControl.Padding`.

```csharp
myTemplatedControl.PaddingTo(200, new Thickness(16, 8), Ease.SineOut).Subscribe();
```

### `CanvasLeftTo(this Control element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates the `Canvas.Left` attached property.

```csharp
myControl.CanvasLeftTo(250, 120, Ease.ExpoOut).Subscribe();
```

### `CanvasTopTo(this Control element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates the `Canvas.Top` attached property.

```csharp
myControl.CanvasTopTo(250, 40, Ease.ExpoOut).Subscribe();
```

### `BrushColorTo(this SolidColorBrush brush, double ms, Color to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates a `SolidColorBrush.Color`.

```csharp
var brush = new SolidColorBrush(Colors.OrangeRed);
myBorder.Background = brush;

brush.BrushColorTo(500, Colors.SteelBlue, Ease.SineInOut).Subscribe();
```

### `ColorTo(this SolidColorBrush brush, double ms, Color to, Ease ease = Ease.None, IScheduler? scheduler = null)`
Alias for `BrushColorTo`.

```csharp
brush.ColorTo(500, Colors.LimeGreen, Ease.ExpoOut).Subscribe();
```

---

## Transforms (Avalonia)

Transform helpers will add the required transform to `Visual.RenderTransform` using a `TransformGroup` if needed.

### `TranslateTransform(this Visual element, double ms, double xPosition, double yPosition, Ease xease = Ease.None, Ease yease = Ease.None, IScheduler? scheduler = null)`
Animates translation.

```csharp
myVisual.TranslateTransform(400, 120, 40, Ease.ExpoOut, Ease.SineIn).Subscribe();
```

### `TranslateTo(this Visual element, double ms, double toX, double toY, ...)`
Alias for `TranslateTransform`.

```csharp
myVisual.TranslateTo(300, 0, 0, Ease.ExpoOut, Ease.ExpoOut).Subscribe();
```

### `RotateTransform(this Visual element, double ms, double angle, Ease ease = Ease.None, IScheduler? scheduler = null)`
Animates rotation to the target angle.

```csharp
myVisual.RotateTransform(300, 45, Ease.SineOut).Subscribe();
```

### `RotateTo(this Visual element, double ms, double toAngle, ...)`
Alias for `RotateTransform`.

```csharp
myVisual.RotateTo(300, 0, Ease.ExpoOut).Subscribe();
```

### `ScaleTransform(this Visual element, double ms, double scaleX, double scaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)`
Animates scale.

```csharp
myVisual.ScaleTransform(250, 1.2, 1.2, Ease.SineInOut, Ease.SineInOut).Subscribe();
```

### `ScaleTo(this Visual element, double ms, double toScaleX, double toScaleY, ...)`
Alias for `ScaleTransform`.

```csharp
myVisual.ScaleTo(250, 1.0, 1.0, Ease.SineOut, Ease.SineOut).Subscribe();
```

### `SkewTransform(this Visual element, double ms, double angleX, double angleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)`
Animates skew.

```csharp
myVisual.SkewTransform(250, 10, 0, Ease.SineOut, Ease.SineOut).Subscribe();
```

### `SkewTo(this Visual element, double ms, double toAngleX, double toAngleY, ...)`
Alias for `SkewTransform`.

```csharp
myVisual.SkewTo(250, 0, 0, Ease.ExpoOut, Ease.ExpoOut).Subscribe();
```

---

## “Effects” helpers (Avalonia)

### `ShakeTranslate(this Visual element, double ms, double amplitude, int shakes = 6, Ease ease = Ease.None, IScheduler? scheduler = null)`
Shakes horizontally and returns to the original position.

```csharp
myVisual.ShakeTranslate(600, amplitude: 12, shakes: 8, ease: Ease.SineOut).Subscribe();
```

### `PulseOpacity(this Visual element, double milliSecondsPerHalf, double low = 0.2, double high = 1.0, int pulses = 1, Ease ease = Ease.None, IScheduler? scheduler = null)`
Pulses opacity between `low` and `high` for `pulses` cycles.

```csharp
myVisual.PulseOpacity(150, low: 0.3, high: 1.0, pulses: 3, ease: Ease.SineInOut).Subscribe();
```

---

## Composition helpers (Avalonia)

### `Sequence(this IEnumerable<rx<Unit>> animations)` -> `rx<Unit>`
Sequences animations.

```csharp
new[]
{
    myVisual.OpacityTo(150, 0.0),
    myVisual.OpacityTo(150, 1.0),
}.Sequence().Subscribe();
```

### `Parallel(this IEnumerable<rx<Unit>> animations)` -> `rx<Unit>`
Runs animations in parallel.

```csharp
new[]
{
    myVisual.OpacityTo(250, 1.0),
    myVisual.TranslateTransform(250, 120, 0)
}.Parallel().Subscribe();
```

### `RepeatAnimation(this rx<Unit> animation, int? count = null)` -> `rx<Unit>`
Repeats an animation.

```csharp
var pulse = myVisual.PulseOpacity(120, pulses: 1);

pulse.RepeatAnimation(5).Subscribe();
```

### `DelayBetween(this IEnumerable<rx<Unit>> animations, TimeSpan delay, IScheduler? scheduler = null)` -> `rx<Unit>`
Inserts a delay between each animation in a sequence.

```csharp
new[]
{
    myVisual.OpacityTo(100, 0.0),
    myVisual.OpacityTo(100, 1.0),
}
.DelayBetween(TimeSpan.FromMilliseconds(200))
.Subscribe();
```

### `Stagger(this IEnumerable<rx<Unit>> animations, TimeSpan staggerBy, IScheduler? scheduler = null)` -> `IEnumerable<rx<Unit>>`
Delays each animation by an incremental stagger.

```csharp
var anims = items.Select(v => v.OpacityTo(250, 1.0, Ease.SineOut));

anims.Stagger(TimeSpan.FromMilliseconds(60))
    .Parallel()
    .Subscribe();
```

---

## Patterns & tips

- Prefer composing animations with Rx (`Concat`, `Merge`, `Sequence`, `Parallel`) rather than timers.
- Always dispose subscriptions for:
  - cancellation
  - preventing leaks if element lifetime is shorter than the animation
- For smooth per-frame loops you can build a `dt` stream from `AnimateFrame(...)` (or `RenderFrames()` in WPF).

---

## License

MIT License. © Chris Pulman

---

**AnimationRx**  - Empowering Automation with Reactive Technology ⚡🏭
