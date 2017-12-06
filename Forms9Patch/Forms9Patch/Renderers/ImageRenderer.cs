using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
namespace Forms9Patch.iOS
#elif __DROID__
using Xamarin.Forms.Platform.Android;
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.Image), typeof(Forms9Patch.UWP.ImageRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    public class ImageRenderer : ViewRenderer<Image, SkiaRoundedBoxAndImageView>
    {
        #region Fields
        bool _disposed;

        bool _debugMessages;

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


        #region Size Management

#if WINDOWS_UWP
        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control.Height = ActualHeight + 1;//Math.Round(ActualHeight * Display.Scale + 0.75) / Display.Scale;
            Control.Width = ActualWidth + 1;
        }
#endif

#if __DROID__
        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            var result = Control != null ? Control.GetDesiredSize(widthConstraint, heightConstraint) : default(Xamarin.Forms.SizeRequest);
            return result;
        }
#else
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] constraints=[" + widthConstraint + ", " + heightConstraint + "]");
            var result = Control != null ? Control.GetDesiredSize(widthConstraint, heightConstraint) : default(Xamarin.Forms.SizeRequest);
            if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "]");
            return result;
        }
#endif

        #endregion


        #region Drawing
#if __IOS__
#elif __DROID__
        // THIS DOENS"T SEEM TO DO ANY THING
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }
#elif WINDOWS_UWP
#else
#endif
        #endregion

        #region Change managements
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //TODO: does this break UWP?
                //SizeChanged -= OnSizeChanged;
                SetAutomationId(null);
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(Element));
                //TODO: does this break UWP?
                //SizeChanged += OnSizeChanged;
                if (!string.IsNullOrEmpty(Element.AutomationId))
                    SetAutomationId(Element.AutomationId);
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
