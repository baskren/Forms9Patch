using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	public class ManualLayout : Xamarin.Forms.Layout<View>
	{
		public event EventHandler<ManualLayoutEventArgs> LayoutChildrenEvent;

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("ManualLayout.LayoutChildren("+x+", "+y+", "+width+", "+height+")");
			LayoutChildrenEvent?.Invoke(this, new ManualLayoutEventArgs(x, y, width, height));
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
