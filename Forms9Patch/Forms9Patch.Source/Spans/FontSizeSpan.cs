using System;
using P42.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Font size span.
	/// </summary>
	class FontSizeSpan : Span, ICopiable<FontSizeSpan>
	{
		internal const string SpanKey = "Size";

		float _size=-1;
		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>The size.</value>
		public float Size {
			get { return _size; }
			set {
				if (Math.Abs (_size - value) < 0.01)
					return;
				_size = value;
				OnPropertyChanged (Key);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.FontSizeSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="size">Size.</param>
		public FontSizeSpan(int start, int end, float size) : base (start, end) {
			Key = SpanKey;
			_size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.FontSizeSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public FontSizeSpan(FontSizeSpan span) : this(span.Start, span.End, span.Size) {
		}

		public void PropertiesFrom(FontSizeSpan source)
		{
			base.PropertiesFrom(source);
			Size = source.Size;
		}

		public override Span Copy()
		{
			return new FontSizeSpan(Start, End, Size);
		}
	}
}

