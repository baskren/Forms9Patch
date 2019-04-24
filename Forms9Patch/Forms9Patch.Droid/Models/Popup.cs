using System;

using Xamarin.Forms;

namespace Forms9Patch
{
    public static class Popup
    {
        public static bool SendBackPressed(Action onBackPressedHandler)
            => Rg.Plugins.Popup.Popup.SendBackPressed(onBackPressedHandler);
    }
}

