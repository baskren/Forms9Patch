using System;
using Xamarin.Forms.Platform.Android;

namespace Forms9Patch.Droid
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
			if (e.OldElement != null) {
				e.OldElement.PropertyChanged -= renderer.HandlePropertyChanged;
				e.OldElement.PropertyChanging -= renderer.HandlePropertyChanging;
				//((Xamarin.Forms.Page)e.OldElement).SetBoundsForChildFunction (null);
			}
			if (e.NewElement != null) {
				e.NewElement.PropertyChanged += renderer.HandlePropertyChanged;
				e.NewElement.PropertyChanging += renderer.HandlePropertyChanging;
				//((Xamarin.Forms.Page)e.NewElement).SetBoundsForChildFunction (renderer.ChildBounds);
			}
		}

		static void HandlePropertyChanged (this IVisualElementRenderer renderer, object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == PopupBase.PopupProperty.PropertyName) {
				var puElement = (Xamarin.Forms.VisualElement)renderer.Element.GetValue (PopupBase.PopupProperty);
				if (puElement != null) {
					// present popup
					if (Platform.GetRenderer (puElement) == null) {
						Platform.SetRenderer (puElement, Platform.CreateRenderer (puElement));
					}
					IVisualElementRenderer puRenderer = Platform.GetRenderer (puElement);
					if (puRenderer != null) {
						((Android.Views.ViewGroup)renderer.ViewGroup.Parent).AddView (puRenderer as FormsViewGroup);
						puRenderer.ViewGroup.Visibility = Android.Views.ViewStates.Visible;
						//System.Diagnostics.Debug.WriteLine ($"\t\tpuRenderer.NativeView.Frame=[{puRenderer.ViewGroup.GetX() }, {puRenderer.ViewGroup.GetY() }, {puRenderer.ViewGroup.Width}, {puRenderer.ViewGroup.Height}]");
					}
				}
			} else if (e.PropertyName == "CurrentPage" ) {
				var puElement = (Forms9Patch.PopupBase)renderer.Element.GetValue (PopupBase.PopupProperty);
				puElement?.Cancel ();
			}
		}

		static void HandlePropertyChanging(this IVisualElementRenderer renderer, object sender, Xamarin.Forms.PropertyChangingEventArgs e) {
			if (e.PropertyName == PopupBase.PopupProperty.PropertyName) {
				var puElement = (Xamarin.Forms.VisualElement)renderer.Element.GetValue (PopupBase.PopupProperty);
				if (puElement != null) {
					IVisualElementRenderer puRenderer = Platform.GetRenderer (puElement);
					if (puRenderer != null) {
						((Android.Views.ViewGroup)renderer.ViewGroup.Parent).RemoveView(puRenderer as FormsViewGroup);
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
			var pageNativeView = renderer.ViewGroup;
			var pageLocation = new int[2];
			pageNativeView.GetLocationInWindow (pageLocation);

			IVisualElementRenderer elementRenderer = Platform.GetRenderer (element);
			var elementNativeView = elementRenderer?.ViewGroup;
			var elementLocation = new int[2];
			elementNativeView.GetLocationInWindow (elementLocation);

			var rect = new Xamarin.Forms.Rectangle (
				(elementLocation [0] - pageLocation [0]) / Display.Scale,
				(elementLocation [1] - pageLocation [1]) / Display.Scale,
				(elementNativeView.Width) / Display.Scale,
				(elementNativeView.Height) / Display.Scale);
			return rect;
		}
		*/

	}
}

