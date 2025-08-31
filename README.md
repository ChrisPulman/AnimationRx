# AnimationRx.Wpf

A small, composable animation library for WPF built on System.Reactive and ReactiveUI. It exposes animation primitives as IObservable<Unit> and value streams, so you can compose, cancel, and sequence animations with Rx operators.

Targets: .NET Framework 4.6.2, 4.7.2, 4.8 and .NET 8/9 (Windows).

- Lightweight (pure Rx + WPF)
- Composable (Sequence, Parallel, Stagger, Repeat)
- Easing functions (Back, Bounce, Circ, Cubic, Elastic, Expo, Quad, Quartic, Quintic, Sine)
- UI-friendly (observe and update on Dispatcher)


## Install

NuGet:
- Package: AnimationRx.Wpf

Package Manager:
- Install-Package AnimationRx.Wpf


## Quick start

Animate the opacity of any UIElement:

```csharp
// Fade in over 300ms using Sine ease
someElement.OpacityTo(300, 1.0, Ease.SineInOut)
    .Subscribe(); // dispose to cancel
```

Move a FrameworkElement using a TranslateTransform:

```csharp
// Slide to (x: 120, y: 40) over 400ms, different easing per axis
someElement.TranslateTransform(400, xPosition: 120, yPosition: 40, Ease.ExpoOut, Ease.SineIn)
    .Subscribe();
```

Run animations in sequence and in parallel:

```csharp
var fadeIn = someElement.OpacityTo(250, 1.0, Ease.SineOut);
var move   = someElement.TranslateTransform(400, 150, 0, Ease.ExpoOut, Ease.SineOut);

new[] { fadeIn, move }.Parallel()            // run together
    .Concat(someElement.ScaleTransform(250, 1.1, 1.1, Ease.SineInOut, Ease.SineInOut))
    .Concat(someElement.ScaleTransform(250, 1.0, 1.0, Ease.SineInOut, Ease.SineInOut))
    .Subscribe();
```


## Concepts

- Duration: an internal record struct representing animation progress as Percent [0..1].
- Eases: extension methods that reshape Duration.Percent (e.g., BackIn, SineOut). Use EaseAnimation to apply an Ease to a Duration stream.
- AnimateValue: generates intermediate values from start to end using an Ease.
- UI thread affinity: all built-in animations capture initial UI values on the Dispatcher and set properties on the Dispatcher.
- Cancellation: dispose the subscription returned by Subscribe to cancel an animation.


## Threading & Schedulers

- Most methods accept an optional IScheduler to drive time (defaults to RxApp.TaskpoolScheduler for time sources and RxApp.MainThreadScheduler for UI updates).
- All property reads and writes are marshaled to the WPF Dispatcher, so you can call from background threads safely.


## API reference with examples

Below, rx means IObservable.

### Frames & timing

- AnimateFrame(double framesPerSecond, IScheduler? scheduler = null): rx<long>
  - Emits a tick roughly every 1/fps seconds. Useful for simple timers.
  - Example:
    ```csharp
    Animations.AnimateFrame(60)
        .Subscribe(_ => { /* per-frame work */ });
    ```

- AnimateFrame(TimeSpan period, IScheduler? scheduler = null): rx<long>
  - Fixed-period ticker.

- RenderFrames(): rx<long>
  - Emits one tick per WPF CompositionTarget.Rendering. Runs on the Dispatcher.
  - Use for game/update loops synchronized to the monitor.
  - Example delta-time (dt) stream:
    ```csharp
    var dt = Animations.RenderFrames()
        .Timestamp(RxApp.MainThreadScheduler)
        .Scan(new { last = DateTimeOffset.Now, dt = 0.0 }, (acc, t) =>
        {
            var now = t.Timestamp;
            var dt  = Math.Clamp((now - acc.last).TotalSeconds, 0, 0.05);
            return new { last = now, dt };
        })
        .Select(x => x.dt);
    ```

- MilliSecondsElapsed(IScheduler scheduler): rx<double>
  - Emits elapsed milliseconds since subscription.

- DurationPercentage(double milliSeconds, IScheduler? scheduler = null): rx<Duration>
- DurationPercentage(rx<double> milliSeconds, IScheduler? scheduler = null): rx<Duration>
  - Emits progress from 0..1 over the specified duration and completes.
  - Always emits a final 1.0 tick before completion.

