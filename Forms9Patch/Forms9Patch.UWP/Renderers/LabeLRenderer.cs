using System;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.UWP.LabeLRenderer))]
namespace Forms9Patch.UWP
{
    class LabeLRenderer : ViewRenderer<Label, TextBlock>
    {
        #region Debug support
        bool DebugCondition => Element?.HtmlText == "Bolt Parameters:" || Element?.HtmlText == "Lag Screw Parameters:" || Element?.HtmlText == "Wood Screw Parameters:" || Element?.HtmlText == "Nail/Spike Parameters:" || Element?.HtmlText == "Ring Shank Nail Parameters:"; /*
        {
            get
            {
                return (Element?.Parent?.ToString() is string str && str.StartsWith("[Bc3.Forms.BcItemTextValueLabel."));

                //if (Element.Parent.ToString() == "Bc3.Forms.BcItemTextValueLabel")
                //string labelTextStart = "I4";
                //if ((Element?.Text != null && Element.Text.StartsWith(labelTextStart)) || (Element?.HtmlText != null && Element.HtmlText.StartsWith(labelTextStart)) )
               // {
                 //   var parent = Element.Parent;
                //}
                //return (Element?.Text != null && Element.Text.StartsWith(labelTextStart)) || (Element?.HtmlText != null && Element.HtmlText.StartsWith(labelTextStart));

            }
        } */

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
            DebugMessage(mark + " Elmt.Sz=[" + Element?.Bounds.Size + "] AvailSz=[" + availableSize + "] " + DebugControlSizes() + " result=[" + result + "]", callerName);
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

        bool LayoutValid
        {
            get; 
            set;
        }

        Windows.Foundation.Size _lastAvailableSize = new Windows.Foundation.Size(0, 0);
        Xamarin.Forms.Size _lastElementSize = Xamarin.Forms.Size.Zero;
        Windows.Foundation.Size _lastMeasureOverrideResult = new Windows.Foundation.Size(0, 0);
        AutoFit _lastAutoFit = (AutoFit)Forms9Patch.Label.AutoFitProperty.DefaultValue;
        int _lastLines = (int)Forms9Patch.Label.LinesProperty.DefaultValue;
        internal static TextBlock _defaultTextBlock = new TextBlock();
        SharpDX.DirectWrite.FontMetrics _fontMetrics  = _defaultTextBlock.GetFontMetrics() ;
        bool newElement;
        #endregion


        #region Element & Property Change Handlers
        protected override void OnElementChanged(ElementChangedEventArgs<Forms9Patch.Label> e)
        {
            //DebugMessage("OnElementChanged");

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelXamarinSize;
                e.OldElement.SizeChanged -= OnElementSizeChanged;
                if (Control != null)
                    Control.SizeChanged -= OnControlSizeChanged;
                    
            }
            if (e.NewElement != null)
            {
                newElement = true;
                if (Control == null)
                {
                    var nativeControl = new TextBlock();
                    _defaultNativeFontFamily = nativeControl.FontFamily;
                    _defaultNativeFontSize = nativeControl.FontSize;
                    _defaultNativeFontIsBold = nativeControl.FontWeight.Weight >= Windows.UI.Text.FontWeights.Bold.Weight;
                    _defaultNativeFontIsItalics = nativeControl.FontStyle != Windows.UI.Text.FontStyle.Normal;
                    SetNativeControl(nativeControl);
                }
                e.NewElement.RendererSizeForWidthAndFontSize += LabelXamarinSize;
                e.NewElement.SizeChanged += OnElementSizeChanged;
                if (Control != null)
                    Control.SizeChanged += OnControlSizeChanged;

                //Control.LayoutUpdated += (s, ex) => DebugMessage("Control.LayoutUpdated");
                //Control.Loading += (s, ex) => DebugMessage("Control.Loading");
                //Control.Loaded += (s, ex) => DebugMessage("Contorl.Loaded");
                //Control.Unloaded += (s, ex) => DebugMessage("Control.Unloaded");

                UpdateColor(Control);
                UpdateHorizontalAlign(Control);
                //UpdateTextAndFont(Control);
                UpdateLineBreakMode(Control);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //DebugMessage("property=[" + e.PropertyName + "]");

            //if (e.PropertyName == "Renderer" && DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            if (e.PropertyName == Forms9Patch.Label.TextProperty.PropertyName || e.PropertyName == Forms9Patch.Label.HtmlTextProperty.PropertyName)
                //UpdateTextAndFont(Control);
                ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.TextColorProperty.PropertyName)
                UpdateColor(Control);
            else if (e.PropertyName == Forms9Patch.Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalAlign(Control);
            else if (e.PropertyName == Forms9Patch.Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateVerticalAlign(Control);
            else if (e.PropertyName == Forms9Patch.Label.FontProperty.PropertyName)
                //UpdateTextAndFont(Control);
                ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.LineBreakModeProperty.PropertyName)
                //UpdateLineBreakMode(Control);
                ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.LinesProperty.PropertyName)
                ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.AutoFitProperty.PropertyName)
                ForceLayout(Control);
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize(Control);
            else if (e.PropertyName == Forms9Patch.Label.SynchronizedFontSizeProperty.PropertyName)
                UpdateSynchrnoizedFontSize(Control);


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
            if (Element.Bounds.Width > 0 && Element.Bounds.Height > 0)
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

