namespace Forms9Patch
{
	public class SwipeMenuItemTappedArgs
	{
		public ICellContentView CellContentView { get; private set;}

		public object Item { get; private set; }

		public object Group { get; private set; }

		public SwipeMenuItem SwipeMenuItem { get; private set; }

		internal SwipeMenuItemTappedArgs(ICellContentView cellContentView, ItemWrapper itemWrapper, SwipeMenuItem swipeMenuItem)
		{
			CellContentView = cellContentView;
			Item = itemWrapper.Source;
			Group = itemWrapper.BindingContext;
			SwipeMenuItem = swipeMenuItem;
		}

	}
}