using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Popup navigation interface
    /// </summary>
    public interface IPopupNavigation
    {
        /// <summary>
        /// Triggered when a popup is pushed
        /// </summary>
        event EventHandler<NavigationEventArgs> Pushed;

        /// <summary>
        /// Triggered when a popup is popped
        /// </summary>
        event EventHandler<NavigationEventArgs> Popped;

        /// <summary>
        /// Triggered when all popups are popped
        /// </summary>
        event EventHandler<AllPagesPoppedEventArgs> PoppedAll;


        /// <summary>
        /// The stack of popups
        /// </summary>
        IReadOnlyList<PopupPage> PopupStack { get; }

        /// <summary>
        /// Add popup to stack
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task PushAsync(PopupPage page, bool animate = true);

        /// <summary>
        /// Remove popup from stack
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task PopAsync(bool animate = true);

        /// <summary>
        /// Clear all popups
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task PopAllAsync(bool animate = true);

        /// <summary>
        /// Remove a particular page from stack
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task RemovePageAsync(PopupPage page, bool animate = true);


    }
}
