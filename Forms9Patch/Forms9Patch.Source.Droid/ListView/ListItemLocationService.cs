using Forms9Patch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency (typeof(ListItemLocationService))]
namespace Forms9Patch.Droid
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

