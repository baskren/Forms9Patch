using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using CoreGraphics;
using System.ComponentModel;
using Foundation;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.iOS.LabelRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// Forms9Patch iOS Label renderer.
	/// </summary>
	public class LabelRenderer : ViewRenderer<Label, UILabel>
	{
		//
		// Fields
		//
		bool perfectSizeValid;

		SizeRequest perfectSize;

		#region Xamarin layout cycle
		/// <summary>
		/// Gets the size of the desired.
		/// </summary>
		/// <returns>The desired size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			//System.Diagnostics.Debug.WriteLine("GetDesiredSize(" + widthConstraint + "," + heightConstraint + ") enter");

			if (Control == null)
				return new SizeRequest(Size.Zero);

			string text = Element.Text ?? Element.FormattedText?.Text;
			if (string.IsNullOrEmpty(text))
				return new SizeRequest(Size.Zero);

			if (Control.LineBreakMode != UILineBreakMode.WordWrap)
				 Control.LineBreakMode = UILineBreakMode.WordWrap;


			if (!perfectSizeValid)
			{
				perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
				perfectSize.Minimum = new Size(Math.Min(10, perfectSize.Request.Width), perfectSize.Request.Height);
				perfectSizeValid = true;
			}
			if (widthConstraint >= perfectSize.Request.Width && heightConstraint >= perfectSize.Request.Height && Element.Fit != LabelFit.Lines)
			{
				//System.Diagnostics.Debug.WriteLine("\tperfectSize=[" + perfectSize.Request.Width + "," + perfectSize.Request.Height + "]");
				return perfectSize;
			}


			var tmpFontSize = Element.FontSize;
			if (tmpFontSize < 0)
				tmpFontSize = UIFont.LabelFontSize * Math.Abs(tmpFontSize);
			if (Math.Abs(tmpFontSize) < double.Epsilon*10)
				tmpFontSize = UIFont.LabelFontSize;
			var maxFontSize = (nfloat)Element.MaxFontSize;
			if (maxFontSize < 0)
				maxFontSize = 256;
			var minFontSize = (nfloat)Element.MinFontSize;
			if (minFontSize < 0)
				minFontSize = 4;
			if (tmpFontSize > maxFontSize)
				tmpFontSize = maxFontSize;
			if (tmpFontSize < minFontSize)
				tmpFontSize = minFontSize;

			Control.AdjustsFontSizeToFitWidth = false;
			Control.Font = Control.Font.WithSize((nfloat)tmpFontSize);
			Control.Lines = int.MaxValue;

			double tmpHt=-1;
			double tmpWd = widthConstraint;

			if (Element.Lines == 0 && Element.Fit != LabelFit.None)
			{
				Control.Lines = 0;
				tmpFontSize = ZeroLinesFit(new NSString(text), widthConstraint, heightConstraint);
			}
			else if (Element.Fit == LabelFit.Lines)
			{

				if (double.IsPositiveInfinity(heightConstraint))
				{
					// we need to set the height of the Control to be Lines * FontHeight
					//System.Diagnostics.Debug.WriteLine("\tAVAIL HT: INFINITE");
					tmpHt = Control.Font.LineHeight * Element.Lines + Control.Font.Leading * (Element.Lines - 1);// + ContentScaleFactor;
				}
				else
				{
					//System.Diagnostics.Debug.WriteLine("\tAVAIL HT: "+heightConstraint);
					var lineHeightRatio = Control.Font.LineHeight / Control.Font.PointSize;
					var leadingRatio = Control.Font.Leading / Control.Font.PointSize;

					tmpFontSize = ((heightConstraint) / (Element.Lines + leadingRatio * (Element.Lines - 1))) / lineHeightRatio - 0.1f;
				}
			}
			else if (Element.Fit == LabelFit.Width)
				tmpFontSize = WidthFit(new NSString(text), widthConstraint, heightConstraint);
			else if (Element.Fit == LabelFit.None && Element.Lines > 0)
			{
				tmpHt = Control.Font.LineHeight * Element.Lines + Control.Font.Leading * (Element.Lines - 1);// + ContentScaleFactor;
				//Control.Lines = Element.Lines;
				//System.Diagnostics.Debug.WriteLine("\tCalculated tmpHt=["+tmpHt+"]");
			}

			if (tmpFontSize > maxFontSize)
				tmpFontSize = maxFontSize;
			if (tmpFontSize < minFontSize)
				tmpFontSize = minFontSize;

			Control.Font = Control.Font.WithSize((nfloat)tmpFontSize);
			if (Element.Text != null)
				Control.Text = Element.Text;
			else if (Settings.IsLicenseValid || Element._id < 4) 
				Control.AttributedText = Element.ToNSAttributedString(Control, tmpFontSize);
			else 
				Control.Text = "UNLICENSED COPY";

			var constraintSize = new CGSize(widthConstraint, heightConstraint);
			CGSize cgSize;
			if (Element.Text == null)
				cgSize = Control.AttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
			else
				cgSize = Control.Text.StringSize(Control.Font, constraintSize, UILineBreakMode.WordWrap);
			if (tmpHt<0)
				tmpHt = cgSize.Height;
			tmpWd = cgSize.Width;

			if (Element.Fit == LabelFit.None && Element.Lines > 0)
			{
				//System.Diagnostics.Debug.WriteLine("\tcgSize.Height=["+cgSize.Height+"]");
				double lines = (cgSize.Height + Control.Font.Leading) / (Control.Font.LineHeight + Control.Font.Leading);
				if (lines < Element.Lines)
				{
					tmpHt = Control.Font.LineHeight * lines + Control.Font.Leading * (lines - 1);
					Control.Lines = (System.nint)Math.Ceiling(lines);
					//System.Diagnostics.Debug.WriteLine("\tRE-calculated tmpHt=[" + tmpHt + "] lines=[" + lines + "]");
				}
				else {
					Control.Lines = Element.Lines;
				}
			}

			//Control.BackgroundColor = UIColor.Blue;

			//if (!(Element.IsDynamicallySized && Element.Fit==LabelFit.None))
			{
				if (Element.Fit==LabelFit.Width)
					Control.Lines = Element.Lines;
				switch (Element.LineBreakMode)
				{
					case LineBreakMode.NoWrap:
						Control.LineBreakMode = UILineBreakMode.Clip;
						Control.Lines = 1;
						break;
					case LineBreakMode.WordWrap:
						Control.LineBreakMode = UILineBreakMode.WordWrap;
						break;
					case LineBreakMode.CharacterWrap:
						Control.LineBreakMode = UILineBreakMode.CharacterWrap;
						break;
					case LineBreakMode.HeadTruncation:
						Control.LineBreakMode = UILineBreakMode.HeadTruncation;
						break;
					case LineBreakMode.TailTruncation:
						Control.LineBreakMode = UILineBreakMode.TailTruncation;
						break;
					case LineBreakMode.MiddleTruncation:
						Control.LineBreakMode = UILineBreakMode.MiddleTruncation;
						break;
				}
			}

			double gap = 0;
			double height = 0;
			if (Element.Fit == LabelFit.None && tmpHt < heightConstraint)
			{
				gap = (nfloat)(heightConstraint - tmpHt);
				height = tmpHt;
			}
			else if (cgSize.Height < heightConstraint && !double.IsPositiveInfinity(heightConstraint))
			{
				gap = (nfloat)(heightConstraint - cgSize.Height);
				height = cgSize.Height;
			}
			else if (cgSize.Height < tmpHt)
			{
				gap = (nfloat)(tmpHt - cgSize.Height);
				height = cgSize.Height / 2.0;
			}
			if (gap > 0 && height > 0)
			{
				double y = 0;
				switch (Element.VerticalTextAlignment)
				{
					case TextAlignment.Center:
						y = gap / 2.0;
						break;
					case TextAlignment.End:
						y = gap;
						break;
				}
				Control.Frame = new CGRect(0, y, widthConstraint, height);
			}
			/*
					nfloat y = 0;
					switch (Element.VerticalTextAlignment)
					{
						case TextAlignment.Center:
							y = gap / 2.0f;
							break;
						case TextAlignment.End:
							y = gap;
							break;
					}
					Control.Frame = new CGRect(0, y, (nfloat)(widthConstraint), tmpHt);
			 */ 
			//System.Diagnostics.Debug.WriteLine("\tresult=["+tmpWd+","+tmpHt+"] [10,"+Control.Font.LineHeight+"]");
			return new SizeRequest(new Size(tmpWd, tmpHt), new Size(10, Control.Font.LineHeight));
		}
		#endregion




		#region Native layout cycle
		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			GetDesiredSize(Element.Width, Element.Height);
		}
		#endregion


		#region Fitting
		nfloat WidthFit(NSString text, double widthConstraint, double heightConstraint)
		{
			UIFont font = Control.Font;
			//nfloat result = ZeroLinesFit(text, widthConstraint, heightConstraint);
			nfloat result = font.PointSize;
			var minFontSize = (nfloat)Element.MinFontSize;
			if (minFontSize < 0)
				minFontSize = 4;

			nfloat step = (result - minFontSize)/5;
			if (step > 0.05f)
			{
				result = DescendingWidthFit(font, text, widthConstraint, heightConstraint, result, minFontSize, step);
				while (step > 0.25f)
				{
					step /= 5;
					result = DescendingWidthFit(font, text, widthConstraint, heightConstraint, result + step * 5, result, step);
				}
			}
			//System.Diagnostics.Debug.WriteLine("WIDTHFIT result=["+result+"]");
			return result;
		}

		nfloat DescendingWidthFit(UIFont font, NSString text, double widthConstraint, double heightConstraint, nfloat start, nfloat min, nfloat step)
		{
			nfloat result;
			var unmarkedText = ((HTMLMarkupString)Element.FormattedText)?.UnmarkedText ?? Element.HtmlText ?? text;
			bool twice = (unmarkedText.Length == 1);
			for (result = start; result > min; result -= step)
			{
				font = font.WithSize(result);
				Control.Font = font;
				var constraintSize = new CGSize(widthConstraint * (twice ? 2 : 1), double.PositiveInfinity);
				CGSize labelSize;
				if (Element.Text == null)
				{
					Control.AttributedText = Element.ToNSAttributedString(Control, font.PointSize, twice: twice);
					labelSize = Control.AttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
				}
				else {
					labelSize = text.StringSize(font, constraintSize, UILineBreakMode.WordWrap);
				}
				//System.Diagnostics.Debug.WriteLine("\tstep=["+step+"] size=["+result+"] lineHeight=["+font.LineHeight+"] height=["+labelSize.Height+"] lines=["+(labelSize.Height / font.LineHeight)+"]");
				if ((labelSize.Height / font.LineHeight) <= Element.Lines + .005f)
				{
					// the backspace character is tripping up this algorithm.  So we need to do a second check
					labelSize = Control.IntrinsicContentSize;

					return result;
				}
			}
			return result;
		}

		nfloat ZeroLinesFit(NSString text, double widthConstraint, double heightConstraint)
		{
			UIFont font = Control.Font;
			var startingFontSize = font.PointSize;
			if (double.IsPositiveInfinity(heightConstraint) || double.IsPositiveInfinity(widthConstraint))
				return startingFontSize;

			var minFontSize = (nfloat)Element.MinFontSize;
			if (minFontSize < 0)
				minFontSize = 4;

			nfloat result = DescendingZeroLinesFit(font, text, widthConstraint, heightConstraint, startingFontSize, minFontSize, 5);
			result = DescendingZeroLinesFit(font, text, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize,result + 5f), result, 1);
			result = DescendingZeroLinesFit(font, text, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 1f), result, 0.2f);
			result = DescendingZeroLinesFit(font, text, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 0.2f), result, 0.04f);
			//System.Diagnostics.Debug.WriteLine("ZEROLINESFIT result=["+result+"]");
			return result;
		}

		nfloat DescendingZeroLinesFit(UIFont font, NSString text, double widthConstraint, double heightConstraint, nfloat start, nfloat min, nfloat step)
		{
			nfloat result;
			var unmarkedText = ((HTMLMarkupString)Element.FormattedText)?.UnmarkedText ?? Element.HtmlText ?? text;
			bool twice = (unmarkedText.Length == 1);
			for (result = start; result > min; result -= step)
			{
				
				font = font.WithSize(result);
				Control.Font = font;
				var constraintSize = new CGSize(widthConstraint * (twice?2:1), double.PositiveInfinity);
				CGSize labelSize;
				if (Element.Text == null) {
					Control.AttributedText = Element.ToNSAttributedString(Control, font.PointSize, twice: twice);
					labelSize = Control.AttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
				} else {
					labelSize = text.StringSize(font, constraintSize, UILineBreakMode.WordWrap);
				}
				//System.Diagnostics.Debug.WriteLine("\tstep=["+step+"] size=[" + result + "] height=[" + labelSize.Height + "]");
				if (labelSize.Height <= heightConstraint)
				{
					labelSize = Control.IntrinsicContentSize;
					break;
				}
			}
			return result;
		}

		int _linesToFit = -1;
		double LinesFitError(double fontSize, double widthConstraint, double heightConstraint)
		{
			var boundingSize = new CGSize((nfloat)widthConstraint, nfloat.PositiveInfinity);
			if (boundingSize.Height < 1 || boundingSize.Width < 1)
				return 0;
			UIFont tmpFont = Control.Font.WithSize((nfloat)fontSize);
			//System.Diagnostics.Debug.WriteLine("fontSize=["+fontSize+"] tmpFont=["+tmpFont+"]");

			CGSize tmpSize;
			CGRect tmpRect;
			double tmpHt;
			double tmpWd;

			if (Element.Text == null)
			{
				Control.AttributedText = Element.ToNSAttributedString(Control, fontSize);
				tmpSize = Control.AttributedText.GetBoundingRect(boundingSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
				tmpHt = tmpSize.Height;
				tmpWd = tmpSize.Width;
			}
			else {
				//tmpSize = Control.Text.StringSize(tmpFont, boundingSize, UILineBreakMode.WordWrap);
				//var floatFontSize = (nfloat)fontSize;
				//tmpSize = Control.Text.StringSize(tmpFont, (System.nfloat)Element.MinFontSize, ref floatFontSize, (System.nfloat)Element.Width, UILineBreakMode.WordWrap);
				var context = new NSStringDrawingContext();
				var nsString = new NSString(Control.Text);

				var attrDict = new NSMutableDictionary();
				attrDict.Add(UIStringAttributeKey.Font, tmpFont);
				var attr = new UIStringAttributes(attrDict);
				tmpRect = nsString.GetBoundingRect(boundingSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attr, context);
				tmpHt = tmpRect.Height;
				tmpWd = tmpRect.Width;
				//System.Diagnostics.Debug.WriteLine("\tmpFont.PointSize=["+tmpFont.PointSize+"] lineHt=["+tmpFont.LineHeight+"] ");
			}
			//System.Diagnostics.Debug.WriteLine("\twd=["+tmpWd+"] ht=["+tmpHt+"]");
			//double lines = tmpHt / tmpFont.LineHeight;
			double lines = (tmpHt + tmpFont.Leading) / (tmpFont.LineHeight + tmpFont.Leading);
			//System.Diagnostics.Debug.WriteLine("\tlines=["+lines+"]");
			//nfloat heightErr = (System.nfloat)(tmpSize.Height - Element.Height);
			if (_linesToFit == -1)
			{
				_linesToFit = (int)Math.Round(lines);
			}
			double error = _linesToFit - lines;
			//System.Diagnostics.Debug.WriteLine("LinesFitError fontSize=["+fontSize+"] tmpHt=["+tmpHt+"] leading=["+tmpFont.Leading+"] lineHeight=["+tmpFont.LineHeight+"] lines=[" + lines + "] error=["+error+"]");
			return error;
		}

		double WidthFitError(double fontSize)
		{
			var boundingSize = new CGSize((nfloat)Element.Width, nfloat.PositiveInfinity);
			if (boundingSize.Height < 1 || boundingSize.Width < 1)
				return 0;
			UIFont tmpFont = Control.Font.WithSize((nfloat)fontSize);
			//System.Diagnostics.Debug.WriteLine("fontSize=["+fontSize+"] tmpFont=["+tmpFont+"]");

			CGSize tmpSize;
			CGRect tmpRect;
			double tmpHt;
			double tmpWd;

			if (Element.Text == null)
			{
				Control.AttributedText = Element.ToNSAttributedString(Control, fontSize);
				tmpSize = Control.AttributedText.GetBoundingRect(boundingSize, 0, null).Size;
				tmpHt = tmpSize.Height;
				tmpWd = tmpSize.Width;
			}
			else {
				//tmpSize = Control.Text.StringSize(tmpFont, boundingSize, UILineBreakMode.WordWrap);
				//var floatFontSize = (nfloat)fontSize;
				//tmpSize = Control.Text.StringSize(tmpFont, (System.nfloat)Element.MinFontSize, ref floatFontSize, (System.nfloat)Element.Width, UILineBreakMode.WordWrap);
				var context = new NSStringDrawingContext();
				var nsString = new NSString(Control.Text);

				var attrDict = new NSMutableDictionary();
				attrDict.Add(UIStringAttributeKey.Font, tmpFont);
				var attr = new UIStringAttributes(attrDict);
				tmpRect = nsString.GetBoundingRect(boundingSize, NSStringDrawingOptions.UsesLineFragmentOrigin, attr,context);
				tmpHt = tmpRect.Height;
				tmpWd = tmpRect.Width;
				//System.Diagnostics.Debug.WriteLine("\tmpFont.PointSize=["+tmpFont.PointSize+"] lineHt=["+tmpFont.LineHeight+"] ");
			}
			//System.Diagnostics.Debug.WriteLine("\twd=["+tmpWd+"] ht=["+tmpHt+"]");
			//double lines = tmpHt / tmpFont.LineHeight;
			//System.Diagnostics.Debug.WriteLine("\tlines=["+lines+"]");
			//nfloat heightErr = (System.nfloat)(tmpSize.Height - Element.Height);
			var linesErr = (nfloat)(tmpHt - tmpFont.LineHeight * Element.Lines);
			//System.Diagnostics.Debug.WriteLine("\tlinesErr=[" + linesErr + "]");
			if (linesErr > 0)
				return linesErr * 1000;
			var widthErr = (nfloat)(tmpWd - Element.Width);
			//System.Diagnostics.Debug.WriteLine("\twidthErr=["+widthErr+"]");
			return widthErr;
		}
		#endregion



		#region Change management
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			if (e.NewElement != null) {
				if (Control == null) {
					SetNativeControl (new UILabel (CGRect.Empty) {
						BackgroundColor = UIColor.Clear
					});
				}
				UpdateText ();
				UpdateAlignment ();
			}
			base.OnElementChanged (e);
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName)
				UpdateAlignment();
			else if (e.PropertyName == Label.TextColorProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.FontProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.TextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Xamarin.Forms.Label.FormattedTextProperty.PropertyName)
				UpdateText();
		}

		/// <summary>
		/// Sets the color of the background.
		/// </summary>
		/// <param name="color">Color.</param>
		protected override void SetBackgroundColor (Color color)
		{
			if (color == Color.Default) {
				BackgroundColor = UIColor.Clear;
				return;
			}
			BackgroundColor = color.ToUIColor ();
		}

		void UpdateAlignment ()
		{
			Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment ();
		}

		void UpdateText()
		{
			perfectSizeValid = false;
			Control.Font = Element.ToUIFont();
			var color = (Color)Element.GetValue(Label.TextColorProperty);
			Control.TextColor = color.ToUIColor(UIColor.Black);
			if (Element.FormattedText != null)
			{
				if (Settings.IsLicenseValid || Element._id < 4)
					Control.AttributedText = Element.ToNSAttributedString(Control);
				else
					Control.Text = "UNLICENSED COPY";
			}
			else 
				Control.Text = (string)Element.GetValue(Label.TextProperty);
		}
		#endregion


	}
}

