using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Forms9Patch")]
[assembly: ExportEffect(typeof(Forms9Patch.iOS.PopupEffect), "PopupEffect")]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Popup effect.
	/// </summary>
	public class PopupEffect : PlatformEffect
	{
		UIKit.UIView _nativeView;

		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnAttached () 
		{
			System.Diagnostics.Debug.WriteLine ("ATTACHED!!!!");
			//throw new NotImplementedException ();
		}

		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnDetached () 
		{
			_nativeView = null;
		}

		/// <param name="args">To be added.</param>
		/// <summary>
		/// To be added.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnElementPropertyChanged (System.ComponentModel.PropertyChangedEventArgs args) {
			base.OnElementPropertyChanged (args);

			if (args.PropertyName == PopupBase.PopupProperty.PropertyName) {
				var puElement = Element.GetValue (PopupBase.PopupProperty) as VisualElement;
				if (puElement != null) {
					// present popup
					if (Platform.GetRenderer (puElement) == null) {
						Platform.SetRenderer (puElement, Platform.CreateRenderer (puElement));
					}
					IVisualElementRenderer puRenderer = Platform.GetRenderer (puElement);
					_nativeView = puRenderer.NativeView;

					Container.Window.AddSubview(_nativeView);
					puRenderer.NativeView.Hidden = false;
					Container.Window.BringSubviewToFront(_nativeView);
				} else {
					_nativeView.RemoveFromSuperview();
					_nativeView = null;
				}
			}

		}
	}
}

