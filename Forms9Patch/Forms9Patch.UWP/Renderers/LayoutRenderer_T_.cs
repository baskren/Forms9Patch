using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;

namespace Forms9Patch.UWP
{
    internal class LayoutRenderer<TElement> : ViewRenderer<TElement, SkiaRoundedBoxAndImageView> where TElement : Layout, IBackground
    {
        #region Fields
        bool _disposed;

        static int _instances;
        int _instance;
        #endregion


        #region Constructor / Disposer
        public LayoutRenderer() => _instances = _instance++;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (Control!=null)
                    Control?.Dispose();
                SetNativeControl(null);
                _disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion


        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
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
                    SetNativeControl(new SkiaRoundedBoxAndImageView(e.NewElement as IRoundedBox));
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

        #endregion
    }
}
