using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using Android.Views;
using Android.Util;
using System;
using FormsGestures.Droid;
using Java.Interop;
using System.Runtime.InteropServices;
using Android.Drm;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.Droid.LabelRenderer))]
namespace Forms9Patch.Droid
{
	/// <summary>
	/// The Forms9Patch Label renderer.
	/// </summary>
	public class LabelRenderer : ViewRenderer<Label, F9PTextView>
	{
		ColorStateList _labelTextColorDefault;

		SizeRequest? _lastSizeRequest;
		ControlState _lastControlState;

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelRenderer"/> class.
		/// </summary>
		public LabelRenderer()
		{
			AutoPackage = false;
		}

		bool showDebugMsg
		{
			get
			{
				return false;
			}
		}

		string elementText
		{
			get
			{
				return (Element.HtmlText ?? Element.Text ?? "");
			}
		}

		Xamarin.Forms.Size LabelXamarinSize(double widthConstraint, double fontSize)
		{
			var layout = LabelLayout((widthConstraint > double.MaxValue / 3 ? int.MaxValue / 2 : (int)widthConstraint), (float)fontSize);
			if (layout == null)
				return Xamarin.Forms.Size.Zero;
			float width = 0;
			float height = 0;
			for (int i = 0; i < layout.LineCount; i++)
			{
				var lineWidth = layout.GetLineWidth(i);
				if (width < lineWidth)
					width = lineWidth;
				var blockHeight = layout.GetLineBottom(i);
				if (height < blockHeight)
					height = blockHeight;
			}

			return new Xamarin.Forms.Size(width, height);
		}

		StaticLayout LabelLayout(int widthConstraint, float fontSize)
		{
			if (Element == null)
				return null;
			if (_currentControlState.JavaText == null)
				return null;
			var paint = new TextPaint(Control.Paint);
			paint.TextSize = fontSize;
			return new StaticLayout(_currentControlState.JavaText, paint, widthConstraint, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
		}

		void LayoutForSize(int width, int height)
		{
			var widthConstraint = MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.AtMost);
			var heightConstraint = MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost);
			GetDesiredSize(widthConstraint, heightConstraint);
		}

		/// <summary>
		/// Gets the size of the desired.
		/// </summary>
		/// <returns>The desired size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (_currentControlState.IsNullOrEmpty || Control == null)
				return new SizeRequest(Xamarin.Forms.Size.Zero);


			_currentControlState.AvailWidth = MeasureSpec.GetSize(widthConstraint);
			if (MeasureSpec.GetMode(widthConstraint) == Android.Views.MeasureSpecMode.Unspecified)
				_currentControlState.AvailWidth = int.MaxValue / 2;
			_currentControlState.AvailHeight = MeasureSpec.GetSize(heightConstraint);
			if (MeasureSpec.GetMode(heightConstraint) == Android.Views.MeasureSpecMode.Unspecified)
				_currentControlState.AvailHeight = int.MaxValue / 2;
			if (_currentControlState.AvailWidth <= 0 || _currentControlState.AvailHeight <= 0)
				return new SizeRequest(Xamarin.Forms.Size.Zero);
			
			if (_currentControlState == _lastControlState && _lastSizeRequest.HasValue)
				return _lastSizeRequest.Value;

			ICharSequence text = _currentControlState.JavaText;
			var tmpFontSize = ModelFontSize;
			Control.TextSize = tmpFontSize;
			Control.SetSingleLine(false);
			Control.SetMaxLines(int.MaxValue / 2);
			Control.SetIncludeFontPadding(false);
			Control.Ellipsize = null;

			int tmpHt = -1;
			int tmpWd = -1;

