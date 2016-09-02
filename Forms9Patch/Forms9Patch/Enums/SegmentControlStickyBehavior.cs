using System;

namespace Forms9Patch
{
	/// <summary>
	/// What happens when a segment is selected
	/// </summary>
	public enum SegmentControlStickyBehavior
	{
		/// <summary>
		/// No buttons are sticky
		/// </summary>
		None,
		/// <summary>
		/// Only one button can be in the selected state
		/// </summary>
		Radio,
		/// <summary>
		/// None, any or all the buttons can be in the selected state
		/// </summary>
		Multiselect,
	}
}

