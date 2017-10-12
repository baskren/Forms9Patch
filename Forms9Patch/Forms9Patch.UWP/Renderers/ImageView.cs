using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;

namespace Forms9Patch.UWP
{
    public class ImageView : Windows.UI.Xaml.Controls.Grid, IDisposable
    {
        #region Fields
        bool _debugMessages = true;

        RangeLists _rangeLists = null;

        int _instance;
        int _tileCanvasWidth = 0;
        int _tileCanvasHeight = 0;

        Windows.UI.Xaml.Controls.Canvas _tileCanvas;

        Forms9Patch.Image _imageElement;
        WriteableBitmap _sourceBitmap;
        Xamarin.Forms.ImageSource _xfImageSource;
        Forms9Patch.IRoundedBox _roundedBoxElement;

        #endregion


        #region Property Management

        public Forms9Patch.Image ImageElement
        {
            get { return _imageElement; }
            set
            {
                if (_imageElement != value)
                {
                    if (_imageElement != null)
                    {
                        _imageElement.PropertyChanged -= OnImageElementPropertyChanged;
                        _imageElement.SizeChanged -= OnImageElementSizeChanged;
                    }
                    _imageElement = value;
                    SetSourceAsync();
                    if (_imageElement != null)
                    {
                        _imageElement.PropertyChanged += OnImageElementPropertyChanged;
                        _imageElement.SizeChanged += OnImageElementSizeChanged;
                    }
                }
            }
        }

        private void OnImageElementSizeChanged(object sender, EventArgs e)
        {
            GenerateImageLayout();
        }

