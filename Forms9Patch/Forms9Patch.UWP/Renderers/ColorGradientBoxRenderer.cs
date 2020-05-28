using Forms9Patch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ColorGradientBox), typeof(Forms9Patch.UWP.ColorGradientBoxRenderer))]
namespace Forms9Patch.UWP
{
    class ColorGradientBoxRenderer : VisualElementRenderer<ColorGradientBox, Rectangle>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ColorGradientBox> e)
        {
            base.OnElementChanged(e);
            if (Control == null && Element != null)
                SetNativeControl(new Rectangle());
            if (Control != null)
            {
                Control.Fill = GenerateBrush();
                Control.IsDoubleTapEnabled = false;
                Control.IsHitTestVisible = false;
                Control.IsHoldingEnabled = false;
                Control.IsRightTapEnabled = false;
                Control.IsTapEnabled = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ColorGradientBox.StartColorProperty.PropertyName
                || e.PropertyName == ColorGradientBox.EndColorProperty.PropertyName
                || e.PropertyName == ColorGradientBox.OrientationProperty.PropertyName)
                Control.Fill = GenerateBrush();

        }

        Windows.UI.Xaml.Media.LinearGradientBrush GenerateBrush()
        {
            var result = new Windows.UI.Xaml.Media.LinearGradientBrush(new Windows.UI.Xaml.Media.GradientStopCollection 
            {
                new Windows.UI.Xaml.Media.GradientStop { Color = Element.StartColor.ToWindowsColor(), Offset = 0 },
                new Windows.UI.Xaml.Media.GradientStop { Color = Element.EndColor.ToWindowsColor(), Offset = 1 }
            }, Element.Orientation == Xamarin.Forms.StackOrientation.Horizontal ? 0 : 90);

            return result;
        }
    }
}
