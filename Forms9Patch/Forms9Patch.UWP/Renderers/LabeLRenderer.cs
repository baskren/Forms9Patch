using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.UWP.LabeLRenderer))]
namespace Forms9Patch.UWP
{
    class LabeLRenderer : Xamarin.Forms.Platform.UWP.LabelRenderer
    {
        Forms9Patch.Label Label => Element as Forms9Patch.Label;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Forms9Patch.Label.HtmlTextProperty.PropertyName && Label.HtmlText != null)
                Control.SetF9pFormattedText(Label);
            else if (e.PropertyName == Xamarin.Forms.Label.FontFamilyProperty.PropertyName)
                Control.FontFamily = new Windows.UI.Xaml.Media.FontFamily(FontService.ReconcileFontFamily(Element.FontFamily));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.FontFamily = new Windows.UI.Xaml.Media.FontFamily(FontService.ReconcileFontFamily(e.NewElement.FontFamily));
                Control.SetF9pFormattedText(e.NewElement as Forms9Patch.Label);
            }
        }
    }
}
