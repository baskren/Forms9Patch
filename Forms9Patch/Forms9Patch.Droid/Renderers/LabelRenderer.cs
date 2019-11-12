using System;
using System.ComponentModel;
using System.Threading;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Text;
using FormsGestures.Droid;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.Droid.LabelRenderer))]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// The Forms9Patch Label renderer.
    /// </summary>
    public class LabelRenderer : ViewRenderer<Label, F9PTextView>
    {
        #region BindableProperties
        #region LastDrawState property
        /// <summary>
        /// BindableProperty key for LastDrawState property
        /// </summary>
        public static readonly BindableProperty LastDrawStateProperty = BindableProperty.Create(nameof(LastDrawState), typeof(TextControlState), typeof(LabelRenderer), default(TextControlState));
        TextControlState LastDrawState
        {
            get => (TextControlState)Element?.GetValue(LastDrawStateProperty);
            set => Element?.SetValue(LastDrawStateProperty, value);
        }
        #endregion LastDrawState property

        #endregion


        #region Fields
        static int _instances;
        public int _instance;
        F9PTextView _measureControl;
        TextControlState _currentMeasureState;
        SizeRequest? _lastMeasureResult;
        TextControlState _lastMeasureState;

        TextControlState _currentDrawState;
        SizeRequest? _lastDrawResult;
        //TextControlState LastDrawState;
        #endregion


        #region Constructor / Disposer
#pragma warning disable CS0618 // Type or member is obsolete
        public LabelRenderer(System.IntPtr intPtr, Android.Runtime.JniHandleOwnership owner)
        => InstanceInit();


        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRenderer"/> class.
        /// </summary>
        public LabelRenderer()
            => InstanceInit();
#pragma warning restore CS0618 // Type or member is obsolete

        public LabelRenderer(Android.Content.Context context) : base(context)
            => InstanceInit();

        public LabelRenderer(Android.Content.Context context, object obj) : base(context)
            => InstanceInit();

        void InstanceInit()
        {
            _instance = _instances++;
            AutoPackage = true;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                if (Element is Forms9Patch.Label element)
                {
                    try // incase it has been garbage collected
                    {
                        element.RendererIndexAtPoint -= IndexAtPoint;
                        element.RendererSizeForWidthAndFontSize -= LabelF9pSize;
                        element.Draw -= DrawLabel;
                    }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                    catch (System.Exception) { }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
                }
                _currentDrawState = null;
                _measureControl?.Dispose();
                _measureControl = null;
            }
            base.Dispose(disposing);
        }
        #endregion


        #region Xamarin & Forms9Patch Measurement / Layout 
        Size LabelF9pSize(double widthConstraint, double fontSize)
        {
            if (_currentDrawState == null
                || _currentDrawState.IsEmpty
                || Control == null
                || Element == null
                || _disposed)
                return Size.Zero;

            var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = (int)System.Math.Floor(System.Math.Min(widthConstraint * displayScale, MaxDim)),
                AvailHeight = MaxDim,
                TextSize = (float)fontSize,
            };

            _measureControl = _measureControl ?? new F9PTextView(Settings.Context);
            _lastMeasureResult = InternalLayout(_measureControl, _currentMeasureState);
            _lastMeasureState = new TextControlState(_currentMeasureState);

            return new Size(_lastMeasureResult.Value.Request.Width / displayScale, _lastMeasureResult.Value.Request.Height / displayScale);
        }

        SizeRequest DrawLabel(double width, double height)
        {
            if (_currentDrawState == null
                || _currentDrawState.IsEmpty
                || Control == null
                || Element == null
                || _disposed
                || width < 0
                || height < 0
                )
                return new SizeRequest(Size.Zero);
            var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            if (double.IsInfinity(width) || width > MaxDim)
                width = MaxDim;
            if (double.IsInfinity(height) || height > MaxDim)
                height = MaxDim;

            _currentDrawState.AvailWidth = (int)System.Math.Floor(width * displayScale);
            _currentDrawState.AvailHeight = (int)System.Math.Floor(height * displayScale);
            _lastDrawResult = InternalLayout(Control, _currentDrawState);
            return _lastDrawResult.Value;
        }


        int _maxDim = 0;
        int MaxDim
        {
            get
            {
                if (_maxDim < 1000)
                {
                    var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
                    _maxDim = (int)(int.MaxValue / 3 / System.Math.Ceiling(displayScale));
                }
                return _maxDim;
            }
        }

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            if (_currentDrawState == null
                || _currentDrawState.IsEmpty
                || Control == null
                || Element == null
                || _disposed)
                return new SizeRequest(Size.Zero);

            var width = MeasureSpec.GetSize(widthConstraint);
            if (MeasureSpec.GetMode(widthConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                width = MaxDim;
            var height = MeasureSpec.GetSize(heightConstraint);
            if (MeasureSpec.GetMode(heightConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                height = MaxDim;
            if (width <= 0 || height <= 0)
                return new SizeRequest(Size.Zero);

            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = width,
                AvailHeight = height
            };

            if (_lastMeasureResult != null && _lastMeasureResult.HasValue
                && _lastMeasureState == _currentMeasureState
                && _lastMeasureResult.Value.Request.Width > 0
                && _lastMeasureResult.Value.Request.Height > 0
                && _lastMeasureState.RenderedFontSize >= _currentMeasureState.TextSize
                && _lastMeasureResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastMeasureResult.Value.Request.Height <= _currentMeasureState.AvailHeight
                )
            {
                return _lastMeasureResult.Value;
            }

            if (_lastDrawResult != null && _lastDrawResult.HasValue
                && LastDrawState == _currentMeasureState
                && _lastDrawResult.Value.Request.Width > 0
                && _lastDrawResult.Value.Request.Height > 0
                && LastDrawState.RenderedFontSize >= _currentDrawState.TextSize
                && _lastDrawResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastDrawResult.Value.Request.Height <= _currentMeasureState.AvailWidth)
            {
                return _lastDrawResult.Value;
            }

            _measureControl = _measureControl ?? new F9PTextView(Settings.Context);
            _lastMeasureResult = InternalLayout(_measureControl, _currentMeasureState);
            _lastMeasureState = new TextControlState(_currentMeasureState);
            return _lastMeasureResult.Value;
        }

        SizeRequest InternalLayout(F9PTextView control, TextControlState state)
        {
            if (Element is Forms9Patch.Label element && control != null)
            {

                ICharSequence text = state.JavaText;
                var tmpFontSize = BoundTextSize(element.FontSize);
                control.Typeface = state.Typeface;
                //control.SetTextColor(state.TextColor.ToAndroid());
                UpdateColor(control);
                control.TextSize = tmpFontSize;

                control.IsNativeDrawEnabled = false;
                control.SetSingleLine(false);
                control.SetMaxLines(int.MaxValue / 256);
                control.SetIncludeFontPadding(false);
                control.Ellipsize = null;

                double tmpHt = -1;
                double tmpWd = -1;

                var fontMetrics = control.Paint.GetFontMetrics();
                var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
                var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);


                if (state.Lines == 0)
                {
                    if (state.AvailHeight < MaxDim)
                    {
                        using (var tmpPaint = new TextPaint(control.Paint))
                            tmpFontSize = TextPaintExtensions.ZeroLinesFit(state.JavaText, tmpPaint, ModelMinFontSize, tmpFontSize, state.AvailWidth, state.AvailHeight);
                    }
                }
                else
                {
                    if (state.AutoFit == AutoFit.Lines)
                    {
                        if (state.AvailHeight >= MaxDim)
                            tmpHt = System.Math.Round(state.Lines * fontLineHeight + (state.Lines - 1) * fontLeading);
                        else
                        {
                            var fontPointSize = tmpFontSize;
                            var lineHeightRatio = fontLineHeight / fontPointSize;
                            var leadingRatio = fontLeading / fontPointSize;
                            tmpFontSize = ((state.AvailHeight / (state.Lines + leadingRatio * (state.Lines - 1))) / lineHeightRatio - 0.1f);
                        }
                    }
                    else if (state.AutoFit == AutoFit.Width)
                    {
                        using (var tmpPaint = new TextPaint(control.Paint))
                            tmpFontSize = TextPaintExtensions.WidthFit(state.JavaText,tmpPaint, state.Lines, ModelMinFontSize, tmpFontSize, state.AvailWidth, state.AvailHeight);
                    }
                }

                tmpFontSize = BoundTextSize(tmpFontSize);

                // this is the optimal font size.  Let it be known!
                if (System.Math.Abs(tmpFontSize - element.FittedFontSize) > 0.1)
                {
                    if (System.Math.Abs(tmpFontSize - element.FontSize) < 0.1 || (element.FontSize < 0 && System.Math.Abs(tmpFontSize - F9PTextView.DefaultTextSize) < 0.1))
                        element.FittedFontSize = -1;
                    else
                        element.FittedFontSize = tmpFontSize;
                }

                var syncFontSize = (float)((ILabel)element).SynchronizedFontSize;
                if (syncFontSize >= 0 && System.Math.Abs(tmpFontSize - syncFontSize) > 0.1)
                {
                    tmpFontSize = syncFontSize;
                }

                control.TextSize = tmpFontSize;
                state.RenderedFontSize = tmpFontSize;
                StaticLayout layout;
                using (var tmpPaint = new TextPaint(control.Paint))
                {
                    layout = TextExtensions.StaticLayout(state.JavaText, tmpPaint, state.AvailWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
                }

                int lines = state.Lines;
                if (lines == 0 && state.AutoFit == AutoFit.None)
                {
                    for (int i = 0; i < layout.LineCount; i++)
                    {
                        if (layout.GetLineBottom(i) <= state.AvailHeight - layout.TopPadding - layout.BottomPadding)
                            lines++;
                        else
                            break;
                    }
                }
                if (layout.Height > state.AvailHeight || (lines > 0 && layout.LineCount > lines))
                {
                    if (state.Lines == 1)
                    {
                        control.SetSingleLine(true);
                        control.SetMaxLines(1);
                        control.Ellipsize = state.LineBreakMode.ToEllipsize();
                    }
                    else
                    {
                        layout.Dispose();
                        using (var tmpPaint = new TextPaint(control.Paint))
                        {
                            layout = TextPaintExtensions.Truncate(state.Text, element.F9PFormattedString, tmpPaint, state.AvailWidth, state.AvailHeight, element.AutoFit, element.LineBreakMode, ref lines, ref text);
                        }
                    }
                }
                lines = lines > 0 ? System.Math.Min(lines, layout.LineCount) : layout.LineCount;

                for (int i = 0; i < lines; i++)
                {
                    tmpHt = layout.GetLineBottom(i);
                    var width = layout.GetLineWidth(i);
                    if (width > tmpWd)
                        tmpWd = System.Math.Ceiling(width);
                }

                layout.Dispose();

                if (state.AutoFit == AutoFit.None && state.Lines > 0)
                    control.SetMaxLines(state.Lines);

                if (element.IsDynamicallySized && state.Lines > 0 && state.AutoFit == AutoFit.Lines)
                {
                    fontMetrics = control.Paint.GetFontMetrics();
                    fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
                    fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
                    tmpHt = fontLineHeight * state.Lines + fontLeading * (state.Lines - 1);
                }

                control.Gravity = element.HorizontalTextAlignment.ToHorizontalGravityFlags() | element.VerticalTextAlignment.ToVerticalGravityFlags();

                if (element.Text != null)
                    control.Text = text.ToString();
                else
                    control.TextFormatted = text;

                var result = new SizeRequest(new Size(System.Math.Ceiling(tmpWd), System.Math.Ceiling(tmpHt)), new Size(10, System.Math.Ceiling(tmpHt)));

                if (element.LineBreakMode == LineBreakMode.NoWrap)
                    control.SetSingleLine(true);

                control.IsNativeDrawEnabled = true;
                if (control == Control)
                {
                    LastDrawState = new TextControlState(state)
                    {
                        ElementHtmlText = Element.HtmlText,
                        ElementText = Element.Text
                    };
                    //Control.Invalidate();
                    Control.ForceLayout();
                }
                return result;
            }
            return new SizeRequest(Size.Zero);
        }
        #endregion


        #region Element Change Handler
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            _lastMeasureState = null;
            _lastDrawResult = null;
            _lastMeasureResult = null;
            _measureControl?.Dispose();
            _measureControl = null;
            _currentDrawState = new TextControlState
            {
                Lines = e.NewElement.Lines,
                AutoFit = e.NewElement.AutoFit,
                LineBreakMode = e.NewElement.LineBreakMode,
                SyncFontSize = (float)e.NewElement.SynchronizedFontSize,
                AvailHeight = MaxDim,
                AvailWidth = MaxDim
            };

            if (e.OldElement != null)
            {
                e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelF9pSize;
                e.OldElement.Draw -= DrawLabel;
                // the below doesn't appear to do anything anymore?
                //Control?.SkipNextInvalidate();
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var view = new F9PTextView(Context);
                    InitControl(view);
                    SetNativeControl(view);
                }
                // the below InitControl needs to be here or else some cells will show the wrong text
                else if (LastDrawState != null && LastDrawState.ElementHtmlText == e.NewElement.HtmlText && LastDrawState.ElementText == e.NewElement.Text)
                {
                    _currentDrawState = new TextControlState(LastDrawState);
                    InitControl(Control);
                    DrawLabel(LastDrawState.AvailWidth, LastDrawState.AvailHeight);
                }

                else
                {
                    Control.IsNativeDrawEnabled = true;
                    InitControl(Control);
                    // the below Layout() needs to be here or else some cells are rendered too small!
                    Layout();
                }

                e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                e.NewElement.RendererSizeForWidthAndFontSize += LabelF9pSize;
                e.NewElement.Draw += DrawLabel;

                if (LastDrawState == null && e.NewElement.Width > 0 && e.NewElement.Height > 0)
                {
                    var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
                    var width = e.NewElement.Width * displayScale;
                    var height = e.NewElement.Height * displayScale;
                    DrawLabel(width, height);
                }
                Control.IsNativeDrawEnabled = true;
            }
        }

        void InitControl(F9PTextView control)
        {
            if (control != null)
            {
                UpdateFont(control);
                UpdateFontSize(control);
                UpdateAlignment(control);
                UpdateColor(control);
                UpdateText(control);
            }
        }
        #endregion



        #region Element Property Change Handlers
        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateAlignment(Control);
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateColor(Control);
                Layout();
            }
            else if (e.PropertyName == Label.FontSizeProperty.PropertyName)
            {
                UpdateFontSize(Control);
                Layout();
            }
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
            {
                if (Control is F9PTextView control && Element is Forms9Patch.Label element && control.TextSize < element.MinFontSize)
                    Layout();
            }
            else if (e.PropertyName == Label.FontProperty.PropertyName || e.PropertyName == Label.FontFamilyProperty.PropertyName || e.PropertyName == Label.FontAttributesProperty.PropertyName)
            {
                UpdateFont(Control);
                Layout();
            }
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
            {
                UpdateLineBreakMode();
                Layout();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.HtmlTextProperty.PropertyName)
            {
                UpdateText(Control);
                Layout();
            }
            else if (e.PropertyName == Label.AutoFitProperty.PropertyName)
            {
                UpdateFit();
                Layout();
            }
            else if (e.PropertyName == Label.LinesProperty.PropertyName)
            {
                UpdateLines();
                Layout();
            }
            else if (e.PropertyName == Label.SynchronizedFontSizeProperty.PropertyName)
            {
                if (Element is Forms9Patch.Label element)
                {
                    _currentDrawState.SyncFontSize = (float)element.SynchronizedFontSize;
                    Layout();
                }
            }
        }

        void Layout()
        {
            if (Control is F9PTextView control)
                InternalLayout(control, _currentDrawState);
        }

        void UpdateAlignment(F9PTextView control)
        {
            if (control != null && Element is Forms9Patch.Label element)
                control.Gravity = element.HorizontalTextAlignment.ToHorizontalGravityFlags() | element.VerticalTextAlignment.ToVerticalGravityFlags();
        }

        void UpdateLineBreakMode()
        {
            if (Element is Forms9Patch.Label element)
                _currentDrawState.LineBreakMode = element.LineBreakMode;
        }


        void UpdateFontSize(F9PTextView control)
        {
            if (control != null && Element is Forms9Patch.Label element)
            {
                if (element.FittedFontSize > 0)
                    _currentDrawState.TextSize = (float)element.FittedFontSize;
                else if (element.FontSize > 0)
                    _currentDrawState.TextSize = (float)element.FontSize;
                else
                    _currentDrawState.TextSize = F9PTextView.DefaultTextSize;
                control.TextSize = _currentDrawState.TextSize;
            }
        }

        void UpdateFit()
        {
            if (Element is Forms9Patch.Label element)
                _currentDrawState.AutoFit = element.AutoFit;
        }

        void UpdateLines()
        {
            if (Element is Forms9Patch.Label element)
                _currentDrawState.Lines = element.Lines;
        }

        void UpdateColor(F9PTextView control)
        {
            if (control != null && Element is Forms9Patch.Label element)
            {
                _currentDrawState.TextColor = element.TextColor;
                if (_currentDrawState.TextColor == Xamarin.Forms.Color.Default || _currentDrawState.TextColor.IsDefault || _currentDrawState.TextColor == default)
                    control?.SetTextColor(F9PTextView.DefaultTextColor);
                else
                    control?.SetTextColor(_currentDrawState.TextColor.ToAndroid());
            }
        }

        void UpdateFont(F9PTextView control)
        {
            if (control != null && Element is Forms9Patch.Label element)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                _currentDrawState.Typeface = FontManagment.TypefaceForFontFamily(element.FontFamily) ?? element.Font.ToTypeface();
#pragma warning restore CS0618 // Type or member is obsolete
                control.Typeface = _currentDrawState.Typeface;
            }
        }

        bool UpdateText(F9PTextView control)
        {
            if (control != null && Element is Forms9Patch.Label element)
            {
                if (element.F9PFormattedString != null)
                {
                    _currentDrawState.TextFormatted = element.F9PFormattedString.ToSpannableString(noBreakSpace: element.LineBreakMode == LineBreakMode.CharacterWrap);
                    control.TextFormatted = _currentDrawState.TextFormatted;
                }
                else
                {
                    var text = element.Text;
                    if (element.LineBreakMode == LineBreakMode.CharacterWrap && !string.IsNullOrEmpty(text))
                        text = text.Replace(' ', '\u00A0');
                    _currentDrawState.Text = text;
                    control.Text = _currentDrawState.Text;
                }
            }
            return true;
        }
        #endregion


        #region Touch to Index
        int IndexAtPoint(Xamarin.Forms.Point p)
            => Control?.IndexForPoint(p.ToNativePoint()) ?? -1;
        #endregion


        #region FontSize helpers
        float BoundTextSize(double textSize) => BoundTextSize((float)textSize);

        float BoundTextSize(float textSize)
        {
            if (Element is Forms9Patch.Label element)
            {
                if (textSize < 0.0001)
#pragma warning disable CS0618 // Type or member is obsolete
                    textSize = (float)(F9PTextView.DefaultTextSize * System.Math.Abs(element.FontSize));
#pragma warning restore CS0618 // Type or member is obsolete
                if (textSize > element.FontSize && element.FontSize > 0)
                    return (float)element.FontSize;
                if (textSize < ModelMinFontSize)
                    textSize = ModelMinFontSize;
                return textSize;
            }
            return F9PTextView.DefaultTextSize;
        }

        float ModelMinFontSize
        {
            get
            {
                var minFontSize = (float)(Element?.MinFontSize ?? 4);
                if (minFontSize < 0)
                    minFontSize = 4;
                return minFontSize;
            }
        }
        #endregion


        #region For better crash diagnostics
        public override void RequestLayout()
        {
            base.RequestLayout();
        }

        public override void Invalidate()
        {
            base.Invalidate();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
        }

        public override void MeasureAndLayout(int p0, int p1, int p2, int p3, int p4, int p5)
        {
            base.MeasureAndLayout(p0, p1, p2, p3, p4, p5);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        public override void OnDrawForeground(Canvas canvas)
        {
            base.OnDrawForeground(canvas);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
        }
        #endregion
    }
}
