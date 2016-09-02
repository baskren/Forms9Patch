using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Forms9Patch.iOS
{
	/// <summary>
	/// Page renderer extensions.
	/// </summary>
	public static class PageRendererExtensions
	{
		/// <summary>
		/// Pages the renderer extension on element changed.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		/// <param name="e">E.</param>
		public static void PageRendererExtensionOnElementChanged(this IVisualElementRenderer renderer, VisualElementChangedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine ($"PageRendererExtensions.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
			if (e.OldElement != null) {
				e.OldElement.PropertyChanged -= renderer.HandlePropertyChanged;
				//((Xamarin.Forms.Page)e.OldElement).SetBoundsForChildFunction (null);
			}
			if (e.NewElement != null) {
				e.NewElement.PropertyChanged += renderer.HandlePropertyChanged;
				//((Xamarin.Forms.Page)e.NewElement).SetBoundsForChildFunction (renderer.ChildBounds);
			}
		}

		static void HandlePropertyChanged (this IVisualElementRenderer renderer, object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine ($"PageRendererExtensions.{System.Reflection.MethodBase.GetCurrentMethod().Name} property={e.PropertyName}");

			if (e.PropertyName == PopupBase.PopupProperty.PropertyName) {
				var puElement = renderer.Element.GetValue (PopupBase.PopupProperty) as Xamarin.Forms.VisualElement;
				if (puElement != null) {
					// present popup
					if (Platform.GetRenderer (puElement) == null) {
						Platform.SetRenderer (puElement, Platform.CreateRenderer (puElement));
					}
					IVisualElementRenderer puRenderer = Platform.GetRenderer (puElement);

					//var uiController = renderer as UIViewController;
					//uiController.View.Window.AddSubview (puRenderer.NativeView);
					UIApplication.SharedApplication.KeyWindow.AddSubview(puRenderer.NativeView);
					puRenderer.NativeView.Hidden = false;
					//uiController.View.Window.BringSubviewToFront (puRenderer.NativeView);
					UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(puRenderer.NativeView);
				} else {
					// dispose popup
					//var uiController = renderer as UIViewController;
					var window = UIApplication.SharedApplication.KeyWindow;
					foreach (var subview in window.Subviews) {
						if (subview is PopupBaseRenderer) {
							subview.RemoveFromSuperview ();
						}
					}
				}
			}

		}

		static void HandleSizeChanged (this IVisualElementRenderer renderer, object sender, EventArgs eventArgs)
		{
			//System.Diagnostics.Debug.WriteLine ($"PageRendererExtensions.{System.Reflection.MethodBase.GetCurrentMethod().Name}");

		}

		/// <summary>
		/// Pages the renderer extension dispose.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		public static void PageRendererExtensionDispose(this IVisualElementRenderer renderer, bool disposing) {
			//System.Diagnostics.Debug.WriteLine ($"PageRendererExtensions.{System.Reflection.MethodBase.GetCurrentMethod().Name}");
			if (disposing) {
				renderer.Element.PropertyChanged -= renderer.HandlePropertyChanged;
			}
		}

		/*
		public static Xamarin.Forms.Rectangle ChildBounds(this IVisualElementRenderer renderer, Xamarin.Forms.VisualElement element) {
			IVisualElementRenderer elementRenderer = Platform.GetRenderer (element);
			var nativeView = elementRenderer?.NativeView;
			var elementBounds = nativeView.Bounds;
			var windowBounds = nativeView.ConvertRectToView (elementBounds, renderer.NativeView).ToRectangle ();
			return windowBounds;
		}
		*/
	}
}

