using System;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using SkiaSharp;

namespace Forms9Patch
{
    class SegmentedControlBackground : SKCanvasView
    {
        SegmentedControl Control => (SegmentedControl)Parent;

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == nameof(Parent) && Parent is SegmentedControl segmentedControl)
            {
                segmentedControl._segments.CollectionChanged -= OnSegmentsCollectionChanged;
                foreach (var segment in segmentedControl._segments)
                    segment._button.PropertyChanged -= OnButtonPropertyChanged;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Parent) && Parent is SegmentedControl segmentedControl)
            {
                Control._segments.CollectionChanged += OnSegmentsCollectionChanged;
                foreach (var segment in Control._segments)
                    segment._button.PropertyChanged += OnButtonPropertyChanged;
            }
        }

        private void OnSegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Invalidate();
        }

        private void OnButtonPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Button button)
            {
                var propertyName = e.PropertyName;
                if (propertyName == Button.BackgroundColorProperty.PropertyName
                    || propertyName == Button.BackgroundImageProperty.PropertyName
                    || propertyName == Button.HasShadowProperty.PropertyName
                    || propertyName == Button.ShadowInvertedProperty.PropertyName
                    || propertyName == Button.OutlineColorProperty.PropertyName
                    || propertyName == Button.OutlineWidthProperty.PropertyName
                    || propertyName == Button.OutlineRadiusProperty.PropertyName
                    || (propertyName == Button.SelectedBackgroundColorProperty.PropertyName && button.IsSelected)
                    || propertyName == Button.IsSelectedProperty.PropertyName
                    || propertyName == Button.IsEnabledProperty.PropertyName
                    || propertyName == Button.DarkThemeProperty.PropertyName
                    || propertyName == Button.WidthProperty.PropertyName
                    || propertyName == Button.HeightProperty.PropertyName
                    || propertyName == Button.OrientationProperty.PropertyName
                        )
                {
                    //System.Diagnostics.Debug.WriteLine("PropertyName: " + propertyName);
                    Invalidate();
                }
            }
        }

        void Invalidate()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                InvalidateMeasure();
                InvalidateSurface();
            }
            else
                Device.BeginInvokeOnMainThread(Invalidate);
        }


        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface?.Canvas;
            var rect = e.Info.Rect;

            //System.Diagnostics.Debug.WriteLine(GetType() + ".OnPaintSurface(" + e.Info.Width + "," + e.Info.Height + ")");

            if (canvas == null)
                return;

            canvas.Clear();
            System.Diagnostics.Debug.WriteLine("======================================================");
            Control.LayoutFunction(rect.Left / Display.Scale, rect.Top / Display.Scale, rect.Width / Display.Scale, rect.Height / Display.Scale, PaintSegmentButtonBackground, e);
        }

        private bool PaintSegmentButtonBackground(View view, Rectangle bounds, object obj)
        {
            if (view is SegmentButton button && obj is SKPaintSurfaceEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("PaintSegmentButtonBackground[" + button.InstanceId + "]: bounds=" + bounds);
                SKRect rect = new SKRect((float)Math.Round(bounds.Left * Display.Scale), (float)Math.Round(bounds.Top * Display.Scale), (float)Math.Round(bounds.Right * Display.Scale), (float)Math.Round(bounds.Bottom * Display.Scale));
                button.CurrentBackgroundImage.SharedOnPaintSurface(e, rect);
            }
            return false;
        }
    }
}