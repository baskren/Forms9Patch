using System;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using PCL.Utils;
using Windows.UI.Xaml;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.UWP.LabeLRenderer))]
namespace Forms9Patch.UWP
{
    class LabeLRenderer : ViewRenderer<Label, TextBlock>
    {
        #region Debug support
        bool DebugCondition
        {
            get
            {
                return (Element.Text != null && Element.Text.StartsWith("Żyłę;^`g ")) || (Element.HtmlText != null && Element.HtmlText.StartsWith("Żyłę;^`g "));
            }
        }

        void DebugMessage(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine(GetType() + "." + callerName + ": " + message);
        }

        void DebugGetDesiredRequest(string mark, double widthConstraint, double heightConstraint, SizeRequest request, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            DebugMessage(mark + " Constr=[" + widthConstraint + "," + heightConstraint + "] " + DebugControlSizes() + " result=[" + request + "]", callerName);
        }

        void DebugArrangeOverride(Windows.Foundation.Size finalSize, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            DebugMessage("FinSize=[" + finalSize + "] " + DebugControlSizes(), callerName);
        }

        void DebugMeasureOverride(string mark, Windows.Foundation.Size availableSize, Windows.Foundation.Size result, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            DebugMessage(mark + " Elmt.Sz=[" + Element.Bounds.Size + "] AvailSz=[" + availableSize + "] " + DebugControlSizes() + " result=[" + result + "]", callerName);
        }

        string DebugControlSizes()
        {
            if (Control != null)
            {
                var result = "Ctrl.MaxLines=[" + Control.MaxLines + "] .DesiredSz=[" + Control?.DesiredSize + "] .ActualSz=[" + Control?.ActualWidth + "," + Control?.ActualHeight + "] ";
                if (!double.IsNaN(Control.Width) || !double.IsNaN(Control.Height))
                    result += ".Sz=[" + Control?.Width + "," + Control.Height + "] ";
                if (!double.IsInfinity(Control.MaxWidth) || !double.IsInfinity(Control.MaxHeight))
                    result += ".MaxSz=[" + Control?.MaxWidth + "," + Control?.MaxHeight + "] ";
                if (Control.MinWidth != 0 || Control.MinHeight != 0)
                    result += ".MinSz=[" + Control?.MinWidth + "," + Control?.MinHeight + "] ";
                if (Control.LineHeight != 0)
                    result += ".LineHt=[" + Control.LineHeight + "] ";

                return result;
            }
            return null;
        }

        #endregion


        #region Fields
        //bool _fontApplied;
        //SizeRequest _perfectSize;
        //bool _perfectSizeValid;

        #region Windows TextBlock label defaults
        Windows.UI.Xaml.Media.FontFamily _defaultNativeFontFamily;
        double _defaultNativeFontSize;
        bool _defaultNativeFontIsBold;
        bool _defaultNativeFontIsItalics;
        #endregion

        bool _layoutValid;
        bool LayoutValid
        {
            get
            {
                return _layoutValid;
            }
            set
            {
                _layoutValid = value;
                if (DebugCondition)
                    System.Diagnostics.Debug.WriteLine("");
            }
        }

        Windows.Foundation.Size _lastAvailableSize = new Windows.Foundation.Size(0, 0);
        Windows.Foundation.Size _lastMeasureOverrideResult = new Windows.Foundation.Size(0, 0);
        AutoFit _lastAutoFit = (AutoFit) Forms9Patch.Label.AutoFitProperty.DefaultValue;
        int _lastLines = (int)Forms9Patch.Label.LinesProperty.DefaultValue;
        #endregion


        #region Element & Property Change Handlers
        protected override void OnElementChanged(ElementChangedEventArgs<Forms9Patch.Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                /*
                e.OldElement.SizeChanged -= Label_SizeChanged;
                if (Control != null)
                    Control.SizeChanged -= Control_SizeChanged;
                    */
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var nativeControl = new TextBlock();
                    _defaultNativeFontFamily = nativeControl.FontFamily;
                    _defaultNativeFontSize = nativeControl.FontSize;
                    _defaultNativeFontIsBold = nativeControl.FontWeight.Weight >= Windows.UI.Text.FontWeights.Bold.Weight;
                    _defaultNativeFontIsItalics = nativeControl.FontStyle != Windows.UI.Text.FontStyle.Normal;
                    SetNativeControl(nativeControl);
                }

