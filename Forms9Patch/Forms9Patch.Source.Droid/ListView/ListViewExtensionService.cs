using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.ListViewExtensionService))]
namespace Forms9Patch.Droid
{
	internal class ListViewExtensionService : IListViewExtensionService
	{
		public ListViewExtensionService()
		{
		}

		public Tuple<int,int> IndexPathAtCenter(Xamarin.Forms.ListView listView)
		{
			var renderer = Renderer(listView);
			var centerY = (renderer.Control.Bottom - renderer.Control.Top)/2;
			var centerX = (renderer.Control.Right - renderer.Control.Left)/2;
			int pos = renderer.Control.PointToPosition(centerX, centerY)-1;
			return PositionToIndexPath(listView, pos);
		}

		public Tuple<int, int> SelectedIndexPath(Xamarin.Forms.ListView listView)
		{
			var renderer = Renderer(listView);
			var view = renderer.Control.SelectedView;
			var pos = renderer.Control.IndexOfChild(view);
			return PositionToIndexPath(listView, pos);
		}

		public Tuple<int, int> IndexPathAtPoint(Xamarin.Forms.ListView listView, Xamarin.Forms.Point p)
		{
			var renderer = Renderer(listView);
			var displayMetrics = global::Android.App.Application.Context.Resources.DisplayMetrics;
			var scale = displayMetrics.Density;

			int pos = renderer.Control.PointToPosition((int)(p.X * scale), (int)(p.Y * scale)) - 1;
			return PositionToIndexPath(listView, pos);
		}


		Tuple<int, int> PositionToIndexPath(Xamarin.Forms.ListView listView, int pos)
		{
			if (listView.IsGroupingEnabled)
			{
				var templatedView = listView as ITemplatedItemsView<Cell>;
				var templatedItems = templatedView.TemplatedItems;
				int leftOver;
				var groupIndex = templatedItems.GetGroupIndexFromGlobal(pos, out leftOver);
				return new Tuple<int, int>(groupIndex, leftOver-1);
			}
			return new Tuple<int, int>(0, pos);
		}

		Xamarin.Forms.Platform.Android.ListViewRenderer Renderer(Xamarin.Forms.ListView listView)
		{
			IVisualElementRenderer visualElementRenderer = Platform.GetRenderer(listView);
			var listViewRenderer = visualElementRenderer as Xamarin.Forms.Platform.Android.ListViewRenderer;
			if (listViewRenderer == null)
				throw new MissingMemberException("ListViewRenderer not found for ListView");
			return listViewRenderer;
		}

		public List<Tuple<int, int>> IndexPathsOfVisibleCells(Xamarin.Forms.ListView listView)
		{
			var renderer = Renderer(listView);
			var first = renderer.Control.FirstVisiblePosition;
			var last = renderer.Control.LastVisiblePosition;
			var results = new List<Tuple<int, int>>();
			for (int i = first; i <= last; i++)
				results.Add(PositionToIndexPath(listView, i));
			return results;
		}

		public void NativeDeselect(Xamarin.Forms.ListView listView)
		{
			throw new NotImplementedException();
		}
	}
}

