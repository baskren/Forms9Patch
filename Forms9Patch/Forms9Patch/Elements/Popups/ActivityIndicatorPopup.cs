using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Activity indicator full page overlay.
	/// </summary>
	public class ActivityIndicatorPopup : ModalPopup
	{
		/// <summary>
		/// Create and displays an activity indictar on a full page layover.
		/// </summary>
		public static ActivityIndicatorPopup Create(VisualElement target)
		{
			var indicator = new ActivityIndicatorPopup(target);
			indicator.IsVisible = true;
			return indicator;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ActivityIndicatorPopup"/> class.
		/// </summary>
		public ActivityIndicatorPopup(VisualElement target) : base(target)
		{
			var indicator = new ActivityIndicator();
			indicator.SetBinding(ActivityIndicator.IsRunningProperty,IsVisibleProperty.PropertyName);
			Content = indicator;
			CancelOnPageOverlayTouch = false;
			indicator.BackgroundColor = Color.Transparent;
			indicator.Color = Color.Blue;
			BackgroundColor = Color.Transparent;
		}
	}
}
