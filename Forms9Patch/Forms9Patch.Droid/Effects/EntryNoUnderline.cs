using System;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(Forms9Patch.Droid.EntryNoUnderlineEffect), "EntryNoUnderlineEffect")]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EntryNoUnderlineEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control is Xamarin.Forms.Platform.Android.FormsEditText control)
            {
                control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                control.SetPadding(control.PaddingLeft, 0, control.PaddingRight, 0);
            }
        }

        protected override void OnDetached() { }



    }

}
