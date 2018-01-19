using System;
using System.Drawing;
using System.Globalization;

namespace Forms9Patch.Droid
{
	//[Serializable]
	/// <summary>
	/// CG size.
	/// </summary>
	public struct CGSize : IEquatable<CGSize>
	{
		//
		// Static Fields
		//
		/// <summary>
		/// The empty.
		/// </summary>
		public static readonly CGSize Empty;

		//
		// Fields
		//
		private float width;

		private float height;

		//
		// Properties
		//
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		public float Height {
			get {
				return this.height;
			}
			set {
				this.height = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty {
			get {
				return this.width == 0 && this.height == 0;
			}
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		public float Width { get {
				return this.width;
			}
			set {
				this.width = value;
			}
		}

		//
		// Constructors
		//
		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGSize"/> struct.
		/// </summary>
		/// <param name="point">Point.</param>
		public CGSize (CGPoint point)
		{
			this.width = point.X;
			this.height = point.Y;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGSize"/> struct.
		/// </summary>
		/// <param name="size">Size.</param>
		public CGSize (CGSize size) {
			this.width = size.width;
			this.height = size.height;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGSize"/> struct.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public CGSize (double width, double height) {
			this.width = (float)width;
			this.height = (float)height;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.Droid.CGSize"/> struct.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public CGSize (float width, float height) {
			this.width = width;
			this.height = height;
		}

		//
		// Static Methods
		//
		/// <summary>
		/// Add the specified size1 and size2.
		/// </summary>
		/// <param name="size1">Size1.</param>
		/// <param name="size2">Size2.</param>
		public static CGSize Add (CGSize size1, CGSize size2)
		{
			return size1 + size2;
		}

		/// <summary>
		/// Subtract the specified size1 and size2.
		/// </summary>
		/// <param name="size1">Size1.</param>
		/// <param name="size2">Size2.</param>
		public static CGSize Subtract (CGSize size1, CGSize size2) {
			return size1 - size2;
		}

		/*
		public static bool TryParse (NSDictionary dictionaryRepresentation, out CGSize size)
		{
			if (dictionaryRepresentation == null) {
				size = CGSize.Empty;
				return false;
			}
			return NativeDrawingMethods.CGSizeMakeWithDictionaryRepresentation (dictionaryRepresentation.Handle, out size);
		}
		*/

		//
		// Methods
		//
		/// <summary>
		/// Determines whether the specified <see cref="Forms9Patch.Droid.CGSize"/> is equal to the current <see cref="Forms9Patch.Droid.CGSize"/>.
		/// </summary>
		/// <param name="size">The <see cref="Forms9Patch.Droid.CGSize"/> to compare with the current <see cref="Forms9Patch.Droid.CGSize"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Forms9Patch.Droid.CGSize"/> is equal to the current
		/// <see cref="Forms9Patch.Droid.CGSize"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (CGSize size) {
			return this == size;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Forms9Patch.Droid.CGSize"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Forms9Patch.Droid.CGSize"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Forms9Patch.Droid.CGSize"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj) {
			return obj is CGSize && this == (CGSize)obj;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Forms9Patch.Droid.CGSize"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode () {
			int num = 23;
			num = num * 31 + this.width.GetHashCode ();
			return num * 31 + this.height.GetHashCode ();
		}

		/// <summary>
		/// Tos the CG point.
		/// </summary>
		/// <returns>The CG point.</returns>
		public CGPoint ToCGPoint () {
			return (CGPoint)this;
		}

		/*
		public NSDictionary ToDictionary ()
		{
			return new NSDictionary (NativeDrawingMethods.CGSizeCreateDictionaryRepresentation (this));
		}
		*/

		/// <summary>
		/// Tos the point f.
		/// </summary>
		/// <returns>The point f.</returns>
		[Obsolete ("Use ToCGPoint instead")]
		public CGPoint ToPointF ()
		{
			return (CGPoint)this;
		}

		/// <summary>
		/// Tos the size of the rounded CG.
		/// </summary>
		/// <returns>The rounded CG size.</returns>
		public CGSize ToRoundedCGSize ()
		{
			return new CGSize ((float)Math.Round (this.width), (float)Math.Round (this.height));
		}

		/// <summary>
		/// Tos the size.
		/// </summary>
		/// <returns>The size.</returns>
		[Obsolete ("Use ToRoundedCGSize instead")]
		public CGSize ToSize ()
		{
			return this.ToRoundedCGSize ();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Droid.CGSize"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.Droid.CGSize"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{{Width={0}, Height={1}}}", this.width.ToString (CultureInfo.CurrentCulture), this.height.ToString (CultureInfo.CurrentCulture));
		}

		//
		// Operators
		//
		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static CGSize operator + (CGSize l, CGSize r) {
			return new CGSize (l.width + r.Width, l.height + r.Height);
		}

		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static bool operator == (CGSize l, CGSize r) {
			return l.width == r.width && l.height == r.height;
		}

		/// <param name="size">Size.</param>
		public static explicit operator CGPoint (CGSize size) {
			return new CGPoint (size.Width, size.Height);
		}

		/// <param name="size">Size.</param>
		public static explicit operator Size (CGSize size) {
			return new Size ((int)size.Width, (int)size.Height);
		}

		/// <param name="size">Size.</param>
		public static explicit operator SizeF (CGSize size) {
			return new SizeF ((float)size.Width, (float)size.Height);
		}

		/// <param name="size">Size.</param>
		public static implicit operator CGSize (SizeF size) {
			return new CGSize (size.Width, size.Height);
		}

		/// <param name="size">Size.</param>
		public static implicit operator CGSize (Size size) {
			return new CGSize (size.Width, size.Height);
		}

		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static bool operator != (CGSize l, CGSize r) {
			return l.width != r.width || l.height != r.height;
		}

		/// <param name="l">L.</param>
		/// <param name="r">The red component.</param>
		public static CGSize operator - (CGSize l, CGSize r) {
			return new CGSize (l.width - r.Width, l.height - r.Height);
		}
	}
}
