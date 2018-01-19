using System;
using Xamarin.Forms;
using P42.Utils;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Background color span.
	/// </summary>
	class BackgroundColorSpan : Span, ICopiable<BackgroundColorSpan>
	{
		internal const string SpanKey = "BackgroundColor";

		Color _color;
		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color {
			get { return _color; }
			set { 
				_color = value; 
				OnPropertyChanged (Key);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BackgroundColorSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="color">Color.</param>
		public BackgroundColorSpan (int start, int end, Color color) : base (start, end)
		{
			Key = SpanKey;
			_color = color;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.BackgroundColorSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public BackgroundColorSpan (BackgroundColorSpan span) : this (span.Start, span.End, span.Color) {
		}

		public void PropertiesFrom(BackgroundColorSpan source)
		{
			base.PropertiesFrom(source);
			Color = source.Color;
		}

		public override Span Copy()
		{
			return new BackgroundColorSpan(Start, End, Color);
		}

	}
}