                UpdateColor(Control);
                UpdateHorizontalAlign(Control);
                UpdateTextAndFont(Control);
                UpdateLineBreakMode(Control);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Forms9Patch.Label.TextProperty.PropertyName || e.PropertyName == Forms9Patch.Label.HtmlTextProperty.PropertyName)
                UpdateTextAndFont(Control);
            else if (e.PropertyName == Forms9Patch.Label.TextColorProperty.PropertyName)
                UpdateColor(Control);
            else if (e.PropertyName == Forms9Patch.Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalAlign(Control);
            else if (e.PropertyName == Forms9Patch.Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateVerticalAlign(Control);
            else if (e.PropertyName == Forms9Patch.Label.FontProperty.PropertyName)
                UpdateTextAndFont(Control);
            else if (e.PropertyName == Forms9Patch.Label.LineBreakModeProperty.PropertyName)
                UpdateLineBreakMode(Control);

            else if (e.PropertyName == Forms9Patch.Label.LinesProperty.PropertyName)
                ForceLayout(Control);
            //else if (e.PropertyName == Forms9Patch.Label.AutoFitProperty.PropertyName)
            //    ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize(Control);


            base.OnElementPropertyChanged(sender, e);
        }

        void UpdateHorizontalAlign(TextBlock textBlock)
        {
            //_perfectSizeValid = false;

            if (textBlock == null)
                return;

            Label label = Element;
            if (label == null)
                return;

            textBlock.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment();
        }

        void UpdateVerticalAlign(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            Label label = Element;
            if (label == null)
                return;

            //textBlock.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment();
            textBlock.VerticalAlignment = label.VerticalTextAlignment.ToNativeVerticalAlignment();
            ArrangeOverride(new Windows.Foundation.Size(Element.Bounds.Width, Element.Bounds.Height));
        }


        void UpdateColor(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            Label label = Element;
            if (label != null && label.TextColor != Color.Default)
                textBlock.Foreground = label.TextColor.ToBrush();
            else
                textBlock.ClearValue(TextBlock.ForegroundProperty);
        }

        void UpdateTextAndFont(TextBlock textBlock)
        {
            if (textBlock != null)
            {
                //_perfectSizeValid = false;
                Label label = Element;
                if (label == null)
                    return;
                LayoutValid = false;
                textBlock.SetAndFormatText(label);
            }
        }

        void UpdateLineBreakMode(TextBlock textBlock)
        {
            //_perfectSizeValid = false;

            if (textBlock == null)
                return;

            LayoutValid = false;
            switch (Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    textBlock.TextTrimming = TextTrimming.Clip;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.WordWrap:
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.TailTruncation:
                    textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.MiddleTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void ForceLayout(TextBlock textBlock)
        {
            LayoutValid = false;
            MeasureOverride(_lastAvailableSize);
        }

        void UpdateMinFontSize(TextBlock textBlock)
        {
            var label = Element;
            if (label == null)
                return;
            if (label.MinFontSize > Control.FontSize || Control.Inlines.ExceedsFontSize(label.MinFontSize))
                ForceLayout(textBlock);
        }
        #endregion


        #region Xamarin GetDesiredSize
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            /*
            DebugMessage("ENTER");

            if (!_perfectSizeValid)
            {
                _perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
                _perfectSize.Minimum = new Xamarin.Forms.Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
                _perfectSizeValid = true;
            }
            DebugMessage("\t\tPerfectSize=[" + _perfectSize + "]");

            var widthFits = widthConstraint >= _perfectSize.Request.Width;
            var heightFits = heightConstraint >= _perfectSize.Request.Height;

            if (widthFits && heightFits)
            {
                DebugGetDesiredRequest("EXIT A", widthConstraint, heightConstraint, _perfectSize);
                return _perfectSize;
            }

            var result = base.GetDesiredSize(widthConstraint, heightConstraint);

            DebugMessage("\t\tBase.GetDesiredSize=[" + result + "]");

            var tinyWidth = Math.Min(10, result.Request.Width);
            result.Minimum = new Size(tinyWidth, result.Request.Height);

            if (widthFits || Element.LineBreakMode == LineBreakMode.NoWrap)
            {
                DebugGetDesiredRequest("EXIT B", widthConstraint, heightConstraint, result);
                return result;
            }

            bool containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

            if (containerIsNotInfinitelyWide)
            {
                bool textCouldHaveWrapped = Element.LineBreakMode == LineBreakMode.WordWrap || Element.LineBreakMode == LineBreakMode.CharacterWrap;
                bool textExceedsContainer = result.Request.Width > widthConstraint;

                if (textExceedsContainer || textCouldHaveWrapped)
                {
                    var expandedWidth = Math.Max(tinyWidth, widthConstraint);
                    result.Request = new Size(expandedWidth, result.Request.Height);
                }
            }



            DebugGetDesiredRequest("EXIT C", widthConstraint, heightConstraint, result);
            return result;
            */

            var desiredSize = MeasureOverride(new Windows.Foundation.Size(widthConstraint, heightConstraint));
            var minSize = new Xamarin.Forms.Size(10, FontExtensions.LineHeightForFontSize(Element.DecipheredMinFontSize()));
            return new SizeRequest(new Xamarin.Forms.Size(desiredSize.Width, desiredSize.Height), minSize);

        }
        #endregion


        #region Windows Arranging and Sizing
        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (Element == null)
                return finalSize;

            var textBlock = Control;
            if (textBlock == null)
                return finalSize;

            DebugMessage("ENTER FontSize=["+textBlock.FontSize+"] BaseLineOffset=["+textBlock.BaselineOffset+"] LineHeight=["+textBlock.LineHeight+"]");
            DebugArrangeOverride(finalSize);

            double topPadding =  textBlock.BaselineOffset - textBlock.FontSize;

            double childHeight = Math.Max(0, Math.Min(Element.Height, Control.DesiredSize.Height));
            var rect = new Windows.Foundation.Rect();

            switch (Element.VerticalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Start:
                    break;
                default:
                case Xamarin.Forms.TextAlignment.Center:
                    rect.Y = (int)((finalSize.Height - childHeight) / 2);
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    rect.Y = finalSize.Height - childHeight;
                    break;
            }
            rect.Y -= topPadding;
            rect.Height = childHeight + topPadding;
            rect.Width = finalSize.Width;
            Control.Arrange(rect);

            DebugArrangeOverride(finalSize);
            DebugMessage("EXIT");
            return finalSize;
        }

        private void Control_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
        DebugMessage("Element.Size=[" + Element.Width + "," + Element.Height + "] ActualSize=[" + ActualWidth + "," + ActualHeight + "] Control.Size=[" + Control?.Width + "," + Control?.Height + "] Control.ActualSize=[" + Control?.ActualWidth + "," + Control?.ActualHeight + "] ");
        }

