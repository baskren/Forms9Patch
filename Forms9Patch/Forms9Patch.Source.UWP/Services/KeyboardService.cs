using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.KeyboardService))]
namespace Forms9Patch.UWP
{
    class KeyboardService : IKeyboardService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.iOS.KeyboardService"/> class.
        /// </summary>
        public KeyboardService()
        {
            //UIKeyboard.Notifications.ObserveWillHide(OnHidden);
            //UIKeyboard.Notifications.ObserveWillShow(OnShown);
            InputPane.GetForCurrentView().Hiding += KeyboardService_Hiding;
            InputPane.GetForCurrentView().Showing += KeyboardService_Showing;
        }

        private void KeyboardService_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
        }

        private void KeyboardService_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
        }


        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            InputPane.GetForCurrentView().TryHide();
        }

    }
}
