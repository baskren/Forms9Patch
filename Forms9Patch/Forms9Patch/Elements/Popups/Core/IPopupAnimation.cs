using System.Threading.Tasks;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Popup animation interface
    /// </summary>
    public interface IPopupAnimation
    {
        /// <summary>
        /// Called before Popup appears 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        void Preparing(View content, PopupPage page);
        /// <summary>
        /// Called after the Popup disappears
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        void Disposing(View content, PopupPage page);
        /// <summary>
        /// Called when animating the popup's appearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task Appearing(View content, PopupPage page);
        /// <summary>
        /// Called when animating the popup's disappearance
        /// </summary>
        /// <param name="content"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task Disappearing(View content, PopupPage page);
    }
}
