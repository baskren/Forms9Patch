using System;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using SkiaSharp;
using P42.Utils;

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
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null)
                            foreach (var item in e.NewItems)
                                if (item is Segment segment)
                                    segment._button.PropertyChanged += OnButtonPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null)
                            foreach (var item in e.OldItems)
                                if (item is Segment segment)
                                    segment._button.PropertyChanged -= OnButtonPropertyChanged;
                    }
                    break;
            }
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
                    || propertyName == SegmentButton.ExtendedElementSeparatorWidthProperty.PropertyName
                    || propertyName == SegmentButton.ExtendedElementShapeProperty.PropertyName
                    || propertyName == SegmentButton.ExtendedElementShapeOrientationProperty.PropertyName
                        )
                {
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
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface?.Canvas;
            var rect = e.Info.Rect;

            //System.Diagnostics.Debug.WriteLine(GetType() + ".OnPaintSurface(" + e.Info.Width + "," + e.Info.Height + ")");

            if (canvas == null)
                return;

            canvas.Clear();
            Control.LayoutFunction(rect.Left / Display.Scale, rect.Top / Display.Scale, rect.Width / Display.Scale, rect.Height / Display.Scale, PaintSegmentButtonBackground, e);
        }

        private bool PaintSegmentButtonBackground(View view, Rectangle bounds, object obj)
        {
            if (view is SegmentButton button && obj is SKPaintSurfaceEventArgs e)
            {
                //System.Diagnostics.Debug.WriteLine("PaintSegmentButtonBackground[" + button.InstanceId + "]: bounds=" + bounds);
                var rect = new SKRect((float)Math.Round(bounds.Left * Display.Scale), (float)Math.Round(bounds.Top * Display.Scale), (float)Math.Round(bounds.Right * Display.Scale), (float)Math.Round(bounds.Bottom * Display.Scale));
                button.CurrentBackgroundImage.SharedOnPaintSurface(e, rect);
            }
            return false;
        }
    }
}