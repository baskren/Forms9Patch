using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	public static class RectangleExtensions
	{
		public static string ToString(this Rectangle rect)
		{
			return rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height;
		}
	}
}
