using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	internal class ManualLayout : Xamarin.Forms.Layout<View>
	{
		public event EventHandler<ManualLayoutEventArgs> LayoutChildrenEvent;

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			LayoutChildrenEvent?.Invoke(this, new ManualLayoutEventArgs(x, y, width, height));
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
		}

		public void LayoutChild(View child, Rectangle bounds)
		{
			LayoutChildIntoBoundingRegion(child, bounds);
		}
	}

	public class ManualLayoutEventArgs : EventArgs
	{
		public double X { get; }
		public double Y { get; }
		public double Width { get; }
		public double Height { get; }

		public ManualLayoutEventArgs(double x, double y, double width, double height) 
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}


}
