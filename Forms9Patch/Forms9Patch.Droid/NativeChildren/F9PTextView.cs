// /*******************************************************************
//  *
//  * AutofitTextView.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using Android.Content;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Android.Text;
using Xamarin.Forms;

namespace Forms9Patch.Droid
{
	public class F9PTextView : TextView
	{
		//Context _context;

		internal delegate bool BoolDelegate();

		internal static float DefaultTextSize=-1f;
		internal static float Scale = -1f;

		#region Constructors


		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public F9PTextView(Context context) : base(context)
		{
			init(context, null, 0);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		public F9PTextView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			init(context, attrs, 0);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		/// <param name="defStyle">Def style.</param>
		public F9PTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			init(context, attrs, defStyle);
		}

		void init(Context context, IAttributeSet attrs, int defStyle)
		{
			if (Scale <= 0)
				Scale = Forms.Context.Resources.DisplayMetrics.Density;
			if (DefaultTextSize <= 0)
				DefaultTextSize = (new TextView(context)).TextSize / Scale;
			//_context = context;
		}

		#endregion


		#region Properties
		/*
		float _minTextSize=4;
		internal float MinTextSize
		{
			get { return _minTextSize; }
			set
			{
				if (System.Math.Abs(value - _minTextSize) > float.Epsilon*5)
				{
					_minTextSize = value;
					if (_minTextSize < 0)
						_minTextSize = 4;
					//Autofit();
				}
			}
		}

		float _maxTextSize= 256;
		internal float MaxTextSize
		{
			get { return _maxTextSize; }
			set
			{
				if (System.Math.Abs(value - _maxTextSize) > float.Epsilon * 5)
				{
					_maxTextSize = value;
					if (_maxTextSize < 4)
						_maxTextSize = 256;
					//Autofit();
				}
			}
		}


		string _text;
		internal new string Text
		{
			get { return _text; }
			set
			{
				if (value != _text)
				{
					_text = value;
					if (_text != null)
					{
						_baseFormattedString = null;
						base.Text = _text;
					}
				}
			}
		}

		internal string BaseText
		{
			get { return base.Text; }
		}

		BaseFormattedString _baseFormattedString;
		internal BaseFormattedString BaseFormattedString
		{
			get { return _baseFormattedString; }
			set
			{
				if (value != _baseFormattedString)
				{
					_baseFormattedString = value;
					if (_baseFormattedString != null)
					{
						_text = null;
						TextFormatted = _baseFormattedString.ToSpannableString();
					}
				}
			}
		}
*/
		internal new float TextSize
		{
			get
			{
				return base.TextSize / Scale;
			}
			set
			{
				if (System.Math.Abs(base.TextSize - value) < float.Epsilon * 5)
					return;
				/*
				if (value > MaxTextSize)
					base.TextSize = MaxTextSize;
				else if (value < MinTextSize)
					base.TextSize = MinTextSize;
				else
				*/
					base.TextSize = value;

				//System.Diagnostics.Debug.WriteLine("\tTextSize=["+value+"] base.TextSize=["+TextSize+"]");
			}
		}



		static float Precision = 0.05f;

		/// <summary>
		/// Ons the text changed.
		/// </summary>
		/// <returns>The text changed.</returns>
		/// <param name="text">Text.</param>
		/// <param name="start">Start.</param>
		/// <param name="lengthBefore">Length before.</param>
		/// <param name="lengthAfter">Length after.</param>
		protected override void OnTextChanged(ICharSequence text, int start, int lengthBefore, int lengthAfter)
		{
			//if (_truncating)
			//	return;
			base.OnTextChanged(text, start, lengthBefore, lengthAfter);
		}
		#endregion

		/// <summary>
		/// Ons the size changed.
		/// </summary>
		/// <returns>The size changed.</returns>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		/// <param name="oldw">Oldw.</param>
		/// <param name="oldh">Oldh.</param>
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			//System.Diagnostics.Debug.WriteLine("\tF9PTextView.OnSizeChanged("+w+","+h+","+oldw+","+oldh+")");
			base.OnSizeChanged(w, h, oldw, oldh);
		}



		#region Truncation