        void UpdateSynchrnoizedFontSize(TextBlock textBlock)
        {
            if (Element is ILabel label && label.SynchronizedFontSize != textBlock.FontSize)
                if (label.SynchronizedFontSize!=-1 && textBlock.FontSize!=_defaultNativeFontSize)
                ForceLayout(textBlock);
        }

        void UpdateLineBreakMode(TextBlock textBlock)
        {
            //_perfectSizeValid = false;

            if (textBlock == null || Element==null)
                return;

            LayoutValid = false;
            switch (Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.WordWrap:
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                    break;
                case LineBreakMode.CharacterWrap:
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                    break;
                case LineBreakMode.TailTruncation:
                    textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                    textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                    break;
                case LineBreakMode.MiddleTruncation:
                    // TODO: This truncates at the end.
                    textBlock.TextTrimming = TextTrimming.WordEllipsis;
                    textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void ForceLayout(TextBlock textBlock)
        {
            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine("");

            LayoutValid = false;
            //MeasureOverride(_lastAvailableSize);  // don't do this.  It will not render label correctly (it will make it tiny).
            //InvalidateArrange();
            InvalidateMeasure();  // this is needed to update header labels when BcItem.Title changes;
            //MeasureOverride(new Windows.Foundation.Size(Element.Width, Element.Height));
            ((Forms9Patch.Label)Element).InternalInvalidateMeasure();  // if this isn't here, app crashes when BcItem.Title changes;
            //((VisualElement)Element)?.InvalidateMeasureNonVirtual(Xamarin.Forms.Internals.InvalidationTrigger.MeasureChanged);
            //System.Diagnostics.Debug.WriteLine("ForceLayout [" + Element.HtmlText + "]");
        }

        void UpdateMinFontSize(TextBlock textBlock)
        {
            var label = Element;
            if (label == null || textBlock==null)
                return;
            if (label.MinFontSize > Control.FontSize || Control.Inlines.ExceedsFontSize(label.MinFontSize))
                ForceLayout(textBlock);
        }
        #endregion


        #region Xamarin GetDesiredSize

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //DebugMessage("GetDesiredSize ENTER(" + widthConstraint + "," + heightConstraint + ")");
            var desiredSize = MeasureOverride(new Windows.Foundation.Size(widthConstraint, heightConstraint));
            //var minSize = new Xamarin.Forms.Size(10, Element!=null ? FontExtensions.LineHeightForFontSize(Element.DecipheredMinFontSize()):10);
            var minSize = new Xamarin.Forms.Size(10, Element != null ? _fontMetrics.LineHeightForFontSize(Element.DecipheredMinFontSize()) : 10);
            //DebugMessage("GetDesiredSize EXIT(" + desiredSize + ")");
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

            //DebugMessage("ArrangeOverride ENTER FontSize=[" + textBlock.FontSize + "] BaseLineOffset=[" + textBlock.BaselineOffset + "] LineHeight=[" + textBlock.LineHeight + "] VerticalAlignment=["+Element.VerticalTextAlignment+"] VerticalOptions=["+Element.VerticalOptions+"]");
            //DebugArrangeOverride(finalSize);

            //if (DebugCondition && (finalSize.Width > Element.Width || finalSize.Height > Element.Height) && Element.FittedFontSize > Element.MinFontSize)
            //    System.Diagnostics.Debug.WriteLine("finalSize a bit big!");
            //    MeasureOverride(finalSize);

            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            /*
            double childHeight = Math.Max(0, Math.Min(Element.Height, Control.DesiredSize.Height));
            var rect = new Windows.Foundation.Rect();

            //var yOffset = _fontMetrics.AscentForFontSize(textBlock.FontSize) - _fontMetrics.CapHeightForFontSize(textBlock.FontSize);

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


            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine("");

            //rect.Y += yOffset;

            rect.Height = childHeight;
            rect.Width = finalSize.Width;

            Control.Arrange(rect);
            */
            PrivateArrange(finalSize);

            //DebugArrangeOverride(finalSize);
            //DebugMessage("ArrangeOverride EXIT");
            return finalSize;
        }

        void PrivateArrange(Windows.Foundation.Size finalSize)
        {
            double childHeight = Math.Max(0, Math.Min(Element.Height, Control.DesiredSize.Height));
            var rect = new Windows.Foundation.Rect();

            //var yOffset = _fontMetrics.AscentForFontSize(textBlock.FontSize) - _fontMetrics.CapHeightForFontSize(textBlock.FontSize);

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

            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            //var offset = _fontMetrics.LineGapForFontSize(Control.FontSize); //_fontMetrics.AscentForFontSize(Control.FontSize) - _fontMetrics.CapHeightForFontSize(Control.FontSize);
            //System.Diagnostics.Debug.WriteLine("Control.FontSize: ["+Control.FontSize+"] offset: " + offset);
            //rect.Y -= offset;

            //_fontMetrics.DebugMetricsForFontSize(Control.FontSize);
            //Control.DebugMetricsForLabel();
            if (FormsGestures.VisualElementExtensions.FindAncestorOfType(Element,typeof(SegmentButton))!=null)
            if (Element.VerticalTextAlignment==Xamarin.Forms.TextAlignment.Center && Control.LineGap() == 0 && Control.Descent()==0 && Control.LineHeight==0)
            {
                var capHeight = Control.CapHeight();
                if (Control.Ascent()==capHeight)
                {
                    var delta = capHeight - Control.XHeight();
                    rect.Y -= delta/3;
                }
            }
            
            //Control.LineHeight = Control.LineHeight();

            rect.Height = childHeight;
            rect.Width = finalSize.Width;

            Control.Arrange(rect);

        }

        private void OnControlSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            //DebugMessage("Element.Size=[" + Element.Width + "," + Element.Height + "] ActualSize=[" + ActualWidth + "," + ActualHeight + "] Control.Size=[" + Control?.Width + "," + Control?.Height + "] Control.ActualSize=[" + Control?.ActualWidth + "," + Control?.ActualHeight + "] ");
            //DebugMessage("e.NewSize=["+e.NewSize+"]");
            //Control.UpdateLayout();
            //MeasureOverride(e.NewSize);
        }

