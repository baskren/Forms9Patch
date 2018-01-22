using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Grid extension.
	/// </summary>
	public static class GridExtension {
		
		/// <summary>
		/// More logical way to add child to Grid
		/// </summary>
		/// <param name="grid">Grid.</param>
		/// <param name="view">View.</param>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		/// <param name="rowspan">Rowspan.</param>
		/// <param name="columnspan">Columnspan.</param>
		public static void AddChild(this Xamarin.Forms.Grid grid, View view, int row, int column, int rowspan = 1, int columnspan = 1) {
			if (row < 0)
				throw new ArgumentOutOfRangeException("row");
			if (column < 0)
				throw new ArgumentOutOfRangeException("column");
			if (rowspan <= 0)
				throw new ArgumentOutOfRangeException("rowspan");
			if (columnspan <= 0)
				throw new ArgumentOutOfRangeException("columnspan");
			if (view == null)
				throw new ArgumentNullException("view");

			Xamarin.Forms.Grid.SetRow((BindableObject)view, row);
			Xamarin.Forms.Grid.SetRowSpan((BindableObject)view, rowspan);
			Xamarin.Forms.Grid.SetColumn((BindableObject)view, column);
			Xamarin.Forms.Grid.SetColumnSpan((BindableObject)view, columnspan);

			grid.Children.Add(view);
		}
	}
}

