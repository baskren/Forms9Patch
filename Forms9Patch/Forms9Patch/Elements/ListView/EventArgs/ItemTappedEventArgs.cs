using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	public class ItemTappedEventArgs: Xamarin.Forms.ItemTappedEventArgs
	{
		/// <summary>
		/// Gets the cell view bound to the tapped item.
		/// </summary>
		/// <value>The cell view.</value>
		public View CellView
		{
			get;
			private set;
		}

		public ItemTappedEventArgs(object group, object item, View cellView) : base (group,item)
		{
			CellView = cellView;
		}
	}
}