        private void Label_SizeChanged(object sender, EventArgs e)
        {
            DebugMessage("Element.Size=[" +Element.Width+","+Element.Height+"] ActualSize=["+ActualWidth+","+ActualHeight+"] Control.Size=["+Control?.Width+","+Control?.Height+"] Control.ActualSize=["+Control?.ActualWidth+","+Control?.ActualHeight+"] ");
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
		{
            var label = Element;
            var textBlock = Control;

            if (label==null || textBlock==null || availableSize.Width==0 || availableSize.Height == 0)
				return new Windows.Foundation.Size(0, 0);

            if (LayoutValid && _lastAvailableSize == availableSize)
                return _lastMeasureOverrideResult;

            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine("");

            DebugMessage("ENTER");
            //Element.IsInNativeLayout = true;
            label.SetIsInNativeLayout(true);

            double width = Math.Max(0, label.Width);
            double height = Math.Max(0, label.Height);
            var result = new Windows.Foundation.Size(width, height);
            if (textBlock != null)
			{
                
				double w = label.Width;
				double h = label.Height;
				if (w == -1)
					w = availableSize.Width;
				if (h == -1)
					h = availableSize.Height;
				w = Math.Max(0, w);
				h = Math.Max(0, h);
                


                // reset FontSize

                var tmpFontSize = label.DecipheredFontSize();
                var minFontSize = label.DecipheredMinFontSize();
                textBlock.MaxLines = int.MaxValue/3;

                double tmpHt = -1;

                if (label.Lines==0)
                {
                    // do our best job to fit the existing space.
                    textBlock.SetAndFormatText(label, tmpFontSize);
                    textBlock?.Measure(new Windows.Foundation.Size(w, double.PositiveInfinity));

                    if (textBlock.DesiredSize.Width - w > Precision || textBlock.DesiredSize.Height - h > Precision)
                        tmpFontSize = ZeroLinesFit(label, textBlock, minFontSize, tmpFontSize, w, h);
                }
                else if (label.AutoFit == AutoFit.Lines)
                {
                    /*
                    if (_lastAutoFit == AutoFit.Lines && _lastAvailableSize.Height==double.PositiveInfinity && LayoutValid)
                    {
                        textBlock.MaxLines = label.Lines;
                        return _lastMeasureOverrideResult;
                    }
                    */
                    
                    // if we have infinite height, we need to return the size of the text with the current font.



                    tmpHt = h;
                    if (availableSize.Height > int.MaxValue / 3)
                        tmpHt = h = label.Lines * FontExtensions.LineHeightForFontSize(tmpFontSize);
                    else // set the font size to fit Label.Lines into the available height
                        tmpFontSize = FontExtensions.FontSizeFromLineHeight( h / label.Lines );

                    tmpFontSize = FontExtensions.ClipFontSize(tmpFontSize, label);

                    textBlock?.SetAndFormatText(label, tmpFontSize);
                    textBlock?.Measure(new Windows.Foundation.Size(w, h));
                }
                else if (label.AutoFit == AutoFit.Width)
                {
                    textBlock.SetAndFormatText(label, tmpFontSize);
                    textBlock?.Measure(new Windows.Foundation.Size(w, double.PositiveInfinity));
                    if (textBlock.DesiredSize.Height / textBlock.LineHeight > label.Lines)
                        tmpFontSize = WidthAndLinesFit(label, textBlock, label.Lines, minFontSize, tmpFontSize, w);
                }
                else // autofit is off!
                {
                    textBlock.SetAndFormatText(label, tmpFontSize);
                    textBlock?.Measure(new Windows.Foundation.Size(w, double.PositiveInfinity));
                }

                // none of these should happen so let's keep an eye out for it to be sure everything upstream is working
                if (tmpFontSize > label.DecipheredFontSize())
                      throw new Exception("fitting somehow returned a tmpFontSize > label.FontSize");
                if (tmpFontSize < label.DecipheredMinFontSize())
                    throw new Exception("fitting somehow returned a tmpFontSize < label.MinFontSize");
                // the following doesn't apply when where growing 
                //if (tmpFontSize > label.DecipheredMinFontSize() && (textBlock.DesiredSize.Width > Math.Ceiling(w) || textBlock.DesiredSize.Height > Math.Ceiling(Math.Max(availableSize.Height, label.Height))) )
                //    throw new Exception("We should never exceed the available bounds if the FontSize is greater than label.MinFontSize");




                Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
                 {
                     label.ActualFontSize = tmpFontSize;
                     return false;
                 });

                //result.Height = tmpHt > 0 ? tmpHt : textBlock.DesiredSize.Height;
                result = new Windows.Foundation.Size(Math.Ceiling(textBlock.DesiredSize.Width),Math.Ceiling(tmpHt > -1 ? tmpHt : textBlock.DesiredSize.Height));
                
                DebugMessage("result=[" + result + "] FontSize=["+textBlock.FontSize+"] LineHeight=["+textBlock.LineHeight+"]");

                textBlock.MaxLines = label.Lines;

            }

            //Element.IsInNativeLayout = false;
            label.SetIsInNativeLayout(false);

            DebugMessage("EXIT");

            LayoutValid = true;
            _lastAvailableSize = availableSize;
            _lastMeasureOverrideResult = result;
            _lastAutoFit = label.AutoFit;
            _lastLines = label.Lines;
			return result;
		}
        #endregion


