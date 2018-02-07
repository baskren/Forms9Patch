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
		/// <summary>
		/// Drags the event arguments for item at point.
		/// </summary>
		/// <returns>The event arguments for item at point.</returns>
		/// <param name="listView">List view.</param>
		/// <param name="p">P.</param>
		public CellProximityEventArgs CellProximityEventArgsForItemAtPoint(ListView listView, Point p) {
			var listViewRenderer = Platform.GetRenderer (listView) as ListViewRenderer;
			return listViewRenderer?.ItemAtPoint (p);
		}

		#endregion

	}
}