			var fontMetrics = Control.Paint.GetFontMetrics();
			var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
			var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);

			if (_currentControlState.Lines == 0 && _currentControlState.Fit != LabelFit.None)
				tmpFontSize = F9PTextView.ZeroLinesFit(_currentControlState.JavaText, new TextPaint(Control.Paint), ModelMinFontSize, tmpFontSize, _currentControlState.AvailWidth, _currentControlState.AvailHeight);
			else if (_currentControlState.Fit == LabelFit.Lines)
			{

				if (_currentControlState.AvailHeight > int.MaxValue / 3)
					tmpHt = (int)System.Math.Round(_currentControlState.Lines * fontLineHeight + (_currentControlState.Lines - 1) * fontLeading);
				else
				{
					var fontPointSize = tmpFontSize;
					var lineHeightRatio = fontLineHeight / fontPointSize;
					var leadingRatio = fontLeading / fontPointSize;
					tmpFontSize = ((_currentControlState.AvailHeight / (_currentControlState.Lines + leadingRatio * (_currentControlState.Lines - 1))) / lineHeightRatio - 0.1f);
				}
			}
			else if (_currentControlState.Fit == LabelFit.Width)
				tmpFontSize = F9PTextView.WidthFit(_currentControlState.JavaText, new TextPaint(Control.Paint), _currentControlState.Lines, ModelMinFontSize, tmpFontSize, _currentControlState.AvailWidth, _currentControlState.AvailHeight);

			Control.TextSize = BoundTextSize(tmpFontSize);;
			var layout = new StaticLayout(_currentControlState.JavaText, new TextPaint(Control.Paint), _currentControlState.AvailWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);

			int lines = _currentControlState.Lines;
			if (lines == 0 && _currentControlState.Fit == LabelFit.None)
			{
				for (int i = 0; i < layout.LineCount; i++)
				{
					if (layout.GetLineBottom(i) <= _currentControlState.AvailHeight - layout.TopPadding - layout.BottomPadding)
						lines++;
					else
						break;
				}
			}
			if (layout.Height > _currentControlState.AvailHeight || (lines > 0 && layout.LineCount > lines))
			{
				if (_currentControlState.Lines == 1)
				{
					Control.SetSingleLine(true);
					Control.SetMaxLines(1);
					Control.Ellipsize = _currentControlState.LineBreakMode.ToEllipsize();
				}
				else
					layout = F9PTextView.Truncate(_currentControlState.Text, Element.F9PFormattedString, new TextPaint(Control.Paint), _currentControlState.AvailWidth, _currentControlState.AvailHeight, Element.Fit, Element.LineBreakMode, ref lines, ref text);
			}
			lines = lines > 0 ? System.Math.Min(lines, layout.LineCount) : layout.LineCount;
			for (int i = 0; i < lines; i++)
			{
				tmpHt = layout.GetLineBottom(i);
				var width = layout.GetLineWidth(i);
				//System.Diagnostics.Debug.WriteLine("\t\tright=["+right+"]");
				if (width > tmpWd)
					tmpWd = (int)System.Math.Ceiling(width);
			}
			if (_currentControlState.Fit == LabelFit.None && _currentControlState.Lines > 0)
				Control.SetMaxLines(_currentControlState.Lines);

			//System.Diagnostics.Debug.WriteLine("\tLabelRenderer.GetDesiredSize\ttmp.size=[" + tmpWd + ", " + tmpHt + "]");
			if (Element.IsDynamicallySized && _currentControlState.Lines > 0 && _currentControlState.Fit == LabelFit.Lines)
			{
				fontMetrics = Control.Paint.GetFontMetrics();
				fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
				fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
				tmpHt = (int)(fontLineHeight * _currentControlState.Lines + fontLeading * (_currentControlState.Lines - 1));
			}

			Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();

			if (Element.Text != null)
				Control.Text = text.ToString();
			else
				Control.TextFormatted = text;

			_lastSizeRequest = new SizeRequest(new Xamarin.Forms.Size(tmpWd, tmpHt), new Xamarin.Forms.Size(10, tmpHt));
			if (!_delayingActualFontSizeUpdate)
			{
				_delayingActualFontSizeUpdate = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
				{
					if (Element != null && Control != null)
						Element.ActualFontSize = Control.TextSize;
					_delayingActualFontSizeUpdate = false;
					return false;
				});
			}
			if (showDebugMsg)
			{
				Control.SetWidth((int)_lastSizeRequest.Value.Request.Width);
				Control.SetHeight((int)_lastSizeRequest.Value.Request.Height);

				System.Diagnostics.Debug.WriteLine("\t[" + elementText + "] LabelRenderer.GetDesiredSize(" + (_currentControlState.AvailWidth > int.MaxValue / 3 ? "infinity" : _currentControlState.AvailWidth.ToString()) + "," + (_currentControlState.AvailHeight > int.MaxValue / 3 ? "infinity" : _currentControlState.AvailHeight.ToString()) + ") exit (" + _lastSizeRequest.Value + ")");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Visibility=[" + Control.Visibility + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.TextFormatted=[" + Control.TextFormatted + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.TextSize=[" + Control.TextSize + "]");
				//System.Diagnostics.Debug.WriteLine("\t\tControl.ClipBounds=["+Control.ClipBounds.Width()+","+Control.ClipBounds.Height()+"]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Width[" + Control.Width + "]  .Height=[" + Control.Height + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.GetX[" + Control.GetX() + "]  .GetY[" + Control.GetY() + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Alpha[" + Control.Alpha + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Background[" + Control.Background + "]");
				//System.Diagnostics.Debug.WriteLine("\t\tControl.Elevation["+Control.Elevation+"]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Enabled[" + Control.Enabled + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.Error[" + Control.Error + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.IsOpaque[" + Control.IsOpaque + "]");
				System.Diagnostics.Debug.WriteLine("\t\tControl.IsShown[" + Control.IsShown + "]");
				//Control.BringToFront();
				System.Diagnostics.Debug.WriteLine("\t\t");

			}

			_lastControlState = new ControlState(_currentControlState);
			return _lastSizeRequest.Value;
		}

		bool _delayingActualFontSizeUpdate;


		ControlState _currentControlState;
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				var view = new F9PTextView(Context);
				_labelTextColorDefault = view.TextColors;
				SetNativeControl(view);
			}

			if (e.OldElement != null)
			{
				e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
				e.OldElement.RendererSizeForWidthAndFontSize -= LabelXamarinSize;
			}

			if (e.NewElement != null)
			{
				e.NewElement.RendererIndexAtPoint += IndexAtPoint;
				e.NewElement.RendererSizeForWidthAndFontSize += LabelXamarinSize;
				_currentControlState = new ControlState
				{
					Lines = Element.Lines,
					Fit = Element.Fit,
					LineBreakMode = Element.LineBreakMode,
				};
				_lastControlState = null;
				Control.SetTextColor(_labelTextColorDefault);
				Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
				UpdateText();
				UpdateColor();
				UpdateFont();
				//Control.SkipNextInvalidate();
			}
		}



		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
				Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
			else if (e.PropertyName == Label.TextColorProperty.PropertyName)
				UpdateColor();
			else if (e.PropertyName == Label.FontProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
				UpdateLineBreakMode();
			else if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.HtmlTextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.FitProperty.PropertyName)
				UpdateFit();
			else if (e.PropertyName == Label.LinesProperty.PropertyName)
				UpdateLines();
			else if (e.PropertyName == Label.MinFontSizeProperty.PropertyName)
				UpdateMinFontSize();
			else if (e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName)
			{
				/*
				//TODO: EVALUATE THE NECESSITY AND EFFICACY OF THIS BLOCK
				if (Element.Width > -1 && Element.Height > -1 && Element.IsVisible)
					if (Element.Width != _lastControlState.AvailWidth || Element.Height != _lastControlState.AvailHeight)
					{
						_lastSizeRequest = null;
						LayoutForSize((int)(Element.Width * Forms9Patch.Display.Scale), (int)(Element.Height * Forms9Patch.Display.Scale));
					}
				*/
				Layout();
			}
		}

		void Layout()
		{
			if (Element.Width > -1 && Element.Height > -1 && Element.IsVisible)
				if (_lastControlState==null || Element.Width != _lastControlState.AvailWidth || Element.Height != _lastControlState.AvailHeight)
			LayoutForSize((int)(Element.Width * Forms9Patch.Display.Scale), (int)(Element.Height * Forms9Patch.Display.Scale));
		}

		void UpdateLineBreakMode()
		{
			_currentControlState.LineBreakMode = Element.LineBreakMode;
			Layout();
		}

		void UpdateMinFontSize()
		{
			_currentControlState.TextSize = ModelFontSize;
			Layout();
		}

		void UpdateFit()
		{
			_currentControlState.Fit = Element.Fit;
			Layout();
		}

		void UpdateLines()
		{
			_currentControlState.Lines = Element.Lines;
			Layout();
		}

		void UpdateColor()
		{
			if (_currentControlState.TextColor == Element.TextColor)
				return;
			_currentControlState.TextColor = Element.TextColor;
			if (_currentControlState.TextColor == Xamarin.Forms.Color.Default)
				Control.SetTextColor(_labelTextColorDefault);
			else
				Control.SetTextColor(_currentControlState.TextColor.ToAndroid());
		}

		void UpdateFont()
		{
			_currentControlState.Typeface = FontManagment.TypefaceForFontFamily(Element.FontFamily) ?? Element.Font.ToTypeface();
			if (_currentControlState.Typeface == Control.Typeface)
				return;
			Control.Typeface = _currentControlState.Typeface;
			Layout();
		}

		#region FontSize helpers
		float ModelFontSize
		{
			get
			{
				var textSize = (float)Element.Font.FontSize;
				if (System.Math.Abs(textSize) < 0.0001)
					textSize = F9PTextView.DefaultTextSize;
				return BoundTextSize(textSize);
			}
		}

		float BoundTextSize(float textSize)
		{
			if (textSize < 0)
				textSize = (float)(F9PTextView.DefaultTextSize * System.Math.Abs(Element.Font.FontSize));
			if (textSize < ModelMinFontSize)
				textSize = ModelMinFontSize;
			return textSize;
		}

		float ModelMinFontSize
		{
			get
			{
				var minFontSize = (float)Element.MinFontSize;
				if (minFontSize < 0)
					minFontSize = 4;
				return minFontSize;
			}
		}

		#endregion


		void UpdateText()
		{
			if (Element.F9PFormattedString != null)
			{
				if (Settings.IsLicenseValid || Element._id < 4)
					_currentControlState.TextFormatted = Element.F9PFormattedString.ToSpannableString();
				else
					_currentControlState.Text = "UNLICENSED COPY";
			}
			else
				_currentControlState.Text = Element.Text;

			UpdateColor();
			//UpdateFont();
			Layout();
		}

		int IndexAtPoint(Xamarin.Forms.Point p)
		{
			return Control.IndexForPoint(p.ToNativePoint());
		}
	}

	class ControlState
	{
		ICharSequence _textFormatted;
		public ICharSequence TextFormatted
		{
			get
			{
				return _textFormatted;
			}
			set
			{
				if (value != _textFormatted)
				{
					_textFormatted = value;
					_text = null;
					_javaText = _textFormatted;
				}
			}
		}
		string _text;
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (value != _text)
				{
					_text = value;
					_textFormatted = null;
					if (value != null)
						_javaText = new Java.Lang.String(_text);
					else
						_javaText = null;
				}
			}
		}
		ICharSequence _javaText;
		public ICharSequence JavaText
		{
			get
			{
				return _javaText;
			}
		}


		public Typeface Typeface;

		public Xamarin.Forms.Color TextColor = Xamarin.Forms.Color.Default;

		public int AvailWidth;
		public int AvailHeight;

		public float TextSize;
		public int Lines = (int)Label.LinesProperty.DefaultValue;
		public LabelFit Fit = (LabelFit)Label.FitProperty.DefaultValue;
		public LineBreakMode LineBreakMode = (LineBreakMode)Label.LineBreakModeProperty.DefaultValue;

		public bool IsNullOrEmpty
		{
			get
			{
				return (_textFormatted == null || _textFormatted.Length() == 0) && string.IsNullOrEmpty(_text);
			}
		}

		public ControlState() { }

		public ControlState(ControlState source)
		{
			_textFormatted = source._textFormatted;
			_text = source._text;
			_javaText = source._javaText;
			Typeface = source.Typeface;
			TextColor = source.TextColor;
			AvailWidth = source.AvailWidth;
			AvailHeight = source.AvailHeight;
			TextSize = source.TextSize;
			Lines = source.Lines;
			Fit = source.Fit;
			LineBreakMode = source.LineBreakMode;
		}


		public override bool Equals(object obj)
		{
			var other = obj as ControlState;
			if (other == null)
				return false;
			return this == other;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(ControlState a, ControlState b)
		{
			if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
				return true;
			if (ReferenceEquals(a,null) || ReferenceEquals(b, null))
				return false;
			if (a.AvailWidth != b.AvailWidth)
				return false;
			if (a.AvailHeight != b.AvailHeight)
				return false;
			if (System.Math.Abs(a.TextSize-b.TextSize)>0.1)
				return false;
			if (a.Lines != b.Lines)
				return false;
			if (a.Fit != b.Fit)
				return false;
			if (a.LineBreakMode != b.LineBreakMode)
				return false;
			if (a._javaText != b._javaText)
				return false;
			if (a.Typeface != b.Typeface)
				return false;
			
			return true;
		}

		public static bool operator !=(ControlState a, ControlState b)
		{
			return !(a == b);
		}
	}
}