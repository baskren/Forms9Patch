using System;

namespace FormsGestures
{
    /// <summary>
    /// Direction of gesture
    /// </summary>
	[Flags]
	public enum Direction
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        NotClear = 0,
        Left = 1,
		Right = 2,
		Up = 4,
		Down = 8,
		Horizontal = 3,
		Vertical = 12,
		All = 15,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Direction extension.
    /// </summary>
    public static class DirectionExtension
	{
		/// <summary>
		/// Is the Direction horizontal?
		/// </summary>
		/// <param name="dir">Direction instance.</param>
		public static bool IsHorizontal(this Direction dir)
		{
			return (dir | Direction.Horizontal) > 0;
		}

		/// <summary>
		/// Is the Direction vertical?
		/// </summary>
		/// <param name="dir">Direction instance.</param>
		public static bool IsVertical(this Direction dir)
		{
			return (dir | Direction.Vertical) > 0;
		}

	}
}
