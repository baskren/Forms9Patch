using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

namespace Forms9Patch.UWP 
{
    public class ImageView : Windows.UI.Xaml.Controls.Grid, IDisposable
    {
        bool _debugMessages = false;

        RangeLists _rangeLists=null;

        Xamarin.Forms.Size BaseImageSize
        {
            get
            {
                if (_source == null)
                    return Size.Zero;

                var result = new Size(_source.PixelWidth, _source.PixelHeight);
                if (_rangeLists!=null)
                {
                    result.Width -= 2;
                    result.Height -= 2;
                }
                return result;
            }
        }

        WriteableBitmap _source;
        public async Task SetSourceAsync(Xamarin.Forms.ImageSource source)
        {

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
                    _source = bitmap;
                else
                    _source = null;

                GenerateLayout();
                UpdateAspect();
            }
        }

        Fill _fill;
        public Forms9Patch.Fill Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                if (value != _fill)
                {
                    _fill = value;
                    UpdateAspect();
                }
            }
        }


        int _instance;

        public ImageView(int instance)
        {
            _instance = instance;
            BorderThickness = new Windows.UI.Xaml.Thickness(1);
            BorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Pink);
            Padding = new Windows.UI.Xaml.Thickness();
        }

        void UpdateAspect()
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect()");
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
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer.UpdateAspect() RETURN");
        }


        void GenerateLayout()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            _rangeLists = _source?.NinePatchRanges();
            if (_rangeLists == null)
            {
                // NinePatch markup was not found
                RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                {
                    Height = Windows.UI.Xaml.GridLength.Auto
                });
                ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition
                {
                    Width = Windows.UI.Xaml.GridLength.Auto
                });

                var image = new Windows.UI.Xaml.Controls.Image
                {
                    Source = _source,
                };

                SetColumn(image, 0);
                SetRow(image, 0);

                Children.Add(image);
            }
            else
            {
                int xPatches = _rangeLists.PatchesX.Count;
                if (xPatches > 0)
                { 
                    if (_rangeLists.PatchesX[0].Start > 0)
                        xPatches++;
                if (_rangeLists.PatchesX.Last<Range>().End < _source.PixelWidth - 1)
                    xPatches++;
                }
                int yPatches = _rangeLists.PatchesY.Count;
                if (yPatches > 0)
                {
                    if (_rangeLists.PatchesY[0].Start > 0)
                        yPatches++;
                    if (_rangeLists.PatchesY.Last<Range>().End < _source.PixelWidth - 1)
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

                if (yPatchEnd < _source.PixelHeight - 1)
                {
                    // there is a row of fixed height cells after the last range
                    yPatchStart = yPatchEnd + 1;
                    yPatchEnd = _source.PixelHeight - 2;
                    PatchesForRow(yPatchRow, yPatchStart, yPatchEnd, Windows.UI.Xaml.VerticalAlignment.Top);

                    RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition
                    {
                        Height = new Windows.UI.Xaml.GridLength(yPatchEnd - yPatchStart + 1),
                    });
                }
            }
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("GenerateLayout ["+_instance+"]: "+stopwatch.ElapsedMilliseconds);
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
                        Source = _source.Crop(rect),
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
                    Source = _source.Crop(rect),
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

            if (xPatchEnd < _source.PixelWidth-1)
            {
                // there is a column of fixed width cells after the last range
                xPatchStart = xPatchEnd + 1;
                xPatchEnd = _source.PixelWidth-2;
                xWidth = xPatchEnd - xPatchStart + 1;
                rect = new Windows.Foundation.Rect(xPatchStart, yPatchStart, xWidth, yWidth);
                image = new Windows.UI.Xaml.Controls.Image
                {
                    Source = _source.Crop(rect),
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

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _source = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Image() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
