using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Forms9Patch;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(Bc3.Forms.iOS.ListViewExtensionService_iOS))]
namespace Bc3.Forms.iOS
{
	/// <summary>
	/// List view extension service  i os.
	/// </summary>
	public class ListViewExtensionService_iOS : IListViewExtensionService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Bc3.Forms.iOS.ListViewExtensionService_iOS"/> class.
		/// </summary>
		public ListViewExtensionService_iOS() { }

		/// <summary>
		/// Indexs the path at center.
		/// </summary>
		/// <returns>The path at center.</returns>
		/// <param name="listView">List view.</param>
		public Tuple<int, int> IndexPathAtCenter(Xamarin.Forms.ListView listView)
		{
			var renderer = Renderer(listView);
			//var center = renderer.Control.Bounds.ToRectangle().Center;
			//var center = new Point(renderer.Control.Bounds.Width/2.0, renderer.Control.Bounds.Height/2.0);
			var center = renderer.Control.Bounds.ToRectangle().Center;
			//System.Diagnostics.Debug.WriteLine("Bounds=["+renderer.Control.Bounds+"] center=["+center+"]");
			var indexPath = renderer.Control.IndexPathForRowAtPoint(new CoreGraphics.CGPoint(center.X, center.Y));
			if (indexPath != null)
				return new Tuple<int, int>(indexPath.Section, indexPath.Row);
			else
				return null;
			//return IndexPathAtPoint(listView, center);
		}

		/// <summary>
		/// Selecteds the index path.
		/// </summary>
		/// <returns>The index path.</returns>
		/// <param name="listView">List view.</param>
		public Tuple<int, int> SelectedIndexPath(Xamarin.Forms.ListView listView)
		{
			var indexPath = Renderer(listView).Control.IndexPathForSelectedRow;
			return new Tuple<int, int>(indexPath.Section, indexPath.Row);
		}

		ListViewRenderer Renderer(Xamarin.Forms.ListView listView)
		{
			IVisualElementRenderer visualElementRenderer = Platform.GetRenderer(listView);
			var listViewRenderer = visualElementRenderer as ListViewRenderer;
			if (listViewRenderer == null)
				throw new MissingMemberException("ListViewRenderer not found for ListView");
			return listViewRenderer;
		}

		/// <summary>
		/// Indexs the path at point.
		/// </summary>
		/// <returns>The path at point.</returns>
		/// <param name="listView">List view.</param>
		/// <param name="p">P.</param>
		public Tuple<int, int> IndexPathAtPoint(Xamarin.Forms.ListView listView, Xamarin.Forms.Point p)
		{
			var renderer = Renderer(listView);
			var point = new CoreGraphics.CGPoint(p.X * Display.Scale, p.Y * Display.Scale);
			//System.Diagnostics.Debug.WriteLine("size=["+renderer.Control.Frame.Size+"]");
			//System.Diagnostics.Debug.WriteLine("point=["+point+"]");
			var visibleCells = renderer.Control.VisibleCells;
			var visibleCellIndexPaths = renderer.Control.IndexPathsForVisibleRows;
			var scroll = renderer.Control.ContentOffset;
			//System.Diagnostics.Debug.WriteLine("Scroll=["+scroll+"]");
			foreach (var ip in visibleCellIndexPaths)
			{
				var cell = renderer.Control.CellAt(ip);
				var frame = cell.Frame;
				var frameX = cell.ConvertRectToView(frame, renderer.Control);
				var frameZ = new CoreGraphics.CGRect(frameX.X - scroll.X * Display.Scale, frameX.Y - scroll.Y * Display.Scale, frameX.Width * Display.Scale, frameX.Height * Display.Scale);
				//System.Diagnostics.Debug.WriteLine("ip=["+ip+"] frame=[" + frameZ + "]");
				if (frameZ.Contains(point))
				{
					return new Tuple<int, int>(ip.Section, ip.Row);
				}
			}
			return null;
			//System.Diagnostics.Debug.WriteLine("");
			//var indexPath = renderer.Control.IndexPathForRowAtPoint(point);
			//return new Tuple<int, int>(indexPath.Section, indexPath.Row);
		}

		/// <summary>
		/// Indexs the paths of visible cells.
		/// </summary>
		/// <returns>The paths of visible cells.</returns>
		/// <param name="listView">List view.</param>
		public List<Tuple<int, int>> IndexPathsOfVisibleCells(Xamarin.Forms.ListView listView)
		{
			var renderer = Renderer(listView);
			var visibleCellIndexPaths = renderer.Control.IndexPathsForVisibleRows;
			var results = new List<Tuple<int, int>>();
			foreach (var ip in visibleCellIndexPaths)
				results.Add(new Tuple<int, int>(ip.Section, ip.Row));
			return results;
		}

	}
}

