using PCL.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Superscript span.
	/// </summary>
	class SuperscriptSpan : Span, ICopiable<SuperscriptSpan>
	{
		internal const string SpanKey = "Superscript";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SuperscriptSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public SuperscriptSpan (int start, int end) : base(start, end) {
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SuperscriptSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public SuperscriptSpan (SuperscriptSpan span) : this(span.Start, span.End) {
		}

		public void ValueFrom(SuperscriptSpan source)
		{
			base.ValueFrom(source);
		}

		public override Span Copy()
		{
			return new SuperscriptSpan(Start, End);
		}
	}
}

