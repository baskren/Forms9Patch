using Android.Graphics;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using Android.App;
using System;


namespace Forms9Patch.Droid
{
	internal class NinePatch : INinePatch, IDisposable
	{
		//static uint _instances;
		readonly Bitmap _bitmap;

		public int SourceWidth {
			get { return _bitmap.Width; }
		}

		public int SourceHeight {
			get { return _bitmap.Height; }
		}

		RangeLists _ranges;
		public RangeLists Ranges {
			get {
				if (_ranges == null)
					_ranges = this.NinePatchRanges ();
				return _ranges;
			}
		}


		internal NinePatch(Bitmap bitmap) {
			_bitmap = bitmap;
		}


		const int _black = unchecked((int)0xff000000);
		public bool IsBlackAt(int x, int y) {
			int pixel = _bitmap.GetPixel (x, y);
			return pixel==_black;
		}

		const int _transparent = 0x00000000;
		public bool IsTransparentAt(int x, int y) {
			return _bitmap.GetPixel(x,y)==_transparent;
		}

		public int Pixel(int x, int y) {
			return _bitmap.GetPixel (x, y);
		}


		const int NO_COLOR = 0x00000001;
		const int TRANSPARENT_COLOR = 0x00000000;


		static ByteBuffer CreateByteBuffer(List<Range> capsX, List<Range> capsY, Bitmap bitmap) {
			int capsXCount = capsX != null ? capsX.Count : 0;
			int capsYCount = capsY != null ? capsY.Count : 0;
			bool xStartFixed = true;
			bool xEndFixed = true;
			if (capsXCount != 0) {
				xStartFixed = capsX [0].Start != 0;
				xEndFixed = capsX[capsXCount-1].End != bitmap.Width-1;
			}
			bool yStartFixed = true;
			bool yEndFixed = true;
			if (capsYCount != 0) {
				yStartFixed = capsY [0].Start != 0;
				yEndFixed = capsY[capsYCount-1].End != bitmap.Height-1;
			}


			int xRegions = (xStartFixed ? 1 : 0) + (capsXCount * 2 - 1) + (xEndFixed ? 1 : 0);
			int yRegions = (yStartFixed ? 1 : 0) + (capsYCount * 2 - 1) + (yEndFixed ? 1 : 0);

			var buffer = new ByteBuffer(4 + sizeof(int)*7 + sizeof(int)*2*capsXCount + sizeof(int)*2*capsYCount + sizeof(int)*xRegions*yRegions);
			//var buffer = new ByteBuffer(32 + 3 * sizeof(int));
			buffer.Add((byte)0x01); // was serialised     	:1
			buffer.Add(capsXCount * 2, 1); // x div  		:2
			buffer.Add(capsYCount * 2, 1); // y div		:3
			//buffer.Add((byte)0x09); // color
			buffer.Add(capsXCount*capsYCount,1);	//		:4
			// skip
			buffer.Add(0);							//		:8
			buffer.Add(0);							//		:12
			// padding		
			buffer.Add(0);							//		:16
			buffer.Add(0);							//		:20
			buffer.Add(0);							//		:24
			buffer.Add(0);							//		:28
			// skip 4 bytes
			buffer.Add(0);							//		:32

			if (capsX!=null) foreach (var range in capsX) {			//		numXdivs * 4 (pointer)
				buffer.Add( (int)range.Start);
				buffer.Add( (int)range.End );
					//System.Diagnostics.Debug.WriteLine ("\t\tcapsX=[" + range.Start + ", " + range.End +"] ");
			}
			if (capsY!=null) foreach (var range in capsY) {			//		numXdivs * 4 (pointer)
				buffer.Add( (int)range.Start );
				buffer.Add( (int)range.End );
					//System.Diagnostics.Debug.WriteLine ("\t\tcapsX=[" + range.Start + ", " + range.End +"] ");
			}
			for (int i = 0; i < xRegions * yRegions; i++)   // numColors * 4 (pointer)
				buffer.Add(NO_COLOR);

			return buffer;
		}
			
		internal Drawable Drawable() {
			if (_ranges.PatchesX.Count==0 || _ranges.PatchesY.Count==0 || _ranges.PatchesX [0].Start < 0 || _ranges.PatchesY [0].Start < 0)
				return null;
			Bitmap trimedBitmap = TrimBitmap(_bitmap);
			var drawable = CreateDrawableWithCapInsets(trimedBitmap ,  _ranges.PatchesX, _ranges.PatchesY);
			return drawable;
		}

		internal static Drawable CreateDrawableWithCapInsets( Bitmap bitmap, List<Range> rangeListX, List<Range> rangeListY)
		{
			//if (!Settings.IsLicenseValid && _instances++ > 0)
			//	return new BitmapDrawable (bitmap);
			ByteBuffer buffer = CreateByteBuffer(rangeListX,rangeListY, bitmap);
			return new NinePatchDrawable (bitmap, buffer.Array, new Rect (), null);
		}

		internal static Bitmap TrimBitmap (Bitmap bitmap) {
			return Bitmap.CreateBitmap (bitmap, 1, 1, bitmap.Width - 2, bitmap.Height - 2);
		}

		#region IDisposable Support
		~NinePatch()
		{
			_ranges = null;
			System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
		}

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_ranges = null;
					System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			Dispose(true);
		}
		#endregion


	}
}

