using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.UWP.BubbleLayoutRenderer))]
namespace Forms9Patch.UWP
{
    internal class BubbleLayoutRenderer : ViewRenderer<BubbleLayout, SkiaRoundedBoxAndImageView> 
    {
        #region Fields
        bool _disposed;

        bool _debugMessages=false;

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
                Control?.Dispose();
                SetNativeControl(null);
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Size Management
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
        #endregion

        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<BubbleLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                SizeChanged -= OnSizeChanged;
                SetAutomationId(null);
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(e.NewElement as IShape));
                SizeChanged += OnSizeChanged;
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

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            // Since layouts in Forms can be interacted with, we need to create automation peers
            // for them so we can interact with them in automated tests
            return new FrameworkElementAutomationPeer(this);
        }

        #endregion
	}
}