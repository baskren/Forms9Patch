using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch List view extensions.
	/// </summary>
	public static class ListViewExtensions
	{
		static IListViewExtensionService _service;

		static IListViewExtensionService Service()
		{
			_service = _service ?? DependencyService.Get<IListViewExtensionService>();
			if (_service == null)
				throw new TypeLoadException("Cannot load IListViewExtensionService dependency service");
			return _service;
		}

		/// <summary>
		/// Indexs the path at center.
		/// </summary>
		/// <returns>The path at center.</returns>
		/// <param name="listView">List view.</param>
		public static Tuple<int,int> IndexPathAtCenter(this Xamarin.Forms.ListView listView)
		{
			return Service().IndexPathAtCenter(listView);
		}

		/// <summary>
		/// Selecteds the index path.
		/// </summary>
		/// <returns>The index path.</returns>
		/// <param name="listView">List view.</param>
		public static Tuple<int, int> SelectedIndexPath(this Xamarin.Forms.ListView listView)
		{
			return Service().SelectedIndexPath(listView);
		}

		/// <summary>
		/// Indexs the path at point.
		/// </summary>
		/// <returns>The path at point.</returns>
		/// <param name="listView">List view.</param>
		/// <param name="p">P.</param>
		public static Tuple<int, int> IndexPathAtPoint(this Xamarin.Forms.ListView listView, Point p)
		{
			return Service().IndexPathAtPoint(listView, p);
		}

	}
}

