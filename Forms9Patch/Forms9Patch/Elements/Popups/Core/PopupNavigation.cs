using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Popup navigation manager
    /// </summary>
    public static class PopupNavigation
    {
        /// <summary>
        /// Sets the instance
        /// </summary>
        /// <param name="instance"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetInstance(IPopupNavigation instance) => _popupNavigation = instance;

        private static IPopupNavigation _popupNavigation;
        /// <summary>
        /// Singleton
        /// </summary>
        public static IPopupNavigation Instance => _popupNavigation = _popupNavigation ?? new PopupNavigationImpl();


        /// <summary>
        /// The stack
        /// </summary>
        public static IReadOnlyList<PopupPage> PopupStack => Instance.PopupStack;

        /// <summary>
        /// Display a popup 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PushAsync(PopupPage page, bool animate = true)
            => Instance.PushAsync(page, animate);


        /// <summary>
        /// Remove a popup
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PopAsync(bool animate = true)
            => Instance.PopAsync(animate);


        /// <summary>
        /// Remove all popups
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PopAllAsync(bool animate = true)
            => Instance.PopAllAsync(animate);


        /// <summary>
        /// Remove a particular popup
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task RemovePageAsync(PopupPage page, bool animate = true)
            => Instance.RemovePageAsync(page, animate);


    }
}
