using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.UWP.ImageRenderer))]
namespace Forms9Patch.UWP
{
    public class ImageRenderer : ViewRenderer<Image, SkiaRoundedBoxAndImageView>
    {
        #region Fields
        bool _disposed;

        bool _debugMessages=false;

        static int _instances;
        int _instance;
        #endregion

        #region Constructor / disposal
        public ImageRenderer()
        {
            _instance = _instances++;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Control?.Dispose();
                SetNativeControl(null);
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion


        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control.Height = ActualHeight + 1;//Math.Round(ActualHeight * Display.Scale + 0.75) / Display.Scale;
            Control.Width = ActualWidth + 1;
        }


        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (_debugMessages)  System.Diagnostics.Debug.WriteLine("["+GetType()+"]["+PCL.Utils.ReflectionExtensions.CallerMemberName()+"] constraints=["+widthConstraint+", "+heightConstraint+"]");
            var result = Control != null ? Control.GetDesiredSize(widthConstraint,heightConstraint) : default(Xamarin.Forms.SizeRequest);
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName()+"] result=["+result+"]");
            return result;
        }

        #region Change managements
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("ImageRenderer["+_instance+"].OnElementChanged(["+e.OldElement+", "+e.NewElement+"]");
            base.OnElementChanged(e);

            if (e.OldElement!=null)
                SizeChanged -= OnSizeChanged;
            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(Element));
                SizeChanged += OnSizeChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                return;
            base.OnElementPropertyChanged(sender, e);
        }



        #endregion
    }
}
