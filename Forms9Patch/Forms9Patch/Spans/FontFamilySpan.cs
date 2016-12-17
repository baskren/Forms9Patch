using System.Windows.Input;
using PCL.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Font family span.
	/// </summary>
	class FontFamilySpan : Span, ICopiable<FontFamilySpan>
	{
		internal const string SpanKey = "FontFamily";

		string _fontName;
		/// <summary>
		/// Gets or sets the name of the font family -OR- resource ID or embedded resource font.
		/// </summary>
		/// <value>The name of the font family.</value>
		public string FontFamilyName {
			get { return _fontName; }
			set { 
				if (_fontName == value)
					return;
				_fontName = value;
				OnPropertyChanged (Key);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.FontFamilySpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="fontName">Font name.</param>
		public FontFamilySpan(int start, int end, string fontName=null) : base (start, end) {
			Key = SpanKey;
			_fontName = fontName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.FontFamilySpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public FontFamilySpan(FontFamilySpan span) : this (span.Start, span.End, span.FontFamilyName) {
		}

		public void ValueFrom(FontFamilySpan source)
		{
			base.ValueFrom(source);
			FontFamilyName = source.FontFamilyName;
		}

		public override Span Copy()
		{
			return new FontFamilySpan(Start, End, FontFamilyName);
		}
	}
}

