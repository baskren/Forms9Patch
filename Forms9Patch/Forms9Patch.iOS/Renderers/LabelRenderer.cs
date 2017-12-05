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

        bool firstRun = true;

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

        //		TextAlignment LastVerticalTextAlignment = (TextAlignment)Xamarin.Forms.Label.VerticalTextAlignmentProperty.DefaultValue;
        double LastWidthConstraint = -1;
        double LastHeightContraint = -1;

        bool Invalid = true;


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

            if (Control == null)
                return new SizeRequest(Size.Zero);

            if (string.IsNullOrEmpty((ControlText ?? ControlAttributedText?.ToString())))
                return new SizeRequest(Size.Zero);


            if (Invalid || Math.Abs(widthConstraint - LastWidthConstraint) > 0.01 || Math.Abs(heightConstraint - LastHeightContraint) > 0.01 || Math.Abs(Element.MinFontSize - LastMinFontSize) > 0.01)
            {
                //if (Element.Text == "HEIGHTS AND AREAS CALCULATOR")
                //if (Element.HtmlText == "2015 IBC")
                //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                //    System.Diagnostics.Debug.WriteLine("GetDesiredSize(" + widthConstraint + "," + heightConstraint + ") enter [" + (Element.Text ?? Element.F9PFormattedString.Text) + "]");

                Invalid = false;
                LastWidthConstraint = widthConstraint;
                LastHeightContraint = heightConstraint;
                LastMinFontSize = Element.MinFontSize;

                ControlLineBreakMode = UILineBreakMode.WordWrap;

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

                ControlFont = ControlFont.WithSize(tmpFontSize);
                ControlLines = int.MaxValue;

                double tmpHt = -1;
                double tmpWd = widthConstraint;

                if (Element.Lines == 0 && Element.AutoFit != AutoFit.None)
                {
                    ControlLines = 0;
                    tmpFontSize = ZeroLinesFit(widthConstraint, heightConstraint, tmpFontSize);
                    if (tmpFontSize < minFontSize)
                        tmpFontSize = minFontSize;
                    ControlFont = ControlFont.WithSize(tmpFontSize);
                    //System.Diagnostics.Debug.WriteLine("F.A");
                }
                else if (Element.AutoFit == AutoFit.Lines)
                {
                    if (double.IsPositiveInfinity(heightConstraint))
                    {
                        // redundant : ControlFont = ControlFont.WithSize(tmpFontSize);
                        // we need to set the height of the Control to be Lines * FontHeight
                        //System.Diagnostics.Debug.WriteLine("\tAVAIL HT: INFINITE");
                        //tmpHt = ControlFont.LineHeight * Element.Lines + ControlFont.Leading * (Element.Lines);//- 1);// + ContentScaleFactor;
                        tmpHt = Element.Lines * (ControlFont.LineHeight + ControlFont.Leading);
                        //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                        //    System.Diagnostics.Debug.WriteLine("F.B");
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("\tAVAIL HT: "+heightConstraint);
                        var lineHeightRatio = ControlFont.LineHeight / ControlFont.PointSize;
                        var leadingRatio = ControlFont.Leading / ControlFont.PointSize;

                        //tmpFontSize = (nfloat)(((heightConstraint) / (Element.Lines + leadingRatio * (Element.Lines /*- 1*/))) / lineHeightRatio - 0.1f);
                        tmpFontSize = (nfloat)(((heightConstraint) / ((1 + leadingRatio) * Element.Lines)) / lineHeightRatio - 0.1f);

                        if (tmpFontSize < minFontSize)
                            tmpFontSize = minFontSize;
                        ControlFont = ControlFont.WithSize(tmpFontSize);
                        //System.Diagnostics.Debug.WriteLine("F.C");
                    }
                }
                else if (Element.AutoFit == AutoFit.Width)
                {
                    tmpFontSize = WidthFit(widthConstraint, tmpFontSize);
                    if (tmpFontSize < minFontSize)
                        tmpFontSize = minFontSize;
                    ControlFont = ControlFont.WithSize(tmpFontSize);
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("F.D");
                }
                else if (Element.AutoFit == AutoFit.None)
                {
                    if (Element.Lines > 0)
                    {
                        // redundant: ControlFont = ControlFont.WithSize(tmpFontSize);
                        //tmpHt = ControlFont.LineHeight * Element.Lines + ControlFont.Leading * (Element.Lines);// - 1);// + ContentScaleFactor;
                        tmpHt = Element.Lines * (ControlFont.LineHeight + ControlFont.Leading);
                        //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                        //    System.Diagnostics.Debug.WriteLine("F.E tmpFontSize=[" + tmpFontSize + "] tmpHt=[" + tmpHt + "]");
                    }
                }

                ControlFontPointSize = tmpFontSize;
                CGSize cgSize = LabelSize(widthConstraint, ControlFontPointSize);
                if (tmpHt < 0)
                {
                    tmpHt = cgSize.Height;
                    if (heightConstraint > double.MaxValue / 4)
                        tmpHt += ControlFont.Leading;  // required to address an issue with unconstrained height, lines=0, some font sizes
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("G 1 tmpHt=[" + tmpHt + "]");
                }


                if (Element.AutoFit == AutoFit.None && Element.Lines > 0)
                {
                    double lines = (cgSize.Height + ControlFont.Leading) / (ControlFont.LineHeight + ControlFont.Leading);
                    ControlLines = lines < Element.Lines ? (int)Math.Ceiling(lines) : Element.Lines;
                    //tmpHt = ControlFont.LineHeight * ControlLines + ControlFont.Leading * (ControlLines);// - 1);
                    if (heightConstraint < double.MaxValue / 5)
                    {
                        //System.Diagnostics.Debug.WriteLine("heightConstraint=[BIG] Element.Lines=[" + Element.Lines + "]");
                        tmpHt = Element.Lines * (ControlFont.LineHeight + ControlFont.Leading);
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("heightConstraint=[" + heightConstraint + "] ControlLines=[" + ControlLines + "]");
                        tmpHt = ControlLines * (ControlFont.LineHeight + ControlFont.Leading);
                    }
                    /*
                    if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    {
                        System.Diagnostics.Debug.WriteLine("\tControlFont.Leading=[" + ControlFont.Leading + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.LineHeight=[" + ControlFont.LineHeight + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.Ascender=[" + ControlFont.Ascender + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.Descender=[" + ControlFont.Descender + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.CapHeight=[" + ControlFont.CapHeight + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.PointSize=[" + ControlFont.PointSize + "]");
                        System.Diagnostics.Debug.WriteLine("\tControlFont.xHeight=[" + ControlFont.xHeight + "]");
                    }
*/

                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("H tmpHt=[" + tmpHt + "]  ControlLines=[" + ControlLines + "] lines=[" + lines + "]");
                }
                tmpWd = cgSize.Width;

                // Control.BackgroundColor = UIColor.FromRGBA(0, 0, 255, 100);

                if (Element.AutoFit == AutoFit.Width)
                    ControlLines = Element.Lines;
                switch (Element.LineBreakMode)
                {
                    case LineBreakMode.NoWrap:
                        ControlLineBreakMode = UILineBreakMode.Clip;
                        ControlLines = 1;
                        break;
                    case LineBreakMode.WordWrap:
                        ControlLineBreakMode = UILineBreakMode.WordWrap;
                        break;
                    case LineBreakMode.CharacterWrap:
                        ControlLineBreakMode = UILineBreakMode.CharacterWrap;
                        break;
                    case LineBreakMode.HeadTruncation:
                        ControlLineBreakMode = UILineBreakMode.HeadTruncation;
                        break;
                    case LineBreakMode.TailTruncation:
                        ControlLineBreakMode = UILineBreakMode.TailTruncation;
                        break;
                    case LineBreakMode.MiddleTruncation:
                        ControlLineBreakMode = UILineBreakMode.MiddleTruncation;
                        break;
                }

                double gap = 0;
                double height = Math.Max(cgSize.Height, tmpHt);
                //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                //    System.Diagnostics.Debug.WriteLine("cgSize.Height=[" + cgSize.Height + "] tmpHt=[" + tmpHt + "] height=[" + height + "]");
                if (!double.IsInfinity(heightConstraint))
                {
                    height = Math.Max(height, heightConstraint);
                    //height = Math.Max(height, tmpHt);
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("J heightContraint=[" + heightConstraint + "] height=[" + height + "]");
                }
                double y = 0;

                //if (Element.HtmlText == "System")
                //	System.Diagnostics.Debug.WriteLine("GetDesiredSize(" + widthConstraint + "," + heightConstraint + ") enter");

                //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                //    System.Diagnostics.Debug.WriteLine("cgSize.Height[" + cgSize.Height + "] tmpHt[" + tmpHt + "] heightConstraint[" + heightConstraint + "] height[" + height + "]");

                if (heightConstraint < tmpHt)
                {
                    gap = -(nfloat)(tmpHt - heightConstraint);
                    height = heightConstraint;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("D gap[" + gap + "] height=[" + height + "] Center.Y=[" + (height / 2.0 + y) + "]");
                }
                else if (Element.AutoFit == AutoFit.None && tmpHt < height)
                //if (tmpHt < height)
                {
                    gap = (nfloat)(height - tmpHt);
                    height = tmpHt;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("A gap[" + gap + "] height=[" + height + "] Center.Y=[" + (height / 2.0 + y) + "]");
                }
                else if (cgSize.Height < height)// && !double.IsPositiveInfinity(heightConstraint))
                {
                    gap = (nfloat)(height - cgSize.Height);
                    height = cgSize.Height;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("B gap[" + gap + "] height=[" + height + "] Center.Y=[" + (height / 2.0 + y) + "]");
                }
                else if (cgSize.Height < tmpHt)
                {
                    gap = (nfloat)(tmpHt - cgSize.Height);
                    height = cgSize.Height / 2.0;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("C gap[" + gap + "] height=[" + height + "] Center.Y=[" + (height / 2.0 + y) + "]");
                }

                /*
                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                {
                    System.Diagnostics.Debug.WriteLine("\tControlFont.Leading=[" + ControlFont.Leading + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.LineHeight=[" + ControlFont.LineHeight + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.Ascender=[" + ControlFont.Ascender + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.Descender=[" + ControlFont.Descender + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.CapHeight=[" + ControlFont.CapHeight + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.PointSize=[" + ControlFont.PointSize + "]");
                    System.Diagnostics.Debug.WriteLine("\tControlFont.xHeight=[" + ControlFont.xHeight + "]");
                }

                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    System.Diagnostics.Debug.WriteLine("0 Controlines=[" + ControlLines + "]");
                */
                if (Element.AutoFit == AutoFit.None && Element.Lines == 0)
                {
                    //var cgLines = Math.Ceiling(cgSize.Height / ControlFont.LineHeight);
                    var cgLines = cgSize.Height / ControlFont.LineHeight;
                    var portLines = heightConstraint / ControlFont.LineHeight;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("cgLines=[" + cgLines + "] portLines=[" + portLines + "]");
                    ControlLines = (int)Math.Min(cgLines, portLines);
                }
                else
                {
                    ControlLines = Math.Min(ControlLines, Element.Lines);
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("1 Controlines=[" + ControlLines + "]");
                    ControlLines = firstRun ? Element.Lines : ControlLines;
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("2 Controlines=[" + ControlLines + "]");
                    ControlLines = (int)Math.Min(Math.Floor(Control.Frame.Height / ControlFont.LineHeight), ControlLines);
                }
                //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                //    System.Diagnostics.Debug.WriteLine("3 Controlines=[" + ControlLines + "]");
                firstRun = false;

                /*
                if (gap > 0 && height > 0)
                {

                    //if (heightConstraint < double.MaxValue / 3.0)
                    {
                        switch (Element.VerticalTextAlignment)
                        {
                            case TextAlignment.Center:
                                y = gap / 2.0;
                                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;"))
                                    || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                                    System.Diagnostics.Debug.WriteLine("CENTER");
                                break;
                            case TextAlignment.End:
                                y = gap;
                                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;"))
                                    || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                                    System.Diagnostics.Debug.WriteLine("END");
                                break;
                        }
                    }
                    //Control.Frame = new CGRect(0, y, widthConstraint, height);  // doesn't work anymore but Control.Center does!

                }
                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                {
                    System.Diagnostics.Debug.WriteLine("ControlFont.LineHeight=[" + ControlFont.LineHeight + "] Control.Lines=[" + Control.Lines + "]");
                    System.Diagnostics.Debug.WriteLine("Controlines=[" + ControlLines + "] gap=[" + gap + "]");
                    System.Diagnostics.Debug.WriteLine("xA");
                }
                */

                if (Element.LineBreakMode == LineBreakMode.NoWrap && ControlLines > 0)
                {
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("xB");
                    switch (Element.VerticalTextAlignment)
                    {
                        case TextAlignment.Start:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("Nw S");
                            Control.Center = new CGPoint(Control.Center.X, 0 + (Control.Font.LineHeight + Control.Font.Leading) / 2.0);
                            break;
                        case TextAlignment.Center:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("Nw C");
                            Control.Center = new CGPoint(Control.Center.X, Control.Frame.Height / 2.0);
                            break;
                        case TextAlignment.End:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("Nw E");
                            Control.Center = new CGPoint(Control.Center.X, Control.Frame.Height - (Control.Font.LineHeight + Control.Font.Leading) / 2.0);
                            break;
                    }
                }
                else if ((Element.AutoFit == AutoFit.Width || Element.AutoFit == AutoFit.Lines) && gap > 0)
                {
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("xC");
                    switch (Element.VerticalTextAlignment)
                    {
                        case TextAlignment.Center:
                            y = gap / 2.0;
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("CENTER");
                            break;
                        case TextAlignment.End:
                            y = gap;
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("END");
                            break;
                    }
                    Control.Center = new CGPoint(Control.Center.X, height / 2.0 + y);
                }
                else if (ControlLines > 0 && Math.Abs(gap) > 0.1)
                {
                    //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    //    System.Diagnostics.Debug.WriteLine("xD");
                    //System.Diagnostics.Debug.WriteLine("calc lines=[" + lines + "]");

                    //if (!(Element.AutoFit == LabelFit.None && Element.Lines == 0))
                    {
                        double textBlockHeight = ControlFont.LineHeight * ControlLines;// + ControlFont.Leading * (ControlLines);// - 1);

                        gap = Control.Frame.Height - textBlockHeight;
                        //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                        //    System.Diagnostics.Debug.WriteLine("CL>0 ABS(gap)>0 Control.Frame.Height=[" + Control.Frame.Height + "] textBlockHeight=[" + textBlockHeight + "] gap=[" + gap + "]");
                    }



                    switch (Element.VerticalTextAlignment)
                    {
                        case TextAlignment.Start:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("CL>0 ABS(gap)>0 S");
                            Control.Center = new CGPoint(Control.Center.X, Control.Center.Y - gap / 2);
                            break;
                        case TextAlignment.Center:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("CL>0 ABS(gap)>0 C");
                            Control.Center = new CGPoint(Control.Center.X, Control.Center.Y);
                            break;
                        case TextAlignment.End:
                            //if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                            //    System.Diagnostics.Debug.WriteLine("CL>0 ABS(gap)>0 E");
                            Control.Center = new CGPoint(Control.Center.X, Control.Center.Y + gap / 2);
                            break;
                    }
                }
                /*
                else
                {
                    if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                        System.Diagnostics.Debug.WriteLine("xE");

                }
                */


                if (Control.Font != ControlFont || Control.AttributedText != ControlAttributedText || Control.Text != ControlText || Control.LineBreakMode != ControlLineBreakMode || Control.Lines != ControlLines)
                {
                    //System.Diagnostics.Debug.WriteLine("Control Property Changed");
                    Control.Hidden = true;
                    Control.Lines = ControlLines;
                    Control.LineBreakMode = ControlLineBreakMode;

                    Control.AdjustsFontSizeToFitWidth = false;
                    Control.ClearsContextBeforeDrawing = true;
                    Control.ContentMode = UIViewContentMode.Redraw;

                    Control.Font = ControlFont;

                    if (ControlAttributedText != null)
                        Control.AttributedText = ControlAttributedText;
                    else
                        Control.Text = ControlText;

                    if (!_delayingActualFontSizeUpdate)
                    {
                        _delayingActualFontSizeUpdate = true;
                        Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
                        {
                            _delayingActualFontSizeUpdate = false;
                            if (Element != null && Control != null)  // multipicker test was getting here with Element and Control both null
                                Element.OptimalFontSize = ControlFontPointSize;
                            return false;
                        });
                    }
                    Control.Hidden = false;
                }


                LastDesiredSize = new SizeRequest(new Size(tmpWd, tmpHt), new Size(10, ControlFont.LineHeight));
                /*
                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    System.Diagnostics.Debug.WriteLine("tmpWd[" + tmpWd + "] tmpHt[" + tmpHt + "]");
                if ((Element.Text != null && Element.Text.StartsWith("Żyłę;")) || (Control.Text != null && Control.Text.StartsWith("Żyłę;")))
                    System.Diagnostics.Debug.WriteLine("");
                    */
                //if (Element.Text == "HEIGHTS AND AREAS CALCULATOR")
                //if (Element.HtmlText=="Fractional Mode")
                //	System.Diagnostics.Debug.WriteLine("\tresult=[" + LastDesiredSize + "] Font=["+Control.Font+"] AutoFit=["+Element.AutoFit+"] Lines=["+Element.Lines+"]");
            }
            return LastDesiredSize;
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
            var cgsize = LabelSize(widthConstraint, (nfloat)fontSize);
            return new Size(cgsize.Width, cgsize.Height);
        }

        /// <summary>
        /// Get Label Size w/o impact to label
        /// </summary>
        /// <returns>The size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="fontSize">Font size.</param>
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
            result = DescendingZeroLinesFit(widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 5f), result, 1);
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



        #endregion


        #region Change management
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
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
                Invalid = true;
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
                Invalid = true;
                LayoutSubviews();
            }
        }

        void UpdateFont()
        {
            ControlFont = Element.ToUIFont();
            InvokeOnMainThread(() =>
            {
                Control.Font = ControlFont;
            });
            ControlFontPointSize = Control.Font.PointSize;
            FontDescriptor = ControlFont.FontDescriptor;
            Invalid = true;
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
                Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
            });
        }

        void UpdateText()
        {
            string text = null;
            NSAttributedString attributedText = null;
            if (Element.F9PFormattedString != null)
            {
                if (Settings.IsLicenseValid || Element._id < 4)
                    attributedText = Element.F9PFormattedString.ToNSAttributedString(ControlFont, ControlTextColor);
                else
                    text = "UNLICENSED COPY";
            }
            else
                text = (string)Element.GetValue(Label.TextProperty);

            if (text != null)
            {
                InvokeOnMainThread(() =>
                {
                    Control.AttributedText = ControlAttributedText = null;
                    Control.Text = ControlText = new NSString(text);
                });
            }
            else
            {
                InvokeOnMainThread(() =>
                {
                    Control.Text = ControlText = null;
                    Control.AttributedText = ControlAttributedText = attributedText;
                });
            }
            Invalid = true;
        }

        void UpdateTextColor()
        {
            var color = (Color)Element.GetValue(Label.TextColorProperty);
            ControlTextColor = color.ToUIColor(UIColor.Black);
            InvokeOnMainThread(() =>
            {
                Control.TextColor = ControlTextColor;
            });
        }
        #endregion


        #region Index of touch point
        int IndexAtPoint(Point point)
        {
            var cgPoint = new CGPoint(point.X, point.Y - Control.Frame.Y);
            //System.Diagnostics.Debug.WriteLine("cgPoint=[" + cgPoint + "] Frame=[" + Control.Frame + "]");

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
            var characterIndex = layoutManager.CharacterIndexForPoint(cgPoint, textContainer, ref partialFraction);

            //[self.layoutManager drawGlyphsForGlyphRange:NSMakeRange(0, self.textStorage.length) atPoint:CGPointMake(0, 0)];
            //layoutManager.DrawGlyphs(new NSRange(0,Control.AttributedText.Length),Control.Frame.Location);

            return (int)characterIndex;
        }
        #endregion
    }
}

