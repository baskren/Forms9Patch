using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Manual layout.
	/// </summary>
	public class ManualLayout : Xamarin.Forms.Layout<View>
	{
		/// <summary>
		/// Occurs when layout children event is triggered.
		/// </summary>
		public event EventHandler<ManualLayoutEventArgs> LayoutChildrenEvent;

		/// <summary>
		/// Layouts the children.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("ManualLayout.LayoutChildren("+x+", "+y+", "+width+", "+height+")");
			LayoutChildrenEvent?.Invoke(this, new ManualLayoutEventArgs(x, y, width, height));
		}
	}

	/// <summary>
	/// Manual layout event arguments.
	/// </summary>
	public class ManualLayoutEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the x.
		/// </summary>
		/// <value>The x.</value>
		public double X { get; }

		/// <summary>
		/// Gets the y.
		/// </summary>
		/// <value>The y.</value>
		public double Y { get; }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public double Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Height { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ManualLayoutEventArgs"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public ManualLayoutEventArgs(double x, double y, double width, double height) 
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}


}
