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
[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.iOS.BubbleLayoutRenderer))]
namespace Forms9Patch.iOS
#elif __DROID__
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.Droid.BubbleLayoutRenderer))]
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms.Platform.UWP;
[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.UWP.BubbleLayoutRenderer))]
namespace Forms9Patch.UWP
#else
namespace Forms9Patch
#endif
{
    internal class BubbleLayoutRenderer : ViewRenderer<BubbleLayout, SkiaRoundedBoxAndImageView>
    {
        #region Fields
        bool _disposed;

        //bool _debugMessages;

        static int _instances;
        int _instance;
        #endregion

        #region Constructor / Disposer
        public BubbleLayoutRenderer()
        {
            _instances = _instance++;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                //Control?.Dispose();
                //SetNativeControl(null);
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Size Management


#if __IOS__


        public override void Draw(CoreGraphics.CGRect rect)
        {
            SendSubviewToBack(Control);
            base.Draw(rect);
        }

#elif __DROID__

#elif WINDOWS_UWP
        protected override void UpdateBackgroundColor()
        {
            base.UpdateBackgroundColor();
            Background = new SolidColorBrush(Colors.Transparent);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            // Since layouts in Forms can be interacted with, we need to create automation peers
            // for them so we can interact with them in automated tests
            return new FrameworkElementAutomationPeer(this);
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control.Height = ActualHeight + 1;
            Control.Width = ActualWidth + 1;
        }

#endif

        /*
#if __DROID__
        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] constraints=[" + widthConstraint + ", " + heightConstraint + "]");
            var result = Control != null ? Control.GetDesiredSize(widthConstraint, heightConstraint) : default(Xamarin.Forms.SizeRequest);
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "]");
            return result;
        }
#else
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] constraints=[" + widthConstraint + ", " + heightConstraint + "]");
            var result = Control != null ? Control.GetDesiredSize(widthConstraint, heightConstraint) : default(Xamarin.Forms.SizeRequest);
            //if (_debugMessages) System.Diagnostics.Debug.WriteLine("[" + GetType() + "][" + PCL.Utils.ReflectionExtensions.CallerMemberName() + "] result=[" + result + "]");
            return result;
        }
#endif
*/
        #endregion

        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<BubbleLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //SizeChanged -= OnSizeChanged;
                SetAutomationId(null);
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(e.NewElement as IShape));
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