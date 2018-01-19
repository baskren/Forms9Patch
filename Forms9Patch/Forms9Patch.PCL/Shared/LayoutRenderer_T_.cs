using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#if __IOS__
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using SkiaSharp;
namespace Forms9Patch.iOS
#elif __DROID__
using Xamarin.Forms.Platform.Android;
namespace Forms9Patch.Droid
#elif WINDOWS_UWP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;
namespace Forms9Patch.UWP
#else 
namespace Forms9Patch
#endif

{
    class F9pLayoutRenderer<TElement> : ViewRenderer<TElement, SkiaRoundedBoxAndImageView> where TElement : Layout, ILayout  // works but renders SkiaRoundedBoxAndImageView over children views
                                                                                                                             //class F9pLayoutRenderer<TElement> : VisualElementRenderer<TElement> where TElement : View, ILayout // VisualElement, IBackgroundImage
    {
        #region Fields
        static int _instances;
        int _instance;
        #endregion


        #region Constructor / Disposer
        public F9pLayoutRenderer() => _instance = _instances++;
        #endregion


        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
#if WINDOWS_UWP
                SizeChanged -= OnSizeChanged;
#endif
                SetAutomationId(null);
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new SkiaRoundedBoxAndImageView(e.NewElement as IShape));
#if __IOS__

#endif
#if WINDOWS_UWP
                SizeChanged += OnSizeChanged;
#endif

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





#if __IOS__

        public override void SubviewAdded(UIView uiview)
        {
            base.SubviewAdded(uiview);

            if (uiview is SkiaRoundedBoxAndImageView)
                SendSubviewToBack(uiview);
            else
                BringSubviewToFront(uiview);
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
        #endregion
    }
}
