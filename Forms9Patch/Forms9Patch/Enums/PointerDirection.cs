using System;


namespace Forms9Patch
{
	
	/// <summary>
	/// BubblePopup Pointer direction.
	/// </summary>
	[Flags]
	public enum PointerDirection
	{
		/// <summary>
		/// No pointer
		/// </summary>
		None = 0,
		/// <summary>
		/// BubblePopup pointer points up
		/// </summary>
		Left = 1,
		/// <summary>
		/// BubblePopup pointer points down.
		/// </summary>
		Up = 2,
		/// <summary>
		/// BubblePopup pointer points left.
		/// </summary>
		Right = 4,
		/// <summary>
		/// BubblePopup pointer points right.
		/// </summary>
		Down = 8,
		/// <summary>
		/// BubblePopup pointer points in what ever direction allow for it to have the most space.
		/// </summary>
		Any = 15,
		/// <summary>
		/// BubblePopup pointer points either right or left, depending on which affords the most space.
		/// </summary>
		Horizontal = 5,
		/// <summary>
		/// BubblePopup pointer points either up or down, depending on which affords the most space.
		/// </summary>
		Vertical = 10,
	}
}

