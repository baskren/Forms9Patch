using System;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Collections.Generic;
using Windows.UI.Text;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.UWP.LabeLRenderer))]
namespace Forms9Patch.UWP
{
    class LabeLRenderer : ViewRenderer<Label, TextBlock>
    {
        #region Debug support
        bool DebugCondition
            => P42.Utils.DebugExtensions.ConditionFunc?.Invoke(Element) ?? false;
        #endregion


        #region Fields

        #region Windows TextBlock label defaults
        internal static double _defaultNativeFontSize;
        internal static Windows.UI.Xaml.Media.Brush _defaultForeground;
        #endregion

        bool LayoutValid
        {
            get; 
            set;
        }

        Windows.Foundation.Size _lastAvailableSize = new Windows.Foundation.Size(0, 0);
        Xamarin.Forms.Size _lastElementSize = Xamarin.Forms.Size.Zero;
        Windows.Foundation.Size _lastInternalMeasure = new Windows.Foundation.Size(0, 0);
        internal static TextBlock _defaultTextBlock = new TextBlock();
        SharpDX.DirectWrite.FontMetrics _fontMetrics  = _defaultTextBlock.GetFontMetrics() ;
        #endregion


        #region Element & Property Change Handlers
        protected override void OnElementChanged(ElementChangedEventArgs<Forms9Patch.Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelF9PSize;
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var nativeControl = new TextBlock();
                    _defaultNativeFontSize = nativeControl.FontSize;
                    _defaultForeground = nativeControl.Foreground;
                    SetNativeControl(nativeControl);
                }
                e.NewElement.RendererSizeForWidthAndFontSize += LabelF9PSize;

                UpdateColor(Control);
                UpdateHorizontalAlign(Control);
                Control.UpdateLineBreakMode(e.NewElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            #region Forms9Patch.Label Properties
            if (e.PropertyName == Forms9Patch.Label.TextProperty.PropertyName || e.PropertyName == Forms9Patch.Label.HtmlTextProperty.PropertyName)
                ForceLayout();
            else if (e.PropertyName == Forms9Patch.Label.AutoFitProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.WidthProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.HeightProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.WidthRequestProperty.PropertyName
                //||e.PropertyName == Forms9Patch.Label.HeightRequestProperty.PropertyName
                )
                ForceLayout();
            else if (e.PropertyName == Forms9Patch.Label.LinesProperty.PropertyName)
                ForceLayout();
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize(Control);
            else if (e.PropertyName == Forms9Patch.Label.SynchronizedFontSizeProperty.PropertyName)
                UpdateSynchrnoizedFontSize(Control);
            #endregion

            #region Xamarin.Forms.Label properties
            else if (e.PropertyName == Forms9Patch.Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateVerticalAlign(Control);
            else if (e.PropertyName == Forms9Patch.Label.FontProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.Label.FontProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.Label.FontSizeProperty.PropertyName
                || e.PropertyName == Xamarin.Forms.Label.FontAttributesProperty.PropertyName)
                ForceLayout();
            else if (e.PropertyName == Forms9Patch.Label.TextColorProperty.PropertyName)
                UpdateColor(Control);
            else if (e.PropertyName == Forms9Patch.Label.LineBreakModeProperty.PropertyName)
                Control?.UpdateLineBreakMode(Element);
            else if (e.PropertyName == Forms9Patch.Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalAlign(Control);
            else if (e.PropertyName == Xamarin.Forms.Label.LineHeightProperty.PropertyName)
                UpdateLineHeight(Control);
            #endregion

            base.OnElementPropertyChanged(sender, e);
        }


        void UpdateHorizontalAlign(TextBlock control)
        {
            if (control!=null && Element is Forms9Patch.Label label)
                control.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment();
        }


        void UpdateVerticalAlign(TextBlock control)
        {
            if (control != null && Element is Forms9Patch.Label label)
            {
                control.VerticalAlignment = label.VerticalTextAlignment.ToNativeVerticalAlignment();
                if (label.Bounds.Width > 0 && label.Bounds.Height > 0)
                    ArrangeOverride(new Windows.Foundation.Size(label.Bounds.Width, label.Bounds.Height));
            }
        }


        void UpdateColor(TextBlock control)
        {
            if (control != null && Element?.TextColor is Xamarin.Forms.Color color)
            {
				if (color == Xamarin.Forms.Color.Default || color == default || color.IsDefault)
					control.ClearValue(TextBlock.ForegroundProperty);
				else
					control.Foreground = color.ToBrush();
			}
		}


        void UpdateLineHeight(TextBlock control)
        {
            if (control == null)
                return;
            if (Element.LineHeight >= 0)
                control.LineHeight = Element.LineHeight * control.FontSize;
            ForceLayout();
        }

        void UpdateSynchrnoizedFontSize(TextBlock control)
        {
            if (control!=null && Element is ILabel label && label.SynchronizedFontSize != control.FontSize)
                if (label.SynchronizedFontSize!=-1 && control.FontSize!=_defaultNativeFontSize)
                    ForceLayout();
        }



        void ForceLayout()
        {
            
            LayoutValid = false;
            if (Control != null)
            {
                //MeasureOverride(_lastAvailableSize);  // don't do this.  It will not render label correctly (it will make it tiny).
                //InvalidateArrange();
                InvalidateMeasure();  // this is needed to update header labels when BcItem.Title changes;
                ((Forms9Patch.Label)Element)?.InternalInvalidateMeasure();  // if this isn't here, app crashes when BcItem.Title changes;
            }
        }


        void UpdateMinFontSize(TextBlock control)
        {
            if (control != null && Element is Forms9Patch.Label label)
                if (label.MinFontSize > control.FontSize || control.Inlines.ExceedsFontSize(label.MinFontSize))
                    ForceLayout();
        }
        #endregion


        #region Xamarin GetDesiredSize

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Control is TextBlock control)
            {
                _fontMetrics = control.GetFontMetrics();
                var desiredSize = InternalMeasure(new Windows.Foundation.Size(widthConstraint, heightConstraint));
                var minSize = new Xamarin.Forms.Size(10, Element != null ? _fontMetrics.LineHeightForFontSize(Element.DecipheredMinFontSize()) : 10);
                var result = new SizeRequest(new Xamarin.Forms.Size(desiredSize.Width, desiredSize.Height), minSize);
                return result;
            }
            return new SizeRequest(Size.Zero);
        }
        #endregion


