using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Item tapped event arguments.
    /// </summary>
    public class ItemTappedEventArgs : Xamarin.Forms.ItemTappedEventArgs
    {
        /// <summary>
        /// Set to true if you have handled this event and don't want other handlers to have a crack at it
        /// </summary>
        public bool Handled;

        /// <summary>
        /// Gets the cell view bound to the tapped item.
        /// </summary>
        /// <value>The cell view.</value>
        public View CellView
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; private set; }

        internal ItemTappedEventArgs(ItemWrapper itemWrapper) : base(itemWrapper.Parent?.Source, itemWrapper.Source, itemWrapper.Index)
        {
            CellView = itemWrapper.CellView;
        }
    }
}
