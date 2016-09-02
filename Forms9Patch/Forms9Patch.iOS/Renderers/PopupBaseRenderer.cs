using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.PopupBase), typeof(Forms9Patch.iOS.PopupBaseRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Popup base renderer.
	/// </summary>
	public class PopupBaseRenderer : VisualElementRenderer<PopupBase>
	{
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<PopupBase> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement != null) {
				//e.OldElement.ForceNativeLayout -= ForceNativeLayout;
			}
			if (e.NewElement != null) {
				//e.NewElement.ForceNativeLayout += ForceNativeLayout;
			}
		}
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			//System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.{System.Reflection.MethodBase.GetCurrentMethod().Name} property={e.PropertyName}");
			//if (e.PropertyName == PopupBase.IsPresentedProperty.PropertyName)
			if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
				Hidden = !Element.IsVisible;
			
			else if (
				e.PropertyName == VisualElement.YProperty.PropertyName ||
				e.PropertyName == VisualElement.XProperty.PropertyName ||
				e.PropertyName == VisualElement.WidthProperty.PropertyName ||
				e.PropertyName == VisualElement.HeightProperty.PropertyName) {
				if (Element.Bounds.Right > Element.Bounds.Left && Element.Bounds.Bottom > Element.Bounds.Top) {
					NativeView.Frame = new CoreGraphics.CGRect((nfloat)Element.Bounds.X, (nfloat)Element.Bounds.Y, (nfloat)Element.Bounds.Width, (nfloat)Element.Bounds.Height);
					//System.Diagnostics.Debug.WriteLine($"\tPopupBaseRenderer.Bounds=[{Element.Bounds.Left}, {Element.Bounds.Top}, {Element.Bounds.Width}, {Element.Bounds.Height}]");
				}
			}
			base.BackgroundColor = Color.Transparent.ToUIColor();
		}

		/*
		void ForceNativeLayout() {
			var scale = Forms.Context.Resources.DisplayMetrics.Density;
			//ViewGroup.Layout ((int)(Element.Bounds.Left * scale), (int)(Element.Bounds.Top * scale), (int)(Element.Bounds.Right * scale), (int)(Element.Bounds.Bottom * scale));

			NativeView.Frame = new CoreGraphics.CGRect((nfloat)Element.Bounds.X, (nfloat)Element.Bounds.Y, (nfloat)Element.Bounds.Width, (nfloat)Element.Bounds.Height);
			NativeView.AutoresizingMask = UIKit.UIViewAutoresizing.FlexibleHeight | UIKit.UIViewAutoresizing.FlexibleWidth;
			//System.Diagnostics.Debug.WriteLine($"\tBounds=[{Element.Bounds.Left}, {Element.Bounds.Top}, {Element.Bounds.Width}, {Element.Bounds.Height}]");
			//System.Diagnostics.Debug.WriteLine ($"\tPadding=[{Element.Padding.Left},{Element.Padding.Top},{Element.Padding.Right},{Element.Padding.Bottom}]");
		}
		*/

	}
}

