using Android.Views;

namespace Forms9Patch.Droid
{
	internal static class AlignmentExtensions
	{
		internal static GravityFlags ToHorizontalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment)
			{
			case Xamarin.Forms.TextAlignment.Center:
				return GravityFlags.CenterHorizontal;
			case Xamarin.Forms.TextAlignment.End:
				return GravityFlags.Right;
			default:
				return GravityFlags.Left;
			}
		}

		internal static GravityFlags ToVerticalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
		{
			switch (alignment)
			{
			case Xamarin.Forms.TextAlignment.Start:
				return GravityFlags.Top;
			case Xamarin.Forms.TextAlignment.End:
				return GravityFlags.Bottom;
			default:
				return GravityFlags.CenterVertical;
			}
		}
	}
}