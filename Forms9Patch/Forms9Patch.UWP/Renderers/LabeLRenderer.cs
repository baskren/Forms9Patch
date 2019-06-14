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
            => P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false;

        void DebugGetDesiredRequest(string mark, double widthConstraint, double heightConstraint, SizeRequest request, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            P42.Utils.Debug.Message(Element,mark + " Constr=[" + widthConstraint + "," + heightConstraint + "] " + DebugControlSizes() + " result=[" + request + "]", callerName);
        }

        void DebugArrangeOverride(Windows.Foundation.Size finalSize, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            P42.Utils.Debug.Message(Element,"FinSize=[" + finalSize + "] " + DebugControlSizes(), callerName);
        }

        void DebugMeasureOverride(string mark, Windows.Foundation.Size availableSize, Windows.Foundation.Size result, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            P42.Utils.Debug.Message(Element,mark + " Elmt.Sz=[" + Element?.Bounds.Size + "] AvailSz=[" + availableSize + "] " + DebugControlSizes() + " result=[" + result + "]", callerName);
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

        void DebugTextBlockProperties(TextBlock source)
        {
            if (DebugCondition)
            {
                System.Diagnostics.Debug.Indent();
                System.Diagnostics.Debug.WriteLine("FontSize =[" + source.FontSize + "]");
                System.Diagnostics.Debug.WriteLine("LineStackingStrategy =[" + source.LineStackingStrategy + "]");
                System.Diagnostics.Debug.WriteLine("LineHeight =[" + source.LineHeight + "]");
                System.Diagnostics.Debug.WriteLine("CharacterSpacing =[" + source.CharacterSpacing + "]");
                System.Diagnostics.Debug.WriteLine("IsTextSelectionEnabled =[" + source.IsTextSelectionEnabled + "]");
                System.Diagnostics.Debug.WriteLine("FontWeight =[" + source.FontWeight + "]");
                System.Diagnostics.Debug.WriteLine("Padding =[" + source.Padding + "]");
                System.Diagnostics.Debug.WriteLine("Foreground =[" + source.Foreground + "]");
                System.Diagnostics.Debug.WriteLine("FontStyle =[" + source.FontStyle + "]");
                System.Diagnostics.Debug.WriteLine("FontStretch =[" + source.FontStretch + "]");
                System.Diagnostics.Debug.WriteLine("FontFamily =[" + source.FontFamily + "]");
                System.Diagnostics.Debug.WriteLine("TextWrapping =[" + source.TextWrapping + "]");
                System.Diagnostics.Debug.WriteLine("TextTrimming =[" + source.TextTrimming + "]");
                System.Diagnostics.Debug.WriteLine("TextAlignment =[" + source.TextAlignment + "]");
                System.Diagnostics.Debug.WriteLine("Text =[" + source.Text + "]");
                System.Diagnostics.Debug.WriteLine("OpticalMarginAlignment =[" + source.OpticalMarginAlignment + "]");
                System.Diagnostics.Debug.WriteLine("TextReadingOrder =[" + source.TextReadingOrder + "]");
                System.Diagnostics.Debug.WriteLine("TextLineBounds =[" + source.TextLineBounds + "]");
                System.Diagnostics.Debug.WriteLine("SelectionHighlightColor =[" + source.SelectionHighlightColor + "]");
                System.Diagnostics.Debug.WriteLine("MaxLines =[" + source.MaxLines + "]");
                System.Diagnostics.Debug.WriteLine("IsColorFontEnabled =[" + source.IsColorFontEnabled + "]");
                System.Diagnostics.Debug.WriteLine("IsTextScaleFactorEnabled =[" + source.IsTextScaleFactorEnabled + "]");
                System.Diagnostics.Debug.WriteLine("TextDecorations =[" + source.TextDecorations + "]");
                System.Diagnostics.Debug.WriteLine("HorizontalTextAlignment =[" + source.HorizontalTextAlignment + "]");
                System.Diagnostics.Debug.WriteLine("FlowDirection =[" + source.FlowDirection + "]");
                System.Diagnostics.Debug.WriteLine("DataContext =[" + source.DataContext + "]");
                System.Diagnostics.Debug.WriteLine("Name =[" + source.Name + ".Copy" + "]");
                System.Diagnostics.Debug.WriteLine("MinWidth =[" + source.MinWidth + "]");
                System.Diagnostics.Debug.WriteLine("MinHeight =[" + source.MinHeight + "]");
                System.Diagnostics.Debug.WriteLine("MaxWidth =[" + source.MaxWidth + "]");
                System.Diagnostics.Debug.WriteLine("MaxHeight =[" + source.MaxHeight + "]");
                System.Diagnostics.Debug.WriteLine("Margin =[" + source.Margin + "]");
                System.Diagnostics.Debug.WriteLine("Language =[" + source.Language + "]");
                System.Diagnostics.Debug.WriteLine("HorizontalAlignment =[" + source.HorizontalAlignment + "]");
                System.Diagnostics.Debug.WriteLine("VerticalAlignment =[" + source.VerticalAlignment + "]");
                System.Diagnostics.Debug.WriteLine("Width =[" + source.Width + "]");
                System.Diagnostics.Debug.WriteLine("Height =[" + source.Height + "]");
                System.Diagnostics.Debug.WriteLine("Style =[" + source.Style + "]");
                System.Diagnostics.Debug.WriteLine("RequestedTheme =[" + source.RequestedTheme + "]");
                System.Diagnostics.Debug.WriteLine("FocusVisualSecondaryThickness =[" + source.FocusVisualSecondaryThickness + "]");
                System.Diagnostics.Debug.WriteLine("FocusVisualSecondaryBrush =[" + source.FocusVisualSecondaryBrush + "]");
                System.Diagnostics.Debug.WriteLine("FocusVisualPrimaryThickness =[" + source.FocusVisualPrimaryThickness + "]");
                System.Diagnostics.Debug.WriteLine("FocusVisualPrimaryBrush =[" + source.FocusVisualPrimaryBrush + "]");
                System.Diagnostics.Debug.WriteLine("FocusVisualMargin =[" + source.FocusVisualMargin + "]");
                System.Diagnostics.Debug.WriteLine("AllowFocusWhenDisabled =[" + source.AllowFocusWhenDisabled + "]");
                System.Diagnostics.Debug.WriteLine("AllowFocusOnInteraction =[" + source.AllowFocusOnInteraction + "]");
                System.Diagnostics.Debug.WriteLine("Clip =[" + source.Clip + "]");
                System.Diagnostics.Debug.Unindent();
            }
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
        Windows.Foundation.Size _lastInternalMeasure = new Windows.Foundation.Size(0, 0);
        AutoFit _lastAutoFit = (AutoFit)Forms9Patch.Label.AutoFitProperty.DefaultValue;
        int _lastLines = (int)Forms9Patch.Label.LinesProperty.DefaultValue;
        internal static TextBlock _defaultTextBlock = new TextBlock();
        SharpDX.DirectWrite.FontMetrics _fontMetrics  = _defaultTextBlock.GetFontMetrics() ;
        //bool isNewElement;

        SizeRequest _perfectSize;
        bool _perfectSizeValid;
        #endregion


        #region Element & Property Change Handlers
        protected override void OnElementChanged(ElementChangedEventArgs<Forms9Patch.Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelF9PSize;
                //e.OldElement.DebugMessageAction -= InnerDebugMessage;
                //e.OldElement.DebugCriteriaFunc -= DebugCriteria;
                //e.OldElement.SizeChanged -= OnElementSizeChanged;
            }
            if (e.NewElement != null)
            {
                //isNewElement = true;
                if (Control == null)
                {
                    var nativeControl = new TextBlock();
                    _defaultNativeFontFamily = nativeControl.FontFamily;
                    _defaultNativeFontSize = nativeControl.FontSize;
                    _defaultNativeFontIsBold = nativeControl.FontWeight.Weight >= Windows.UI.Text.FontWeights.Bold.Weight;
                    _defaultNativeFontIsItalics = nativeControl.FontStyle != Windows.UI.Text.FontStyle.Normal;
                    SetNativeControl(nativeControl);
                }
                e.NewElement.RendererSizeForWidthAndFontSize += LabelF9PSize;
                //e.NewElement.DebugMessageAction += InnerDebugMessage;
                //e.NewElement.DebugCriteriaFunc += DebugCriteria;
                //e.NewElement.SizeChanged += OnElementSizeChanged;

                //Control.LayoutUpdated += (s, ex) => P42.Utils.Debug.Message(Element,"Control.LayoutUpdated");
                //Control.Loading += (s, ex) => P42.Utils.Debug.Message(Element,"Control.Loading");
                //Control.Loaded += (s, ex) => P42.Utils.Debug.Message(Element,"Contorl.Loaded");
                //Control.Unloaded += (s, ex) => P42.Utils.Debug.Message(Element,"Control.Unloaded");

                UpdateColor(Control);
                UpdateHorizontalAlign(Control);
                UpdateLineBreakMode(Control);
                
            }
        }

        private void OnElementSizeChanged(object sender, EventArgs e)
        {
            P42.Utils.Debug.Message(Element,"ENTER size=["+Element.Bounds.Size+"]");
            P42.Utils.Debug.Message(Element,"EXIT");
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            #region Forms9Patch.Label Properties
            if (e.PropertyName == Forms9Patch.Label.TextProperty.PropertyName || e.PropertyName == Forms9Patch.Label.HtmlTextProperty.PropertyName)
            {
                P42.Utils.Debug.Message(Element,"ENTER");
                ForceLayout(Control);
                P42.Utils.Debug.Message(Element,"EXIT");
            }
            else if (e.PropertyName == Forms9Patch.Label.AutoFitProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.WidthProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.HeightProperty.PropertyName
                //|| e.PropertyName == Forms9Patch.Label.WidthRequestProperty.PropertyName
                //||e.PropertyName == Forms9Patch.Label.HeightRequestProperty.PropertyName
                )
            {
                P42.Utils.Debug.Message(Element,"ENTER");
                ForceLayout(Control);
                P42.Utils.Debug.Message(Element,"EXIT");
            }
            else if (e.PropertyName == Forms9Patch.Label.LinesProperty.PropertyName)
            {
                P42.Utils.Debug.Message(Element,"ENTER");
                ForceLayout(Control);
                P42.Utils.Debug.Message(Element,"EXIT");
            }
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
            //UpdateTextAndFont(Control);
            {
                P42.Utils.Debug.Message(Element,"ENTER");
                ForceLayout(Control);
                P42.Utils.Debug.Message(Element,"EXIT");
            }
            else if (e.PropertyName == Forms9Patch.Label.TextColorProperty.PropertyName)
                UpdateColor(Control);
            else if (e.PropertyName == Forms9Patch.Label.LineBreakModeProperty.PropertyName)
            {
                P42.Utils.Debug.Message(Element,"ENTER");
                ForceLayout(Control);
                P42.Utils.Debug.Message(Element,"EXIT");
            }
            else if (e.PropertyName == Forms9Patch.Label.HorizontalTextAlignmentProperty.PropertyName)
                UpdateHorizontalAlign(Control);
            #endregion

            base.OnElementPropertyChanged(sender, e);
        }


        void UpdateHorizontalAlign(TextBlock textBlock)
        {
            _perfectSizeValid = false;
            if (textBlock == null)
                return;
            var label = Element;
            if (label == null)
                return;
            textBlock.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment();
            //ForceLayout(textBlock);
        }


        void UpdateVerticalAlign(TextBlock textBlock)
        {
            _perfectSizeValid = false;
            if (textBlock == null)
                return;
            var label = Element;
            if (label == null)
                return;
            textBlock.VerticalAlignment = label.VerticalTextAlignment.ToNativeVerticalAlignment();
            if (Element.Bounds.Width > 0 && Element.Bounds.Height > 0)
                ArrangeOverride(new Windows.Foundation.Size(Element.Bounds.Width, Element.Bounds.Height));
        }


        void UpdateColor(TextBlock textBlock)
        {
            if (textBlock == null)
                return;
            var label = Element;
            if (label != null && label.TextColor != Color.Default)
                textBlock.Foreground = label.TextColor.ToBrush();
            else
                textBlock.ClearValue(TextBlock.ForegroundProperty);
        }


        void UpdateSynchrnoizedFontSize(TextBlock textBlock)
        {
            P42.Utils.Debug.Message(Element,"ENTER");
            _perfectSizeValid = false;
            if (Element is ILabel label && label.SynchronizedFontSize != textBlock.FontSize)
                if (label.SynchronizedFontSize!=-1 && textBlock.FontSize!=_defaultNativeFontSize)
                ForceLayout(textBlock);
            P42.Utils.Debug.Message(Element,"EXIT");
        }


        void UpdateLineBreakMode(TextBlock textBlock)
        {
            P42.Utils.Debug.Message(Element,"ENTER");
            _perfectSizeValid = false;

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
            P42.Utils.Debug.Message(Element,"EXIT");
        }


        void ForceLayout(TextBlock textBlock)
        {
            P42.Utils.Debug.Message(Element,"ENTER");
            _perfectSizeValid = false;
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
            P42.Utils.Debug.Message(Element,"EXIT");
        }


        void UpdateMinFontSize(TextBlock textBlock)
        {
            P42.Utils.Debug.Message(Element,"ENTER");
            var label = Element;
            if (label == null || textBlock==null)
                return;
            if (label.MinFontSize > Control.FontSize || Control.Inlines.ExceedsFontSize(label.MinFontSize))
                ForceLayout(textBlock);
            P42.Utils.Debug.Message(Element,"EXIT");
        }
        #endregion


        #region Xamarin GetDesiredSize

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //P42.Utils.Debug.Message(Element,"------------------------------------------------------------------------------------------------");
            P42.Utils.Debug.Message(Element,"ENTER widthConstrain=[" + widthConstraint + "] heightConstrain=[" + heightConstraint + "]");
            /*
            if (!_perfectSizeValid)
            {
                _perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
                _perfectSize.Minimum = new Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
                _perfectSizeValid = true;
            }

            var widthFits = widthConstraint >= _perfectSize.Request.Width;
            var heightFits = heightConstraint >= _perfectSize.Request.Height;

            if (widthFits && heightFits)
            {
                P42.Utils.Debug.Message(Element,"EXIT perfect fits ["+_perfectSize+"]");
                return _perfectSize;
            }
            /*
            var result = base.GetDesiredSize(widthConstraint, heightConstraint);
            var tinyWidth = Math.Min(10, result.Request.Width);
            result.Minimum = new Size(tinyWidth, result.Request.Height);

            if (widthFits || Element.LineBreakMode == LineBreakMode.NoWrap)
                return result;

            var containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

            if (containerIsNotInfinitelyWide)
            {
                var textCouldHaveWrapped = Element.LineBreakMode == LineBreakMode.WordWrap || Element.LineBreakMode == LineBreakMode.CharacterWrap;
                var textExceedsContainer = result.Request.Width > widthConstraint;

                if (textExceedsContainer || textCouldHaveWrapped)
                {
                    var expandedWidth = Math.Max(tinyWidth, widthConstraint);
                    result.Request = new Size(expandedWidth, result.Request.Height);
                }
            }

            return result;
            */


            var desiredSize = InternalMeasure(new Windows.Foundation.Size(widthConstraint, heightConstraint));
            var minSize = new Xamarin.Forms.Size(10, Element != null ? _fontMetrics.LineHeightForFontSize(Element.DecipheredMinFontSize()) : 10);
            var result = new SizeRequest(new Xamarin.Forms.Size(desiredSize.Width, desiredSize.Height), minSize);
            P42.Utils.Debug.Message(Element,"EXIT result=["+result+"]");
            return result;
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

            P42.Utils.Debug.Message(Element,"ENTER finalSize=[" + finalSize + "]");


            //InternalMeasure(finalSize);
            
            if (LayoutValid)
                textBlock.Measure(_lastInternalMeasure);
            else
                textBlock.Measure(finalSize);
                
            PrivateArrange(finalSize);
            P42.Utils.Debug.Message(Element,"EXIT finalSize=[" + finalSize + "]");
            return finalSize;
        }

        void PrivateArrange(Windows.Foundation.Size finalSize)
        {
            P42.Utils.Debug.Message(Element,"ENTER finalSize=[" + finalSize + "] ");
            //var childHeight = Math.Max(0, Math.Min(Element.Height, Control.DesiredSize.Height));
            var childHeight = finalSize.Height;
            P42.Utils.Debug.Message(Element," childHeight=["+childHeight+"]");

            if (double.IsInfinity(finalSize.Width) || double.IsInfinity(finalSize.Height))
                return;
            //DebugTextBlockProperties(Control);

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
            /*
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
            */
            rect.Height = childHeight;
            rect.Width = finalSize.Width;
            P42.Utils.Debug.Message(Element," rect=[" + rect + "]");
            Control.Arrange(rect);
            P42.Utils.Debug.Message(Element,"EXIT finalSize=[" + finalSize + "]");
        }



        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {
            if (DebugCondition)
                System.Diagnostics.Debug.WriteLine(GetType() + ".");
            //P42.Utils.Debug.Message(Element,"~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~");
            P42.Utils.Debug.Message(Element,"ENTER availableSize=[" + availableSize + "]");
            //var result = InternalMeasure(availableSize);

            Windows.Foundation.Size result = _lastInternalMeasure;
            if (Element.Width > 0 && Element.Height > 0)
            {
                if (!LayoutValid || Element.Width != _lastInternalMeasure.Width || Element.Height != _lastInternalMeasure.Height)
                {
                    InternalMeasure(new Windows.Foundation.Size(Element.Width, Element.Height));
                    result = _lastInternalMeasure;
                }
                //else if (Element.Width < _lastInternalMeasure.Width || Element.Height < _lastInternalMeasure.Height)
                //{
                //    InternalMeasure(new Windows.Foundation.Size(Element.Width, Element.Height));
                //    result = _lastInternalMeasure;
                //}
            }
            else if (!LayoutValid)
                result = InternalMeasure(availableSize);



            P42.Utils.Debug.Message(Element,"EXIT result=[" + result + "]");
            return result;
        }

        DateTime _lastMeasure = DateTime.MaxValue;
        int _measureOverrideInvocation;
        bool _measuring;
        protected Windows.Foundation.Size InternalMeasure(Windows.Foundation.Size availableSize, TextBlock textBlock = null)
        {
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("MeasureOverride =" + Element.HtmlText);


            var label = Element;
            textBlock = textBlock ?? Control;
            /*
            P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] MeasureOverride pre-Enter availableSize=[" + availableSize+"] ElemmentSize=["+Element.Bounds.Size+"]  PageSize=["+Xamarin.Forms.Application.Current.MainPage.Bounds.Size+"]");
            P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] \t\t availWidth>=Page.Width=[" + (Math.Round(availableSize.Width) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Width)) + "]");
            P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] \t\t availHeight>=Page.Height=[" + (Math.Round(availableSize.Height) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Height)) + "]");
            P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] \t\t Element.Parent=["+ Element.Parent + "]");
            P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] \t\t MeasureOverride FontSize=[" + textBlock.FontSize + "] BaseLineOffset=[" + textBlock.BaselineOffset + "] LineHeight=[" + textBlock.LineHeight + "]");
*/
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");



            if (label == null || textBlock == null || availableSize.Width <= 0 || availableSize.Height <= 0)
            {
                _lastAvailableSize = availableSize;
                _lastElementSize = Element.Bounds.Size;
                return new Windows.Foundation.Size(0, 0);
            }
            P42.Utils.Debug.Message(Element,"ENTER availableSize=[" + availableSize + "]");
            //if (Double.IsInfinity(availableSize.Height) && Double.IsInfinity(availableSize.Width))
            //    LayoutValid = false;

            // Next block was commented out because of failure to render BcNotes in Heights and Areas (in notes popup and on Disclaimer)
            // ... and, by doing so, it broke the picker and the keypad in Heights and Areas.
            if (LayoutValid 
                && textBlock == Control
                && _lastAvailableSize.Width == availableSize.Width 
                && _lastAvailableSize.Height == availableSize.Height 
                && _lastElementSize == Element.Bounds.Size 
                && DateTime.Now - _lastMeasure < TimeSpan.FromSeconds(1))
            //    if (LayoutValid && _lastAvailableSize.Width == availableSize.Width && _lastAvailableSize.Height <= availableSize.Height && _lastElementSize.Width == Element.Bounds.Size.Width && _lastElementSize.Height <= Element.Bounds.Size.Height && DateTime.Now - _lastMeasure < TimeSpan.FromSeconds(1))
            {
                P42.Utils.Debug.Message(Element,"EXIT USE LAST SIZE  ["+_lastInternalMeasure+"]");
                return _lastInternalMeasure;
            }

            _measuring = true;
            //_lastAvailableSize = availableSize;
            //if (DebugCondition)
            //    System.Diagnostics.Debug.WriteLine("");

            //P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] MeasureOverride ENTER availableSize=[" + availableSize + "]");
            //Element.IsInNativeLayout = true;
            label.SetIsInNativeLayout(true);

            var width = Math.Min( availableSize.Width, int.MaxValue/2);
            var height = Math.Min(availableSize.Height, int.MaxValue/2);

            /*
            var width = (Math.Round(availableSize.Width) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Width)) && label.Width > 0 
                ? Math.Min(label.Width, availableSize.Width) 
                : availableSize.Width;
            var height = (Math.Round(availableSize.Height) >= Math.Round(Xamarin.Forms.Application.Current.MainPage.Height)) && label.Height > 0 
                ? Math.Min(label.Height, availableSize.Height) 
                : availableSize.Height;

            isNewElement = Element.Width <= 0 && Element.Height <= 0;

            if (Double.IsInfinity(availableSize.Width) && (label.Width < 0 || !isNewElement) )
                width = availableSize.Width/3.0;
            if (Double.IsInfinity(availableSize.Height) && (label.Height < 0 || !isNewElement))
                height = availableSize.Height/3.0;
                */


            /*
             * // if everything below is not included, notes render as if they were requesting a little wider space and the keypad does not render correctly the first presentation;
            if (Element.Width > 0 && Element.Height > 0 && !Double.IsInfinity(width) && !Double.IsInfinity(height))  
            // This line was causing UWP to fail to correctly update width of BcOperandLabel during editing.
            //if (Element.Width > width && Element.Height > height && !Double.IsInfinity(width) && !Double.IsInfinity(height))
            {
                width = Math.Max(Element.Width,width);
                height = Math.Max(Element.Height,height);
            }
            */

            //P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] \t\t width=[" + width+"] height=["+height+"]");
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

                textBlock.FontStyle = Element.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
                textBlock.FontWeight = Element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;

                UpdateLineBreakMode(textBlock);

                double tmpHt = -1;

                textBlock.SetAndFormatText(label, tmpFontSize);
                textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                P42.Utils.Debug.Message(Element,"ENTER AUTOFIT label.Lines==0 tmpFontSize=["+tmpFontSize+"] textBlock.DesiredSize=[" + textBlock.DesiredSize + "] width=[" + width + "] height=[" + height + "]");
                _fontMetrics = textBlock.GetFontMetrics();
                if (label.Lines == 0)
                {
                    P42.Utils.Debug.Message(Element,"A label.Lines==0");
                    // do our best job to fit the existing space.
                    if (textBlock.DesiredSize.Width - width > -Precision || textBlock.DesiredSize.Height - height > -Precision)
                    {
                        if (DebugCondition)
                            System.Diagnostics.Debug.WriteLine(GetType() + ".");
                        tmpFontSize = ZeroLinesFit(label, textBlock, minFontSize, tmpFontSize, width, height);
                        if (DebugCondition)
                            System.Diagnostics.Debug.WriteLine(GetType() + ".");
                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                        P42.Utils.Debug.Message(Element,"A tmpFontSize=[" + tmpFontSize + "]  textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                    }
                }
                else if (label.AutoFit == AutoFit.Lines)
                {
                    P42.Utils.Debug.Message(Element,"B label.AutoFit==Lines");
                    tmpHt = height;
                    if (height > int.MaxValue / 3)
                    {
                        //tmpHt = height = label.Lines * (_fontMetrics.LineHeightForFontSize(tmpFontSize) + ;
                        tmpHt = height = _fontMetrics.HeightForLinesAtFontSize(label.Lines, tmpFontSize);
                        P42.Utils.Debug.Message(Element,"B.1 tmpFontSize=[" + tmpFontSize + "]  textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                    }
                    else
                    {// set the font size to fit Label.Lines into the available height
                        //tmpFontSize = _fontMetrics.FontSizeFromLineHeight(height / label.Lines);
                        var constrainedSize = textBlock.DesiredSize;

                        tmpFontSize = _fontMetrics.FontSizeFromLinesInHeight(label.Lines, tmpHt);
                        tmpFontSize = FontExtensions.ClipFontSize(tmpFontSize, label);

                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                        if (Element.Lines>1)
                        {
                            var unconstrainedSize = textBlock.DesiredSize;
                            if (unconstrainedSize.Width > constrainedSize.Width)
                            {
                                var bestWidth = FindBestWidthForLineFit(label, textBlock, tmpFontSize, tmpHt, constrainedSize.Width, unconstrainedSize.Width);
                                textBlock.SetAndFormatText(label, tmpFontSize);
                                textBlock.Measure(new Windows.Foundation.Size(bestWidth, double.PositiveInfinity));
                                P42.Utils.Debug.Message(Element,"B.2 bestWidth=[" + bestWidth + "]");
                            }
                        }
                        P42.Utils.Debug.Message(Element,"B.3 tmpFontSize=[" + tmpFontSize + "]  textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                    }
                }
                else if (label.AutoFit == AutoFit.Width)
                {
                    P42.Utils.Debug.Message(Element,"C textBlock.ActualWidth=[" + textBlock.ActualWidth + "]  textBlock.LineHeight=["+ textBlock.LineHeight + "] textBlock.DesiredSize.Height / textBlock.LineHeight=[" + textBlock.DesiredSize.Height / textBlock.LineHeight + "]  label.Lines=[" + label.Lines + "]");
                    //if (textBlock.DesiredSize.Height / textBlock.LineHeight > label.Lines)
                    if (textBlock.ActualWidth > textBlock.DesiredSize.Width || textBlock.DesiredSize.Height / textBlock.LineHeight > label.Lines)
                    {
                        tmpFontSize = WidthAndLinesFit(label, textBlock, label.Lines, minFontSize, tmpFontSize, width);
                        textBlock.SetAndFormatText(label, tmpFontSize);
                        textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                        P42.Utils.Debug.Message(Element,"C tmpFontSize=[" + tmpFontSize + "]  textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                    }
                }

                P42.Utils.Debug.Message(Element,"EXIT AUTOFIT");
                // autofit is off!  
                // No need to do anything at the moment.  Will textBlock.SetAndFormat and textBlock.Measure at the end
                //{
                //    textBlock?.SetAndFormatText(label, tmpFontSize);
                //    textBlock?.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
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
                if (Element != null && Control == textBlock)  // multipicker test was getting here with Element and Control both null
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
                //P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] MeasureOverride Element.FittedFontSize=[" + Element.FittedFontSize + "]");

                /*
                var syncFontSize = ((ILabel)label).SynchronizedFontSize;
                P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] syncFontSize=[" + syncFontSize+"]");
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
                //result = new Windows.Foundation.Size(Math.Ceiling(textBlock.DesiredSize.Width), Math.Ceiling(tmpHt > -1 ? tmpHt : textBlock.DesiredSize.Height));
                result = new Windows.Foundation.Size(Math.Ceiling(textBlock.DesiredSize.Width), Math.Ceiling(textBlock.DesiredSize.Height));

                //if (DebugCondition && label.Width>0 && label.Height > 0 &&(textBlock.DesiredSize.Width > label.Width || textBlock.DesiredSize.Height > label.Height))
                //    System.Diagnostics.Debug.WriteLine("");
                //P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] availableSize=[" + availableSize + "] result=[" + result + "] FontSize=[" + textBlock.FontSize + "] LineHeight=[" + textBlock.LineHeight + "] ");
                //System.Diagnostics.Debug.WriteLine("[" + _measureOverrideInvocation + "] MeasureOverride [" + (Element.Text ?? Element.HtmlText)+"] availableSize=[" + availableSize + "] result=[" + result + "] FontSize=[" + textBlock.FontSize + "] LineHeight=[" + textBlock.LineHeight + "] ");

                textBlock.MaxLines = label.Lines;



            }

            label.SetIsInNativeLayout(false);

            //P42.Utils.Debug.Message(Element,"[" + _measureOverrideInvocation + "] MeasureOverride EXIT");

            if (textBlock == Control)
            {
                LayoutValid = true;
                _lastAvailableSize = availableSize;
                _lastElementSize = Element.Bounds.Size;
                _lastAutoFit = label.AutoFit;
                _lastLines = label.Lines;
                _lastMeasure = DateTime.Now;
                _lastInternalMeasure = result;

                PrivateArrange(availableSize);
            }
            if (DebugCondition)
                _measureOverrideInvocation++;

            _measuring = false;
            P42.Utils.Debug.Message(Element,"EXIT result=["+result+"]");
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

        Size LabelF9PSize(double widthConstraint, double fontSize)
        {
            if (Element!=null && Control?.Copy() is TextBlock textBlock)
            {
                P42.Utils.Debug.Message(Element,"ENTER widthConstraint=[" + widthConstraint + "] fontSize=[" + fontSize + "]");

                textBlock.SetAndFormatText(Element, fontSize);
                textBlock.Measure(new Windows.Foundation.Size(widthConstraint, double.PositiveInfinity));

                var size = new Windows.Foundation.Size(textBlock.DesiredSize.Width, textBlock.DesiredSize.Height);

                var result = new Size(size.Width, size.Height);
                P42.Utils.Debug.Message(Element,"EXIT result=[" + result + "]");
                return result;
            }
            return new Size(10, 10);
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
            {
                P42.Utils.Debug.Message(Element,"textBlock.FontSize==[" + textBlock.FontSize + "] max=[" + max + "] availHeight=[" + availHeight + "] textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                return max;
            }
            if (textBlock.FontSize == min && availHeight <= textBlock.DesiredSize.Height)
            {
                P42.Utils.Debug.Message(Element,"textBlock.FontSize==[" + textBlock.FontSize + "] min=[" + min + "] availHeight=[" + availHeight + "] textBlock.DesiredSize=[" + textBlock.DesiredSize + "]");
                return min;
            }

            if (max - min < Precision)
            {
                P42.Utils.Debug.Message(Element,"min=[" + min + "] max=[" + max + "]");
                return min;
            }

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
            P42.Utils.Debug.Message(Element,"mid=["+mid+"] height=[" + height + "] availHeight=[" + availHeight + "]");
            return mid;
        }

        double WidthAndLinesFit(Label label, TextBlock textBlock, int lines, double min, double max, double availWidth)
        {
            if (max - min < Precision)
            {
                //P42.Utils.Debug.Message(Element,"Precision reached: result=[" + min + "]");
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
            //P42.Utils.Debug.Message(Element,"mid=["+mid+"] renderedLines=[" + renderedLines + "] DesiredSize=[" + textBlock.DesiredSize.Height + "] LineHeight=[" + textBlock.LineHeight + "]");

            if (Math.Round(renderedLines) > lines || textBlock.ActualWidth > textBlock.DesiredSize.Width)
                return WidthAndLinesFit(label, textBlock, lines, min, mid, availWidth);
            return WidthAndLinesFit(label, textBlock, lines, mid, max, availWidth);
        }

        double FindBestWidthForLineFit(Label label, TextBlock textBlock, double fontSize, double heightTarget, double min, double max)
        {
            if (max - min < Precision)
            {
                P42.Utils.Debug.Message(Element,"min=[" + min + "] max=[" + max + "]");
                return min;
            }
            var mid = (max + min) / 2.0;
            textBlock.SetAndFormatText(label, fontSize);
            textBlock.Measure(new Windows.Foundation.Size(mid, double.PositiveInfinity));


            var height = textBlock.DesiredSize.Height;

            if (height <= heightTarget)
                return FindBestWidthForLineFit(label, textBlock, fontSize, heightTarget, min, mid);
            else
                return FindBestWidthForLineFit(label, textBlock, fontSize, heightTarget, mid, max);
        }
        #endregion



    }


}
