using System;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Thickness extension.
	/// </summary>
	public static class ThicknessExtension
	{
		/// <summary>
		/// Description the specified thickness.
		/// </summary>
		/// <param name="thickness">Thickness.</param>
		public static string Description(this Xamarin.Forms.Thickness thickness) {
			return "[Thickness:"+thickness.Left+","+thickness.Top+","+thickness.Right+","+thickness.Bottom+"]";
		}

	}
}