        private void OnElementSizeChanged(object sender, EventArgs e)
        {
            //DebugMessage("OnElementSizeChanged  Element.Size=[" + Element.Width + "," + Element.Height + "] ActualSize=[" + ActualWidth + "," + ActualHeight + "] Control.Size=[" + Control?.Width + "," + Control?.Height + "] Control.ActualSize=[" + Control?.ActualWidth + "," + Control?.ActualHeight + "] ");
            //Element.Layout(Element.Bounds);

            // the below line made a big difference in Heights & Areas keypad button labels.  
            MeasureOverride(new Windows.Foundation.Size(Element.Width, Element.Height));
        }

        /*
        private Size MeasureTextSize(string text, Microsoft.Graphics.Canvas.Text.CanvasTextFormat textFormat, float limitedToWidth = 0.0f, float limitedToHeight = 0.0f)
        {
            var device = Microsoft.Graphics.Canvas.CanvasDevice.GetSharedDevice();

            var layout = new Microsoft.Graphics.Canvas.Text.CanvasTextLayout(device, text, textFormat, limitedToWidth, limitedToHeight);

            var width = layout.DrawBounds.Width;
            var height = layout.DrawBounds.Height;

            return new Size(width, height);
        }
        */

        DateTime _lastMeasure = DateTime.MaxValue;
        int _measureOverrideInvocation;
        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine("MeasureOverride =" + Element.HtmlText);


