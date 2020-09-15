using System;

using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using SkiaSharp;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022:A catch clause that catches System.Exception and has an empty body", Justification = "<Pending>")]
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                base.OnPropertyChanged(propertyName);
            }
            catch (Exception) { }

            if (propertyName == nameof(Parent) && Parent is SegmentedControl segmentedControl && Control is SegmentedControl control)
            {
                control._segments.CollectionChanged += OnSegmentsCollectionChanged;
                foreach (var segment in control._segments)
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
                    || propertyName == ContentView.BackgroundImageProperty.PropertyName
                    || propertyName == Button.HasShadowProperty.PropertyName
                    || propertyName == ContentView.ShadowInvertedProperty.PropertyName
                    || propertyName == Button.OutlineColorProperty.PropertyName
                    || propertyName == Button.OutlineWidthProperty.PropertyName
                    || propertyName == ContentView.OutlineRadiusProperty.PropertyName
                    || (propertyName == Button.SelectedBackgroundColorProperty.PropertyName && button.IsSelected)
                    || propertyName == Button.IsSelectedProperty.PropertyName
                    || propertyName == IsEnabledProperty.PropertyName
                    || propertyName == Button.DarkThemeProperty.PropertyName
                    || propertyName == WidthProperty.PropertyName
                    || propertyName == HeightProperty.PropertyName
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

        bool invalidating = false;
        bool pendingInvalidate = false;
        void Invalidate()
        {
            if (invalidating)
            {
                pendingInvalidate = true;
                System.Diagnostics.Debug.WriteLine("pending");
                return;
            }
            invalidating = true;
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                InvalidateMeasure();
                InvalidateSurface();
                invalidating = false;
                if (pendingInvalidate)
                {
                    pendingInvalidate = false;
                    Invalidate();
                }
            });

        }


        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (e.Surface?.Canvas is SKCanvas canvas && e.Info.Rect is SKRectI rect)
            {
                canvas.Clear();
                Control?.LayoutFunction(rect.Left / Display.Scale, rect.Top / Display.Scale, rect.Width / Display.Scale, rect.Height / Display.Scale, PaintSegmentButtonBackground, e);
            }
        }

        private static bool PaintSegmentButtonBackground(View view, Rectangle bounds, object obj)
        {
            if (view is SegmentButton button && obj is SKPaintSurfaceEventArgs e)
            {
                var rect = new SKRect((float)Math.Round(bounds.Left * Display.Scale), (float)Math.Round(bounds.Top * Display.Scale), (float)Math.Round(bounds.Right * Display.Scale), (float)Math.Round(bounds.Bottom * Display.Scale));
                button.CurrentBackgroundImage.SharedOnPaintSurface(e, rect);
            }
            return false;
        }
    }
}