using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Settings (for use by Forms9Patch PCL code).
	/// </summary>
	public static class Settings
	{
		/// <summary>
		/// The shadow offset.
		/// </summary>
		public static Point ShadowOffset = new Point (0,1);

		/// <summary>
		/// The shadow radius.
		/// </summary>
		public static double ShadowRadius = 3;

		internal static TimeSpan MsUntilTapped = TimeSpan.FromMilliseconds(210);
	}
}