- ToDuration(this rx<double>): rx<Duration>
  - Maps raw double (0..1) to a Duration.

- TakeOneEvery<T>(this rx<T> source, TimeSpan interval, IScheduler? scheduler = null): rx<T>
  - Back-pressure for high-rate streams. Delays output by interval per element.


### Easing

- Ease (enum): None, BackIn/Out/InOut, BounceIn/Out/InOut, CircIn/Out/InOut, Cubic..., Elastic..., Expo..., Quad..., Quartic..., Quintic..., Sine...
- EaseAnimation(this rx<Duration> progress, Ease ease): rx<Duration>
  - Applies the selected ease to the progress stream.
  - Example:
    ```csharp
    Animations.DurationPercentage(500)
        .EaseAnimation(Ease.ExpoOut)
        .Subscribe(d => Debug.WriteLine(d.Percent));
    ```


### Value animation

- AnimateValue(double ms, double from, double to, Ease ease = Ease.None, IScheduler? scheduler = null): rx<double>
  - Emits interpolated values from -> to using easing.
  - Example: animate a numeric property you manage yourself
    ```csharp
    Animations.AnimateValue(400, from: 0, to: 100, Ease.SineInOut)
        .ObserveOn(RxApp.MainThreadScheduler)
        .Subscribe(x => viewModel.Progress = x);
    ```

- Distance helpers
  - Distance(this rx<Duration> progress, double distance): rx<double>
  - Distance(this rx<Duration> progress, rx<double> distance): rx<double>
  - Convert 0..1 progress into a delta over a distance.


### Element property animation

All of the following return rx<Unit> that completes when the animation ends. Subscribe and dispose to cancel mid-flight.

- OpacityTo(this UIElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)
  - Example: fade in/out
    ```csharp
    panel.OpacityTo(300, 0.0, Ease.SineOut)
         .Concat(panel.OpacityTo(300, 1.0, Ease.SineIn))
         .Subscribe();
    ```

- WidthTo / HeightTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)
  - Animates actual Width/Height (falls back to ActualWidth/ActualHeight when NaN).

- MarginTo(this FrameworkElement element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)
- PaddingTo(this Control element, double ms, Thickness to, Ease ease = Ease.None, IScheduler? scheduler = null)

- CanvasLeftTo / CanvasTopTo(this FrameworkElement element, double ms, double to, Ease ease = Ease.None, IScheduler? scheduler = null)
  - Animates the Canvas.Left / Canvas.Top attached property.

- BrushColorTo(this SolidColorBrush brush, double ms, Color to, Ease ease = Ease.None, IScheduler? scheduler = null)
  - If the source brush may be frozen, clone it first or assign a non-frozen clone to your element before animating.


### Margin edges

- LeftMarginMove / TopMarginMove / RightMarginMove / BottomMarginMove
  - Overloads:
    - (rx<double> ms, rx<double> position, Ease ease = Ease.None, IScheduler? scheduler = null)
    - (rx<double> ms, rx<double> position, rx<Ease> ease, IScheduler? scheduler = null)
    - (double ms, double position, Ease ease = Ease.None, IScheduler? scheduler = null)
  - Sets the corresponding Thickness component each frame.

Example: move a control left and up at different speeds
```csharp
var ms = Observable.Return(500.0);
var leftPositions = new[] { 50.0, 200.0, 120.0 }.ToObservable();
var topPositions  = new[] { 10.0,  40.0,  20.0 }.ToObservable();

someElement.LeftMarginMove(ms, leftPositions, Ease.ExpoOut)
    .Merge(someElement.TopMarginMove(ms, topPositions, Ease.SineOut))
    .Subscribe();
```


### Transforms

- TranslateTransform(this FrameworkElement element, rx<double> ms, rx<Point> position, Ease xease = Ease.None, Ease yease = Ease.None)
- TranslateTransform(this FrameworkElement element, double ms, double xPosition, double yPosition, Ease xease = Ease.None, Ease yease = Ease.None, IScheduler? scheduler = null)

- RotateTransform(this FrameworkElement element, rx<double> ms, rx<double> angle, Ease ease = Ease.None)
- RotateTransform(this FrameworkElement element, rx<double> ms, rx<double> angle, rx<Ease> ease)
- RotateTransform(this FrameworkElement element, double ms, double angle, Ease ease = Ease.None, IScheduler? scheduler = null)

