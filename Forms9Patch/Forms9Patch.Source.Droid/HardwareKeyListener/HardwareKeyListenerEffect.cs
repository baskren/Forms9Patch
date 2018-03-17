using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Views;

[assembly: ExportEffect(typeof(Forms9Patch.Droid.HardwareKeyListenerEffect), "HardwareKeyListenerEffect")]
namespace Forms9Patch.Droid
{
    public class HardwareKeyListenerEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
                Control.KeyPress += OnControlKeyPress;
            else if (Container != null)
                Container.KeyPress += OnControlKeyPress;
        }

        protected override void OnDetached()
        {
            if (Control != null)
                Control.KeyPress -= OnControlKeyPress;
            else if (Container != null)
                Container.KeyPress -= OnControlKeyPress;
        }

        private void OnControlKeyPress(object sender, Android.Views.View.KeyEventArgs e)
        {
            if (e.Event.Action == KeyEventActions.Down)
                e.Handled = HardwareKeyListener.OnKeyDown(e.KeyCode, e.Event);
            else
                e.Handled = false;
            //System.Diagnostics.Debug.WriteLine("HardwareKeyListenerEffect.OnControlKeyPress [" + e.Event + "][" + e.Handled + "][" + e.KeyCode + "]");
        }


    }
}