
namespace Forms9Patch
{
	/// <summary>
	/// The position of a cell being dragged relative to the cell it is hovering over
	/// </summary>
	public enum Proximity {
		/// <summary>
		/// None: Can't be calculated or the cell being dragged isn't hovering over.
		/// </summary>
		None,
		/// <summary>
		/// Before: the cell being dragged over is biased towards the left/top of the cell it is hovering over.
		/// </summary>
		Before,
		/// <summary>
		/// Center: the cell being dragged over is centered over the cell it is hovering over.
		/// </summary>
		Aligned,
		/// <summary>
		/// After: the cell being dragged over is biased towards the right/bottom of the cell it is hovering over.
		/// </summary>
		After,
	};
}

