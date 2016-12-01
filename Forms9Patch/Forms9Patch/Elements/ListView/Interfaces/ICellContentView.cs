using System;
using System.Collections.Generic;
using FormsGestures;
namespace Forms9Patch
{
	/// <summary>
	/// Interface Cell content view.
	/// </summary>
	public interface ICellContentView
	{
		/// <summary>
		/// Gets the height of the cell if the list HasUnevenRows=true.
		/// </summary>
		/// <value>The height of the cell.</value>
		double RowHeight { get; }

		/// <summary>
		/// Gets the start swipe menu items.
		/// </summary>
		/// <value>The start swipe menu.</value>
		List<SwipeMenuItem> StartSwipeMenu { get; }

		/// <summary>
		/// Gets the end swipe menu items.
		/// </summary>
		/// <value>The end swipe menu.</value>
		List<SwipeMenuItem> EndSwipeMenu { get; }
	}
}
