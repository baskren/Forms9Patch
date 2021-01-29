using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.EntryNoOutlineEffect), "EntryNoOutlineEffect")]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EntryNoOutlineEffect : PlatformEffect
    {
        protected override void OnAttached()
            => ((UITextField)Control).BorderStyle = UITextBorderStyle.None;


        protected override void OnDetached() { }
    }
}