		//bool _truncating;
		internal static StaticLayout Truncate(string text, BaseFormattedString baseFormattedString, TextPaint paint, int availWidth, int availHeight, LabelFit fit, LineBreakMode lineBreakMode, ref int lines, ref ICharSequence textFormatted)
		{
			StaticLayout layout=null;
			var fontMetrics = paint.GetFontMetrics();
			var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
			var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
			textFormatted = ((ICharSequence)baseFormattedString?.ToSpannableString()) ?? new String(text);
			if (lines > 0)
			{
				if (baseFormattedString != null)
				{
					layout = new StaticLayout(textFormatted, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
					if (layout.Height > availHeight)
					{
						var visibleLines = (int)((fontLeading + availHeight) / (fontLineHeight + fontLeading));
						if (visibleLines < lines)
							lines = visibleLines;
					}
					if (layout.LineCount > lines && lines > 0)
					{

						var secondToLastEnd = lines > 1 ? layout.GetLineEnd(lines - 2) : 0;
						var start = lines > 1 ? layout.GetLineStart(layout.LineCount - 2) : 0;
						switch (lineBreakMode)
						{
							case LineBreakMode.HeadTruncation:
								textFormatted = StartTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							case LineBreakMode.MiddleTruncation:
								textFormatted = MidTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, layout.GetLineStart(lines - 1), (layout.GetLineEnd(lines - 1) + layout.GetLineStart(lines - 1)) / 2 - 1, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							case LineBreakMode.TailTruncation:
								textFormatted = EndTruncatedFormatted(baseFormattedString, paint, secondToLastEnd, layout.GetLineStart(lines - 1), layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							default:
								textFormatted = baseFormattedString.ToSpannableString(EllipsePlacement.None, 0, 0, layout.GetLineEnd(lines - 1));
								break;
						}
					}
				}
				else
				{
					layout = new StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
					if (layout.Height > availHeight)
					{
						var visibleLines = (int)((fontLeading + availHeight) / (fontLineHeight + fontLeading));
						if (visibleLines < lines)
							lines = visibleLines;
					}
					if (layout.LineCount > lines && lines > 0)
					{
						var secondToLastEnd = lines > 1 ? layout.GetLineEnd(lines - 2): 0;
						var start = lines > 1 ? layout.GetLineStart(layout.LineCount - 2) : 0;
						switch (lineBreakMode)
						{
							case LineBreakMode.HeadTruncation:
								text = StartTruncatedLastLine(text, paint, secondToLastEnd, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							case LineBreakMode.MiddleTruncation:
								text = MidTruncatedLastLine(text, paint, secondToLastEnd, layout.GetLineStart(lines - 1), (layout.GetLineEnd(lines - 1) + layout.GetLineStart(lines - 1)) / 2 - 1, start, layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							case LineBreakMode.TailTruncation:
								text = EndTruncatedLastLine(text, paint, secondToLastEnd, layout.GetLineStart(lines - 1), layout.GetLineEnd(layout.LineCount - 1), availWidth);
								break;
							default:
								text = text.Substring(0, layout.GetLineEnd(lines-1));
								break;
						}
						textFormatted = new String(text);
					}
				}
			}
			return new StaticLayout(textFormatted, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
		}

		static ICharSequence StartTruncatedFormatted(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
		{
			return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, end, end, availWidth);
		}

		static ICharSequence StartTruncatedFormattedIter(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLow, int startHigh, int end, float availWidth)
		{
			if (startHigh-startLow<=1)
				return baseFormattedString.ToSpannableString(EllipsePlacement.Start, secondToLastEnd, startHigh, end);
			int mid = (startLow + startHigh) / 2;
			SpannableStringBuilder formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.Start, 0, mid, end);
			var layout = new StaticLayout(formattedText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, mid, startHigh, end, availWidth);
			return StartTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLow, mid, end, availWidth);
		}

		static ICharSequence MidTruncatedFormatted(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int start, int end, float availWidth)
		{
			return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, start, end, end, availWidth);
		}

		static ICharSequence MidTruncatedFormattedIter(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int startLow, int startHigh, int end, float availWidth)
		{
			if (startHigh - startLow <= 1)
				return baseFormattedString.ToSpannableString(EllipsePlacement.Mid, secondToLastEnd, startHigh, end, startLastVisible, midLastVisible);
			int mid = (startLow + startHigh) / 2;
			SpannableStringBuilder formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.Mid, 0, mid, end, startLastVisible, midLastVisible);
			var layout = new StaticLayout(formattedText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, mid, startHigh, end, availWidth);
			return MidTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, startLastVisible, midLastVisible, startLow, mid, end, availWidth);
		}

