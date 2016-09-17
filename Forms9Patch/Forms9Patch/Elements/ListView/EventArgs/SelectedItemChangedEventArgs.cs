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


		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.SelectedItemChangedEventArgs"/> class.
		/// </summary>
		/// <param name="group">Group.</param>
		/// <param name="selectedItem">Selected item.</param>
		public SelectedItemChangedEventArgs(object group, object selectedItem, View cellView) : base (selectedItem)
		{
			Group = group;
			CellView = cellView;
		}
	}
}

