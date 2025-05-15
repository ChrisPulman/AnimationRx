# AnimationRx
An Animation Library for ReactiveUI based projects

## Features
- Simple to use
- ReactiveUI based
- Lightweight
- No dependencies

## Installation
You can install the library via NuGet Package Manager Console:
```
Install-Package AnimationRx.Wpf
```
or by adding the following line to your `.csproj` file:
```xml
<PackageReference Include="AnimationRx.Wpf" Version="1.0.*" />
```
or by using the NuGet Package Manager in Visual Studio.

## Usage
```csharp
using System;
using System.Reactive.Linq;
using System.Windows;
using CP.AnimationRx;
using ReactiveMarbles.ObservableEvents;

namespace ExAnimationRx
{
    public class AnimationViewModel : ReactiveObject
    {
        public AnimationViewModel()
        {
            Ball1.LeftMarginMove(
                Observable.Return(1000.0),
                new double[] { 300.0, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300, 50, 300 }
                .ToObservable().TakeOneEvery(TimeSpan.FromMilliseconds(1500)),
                Ease.ExpoOut)
                .Subscribe();

            Ball1.TopMarginMove(
                Observable.Return(1000.0),
                new double[] { 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0, 10, 150.0 }
                .ToObservable().TakeOneEvery(TimeSpan.FromMilliseconds(1500)),
                Ease.BounceOut)
                .Subscribe();
        }
    }
}
```

## Features
- **Ease**: A set of easing functions to create smooth animations.
- **Animation**: A set of animation functions to create different types of animations.

### Easing Functions
- BackIn,
- BackInOut,
- BackOut,
- BounceIn,
- BounceInOut,
- BounceOut,
- CircIn,
- CircInOut,
- CircOut,
- CubicIn,
- CubicInOut,
- CubicOut,
- ElasticIn,
- ElasticInOut,
- ElasticOut,
- ExpoIn,
- ExpoInOut,
- ExpoOut,
- QuinticIn,
- QuinticInOut,
- QuinticOut,
- QuadIn,
- QuadInOut,
- QuadOut,
- QuarticIn,
- QuarticInOut,
- QuarticOut,
- SineIn,
- SineInOut,
- SineOut

### Animation Functions
- AnimateFrame
- BottomMarginMove
- Distance
- DurationPercentage
- TranslateXMove
- LeftMarginMove
- MilliSecondsElapsed
- PixelsPerSecond
- RightMarginMove
- RotateTransform
- TakeOneEvery
- ToDuration
- TopMarginMove
- TranslateTransform
