namespace Forms9Patch
{
	class ItemWrapperLongPressEventArgs : FormsGestures.LongPressEventArgs
	{
		public ItemWrapper ItemWrapper;

		public ItemWrapperLongPressEventArgs(ItemWrapper itemWrapper)
		{
			ItemWrapper = itemWrapper;
		}
	}
}