- ScaleTransform(this FrameworkElement element, double ms, double scaleX, double scaleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)

- SkewTransform(this FrameworkElement element, double ms, double angleX, double angleY, Ease easeX = Ease.None, Ease easeY = Ease.None, IScheduler? scheduler = null)

Tip: The library will add the required transform to RenderTransform (using a TransformGroup) if it is not already present.


### Composition helpers

- Sequence(this IEnumerable<rx<Unit>> animations): rx<Unit>
  - Concat all animations in order.

- Parallel(this IEnumerable<rx<Unit>> animations): rx<Unit>
  - Merge all animations and complete when the last finishes.

- Repeat(this rx<Unit> animation, int? count = null): rx<Unit>
  - Repeat a single animation count times, or indefinitely when null.

- DelayBetween(this IEnumerable<rx<Unit>> animations, TimeSpan delay, IScheduler? scheduler = null): rx<Unit>
  - Inserts a delay before each item in the sequence.

- Stagger(this IEnumerable<rx<Unit>> animations, TimeSpan staggerBy, IScheduler? scheduler = null): IEnumerable<rx<Unit>>
  - Returns a new enumeration that delays each animation by an incremental stagger.

Examples:

```csharp
// Stagger a stack of items sliding in from the left
var anims = listBox.Items.Cast<FrameworkElement>()
    .Select((el, i) => el.TranslateTransform(400, -40, 0, Ease.SineOut, Ease.SineOut)
        .Concat(el.TranslateTransform(300, 0, 0, Ease.ExpoOut, Ease.SineOut)));

anims.Stagger(TimeSpan.FromMilliseconds(100))
     .Parallel()
     .Subscribe();
```

```csharp
// Pulse opacity forever until disposed
var pulse = someElement.OpacityTo(200, 0.2, Ease.SineOut)
    .Concat(someElement.OpacityTo(200, 1.0, Ease.SineIn));

pulse.Repeat().Subscribe(); // dispose to stop
```


## Advanced: game-loop style updates

Use RenderFrames for smooth per-frame processing and bind input streams as needed.

```csharp
var frame = Animations.RenderFrames();

// Example: use dt to move a rectangle at 200 px/s to the right
var dt = frame
    .Timestamp(RxApp.MainThreadScheduler)
    .Scan(new { last = DateTimeOffset.Now, dt = 0.0 }, (acc, t) =>
    {
        var now = t.Timestamp;
        var dt  = Math.Clamp((now - acc.last).TotalSeconds, 0, 0.05);
        return new { last = now, dt };
    })
    .Select(x => x.dt);

var rect = new Rectangle { Width = 20, Height = 20, Fill = Brushes.OrangeRed };
canvas.Children.Add(rect);
Canvas.SetLeft(rect, 0);
Canvas.SetTop(rect, 10);

dt.ObserveOn(RxApp.MainThreadScheduler)
  .Subscribe(d => Canvas.SetLeft(rect, Canvas.GetLeft(rect) + (200 * d)));
```


## Patterns & tips

- Dispose subscriptions to cancel animations or to avoid leaks when elements go out of scope.
- You can combine animations with Rx operators: Concat, Merge/Parallel, SelectMany, TakeUntil, etc.
- When animating Brush colors, clone frozen brushes before animating (the helpers do not mutate frozen brushes).
- If you animate element size (Width/Height), ensure the element is measured/arranged; otherwise initial values may be NaN.
- The time-source scheduler (defaults to Taskpool) is independent from the UI update (Dispatcher) path; you can pass your own if needed.


## API surface (summary)

- Timing: AnimateFrame(fps|period), RenderFrames, MilliSecondsElapsed, DurationPercentage, ToDuration, TakeOneEvery
- Easing: Ease enum, EaseAnimation, all easing families
- Values: AnimateValue, Distance
- Element properties: OpacityTo, WidthTo, HeightTo, MarginTo, PaddingTo, CanvasLeftTo, CanvasTopTo, BrushColorTo
- Margin edges: LeftMarginMove, TopMarginMove, RightMarginMove, BottomMarginMove
- Transforms: TranslateTransform, RotateTransform, ScaleTransform, SkewTransform
- Composition: Sequence, Parallel, Repeat, DelayBetween, Stagger


## License

MIT License. © Chris Pulman
