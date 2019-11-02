using System.ComponentModel;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    class ItemWrapperLongPressEventArgs : FormsGestures.LongPressEventArgs
    {
        public ItemWrapper ItemWrapper;

        public ItemWrapperLongPressEventArgs(ItemWrapper itemWrapper)
        {
            ItemWrapper = itemWrapper;
        }
    }
}