using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Forms9Patch")]
//[assembly: ExportEffect(typeof(Forms9Patch.Droid.PopupEffect), "PopupEffect")]
namespace Forms9Patch.Droid
{
	/// <summary>
	/// Popup effect
	/// </summary>
	public class PopupEffect : PlatformEffect
	{
		IVisualElementRenderer _puRenderer;

		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnAttached ()
		{
			System.Diagnostics.Debug.WriteLine ("PopupEffect ATTACHED!!!!");
		}

		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnDetached ()
		{
			DetachPopup ();
		}

		/// <param name="args">To be added.</param>
		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnElementPropertyChanged (System.ComponentModel.PropertyChangedEventArgs args)
		{
			if (args.PropertyName == PopupBase.PopupProperty.PropertyName) {
				var puElement = (PopupBase)Element.GetValue (PopupBase.PopupProperty);
				if (puElement != null) {
					// present popup
					if (Platform.GetRenderer (puElement) == null) {
						Platform.SetRenderer (puElement, Platform.CreateRenderer (puElement));
					}
					var newRenderer = Platform.GetRenderer (puElement);
					if (newRenderer != null && newRenderer != _puRenderer)
						DetachPopup ();
					_puRenderer = newRenderer;
					if (_puRenderer != null && ((FormsViewGroup)_puRenderer).Parent==null) {
						((Android.Views.ViewGroup)Container.Parent).AddView (_puRenderer as FormsViewGroup);
						_puRenderer.ViewGroup.Visibility = Android.Views.ViewStates.Visible;
					}
				} else				
					DetachPopup ();
			} else if (args.PropertyName == "CurrentPage" ) {
				var puElement = (PopupBase)Element.GetValue (PopupBase.PopupProperty);
				puElement?.Cancel ();
				DetachPopup ();
			}
		}

		void DetachPopup() {
			if (_puRenderer != null) 
				((Android.Views.ViewGroup)Container.Parent).RemoveView (_puRenderer as FormsViewGroup);
			_puRenderer = null;
		}
	}
}

