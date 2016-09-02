using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Forms9Patch.PopupBase), typeof(Forms9Patch.Droid.PopupBaseRenderer))]
namespace Forms9Patch.Droid
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
				e.OldElement.ForceNativeLayout -= ForceNativeLayout;
			}
			if (e.NewElement != null) {
				e.NewElement.ForceNativeLayout += ForceNativeLayout;
			}
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine ($"{this.GetType().FullName}.{System.Reflection.MethodBase.GetCurrentMethod().Name} property={e.PropertyName}");
			if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
				Visibility = Element.IsVisible ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
			base.OnElementPropertyChanged (sender, e);
		}

		void ForceNativeLayout() {
			var scale = Forms.Context.Resources.DisplayMetrics.Density;
			ViewGroup.Layout ((int)(Element.Bounds.Left * scale), (int)(Element.Bounds.Top * scale), (int)(Element.Bounds.Right * scale), (int)(Element.Bounds.Bottom * scale));
			//System.Diagnostics.Debug.WriteLine($"\tBounds=[{Element.Bounds.Left}, {Element.Bounds.Top}, {Element.Bounds.Width}, {Element.Bounds.Height}]");
			//System.Diagnostics.Debug.WriteLine ($"\tPadding=[{Element.Padding.Left},{Element.Padding.Top},{Element.Padding.Right},{Element.Padding.Bottom}]");
		}


		//protected override void OnLayout (bool changed, int l, int t, int r, int b)
		//{
		//	//System.Diagnostics.Debug.WriteLine ("\tPopupBaseRenderer.OnLayout(" + changed + ", " + l + ", " + t + ", " + r + ", " + b + ")");
		//	base.OnLayout (changed, l, t, r, b);
			/*
			if (this.Control == null) {
				return;
			}
			TNativeView expr_37 = (this.container == this) ? this.Control : this.container;
			expr_37.Measure (MeasureSpecFactory.MakeMeasureSpec (r - l, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec (b - t, MeasureSpecMode.Exactly));
			expr_37.Layout (0, 0, r - l, b - t);
			*/
		//}

	}
}

