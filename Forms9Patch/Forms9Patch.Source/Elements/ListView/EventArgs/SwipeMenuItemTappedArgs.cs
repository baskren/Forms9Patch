namespace Forms9Patch
{
	/// <summary>
	/// Cell swipe menu item tapped arguments.
	/// </summary>
	public class SwipeMenuItemTappedArgs
	{
		/// <summary>
		/// Gets the cell content view where the swipe happened.
		/// </summary>
		/// <value>The cell content view.</value>
		public ICellSwipeMenus CellContentView { get; private set;}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <value>The item.</value>
		public object Item { get; private set; }

		/// <summary>
		/// Gets the group.
		/// </summary>
		/// <value>The group.</value>
		public object Group { get; private set; }

		/// <summary>
		/// Gets the swipe menu item that was tapped.
		/// </summary>
		/// <value>The swipe menu item.</value>
		public SwipeMenuItem SwipeMenuItem { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.SwipeMenuItemTappedArgs"/> class.
		/// </summary>
		/// <param name="cellContentView">Cell content view.</param>
		/// <param name="itemWrapper">Item wrapper.</param>
		/// <param name="swipeMenuItem">Swipe menu item.</param>
		internal SwipeMenuItemTappedArgs(ICellSwipeMenus cellContentView, ItemWrapper itemWrapper, SwipeMenuItem swipeMenuItem)
		{
			CellContentView = cellContentView;
			Item = itemWrapper.Source;
			Group = ((GroupWrapper)itemWrapper.BindingContext).Source;
			SwipeMenuItem = swipeMenuItem;
		}

	}
}