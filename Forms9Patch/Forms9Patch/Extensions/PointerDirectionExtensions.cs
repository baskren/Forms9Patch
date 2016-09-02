using System;

using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Pointer direction extensions.
	/// </summary>
	public static class PointerDirectionExtensions {
		/// <summary>
		/// Determines if pointer direction is vertical.
		/// </summary>
		/// <returns><c>true</c> if is vertical the specified dir; otherwise, <c>false</c>.</returns>
		/// <param name="dir">Dir.</param>
		public static bool IsVertical(this PointerDirection dir) {
			return dir==PointerDirection.Up || dir==PointerDirection.Down;
		}
		/// <summary>
		/// Determines if pointer direction is horizontal.
		/// </summary>
		/// <returns><c>true</c> if is horizontal the specified dir; otherwise, <c>false</c>.</returns>
		/// <param name="dir">Dir.</param>
		public static bool IsHorizontal(this PointerDirection dir) {
			return dir==PointerDirection.Left || dir==PointerDirection.Right;
		}

		/// <summary>
		/// Determines if a pointer is allowed to point left.
		/// </summary>
		/// <returns><c>true</c>, if allowed was lefted, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool LeftAllowed(this PointerDirection dir) {
			return (dir & PointerDirection.Left) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to point right.
		/// </summary>
		/// <returns><c>true</c>, if allowed was righted, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool RightAllowed(this PointerDirection dir) {
			return (dir & PointerDirection.Right) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to point up.
		/// </summary>
		/// <returns><c>true</c>, if allowed was uped, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool UpAllowed(this PointerDirection dir) {
			return (dir & PointerDirection.Up) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to down.
		/// </summary>
		/// <returns><c>true</c>, if allowed was downed, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool DownAllowed(this PointerDirection dir) {
			return (dir & PointerDirection.Down) != 0;
		}
	}
}


