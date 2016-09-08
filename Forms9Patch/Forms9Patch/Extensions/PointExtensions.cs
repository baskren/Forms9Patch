using Xamarin.Forms;

namespace Forms9Patch

{
	static class PointExtensions {
		
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
	}
}
