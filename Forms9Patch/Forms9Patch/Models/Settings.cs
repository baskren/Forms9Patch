using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Settings (for use by Forms9Patch PCL code).
	/// </summary>
	public class Settings
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


		public static double ListViewCellSwipePopupMenuWidthRequest = 250;
		public static double ListViewCellSwipePopupMenuFontSize = 16;
		public static Color ListViewCellSwipePopupMenuFontColor = Color.FromHex("0076FF");
		public static Color ListViewCellSwipePopupMenuButtonColor = Color.White;
		public static Color ListViewCellSwipePopupMenuButtonOutlineColor = Color.FromHex("CCC");
		public static float ListViewCellSwipePopupMenuButtonOutlineWidth = 0;
		public static float ListViewCellSwipePopupMenuButtonOutlineRadius = 6;
		public static float ListViewCellSwipePopupMenuButtonSeparatorWidth = 0;
	}
}