        #region Windows Arranging and Sizing
        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (Element == null)
                return finalSize;
            if (Control is TextBlock control)
            {
                if (LayoutValid)
                    control.Measure(_lastInternalMeasure);
                else
                    control.Measure(finalSize);
                PrivateArrange(finalSize);
            }
            return finalSize;
        }

        void PrivateArrange(Windows.Foundation.Size finalSize)
        {
            if (double.IsInfinity(finalSize.Width) || double.IsInfinity(finalSize.Height))
                return;

            if (Element is Forms9Patch.Label element && Control is TextBlock control)
            {
                var childHeight = Math.Max(0, Math.Min(element.Height, control.DesiredSize.Height));
                var rect = new Windows.Foundation.Rect();
                switch (element.VerticalTextAlignment)
                {
                    case Xamarin.Forms.TextAlignment.Start:
                        break;
                    default:
                    case Xamarin.Forms.TextAlignment.Center:
                        rect.Y = (int)((finalSize.Height - childHeight) / 2) - control.FontSize / 10;
                        break;
                    case Xamarin.Forms.TextAlignment.End:
                        rect.Y = finalSize.Height - childHeight;
                        break;
                }
                /*
                if (FormsGestures.VisualElementExtensions.FindAncestorOfType(element, typeof(SegmentButton)) != null)
                    if (element.VerticalTextAlignment == Xamarin.Forms.TextAlignment.Center && control.LineGap() == 0 && control.Descent() == 0 && control.LineHeight == 0)
                    {
                        var capHeight = control.CapHeight();
                        if (control.Ascent() == capHeight)
                        {
                            var delta = capHeight - control.XHeight();
                            rect.Y -= delta / 3;
                        }
                    }
                    */
                rect.Height = childHeight;
                rect.Width = finalSize.Width;
                control.Arrange(rect);
            }
        }



        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            Windows.Foundation.Size result = _lastInternalMeasure;
            if (Element is Forms9Patch.Label element)
            {
                if (element.Width > 0 && element.Height > 0)
                {
                    if (!LayoutValid || element.Width != _lastInternalMeasure.Width || element.Height != _lastInternalMeasure.Height)
                    {
                        InternalMeasure(new Windows.Foundation.Size(element.Width, element.Height));
                        result = _lastInternalMeasure;
                    }
                    //else if (element.Width < _lastInternalMeasure.Width || element.Height < _lastInternalMeasure.Height)
                    //{
                    //    InternalMeasure(new Windows.Foundation.Size(element.Width, element.Height));
                    //    result = _lastInternalMeasure;
                    //}
                }
                else if (!LayoutValid)
                    result = InternalMeasure(availableSize);
            }
            return result;
        }

        DateTime _lastMeasure = DateTime.MaxValue;
        protected Windows.Foundation.Size InternalMeasure(Windows.Foundation.Size availableSize, TextBlock control = null)
        {
            if (Element is Forms9Patch.Label element)
            {
                control = control ?? Control;
                if (control == null || availableSize.Width <= 0 || availableSize.Height <= 0)
                {
                    _lastAvailableSize = availableSize;
                    _lastElementSize = element.Bounds.Size;
                    return new Windows.Foundation.Size(0, 0);
                }
                // Next block was commented out because of failure to render BcNotes in Heights and Areas (in notes popup and on Disclaimer)
                // ... and, by doing so, it broke the picker and the keypad in Heights and Areas.
                if (LayoutValid
                    && control == Control
                    && _lastAvailableSize.Width == availableSize.Width
                    && _lastAvailableSize.Height == availableSize.Height
                    && _lastElementSize == element.Bounds.Size
                    && DateTime.Now - _lastMeasure < TimeSpan.FromSeconds(1))
                    return _lastInternalMeasure;
                
                element.SetIsInNativeLayout(true);

                var width = Math.Min(availableSize.Width, int.MaxValue / 2);
                var height = Math.Min(availableSize.Height, int.MaxValue / 2);
                var result = new Windows.Foundation.Size(width, height);

                if (control != null)
                {
                    // reset FontSize
                    var tmpFontSize = element.DecipheredFontSize();

                    if (element.SynchronizedFontSize > element.MinFontSize)
                        tmpFontSize = element.SynchronizedFontSize;
                    var minFontSize = element.DecipheredMinFontSize();
                    control.MaxLines = 0; // int.MaxValue / 3;
                    control.MaxWidth = double.PositiveInfinity;
                    control.MaxHeight = double.PositiveInfinity;
                    control.MinHeight = 0;
                    control.MinWidth = 0;

                    control.FontStyle = element.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
                    control.FontWeight = element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;

                    control.UpdateLineBreakMode(element);

                    control.SetAndFormatText(element, tmpFontSize);
                    control.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                    double tmpHt = control.DesiredSize.Height;
                    _fontMetrics = control.GetFontMetrics();
                    if (element.Lines == 0)
                    {
                        // do our best job to fit the existing space.
                        if (control.DesiredSize.Width - width > -Precision || control.DesiredSize.Height - height > -Precision)
                        {
                            if (DebugCondition)
                                System.Diagnostics.Debug.WriteLine(GetType() + ".");
                            tmpFontSize = ZeroLinesFit(element, control, minFontSize, tmpFontSize, width, height);
                            if (DebugCondition)
                                System.Diagnostics.Debug.WriteLine(GetType() + ".");
                            control.SetAndFormatText(element, tmpFontSize);
                            control.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                            tmpHt = control.DesiredSize.Height;
                        }
                    }
                    else if (element.AutoFit == AutoFit.Lines)
                    {
                        tmpHt = height;
                        if (height > int.MaxValue / 3)
                            tmpHt = height = _fontMetrics.HeightForLinesAtFontSize(element.Lines, tmpFontSize);
                        else
                        {
                            // set the font size to fit Label.Lines into the available height
                            var constrainedSize = control.DesiredSize;

                            tmpFontSize = _fontMetrics.FontSizeFromLinesInHeight(element.Lines, tmpHt);
                            tmpFontSize = FontExtensions.ClipFontSize(tmpFontSize, element);

                            control.SetAndFormatText(element, tmpFontSize);
                            control.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                            if (element.Lines > 1)
                            {
                                var unconstrainedSize = control.DesiredSize;
                                if (unconstrainedSize.Width > constrainedSize.Width)
                                {
                                    control.SetAndFormatText(element, tmpFontSize);
                                    control.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                                }
                            }
                        }
                    }
                    else if (element.AutoFit == AutoFit.Width)
                    {
                        if (control.ActualWidth > control.DesiredSize.Width || control.DesiredSize.Height / control.LineHeight > element.Lines)
                        {
                            tmpFontSize = WidthAndLinesFit(element, control, element.Lines + 1, minFontSize, tmpFontSize, width);
                            control.SetAndFormatText(element, tmpFontSize);
                            control.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                            tmpHt = control.DesiredSize.Height;
                        }
                    }

                    // none of these should happen so let's keep an eye out for it to be sure everything upstream is working
                    if (tmpFontSize > element.DecipheredFontSize())
                        throw new Exception("fitting somehow returned a tmpFontSize > label.FontSize");
                    if (tmpFontSize < element.DecipheredMinFontSize())
                        throw new Exception("fitting somehow returned a tmpFontSize < label.MinFontSize");
                    // the following doesn't apply when where growing 
                    //if (tmpFontSize > label.DecipheredMinFontSize() && (control.DesiredSize.Width > Math.Ceiling(w) || control.DesiredSize.Height > Math.Ceiling(Math.Max(availableSize.Height, label.Height))) )
                    //    throw new Exception("We should never exceed the available bounds if the FontSize is greater than label.MinFontSize");

                    // we needed the following in Android as well.  Xamarin layout really doesn't like this to be changed in real time.
                    if (element != null && Control == control)  // multipicker test was getting here with element and Control both null
                    {
                        if (tmpFontSize == element.FontSize || (element.FontSize == -1 && tmpFontSize == FontExtensions.DefaultFontSize()))
                            element.FittedFontSize = -1;
                        else if (Math.Abs(element.FittedFontSize - tmpFontSize) > 1)
                            element.FittedFontSize = tmpFontSize;
                    }

                    result = new Windows.Foundation.Size(Math.Ceiling(control.DesiredSize.Width), Math.Ceiling(tmpHt));
                    control.MaxLines = element.Lines;
                }

                element.SetIsInNativeLayout(false);

                if (control == Control)
                {
                    LayoutValid = true;
                    _lastAvailableSize = availableSize;
                    _lastElementSize = element.Bounds.Size;
                    //_lastAutoFit = label.AutoFit;
                    //_lastLines = label.Lines;
                    _lastMeasure = DateTime.Now;
                    _lastInternalMeasure = result;

                    PrivateArrange(availableSize);
                }
                return result;
            }
            return Windows.Foundation.Size.Empty;
        }
        #endregion


        #region Fitting

        Size LabelF9PSize(double widthConstraint, double fontSize)
        {
            if (Element is Forms9Patch.Label element && Control?.Copy() is TextBlock control)
            {
                //P42.Utils.DebugExtensions.Message(element,"ENTER widthConstraint=[" + widthConstraint + "] fontSize=[" + fontSize + "]");

                control.SetAndFormatText(element, fontSize);
                control.Measure(new Windows.Foundation.Size(widthConstraint, double.PositiveInfinity));

                var size = new Windows.Foundation.Size(control.DesiredSize.Width, control.DesiredSize.Height);

                var result = new Size(size.Width, size.Height);
                //P42.Utils.DebugExtensions.Message(element,"EXIT result=[" + result + "]");
                return result;
            }
            return new Size(10, 10);
        }


        // remember, we enter these methods with control's FontSize set to min or max.  

        const double Precision = 0.05f;

        double ZeroLinesFit(Label label, TextBlock control, double min, double max, double availWidth, double availHeight)
        {
            if (availHeight > int.MaxValue / 3)
                return max;
            if (availWidth > int.MaxValue / 3)
                return max;



            if (control.FontSize == max && availHeight >= control.DesiredSize.Height)
                return max;
            if (control.FontSize == min && availHeight <= control.DesiredSize.Height)
                return min;

            if (max - min < Precision)
                return min;

            var mid = (max + min) / 2.0;
            control.SetAndFormatText(label, mid);
            control.Measure(new Windows.Foundation.Size(availWidth-4, double.PositiveInfinity));
            var height = control.DesiredSize.Height;

            if (height <= _fontMetrics.AscentForFontSize(mid) + _fontMetrics.DescentForFontSize(mid) + _fontMetrics.LineGapForFontSize(mid))
                height = _fontMetrics.CapHeightForFontSize(mid) + _fontMetrics.DescentForFontSize(mid);

            if (height > availHeight)
                return ZeroLinesFit(label, control, min, mid, availWidth, availHeight);
            if (height < availHeight)
                return ZeroLinesFit(label, control, mid, max, availWidth, availHeight);
            return mid;
        }

        double WidthAndLinesFit(Label label, TextBlock control, int lines, double min, double max, double availWidth)
        {
            if (max - min < Precision)
            {
                if (control.FontSize != min)
                {
                    control?.SetAndFormatText(label, min);
                    control?.Measure(new Windows.Foundation.Size(availWidth-4, double.PositiveInfinity));
                }
                return min;
            }
            var mid = (max + min) / 2.0;
            control?.SetAndFormatText(label, mid);
            control?.Measure(new Windows.Foundation.Size(availWidth, double.PositiveInfinity));
            var midHeight = _fontMetrics.HeightForLinesAtFontSize(lines, mid);
            if (control.DesiredSize.Height > midHeight || control.DesiredSize.Width > availWidth)
                return WidthAndLinesFit(label, control, lines, min, mid, availWidth);
            return WidthAndLinesFit(label, control, lines, mid, max, availWidth);
        }

        double FindBestWidthForLineFit(Label label, TextBlock control, double fontSize, double heightTarget, double min, double max)
        {
            if (max - min < Precision)
                return min;
            var mid = (max + min) / 2.0;
            control.SetAndFormatText(label, fontSize);
            control.Measure(new Windows.Foundation.Size(mid, double.PositiveInfinity));
            var height = control.DesiredSize.Height;
            if (height <= heightTarget)
                return FindBestWidthForLineFit(label, control, fontSize, heightTarget, min, mid);
            else
                return FindBestWidthForLineFit(label, control, fontSize, heightTarget, mid, max);
        }
        #endregion



    }


}
