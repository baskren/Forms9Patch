using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Rectangle extensions.
	/// </summary>
	public static class RectangleExtensions
	{
		/// <summary>
		/// Tos the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="rect">Rect.</param>
		public static string ToString(this Rectangle rect)
		{
			return rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height;
		}
	}
}
