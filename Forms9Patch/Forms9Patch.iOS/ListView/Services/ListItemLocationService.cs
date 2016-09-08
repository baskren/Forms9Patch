using Forms9Patch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency (typeof(ListItemLocationService))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Descendent bounds.
	/// </summary>
	public class ListItemLocationService : IListItemLocation
	{
		#region ILocation implementation

		public DragEventArgs DragEventArgsForItemAtPoint(ListView listView, Point p) {
			var listViewRenderer = Platform.GetRenderer (listView) as ListViewRenderer;
			return listViewRenderer?.ItemAtPoint (p);
		}

		#endregion

	}
}

