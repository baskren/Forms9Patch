using System;
using Xamarin.Forms;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(Forms9Patch.HardwareKeyPage), typeof(Forms9Patch.Droid.HardwareKeyPageRenderer))]
namespace Forms9Patch.Droid
{
    class HardwareKeyPageRenderer : Xamarin.Forms.Platform.Android.PageRenderer
    {
        static HardwareKeyPageRenderer()
        {
            //Forms9Patch.FocusMonitor.FocusedElementChanged += OnFocusedElementChanged;
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public HardwareKeyPageRenderer(System.IntPtr intPtr, Android.Runtime.JniHandleOwnership owner) { }
#pragma warning restore CS0618

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRenderer"/> class.
        /// </summary>
        public HardwareKeyPageRenderer(Context context) : base(context) { }

        public HardwareKeyPageRenderer(Context context, object obj) : base(context) { }

        [Obsolete("This constructor is obsolete as of version 2.5. Please use PageRenderer(Context) instead.")]
        public HardwareKeyPageRenderer() { }


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