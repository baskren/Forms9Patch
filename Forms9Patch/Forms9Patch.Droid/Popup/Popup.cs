using System;
using System.Linq;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms;

namespace Forms9Patch
{
    public static class Popup
    {
        public static bool SendBackPressed(Action onBackPressedHandler=null)
        {
            if (PopupNavigation.Instance.PopupStack.LastOrDefault() is PopupPage lastPage)
            {
                if (!(lastPage.DisappearingTransactionTask != null || lastPage.SendBackButtonPressed()))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.Instance.PopAsync();
                    });
                }
                return true;
            }
            onBackPressedHandler?.Invoke();
            return false;
        }
    }
}

