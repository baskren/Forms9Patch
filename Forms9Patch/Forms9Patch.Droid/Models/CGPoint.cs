using System;
using System.Drawing;
using System.Globalization;

namespace Forms9Patch.Droid
{
	//[Serializable]
	/// <summary>
	/// CG point.
	/// </summary>
	public struct CGPoint : IEquatable<CGPoint>
	{
		//
		// Static Fields
		//
		/// <summary>
		/// The empty.
		/// </summary>
		public static readonly CGPoint Empty;

		//
		// Fields
		//
		private float x;

		private float y;

		//
		// Properties
		//
		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty {
			get {
				return this.x == 0 && this.y == 0;
			}
		}

		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>The x.</value>
		public float X {
			get {
				return this.x;
			}
			set {
				this.x = value;
			}
		}

		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>The y.</value>
		public float Y {
			get {
				return this.y;
			}
			set {
				this.y = value;
			}
		}

		//
		// Constructors
		//
		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGPoint"/> struct.
		/// </summary>
		/// <param name="point">Point.</param>
		public CGPoint (CGPoint point)
		{
			this.x = point.x;
			this.y = point.y;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGPoint"/> struct.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public CGPoint (double x, double y)
		{
			this.x = (float)x;
			this.y = (float)y;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGPoint"/> struct.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public CGPoint (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		//
		// Static Methods
		//
		/// <summary>
		/// Add the specified point and size.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="size">Size.</param>
		public static CGPoint Add (CGPoint point, CGSize size)
		{
			return point + size;
		}

		/// <summary>
		/// Subtract the specified point and size.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="size">Size.</param>
		public static CGPoint Subtract (CGPoint point, CGSize size)
		{
			return point - size;
		}

		/*
		public static bool TryParse (NSDictionary dictionaryRepresentation, out CGPoint point)
		{
			if (dictionaryRepresentation == null) {
				point = CGPoint.Empty;
				return false;
			}
			return NativeDrawingMethods.CGPointMakeWithDictionaryRepresentation (dictionaryRepresentation.Handle, out point);
		}
		*/

		//
		// Methods
		//
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Forms9Patch.Droid.CGPoint"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Forms9Patch.Droid.CGPoint"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Forms9Patch.Droid.CGPoint"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj) {
			return obj is CGPoint && this == (CGPoint)obj;
		}
		/// <summary>
		/// Determines whether the specified <see cref="Forms9Patch.Droid.CGPoint"/> is equal to the current <see cref="Forms9Patch.Droid.CGPoint"/>.
		/// </summary>
		/// <param name="point">The <see cref="Forms9Patch.Droid.CGPoint"/> to compare with the current <see cref="Forms9Patch.Droid.CGPoint"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Forms9Patch.Droid.CGPoint"/> is equal to the current
		/// <see cref="Forms9Patch.Droid.CGPoint"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (CGPoint point) {
			return this == point;
		}
		/// <summary>
		/// Serves as a hash function for a <see cref="Forms9Patch.Droid.CGPoint"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode () {
			int num = 23;
			num = num * 31 + this.x.GetHashCode ();
			return num * 31 + this.y.GetHashCode ();
		}

		/*
		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (NativeDrawingMethods.CGPointCreateDictionaryRepresentation (this));
		}
		*/
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Droid.CGPoint"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Droid.CGPoint"/>.</returns>
		public override string ToString () {
			return string.Format ("{{X={0}, Y={1}}}", this.x.ToString (CultureInfo.CurrentCulture), this.y.ToString (CultureInfo.CurrentCulture));
		}

		//
		// Operators
		//
		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static CGPoint operator + (CGPoint l, CGSize r) {
			return new CGPoint (l.x + r.Width, l.y + r.Height);
		}
		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static bool operator == (CGPoint l, CGPoint r) {
			return l.x == r.x && l.y == r.y;
		}
		/// <param name="point">Point.</param>
		public static explicit operator Point (CGPoint point) {
			return new Point ((int)point.X, (int)point.Y);
		}
		/// <param name="point">Point.</param>
		public static explicit operator PointF (CGPoint point) {
			return new PointF ((float)point.X, (float)point.Y);
		}
		/// <param name="point">Point.</param>
		public static implicit operator CGPoint (PointF point) {
			return new CGPoint (point.X, point.Y);
		}
		/// <param name="point">Point.</param>
		public static implicit operator CGPoint (Point point) {
			return new CGPoint (point.X, point.Y);
		}
		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static bool operator != (CGPoint l, CGPoint r) {
			return l.x != r.x || l.y != r.y;
		}

		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static CGPoint operator - (CGPoint l, CGSize r) {
			return new CGPoint (l.x - r.Width, l.y - r.Height);
		}
	}
}
