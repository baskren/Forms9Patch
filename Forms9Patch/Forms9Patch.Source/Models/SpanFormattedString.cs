using System.Collections.ObjectModel;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Span formatted string.
	/// </summary>
	class SpanFormattedString : F9PFormattedString
	{
		/// <summary>
		/// Gets the collection of spans.
		/// </summary>
		/// <value>The spans.</value>
		public ObservableCollection<Span> Spans {
			get { return _spans; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SpanFormattedString"/> class.
		/// </summary>
		public SpanFormattedString () : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.SpanFormattedString"/> class.
		/// </summary>
		/// <param name="s">S.</param>
		public SpanFormattedString (string s) : base (s) {
		}

		/// <param name="text">Text.</param>
		public static implicit operator SpanFormattedString (string text) {
			return new SpanFormattedString (text);
		}

	}
}