            var label = Element;
            var textBlock = Control;
            /*
            DebugMessage("[" + _measureOverrideInvocation + "] MeasureOverride pre-Enter availableSize=[" + availableSize+"] ElemmentSize=["+Element.Bounds.Size+"]  PageSize=["+Xamarin.Forms.Application.Current.MainPage.Bounds.Size+"]");
            DebugMessage("[" + _measureOverrideInvocation + "] \t\t availWidth>=Page.Width=[" + (Math.Round(availableSize.Width) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Width)) + "]");
            DebugMessage("[" + _measureOverrideInvocation + "] \t\t availHeight>=Page.Height=[" + (Math.Round(availableSize.Height) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Height)) + "]");
            DebugMessage("[" + _measureOverrideInvocation + "] \t\t Element.Parent=["+ Element.Parent + "]");
            DebugMessage("[" + _measureOverrideInvocation + "] \t\t MeasureOverride FontSize=[" + textBlock.FontSize + "] BaseLineOffset=[" + textBlock.BaselineOffset + "] LineHeight=[" + textBlock.LineHeight + "]");
*/
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");



            if (label == null || textBlock == null || availableSize.Width == 0 || availableSize.Height == 0)
                return new Windows.Foundation.Size(0, 0);

            //if (Double.IsInfinity(availableSize.Height) && Double.IsInfinity(availableSize.Width))
            //    LayoutValid = false;

            if (LayoutValid &&  _lastAvailableSize.Width<=availableSize.Width && _lastAvailableSize.Height<=availableSize.Height && _lastElementSize == Element.Bounds.Size && DateTime.Now - _lastMeasure < TimeSpan.FromSeconds(1))
                return _lastMeasureOverrideResult;

            //_lastAvailableSize = availableSize;
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            //DebugMessage("[" + _measureOverrideInvocation + "] MeasureOverride ENTER availableSize=[" + availableSize + "]");
            //Element.IsInNativeLayout = true;
            label.SetIsInNativeLayout(true);

