using System;
namespace Forms9Patch
{
	class ItemWrapperTapEventArgs : FormsGestures.TapEventArgs
	{
		public ItemWrapper ItemWrapper;

		public ItemWrapperTapEventArgs(ItemWrapper itemWrapper, FormsGestures.TapEventArgs source=null)
		{
			ItemWrapper = itemWrapper;
            if (source != null)
                ValueFrom(source);
		}
	}

    class ItemWrapperPanEventArgs : FormsGestures.PanEventArgs
    {
        public ItemWrapper ItemWrapper;

        public ItemWrapperPanEventArgs(ItemWrapper itemWrapper, FormsGestures.PanEventArgs source=null)
        {
            ItemWrapper = itemWrapper;
            if (source != null)
                ValueFrom(source);
        }
    }
}
