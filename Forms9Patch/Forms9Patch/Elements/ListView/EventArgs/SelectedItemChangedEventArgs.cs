using System;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Selected item changed event arguments.
    /// </summary>
    public class SelectedItemChangedEventArgs : Xamarin.Forms.SelectedItemChangedEventArgs
    {
        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <value>The group.</value>
        public object Group
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the CellView bound to the selected item
        /// </summary>
        /// <value>The cell view.</value>
        public Xamarin.Forms.View CellView
        {
            get;
            private set;
        }

        internal SelectedItemChangedEventArgs(ItemWrapper itemWrapper) : base(itemWrapper.Source, itemWrapper.Index)
        {
            if (itemWrapper?.Parent is GroupWrapper groupWrapper)
                Group = groupWrapper.Source;
            CellView = itemWrapper.CellView;
        }
    }
}

