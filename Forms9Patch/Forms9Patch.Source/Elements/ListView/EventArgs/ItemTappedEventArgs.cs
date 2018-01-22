using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Item tapped event arguments.
	/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ItemTappedEventArgs"/> class.
		/// </summary>
		/// <param name="group">Group.</param>
		/// <param name="item">Item.</param>
		/// <param name="cellView">Cell view.</param>
		public ItemTappedEventArgs(object group, object item, View cellView) : base (group,item)
		{
			CellView = cellView;
		}
	}
}
