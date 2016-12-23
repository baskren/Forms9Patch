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
		NSString Text;
		//string UnmarkedText;

		//bool InvalidLayout;

		//
		// Fields
		//
		//bool perfectSizeValid;

		//SizeRequest perfectSize;

		#region Xamarin layout cycle
		/// <summary>
		/// Gets the size of the desired.
		/// </summary>
		/// <returns>The desired size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			//if (Element.Text == "HEIGHTS AND AREAS CALCULATOR")
			// System.Diagnostics.Debug.WriteLine("GetDesiredSize(" + widthConstraint + "," + heightConstraint + ") enter");
			if (Control == null)
				return new SizeRequest(Size.Zero);

			Control.ClearsContextBeforeDrawing = true;  
			Control.ContentMode = UIViewContentMode.Redraw;

			if (string.IsNullOrEmpty(Text))
				return new SizeRequest(Size.Zero);

			 Control.LineBreakMode = UILineBreakMode.WordWrap;


			var tmpFontSize = (nfloat)Element.FontSize;
			if (tmpFontSize < 0)
				tmpFontSize = (nfloat)(UIFont.LabelFontSize * Math.Abs(tmpFontSize));
			if (Math.Abs(tmpFontSize) <= double.Epsilon*10)
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
			Control.Lines = int.MaxValue;

			double tmpHt=-1;
			double tmpWd = widthConstraint;

			if (Element.Lines == 0 && Element.Fit != LabelFit.None)
			{
				Control.Lines = 0;
				tmpFontSize = ZeroLinesFit(widthConstraint, heightConstraint, tmpFontSize);
				if (tmpFontSize > maxFontSize)
					tmpFontSize = maxFontSize;
				if (tmpFontSize < minFontSize)
					tmpFontSize = minFontSize;
				Control.Font = Control.Font.WithSize(tmpFontSize);
			}
			else if (Element.Fit == LabelFit.Lines)
			{
				if (double.IsPositiveInfinity(heightConstraint))
				{
					Control.Font = Control.Font.WithSize(tmpFontSize);
					// we need to set the height of the Control to be Lines * FontHeight
					//System.Diagnostics.Debug.WriteLine("\tAVAIL HT: INFINITE");
					tmpHt = Control.Font.LineHeight * Element.Lines + Control.Font.Leading * (Element.Lines - 1);// + ContentScaleFactor;
				}
				else
				{
					//System.Diagnostics.Debug.WriteLine("\tAVAIL HT: "+heightConstraint);
					var lineHeightRatio = Control.Font.LineHeight / Control.Font.PointSize;
					var leadingRatio = Control.Font.Leading / Control.Font.PointSize;

					tmpFontSize = (nfloat)(((heightConstraint) / (Element.Lines + leadingRatio * (Element.Lines - 1))) / lineHeightRatio - 0.1f);
					if (tmpFontSize > maxFontSize)
						tmpFontSize = maxFontSize;
					if (tmpFontSize < minFontSize)
						tmpFontSize = minFontSize;
					Control.Font = Control.Font.WithSize(tmpFontSize);
				}
			}
			else if (Element.Fit == LabelFit.Width)
			{
				tmpFontSize = WidthFit(widthConstraint,tmpFontSize);
				if (tmpFontSize > maxFontSize)
					tmpFontSize = maxFontSize;
				if (tmpFontSize < minFontSize)
					tmpFontSize = minFontSize;
				Control.Font = Control.Font.WithSize(tmpFontSize);
			}
			else if (Element.Fit == LabelFit.None && Element.Lines > 0)
			{
				Control.Font = Control.Font.WithSize(tmpFontSize);
				tmpHt = Control.Font.LineHeight * Element.Lines + Control.Font.Leading * (Element.Lines - 1);// + ContentScaleFactor;
			}


			CGSize cgSize = LabelSize(widthConstraint, Control.Font.PointSize);
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
					Control.Lines = (nint)Math.Ceiling(lines);
				}
				else 
					Control.Lines = Element.Lines;
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
			//Element.ActualFontSize = Control.Font.PointSize;  // crashes on Unimposed Height LabelFit when Fit is set to Lines


			if (!_delayingActualFontSizeUpdate)
			{
				_delayingActualFontSizeUpdate = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
				{
					_delayingActualFontSizeUpdate = false;
					if (Element!=null && Control != null)  // multipicker test was getting here with Element and Control both null
						Element.ActualFontSize = Control.Font.PointSize;
					return false;
				});
			}
			//if (Element.Text == "HEIGHTS AND AREAS CALCULATOR")
			//if (Element.HtmlText=="Fractional Mode")
			//	System.Diagnostics.Debug.WriteLine("\tresult=[" + tmpWd + "," + tmpHt + "] cgSize=[" + cgSize.Width + "," + cgSize.Height + "] [10," + Control.Font.LineHeight + "]");
			return new SizeRequest(new Size(tmpWd, tmpHt), new Size(10, Control.Font.LineHeight));
		}
		bool _delayingActualFontSizeUpdate;

		#endregion


		#region Native layout cycle
		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (Element != null)
				GetDesiredSize(Element.Width, Element.Height);
		}
		#endregion


		#region Fitting
		/// <summary>
		/// Get Label Size w/o impact to label
		/// </summary>
		/// <returns>The size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="fontSize">Font size.</param>
		Size LabelXamarinSize(double widthConstraint, double fontSize)
		{
			var cgsize = LabelSize(widthConstraint, fontSize);
			return new Size(cgsize.Width, cgsize.Height);
		}

		/// <summary>
		/// Get Label Size w/o impact to label
		/// </summary>
		/// <returns>The size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="fontSize">Font size.</param>
		CGSize LabelSize(double widthConstraint, double fontSize)
		{
			var font = Control.Font.WithSize((nfloat)fontSize);
			CGSize labelSize;
			var constraintSize = new CGSize(widthConstraint, double.PositiveInfinity);
			if (Element.Text == null)
			{
				var attributedText = Element.ToNSAttributedString(Control, font.PointSize);//, twice: twice);
				labelSize = attributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
			}
			else {
				labelSize = Text.StringSize(font, constraintSize, UILineBreakMode.WordWrap);
			}
			return labelSize;
		}

		nfloat WidthFit(double widthConstraint, nfloat startFontSize)
		{
			//UIFont font = Control.Font;
			//nfloat result = ZeroLinesFit(text, widthConstraint, heightConstraint);
			//nfloat result = font.PointSize;
			nfloat result = startFontSize;
			var minFontSize = (nfloat)Element.MinFontSize;
			if (minFontSize < 0)
				minFontSize = 4;


			nfloat step = (result - minFontSize)/5;
			if (step > 0.05f)
			{
				result = DescendingWidthFit(widthConstraint, result, minFontSize, step);
				while (step > 0.25f)
				{
					step /= 5;
					result = DescendingWidthFit(widthConstraint, result + step * 5, result, step);
				}
			}
			//System.Diagnostics.Debug.WriteLine("WIDTHFIT result=["+result+"]");
			return result;
		}

		nfloat DescendingWidthFit(double widthConstraint, nfloat start, nfloat min, nfloat step)
		{
			nfloat result;
			for (result = start; result > min; result -= step)
			{
				var font = Control.Font.WithSize(result);
				CGSize labelSize = LabelSize(widthConstraint, result);
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

		nfloat ZeroLinesFit(double widthConstraint, double heightConstraint, nfloat startingFontSize)
		{
			//UIFont font = Control.Font.WithSize((nfloat)startSize);
			//var startingFontSize = font.PointSize;
			if (double.IsPositiveInfinity(heightConstraint) || double.IsPositiveInfinity(widthConstraint))
				return startingFontSize;

			var minFontSize = (nfloat)Element.MinFontSize;
			if (minFontSize < 0)
				minFontSize = 4;

			nfloat result = DescendingZeroLinesFit(widthConstraint, heightConstraint, startingFontSize, minFontSize, 5);
			result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize,result + 5f), result, 1);
			result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 1f), result, 0.2f);
			result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 0.2f), result, 0.04f);
			//System.Diagnostics.Debug.WriteLine("ZEROLINESFIT result=["+result+"]");
			return result;
		}

		nfloat DescendingZeroLinesFit(double widthConstraint, double heightConstraint, nfloat start, nfloat min, nfloat step)
		{
			nfloat result;
			for (result = start; result > min; result -= step)
			{
				CGSize labelSize = LabelSize(widthConstraint, result);
				//System.Diagnostics.Debug.WriteLine("\tstep=["+step+"] size=[" + result + "] height=[" + labelSize.Height + "]");
				if (labelSize.Height <= heightConstraint)
				{
					labelSize = Control.IntrinsicContentSize;
					break;
				}
			}
			return result;
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
			if (e.OldElement != null)
				e.OldElement.RendererIndexAtPoint -= IndexAtPoint;

			if (e.NewElement != null) {
				if (Control == null) {
					SetNativeControl (new UILabel (CGRect.Empty) {
						BackgroundColor = UIColor.Clear
					});
				}
				Control.Font = Element.ToUIFont();
				UpdateFontColor();
				UpdateText ();
				UpdateAlignment ();
				e.NewElement.RendererIndexAtPoint += IndexAtPoint;
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
				UpdateFontColor();
			else if (e.PropertyName == Label.FontProperty.PropertyName)
			{
				Control.Font = Element.ToUIFont();
				UpdateFontColor();
				//InvalidLayout = true;
			}
			else if (e.PropertyName == Label.TextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.F9PFormattedStringProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
				LayoutSubviews();
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
			if (Element.F9PFormattedString != null)
			{
				if (Settings.IsLicenseValid || Element._id < 4)
					Control.AttributedText = Element.ToNSAttributedString(Control);
				else
					Control.Text = "UNLICENSED COPY";
			}
			else 
				Control.Text = (string)Element.GetValue(Label.TextProperty);
			var text = Element.Text ?? Element.F9PFormattedString?.Text;
			if (text != null)
			{
				Text = new NSString(text);
				//UnmarkedText = ((HTMLMarkupString)Element.F9PFormattedString)?.UnmarkedText ?? Element.HtmlText ?? text;
			}
			else
			{
				Text = null;
				//UnmarkedText = null;
			}
			//InvalidLayout = true;
		}

		void UpdateFontColor()
		{
			var color = (Color)Element.GetValue(Label.TextColorProperty);
			Control.TextColor = color.ToUIColor(UIColor.Black);
		}
		#endregion


		#region Index of touch point
		int IndexAtPoint(Point point)
		{
			var cgPoint = new CGPoint(point.X , point.Y - Control.Frame.Y);
			System.Diagnostics.Debug.WriteLine("cgPoint=["+cgPoint+"] Frame=["+Control.Frame+"]");

			// init text storage
			var textStorage = new NSTextStorage();
			//if (Control.AttributedText != null)
			var attrText = new NSAttributedString(Control.AttributedText);
			textStorage.SetString(attrText);

			/*
			else
			{
				var attrString = new NSMutableAttributedString(Control.Text);
				attrString.AddAttribute(UIStringAttributeKey.Font, Control.Font, new NSRange(0, Control.Text.Length));
				textStorage.SetString(attrString);
			}
			*/

			// init layout manager
			var layoutManager = new NSLayoutManager();
			textStorage.AddLayoutManager(layoutManager);

			// init text container
			var textContainer = new NSTextContainer(new CGSize(Control.Frame.Width, Control.Frame.Height*2));
			textContainer.LineFragmentPadding = 0;
			textContainer.MaximumNumberOfLines = (nuint)Control.Lines;
			textContainer.LineBreakMode = UILineBreakMode.WordWrap;//Control.LineBreakMode;

			//if (Control.LineBreakMode == UILineBreakMode.TailTruncation || Control.LineBreakMode == UILineBreakMode.HeadTruncation || Control.LineBreakMode == UILineBreakMode.MiddleTruncation)
			//	textContainer.LineBreakMode = UILineBreakMode.WordWrap;

			textContainer.Size = new CGSize(Control.Frame.Width, Control.Frame.Height * 2);
			layoutManager.AddTextContainer(textContainer);
			layoutManager.AllowsNonContiguousLayout = true;

			//layoutManager.SetTextContainer(textContainer,new NSRange(0,Control.AttributedText.Length));

			//layoutManager.UsesFontLeading = true;
			//layoutManager.EnsureLayoutForCharacterRange(new NSRange(0,Control.AttributedText.Length));
			//layoutManager.EnsureLayoutForTextContainer(textContainer);
			nfloat partialFraction=0;
			var characterIndex = layoutManager.CharacterIndexForPoint(cgPoint, textContainer, ref partialFraction);

			 //[self.layoutManager drawGlyphsForGlyphRange:NSMakeRange(0, self.textStorage.length) atPoint:CGPointMake(0, 0)];
			//layoutManager.DrawGlyphs(new NSRange(0,Control.AttributedText.Length),Control.Frame.Location);

			return (int)characterIndex;
		}
		#endregion
	}
}

