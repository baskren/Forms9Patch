using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// List item location Interface.
	/// </summary>
	public interface IListItemLocation
	{
		/// <summary>
		/// Return CellProximityEventArgs for Item at point.
		/// </summary>
		/// <returns>The at point.</returns>
		/// <param name="listView">List view.</param>
		/// <param name="p">P.</param>
		CellProximityEventArgs CellProximityEventArgsForItemAtPoint(ListView listView, Point p);
	}
}

