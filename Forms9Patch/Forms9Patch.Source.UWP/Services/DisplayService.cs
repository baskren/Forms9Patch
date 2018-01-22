using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.UWP.DisplayService))]
namespace Forms9Patch.UWP
{
    class DisplayService : Forms9Patch.IDisplayService
    {
        ApplicationView _applicationView;
        ApplicationView ApplicationView
        {
            get
            {
                _applicationView = _applicationView ?? ApplicationView.GetForCurrentView();
                return _applicationView;
            }
        }

        public float Density => Scale * 160;

        public float Scale => (float)DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

        //public float Width => (float)((Windows.UI.Xaml.Controls.Frame)Window.Current.Content).ActualWidth;

        //public float Height => (float)((Windows.UI.Xaml.Controls.Frame)Window.Current.Content).ActualHeight;

        public float Width => (float)_applicationView.VisibleBounds.Width;

        public float Height => (float)_applicationView.VisibleBounds.Height;

        public Xamarin.Forms.Thickness SafeAreaInset => default(Xamarin.Forms.Thickness);


    }
}
