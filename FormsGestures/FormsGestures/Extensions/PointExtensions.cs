using Xamarin.Forms;

namespace FormsGestures
{
	/// <summary>
	/// Make your life easier
	/// </summary>
	public static class PointExtensions {
		
		/// <summary>
		/// Add the specified first and second point.
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		public static Point Add(this Point first, Point second) {
			return new Point(first.X + second.X, first.Y + second.Y);
		}

		/// <summary>
		/// Subtract the specified first and second point.
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		public static Point Subtract(this Point first, Point second) {
			return new Point(first.X - second.X, first.Y - second.Y);
		}

		/// <summary>
		/// Handy dandy formated ToString for Xamarin.Forms.Point
		/// </summary>
		/// <param name="point"></param>
		/// <param name="format"></param>
		/// <returns></returns>
        public static string ToString(this Point point, string format)
        {
			return "{X=" + point.X.ToString(format) + " Y=" + point.Y.ToString(format) + "}";
        }

		/// <summary>
		/// Handy dandy formated ToString for Xamarin.Forms.Rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string ToString(this Rectangle rectangle, string format)
		{
			return "{X=" + rectangle.X.ToString(format) + " Y=" + rectangle.Y.ToString(format) + " Width=" + rectangle.Width.ToString(format) + " Height=" + rectangle.Height.ToString(format) + "}";
		}

	}
}
