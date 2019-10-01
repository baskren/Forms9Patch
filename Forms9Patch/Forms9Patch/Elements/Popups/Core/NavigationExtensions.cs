using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Navigation extensions
    /// </summary>
    public static class NavigationExtension
    {
        /// <summary>
        /// Display popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PushPopupAsync(this INavigation sender, PopupPage page, bool animate = true)
            => PopupNavigation.Instance.PushAsync(page, animate);

        /// <summary>
        /// Remove popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PopPopupAsync(this INavigation sender, bool animate = true)
            => PopupNavigation.Instance.PopAsync(animate);

        /// <summary>
        /// Remove all popups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task PopAllPopupAsync(this INavigation sender, bool animate = true)
            => PopupNavigation.Instance.PopAllAsync(animate);

        /// <summary>
        /// Remove a particular popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public static Task RemovePopupPageAsync(this INavigation sender, PopupPage page, bool animate = true)
            => PopupNavigation.Instance.RemovePageAsync(page, animate);

    }
}
