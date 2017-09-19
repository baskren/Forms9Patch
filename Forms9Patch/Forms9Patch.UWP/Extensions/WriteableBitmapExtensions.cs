using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Forms9Patch.UWP
{
    static class WriteableBitmapExtensions
    {
        public static Windows.UI.Xaml.Thickness NotANineGrid = new Windows.UI.Xaml.Thickness(0,0,0,0);

        public static bool IsANineGrid(this Windows.UI.Xaml.Thickness thickness)
        {
            return thickness != NotANineGrid;
        }

        public static async Task<BitmapImage> ToBitmapImage(this WriteableBitmap writeableBitmap)
        {
            var bytes = writeableBitmap.ToByteArray();
            var stream = new RandomAccessMemoryStream(bytes);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(stream);
            return bitmapImage;
        }

        static bool IsTransparentAt(this WriteableBitmap bitmap, int x, int y)
        {
            return bitmap.GetPixel(x, y).A == 0; 
        }

        static bool IsBlackAt(this WriteableBitmap bitmap, int x, int y)
        {
            return bitmap.GetPixel(x, y) == Colors.Black;
        }

        static public RangeLists NinePatchRanges(this WriteableBitmap bitmap)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;

            var capsX = new List<Range>();
            int pos = -1;
            for (int i = 1; i < width - 1; i++)
            {
                if (bitmap.IsBlackAt(i, 0))
                {
                    if (pos == -1)
                    {
                        pos = i;
                    }
                }
                else if (bitmap.IsTransparentAt(i, 0))
                {
                    if (pos != -1)
                    {
                        var range = new Range();
                        range.Start = pos;
                        range.End = i - 1;
                        capsX.Add(range);
                        pos = -1;
                    }
                }
                else
                {
                    // this is not a nine-patch;
                    return null;
                }
            }
            if (pos != -1)
            {
                var range = new Range();
                range.Start = pos;
                range.End = width - 1;
                capsX.Add(range);
            }

            var capsY = new List<Range>();
            pos = -1;
            for (int i = 1; i < height - 1; i++)
            {
                if (bitmap.IsBlackAt(0, i))
                {
                    if (pos == -1)
                    {
                        pos = i;
                    }
                }
                else if (bitmap.IsTransparentAt(0, i))
                {
                    if (pos != -1)
                    {
                        var range = new Range();
                        range.Start = pos;
                        range.End = i - 1;
                        capsY.Add(range);
                        pos = -1;
                    }
                }
                else
                {
                    return null;
                }
            }
            if (pos != -1)
            {
                var range = new Range();
                range.Start = pos;
                range.End = height - 1;
                capsY.Add(range);
            }

            var margX = null as Range;
            pos = -1;
            for (int i = 1; i < width - 1; i++)
            {
                if (bitmap.IsBlackAt(i, height - 1))
                {
                    if (pos == -1)
                    {
                        pos = i - 1;
                        break;
                    }
                }
            }
            if (pos != -1)
            {
                for (int i = width - 1; i > pos; i--)
                {
                    if (bitmap.IsBlackAt(i, height - 1))
                    {
                        margX = new Range();
                        margX.Start = pos;
                        margX.End = i - 1;
                        break;
                    }
                }
            }

            var margY = null as Range;
            pos = -1;
            for (int i = 1; i < height - 1; i++)
            {
                if (bitmap.IsBlackAt(width - 1, i))
                {
                    if (pos == -1)
                    {
                        pos = i - 1;
                        break;
                    }
                }
            }
            if (pos != -1)
            {
                for (int i = height - 1; i > pos; i--)
                {
                    if (bitmap.IsBlackAt(width - 1, i))
                    {
                        margY = new Range();
                        margY.Start = pos;
                        margY.End = i - 1;
                        break;
                    }
                }
            }

            var rangeLists = new RangeLists();
            rangeLists.PatchesX = capsX;
            rangeLists.PatchesY = capsY;
            rangeLists.MarginX = margX;
            rangeLists.MarginY = margY;

            return rangeLists;
        }


        public static Windows.UI.Xaml.Thickness NineGridThickness(this WriteableBitmap writeableBitmap)
        {
            // examine first row
            var rows = writeableBitmap.PixelHeight;
            var cols = writeableBitmap.PixelWidth;

            int hzMarkCount = 0;
            int vtMarkCount = 0;
            var hzMarks = new int[2];
            var vtMarks = new int[2];

            byte lastAlpha = 0;

            //bool notNinePatch = false;

            //var byteArray = writeableBitmap.ToByteArray();

            var pixels = writeableBitmap.GetBitmapContext().Pixels;
            var lastRowOffset = (rows - 1) * cols;
            for (int offset = 1; offset < (cols-1); offset += 1)
            {
                if (pixels[lastRowOffset + offset].IsNotNinePatchMark() )
                    return NotANineGrid;
                if (pixels[offset].IsNotNinePatchMark())
                    return NotANineGrid;
                byte alpha = (byte)((pixels[offset] & 0xFF000000) >> 24);
                if ( alpha != lastAlpha)
                {
                    if (hzMarkCount > 1)
                        return NotANineGrid;
                    hzMarks[hzMarkCount++] = offset - (lastAlpha>0?1:0);
                    lastAlpha = alpha;
                }
            }

            lastAlpha = 0;

            for (int row=1; row < rows-1; row++)
            {
                var offset = (row * cols) + cols - 1;
                if (pixels[offset].IsNotNinePatchMark())
                    return NotANineGrid;
                offset = row * cols;
                var pixel = pixels[offset];
                if (pixel.IsNotNinePatchMark())
                    return NotANineGrid;
                byte alpha = (byte)((pixel & 0xFF000000) >> 24);
                if (alpha != lastAlpha)
                {
                    if (vtMarkCount > 1)
                        return NotANineGrid;
                    vtMarks[vtMarkCount++] = offset - (lastAlpha > 0 ? 1 : 0);
                    lastAlpha = alpha;
                }
            }


            if (hzMarkCount + vtMarkCount == 0)
                return NotANineGrid;

            var result = new Windows.UI.Xaml.Thickness(0,0,cols-2,rows-2);
            if (hzMarkCount > 0)
                result.Left = hzMarks[0];
            if (hzMarkCount > 1)
                result.Right = hzMarks[1];
            if (vtMarkCount > 0)
                result.Top = vtMarks[0];
            if (vtMarkCount > 1)
                result.Bottom = vtMarks[1];

            return result;
        }


    }
}
