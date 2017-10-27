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

        bool _actualSizeIsValid = false;

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
            //GenerateImageLayout();

            if (_debugMessages) System.Diagnostics.Debug.WriteLine("["+_instance+"]["+PCL.Utils.ReflectionExtensions.CallerMemberName()+"] ImageElement.Size=["+_imageElement.Bounds.Size+"] Size=["+Width+", "+Height+"] ActualSize=["+ActualWidth+", "+ActualHeight+"]");
        }

        private void OnImageElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.Image.SourceProperty.PropertyName)
                SetSourceAsync();
            else if (e.PropertyName == Forms9Patch.Image.CapInsetsProperty.PropertyName)
            {
                _validImageLayout = false;
                GenerateImageLayout();
            }
            else if (e.PropertyName == Forms9Patch.Image.FillProperty.PropertyName)
                GenerateImageLayout();
            else if (e.PropertyName == Forms9Patch.Image.TintColorProperty.PropertyName)
                TintImage();
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
                    return Xamarin.Forms.Size.Zero;

                var result = new Xamarin.Forms.Size(_sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight);
                if (_rangeLists != null)
                {
                    result.Width -= 2;
                    result.Height -= 2;
                }
                return result;
            }
        }



        internal async Task SetSourceAsync()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageView.SetSourceAsync ENTER");
            if (ImageElement?.Source == null)
            {
                ImmediatelyClearImageElements();
                return;
            }
            if (ImageElement.Source != _xfImageSource)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                ((IImageController)ImageElement)?.SetIsLoading(true);
                if (Settings.IsLicenseValid || _instance < 4)
                    _xfImageSource = ImageElement?.Source;
                else
                    _xfImageSource = Forms9Patch.ImageSource.FromMultiResource("Forms9Patch.Resources.unlicensedcopy");
                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine("["+_instance+"]["+PCL.Utils.ReflectionExtensions.CallerMemberName()+"] A["+stopWatch.ElapsedMilliseconds+"]");
                stopWatch.Reset();
                stopWatch.Start();
                //_xfImageSource = ImageElement.Source;
                Windows.UI.Xaml.Media.ImageSource imagesource = null;
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
                stopWatch.Stop();
                System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] B[" + stopWatch.ElapsedMilliseconds + "]");

                if (handler != null && _xfImageSource != null)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
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

                    stopWatch.Stop();
                    System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] C[" + stopWatch.ElapsedMilliseconds + "]");
                    stopWatch.Reset();
                    stopWatch.Start();

                    if (imagesource is WriteableBitmap bitmap)
                        _sourceBitmap = bitmap;
                    else
                        _sourceBitmap = null;
                    _validImageLayout = false;
                    _tileCanvas = null;


                    GenerateImageLayout();

                    stopWatch.Stop();
                    System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] D[" + stopWatch.ElapsedMilliseconds + "]");
                    stopWatch.Reset();
                    stopWatch.Start();


                }
            }
            ((IImageController)ImageElement)?.SetIsLoading(false);
            //RefreshImage();
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("["+_instance+"]ImageView.SetSourceAsync EXIT");
        }

        #endregion Property Management


        #region Constructor
        public ImageView(int instance)
        {
            _instance = instance;
            //BorderThickness = new Windows.UI.Xaml.Thickness(1);
            //BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Pink);
            Padding = new Windows.UI.Xaml.Thickness();
            SizeChanged += ImageView_SizeChanged;
        }

        private void ImageView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (_imageElement!=null)
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName()+ "] ImageElement.Size=[" + _imageElement.Bounds.Size + "] Size=[" + Width+", "+Height+"] ActualSize=["+ActualWidth+", "+ActualHeight+"]");
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


        #region Image Support 
        void RefreshImage()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageView[" + _instance + "].RefreshImage()");
            ((IVisualElementController)ImageElement)?.InvalidateMeasure(Xamarin.Forms.Internals.InvalidationTrigger.RendererReady);
            _actualSizeIsValid = false;
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageView[" + _instance + "].RefreshImage() EXIT ");
        }

        void UpdateAspect()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageRenderer.UpdateAspect[" + _instance + "]");
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
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageRenderer.UpdateAspect[" + _instance + "] RETURN");
        }

        void ImmediatelyClearImageElements()
        {
            if (Children.Any())
            {
                RowDefinitions.Clear();
                ColumnDefinitions.Clear();
                Children.Clear();
            }
            /*
            if (Children.Contains(_tileCanvas))
                Children.Remove(_tileCanvas);
            if (Children.Any((view) => view is Windows.UI.Xaml.Controls.Image))
            {
                var oldImageView = Children.Where((view) => view is Windows.UI.Xaml.Controls.Image).First();
                Children.Remove(oldImageView);
            }
            */
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + availableSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");
            var result = base.MeasureOverride(availableSize);

            if (_sourceBitmap != null)
            {
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.PixelWidth + ", " + _sourceBitmap.PixelHeight + "]");
            }
            else
            {
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap is null");
            }


            if (!_actualSizeIsValid && _sourceBitmap != null)
            {
                bool constrainedWidth = !double.IsInfinity(availableSize.Width) || ImageElement.WidthRequest > -1;
                bool constrainedHeight = !double.IsInfinity(availableSize.Height) || ImageElement.HeightRequest > -1;

                double constrainedWidthValue = availableSize.Width;
                if (ImageElement.WidthRequest > -1)
                    constrainedWidthValue = Math.Min(constrainedWidthValue, ImageElement.WidthRequest);
                double constrainedHeightValue = availableSize.Height;
                if (ImageElement.HeightRequest > -1)
                    constrainedHeightValue = Math.Min(constrainedHeightValue, ImageElement.HeightRequest);

                if ((!constrainedWidth && !constrainedHeight) || ImageElement.Fill == Fill.None)
                    result = new Windows.Foundation.Size(BaseImageSize.Width, BaseImageSize.Height);
                else
                {
                    var sourceAspect = BaseImageSize.Height / BaseImageSize.Width;

                    if (constrainedWidth && constrainedHeight)
                    {
                        // if single image, SetAspect should do all the heavy lifting.  if stitched together, then it's ImageFill.Fill;
                        result = new Windows.Foundation.Size(constrainedWidthValue, constrainedHeightValue);
                    }
                    else if (constrainedWidth)
                    {
                        if (ImageElement.Fill == Fill.Tile || ImageElement.Fill == Fill.Fill)
                            result = new Windows.Foundation.Size(constrainedWidthValue, availableSize.Height);
                        else
                            result = new Windows.Foundation.Size(constrainedWidthValue, constrainedWidthValue * sourceAspect);
                    }
                    else if (constrainedHeight)
                    {
                        if (ImageElement.Fill == Fill.Tile || ImageElement.Fill == Fill.Fill)
                            result = new Windows.Foundation.Size(availableSize.Width, constrainedHeightValue);
                        else
                            result = new Windows.Foundation.Size(constrainedHeightValue / sourceAspect, constrainedHeightValue);
                    }
                }
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.PixelWidth + ", " + _sourceBitmap.PixelHeight + "]");
            }

            return result;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] availableSize=[" + finalSize + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "]");
            var result = base.ArrangeOverride(finalSize);

            if (!_actualSizeIsValid && _sourceBitmap != null && Children.Count > 0)
            {
                _validImageLayout = false;
                GenerateImageLayout(finalSize);
                _actualSizeIsValid = true;
            }

            if (_debugMessages)
            {
                if (_sourceBitmap != null)
                    System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap.Size=[" + _sourceBitmap.PixelWidth + ", " + _sourceBitmap.PixelHeight + "]");
                else
                    System.Diagnostics.Debug.WriteLine("[" + _instance + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "] ActualSize=[" + ActualWidth + ", " + ActualHeight + "] _sourceBitmap is null");
            }

            return result;
        }

        bool _validImageLayout;
        internal void GenerateImageLayout(Windows.Foundation.Size size = default(Windows.Foundation.Size))
        {
            if (_validImageLayout)
                return;
            // clear out existing image
            ImmediatelyClearImageElements();

            // return if no image
            if (_sourceBitmap == null)
                return;

            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageView.GenerateLayout[" + _instance + "]  Fill=[" + ImageElement.Fill + "] W,H=[" + Width + "," + Height + "] ActualWH=[" + ActualWidth + "," + ActualHeight + "] size=[" + size + "]");


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

                TintImage();

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

                List<int> rowHeights = new List<int>();
                List<int> minRowHeights = new List<int>();

                int minHeight = 0;
                int rowHeight;
                for (int yRange = 0; yRange < _rangeLists.PatchesY.Count; yRange++)
                {
                    if (_rangeLists.PatchesY[yRange].Start > 0)
                    {
                        // there is a row of fixed height cells before this row 
                        yPatchStart = yPatchEnd + 1;
                        yPatchEnd = (int)_rangeLists.PatchesY[yRange].Start - 1;
                        PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Top, size.Width);
                        yPatchRow++;

                        rowHeight = yPatchEnd - yPatchStart + 1;
                        rowHeights.Add(rowHeight);
                        minRowHeights.Add(rowHeight);
                        minHeight += rowHeight;
                    }

                    yPatchStart = (int)_rangeLists.PatchesY[yRange].Start;
                    yPatchEnd = (int)_rangeLists.PatchesY[yRange].End;
                    PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Stretch, size.Width);
                    yPatchRow++;

                    rowHeight = yPatchEnd - yPatchStart + 1;
                    rowHeights.Add(rowHeight);
                    minRowHeights.Add(0);
                }

                if (yPatchEnd < _sourceBitmap.PixelHeight - 1)
                {
                    // there is a row of fixed height cells after the last range
                    yPatchStart = yPatchEnd + 1;
                    yPatchEnd = _sourceBitmap.PixelHeight - 2;
                    PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Top, size.Width);

                    rowHeight = yPatchEnd - yPatchStart + 1;
                    rowHeights.Add(rowHeight);
                    minRowHeights.Add(rowHeight);
                    minHeight += rowHeight;
                }

                var availHeight = size.Height > 0 ? size.Height : ActualHeight;
                if (availHeight == 0)
                    availHeight = _sourceBitmap.PixelHeight;
                bool scaledRows = minHeight > availHeight;
                for (int row= 0; row < rowHeights.Count; row++)
                {
                    double height = scaledRows ? minRowHeights[row] * availHeight / minHeight : rowHeights[row];
                    var gridLength = new Windows.UI.Xaml.GridLength(height, minRowHeights[row] == 0 ? Windows.UI.Xaml.GridUnitType.Star : Windows.UI.Xaml.GridUnitType.Pixel);
                    if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]rowDefinition.Height=[" + gridLength+"]");
                    var rowDefinition = new Windows.UI.Xaml.Controls.RowDefinition
                    {
                        Height = gridLength
                    };
                    RowDefinitions.Add(rowDefinition);
                }
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]ImageView.GenerateLayout[" + _instance + "] EXIT");
            _validImageLayout = true;

            RefreshImage();
        }

        void PatchesForRow(int yPatchRow, int yPatchStart, int yPatchEnd, Windows.UI.Xaml.VerticalAlignment yStretch, double sizeWidth)
        {
            int xPatchCol = 0;
            int xPatchStart;
            int xPatchEnd = 0;
            int xWidth, yWidth = yPatchEnd - yPatchStart + 1;

            Windows.Foundation.Rect rect;
            Windows.UI.Xaml.Controls.Image image;

            List<int> colWidths = new List<int>();
            List<int> minColWidths = new List<int>();
            int minWidth = 0;

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

                    colWidths.Add(xWidth);
                    minColWidths.Add(xWidth);
                    minWidth += xWidth;
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

                colWidths.Add(xWidth);
                minColWidths.Add(0);
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

                colWidths.Add(xWidth);
                minColWidths.Add(xWidth);
                minWidth += xWidth;
            }

            if (ColumnDefinitions.Count < xPatchCol)
            {
                var availWidth = sizeWidth > 0 ? sizeWidth : ActualWidth;
                if (availWidth == 0)
                    availWidth = _sourceBitmap.PixelWidth;
                bool scaledColumns = minWidth > availWidth;
                for (int col = 0; col < colWidths.Count; col++)
                {
                    double height = scaledColumns ? minColWidths[col] * availWidth / minWidth : colWidths[col];
                    var gridLenth = new Windows.UI.Xaml.GridLength(height, minColWidths[col] == 0 ? Windows.UI.Xaml.GridUnitType.Star : Windows.UI.Xaml.GridUnitType.Pixel);
                    var colDefinition = new Windows.UI.Xaml.Controls.ColumnDefinition
                    {
                        Width = gridLenth
                    };
                    ColumnDefinitions.Add(colDefinition);
                    System.Diagnostics.Debug.WriteLine("["+_instance+"]["+PCL.Utils.ReflectionExtensions.CallerMemberName()+"] colDefinition:["+gridLenth+"]");
                }
            }

        }

        void TintImage([System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            if (ImageElement?.Source != null && Children.Count == 1 && Children[0] is Windows.UI.Xaml.Controls.Image image && image.Source is WriteableBitmap bitmap)
            {
                if (ImageElement.TintColor != Xamarin.Forms.Color.Default)
                    bitmap.ForEach((x, y, color) => Windows.UI.Color.FromArgb(color.A, (byte)(ImageElement.TintColor.R * 255), (byte)(ImageElement.TintColor.G * 255), (byte)(ImageElement.TintColor.B * 255)));
                else if (callerName != "GenerateImageLayout")
                {
                    _validImageLayout = false;
                    GenerateImageLayout();
                }
            }
        }
        #endregion


        #region RoundedBoxLayout Support

        internal void GenerateOutlineLayout(Windows.Foundation.Size size = default(Windows.Foundation.Size))
        {
            if (RoundedBoxElement == null || ImageElement != null)
                return;

            ImmediatelyClearImageElements();

            if (RoundedBoxElement is MaterialSegmentedControl materialSegmentedControl)
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
            var rect = new Rect(0, 0, width, height);
            var canvas = new Windows.UI.Xaml.Controls.Canvas();

            SegmentType segmentType = SegmentType.Not;
            var hz = true;
            if (RoundedBoxElement is MaterialButton materialButton)
            {
                segmentType = materialButton.SegmentType;
                hz = materialButton.ParentSegmentsOrientation == StackOrientation.Horizontal;
            }
            else
                materialButton = null;
            var vt = !hz;

            double separatorWidth = materialButton == null || segmentType == SegmentType.Not ? 0 : materialButton.SeparatorWidth < 0 ? _roundedBoxElement.OutlineWidth : Math.Max(0, materialButton.SeparatorWidth);
            if (_roundedBoxElement.BackgroundColor.A < 0.01 && (_roundedBoxElement.OutlineColor.A < 0.01 || (_roundedBoxElement.OutlineWidth < 0.01 && separatorWidth < 0.01)))
                return;

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
                if (!RoundedBoxElement.ShadowInverted)
                {
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
                        FillBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth);
                        break;
                    case SegmentType.Start:
                        FillBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth, RoundedBoxElement.OutlineWidth, vt ? RoundedBoxElement.OutlineWidth : 0, hz ? RoundedBoxElement.OutlineWidth : 0);
                        break;
                    case SegmentType.Mid:
                        FillBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth, RoundedBoxElement.OutlineWidth, vt ? RoundedBoxElement.OutlineWidth : 0, hz ? RoundedBoxElement.OutlineWidth : 0);
                        break;
                    case SegmentType.End:
                        FillBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth);
                        break;
                }
                if (segmentType == SegmentType.Not)
                {
                    if (RoundedBoxElement.IsElliptical)
                        fillPath = Ellipse(FillBoundary);
                    else
                        fillPath = RectangularPerimeterPath(RoundedBoxElement, FillBoundary, radius - (drawOutline ? RoundedBoxElement.OutlineWidth : 0));
                }
                else
                {
                    // make the button bigger on the overlap sides so the mask can trim off excess, including shadow
                    //Rect newPerimenter = SegmentAllowanceRect(outlinePerimeter, 0, materialButton.Orientation, materialButton.SegmentType);
                    fillPath = RectangularPerimeterPath(RoundedBoxElement, FillBoundary, radius - (drawOutline ? RoundedBoxElement.OutlineWidth : 0));
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
                        outlineBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth / 2);
                        break;
                    case SegmentType.Start:
                        outlineBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth / 2, RoundedBoxElement.OutlineWidth / 2, vt ? RoundedBoxElement.OutlineWidth / 2 : 0, hz ? RoundedBoxElement.OutlineWidth / 2 : 0);
                        break;
                    case SegmentType.Mid:
                        outlineBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth / 2, RoundedBoxElement.OutlineWidth / 2, vt ? RoundedBoxElement.OutlineWidth / 2 : 0, hz ? RoundedBoxElement.OutlineWidth / 2 : 0);
                        break;
                    case SegmentType.End:
                        outlineBoundary = RectInset(perimeter, RoundedBoxElement.OutlineWidth / 2);
                        break;
                }
                if (segmentType == SegmentType.Not)
                {
                    if (RoundedBoxElement.IsElliptical)
                        outlinePath = Ellipse(outlineBoundary);
                    else
                        outlinePath = RectangularPerimeterPath(RoundedBoxElement, outlineBoundary, radius - (drawOutline ? RoundedBoxElement.OutlineWidth / 2 : 0));
                }
                else
                {
                    // make the button bigger on the overlap sides so the mask can trim off excess, including shadow
                    //Rect newPerimenter = SegmentAllowanceRect(outlinePerimeter, 0, materialButton.Orientation, materialButton.SegmentType);
                    outlinePath = RectangularPerimeterPath(RoundedBoxElement, outlineBoundary, radius - (drawOutline ? RoundedBoxElement.OutlineWidth / 2 : 0));
                }
                var path = new Path();
                path.Stroke = new SolidColorBrush(RoundedBoxElement.OutlineColor.ToWindowsColor());
                path.StrokeThickness = RoundedBoxElement.OutlineWidth;
                path.Data = outlinePath;
                canvas.Children.Add(path);
            }

            // separators
            if (materialButton!=null && RoundedBoxElement.OutlineWidth < 0.05 && separatorWidth > 0 && !_roundedBoxElement.IsElliptical)
            {
                var inset = RoundedBoxElement.OutlineColor.A > 0 ? separatorWidth  / 2.0f : 0;
                var line = new Line();
                line.Stroke = new SolidColorBrush(materialButton.OutlineColor.ToWindowsColor());
                line.StrokeThickness = separatorWidth;
                if (segmentType == SegmentType.Start || segmentType == SegmentType.Mid)
                {
                    if (hz)
                    {
                        line.X1 = perimeter.Right;
                        line.Y1 = perimeter.Top + inset;
                        line.X2 = line.X1;
                        line.Y2 = perimeter.Bottom - inset;
                    }
                    else
                    {
                        line.X1 = perimeter.Left + inset;
                        line.Y1 = perimeter.Bottom;
                        line.X2 = perimeter.Right - inset;
                        line.Y2 = line.Y1;
                    }
                }
                if (segmentType == SegmentType.Mid || segmentType == SegmentType.End)
                {
                    if (hz)
                    {
                        line.X1 = perimeter.Left;
                        line.Y1 = perimeter.Top + inset;
                        line.X2 = line.X1;
                        line.Y2 = perimeter.Bottom - inset;
                    }
                    else
                    {
                        line.X1 = perimeter.Left + inset;
                        line.Y1 = perimeter.Top;
                        line.X2 = perimeter.Right - inset;
                        line.Y2 = line.Y1;
                    }
                }
                canvas.Children.Add(line);

                /*
                g.SetShadow(new CGSize(0, 0), 0, Color.Transparent.ToCGColor());
                nfloat inset = _roundedBoxElement.OutlineColor.A > 0 ? _roundedBoxElement.OutlineWidth / 2.0f : 0;
                g.SetStrokeColor(_roundedBoxElement.OutlineColor.ToCGColor());
                g.SetLineWidth(separatorWidth);
                if (_segmentType == SegmentType.Start || _segmentType == SegmentType.Mid)
                {
                    if (_hz)
                    {
                        //g.MoveTo (perimeter.Right, perimeter.Top + inset);
                        //g.AddLineToPoint (perimeter.Right, perimeter.Bottom - inset);
                        g.MoveTo((nfloat)Math.Ceiling(perimeter.Right), perimeter.Top + inset);
                        g.AddLineToPoint((nfloat)Math.Ceiling(perimeter.Right), perimeter.Bottom - inset);
                    }
                    else
                    {
                        g.MoveTo(perimeter.Left + inset, (nfloat)Math.Ceiling(perimeter.Bottom));
                        g.AddLineToPoint(perimeter.Right - inset, (nfloat)Math.Ceiling(perimeter.Bottom));
                    }
                }
                if (_segmentType == SegmentType.Mid || _segmentType == SegmentType.End)
                {
                    if (_hz)
                    {
                        g.MoveTo((nfloat)Math.Round(perimeter.Left), perimeter.Top + inset);
                        g.AddLineToPoint((nfloat)Math.Round(perimeter.Left), perimeter.Bottom - inset);
                    }
                    else
                    {
                        g.MoveTo(perimeter.Left + inset, (nfloat)Math.Round(perimeter.Top));
                        g.AddLineToPoint(perimeter.Right - inset, (nfloat)Math.Round(perimeter.Top));
                    }
                }

                g.StrokePath();
                */
            }



            Children.Add(canvas);

            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + _instance + "]====================================================================================");
        }


        static Rect RoundRect(Rect rect, StackOrientation orientation, Forms9Patch.SegmentType type)
        {
            return rect;
            /*
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
            */
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
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SegmentAllowanceRect result=[" + result + "]");
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
