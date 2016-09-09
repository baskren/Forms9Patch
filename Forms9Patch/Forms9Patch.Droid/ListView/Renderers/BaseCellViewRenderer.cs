using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Forms9Patch;
using Forms9Patch.Droid;

[assembly: ExportRenderer(typeof(Forms9Patch.BaseCellView), typeof(BaseCellViewRenderer))]
namespace Forms9Patch.Droid
{
	class BaseCellViewRenderer : ViewRenderer<BaseCellView,global::Android.Widget.RelativeLayout> {

		protected override void OnElementChanged(ElementChangedEventArgs<BaseCellView> e)
		{
			base.OnElementChanged(e);
			Background = e.NewElement != null ? new BaseCellViewDrawable (e.NewElement) : null;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == BaseCellView.SeparatorIsVisibleProperty.PropertyName 
				|| e.PropertyName == BaseCellView.SeparatorColorProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorHeightProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorLeftIndentProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorRightIndentProperty.PropertyName
			    || e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName
			) {
				Background = new BaseCellViewDrawable (Element);
			}
		}

	}
}
