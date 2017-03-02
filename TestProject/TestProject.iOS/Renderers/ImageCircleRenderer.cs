using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Forms9PatchDemo;
using Forms9PatchDemo.iOS;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]
namespace Forms9PatchDemo.iOS
{
	public class ImageCircleRenderer : ImageRenderer
	{
	
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement != null || Element == null)
				return;
			CreateCircle ();
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
			    e.PropertyName == VisualElement.WidthProperty.PropertyName)
				CreateCircle ();
		}

		void CreateCircle () {
			try {
				double min = Math.Min(Element.Width, Element.Height);
				Control.Layer.CornerRadius = (float)(min/2.0);
				Control.Layer.MasksToBounds = false;
				Control.Layer.BorderColor = Color.White.ToCGColor();
				Control.Layer.BorderWidth = 3;
				Control.ClipsToBounds = true;
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine ("Unable to create circle image: " + ex);
			}
		}
	}
}

