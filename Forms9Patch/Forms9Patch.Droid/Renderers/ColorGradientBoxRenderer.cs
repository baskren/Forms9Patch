using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Forms9Patch;
using Forms9Patch.Droid;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(ColorGradientBox), typeof(ColorGradientBoxRenderer))]
namespace Forms9Patch.Droid
{
    class ColorGradientBoxRenderer : VisualElementRenderer<ColorGradientBox>
    {
        public ColorGradientBoxRenderer(Android.Content.Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<ColorGradientBox> e)
        {
            base.OnElementChanged(e);
            GradientDrawable.Orientation orientation = (Element.Orientation == StackOrientation.Horizontal ? GradientDrawable.Orientation.LeftRight : GradientDrawable.Orientation.BottomTop);
            int[] colors = { Element.EndColor.ToAndroid(), Element.StartColor.ToAndroid() };
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
#pragma warning disable CS0618 // Type or member is obsolete
                SetBackgroundDrawable(e.NewElement != null ? new GradientDrawable(orientation, colors) : null);
#pragma warning restore CS0618 // Type or member is obsolete
            else
            {
                if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
#pragma warning disable CS0618 // Type or member is obsolete
                    SetBackgroundDrawable(e.NewElement != null ? new GradientDrawable(orientation, colors) : null);
#pragma warning restore CS0618 // Type or member is obsolete
                else
                    Background = e.NewElement != null ? new GradientDrawable(orientation, colors) : null;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            /*
			if (e.PropertyName == BaseCellView.SeparatorIsVisibleProperty.PropertyName 
				|| e.PropertyName == BaseCellView.SeparatorColorProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorHeightProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorLeftIndentProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorRightIndentProperty.PropertyName
			) {
				GradientDrawable.Orientation orientation = (Element.Orientation == StackOrientation.Horizontal ? GradientDrawable.Orientation.LeftRight : GradientDrawable.Orientation.BottomTop);
				int[] colors = { Element.EndColor.ToAndroid (),Element.StartColor.ToAndroid () };
				Background = new GradientDrawable(orientation,colors);
			}
			*/
        }

    }
}
