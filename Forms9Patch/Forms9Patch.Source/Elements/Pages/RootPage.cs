// /*******************************************************************
//  *
//  * PopupPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.RootPage is no longer needed for display of Popups!
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Forms9Patch.RootPage is no longer necessary!", false)]
    public class RootPage : Page
    {
        #region Constructor
        /// <summary>
        /// Forms9Patch.RootPage is no longer needed for display of Popups!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Forms9Patch.RootPage is no longer necessary!", false)]
        private RootPage() { }

        /// <summary>
        /// Forms9Patch.RootPage is no longer needed for display of Popups!
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Forms9Patch.RootPage is no longer necessary!", false)]
        public static Page Create(Page page) 
        {
            return page;
        }
        #endregion



    }
}
