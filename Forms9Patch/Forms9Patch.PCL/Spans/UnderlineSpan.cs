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

		/*
		Color _color;
		public Color Color {
			get { return _color; }
			set {
				if (_color == value)
					return;
				_color = value;
				OnPropertyChanged (Key);
			}
		}

		UnderlineStyle _style;
		public UnderlineStyle Style {
			get { return _style; }
			set {
				if (_style == value)
					return;
				_style = value;
				OnPropertyChanged (Key);
			}
		}
		*/	

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.UnderlineSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public UnderlineSpan (int start, int end/*, Color color=default(Color), UnderlineStyle style=UnderlineStyle.Single*/) : base (start, end) {
			//Color = color;
			//Style = style;
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.UnderlineSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public UnderlineSpan (UnderlineSpan span) : this (span.Start, span.End/*, span.Color, span.Style*/) {
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

