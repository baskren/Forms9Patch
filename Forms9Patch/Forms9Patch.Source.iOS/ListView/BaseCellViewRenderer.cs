using System;
using CoreGraphics;
using Forms9Patch.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(Forms9Patch.BaseCellView), typeof(BaseCellViewRenderer))]
namespace Forms9Patch.iOS
{
	class BaseCellViewRenderer : VisualElementRenderer<BaseCellView> {
	
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			//System.Diagnostics.Debug.WriteLine ("\t\tOEPC "+(Element!=null?((Item)Element.BindingContext).Title:"")+" "+e.PropertyName);
			/*
			if (e.PropertyName == BaseCellView.SeparatorIsVisibleProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorColorProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorHeightProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorLeftIndentProperty.PropertyName
				|| e.PropertyName == BaseCellView.SeparatorRightIndentProperty.PropertyName
				|| e.PropertyName == "Renderer"
			)
			{
				//Device.StartTimer(TimeSpan.FromMilliseconds(20), () => {
				SetNeedsDisplay();
				//	return false;
				//});
			}
			*/
		}

		public override void Draw (CGRect rect)
		{
			//if (((Item)Element.BindingContext).Title == "clr[20]") {
			//System.Diagnostics.Debug.WriteLine ("\t\tTitle=[{0}] SepVis=[{1}]",((Item)Element.BindingContext).Title,Element.SeparatorIsVisible);
			//}

			ContentMode = UIViewContentMode.Redraw;

			/*
			var separatorColor = Element.SeparatorColor;
			if (separatorColor == Color.Default) {
				var tv = new UITableView ();
				separatorColor = tv.SeparatorColor.ToColor ();
			}
			var separatorHeight = Element.SeparatorHeight<0 ? 1.0f : (nfloat)(Math.Max (0, Element.SeparatorHeight) );
			System.Diagnostics.Debug.WriteLine("BaseCellRenderer.Draw ["+separatorColor+"]["+separatorHeight+"]["+Element.SeparatorIsVisible+"]");

			base.Draw(rect);
			if (Element.SeparatorIsVisible && (separatorColor.A > 0.01 && separatorHeight > 0.01 )) {
				using (var g = UIGraphics.GetCurrentContext())
				{
					g.SaveState();
					g.SetLineWidth(separatorHeight);
					g.SetStrokeColor(separatorColor.ToCGColor());
					g.MoveTo((nfloat)Element.SeparatorLeftIndent, 0);
					g.AddLineToPoint((nfloat)(Element.Width - Element.SeparatorRightIndent), 0);
					g.StrokePath();
					g.RestoreState();
				}
			}
			*/
			BackgroundColor = Element.BackgroundColor.ToUIColor();
			//System.Diagnostics.Debug.WriteLine("BaseCellViewRenderer["+Element.ID+"].BackgroundColor=[" + BackgroundColor + "] Element.BackgroundColor=[" + Element.BackgroundColor + "]");
		}

	}
}