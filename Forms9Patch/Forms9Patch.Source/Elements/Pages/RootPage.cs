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
    /// Forms9Patch.RootPage: OBSOLETE, use Forms9Patch.PopupPage
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
    public class RootPage : PopupPage
    {
        #region Constructor
        /// <summary>
        /// Forms9Patch.RootPage: OBSOLETE, use Forms9Patch.PopupPage
        /// </summary>
        /// <param name="page"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
        public RootPage(Page page) : base(page) { }

        /// <summary>
        /// Forms9Patch.RootPage: OBSOLETE, use Forms9Patch.PopupPage
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Forms9Patch.RootPage does not work with iOS Modal Pages.  User Forms9Patch.PopupPage instead.", false)]
        public static new RootPage Create(Page page)
        {
            //_instance = _instance ?? new RootPage(page);
            var _instance = new RootPage(page);
            return _instance;
        }
        #endregion



    }
}