		static ICharSequence EndTruncatedFormatted(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
		{
			return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, start, end, availWidth);
		}

		static ICharSequence EndTruncatedFormattedIter(BaseFormattedString baseFormattedString, TextPaint paint, int secondToLastEnd, int start, int endLow, int endHigh, float availWidth)
		{
			if (endHigh - endLow <= 1)
			{
				return baseFormattedString.ToSpannableString(EllipsePlacement.End, secondToLastEnd, start, endLow);
			}
			int mid = (endLow + endHigh) / 2;
			SpannableStringBuilder formattedText = baseFormattedString.ToSpannableString(EllipsePlacement.End, 0, start, mid);
			var layout = new StaticLayout(formattedText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, endLow, mid, availWidth);
			return EndTruncatedFormattedIter(baseFormattedString, paint, secondToLastEnd, start, mid, endHigh, availWidth);
		}



		static string StartTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
		{
			return StartTruncatedIter(text, paint, secondToLastEnd, start, end, end, availWidth);
		}

		//Android.Graphics.Rect _truncBounds = new Android.Graphics.Rect();
		static string StartTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int startLow, int startHigh, int end, float availWidth)
		{
			if (startHigh-startLow<=1)
				return  (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + "…" + text.Substring(startHigh, end - startHigh).TrimStart();
			int mid = (startLow + startHigh) / 2;
			var lastLineText = new String("…" + text.Substring(mid, end - mid).TrimStart());
			//var truncBounds = new Android.Graphics.Rect();
			//paint.GetTextBounds(lastLineText, 0, lastLineText.Length, truncBounds);
			//if (truncBounds.Width() > availWidth)
			var layout = new StaticLayout(lastLineText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return StartTruncatedIter(text, paint, secondToLastEnd, mid, startHigh, end, availWidth);
			return StartTruncatedIter(text, paint, secondToLastEnd, startLow, mid, end, availWidth);
		}

		static string MidTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int start, int end, float availWidth)
		{
			return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, start, end, end, availWidth);
		}

		static string MidTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int startLastVisible, int midLastVisible, int startLow, int startHigh, int end, float availWidth)
		{
			if (startHigh - startLow <= 1)
				return (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + text.Substring(startLastVisible, midLastVisible - startLastVisible).TrimEnd() + "…" + text.Substring(startHigh, end - startHigh).TrimStart();
			int mid = (startLow + startHigh) / 2;
			var lastLineText = new String(text.Substring(startLastVisible, midLastVisible - startLastVisible).TrimEnd() + "…" + text.Substring(mid, end - mid).TrimStart());
			//var truncBounds = new Android.Graphics.Rect();
			//paint.GetTextBounds(lastLineText, 0, lastLineText.Length, truncBounds);
			//if (truncBounds.Width() > availWidth)
			var layout = new StaticLayout(lastLineText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, mid, startHigh, end, availWidth);
			return MidTruncatedIter(text, paint, secondToLastEnd, startLastVisible, midLastVisible, startLow, mid, end, availWidth);
		}

		static string EndTruncatedLastLine(string text, TextPaint paint, int secondToLastEnd, int start, int end, float availWidth)
		{
			return EndTruncatedIter(text, paint, secondToLastEnd, start, start, end, availWidth);
		}

		static string EndTruncatedIter(string text, TextPaint paint, int secondToLastEnd, int start, int endLow, int endHigh, float availWidth)
		{
			if (endHigh - endLow <= 1)
				return (secondToLastEnd > 0 ? text.Substring(0, secondToLastEnd).TrimEnd() + "\n" : "") + text.Substring(start, endLow - start) + "…";
			int mid = (endLow + endHigh) / 2;
			var lastLineText = new String(text.Substring(start, mid - start) + "…");
			//var truncBounds = new Android.Graphics.Rect();
			//paint.GetTextBounds(lastLineText, 0, lastLineText.Length, truncBounds);
			//if (truncBounds.Width() > availWidth-Scale)
			var layout = new StaticLayout(lastLineText, paint, int.MaxValue, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			if (layout.GetLineWidth(0) > availWidth)
				return EndTruncatedIter(text, paint, secondToLastEnd, start, endLow, mid, availWidth);
			return EndTruncatedIter(text, paint, secondToLastEnd, start, mid, endHigh, availWidth);
		}

		#endregion



		#region Fitting
		internal static float WidthFit(ICharSequence text, TextPaint paint, int lines, float min, float max, int availWidth, int availHeight)
		{
			//System.Diagnostics.Debug.WriteLine("F9PTextView.WidthFit: availWidth=["+availWidth+"] availHeight=["+availHeight+"]");

			if (availWidth == int.MaxValue)
			{
				if (availHeight == int.MaxValue)
					return max;
				var fontMetrics = paint.GetFontMetrics();
				var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
				var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
				var fontPixelSize = paint.TextSize;
				var lineHeightRatio = fontLineHeight / fontPixelSize;
				var leadingRatio = fontLeading / fontPixelSize;
				float ans = ((availHeight / (lines + leadingRatio * (lines - 1))) / lineHeightRatio - 0.1f);
				if (ans > max)
					ans = max;
				return ans;
			}
			//if (availHeight == int.MaxValue)
			//	return max;

			/*
			bool twice = (text.Length() == 1);

			if (twice)
			{
				text = new Java.Lang.String(new char[] { text.CharAt(0), text.CharAt(0)});
				availWidth *= 2;
			}
			*/

			float result = ZeroLinesFit(text, paint, min, max, availWidth, availHeight);

			float step = (result - min) / 5;
			if (step > 0.05f)
			{
				result = DescendingWidthFit(text, paint, lines, min, result, availWidth, availHeight, step);
				while (step > 0.25f)
				{
					step /= 5;
					result = DescendingWidthFit(text, paint, lines, result, result + step * 5, availWidth, availHeight, step);
				}
			}
			return result;
		}

		static float DescendingWidthFit(ICharSequence text, TextPaint paint, int lines, float min, float max, int availWidth, int availHeight, float step)
		{
			float result;
			for (result = max; result > min; result -= step)
			{
				paint.TextSize = result * Scale;
				var layout = new StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
				if (layout.LineCount <= lines)
					return result;
			}
			return result;
		}

		internal static float ZeroLinesFit(ICharSequence text, TextPaint paint, float min, float max, int availWidth, int availHeight)
		{
			if (availHeight == int.MaxValue)
				return max;
			if (availWidth == int.MaxValue)
			{
				var fontMetrics = paint.GetFontMetrics();
				var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
				//var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
				var fontPixelSize = paint.TextSize;
				var lineHeightRatio = fontLineHeight / fontPixelSize;
				//var leadingRatio = fontLeading / fontPixelSize;

				return (availHeight / lineHeightRatio - 0.1f);
			}

			if (max - min < Precision)
				return min;

			float mid = (max + min) / 2.0f;
			paint.TextSize = mid * Scale;
			var layout = new StaticLayout(text, paint, availWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
			var lineCount = layout.LineCount;
			var height = layout.Height - layout.BottomPadding + layout.TopPadding;
			if (height > availHeight)
				return ZeroLinesFit(text, paint, min, mid, availWidth, availHeight);
			if (height < availHeight)
				return ZeroLinesFit(text, paint, mid, max, availWidth, availHeight);
			float maxLineWidth = 0;
			for (int i = 0; i < lineCount; i++)
				if (layout.GetLineWidth(i) > maxLineWidth)
					maxLineWidth = layout.GetLineWidth(i);
			if (maxLineWidth > availWidth)
				return ZeroLinesFit(text, paint, min, mid, availWidth, availHeight);
			if (maxLineWidth < availWidth)
				return ZeroLinesFit(text, paint, mid, max, availWidth, availHeight);
			return mid;
		}
		#endregion



		#region Invalidation Skip
		//bool _skip;

		/// <summary>
		/// Invalidate this instance.
		/// </summary>
		public override void Invalidate()
		{
		//	if (!_skip)
				base.Invalidate();
		//	_skip = false;
		}

		internal void SkipNextInvalidate()
		{
		//	_skip = true;
		}

		#endregion
	}
}

