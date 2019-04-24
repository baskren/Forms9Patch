using System;
using Xamarin.Forms;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.Droid.HardwareKeyPageRenderer))]
namespace Forms9Patch.Droid
{
    class HardwareKeyPageRenderer : Xamarin.Forms.Platform.Android.PageRenderer
    {
        static HardwareKeyPageRenderer()
        {
            //Forms9Patch.FocusMonitor.FocusedElementChanged += OnFocusedElementChanged;
        }

        //static Android.Views.View _control;
        static void OnFocusedElementChanged(object sender, EventArgs args)
        {
            /*
            if (FocusMonitor.FocusedElement != null)
            {
                var renderer = Platform.GetRenderer(FocusMonitor.FocusedElement);

                if (renderer is Xamarin.Forms.Platform.Android.EntryRenderer entryRenderer)
                {
                    _control = entryRenderer.Control as Android.Views.View;
                    _control.KeyPress += (object s, KeyEventArgs e) =>
                    {
                        System.Diagnostics.Debug.WriteLine("KeyPress event[" + e.Event + "] handled[" + e.Handled + "] ");
                        e.Handled = false;
                    };

                }
            }
*/
        }
    }
}