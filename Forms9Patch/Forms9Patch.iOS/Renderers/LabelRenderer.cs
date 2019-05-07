using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using CoreGraphics;
using System.ComponentModel;
using Foundation;
using CoreMotion;
using System.Runtime.CompilerServices;


[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.iOS.LabelRenderer))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// Forms9Patch iOS Label renderer.
    /// </summary>
    public class LabelRenderer : ViewRenderer<Label, UILabel>
    {

        static LabelRenderer()
        {
            Forms9Patch.Label.DefaultFontSize = UIFont.LabelFontSize;
        }

        NSString ControlText;
        NSAttributedString ControlAttributedText;
        UIColor ControlTextColor;
        UIFont ControlFont;
        nfloat ControlFontPointSize;
        UIFontDescriptor FontDescriptor;
        UILineBreakMode ControlLineBreakMode = UILineBreakMode.CharacterWrap;
        int ControlLines = -1;

        SizeRequest LastDesiredSize = new SizeRequest(Size.Zero, Size.Zero);
        double LastMinFontSize = (double)Label.MinFontSizeProperty.DefaultValue;

        double LastWidthConstraint = -1;
        double LastHeightContraint = -1;


        void Debug(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0)
        {
            //var text = Element.Text ?? Element.HtmlText;
            //if (text == "BACKGROUND")
            //    System.Diagnostics.Debug.WriteLine(GetType() + "." + method + "[" + lineNumber + "]: " + message);
        }


        #region Xamarin layout cycle

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (widthConstraint < 0 || heightConstraint < 0)
                return new SizeRequest(Size.Zero);

            if (Control == null || Element == null)
                return new SizeRequest(Size.Zero);

            if (string.IsNullOrEmpty((ControlText ?? ControlAttributedText?.ToString())))
                return new SizeRequest(Size.Zero);

            UpdateFont();

            //if (Invalid || Math.Abs(widthConstraint - LastWidthConstraint) > 0.01 || Math.Abs(heightConstraint - LastHeightContraint) > 0.01 || Math.Abs(Element.MinFontSize - LastMinFontSize) > 0.01)
            {

                LastWidthConstraint = widthConstraint;
                LastHeightContraint = heightConstraint;
                LastMinFontSize = Element.MinFontSize;

                switch (Element.LineBreakMode)
                {
                    case LineBreakMode.HeadTruncation:
                        ControlLineBreakMode = UILineBreakMode.HeadTruncation;
                        break;
                    case LineBreakMode.TailTruncation:
                        ControlLineBreakMode = UILineBreakMode.TailTruncation;
                        break;
                    case LineBreakMode.MiddleTruncation:
                        ControlLineBreakMode = UILineBreakMode.MiddleTruncation;
                        break;
                    case LineBreakMode.NoWrap:
                        ControlLineBreakMode = UILineBreakMode.Clip;
                        ControlLines = 1;
                        break;
                    case LineBreakMode.CharacterWrap:
                        ControlLineBreakMode = UILineBreakMode.CharacterWrap;
                        break;
                    case LineBreakMode.WordWrap:
                    default:
                        ControlLineBreakMode = UILineBreakMode.WordWrap;
                        break;
                }


                var tmpFontSize = (nfloat)Element.FontSize;
                if (tmpFontSize < 0)
                    tmpFontSize = (nfloat)(UIFont.LabelFontSize * Math.Abs(tmpFontSize));
                if (Math.Abs(tmpFontSize) <= double.Epsilon * 10)
                    tmpFontSize = UIFont.LabelFontSize;
                var minFontSize = (nfloat)LastMinFontSize;
                if (minFontSize < 0)
                    minFontSize = 4;
                if (tmpFontSize < minFontSize)
                    tmpFontSize = minFontSize;

                ControlLines = int.MaxValue;

                double linesHeight = -1;
                double desiredWidth = widthConstraint;

                if (Element.Lines == 0)
                {
                    if (!double.IsInfinity(heightConstraint))
                    {
                        ControlLines = 0;
                        tmpFontSize = ZeroLinesFit(widthConstraint, heightConstraint, tmpFontSize);
                    }
                }
                else
                {
                    if (Element.AutoFit == AutoFit.Lines)
                    {
                        if (double.IsPositiveInfinity(heightConstraint))
                            linesHeight = Element.Lines * (ControlFont.LineHeight + ControlFont.Leading);
                        else
                        {
                            var lineHeightRatio = ControlFont.LineHeight / ControlFont.PointSize;
                            var leadingRatio = ControlFont.Leading / ControlFont.PointSize;

                            //tmpFontSize = (nfloat)(((heightConstraint) / ((1 + leadingRatio) * Element.Lines)) / lineHeightRatio - 0.1f);
                            var tmpLineSize = (nfloat)(heightConstraint - 0.05f) / Element.Lines;
                            tmpFontSize = tmpLineSize / lineHeightRatio;

                        }
                    }
                    else if (Element.AutoFit == AutoFit.Width)
                        tmpFontSize = WidthFit(widthConstraint, tmpFontSize);
                }

                if (tmpFontSize < minFontSize)
                    tmpFontSize = minFontSize;


                if (Math.Abs(tmpFontSize - Element.FittedFontSize) > 0.05)
                {
                    //Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                    //{
                    if (Element != null && Control != null)  // multipicker test was getting here with Element and Control both null
                    {
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
                        Element.FittedFontSize = tmpFontSize == Element.FontSize || (Element.FontSize == -1 && tmpFontSize == UIFont.LabelFontSize) ? -1 : (double)tmpFontSize;
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
                        Debug("SETTING FITTED FONT SIZE: " + Element?.FittedFontSize);
                    }
                    //return false;
                    //});
                }


                var syncFontSize = (nfloat)((ILabel)Element).SynchronizedFontSize;
                if (syncFontSize >= 0 && tmpFontSize != syncFontSize)
                    tmpFontSize = syncFontSize;

                ControlFont = ControlFont.WithSize(tmpFontSize);

                CGSize cgSize = LabelSize(widthConstraint, tmpFontSize);
                Debug("cgSize: " + cgSize);

                //if (Control.Font != ControlFont || Control.AttributedText != ControlAttributedText || Control.Text != ControlText || Control.LineBreakMode != ControlLineBreakMode || Control.Lines != ControlLines)
                {
                    Control.Hidden = true;
                    Control.Lines = Element.Lines;
                    Control.LineBreakMode = ControlLineBreakMode;

                    Control.AdjustsFontSizeToFitWidth = false;
                    Control.ClearsContextBeforeDrawing = true;
                    Control.ContentMode = UIViewContentMode.Redraw;

                    Control.Font = ControlFont;

                    if (ControlAttributedText != null)
                        Control.AttributedText = ControlAttributedText;
                    else
                        Control.Text = ControlText;

                    Control.Hidden = false;
                }


                double reqWidth = cgSize.Width;
                double reqHeight = cgSize.Height + 0.05;
                var textHeight = cgSize.Height;
                var textLines = Lines(textHeight, Control.Font);
                string alg = "--";
                //string cnstLinesStr = "CL: n/a    ";
                //string lineHeight = "LH: " + Control.Font.LineHeight.ToString("00.000");
                //string cnstLinesHeight = "CLH: n/a   ";

                if (double.IsPositiveInfinity(heightConstraint))
                {
                    Debug("A");
                    if (Element.Lines > 0)
                    {
                        if (Element.AutoFit == AutoFit.Lines)// && Element.Lines <= textLines)
                            reqHeight = Element.Lines * Control.Font.LineHeight;
                        else if (Element.AutoFit == AutoFit.None && Element.Lines <= textLines)
                            reqHeight = Element.Lines * Control.Font.LineHeight;
                    }

                    //    alg = "∞A";
                    //}
                    Control.Center = new CGPoint(Control.Center.X, reqHeight / 2);
                    Debug("Control.Center: " + Control.Center);
                }
                else
                {
                    Debug("B");
                    var constraintLines = Lines(heightConstraint, Control.Font);
                    Debug("\t constraintLines: " + constraintLines);
                    var constraintLinesHeight = Math.Floor(constraintLines) * Control.Font.LineHeight;
                    Debug("\t constraintLinesHeight: " + constraintLinesHeight);
                    //cnstLinesStr = "CL: " + constraintLines.ToString("0.000");

                    if (Element.Lines > 0 && Element.Lines <= Math.Min(textLines, constraintLines))
                    {
                        reqHeight = Element.Lines * Control.Font.LineHeight;
                        alg = "A";
                    }
                    else if (textLines <= constraintLines)
                    {
                        reqHeight = textHeight;
                        alg = "B";
                    }
                    else if (constraintLines >= 1)
                    {
                        reqHeight = constraintLinesHeight;
                        alg = "C";
                    }
                    else
                    {
                        reqHeight = heightConstraint;
                        alg = "D";
                    }
                    Debug("\t alg: " + alg);
                    Debug("\t reqHeight: " + reqHeight);

                    Debug("\t Element.VerticalTextAlignment: " + Element.VerticalTextAlignment);
                    if (Element.VerticalTextAlignment == TextAlignment.Start)
                        Control.Center = new CGPoint(Control.Center.X, reqHeight / 2);
                    else if (Element.VerticalTextAlignment == TextAlignment.End)
                        Control.Center = new CGPoint(Control.Center.X, heightConstraint - reqHeight / 2);
                    Debug("Control.Center: " + Control.Center);
                }
                LastDesiredSize = new SizeRequest(new Size(Math.Ceiling(reqWidth), Math.Ceiling(reqHeight)), new Size(10, Math.Ceiling(ControlFont.LineHeight)));
            }
            return LastDesiredSize;
        }

        void UpdateSynchronizedFontSize()
        {
            var syncFontSize = (nfloat)((ILabel)Element).SynchronizedFontSize;
            var syncFont = ControlFont.WithSize(syncFontSize);
            if (syncFont != ControlFont)
            {
                GetDesiredSize(LastWidthConstraint, LastHeightContraint);
            }
        }
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
        Size LabelXamarinSize(double widthConstraint, double fontSize)
        {
            var cgsize = LabelSize(widthConstraint, (nfloat)fontSize);
            return new Size(cgsize.Width, cgsize.Height);
        }

        CGSize LabelSize(double widthConstraint, nfloat fontSize)
        {
            var font = UIFont.FromDescriptor(FontDescriptor, fontSize);
            CGSize labelSize = CGSize.Empty;
            var constraintSize = new CGSize(widthConstraint, double.PositiveInfinity);
            if (Element?.F9PFormattedString != null)
            {
                ControlAttributedText = Element.F9PFormattedString.ToNSAttributedString(font, ControlTextColor);//, twice: twice);
                labelSize = ControlAttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
                ControlText = null;
            }
            else if (ControlText != null)
            {
                labelSize = ControlText.StringSize(font, constraintSize, (Element.LineBreakMode == LineBreakMode.CharacterWrap ? UILineBreakMode.CharacterWrap : UILineBreakMode.WordWrap));
                ControlAttributedText = null;
            }
            return labelSize;
        }

        nfloat WidthFit(double widthConstraint, nfloat startFontSize)
        {
            if (Math.Abs(widthConstraint) < 0.01)
                return 0;
            if (widthConstraint < 0)
                return startFontSize;

            nfloat result = startFontSize;
            var minFontSize = (nfloat)Element.MinFontSize;
            if (minFontSize < 0)
                minFontSize = 4;

            if (Element.Lines == 1)
            {
                var size = LabelSize(double.MaxValue, startFontSize);
                if (size.Width > widthConstraint)
                    result = (nfloat)(startFontSize * widthConstraint / size.Width);
            }
            else
            {
                nfloat step = (result - minFontSize) / 5;
                if (step > 0.05f)
                {
                    result = DescendingWidthFit(widthConstraint, result, minFontSize, step);
                    while (step > 0.25f)
                    {
                        step /= 5;
                        result = DescendingWidthFit(widthConstraint, result + step * 5, result, step);
                    }
                }
            }
            return result;
        }

        nfloat DescendingWidthFit(double widthConstraint, nfloat start, nfloat min, nfloat step)
        {
            nfloat result;
            for (result = start; result > min; result -= step)
            {
                var font = Control.Font.WithSize(result);
                CGSize labelSize = LabelSize(widthConstraint, result);
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
            result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 5f), result, 1);
            result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 1f), result, 0.2f);
            result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 0.2f), result, 0.04f);
            return result;
        }

        nfloat DescendingZeroLinesFit(double widthConstraint, double heightConstraint, nfloat start, nfloat min, nfloat step)
        {
            nfloat result;
            for (result = start; result > min; result -= step)
            {
                CGSize labelSize = LabelSize(widthConstraint, result);
                if (labelSize.Height <= heightConstraint)
                {
                    labelSize = Control.IntrinsicContentSize;
                    break;
                }
            }
            return result;
        }



        #endregion


        double Lines(double height, UIFont font)
        {
            //return (height + font.Leading) / (font.LineHeight + font.Leading);
            return (height) / (font.LineHeight);
        }

        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            if (e.OldElement != null)
            {
                e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelXamarinSize;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(new UILabel(CGRect.Empty)
                    {
                        BackgroundColor = UIColor.Clear
                    });
                }
                UpdateTextColor();
                UpdateFont();
                UpdateText();
                UpdateHorizontalAlignment();
                e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                e.NewElement.RendererSizeForWidthAndFontSize += LabelXamarinSize;
            }
            base.OnElementChanged(e);

        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalAlignment();
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
                if (ControlAttributedText != null)
                    UpdateText();
            }
            else if (e.PropertyName == Label.FontProperty.PropertyName || e.PropertyName == Label.FontFamilyProperty.PropertyName || e.PropertyName == Label.FontSizeProperty.PropertyName || e.PropertyName == Label.FontAttributesProperty.PropertyName)
            {
                UpdateFont();
                if (ControlAttributedText != null)
                    UpdateText();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                UpdateText();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.F9PFormattedStringProperty.PropertyName)
            {
                UpdateText();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName ||
                     e.PropertyName == Label.AutoFitProperty.PropertyName ||
                     e.PropertyName == Label.LinesProperty.PropertyName ||
                     e.PropertyName == Label.LineBreakModeProperty.PropertyName
                    )
            {
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.SynchronizedFontSizeProperty.PropertyName)
                UpdateSynchronizedFontSize();
        }

        void UpdateFont()
        {
            ControlFont = Element.ToUIFont();
            InvokeOnMainThread(() =>
            {
                if (Control != null)
                    Control.Font = ControlFont;
            });
            ControlFontPointSize = Control.Font.PointSize;
            FontDescriptor = ControlFont.FontDescriptor;
        }

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="color">Color.</param>
        protected override void SetBackgroundColor(Color color)
        {
            if (color == Color.Default)
            {
                BackgroundColor = UIColor.Clear;
                return;
            }
            BackgroundColor = color.ToUIColor();
        }

        void UpdateHorizontalAlignment()
        {
            InvokeOnMainThread(() =>
            {
                if (Control != null)
                    Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
            });
        }

        void UpdateText()
        {
            string text = null;
            NSAttributedString attributedText = null;
            if (Element.F9PFormattedString != null)
            {
                var color = (Color)Element.GetValue(Label.TextColorProperty);
                ControlTextColor = color.ToUIColor(UIColor.Black);
                attributedText = Element.F9PFormattedString.ToNSAttributedString(ControlFont, ControlTextColor);
            }
            else
                text = (string)Element.GetValue(Label.TextProperty);

            if (text != null && Control != null)
            {
                InvokeOnMainThread(() =>
                {
                    if (Control != null)
                    {
                        Control.AttributedText = ControlAttributedText = null;
                        Control.Text = ControlText = new NSString(text);
                    }
                });
            }
            else
            {
                InvokeOnMainThread(() =>
                {
                    if (Control != null)
                    {
                        Control.Text = ControlText = null;
                        Control.AttributedText = ControlAttributedText = attributedText;
                    }
                });
            }
        }

        void UpdateTextColor()
        {
            InvokeOnMainThread(() =>
            {
                var color = (Color)Element.GetValue(Label.TextColorProperty);
                ControlTextColor = color.ToUIColor(UIColor.Black);
                if (Control != null)
                    Control.TextColor = ControlTextColor;
            });
        }
        #endregion


        #region Index of touch point
        int IndexAtPoint(Point point)
        {
            var cgPoint = new CGPoint(point.X, point.Y - Control.Frame.Y);

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
            var textContainer = new NSTextContainer(new CGSize(Control.Frame.Width, Control.Frame.Height * 2));
            textContainer.LineFragmentPadding = 0;
            textContainer.MaximumNumberOfLines = (nuint)ControlLines;
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
            nfloat partialFraction = 0;
            //var characterIndex = layoutManager.CharacterIndexForPoint(cgPoint, textContainer, ref partialFraction);
            var characterIndex = layoutManager.GetCharacterIndex(cgPoint, textContainer);

            //[self.layoutManager drawGlyphsForGlyphRange:NSMakeRange(0, self.textStorage.length) atPoint:CGPointMake(0, 0)];
            //layoutManager.DrawGlyphs(new NSRange(0,Control.AttributedText.Length),Control.Frame.Location);

            return (int)characterIndex;
        }
        #endregion
    }
}

