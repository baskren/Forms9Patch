using Xamarin.Forms;

namespace FormsGestures

{
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

        public static string ToString(this Point point, string format)
        {
			return "{X=" + point.X.ToString(format) + " Y=" + point.Y.ToString(format) + "}";
        }

		public static string ToString(this Rectangle rectangle, string format)
		{
			return "{X=" + rectangle.X.ToString(format) + " Y=" + rectangle.Y.ToString(format) + " Width=" + rectangle.Width.ToString(format) + " Height=" + rectangle.Height.ToString(format) + "}";
		}

	}
}