            double width = (Math.Round(availableSize.Width) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Width)) && label.Width > 0 ? Math.Min(label.Width, availableSize.Width) : availableSize.Width;
            double height = (Math.Round(availableSize.Height) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Height)) && label.Height > 0 ? Math.Min(label.Height, availableSize.Height) : availableSize.Height;

            
            if (Double.IsInfinity(availableSize.Width) && (label.Width < 0 || !newElement) )
                width = availableSize.Width;
            if (Double.IsInfinity(availableSize.Height) && (label.Height < 0 || !newElement))
                height = availableSize.Height;

            newElement = false;


            if (Element.Width > 0 && Element.Height > 0 && !Double.IsInfinity(width) && !Double.IsInfinity(height))  // This line was causing UWP to fail to correctly update width of BcOperandLabel during editing.
            //if (Element.Width > width && Element.Height > height && !Double.IsInfinity(width) && !Double.IsInfinity(height))
            {
                width = Math.Max(Element.Width,width);
                height = Math.Max(Element.Height,height);
            }

            //DebugMessage("[" + _measureOverrideInvocation + "] \t\t width=[" + width+"] height=["+height+"]");
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            //double width = !double.IsInfinity(availableSize.Width) && label.Width > 0 ? Math.Min(label.Width, availableSize.Width) : availableSize.Width;
            //double height = !double.IsInfinity(availableSize.Height) && label.Height > 0 ? Math.Min(label.Height, availableSize.Height) : availableSize.Height;
            //double width  = availableSize.Width;
            //double height = availableSize.Height;

            var result = new Windows.Foundation.Size(width, height);

            if (textBlock != null)
            {
                // reset FontSize
                var tmpFontSize = label.DecipheredFontSize();

                //if (DebugCondition)
                //    System.Diagnostics.Debug.WriteLine("");

                if (Element.SynchronizedFontSize > Element.MinFontSize)
                    tmpFontSize = Element.SynchronizedFontSize;
                var minFontSize = label.DecipheredMinFontSize();
                textBlock.MaxLines = 0; // int.MaxValue / 3;
                textBlock.MaxWidth = double.PositiveInfinity;
                textBlock.MaxHeight = double.PositiveInfinity;
                textBlock.MinHeight = 0;
                textBlock.MinWidth = 0;
                //textBlock.LineStackingStrategy = LineStackingStrategy.
                //  textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                UpdateLineBreakMode(textBlock);

                double tmpHt = -1;

                textBlock.SetAndFormatText(label, tmpFontSize);
                textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                _fontMetrics = textBlock.GetFontMetrics();
                if (label.Lines == 0)
                {
                    // do our best job to fit the existing space.
                    if (textBlock.DesiredSize.Width - width > Precision || textBlock.DesiredSize.Height - height > Precision)
                    {
                        tmpFontSize = ZeroLinesFit(label, textBlock, minFontSize, tmpFontSize, width, height);

                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                    }
                }
                else if (label.AutoFit == AutoFit.Lines)
                {
                    tmpHt = height;
                    if (height > int.MaxValue / 3)
                        //tmpHt = height = label.Lines * (_fontMetrics.LineHeightForFontSize(tmpFontSize) + ;
                        tmpHt = height = _fontMetrics.HeightForLinesAtFontSize(label.Lines, tmpFontSize);
                    else
                    {// set the font size to fit Label.Lines into the available height
                        //tmpFontSize = _fontMetrics.FontSizeFromLineHeight(height / label.Lines);
                        tmpFontSize = _fontMetrics.FontSizeFromLinesInHeight(label.Lines, tmpHt);

                        tmpFontSize = FontExtensions.ClipFontSize(tmpFontSize, label);

                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                    }
                }
                else if (label.AutoFit == AutoFit.Width)
                {
                    //if (textBlock.DesiredSize.Height / textBlock.LineHeight > label.Lines)
                    if (textBlock.ActualWidth > textBlock.DesiredSize.Width || textBlock.DesiredSize.Height / textBlock.LineHeight > label.Lines)
                    {
                        tmpFontSize = WidthAndLinesFit(label, textBlock, label.Lines, minFontSize, tmpFontSize, width);

                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                    }

                }
                // autofit is off!  
                // No need to do anything at the moment.  Will textBlock.SetAndFormat and textBlock.Measure at the end
                //{
                //    textBlock.SetAndFormatText(label, tmpFontSize);
                //    textBlock?.Measure(new Windows.Foundation.Size(w, double.PositiveInfinity));
                //}

                // none of these should happen so let's keep an eye out for it to be sure everything upstream is working
                if (tmpFontSize > label.DecipheredFontSize())
                    throw new Exception("fitting somehow returned a tmpFontSize > label.FontSize");
                if (tmpFontSize < label.DecipheredMinFontSize())
                    throw new Exception("fitting somehow returned a tmpFontSize < label.MinFontSize");
                // the following doesn't apply when where growing 
                //if (tmpFontSize > label.DecipheredMinFontSize() && (textBlock.DesiredSize.Width > Math.Ceiling(w) || textBlock.DesiredSize.Height > Math.Ceiling(Math.Max(availableSize.Height, label.Height))) )
                //    throw new Exception("We should never exceed the available bounds if the FontSize is greater than label.MinFontSize");



                // we needed the following in Android as well.  Xamarin layout really doesn't like this to be changed in real time.
                if (Element != null && Control != null)  // multipicker test was getting here with Element and Control both null
                {
                    if (tmpFontSize == Element.FontSize || (Element.FontSize == -1 && tmpFontSize == FontExtensions.DefaultFontSize()))
                    {
                        //if (DebugCondition)
                        //    System.Diagnostics.Debug.WriteLine("");
                        Element.FittedFontSize = -1;
                    }
                    else if (Math.Abs(Element.FittedFontSize - tmpFontSize) > 1)
                    {
                        //if (DebugCondition)
                        //    System.Diagnostics.Debug.WriteLine("");
                        Element.FittedFontSize = tmpFontSize;
                    }
                }
                //DebugMessage("[" + _measureOverrideInvocation + "] MeasureOverride Element.FittedFontSize=[" + Element.FittedFontSize + "]");

                /*
                var syncFontSize = ((ILabel)label).SynchronizedFontSize;
                DebugMessage("[" + _measureOverrideInvocation + "] syncFontSize=[" + syncFontSize+"]");
                if (syncFontSize >= 0 && tmpFontSize != syncFontSize)
                {
                    tmpHt = -1;
                    textBlock.SetAndFormatText(label, syncFontSize);
                    textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                }
                else
                {
                    textBlock.SetAndFormatText(label, tmpFontSize);
                    textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                }
                */
                result = new Windows.Foundation.Size(Math.Ceiling(textBlock.DesiredSize.Width), Math.Ceiling(tmpHt > -1 ? tmpHt : textBlock.DesiredSize.Height));

                //if (DebugCondition && label.Width>0 && label.Height > 0 &&(textBlock.DesiredSize.Width > label.Width || textBlock.DesiredSize.Height > label.Height))
                //    System.Diagnostics.Debug.WriteLine("");
                //DebugMessage("[" + _measureOverrideInvocation + "] availableSize=[" + availableSize + "] result=[" + result + "] FontSize=[" + textBlock.FontSize + "] LineHeight=[" + textBlock.LineHeight + "] ");
                //System.Diagnostics.Debug.WriteLine("[" + _measureOverrideInvocation + "] MeasureOverride [" + (Element.Text ?? Element.HtmlText)+"] availableSize=[" + availableSize + "] result=[" + result + "] FontSize=[" + textBlock.FontSize + "] LineHeight=[" + textBlock.LineHeight + "] ");

                textBlock.MaxLines = label.Lines;

                // No need for this here, it is overridden by ArrangeOverride
                //PrivateArrange(textBlock.DesiredSize, -20);
            }

            label.SetIsInNativeLayout(false);

            //DebugMessage("[" + _measureOverrideInvocation + "] MeasureOverride EXIT");

            LayoutValid = true;
            _lastAvailableSize = availableSize;
            _lastElementSize = Element.Bounds.Size;
            _lastAutoFit = label.AutoFit;
            _lastLines = label.Lines;
            _lastMeasure = DateTime.Now;
            _lastMeasureOverrideResult = result;

            if (DebugCondition)
                _measureOverrideInvocation++;

            return result;
        }
        #endregion


        #region Keypad issue
        /*
        void Layout()
        {
            if (Element.IsVisible)
            {
                if (Element.Width > -1 && Element.Height > -1 && (Element.Width != _lastControlState.AvailWidth || Element.Height != _lastControlState.AvailHeight))
                    LayoutForSize((int)(Element.Width * Forms9Patch.Display.Scale), (int)(Element.Height * Forms9Patch.Display.Scale));
                else
                    RequestLayout();
            }
        }
        */

        #endregion


        #region Fitting

        Size LabelXamarinSize(double widthConstraint, double fontSize)
        {
            var size = LabelSize(widthConstraint, fontSize);
            return new Size(size.Width, size.Height);
        }

        Windows.Foundation.Size LabelSize(double widthConstraint, double fontSize)
        {
            var textBlock = Control;
            var label = Element;
            if (textBlock != null && label!=null)
            {
                textBlock.SetAndFormatText(label, fontSize);
                textBlock.Measure(new Windows.Foundation.Size(widthConstraint, double.PositiveInfinity));
                return new Windows.Foundation.Size(textBlock.DesiredSize.Width, textBlock.DesiredSize.Height);
            }
            return Windows.Foundation.Size.Empty;
        }


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
            textBlock.SetAndFormatText(label, mid);
            textBlock.Measure(new Windows.Foundation.Size(availWidth-4, double.PositiveInfinity));
            var height = textBlock.DesiredSize.Height;

            if (height <= _fontMetrics.AscentForFontSize(mid) + _fontMetrics.DescentForFontSize(mid) + _fontMetrics.LineGapForFontSize(mid))
                height = _fontMetrics.CapHeightForFontSize(mid) + _fontMetrics.DescentForFontSize(mid);

            if (height > availHeight)
                return ZeroLinesFit(label, textBlock, min, mid, availWidth, availHeight);
            if (height < availHeight)
                return ZeroLinesFit(label, textBlock, mid, max, availWidth, availHeight);
            return min;
        }

        double WidthAndLinesFit(Label label, TextBlock textBlock, int lines, double min, double max, double availWidth)
        {
            if (max - min < Precision)
            {
                //DebugMessage("Precision reached: result=[" + min + "]");
                if (textBlock.FontSize != min)
                {
                    textBlock?.SetAndFormatText(label, min);
                    textBlock?.Measure(new Windows.Foundation.Size(availWidth-4, double.PositiveInfinity));
                }
                return min;
            }

            var mid = (max + min) / 2.0;

            textBlock?.SetAndFormatText(label, mid);
            textBlock?.Measure(new Windows.Foundation.Size(availWidth, double.PositiveInfinity));

            var renderedLines = textBlock.DesiredSize.Height / _fontMetrics.LineHeightForFontSize(mid); // textBlock.LineHeight;
            //DebugMessage("mid=["+mid+"] renderedLines=[" + renderedLines + "] DesiredSize=[" + textBlock.DesiredSize.Height + "] LineHeight=[" + textBlock.LineHeight + "]");

            if (Math.Round(renderedLines) > lines || textBlock.ActualWidth > textBlock.DesiredSize.Width)
                return WidthAndLinesFit(label, textBlock, lines, min, mid, availWidth);
            return WidthAndLinesFit(label, textBlock, lines, mid, max, availWidth);
        }


        #endregion
    }
}
