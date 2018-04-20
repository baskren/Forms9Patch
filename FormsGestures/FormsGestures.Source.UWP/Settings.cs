using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormsGestures.UWP
{
    public class Settings
    {
        static Windows.UI.Xaml.Application _application;
        static Windows.UI.Xaml.Application Application
        {
            get
            {
                return _application ?? Windows.UI.Xaml.Application.Current;
            }
            set
            {
                _application = value;
            }
        }

        public static void Init(Windows.UI.Xaml.Application applcation)
        {
            if (_application != null)
                return;
            Application = applcation;
            P42.Utils.UWP.Settings.Init(Application);
            Xamarin.Forms.DependencyService.Register<DisplayService>();
            Xamarin.Forms.DependencyService.Register<GestureService>();
        }
    }
}
