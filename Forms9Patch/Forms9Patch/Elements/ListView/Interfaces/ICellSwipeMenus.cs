using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Forms9Patch
{
    /// <summary>
    /// Interface to implement Swipe Menus in content views for Forms9Patch ListView cells
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public interface ICellSwipeMenus
    {
        /// <summary>
        /// Gets the start swipe menu items.
        /// </summary>
        /// <value>The start swipe menu.</value>
        List<SwipeMenuItem> StartSwipeMenu { get; }

        /// <summary>
        /// Gets the end swipe menu items.
        /// </summary>
        /// <value>The end swipe menu.</value>
        List<SwipeMenuItem> EndSwipeMenu { get; }

        /// <summary>
        /// Ons the swipe menu item button tapped.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        void OnSwipeMenuItemButtonTapped(object sender, SwipeMenuItemTappedArgs args);

    }
}
