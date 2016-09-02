using System;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Forms9Patch.Droid
{
	/// <summary>
	/// Path extensions.
	/// </summary>
	public static class PathExtensions
	{
		/// <summary>
		/// Adds the relative arc.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="startRadians">Start radians.</param>
		/// <param name="sweepRadians">Sweep radians.</param>
		public static void AddRelativeArc(this Path path, float x, float y, float radius, float startRadians, float sweepRadians) {
			var rect = new RectF (x - radius, y - radius, x + radius, y + radius);
			float startDegrees = (float)(startRadians * 180.0 / Math.PI);
			float sweepDegrees = (float)(sweepRadians * 180.0 / Math.PI);
			//path.ArcTo (x - radius, y - radius, x + radius, y + radius, startDegrees, sweepDegrees, true);
			path.ArcTo(rect, startDegrees,sweepDegrees);
		}

		/// <summary>
		/// Moves to point.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static void MoveToPoint(this Path path, float x, float y) {
			path.MoveTo (x, y);
		}

		/// <summary>
		/// Adds the line to point.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static void AddLineToPoint(this Path path, float x, float y) {
			path.LineTo (x, y);
		}

		/// <summary>
		/// Adds the curve to point.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="p3">P3.</param>
		public static void AddCurveToPoint(this Path path, CGPoint p1, CGPoint p2, CGPoint p3) {
			path.CubicTo (p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y);
		}
	}
}

