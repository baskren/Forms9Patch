using System;
namespace Forms9Patch
{
	class ItemWrapperTapEventArgs : FormsGestures.TapEventArgs
	{
		public ItemWrapper ItemWrapper;

		public ItemWrapperTapEventArgs(ItemWrapper itemWrapper)
		{
			ItemWrapper = itemWrapper;
		}
	}
}