        private void OnImageElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.Image.SourceProperty.PropertyName ||
                e.PropertyName == Forms9Patch.Image.TintColorProperty.PropertyName ||
                e.PropertyName == Forms9Patch.Image.CapInsetsProperty.PropertyName ||
                e.PropertyName == Forms9Patch.Image.FillProperty.PropertyName)
                GenerateImageLayout();
        }

        public Forms9Patch.IRoundedBox RoundedBoxElement
        {
            get { return _roundedBoxElement; }
            set
            {
                if (_roundedBoxElement != value)
                {
                    if (_roundedBoxElement is VisualElement oldVisualElement)
                    {
                        oldVisualElement.PropertyChanged -= OnRoundedBoxElementPropertyChanged;
                        oldVisualElement.SizeChanged -= OnRoundedBoxElementSizeChanged;
                    }
                    _roundedBoxElement = value;
                    GenerateOutlineLayout();
                    if (_roundedBoxElement is VisualElement newVisualElement)
                    {
                        newVisualElement.PropertyChanged += OnRoundedBoxElementPropertyChanged;
                        newVisualElement.SizeChanged += OnRoundedBoxElementSizeChanged;
                    }
                }
            }
        }

        private void OnRoundedBoxElementSizeChanged(object sender, EventArgs e)
        {
            GenerateOutlineLayout();
        }

        private void OnRoundedBoxElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.RoundedBoxBase.HasShadowProperty.PropertyName ||
                e.PropertyName == Forms9Patch.RoundedBoxBase.IsEllipticalProperty.PropertyName ||
                e.PropertyName == Forms9Patch.RoundedBoxBase.OutlineColorProperty.PropertyName ||
                e.PropertyName == Forms9Patch.RoundedBoxBase.OutlineRadiusProperty.PropertyName ||
                e.PropertyName == Forms9Patch.RoundedBoxBase.OutlineWidthProperty.PropertyName ||
                e.PropertyName == Forms9Patch.RoundedBoxBase.ShadowInvertedProperty.PropertyName)
                GenerateOutlineLayout();
        }

        Xamarin.Forms.Size BaseImageSize
        {
            get
            {
                if (_sourceBitmap == null)
                {
                    if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.set_BaseImageSize[" + _instance + "]: " + Xamarin.Forms.Size.Zero);
                    return Xamarin.Forms.Size.Zero;
                }

                var result = new Xamarin.Forms.Size(_sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight);
                if (_rangeLists != null)
                {
                    result.Width -= 2;
                    result.Height -= 2;
                }
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.set_BaseImageSize[" + _instance + "]: " + result);
                return result;
            }
        }

        internal async Task SetSourceAsync()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SetSourceAsync enter");
            if (ImageElement == null)
                return;
            if (ImageElement.Source != _xfImageSource)
            {
                ((IImageController)ImageElement)?.SetIsLoading(true);
                _xfImageSource = ImageElement.Source;
                Xamarin.Forms.Platform.UWP.IImageSourceHandler handler = null;
                if (ImageElement.Source != null)
                {
                    if (_xfImageSource is FileImageSource)
                        handler = new FileImageSourceHandler();
                    else if (_xfImageSource is UriImageSource)
                        handler = new UriImageSourceHandler();
                    else if (_xfImageSource is StreamImageSource)
                        handler = new StreamImageSourceHandler();
                }
                Windows.UI.Xaml.Media.ImageSource imagesource = null;
                if (handler != null && _xfImageSource != null)
                {
                    try
                    {
                        imagesource = await handler.LoadImageAsync(_xfImageSource);
                    }
                    catch (OperationCanceledException)
                    {
                        imagesource = null;
                    }
                    // In the time it takes to await the imagesource, some zippy little app
                    // might have disposed of this Image already.


                    if (imagesource is WriteableBitmap bitmap)
                        _sourceBitmap = bitmap;
                    else
                        _sourceBitmap = null;

                    _tileCanvas = null;
                    GenerateImageLayout();
                }
            }
            ((IImageController)ImageElement)?.SetIsLoading(false);
            RefreshImage();
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SetSourceAsync enter");
        }

        #endregion Property Management


        #region Constructor
        public ImageView(int instance)
        {
            _instance = instance;
            //BorderThickness = new Windows.UI.Xaml.Thickness(1);
            //BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Pink);
            Padding = new Windows.UI.Xaml.Thickness();
        }
        #endregion


        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _sourceBitmap = null;
                    ImageElement = null;
                    RoundedBoxElement = null;
                    _xfImageSource = null;
                    _rangeLists = null;
                    _tileCanvas = null;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


        #region Layout 
        void RefreshImage()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView[" + _instance + "].RefreshImage()");
            ((IVisualElementController)ImageElement)?.InvalidateMeasure(Xamarin.Forms.Internals.InvalidationTrigger.RendererReady);
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView[" + _instance + "].RefreshImage() RETURN ");
        }

        void UpdateAspect()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect[" + _instance + "]");
            if (_disposed)
                return;

            if (Children.Count == 1 && Children[0] is Windows.UI.Xaml.Controls.Image image)
            {
                // it's a simple image

                if (ImageElement.Fill == Fill.Tile || ImageElement.Fill == Fill.None)
                {
                    image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                    image.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                }
                else
                {
                    image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    image.Stretch = ImageElement.Fill.ToStretch();
                }
                return;
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect[" + _instance + "] RETURN");
        }

        internal void GenerateImageLayout(Windows.Foundation.Size size = default(Windows.Foundation.Size))
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.GenerateLayout[" + _instance + "]  Fill=[" + ImageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] size=[" + size + "]");
            if (_sourceBitmap == null)
                return;
            //Stopwatch stopwatch = Stopwatch.StartNew();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            //Children.Clear();
            if (Children.Contains(_tileCanvas))
                Children.Remove(_tileCanvas);
            if (Children.Any((view) => view is Windows.UI.Xaml.Controls.Image))
            {
                var oldImageView = Children.Where((view) => view is Windows.UI.Xaml.Controls.Image).First();
                Children.Remove(oldImageView);
            }


            _rangeLists = _sourceBitmap?.NinePatchRanges();
            bool sourceBitmapIsNinePatch = _rangeLists != null;
            _rangeLists = ImageElement.CapInsets.ToRangeLists(_sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight, _xfImageSource, sourceBitmapIsNinePatch) ?? _rangeLists;

            if (ImageElement.Fill == Fill.Tile)
            {
                _tileCanvas = _tileCanvas ?? new Windows.UI.Xaml.Controls.Canvas();
                var width = ActualWidth;
                var height = ActualHeight;
                if (size != default(Windows.Foundation.Size))
                {
                    width = size.Width;
                    height = size.Height;
                }
                for (int i = _tileCanvasWidth; i < width; i += _sourceBitmap.PixelWidth)
                {
                    for (int j = _tileCanvasHeight; j < height; j += _sourceBitmap.PixelHeight)
                    {
                        var image = new Windows.UI.Xaml.Controls.Image { Source = _sourceBitmap };
                        _tileCanvas.Children.Add(image);
                        Windows.UI.Xaml.Controls.Canvas.SetLeft(image, i);
                        Windows.UI.Xaml.Controls.Canvas.SetTop(image, j);
                        if (j > _tileCanvasHeight)
                            _tileCanvasHeight = j;
                    }
                    if (i > _tileCanvasWidth)
                        _tileCanvasWidth = i;
                }
                //var softBitmap = SoftwareBitmap.CreateCopyFromBuffer(_sourceBitmap.PixelBuffer, BitmapPixelFormat.Bgra8, _sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight);
                //var source = new SoftwareBitmapSource();
                //await source.SetBitmapAsync(softBitmap);
                Children.Add(_tileCanvas);
                _tileCanvas.Height = height;
                _tileCanvas.Width = width;
                var clip = new Windows.UI.Xaml.Media.RectangleGeometry();
                var rect = new Rect(0, 0, width, height);
                clip.Rect = rect;
                _tileCanvas.Clip = clip;
                //canvas.Clip.Bounds.Height = height;
            }
            else if (_rangeLists == null)
            {
                // NinePatch markup was not found
                RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                {
                    Height = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star)
                });
                ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition
                {
                    Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star)
                });

                var image = new Windows.UI.Xaml.Controls.Image
                {
                    // Source = SoftwareBitmap.CreateCopyFromBuffer(_sourceBitmap.PixelBuffer, BitmapPixelFormat.Bgra8, _sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight);
                    Source = _sourceBitmap
                };


                SetColumn(image, 0);
                SetRow(image, 0);

                Children.Add(image);
                UpdateAspect();
            }
            else
            {
                int xPatches = _rangeLists.PatchesX.Count;
                if (xPatches > 0)
                {
                    if (_rangeLists.PatchesX[0].Start > 0)
                        xPatches++;
                    if (_rangeLists.PatchesX.Last<Range>().End < _sourceBitmap.PixelWidth - 1)
                        xPatches++;
                }
                int yPatches = _rangeLists.PatchesY.Count;
                if (yPatches > 0)
                {
                    if (_rangeLists.PatchesY[0].Start > 0)
                        yPatches++;
                    if (_rangeLists.PatchesY.Last<Range>().End < _sourceBitmap.PixelWidth - 1)
                        yPatches++;
                }

                int yPatchRow = 0;
                int yPatchStart;
                int yPatchEnd = 0;

                for (int yRange = 0; yRange < _rangeLists.PatchesY.Count; yRange++)
                {
                    if (_rangeLists.PatchesY[yRange].Start > 0)
                    {
                        // there is a row of fixed height cells before this row 
                        yPatchStart = yPatchEnd + 1;
                        yPatchEnd = (int)_rangeLists.PatchesY[yRange].Start - 1;
                        PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Top);
                        yPatchRow++;

                        RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                        {
                            Height = new Windows.UI.Xaml.GridLength(yPatchEnd - yPatchStart + 1),
                        });
                    }

                    yPatchStart = (int)_rangeLists.PatchesY[yRange].Start;
                    yPatchEnd = (int)_rangeLists.PatchesY[yRange].End;
                    PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Stretch);
                    yPatchRow++;

                    RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                    {
                        Height = new Windows.UI.Xaml.GridLength(yPatchEnd - yPatchStart + 1, Windows.UI.Xaml.GridUnitType.Star),
                    });
                }

                if (yPatchEnd < _sourceBitmap.PixelHeight - 1)
                {
                    // there is a row of fixed height cells after the last range
                    yPatchStart = yPatchEnd + 1;
                    yPatchEnd = _sourceBitmap.PixelHeight - 2;
                    PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Top);

                    RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                    {
                        Height = new Windows.UI.Xaml.GridLength(yPatchEnd - yPatchStart + 1),
                    });
                }
            }
            //stopwatch.Stop();
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("GenerateLayout ["+_instance+"]: "+stopwatch.ElapsedMilliseconds);
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.GenerateLayout[" + _instance + "] ");
        }

        void PatchesForRow(int yPatchRow, int yPatchStart, int yPatchEnd, Windows.UI.Xaml.VerticalAlignment yStretch)
        {
            int xPatchCol = 0;
            int xPatchStart;
            int xPatchEnd = 0;
            int xWidth, yWidth = yPatchEnd - yPatchStart + 1;

            Windows.Foundation.Rect rect;
            Windows.UI.Xaml.Controls.Image image;

            for (int xRange = 0; xRange < _rangeLists.PatchesX.Count; xRange++)
            {
                if (_rangeLists.PatchesX[xRange].Start > 0)
                {
                    // this is a column of fixed width cells before this range
                    xPatchStart = xPatchEnd + 1;
                    xPatchEnd = (int)_rangeLists.PatchesX[xRange].Start - 1;
                    xWidth = xPatchEnd - xPatchStart + 1;
                    rect = new Windows.Foundation.Rect(xPatchStart, yPatchStart, xWidth, yWidth);
                    image = new Windows.UI.Xaml.Controls.Image
                    {
                        Source = _sourceBitmap.Crop(rect),
                        HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                        VerticalAlignment = yStretch,
                        Stretch = Windows.UI.Xaml.Media.Stretch.Fill
                    };
                    SetColumn(image, xPatchCol);
                    SetRow(image, yPatchRow);
                    Children.Add(image);
                    xPatchCol++;

                    if (ColumnDefinitions.Count < xPatchCol)
                    {
                        ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition
                        {
                            Width = new Windows.UI.Xaml.GridLength(xWidth)
                        });
                    }



                }

                xPatchStart = (int)_rangeLists.PatchesX[xRange].Start;
                xPatchEnd = (int)_rangeLists.PatchesX[xRange].End;
                xWidth = xPatchEnd - xPatchStart + 1;
                rect = new Windows.Foundation.Rect(xPatchStart, yPatchStart, xWidth, yWidth);
                image = new Windows.UI.Xaml.Controls.Image
                {
                    Source = _sourceBitmap.Crop(rect),
                    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                    VerticalAlignment = yStretch,
                    Stretch = Windows.UI.Xaml.Media.Stretch.Fill
                };
                SetColumn(image, xPatchCol);
                SetRow(image, yPatchRow);
                Children.Add(image);
                xPatchCol++;

                if (ColumnDefinitions.Count < xPatchCol)
                {
                    ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition
                    {
                        Width = new Windows.UI.Xaml.GridLength(xWidth, Windows.UI.Xaml.GridUnitType.Star)
                    });
                }
            }

            if (xPatchEnd < _sourceBitmap.PixelWidth - 1)
            {
                // there is a column of fixed width cells after the last range
                xPatchStart = xPatchEnd + 1;
                xPatchEnd = _sourceBitmap.PixelWidth - 2;
                xWidth = xPatchEnd - xPatchStart + 1;
                rect = new Windows.Foundation.Rect(xPatchStart, yPatchStart, xWidth, yWidth);
                image = new Windows.UI.Xaml.Controls.Image
                {
                    Source = _sourceBitmap.Crop(rect),
                    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                    VerticalAlignment = yStretch,
                    Stretch = Windows.UI.Xaml.Media.Stretch.Fill
                };
                SetColumn(image, xPatchCol);
                SetRow(image, yPatchRow);
                Children.Add(image);
                xPatchCol++;

                if (ColumnDefinitions.Count < xPatchCol)
                {
                    ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition
                    {
                        Width = new Windows.UI.Xaml.GridLength(xWidth)
                    });
                }
            }
        }

        internal void GenerateOutlineLayout(Windows.Foundation.Size size = default(Windows.Foundation.Size))
        {

            if (RoundedBoxElement == null)
                return;

            var radius = RoundedBoxElement.OutlineRadius;
            bool drawOutline = RoundedBoxElement.OutlineWidth > 0.05 && RoundedBoxElement.OutlineColor.A > 0.01;
            bool drawFill = RoundedBoxElement.BackgroundColor.A > 0.01;

            if (Children.Any((view) => view != _tileCanvas && view is Windows.UI.Xaml.Controls.Canvas))
            {
                var oldCanvas = Children.Where((view) => view != _tileCanvas && view is Windows.UI.Xaml.Controls.Canvas).First();
                Children.Remove(oldCanvas);
            }
            if (!drawFill && !drawOutline)
                return;

            var visualElement = RoundedBoxElement as VisualElement;
            var width = visualElement.Width;
            var height = visualElement.Height;

            if (size != default(Windows.Foundation.Size))
            {
                width = size.Width;
                height = size.Height;
            }
            if (width <= 0 || height <= 0)
                return;

            System.Diagnostics.Debug.WriteLine("====================================================================================");
            System.Diagnostics.Debug.WriteLine("Bounds=[" + visualElement.Bounds + "]  width=[" + width + "] height=[" + height + "]");
            //System.Diagnostics.Debug.WriteLine("BackgroundColor=[" + RoundedBoxElement.BackgroundColor + "]");
            //System.Diagnostics.Debug.WriteLine("OutlineColor=[" + RoundedBoxElement.OutlineColor + "]  OutlineWidth=[" + RoundedBoxElement.OutlineWidth + "]");


            var canvas = new Windows.UI.Xaml.Controls.Canvas();

            var rect = new Rect(0, 0, width, height);
            var outlineWidth = RoundedBoxElement.OutlineWidth;

            SegmentType segmentType = SegmentType.Not;
            var hz = true;
            if (RoundedBoxElement is MaterialButton materialButton)
            {
                segmentType = materialButton.SegmentType;
                hz = materialButton.ParentSegmentsOrientation == StackOrientation.Horizontal;
            }
            else
                materialButton = null;

            //rect = RoundRect(rect, materialButton.Orientation, segmentType);

            var vt = !hz;

            var makeRoomForShadow = RoundedBoxElement.HasShadow && RoundedBoxElement.BackgroundColor.A > 0.01 && !RoundedBoxElement.ShadowInverted;

            var shadowX = Forms9Patch.Settings.ShadowOffset.X;
            var shadowY = Forms9Patch.Settings.ShadowOffset.Y;
            var shadowR = Forms9Patch.Settings.ShadowRadius;

            var shadowColor = Color.FromRgba(0.0, 0.0, 0.0, 0.55).ToWindowsColor();

            var shadowPadding = RoundedBoxBase.ShadowPadding(RoundedBoxElement as Layout);

            Rect perimeter = rect;
            if (makeRoomForShadow)
            {
                // what additional padding was allocated to cast  the button's shadow?
                //CompositionShadow shadow = new CompositionShadow()
                perimeter = new Rect(rect.Left + shadowPadding.Left, rect.Top + shadowPadding.Top, rect.Width - shadowPadding.HorizontalThickness, rect.Height - shadowPadding.VerticalThickness);
                if (segmentType != SegmentType.Not)
                {
                    // if it is a segment, cast the shadow beyond the button's parimeter and clip it (so no overlaps or gaps)
                    double allowance = Math.Abs(shadowX) + Math.Abs(shadowY) + Math.Abs(shadowR);
                    Rect result;
                    if (segmentType == SegmentType.Start)
                        result = new Rect(perimeter.Left - allowance, perimeter.Top - allowance, perimeter.Width + allowance * (hz ? 1 : 2), perimeter.Height + allowance * (vt ? 1 : 2));
                    else if (segmentType == SegmentType.Mid)
                        result = new Rect(perimeter.Left - (hz ? 0 : allowance), perimeter.Top - (vt ? 0 : allowance), perimeter.Width + (hz ? 0 : 2 * allowance), perimeter.Height + (vt ? 0 : 2 * allowance));
                    else
                        result = new Rect(perimeter.Left - (hz ? 0 : allowance), perimeter.Top - (vt ? 0 : allowance), perimeter.Width + allowance * (hz ? 1 : 2), perimeter.Height + allowance * (vt ? 1 : 2));
                    canvas.Clip = new Windows.UI.Xaml.Media.RectangleGeometry
                    {
                        Rect = result
                    };

                }
            }


            // generate background
            if (drawFill)
            {
                GeometryGroup fillPath = null;
                var FillBoundary = perimeter;
                //System.Diagnostics.Debug.WriteLine("FILL: ");
                switch (segmentType)
                {
                    case SegmentType.Not:
                        FillBoundary = RectInset(perimeter, outlineWidth);
                        break;
                    case SegmentType.Start:
                        FillBoundary = RectInset(perimeter, outlineWidth, outlineWidth, vt ? outlineWidth : 0, hz ? outlineWidth : 0);
                        break;
                    case SegmentType.Mid:
                        FillBoundary = RectInset(perimeter, outlineWidth, outlineWidth, vt ? outlineWidth : 0, hz ? outlineWidth : 0);
                        break;
                    case SegmentType.End:
                        FillBoundary = RectInset(perimeter, outlineWidth);
                        break;
                }
                if (segmentType == SegmentType.Not)
                {
                    if (RoundedBoxElement.IsElliptical)
                        fillPath = Ellipse(FillBoundary);
                    else
                        fillPath = RectangularPerimeterPath(RoundedBoxElement, FillBoundary, radius - (drawOutline ? outlineWidth : 0));
                }
                else
                {
                    // make the button bigger on the overlap sides so the mask can trim off excess, including shadow
                    //Rect newPerimenter = SegmentAllowanceRect(outlinePerimeter, 0, materialButton.Orientation, materialButton.SegmentType);
                    fillPath = RectangularPerimeterPath(RoundedBoxElement, FillBoundary, radius - (drawOutline ? outlineWidth : 0));
                }
                var path = new Path();
                path.Fill = new SolidColorBrush(RoundedBoxElement.BackgroundColor.ToWindowsColor());
                path.Data = fillPath;
                canvas.Children.Add(path);
            }


            if (drawOutline)
            {
                GeometryGroup outlinePath = null;
                var outlineBoundary = perimeter;
                switch (segmentType)
                {
                    case SegmentType.Not:
                        outlineBoundary = RectInset(perimeter, outlineWidth / 2);
                        break;
                    case SegmentType.Start:
                        outlineBoundary = RectInset(perimeter, outlineWidth / 2, outlineWidth / 2, vt ? outlineWidth / 2 : 0, hz ? outlineWidth / 2 : 0);
                        break;
                    case SegmentType.Mid:
                        outlineBoundary = RectInset(perimeter, outlineWidth / 2, outlineWidth / 2, vt ? outlineWidth / 2 : 0, hz ? outlineWidth / 2 : 0);
                        break;
                    case SegmentType.End:
                        outlineBoundary = RectInset(perimeter, outlineWidth / 2);
                        break;
                }
                if (segmentType == SegmentType.Not)
                {
                    if (RoundedBoxElement.IsElliptical)
                        outlinePath = Ellipse(outlineBoundary);
                    else
                        outlinePath = RectangularPerimeterPath(RoundedBoxElement, outlineBoundary, radius - (drawOutline ? outlineWidth / 2 : 0));
                }
                else
                {
                    // make the button bigger on the overlap sides so the mask can trim off excess, including shadow
                    //Rect newPerimenter = SegmentAllowanceRect(outlinePerimeter, 0, materialButton.Orientation, materialButton.SegmentType);
                    outlinePath = RectangularPerimeterPath(RoundedBoxElement, outlineBoundary, radius - (drawOutline ? outlineWidth / 2 : 0));
                }
                var path = new Path();
                path.Stroke = new SolidColorBrush(RoundedBoxElement.OutlineColor.ToWindowsColor());
                path.StrokeThickness = RoundedBoxElement.OutlineWidth;
                path.Data = outlinePath;
                canvas.Children.Add(path);
            }


            Children.Add(canvas);

            System.Diagnostics.Debug.WriteLine("====================================================================================");
        }
        #endregion





        #region RoundedBoxLayout Support

        static Rect RoundRect(Rect rect, StackOrientation orientation, Forms9Patch.SegmentType type)
        {
            return rect;

            var left = Math.Ceiling(rect.Left);
            var right = Math.Floor(rect.Right);
            var top = Math.Ceiling(rect.Top);
            var bottom = Math.Floor(rect.Bottom);


            if (orientation == StackOrientation.Horizontal)
            {
                switch (type)
                {
                    case SegmentType.Start:
                        right = Math.Round(rect.Right);
                        break;
                    case SegmentType.Mid:
                        left = Math.Round(rect.Left);
                        right = Math.Round(rect.Right);
                        break;
                    case SegmentType.End:
                        left = Math.Round(rect.Left);
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case SegmentType.Start:
                        bottom = Math.Round(rect.Bottom);
                        break;
                    case SegmentType.Mid:
                        bottom = Math.Round(rect.Bottom);
                        top = Math.Round(rect.Top);
                        break;
                    case SegmentType.End:
                        top = Math.Round(rect.Top);
                        break;
                }
            }
            return new Rect(left, top, right - left, bottom - top);

        }

        static Rect SegmentAllowanceRect(Rect rect, double allowance, StackOrientation orientation, Forms9Patch.SegmentType type)
        {
            Rect result;
            switch (type)
            {
                case SegmentType.Start:
                    result = new Rect(rect.Left, rect.Top, rect.Width + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance : 0));
                    break;
                case SegmentType.Mid:
                    result = new Rect(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == StackOrientation.Horizontal ? allowance * 2 : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance * 2 : 0));
                    break;
                case SegmentType.End:
                    result = new Rect(rect.Left - (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Top - (orientation == StackOrientation.Vertical ? allowance : 0), rect.Width + (orientation == StackOrientation.Horizontal ? allowance : 0), rect.Height + (orientation == StackOrientation.Vertical ? allowance : 0));
                    break;
                default:
                    result = rect;
                    break;
            }
            System.Diagnostics.Debug.WriteLine("ImageView.SegmentAllowanceRect result=[" + result + "]");
            return result;
        }

        static Rect RectInset(Rect rect, double inset)
        {
            return RectInset(rect, inset, inset, inset, inset);
        }

        static Rect RectInset(Rect rect, double left, double top, double right, double bottom)
        {
            return new Rect(rect.X + left, rect.Y + top, rect.Width - left - right, rect.Height - top - bottom);
        }

        internal static GeometryGroup Ellipse(Rect perimeter)
        {
            var geometryGroup = new GeometryGroup
            {
                Children =
                                {
                                    new EllipseGeometry
                                    {
                                        Center = new Windows.Foundation.Point(perimeter.X + perimeter.Width/2, perimeter.Y + perimeter.Height/2),
                                        RadiusX = perimeter.Width/2,
                                        RadiusY = perimeter.Height/2
                                    }
                                }
            };
            return geometryGroup;
        }

        internal static GeometryGroup RectangularPerimeterPath(Forms9Patch.IRoundedBox element, Rect rect, float radius)
        {
            radius = Math.Max(radius, 0);

            var materialButton = element as MaterialButton;
            SegmentType type = materialButton == null ? SegmentType.Not : materialButton.SegmentType;
            StackOrientation orientation = materialButton == null ? StackOrientation.Horizontal : materialButton.ParentSegmentsOrientation;

            var pathFigure = new PathFigure();

            //System.Diagnostics.Debug.WriteLine("RectangularPerimeterPath rect[" + rect + "] type=["+type+"] ");
            //rect = RoundRect(rect, orientation, type);
            //System.Diagnostics.Debug.WriteLine("RectangularPerimeterPath rect[" + rect + "] type=[" + type + "] ");
            //System.Diagnostics.Debug.WriteLine("");

            if (type == SegmentType.Not)
            {
                pathFigure.SetStartPoint((rect.Left + rect.Right) / 2, rect.Top);
                pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                if (radius > 0)
                    pathFigure.AddArcToPoint(rect.Left, rect.Top + radius, radius, SweepDirection.Counterclockwise);
                pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                if (radius > 0)
                    pathFigure.AddArcToPoint(rect.Left + radius, rect.Bottom, radius, SweepDirection.Counterclockwise);
                pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                if (radius > 0)
                    pathFigure.AddArcToPoint(rect.Right, rect.Bottom - radius, radius, SweepDirection.Counterclockwise);
                pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                if (radius > 0)
                    pathFigure.AddArcToPoint(rect.Right - radius, rect.Top, radius, SweepDirection.Counterclockwise);
                pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
            }
            else if (type == SegmentType.Start)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    pathFigure.SetStartPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Left, rect.Top + radius, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Left + radius, rect.Bottom, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    pathFigure.SetStartPoint(rect.Right, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Right - radius, rect.Top, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Left + radius, rect.Top);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Left, rect.Top + radius, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == SegmentType.Mid)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    pathFigure.SetStartPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Bottom);
                }
                else
                {
                    pathFigure.SetStartPoint(rect.Right, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                }
            }
            else if (type == SegmentType.End)
            {
                if (orientation == StackOrientation.Horizontal)
                {
                    pathFigure.SetStartPoint((rect.Left + rect.Right) / 2, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom);
                    pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Right, rect.Bottom - radius, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top + radius);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Right - radius, rect.Top, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
                else
                {
                    pathFigure.SetStartPoint((rect.Left + rect.Right) / 2, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Top);
                    pathFigure.AddLineToPoint(rect.Left, rect.Bottom - radius);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Left + radius, rect.Bottom, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Right - radius, rect.Bottom);
                    if (radius > 0)
                        pathFigure.AddArcToPoint(rect.Right, rect.Bottom - radius, radius, SweepDirection.Counterclockwise);
                    pathFigure.AddLineToPoint(rect.Right, rect.Top);
                    pathFigure.AddLineToPoint((rect.Left + rect.Right) / 2, rect.Top);
                }
            }

            var geometryGroup = new GeometryGroup
            {
                Children =
                        {
                            new PathGeometry
                            {
                                Figures =
                                {
                                    pathFigure
                                }
                            }
                        }
            };
            return geometryGroup;
        }


        #endregion

    }
}