        #region LineHeight estimation
        #endregion

        #region Fitting

        // remember, we enter these methods with textBlock's FontSize set to min or max.  

        const double Precision = 0.05f;

        double ZeroLinesFit(Label label, TextBlock textBlock, double min, double max, double availWidth, double availHeight)
        {
            if (availHeight > int.MaxValue / 3)
                return max;
            if (availWidth > int.MaxValue / 3)
                return max;



            if (textBlock.FontSize == max && availHeight >= textBlock.DesiredSize.Height)
                return max;
            if (textBlock.FontSize == min && availHeight <= textBlock.DesiredSize.Height)
                return min;

            if (max - min < Precision)
                return min;

            var mid = (max + min) / 2.0;
            textBlock?.SetAndFormatText(label, mid);
            textBlock?.Measure(new Windows.Foundation.Size(availWidth, double.PositiveInfinity));
            var height = textBlock.DesiredSize.Height;
            if (height > availHeight)
                return ZeroLinesFit(label, textBlock, min, mid, availWidth, availHeight);
            if (height < availHeight)
                return ZeroLinesFit(label, textBlock, mid, max, availWidth, availHeight);
            return mid;
        }

        double WidthAndLinesFit(Label label, TextBlock textBlock, int lines, double min, double max, double availWidth)
        {
            if (max - min < Precision)
            {
                //DebugMessage("Precision reached: result=[" + min + "]");
                if (textBlock.FontSize!=min)
                {
                    textBlock?.SetAndFormatText(label, min);
                    textBlock?.Measure(new Windows.Foundation.Size(availWidth, double.PositiveInfinity));
                }
                return min;
            }

            var mid = (max + min) / 2.0;
            textBlock?.SetAndFormatText(label, mid);
            textBlock?.Measure(new Windows.Foundation.Size(availWidth, double.PositiveInfinity));


            var renderedLines = textBlock.DesiredSize.Height / textBlock.LineHeight;
            //DebugMessage("mid=["+mid+"] renderedLines=[" + renderedLines + "] DesiredSize=[" + textBlock.DesiredSize.Height + "] LineHeight=[" + textBlock.LineHeight + "]");

            if (Math.Round(renderedLines) > lines)
                return WidthAndLinesFit(label, textBlock, lines, min, mid, availWidth);
            return WidthAndLinesFit(label, textBlock, lines, mid, max, availWidth);
        }


        #endregion
    }
}
