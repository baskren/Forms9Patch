using P42.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Bold span.
	/// </summary>
	class BoldSpan : Span, ICopiable<BoldSpan>
	{
		internal const string SpanKey = "Bold";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BoldSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public BoldSpan (int start, int end) : base (start, end) {
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BoldSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public BoldSpan (BoldSpan span) : this (span.Start, span.End) {
		}

		public void PropertiesFrom(BoldSpan source)
		{
			base.PropertiesFrom(source);
		}

		public override Span Copy()
		{
			return new BoldSpan(Start, End);
		}
	}
}

