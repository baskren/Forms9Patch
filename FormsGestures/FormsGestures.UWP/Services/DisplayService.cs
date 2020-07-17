using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Xamarin.Forms;

[assembly: Dependency(typeof(FormsGestures.UWP.DisplayService))]
namespace FormsGestures.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class DisplayService : IDisplayService
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

        public float Width => _applicationView == null ? (float) Xamarin.Forms.Application.Current.MainPage.Bounds.Width : (float)_applicationView.VisibleBounds.Width;

        public float Height => _applicationView == null ? (float)Xamarin.Forms.Application.Current.MainPage.Bounds.Height : (float)_applicationView.VisibleBounds.Height;

        public Xamarin.Forms.Thickness SafeAreaInset => default;

        public double StatusBarOffset => 0.0;

        public DisplayOrientation Orientation => Windows.Graphics.Display.DisplayInformation.GetForCurrentView().CurrentOrientation.ToF9pDisplayOrientation();

    }
}
