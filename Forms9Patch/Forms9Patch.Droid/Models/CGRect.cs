using System;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Forms9Patch.Droid
{
	/// <summary>
	/// CG rect.
	/// </summary>
	public class CGRect : RectF
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public new float Width {
			get { return this.Width (); }
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public new float Height {
			get { return this.Height (); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGRect"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public CGRect(CGRect rect) : base (rect) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGRect"/> class.
		/// </summary>
		public CGRect() : base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGRect"/> class.
		/// </summary>
		/// <param name="left">Left.</param>
		/// <param name="top">Top.</param>
		/// <param name="right">Right.</param>
		/// <param name="bottom">Bottom.</param>
		public CGRect(float left, float top, float right, float bottom) : base (left, top, right, bottom) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGRect"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public CGRect(Rect rect) : base (rect) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGRect"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public CGRect(RectF rect) : base (rect) {
		}

		/// <summary>
		/// Tos the rect f.
		/// </summary>
		/// <returns>The rect f.</returns>
		public RectF ToRectF() {
			return this as RectF;
		}
	}
}

