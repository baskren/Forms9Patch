using System;
using System.Collections.Generic;

namespace Forms9Patch
{
	internal interface IListViewExtensionService
	{
		Tuple<int,int> IndexPathAtCenter(Xamarin.Forms.ListView listView);

		Tuple<int, int> SelectedIndexPath(Xamarin.Forms.ListView listView);

		Tuple<int, int> IndexPathAtPoint(Xamarin.Forms.ListView listView, Xamarin.Forms.Point p);

		List<Tuple<int, int>> IndexPathsOfVisibleCells(Xamarin.Forms.ListView listView);

	}
}

