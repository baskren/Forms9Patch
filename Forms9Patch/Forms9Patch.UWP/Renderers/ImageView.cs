using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

namespace Forms9Patch.UWP 
{
    public class ImageView : Windows.UI.Xaml.Controls.Grid, IDisposable
    {
        #region Fields
        bool _debugMessages = true;

        RangeLists _rangeLists=null;

        int _instance;
        int _tileCanvasWidth = 0;
        int _tileCanvasHeight = 0;

        bool _imageInvalid = true;
        Windows.UI.Xaml.Controls.Canvas _tileCanvas;
        bool _roundedBoxInvalid = true;
        Windows.UI.Xaml.Controls.Canvas _roundedBoxCanvas;

        #endregion


        #region Image Properties

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
                if (_rangeLists!=null)
                {
                    result.Width -= 2;
                    result.Height -= 2;
                }
                if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.set_BaseImageSize[" + _instance + "]: " + result);
                return result;
            }
        }

        WriteableBitmap _sourceBitmap;
        Xamarin.Forms.ImageSource _xfImageSource;
        internal async Task SetSourceAsync(Xamarin.Forms.ImageSource source)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SetSourceAsync enter");
            if (source != _xfImageSource)
            {
                _imageInvalid = true;
                _xfImageSource = source;
                Xamarin.Forms.Platform.UWP.IImageSourceHandler handler = null;
                if (source != null)
                {
                    if (source is FileImageSource)
                        handler = new FileImageSourceHandler();
                    else if (source is UriImageSource)
                        handler = new UriImageSourceHandler();
                    else if (source is StreamImageSource)
                        handler = new StreamImageSourceHandler();
                }
                if (handler != null)
                {
                    Windows.UI.Xaml.Media.ImageSource imagesource;
                    try
                    {
                        imagesource = await handler.LoadImageAsync(source);
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


                    GenerateImageLayout();
                }
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.SetSourceAsync enter");
        }

        Fill _fill;
        internal Forms9Patch.Fill Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                if (value != _fill)
                {
                    _imageInvalid = true;
                    if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.set_Fill[" + _instance + "]: " + value);
                    _fill = value;
                    GenerateImageLayout();
                    UpdateAspect();
                }
            }
        }

        Xamarin.Forms.Thickness _capInsets;
        internal Xamarin.Forms.Thickness CapInsets
        {
            get
            {
                return _capInsets;
            }
            set
            {
                if (value != _capInsets)
                {
                    _imageInvalid = true;
                    _capInsets = value;
                    GenerateImageLayout();
                }
            }
        }
        #endregion Image Properties


        #region RoundedBox Properties
        Windows.UI.Color _backgroundColor;
        internal Color BackgroundColor
        {
            get
            {
                return _backgroundColor.ToXfColor();
            }
            set
            {
                var winColor = value.ToWindowsColor();
                if (winColor != _backgroundColor)
                {
                    _backgroundColor = winColor;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        bool _hasShadow;
        internal bool HasShadown
        {
            get { return _hasShadow; }
            set
            {
                if (value != _hasShadow)
                {
                    _hasShadow = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        bool _shadowInverted;
        internal bool ShadowInverted
        {
            get { return _shadowInverted; }
            set
            {
                if (value != _shadowInverted)
                {
                    _shadowInverted = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        Windows.UI.Color _outlineColor;
        internal Color OutlineColor
        {
            get
            {
                return _outlineColor.ToXfColor();
            }
            set
            {
                var winColor = value.ToWindowsColor(); 
                if (winColor != _outlineColor)
                {
                    _outlineColor = winColor;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        float _outlineRadius;
        internal float OutlineRadius
        {
            get { return _outlineRadius; }
            set
            {
                if (_outlineRadius!=value)
                {
                    _outlineRadius = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        float _outlineWidth;
        internal float OutlineWidth
        {
            get { return _outlineWidth; }
            set
            {
                if (_outlineWidth != value)
                {
                    _outlineWidth = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        bool _isElliptical;
        internal bool IsElliptical
        {
            get { return _isElliptical; }
            set
            {
                if (_isElliptical != value)
                {
                    _isElliptical = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }
        #endregion RoundedBoxProperties

        #region MaterialButton Properties
        StackOrientation _orientation;
        internal StackOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (value!=_orientation)
                {
                    _orientation = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        SegmentType _segmentType;
        internal SegmentType SegmentType
        {
            get { return _segmentType; }
            set
            {
                if (value != _segmentType)
                {
                    _segmentType = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        StackOrientation _parentSegmentOrientation;
        internal StackOrientation ParentSegmentOrientation
        {
            get { return _parentSegmentOrientation; }
            set
            {
                if (value != _parentSegmentOrientation)
                {
                    _parentSegmentOrientation = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        float _separatorWidth;
        internal float SeparatorWidth
        {
            get { return _separatorWidth; }
            set
            {
                if (_separatorWidth!=value)
                {
                    _separatorWidth = value;
                    _roundedBoxInvalid = true;
                    GenerateOutlineLayout();
                }
            }
        }

        internal Func<Xamarin.Forms.Thickness> ShadowPaddingFunc;
        internal Xamarin.Forms.Thickness ShadowPadding
        {
            get
            {
                return ShadowPaddingFunc?.Invoke() ?? default(Xamarin.Forms.Thickness);
            }
        }
        #endregion


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
                    ShadowPaddingFunc = null;
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

        void UpdateAspect()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect[" + _instance + "]");
            if (_disposed)
                return;

            if (Children.Count == 1 && Children[0] is Windows.UI.Xaml.Controls.Image image)
            {
                // it's a simple image
                
                if (Fill == Fill.Tile || Fill == Fill.None)
                {
                    image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                    image.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                }
                else
                {
                    image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    image.Stretch = Fill.ToStretch();
                }
                return;
            }
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect[" + _instance + "] RETURN");
        }


        internal void GenerateImageLayout(Windows.Foundation.Size size = default(Windows.Foundation.Size))
        {
            if (!_imageInvalid)
                return;
            _imageInvalid = false;
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.GenerateLayout[" + _instance + "]  Fill=[" + Fill+"] W,H=["+Width+","+Height+"] ActualWH=["+ActualWidth+","+ActualHeight+"] size=["+size+"]");
            if (_sourceBitmap == null )
                return;
            //Stopwatch stopwatch = Stopwatch.StartNew();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            Children.Clear();

            _rangeLists = _sourceBitmap?.NinePatchRanges();
            bool sourceBitmapIsNinePatch = _rangeLists!=null;
            _rangeLists = CapInsets.ToRangeLists(_sourceBitmap.PixelWidth, _sourceBitmap.PixelHeight, _xfImageSource, sourceBitmapIsNinePatch) ?? _rangeLists;  

            if (Fill == Fill.Tile)
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
                    Height = new Windows.UI.Xaml.GridLength(1,Windows.UI.Xaml.GridUnitType.Star)
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
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageView.GenerateLayout["+_instance+"] ");
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
                    xPatchEnd = (int)_rangeLists.PatchesX[xRange].Start-1;
                    xWidth = xPatchEnd - xPatchStart + 1;
                    rect = new Windows.Foundation.Rect(xPatchStart, yPatchStart, xWidth, yWidth);
                    image = new Windows.UI.Xaml.Controls.Image
                    {
                        Source = _sourceBitmap.Crop(rect),
                        HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                        VerticalAlignment = yStretch,
                        Stretch = Windows.UI.Xaml.Media.Stretch.Fill
                    };
                    SetColumn(image,xPatchCol);
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

            if (xPatchEnd < _sourceBitmap.PixelWidth-1)
            {
                // there is a column of fixed width cells after the last range
                xPatchStart = xPatchEnd + 1;
                xPatchEnd = _sourceBitmap.PixelWidth-2;
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

        internal void GenerateOutlineLayout(Windows.Foundation.Size size= default(Windows.Foundation.Size))
        {
            if (!_roundedBoxInvalid)
                return;
            _roundedBoxCanvas = _roundedBoxCanvas ?? new Windows.UI.Xaml.Controls.Canvas();

            bool drawBorder = _outlineWidth > 0.05 && _outlineColor.A > 0.01;
            bool drawFill = _backgroundColor.A > 0.01;
            if (!drawFill && !drawBorder && Children.Contains(_roundedBoxCanvas))
            {
                Children.Remove(_roundedBoxCanvas);
                _roundedBoxCanvas = null;
                return;
            }

            var width = ActualWidth;
            var height = ActualHeight;
            if (size != default(Windows.Foundation.Size))
            {
                width = size.Width;
                height = size.Height;
            }

            var hz = Orientation == StackOrientation.Horizontal;
            var vt = !hz;

            var makeRoomForShadow = _hasShadow && !_shadowInverted;

            var shadowX = Forms9Patch.Settings.ShadowOffset.X;
            var shadowY = Forms9Patch.Settings.ShadowOffset.Y;
            var shadowR = Forms9Patch.Settings.ShadowRadius;

            var shadowColor = Color.FromRgba(0.0, 0.0, 0.0, 0.55).ToWindowsColor();

            if (makeRoomForShadow)
            {
                // what additional padding was allocated to cast  the button's shadow?
            }
        }
        #endregion  



    }
}
