using PCL.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Subscript span.
	/// </summary>
	class SubscriptSpan : Span, ICopiable<SubscriptSpan>
	{
		internal const string SpanKey = "Subscript";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SubscriptSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public SubscriptSpan (int start, int end) : base (start, end) {
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SubscriptSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public SubscriptSpan (SubscriptSpan span) : this (span.Start, span.End) {
		}

		public void PropertiesFrom(SubscriptSpan source)
		{
			base.PropertiesFrom(source);
		}

		public override Span Copy()
		{
			return new SubscriptSpan(Start, End);
		}
	}
}

