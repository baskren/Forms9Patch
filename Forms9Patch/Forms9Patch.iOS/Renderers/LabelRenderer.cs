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
            //if (UIDevice.CurrentDevice.CheckSystemVersion(11, 4))
            Forms9Patch.Label.DefaultFontSize = UIFont.LabelFontSize;
        }

        UIColor _defaultTextColor;

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
            if (width >= 0
                && height >= 0
                && !_currentDrawState.IsBlank
                && Control is UILabel control
                && Element != null)
            {
                _currentDrawState.AvailWidth = width;
                _currentDrawState.AvailHeight = height;
                if (_currentDrawState == _lastDrawState && _lastDrawResult.HasValue)
                    return _lastDrawResult.Value;
                _lastDrawResult = InternalLayout(control, _currentDrawState);
                //_lastDrawState?.Dispose();
                _lastDrawState = new TextControlState(_currentDrawState);
                return _lastDrawResult.Value;
            }
            return new SizeRequest(Xamarin.Forms.Size.Zero);
        }

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (widthConstraint < 0
                || heightConstraint < 0
                || _currentDrawState == null
                || _currentDrawState.IsBlank
                || Control == null
                || Element == null)
                return new SizeRequest(Size.Zero);

            //_currentMeasureState?.Dispose();
            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = widthConstraint,
                AvailHeight = heightConstraint
            };

            if (_currentMeasureState == _lastMeasureState && _lastMeasureResult.HasValue)
                return _lastMeasureResult.Value;

            _lastMeasureResult = InternalLayout(MeasureControl, _currentMeasureState);
            //_lastMeasureState?.Dispose();
            _lastMeasureState = new TextControlState(_currentMeasureState);

            return _lastMeasureResult.Value;
        }

        SizeRequest InternalLayout(UILabel control, TextControlState state)
        {
            if (Element is Forms9Patch.Label element)
            {
                var tmpFontSize = BoundTextSize(element.FontSize);

                if (tmpFontSize < 0)
                    System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": [" + null + "]");

                control.PropertiesFromControlState(state);
                control.Lines = 0;

                if (element.Lines == 0)
                {
                    if (state.AvailHeight < int.MaxValue / 3)
                        tmpFontSize = ZeroLinesFit(control, state.AvailWidth, state.AvailHeight, tmpFontSize);
                }
                else
                {
                    if (element.AutoFit == AutoFit.Lines)
                    {
                        if (state.AvailHeight < int.MaxValue / 3)
                        {
                            var font = control.Font = UIFont.FromDescriptor(_currentDrawState.FontDescriptor, tmpFontSize);
                            var lineHeightRatio = font.LineHeight / font.PointSize;
                            var tmpLineSize = (nfloat)(state.AvailHeight - 0.05f) / element.Lines;
                            tmpFontSize = tmpLineSize / lineHeightRatio;
                        }
                    }
                    else if (element.AutoFit == AutoFit.Width)
                        tmpFontSize = WidthFit(control, state.AvailWidth, tmpFontSize);
                }

                tmpFontSize = BoundFontSize(tmpFontSize);

                if (tmpFontSize < 0)
                    System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": [" + null + "]");


                if (Math.Abs(tmpFontSize - element.FittedFontSize) > 0.1)
                {
                    if (element != null && control != null)  // multipicker test was getting here with Element and control both null
                    {
                        if (System.Math.Abs(tmpFontSize - element.FontSize) < 0.1 || (element.FontSize < 0 && System.Math.Abs(tmpFontSize - UIFont.LabelFontSize) < 0.1))
                            element.FittedFontSize = -1;
                        else
                            element.FittedFontSize = tmpFontSize;
                    }
                }


                var syncFontSize = (nfloat)((ILabel)element).SynchronizedFontSize;
                if (syncFontSize >= 0 && System.Math.Abs(tmpFontSize - syncFontSize) > 0.1)
                    tmpFontSize = syncFontSize;

                state.FontPointSize = tmpFontSize;
                control.Font = state.Font;
                control.Lines = 0;
                control.AdjustsFontSizeToFitWidth = false;
                control.ClearsContextBeforeDrawing = true;
                control.ContentMode = UIViewContentMode.Redraw;

                CGSize cgSize = LabelSize(control, state.AvailWidth, tmpFontSize);

                control.Lines = state.Lines;

                double reqWidth = cgSize.Width;
                double reqHeight = cgSize.Height + 0.05;
                var textHeight = cgSize.Height;
                var textLines = Lines(textHeight, control.Font);

                if (double.IsPositiveInfinity(state.AvailHeight))
                {
                    if (element.Lines > 0)
                    {
                        if (element.AutoFit == AutoFit.Lines)
                            reqHeight = element.Lines * control.Font.LineHeight;
                        else if (element.AutoFit == AutoFit.None && element.Lines <= textLines)
                            reqHeight = element.Lines * control.Font.LineHeight;
                    }
                    control.Center = new CGPoint(control.Center.X, reqHeight / 2);
                }
                else
                {
                    var constraintLines = Lines(state.AvailHeight, control.Font);
                    var constraintLinesHeight = Math.Floor(constraintLines) * control.Font.LineHeight;

                    if (element.Lines > 0 && element.Lines <= Math.Min(textLines, constraintLines))
                        reqHeight = element.Lines * control.Font.LineHeight;
                    else if (textLines <= constraintLines)
                        reqHeight = textHeight;
                    else if (constraintLines >= 1)
                        reqHeight = constraintLinesHeight;
                    else
                        reqHeight = state.AvailHeight;

                    if (element.VerticalTextAlignment == TextAlignment.Start)
                        control.Center = new CGPoint(control.Center.X, reqHeight / 2);
                    else if (element.VerticalTextAlignment == TextAlignment.End)
                        control.Center = new CGPoint(control.Center.X, state.AvailHeight - reqHeight / 2);
                }
                SizeRequest result;
                //if (UIDevice.CurrentDevice.CheckSystemVersion(11, 3))
                result = new SizeRequest(new Size(Math.Ceiling(reqWidth), Math.Ceiling(reqHeight)), new Size(10, Math.Ceiling(state.Font.LineHeight)));
                //else
                //    result = new SizeRequest(new Size(Math.Ceiling(reqWidth * 1.25), Math.Ceiling(reqHeight * 1.25)), new Size(10, Math.Ceiling(state.Font.LineHeight)));

                return result;
            }
            return new SizeRequest(Size.Zero);
        }

        void UpdateSynchronizedFontSize()
        {
            if (Element is Forms9Patch.Label element)
            {
                var syncFontSize = element.SynchronizedFontSize;
                if (syncFontSize > 0 && syncFontSize < _currentDrawState.FontPointSize)
                {
                    _currentDrawState.FontPointSize = (System.nfloat)syncFontSize;
                    LayoutSubviews();
                }
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
            if (Element is Forms9Patch.Label element)
                DrawLabel(element.Width, element.Height);
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
            var font = label.Font.WithSize(fontSize);
            CGSize labelSize = CGSize.Empty;
            var constraintSize = new CGSize(Math.Floor(widthConstraint), double.PositiveInfinity);
            if (Element is Label element)
            {
                if (element.Text != null)
                {
                    label.Text = element.Text;
                    labelSize = label.Text.StringSize(font, constraintSize,// _currentDrawState.LineBreakMode);
                    element.LineBreakMode == LineBreakMode.CharacterWrap
                        ? UILineBreakMode.CharacterWrap
                        : UILineBreakMode.WordWrap);
                }
                else if (element?.HtmlText != null)
                {
                    var color = element.TextColor;
                    label.AttributedText = element.F9PFormattedString.ToNSAttributedString(font, color.ToUIColor(Color.Black), element.LineHeight);
                    labelSize = label.AttributedText.GetBoundingRect(constraintSize, NSStringDrawingOptions.UsesLineFragmentOrigin, null).Size;
                }
            }
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
            var minFontSize = (nfloat)(Element?.MinFontSize ?? 4);
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
                var font = Control?.Font.WithSize(result) ?? UIFont.SystemFontOfSize(result);
                CGSize labelSize = LabelSize(label, widthConstraint, result);
                if ((labelSize.Height / font.LineHeight) <= (Element?.Lines ?? 1000) + .005f)
                    return result;
            }
            return result;
        }

        nfloat ZeroLinesFit(UILabel label, double widthConstraint, double heightConstraint, nfloat startingFontSize)
        {
            if (double.IsPositiveInfinity(heightConstraint) || double.IsPositiveInfinity(widthConstraint))
                return startingFontSize;

            var minFontSize = (nfloat)(Element?.MinFontSize ?? 4);
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
                    break;
            }
            return result;
        }
        #endregion


        #region Helper methods
        double Lines(double height, UIFont font)
            => (height) / (font.LineHeight);

        #endregion


        #region Change management
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
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
                    var view = new UILabel(CGRect.Empty)
                    {
                        BackgroundColor = UIColor.Clear
                    };
                    _defaultTextColor = view.TextColor;
                    SetNativeControl(view);
                }
                UpdateTextColor();
                UpdateFont();
                UpdateHorizontalAlignment();
                UpdateLineBreakMode();
                UpdateStrings();
                e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                e.NewElement.RendererSizeForWidthAndFontSize += LabelF9PSize;
                e.NewElement.Draw += DrawLabel;
            }
            base.OnElementChanged(e);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                if (Element is Label label)
                {
                    try
                    {
                        label.RendererIndexAtPoint -= IndexAtPoint;
                        label.RendererSizeForWidthAndFontSize -= LabelF9PSize;
                        label.Draw -= DrawLabel;
                    }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                    catch (System.Exception) { }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
                }

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
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.FontProperty.PropertyName
                || e.PropertyName == Label.FontFamilyProperty.PropertyName
                || e.PropertyName == Label.FontSizeProperty.PropertyName
                || e.PropertyName == Label.FontAttributesProperty.PropertyName)
            {
                UpdateFont();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName
                || e.PropertyName == Label.HtmlTextProperty.PropertyName
                || e.PropertyName == Label.LineHeightProperty.PropertyName
                )
            {
                UpdateStrings();
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
            {
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.AutoFitProperty.PropertyName)
            {
                if (Element is Forms9Patch.Label element)
                    _currentDrawState.AutoFit = element.AutoFit;
                LayoutSubviews();
            }
            else if (e.PropertyName == Label.LinesProperty.PropertyName)
            {
                if (Element is Forms9Patch.Label element)
                    _currentDrawState.Lines = element.Lines;
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
            if (Element is Forms9Patch.Label element)
            {
                var minFontSize = element.FontSize > 0 ? element.MinFontSize : 4;
                if (_currentDrawState.FontPointSize < minFontSize)
                    _currentDrawState.FontPointSize = -1;
            }
        }

        void UpdateFont()
        {
            if (Element is Forms9Patch.Label label)
            {
                var font = label.ToUIFont();
                _currentDrawState.FontPointSize = font.PointSize;
                _currentDrawState.FontDescriptor = font.FontDescriptor;
                UpdateStrings();
            }
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
            if (Element is Forms9Patch.Label element)
            {
                _currentDrawState.HorizontalTextAlignment = element.HorizontalTextAlignment.ToNativeTextAlignment();
                InvokeOnMainThread(() =>
                {
                    if (Control is UILabel control)
                        control.TextAlignment = _currentDrawState.HorizontalTextAlignment;
                });
            }
        }

        void UpdateStrings()
        {
            if (Element is Forms9Patch.Label element && element.HtmlText != null)
                UpdateAttributedText();
            else
                UpdateText();
        }

        void UpdateText()
        {
            if (Element is Forms9Patch.Label element)
            {
                _currentDrawState.AttributedString?.Dispose();
                _currentDrawState.AttributedString = null;

                _currentDrawState.Text?.Dispose();
                _currentDrawState.Text = null;

                _currentDrawState.Text = element?.Text is null
                    ? null
                    : new NSString(element.Text);

                if (Control is UILabel control)
                {
                    control.AttributedText?.Dispose();
                    control.AttributedText = null;
                    control.Text = null;

                    if (string.IsNullOrEmpty(_currentDrawState.Text) || element.LineHeight <= 0 || Math.Abs(element.LineHeight - 1) < 0.01)
                        control.Text = _currentDrawState.Text;
                    else
                    {
                        var attributedString = new NSMutableAttributedString(_currentDrawState.Text);
                        attributedString.AddAttribute(UIStringAttributeKey.ParagraphStyle, new NSMutableParagraphStyle { LineHeightMultiple = new nfloat(element.LineHeight) }, new NSRange(0, _currentDrawState.Text.Length));
                        control.AttributedText = attributedString;
                    }
                }
            }
        }

        void UpdateAttributedText()
        {
            if (Element is Forms9Patch.Label element)
            {
                var color = element.TextColor;
                _currentDrawState.Text = null;
                _currentDrawState.AttributedString = element?.HtmlText is null
                    ? null
                    : element.F9PFormattedString.ToNSAttributedString(_currentDrawState.Font, color.ToUIColor(Color.Black), element.LineHeight);
                if (Control is UILabel control)
                    control.AttributedText = _currentDrawState.AttributedString;
            }
        }

        void UpdateTextColor()
        {
            InvokeOnMainThread(() =>
            {
                if (Control is UILabel control && Element is Forms9Patch.Label label)
                {
                    var color = label.TextColor;
                    if (color == Color.Default || color == default || color.IsDefault)
                        control.TextColor = _defaultTextColor;
                    else
                        control.TextColor = color.ToUIColor();
                    UpdateStrings();
                }
            });
        }

        void UpdateLineBreakMode()
        {
            if (Element is Forms9Patch.Label element)
            {
                switch (element.LineBreakMode)
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
            }
        }
        #endregion


        #region FontSize helpers
        nfloat BoundTextSize(double textSize) => BoundFontSize((nfloat)textSize);

        nfloat BoundFontSize(nfloat textSize)
        {
            if (Element is Forms9Patch.Label element)
            {
                if (textSize < 0.0001)
#pragma warning disable CS0618 // Type or member is obsolete
                    textSize = (System.nfloat)(UIFont.LabelFontSize * System.Math.Abs(element.FontSize));
#pragma warning restore CS0618 // Type or member is obsolete
                if (element.FontSize > 0 && textSize > element.FontSize)
                    return (nfloat)element.FontSize;
                if (Math.Abs(element.FontSize - -1) < 0.0001 && textSize > Label.DefaultFontSize)
                    return (System.nfloat)Label.DefaultFontSize;
                if (textSize < ModelMinFontSize)
                    textSize = ModelMinFontSize;
            }
            return textSize;
        }

        nfloat ModelMinFontSize
        {
            get
            {
                var minFontSize = (nfloat)(Element?.MinFontSize ?? 4);
                if (minFontSize < 0)
                    minFontSize = 4;
                return minFontSize;
            }
        }

        #endregion


        #region Index of touch point
        int IndexAtPoint(Point point)
        {
            if (Control is UILabel control)
            {
                var cgPoint = new CGPoint(point.X, point.Y - control.Frame.Y);

                // init text storage
                using (var textStorage = new NSTextStorage())
                using (var attrText = new NSAttributedString(control.AttributedText))
                using (var layoutManager = new NSLayoutManager())
                using (var textContainer = new NSTextContainer(new CGSize(control.Frame.Width, control.Frame.Height * 2))
                {
                    LineFragmentPadding = 0,
                    MaximumNumberOfLines = (nuint)_currentDrawState.Lines,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    //Size = new CGSize(control.Frame.Width, control.Frame.Height * 2)
                })
                {
                    textStorage.SetString(attrText);
                    textStorage.AddLayoutManager(layoutManager);
                    layoutManager.AddTextContainer(textContainer);
                    layoutManager.AllowsNonContiguousLayout = true;

                    var characterIndex = layoutManager.GetCharacterIndex(cgPoint, textContainer);
                    return (int)characterIndex;
                }
            }
            return -1;
        }
        #endregion
    }
}

