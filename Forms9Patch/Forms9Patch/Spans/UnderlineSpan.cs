using Xamarin.Forms;
using P42.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Underline span.
	/// </summary>
	class UnderlineSpan : Span, ICopiable<UnderlineSpan>
	{
		internal const string SpanKey = "Underline";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.UnderlineSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public UnderlineSpan (int start, int end) : base (start, end) {
			//Color = color;
			//Style = style;
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.UnderlineSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public UnderlineSpan (UnderlineSpan span) : this (span.Start, span.End) {
		}

		public void PropertiesFrom(UnderlineSpan source)
		{
			base.PropertiesFrom(source);
		}

		public override Span Copy()
		{
			return new UnderlineSpan(Start, End);
		}
	}
}

