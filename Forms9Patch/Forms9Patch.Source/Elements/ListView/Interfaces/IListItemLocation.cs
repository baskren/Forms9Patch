using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// List item location Interface.
	/// </summary>
	public interface IListItemLocation
	{
		/// <summary>
		/// Return DragEventArgs for Item at point.
		/// </summary>
		/// <returns>The at point.</returns>
		/// <param name="listView">List view.</param>
		/// <param name="p">P.</param>
		DragEventArgs DragEventArgsForItemAtPoint(ListView listView, Point p);
	}
}

