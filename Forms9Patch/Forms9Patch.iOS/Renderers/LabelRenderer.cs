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


        TextControlState _currentDrawState;
        TextControlState _lastDrawState;
        SizeRequest? _lastDrawResult;

        UILabel _measureControl;
        UILabel MeasureControl => _measureControl = _measureControl ?? new UILabel();
        TextControlState _currentMeasureState;
        SizeRequest? _lastMeasureResult;
        TextControlState _lastMeasureState;


        #region Xamarin layout cycle
        SizeRequest DrawLabel(double width, double height)
        {
            if (_currentDrawState.IsBlank || Control == null || Element == null)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            if (width < 0 || height < 0)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            //_currentDrawState = _currentDrawState ?? new TextControlState(_currentDesiredSizeState);
            //var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            _currentDrawState.AvailWidth = width; // (int)System.Math.Floor(width * displayScale);
            _currentDrawState.AvailHeight = height; // (int)System.Math.Floor(height * displayScale);

            //P42.Utils.Debug.Message(Element, "ENTER  _currentDrawState.AvailWidth=[" + _currentDrawState.AvailWidth + "]  _currentDrawState.AvailHeight=[" + _currentDrawState.AvailHeight + "] Element.Id=" + Element?.Id);
            //P42.Utils.Debug.Message(Element, "_currentDrawState._id=[" + _currentDrawState._id + "]");
            //P42.Utils.Debug.Message(Element, "Control.Font.PointSize=[" + Control.Font.PointSize + "] Element.FontSize=[" + Element.FontSize + "]");

            if (_currentDrawState == _lastDrawState && _lastDrawResult.HasValue)
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDrawResult.Value + "]");
                return _lastDrawResult.Value;
            }

            _lastDrawResult = InternalLayout(Control, _currentDrawState);
            _lastDrawState = new TextControlState(_currentDrawState);

            //P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastDrawResult + "]");
            return _lastDrawResult.Value;
        }

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (widthConstraint < 0 || heightConstraint < 0)
            {
                //P42.Utils.Debug.Message(Element, "skip A");
                return new SizeRequest(Size.Zero);
            }

            if (_currentDrawState == null || _currentDrawState.IsBlank || Control == null || Element == null)
            {
                //P42.Utils.Debug.Message(Element, "skip B Element?.Id=" + Element?.Id);
                return new SizeRequest(Size.Zero);
            }

            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = widthConstraint,
                AvailHeight = heightConstraint
            };


            //P42.Utils.Debug.Message(Element, "ENTER  _currentMeasureState.AvailWidth=[" + _currentMeasureState.AvailWidth + "]  _currentMeasureState.AvailHeight=[" + _currentMeasureState.AvailHeight + "] Element?.Id=" + Element?.Id);
            //P42.Utils.Debug.Message(Element, "MeasureControl.Font.PointSize=[" + MeasureControl.Font.PointSize + "] Element.FontSize=[" + Element.FontSize + "]");

            if (_currentMeasureState == _lastMeasureState && _lastMeasureResult.HasValue)
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastMeasureResult.Value + "]");
                return _lastMeasureResult.Value;
            }

            _lastMeasureResult = InternalLayout(MeasureControl, _currentMeasureState);
            _lastMeasureState = new TextControlState(_currentMeasureState);

            //P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastMeasureResult + "]");
            return _lastMeasureResult.Value;
        }

        SizeRequest InternalLayout(UILabel control, TextControlState state)
        {
            var tmpFontSize = BoundTextSize(Element.FontSize);
            control.PropertiesFromControlState(state);
            control.Lines = 0;

            //P42.Utils.Debug.Message(Element, "ENTER  state.AvailWidth=[" + state.AvailWidth + "]  state.AvailHeight=[" + state.AvailHeight + "]");
            //P42.Utils.Debug.Message(Element, "control.Font.PointSize=[" + control.Font.PointSize + "] Element.FontSize=[" + Element.FontSize + "]");
            //P42.Utils.Debug.Message(Element, "control.LineBreakMode=[" + control.LineBreakMode + "] Element.LineBreakMode=[" + Element.LineBreakMode + "]");
            //P42.Utils.Debug.Message(Element, "Element.Lines=[" + Element.Lines + "] _currentControlState.Lines=[" + state.Lines + "]");

            if (Element.Lines == 0)
            {
                if (state.AvailHeight < int.MaxValue / 3)
                {
                    tmpFontSize = ZeroLinesFit(control, state.AvailWidth, state.AvailHeight, tmpFontSize);
                    //P42.Utils.Debug.Message(Element, "ZeroLinesFit tmpFontSize=[" + tmpFontSize + "]");
                }
            }
            else
            {
                if (Element.AutoFit == AutoFit.Lines)
                {
                    if (state.AvailHeight < int.MaxValue / 3)
                    {
                        var font = control.Font = UIFont.FromDescriptor(_currentDrawState.FontDescriptor, tmpFontSize);
                        var lineHeightRatio = font.LineHeight / font.PointSize;
                        var tmpLineSize = (nfloat)(state.AvailHeight - 0.05f) / Element.Lines;
                        tmpFontSize = tmpLineSize / lineHeightRatio;
                        //P42.Utils.Debug.Message(Element, "AutoFit.Lines B (FIXED HT) tmpFontSize=[" + tmpFontSize + "]");
                    }
                }
                else if (Element.AutoFit == AutoFit.Width)
                {
                    tmpFontSize = WidthFit(control, state.AvailWidth, tmpFontSize);
                    //P42.Utils.Debug.Message(Element, "AutoFit.Width tmpFontSize=[" + tmpFontSize + "]");
                }
            }

            //P42.Utils.Debug.Message(Element, "Fit Complete: control.Font.PointSize=[" + control.Font.PointSize + "] tmpFontSize=[" + tmpFontSize + "]");
            tmpFontSize = BoundFontSize(tmpFontSize);
            //P42.Utils.Debug.Message(Element, "Bound Complete: control.Font.PointSize=[" + control.Font.PointSize + "] tmpFontSize=[" + tmpFontSize + "]");

            if (Math.Abs(tmpFontSize - Element.FittedFontSize) > 0.1)
            {
                if (Element != null && control != null)  // multipicker test was getting here with Element and control both null
                {
                    if (System.Math.Abs(tmpFontSize - Element.FontSize) < 0.1 || (Element.FontSize < 0 && System.Math.Abs(tmpFontSize - UIFont.LabelFontSize) < 0.1))
                        Element.FittedFontSize = -1;
                    else
                        Element.FittedFontSize = tmpFontSize;
                    //P42.Utils.Debug.Message(Element, "Element.FittedFontSize=[" + tmpFontSize + "]");
                }
            }


            var syncFontSize = (nfloat)((ILabel)Element).SynchronizedFontSize;
            if (syncFontSize >= 0 && System.Math.Abs(tmpFontSize - syncFontSize) > 0.1)
            {
                tmpFontSize = syncFontSize;
                //P42.Utils.Debug.Message(Element, "syncFontSize=[" + syncFontSize + "]");
            }

            state.FontPointSize = tmpFontSize;
            control.Font = state.Font;
            control.Lines = 0;
            control.AdjustsFontSizeToFitWidth = false;
            control.ClearsContextBeforeDrawing = true;
            control.ContentMode = UIViewContentMode.Redraw;

            CGSize cgSize = LabelSize(control, state.AvailWidth, tmpFontSize);
            //P42.Utils.Debug.Message(Element, "cgSize: " + cgSize);

            control.Lines = state.Lines;

            /*
            if (state.AttributedString != null)
                control.AttributedText = state.AttributedString;
            else
                control.Text = state.Text;

            control.Hidden = false;
            */

            double reqWidth = cgSize.Width;
            double reqHeight = cgSize.Height + 0.05;
            var textHeight = cgSize.Height;
            var textLines = Lines(textHeight, control.Font);
            string alg = "--";
            //string cnstLinesStr = "CL: n/a    ";
            //string lineHeight = "LH: " + control.Font.LineHeight.ToString("00.000");
            //string cnstLinesHeight = "CLH: n/a   ";

            if (double.IsPositiveInfinity(state.AvailHeight))
            {
                //P42.Utils.Debug.Message(Element, "A");
                if (Element.Lines > 0)
                {
                    if (Element.AutoFit == AutoFit.Lines)// && Element.Lines <= textLines)
                        reqHeight = Element.Lines * control.Font.LineHeight;
                    else if (Element.AutoFit == AutoFit.None && Element.Lines <= textLines)
                        reqHeight = Element.Lines * control.Font.LineHeight;
                }

                alg = "∞A";
                //}
                control.Center = new CGPoint(control.Center.X, reqHeight / 2);
                //P42.Utils.Debug.Message(Element, "control.Center: " + control.Center);
            }
            else
            {
                //P42.Utils.Debug.Message(Element, "B");
                var constraintLines = Lines(state.AvailHeight, control.Font);
                //P42.Utils.Debug.Message(Element, "\t constraintLines: " + constraintLines);
                var constraintLinesHeight = Math.Floor(constraintLines) * control.Font.LineHeight;
                //P42.Utils.Debug.Message(Element, "\t constraintLinesHeight: " + constraintLinesHeight);
                //cnstLinesStr = "CL: " + constraintLines.ToString("0.000");

                if (Element.Lines > 0 && Element.Lines <= Math.Min(textLines, constraintLines))
                {
                    reqHeight = Element.Lines * control.Font.LineHeight;
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
                    reqHeight = state.AvailHeight;
                    alg = "D";
                }
                //P42.Utils.Debug.Message(Element, "\t alg: " + alg);
                //P42.Utils.Debug.Message(Element, "\t reqHeight: " + reqHeight);

                //P42.Utils.Debug.Message(Element, "\t Element.VerticalTextAlignment: " + Element.VerticalTextAlignment);
                if (Element.VerticalTextAlignment == TextAlignment.Start)
                    control.Center = new CGPoint(control.Center.X, reqHeight / 2);
                else if (Element.VerticalTextAlignment == TextAlignment.End)
                    control.Center = new CGPoint(control.Center.X, state.AvailHeight - reqHeight / 2);
                //P42.Utils.Debug.Message(Element, "control.Center: " + control.Center);
            }
            var result = new SizeRequest(new Size(Math.Ceiling(reqWidth), Math.Ceiling(reqHeight)), new Size(10, Math.Ceiling(state.Font.LineHeight)));
            //P42.Utils.Debug.Message(Element, "EXIT _lastSizeRequest=[" + result + "]");
            return result;
        }

        void UpdateSynchronizedFontSize()
        {
            var syncFontSize = (nfloat)((ILabel)Element).SynchronizedFontSize;
            if (syncFontSize > 0 && syncFontSize < _currentDrawState.FontPointSize)
            {
                _currentDrawState.FontPointSize = syncFontSize;
                LayoutSubviews();
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
                //GetDesiredSize(Element.Width, Element.Height);
                DrawLabel(Element.Width, Element.Height);
        }
        #endregion


        #region Measuring
        Size LabelF9PSize(double widthConstraint, double fontSize)
        {
            MeasureControl.PropertiesFromControlState(_currentDrawState);
            var cgsize = LabelSize(MeasureControl, widthConstraint, (nfloat)fontSize);
            return new Size(cgsize.Width, cgsize.Height);
        }

        CGSize LabelSize(UILabel label, double widthConstraint, nfloat fontSize)
        {
            //P42.Utils.Debug.Message(Element, "ENTER widthConstraint=[" + widthConstraint + "] fontSize=[" + fontSize + "]");
            var font = label.Font.WithSize(fontSize);
            //P42.Utils.Debug.Message(Element, "font=[" + font + "] Element.FontFamily=[" + Element.FontFamily + "]");
            CGSize labelSize = CGSize.Empty;
            var constraintSize = new CGSize(widthConstraint, double.PositiveInfinity);
            if (Element.Text != null)
            {
                labelSize = label.Text.StringSize(font, constraintSize,// _currentDrawState.LineBreakMode);
                Element.LineBreakMode == LineBreakMode.CharacterWrap
                    ? UILineBreakMode.CharacterWrap
                    : UILineBreakMode.WordWrap);
            }
            else if (Element.HtmlText != null)
            {
                var color = Element.TextColor;
                label.AttributedText = Element.F9PFormattedString.ToNSAttributedString(font, color.ToUIColor(Color.Black));
                labelSize = label.AttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
            }
            //P42.Utils.Debug.Message(Element, "EXIT labelSize=[" + labelSize + "]");
            return labelSize;
        }
        #endregion


        #region Fit Calculations
        nfloat WidthFit(UILabel label, double widthConstraint, nfloat startFontSize)
        {
            if (Math.Abs(widthConstraint) < 0.01)
                return 0;
            if (widthConstraint < 0)
                return startFontSize;

            nfloat result = startFontSize;
            var minFontSize = (nfloat)Element.MinFontSize;
            if (minFontSize < 0)
                minFontSize = 4;

            nfloat step = (result - minFontSize) / 5;
            if (step > 0.05f)
            {
                result = DescendingWidthFit(label, widthConstraint, result, minFontSize, step);
                while (step > 0.25f)
                {
                    step /= 5;
                    result = DescendingWidthFit(label, widthConstraint, result + step * 5, result, step);
                }
            }

            return result;
        }

        nfloat DescendingWidthFit(UILabel label, double widthConstraint, nfloat start, nfloat min, nfloat step)
        {
            nfloat result;
            for (result = start; result > min; result -= step)
            {
                var font = Control.Font.WithSize(result);
                CGSize labelSize = LabelSize(label, widthConstraint, result);
                if ((labelSize.Height / font.LineHeight) <= Element.Lines + .005f)
                {
                    // the backspace character is tripping up this algorithm.  So we need to do a second check
                    //labelSize = Control.IntrinsicContentSize;
                    return result;
                }
            }
            return result;
        }

        nfloat ZeroLinesFit(UILabel label, double widthConstraint, double heightConstraint, nfloat startingFontSize)
        {
            if (double.IsPositiveInfinity(heightConstraint) || double.IsPositiveInfinity(widthConstraint))
                return startingFontSize;

            var minFontSize = (nfloat)Element.MinFontSize;
            if (minFontSize < 0)
                minFontSize = 4;

            nfloat result = DescendingZeroLinesFit(label, widthConstraint, heightConstraint, startingFontSize, minFontSize, 5);
            result = DescendingZeroLinesFit(label, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 5f), result, 1);
            result = DescendingZeroLinesFit(label, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 1f), result, 0.2f);
            result = DescendingZeroLinesFit(label, widthConstraint, heightConstraint, (nfloat)Math.Min(startingFontSize, result + 0.2f), result, 0.04f);
            return result;
        }

        nfloat DescendingZeroLinesFit(UILabel label, double widthConstraint, double heightConstraint, nfloat start, nfloat min, nfloat step)
        {
            nfloat result;
            for (result = start; result > min; result -= step)
            {
                CGSize labelSize = LabelSize(label, widthConstraint, result);
                if (labelSize.Height <= heightConstraint)
                {
                    //labelSize = Control.IntrinsicContentSize;
                    break;
                }
            }
            return result;
        }
        #endregion


        #region Helper methods
        double Lines(double height, UIFont font)
        {
            //return (height + font.Leading) / (font.LineHeight + font.Leading);
            return (height) / (font.LineHeight);
        }
        #endregion


        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            //P42.Utils.Debug.Message(Element, "ENTER Element?.Id=" + Element?.Id);
            if (e.OldElement != null)
            {
                e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelF9PSize;
                e.OldElement.Draw -= DrawLabel;
            }

            if (e.NewElement != null)
            {
                _currentDrawState = new TextControlState();
                if (Control == null)
                {
                    SetNativeControl(new UILabel(CGRect.Empty)
                    {
                        BackgroundColor = UIColor.Clear
                    });
                }
                UpdateTextColor();
                UpdateFont();
                UpdateHorizontalAlignment();
                UpdateLineBreakMode();
                if (Element.HtmlText != null)
                    UpdateAttributedText();
                else
                    UpdateText();
                e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                e.NewElement.RendererSizeForWidthAndFontSize += LabelF9PSize;
                e.NewElement.Draw += DrawLabel;
            }
            base.OnElementChanged(e);
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _measureControl?.Dispose();
                _measureControl = null;
            }
            base.Dispose(disposing);
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
            }
            else if (e.PropertyName == Label.FontProperty.PropertyName
                || e.PropertyName == Label.FontFamilyProperty.PropertyName
                || e.PropertyName == Label.FontSizeProperty.PropertyName
                || e.PropertyName == Label.FontAttributesProperty.PropertyName)
            {
                UpdateFont();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                UpdateText();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.F9PFormattedStringProperty.PropertyName)
            {
                UpdateAttributedText();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
            {
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.AutoFitProperty.PropertyName)
            {
                _currentDrawState.AutoFit = Element.AutoFit;
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.LinesProperty.PropertyName)
            {
                _currentDrawState.Lines = Element.Lines;
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
            {
                UpdateLineBreakMode();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.SynchronizedFontSizeProperty.PropertyName)
                UpdateSynchronizedFontSize();
            else if (e.PropertyName == Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize();
        }


        void UpdateMinFontSize()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            if (Element != null && Control != null)
            {
                var minFontSize = Element.FontSize > 0 ? Element.MinFontSize : 4;
                if (_currentDrawState.FontPointSize < minFontSize)
                {
                    _currentDrawState.FontPointSize = -1;
                    LayoutSubviews();
                }
            }
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateFont()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            /*ControlFont = Element.ToUIFont();
            InvokeOnMainThread(() =>
            {
                if (Control != null)
                    Control.Font = ControlFont;
            });
            */
            var font = Element.ToUIFont();
            _currentDrawState.FontPointSize = font.PointSize;
            _currentDrawState.FontDescriptor = font.FontDescriptor;
            if (!string.IsNullOrEmpty(Element.HtmlText))
                UpdateAttributedText();
            else
                UpdateText();
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="color">Color.</param>
        protected override void SetBackgroundColor(Color color)
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            if (color == Color.Default)
            {
                BackgroundColor = UIColor.Clear;
                return;
            }
            BackgroundColor = color.ToUIColor();
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateHorizontalAlignment()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            _currentDrawState.HorizontalTextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
            InvokeOnMainThread(() =>
            {
                if (Control != null)
                    Control.TextAlignment = _currentDrawState.HorizontalTextAlignment;
            });
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        /*
        void UpdateText()
        {
            string text = null;
            NSAttributedString attributedText = null;
            if (Element.F9PFormattedString != null)
            {
                var color = (Color)Element.GetValue(Label.TextColorProperty);
                _currentDrawState.TextColor = color.ToUIColor(UIColor.Black);
                attributedText = Element.F9PFormattedString.ToNSAttributedString(ControlFont, _currentDrawState.TextColor);
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
        */

        void UpdateText()
        {
            //P42.Utils.Debug.Message(Element, "ENTER Element?.Id=" + Element?.Id);
            _currentDrawState.AttributedString = null;
            _currentDrawState.Text = Element?.Text is null
                ? null
                : new NSString(Element.Text);
            Control.Text = _currentDrawState.Text;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateAttributedText()
        {
            //P42.Utils.Debug.Message(Element, "ENTER Element?.Id=" + Element?.Id);
            var color = Element.TextColor;
            _currentDrawState.Text = null;
            _currentDrawState.AttributedString = Element?.HtmlText is null
                ? null
                : Element.F9PFormattedString.ToNSAttributedString(_currentDrawState.Font, color.ToUIColor(Color.Black));
            Control.AttributedText = _currentDrawState.AttributedString;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateTextColor()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            InvokeOnMainThread(() =>
            {
                var color = Element.TextColor;
                if (Control != null)
                {
                    Control.TextColor = color.ToUIColor();
                    if (Control.AttributedText != null)
                        UpdateAttributedText();
                }
            });
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateLineBreakMode()
        {
            //P42.Utils.Debug.Message(Element, "ENTER Element.LineBreakMode-[" + Element.LineBreakMode + "]");
            switch (Element.LineBreakMode)
            {
                case LineBreakMode.HeadTruncation:
                    _currentDrawState.LineBreakMode = UILineBreakMode.HeadTruncation;
                    break;
                case LineBreakMode.TailTruncation:
                    _currentDrawState.LineBreakMode = UILineBreakMode.TailTruncation;
                    break;
                case LineBreakMode.MiddleTruncation:
                    _currentDrawState.LineBreakMode = UILineBreakMode.MiddleTruncation;
                    break;
                case LineBreakMode.NoWrap:
                    _currentDrawState.LineBreakMode = UILineBreakMode.Clip;
                    _currentDrawState.Lines = 1;
                    break;
                case LineBreakMode.CharacterWrap:
                    _currentDrawState.LineBreakMode = UILineBreakMode.CharacterWrap;
                    break;
                case LineBreakMode.WordWrap:
                default:
                    _currentDrawState.LineBreakMode = UILineBreakMode.WordWrap;
                    break;
            }
            //P42.Utils.Debug.Message(Element, "EXIT _currentDrawState.LineBreakMode-[" + _currentDrawState.LineBreakMode + "]");
        }
        #endregion


        #region FontSize helpers
        nfloat BoundTextSize(double textSize) => BoundFontSize((nfloat)textSize);

        nfloat BoundFontSize(nfloat textSize)
        {
            if (textSize < 0.0001)
#pragma warning disable CS0618 // Type or member is obsolete
                textSize = (System.nfloat)(UIFont.LabelFontSize * System.Math.Abs(Element.FontSize));
#pragma warning restore CS0618 // Type or member is obsolete
            if (textSize > Element.FontSize)
                return (nfloat)Element.FontSize;
            if (textSize < ModelMinFontSize)
                textSize = ModelMinFontSize;
            return textSize;
        }

        nfloat ModelMinFontSize
        {
            get
            {
                var minFontSize = (nfloat)Element.MinFontSize;
                if (minFontSize < 0)
                    minFontSize = 4;
                return minFontSize;
            }
        }

        #endregion


        #region Index of touch point
        int IndexAtPoint(Point point)
        {
            var cgPoint = new CGPoint(point.X, point.Y - Control.Frame.Y);

            // init text storage
            var textStorage = new NSTextStorage();
            var attrText = new NSAttributedString(Control.AttributedText);
            textStorage.SetString(attrText);

            // init layout manager
            var layoutManager = new NSLayoutManager();
            textStorage.AddLayoutManager(layoutManager);

            // init text container
            var textContainer = new NSTextContainer(new CGSize(Control.Frame.Width, Control.Frame.Height * 2));
            textContainer.LineFragmentPadding = 0;
            textContainer.MaximumNumberOfLines = (nuint)_currentDrawState.Lines;
            textContainer.LineBreakMode = UILineBreakMode.WordWrap;

            textContainer.Size = new CGSize(Control.Frame.Width, Control.Frame.Height * 2);
            layoutManager.AddTextContainer(textContainer);
            layoutManager.AllowsNonContiguousLayout = true;

            var characterIndex = layoutManager.GetCharacterIndex(cgPoint, textContainer);
            return (int)characterIndex;
        }
        #endregion
    }
}

