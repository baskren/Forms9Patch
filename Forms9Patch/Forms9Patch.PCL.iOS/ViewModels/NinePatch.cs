using System;
using UIKit;
using CoreImage;
using CoreGraphics;
using System.Collections.Generic;

#if __IOS__
namespace Forms9Patch.iOS
#elif __DROID__
namespace Forms9Patch.Droid
#else
namespace Forms9Patch
#endif
{
    class NinePatch : UIImageView, INinePatch, IDisposable
    {
        //static uint _instances;
        //uint id;
        UIImageView[,] _patchViews;
        nint _fixedX;
        nint _fixedY;
        double _compliantX, _compliantY;

        int _width;
        public int SourceWidth
        {
            get { return _width; }
        }

        int _height;
        public int SourceHeight
        {
            get { return _height; }
        }

        readonly byte[] _data;
        public int Pixel(int x, int y)
        {
            int index = 4 * (y * _width + x);
            return BitConverter.ToInt32(_data, index);
        }

        public byte Data(int x, int y, int c)
        {
            int index = 4 * (y * _width + x) + c;
            return _data[index];
        }
        /*
		public byte Red(int x, int y) {
			return _data [4 * (y * _width + x)];
		}

		public byte Green(int x, int y) {
			return _data [4 * (y * _width + x)+1];
		}

		public byte Blue(int x, int y) {
			return _data [4 * (y * _width + x)+2];
		}

		public byte Alpha(int x, int y) {
			return _data [4 * (y * _width + x)+3];
		}
		*/
        const int _black = unchecked((int)0xFF000000);
        public bool IsBlackAt(int x, int y)
        {
            return Pixel(x, y) == _black;
        }

        const int _transparent = 0x00000000;
        public bool IsTransparentAt(int x, int y)
        {
            return Pixel(x, y) == _transparent;
        }

        RangeLists _ranges;
        public RangeLists Ranges
        {
            get
            {
                if (_ranges == null)
                    _ranges = this.NinePatchRanges();
                return _ranges;
            }
        }

        int _xRegions, _yRegions;
        bool _xStartFixed;
        bool _yStartFixed;

        public NinePatch(UIImage image, List<Range> xPatches = null, List<Range> yPatches = null)
        {
            //id = _instances++;
            var cgImage = image.CGImage;
            if (cgImage == null)
            {
                var ciContext = CIContext.FromOptions(null);
                var ciImage = ciContext.CreateCGImage(image.CIImage, image.CIImage.Extent);
                var altImage = new UIImage(ciImage);
                cgImage = altImage.CGImage;
            }

            // ninePatch detection
            _width = (int)cgImage.Width;
            _height = (int)cgImage.Height;
            _data = new byte[_height * _width * 4];
            nint bytesPerPixel = 4;
            nint bytesPerRow = bytesPerPixel * _width;
            nint bitsPerComponent = 8;
            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
            var context = new CGBitmapContext(_data, cgImage.Width, cgImage.Height, bitsPerComponent, bytesPerRow, colorSpace, CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big);
            context.DrawImage(new CGRect(0, 0, _width, _height), cgImage);
            //context.Dispose ();
            //colorSpace.Dispose ();
            _ranges = this.NinePatchRanges();

            // if it is a nine patch image, crop out the markers
            if (_ranges != null && (_ranges.PatchesX.Count > 0 || _ranges.PatchesY.Count > 0))
            {
                image = image.Crop(new CGRect(1, 1, cgImage.Width - 2, cgImage.Height - 2));
                _width -= 2;
                _height -= 2;
            }
            // override the nine patch markers?
            if (_ranges == null)
            {
                _ranges = new RangeLists();
                _ranges.PatchesX = xPatches ?? new List<Range>();
                _ranges.PatchesY = yPatches ?? new List<Range>();
            }
            if (xPatches != null)
                _ranges.PatchesX = xPatches;
            if (yPatches != null)
                _ranges.PatchesY = yPatches;

            _xStartFixed = true;
            _xRegions = 1;
            if (_ranges.PatchesX.Count > 0)
            {
                _xStartFixed = _ranges.PatchesX[0].Start >= 0;
                _xRegions = (_ranges.PatchesX[0].Start > 0 ? 1 : 0) + (_ranges.PatchesX.Count * 2 - 1) + (_ranges.PatchesX[_ranges.PatchesX.Count - 1].End < cgImage.Width - 2 ? 1 : 0);
            }
            _yStartFixed = true;
            _yRegions = 1;
            if (_ranges.PatchesY.Count > 0)
            {
                _yStartFixed = _ranges.PatchesY[0].Start >= 0;
                _yRegions = (_ranges.PatchesY[0].Start > 0 ? 1 : 0) + (_ranges.PatchesY.Count * 2 - 1) + (_ranges.PatchesY[_ranges.PatchesY.Count - 1].End < cgImage.Height - 2 ? 1 : 0);
            }

            MakePatches(image);

            // free up memory that won't be used again
            _data = null;

            Frame = new CGRect(0, 0, _width / Display.Scale, _height / Display.Scale);

        }

        void MakePatches(UIImage image)
        {
            _patchViews = new UIImageView[_xRegions, _yRegions];
            nint x = 0, w;
            int region = 0;
            int patch = 0;
            _fixedX = 0;

            if (_xStartFixed)
            {
                w = (_xRegions == 1 ? _width : (nint)Ranges.PatchesX[0].Start);
                MakeYPatches(image, region++, x, w);
                _fixedX += w;
            }
            //for (int i = 0; i < Ranges.PatchesX.Count; i++) {
            while (region < _xRegions)
            {
                //flex region
                x = (nint)Ranges.PatchesX[patch].Start;
                w = (nint)Ranges.PatchesX[patch].End - x;
                MakeYPatches(image, region++, x, w);
                x += w;
                //fixed region
                if (region < _xRegions)
                {
                    w = (patch < Ranges.PatchesX.Count - 1 ? (nint)Ranges.PatchesX[patch + 1].Start : _width) - x;
                    MakeYPatches(image, region++, x, w);
                    //x += w;
                    _fixedX += w;
                }
                patch++;
            }
            _compliantX = _width - _fixedX;
        }

        void MakeYPatches(UIImage image, int xIndex, nint x, nint w)
        {
            CGRect frame;
            nint y = 0, h;
            int region = 0;
            int patch = 0;
            _fixedY = 0;

            if (_yStartFixed)
            {
                h = (_yRegions == 1 ? _height : (nint)Ranges.PatchesY[0].Start);
                frame = new CGRect(x, y, w, h);
                _patchViews[xIndex, region] = image.UIImageView(frame);
                Add(_patchViews[xIndex, region++]);
                _fixedY += h;
            }
            while (region < _yRegions)
            {
                // flex region
                y = (nint)Ranges.PatchesY[patch].Start;
                h = (nint)Ranges.PatchesY[patch].End - y;
                frame = new CGRect(x, y, w, h);
                _patchViews[xIndex, region] = image.UIImageView(frame);
                Add(_patchViews[xIndex, region++]);
                y += h;
                //fixed region
                if (region < _yRegions)
                {
                    h = (patch < Ranges.PatchesY.Count - 1 ? (nint)Ranges.PatchesY[patch + 1].Start : _height) - y;
                    frame = new CGRect(x, y, w, h);
                    _patchViews[xIndex, region] = image.UIImageView(frame);
                    Add(_patchViews[xIndex, region++]);
                    //y += h;
                    _fixedY += h;
                }
                patch++;
            }
            _compliantY = _height - _fixedY;
        }

        public override void Draw(CGRect rect)
        {
            if (rect.IsEmpty || rect.IsNull() || rect.IsInfinite())
            {
                //base.Draw (rect);
                return;
            }
            if (Frame.IsEmpty || Frame.IsNull() || Frame.IsInfinite())
            {
                //base.Draw (rect);
                //return;
                Frame = rect;
            }
            /*
			if (!Settings.IsLicenseValid  && id > 0) {
				base.Draw (rect);
				return;
			}
			*/

            double scaleX = 0;
            double failScaleX = 1;
            if (_width - _compliantX > rect.Width * Display.Scale || _compliantX <= 0)
                failScaleX = rect.Width * Display.Scale / (_width - _compliantX);
            else
                scaleX = (rect.Width * Display.Scale - _fixedX) / _compliantX;

            double xScaled = 0, xSourced, wSourced, wScaled;
            int region = 0;
            int patch = 0;
            if (_xStartFixed)
            {
                wSourced = (_xRegions == 1 ? _width : Ranges.PatchesX[0].Start);
                wScaled = wSourced * failScaleX / Display.Scale;
                DrawYPatches(rect, region++, xScaled, wScaled);
                xScaled = wScaled;
            }

            //for (int i = 0; i < Ranges.PatchesX.Count; i++) {
            while (region < _xRegions)
            {
                //flex region
                xSourced = Ranges.PatchesX[patch].Start;
                wSourced = Ranges.PatchesX[patch].End - xSourced;
                wScaled = wSourced * scaleX / Display.Scale;
                DrawYPatches(rect, region++, xScaled, wScaled);
                xScaled += wScaled;
                //fixed region
                if (region < _xRegions)
                {
                    xSourced = Ranges.PatchesX[patch].End;
                    wSourced = (patch < Ranges.PatchesX.Count - 1 ? Ranges.PatchesX[patch + 1].Start : _width) - xSourced;
                    wScaled = wSourced * failScaleX / Display.Scale;
                    DrawYPatches(rect, region++, xScaled, wScaled);
                    xScaled += wScaled;
                }
                patch++;
            }

            base.Draw(rect);
        }

        public void DrawYPatches(CGRect rect, int xIndex, double xScaled, double wScaled)
        {
            double scaleY = 0;
            double failScaleY = 1;
            if (_height - _compliantY > rect.Height * Display.Scale || _compliantY <= 0)
                failScaleY = rect.Height * Display.Scale / (_height - _compliantY);
            else
                scaleY = (rect.Height * Display.Scale - _fixedY) / _compliantY;

            double yScaled = 0, ySourced, hSourced, hScaled;
            int region = 0;
            int patch = 0;
            if (_yStartFixed)
            {
                hSourced = (_yRegions == 1 ? _height : Ranges.PatchesY[0].Start);
                hScaled = hSourced * failScaleY / Display.Scale;
                _patchViews[xIndex, region++].Frame = new CGRect(xScaled, yScaled, wScaled, hScaled);
                yScaled = hScaled;
            }
            while (region < _yRegions)
            {
                //flex region
                ySourced = Ranges.PatchesY[patch].Start;
                hSourced = Ranges.PatchesY[patch].End - ySourced;
                hScaled = hSourced * scaleY / Display.Scale;
                _patchViews[xIndex, region++].Frame = new CGRect(xScaled, yScaled, wScaled, hScaled);
                yScaled += hScaled;
                //fixed region
                if (region < _yRegions)
                {
                    ySourced = Ranges.PatchesY[patch].End;
                    hSourced = (patch < Ranges.PatchesY.Count - 1 ? Ranges.PatchesY[patch + 1].Start : _height) - ySourced;
                    hScaled = hSourced * failScaleY / Display.Scale;
                    _patchViews[xIndex, region++].Frame = new CGRect(xScaled, yScaled, wScaled, hScaled);
                    yScaled += hScaled;
                }
                patch++;
            }
        }
    }
}

