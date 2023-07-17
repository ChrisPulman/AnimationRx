// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Windows;
using CP.AnimationRx;
using ReactiveMarbles.ObservableEvents;

namespace CP.Animation.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // const double framesPerSecond = 50.0;
            // Animations.AnimateFrame(framesPerSecond).Subscribe(frame =>
            // {
            //    Thickness t = this.Ball1.Margin;
            //    t.Left = frame;
            //    this.Ball1.Margin = t;
            // });

            // Animations.MilliSecondsElapsed(RxApp.TaskpoolScheduler)
            //    .TakeWhile(t => t <= 5000)
            //    .PixelsPerSecond(50)
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(frame =>
            // {
            //    Thickness t = this.Ball1.Margin;
            //    t.Left = frame;
            //    this.Ball1.Margin = t;
            // });

            // Animations.MoveDuration(2000, RxApp.TaskpoolScheduler)
            //    .BounceInOut()
            //    .Distance(300)
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(frame =>
            //    {
            //        Thickness t = this.Ball1.Margin;
            //        t.Left = frame;
            //        this.Ball1.Margin = t;
            //    });
            this.Events().Loaded.Subscribe(async _ =>
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

                // this.Ball1.TranslateTransform(Observable.Return(2000.0)
                //    , new Point[] { new Point(300.0, 150.0), new Point(50, 10), new Point(300.0, 150.0), new Point(50, 10), new Point(300.0, 150.0), new Point(50, 10), new Point(300.0, 150.0), new Point(50, 10), new Point(300.0, 150.0), new Point(50, 10), new Point(300.0, 150.0) }.ToObservable().TakeOneEvery(TimeSpan.FromSeconds(5))
                //        , Ease.BackOut, Ease.BounceOut).Subscribe();
                await Ball1.RotateTransform(2000, 90);

                await Ball1.RotateTransform(2000, 0);
            });
        }
    }
}
