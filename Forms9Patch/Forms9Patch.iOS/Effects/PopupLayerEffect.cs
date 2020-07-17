using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using ObjCRuntime;
using Xamarin.Forms.Internals;

[assembly: ExportEffect(typeof(Forms9Patch.iOS.PopupLayerEffect), "PopupLayerEffect")]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PopupLayerEffect : PlatformEffect
    {

        protected override void OnAttached()
        {
            if (Element.Effects.FirstOrDefault(e => e is Forms9Patch.PopupLayerEffect) is Forms9Patch.PopupLayerEffect effect && effect.PopupBase != null)
            {
                var layer = Forms9Patch.Elements.Popups.Core.PopupNavigation.Instance.PopupStack.IndexOf(effect.PopupBase) + 1;
                if (Container?.Window != null)
                    Container.Window.WindowLevel = layer;// UIWindowLevel.Alert;
                if (Control?.Window != null)
                    Control.Window.WindowLevel = layer; // UIWindowLevel.Alert;
            }
        }

        protected override void OnDetached()
        {
        }


    }
}
