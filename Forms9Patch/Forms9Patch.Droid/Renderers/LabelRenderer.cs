using System;
using System.ComponentModel;
using System.Threading;
using Android.Content.Res;
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


        static int _instances;
        public int _instance;

        ColorStateList _labelTextColorDefault;

        static float _aStupidWayToImplementFontScaling = 1.0f;

        #region Constructor / Disposer
#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRenderer"/> class.
        /// </summary>
        public LabelRenderer()
            => InstanceInit();
#pragma warning restore CS0618 // Type or member is obsolete

        public LabelRenderer(Android.Content.Context context) : base(context)
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
        F9PTextView _measureControl;
        TextControlState _currentMeasureState;
        SizeRequest? _lastMeasureResult;
        TextControlState _lastMeasureState;

        TextControlState _currentDrawState;
        SizeRequest? _lastDrawResult;
        TextControlState _lastDrawState;


        Xamarin.Forms.Size LabelF9pSize(double widthConstraint, double fontSize)
        {
            if (_currentDrawState == null || _currentDrawState.IsEmpty || Control == null || Element == null || _disposed)
                return Xamarin.Forms.Size.Zero;

            var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = (int)System.Math.Floor(System.Math.Min(widthConstraint * displayScale, int.MaxValue / 3)),
                AvailHeight = int.MaxValue / 3,
                TextSize = (float)fontSize,
            };

            //P42.Utils.Debug.Message(Element, "ENTER  _currentControlState.AvailWidth=[" + _currentMeasureState.AvailWidth + "]  _currentControlState.AvailHeight=[" + _currentMeasureState.AvailHeight + "]");
            ////P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");

            /* This doesn't seem to be working!!!

            if (_lastMeasureResult != null && _lastMeasureResult.HasValue
                && _lastMeasureState == _currentMeasureState
                && _lastMeasureResult.Value.Request.Width > 0
                && _lastMeasureResult.Value.Request.Height > 0
                && _lastMeasureResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastMeasureResult.Value.Request.Height <= _currentMeasureState.AvailHeight
                )
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastMeasureResult.Value + "]");
                return _lastMeasureResult.Value.Request;
            }

            if (_lastDrawResult != null && _lastDrawResult.HasValue
                && _lastDrawState == _currentMeasureState
                && _lastDrawResult.Value.Request.Width > 0
                && _lastDrawResult.Value.Request.Height > 0
                && _lastDrawResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastDrawResult.Value.Request.Height <= _currentMeasureState.AvailWidth
                )
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDrawResult.Value + "]");
                return _lastDrawResult.Value.Request;
            }

*/
            _measureControl = _measureControl ?? new F9PTextView(Settings.Context);

            _lastMeasureResult = InternalLayout(_measureControl, _currentMeasureState);
            _lastMeasureState = new TextControlState(_currentMeasureState);

            var result = new Xamarin.Forms.Size(_lastMeasureResult.Value.Request.Width / displayScale, _lastMeasureResult.Value.Request.Height / displayScale);
            //P42.Utils.Debug.Message(Element, "EXIT result = [" + result + "]  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            return result;
        }

        SizeRequest DrawLabel(double width, double height)
        {
            if (_currentDrawState == null || _currentDrawState.IsEmpty || Control == null || Element == null || _disposed)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            if (width < 0 || height < 0)
                return new SizeRequest(Xamarin.Forms.Size.Zero);


            if (double.IsInfinity(width))
                width = int.MaxValue / 3;
            if (double.IsInfinity(height))
                height = int.MaxValue / 3;

            var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            _currentDrawState.AvailWidth = (int)System.Math.Floor(width * displayScale);
            _currentDrawState.AvailHeight = (int)System.Math.Floor(height * displayScale);

            //P42.Utils.Debug.Message(Element, "ENTER  _currentDrawState.AvailWidth=[" + _currentDrawState.AvailWidth + "]  _currentDrawState.AvailHeight=[" + _currentDrawState.AvailHeight + "] Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            //P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");

            /* This seems to work */
            if (_lastDrawResult != null && _lastDrawResult.HasValue
                && _lastDrawState == _currentDrawState
                && _lastDrawResult.Value.Request.Width > 0
                && _lastDrawResult.Value.Request.Height > 0
                && _lastDrawState.RenderedFontSize >= _currentDrawState.TextSize
                && _lastDrawResult.Value.Request.Width <= _currentDrawState.AvailWidth
                //&& System.Math.Abs(_lastDrawResult.Value.Request.Width - _currentDrawState.AvailWidth) < 5
                && _lastDrawResult.Value.Request.Height <= _currentDrawState.AvailHeight
                //&& System.Math.Abs(_lastDrawResult.Value.Request.Height - _currentDrawState.AvailHeight) < 5
                )
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDrawResult.Value + "]");
                return _lastDrawResult.Value;
            }


            _lastDrawResult = InternalLayout(Control, _currentDrawState);
            _lastDrawState = new TextControlState(_currentDrawState);

            //P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastDrawResult + "]  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            return _lastDrawResult.Value;
        }

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            if (_currentDrawState == null || _currentDrawState.IsEmpty || Control == null || Element == null || _disposed)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            var width = MeasureSpec.GetSize(widthConstraint);
            if (MeasureSpec.GetMode(widthConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                width = int.MaxValue / 2;
            var height = MeasureSpec.GetSize(heightConstraint);
            if (MeasureSpec.GetMode(heightConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                height = int.MaxValue / 2;

            if (width <= 0 || height <= 0)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            _currentMeasureState = new TextControlState(_currentDrawState)
            {
                AvailWidth = width,
                AvailHeight = height
            };

            //P42.Utils.Debug.Message(Element, "ENTER  _currentControlState.AvailWidth=[" + _currentMeasureState.AvailWidth + "]  _currentControlState.AvailHeight=[" + _currentMeasureState.AvailHeight + "] Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + " Height=[" + Height + "]]");
            //P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");

            if (_lastMeasureResult != null && _lastMeasureResult.HasValue
                && _lastMeasureState == _currentMeasureState
                && _lastMeasureResult.Value.Request.Width > 0
                && _lastMeasureResult.Value.Request.Height > 0
                && _lastMeasureState.RenderedFontSize >= _currentMeasureState.TextSize
                && _lastMeasureResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastMeasureResult.Value.Request.Height <= _currentMeasureState.AvailHeight
                )
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastMeasureResult.Value + "]");
                return _lastMeasureResult.Value;
            }

            if (_lastDrawResult != null && _lastDrawResult.HasValue
                && _lastDrawState == _currentMeasureState
                && _lastDrawResult.Value.Request.Width > 0
                && _lastDrawResult.Value.Request.Height > 0
                && _lastDrawState.RenderedFontSize >= _currentDrawState.TextSize
                && _lastDrawResult.Value.Request.Width <= _currentMeasureState.AvailWidth
                && _lastDrawResult.Value.Request.Height <= _currentMeasureState.AvailWidth)
            {
                //P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDrawResult.Value + "]");
                return _lastDrawResult.Value;
            }

            _measureControl = _measureControl ?? new F9PTextView(Settings.Context);

            _lastMeasureResult = InternalLayout(_measureControl, _currentMeasureState);
            _lastMeasureState = new TextControlState(_currentMeasureState);

            //P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastMeasureResult + "]  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            return _lastMeasureResult.Value;
        }

        SizeRequest InternalLayout(F9PTextView control, TextControlState state)
        {
            var element = Element;
            if (element == null)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            //P42.Utils.Debug.Message(Element, "ENTER  state.AvailWidth=[" + state.AvailWidth + "]  state.AvailHeight=[" + state.AvailHeight + "]  element.Size=[" + Element?.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            //P42.Utils.Debug.Message(Element, "control.TextSize=[" + control.TextSize + "] element.FontSize=[" + element.FontSize + "]");

            ICharSequence text = state.JavaText;
            var tmpFontSize = BoundTextSize(element.FontSize);
            control.Typeface = state.Typeface;
            control.SetTextColor(state.TextColor.ToAndroid());
            control.TextSize = tmpFontSize;

            //P42.Utils.Debug.Message(Element, "control.TypeFace=[" + control.Typeface + "] ");
            //P42.Utils.Debug.Message(Element, "control.TextColor=[" + string.Join(", ", control.TextColors) + "]");
            //P42.Utils.Debug.Message(Element, "control.TextSize=[" + control.TextSize + "]");

            _aStupidWayToImplementFontScaling = control.TextSize / tmpFontSize;
            //control.TextSize = tmpFontSize / _aStupidWayToImplementFontScaling;

            /*
            if (P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false)
            {
                var metrics = Resources.DisplayMetrics;
                var density = metrics.DensityDpi;
                var displayScale = (float)density / (float)Android.Util.DisplayMetricsDensity.Default;// DisplayMetrics.DensityDefault;

                //P42.Utils.Debug.Message(Element, "control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "] fontScale=[" + _aStupidWayToImplementFontScaling + "] displayScale=[" + displayScale + "]");
            }
            */

            control.IsNativeDrawEnabled = false;
            control.SetSingleLine(false);
            control.SetMaxLines(int.MaxValue / 2);
            control.SetIncludeFontPadding(false);
            control.Ellipsize = null;

            int tmpHt = -1;
            int tmpWd = -1;

            var fontMetrics = control.Paint.GetFontMetrics();
            var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
            var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);

            //P42.Utils.Debug.Message(Element, "element.Line=[" + element.Lines + "] _currentControlState.Linst=[" + state.Lines + "]");

            if (state.Lines == 0)
            {
                if (state.AvailHeight < int.MaxValue / 3)
                {
                    tmpFontSize = TextPaintExtensions.ZeroLinesFit(state.JavaText, new TextPaint(control.Paint), ModelMinFontSize, tmpFontSize, state.AvailWidth, state.AvailHeight);
                    //P42.Utils.Debug.Message(Element, "ZeroLinesFit tmpFontSize=[" + tmpFontSize + "]");
                }
            }
            else
            {
                if (state.AutoFit == AutoFit.Lines)
                {
                    if (state.AvailHeight > int.MaxValue / 3)
                    {
                        tmpHt = (int)System.Math.Round(state.Lines * fontLineHeight + (state.Lines - 1) * fontLeading);
                        //P42.Utils.Debug.Message(Element, "AutoFit.Lines A (MAX HEIGHT) tmpHt=[" + tmpHt + "]");
                    }
                    else
                    {
                        var fontPointSize = tmpFontSize;
                        var lineHeightRatio = fontLineHeight / fontPointSize;
                        var leadingRatio = fontLeading / fontPointSize;
                        tmpFontSize = ((state.AvailHeight / (state.Lines + leadingRatio * (state.Lines - 1))) / lineHeightRatio - 0.1f);
                        if ((Element?.HtmlText ?? Element?.Text ?? "") == "7")
                            System.Diagnostics.Debug.WriteLine(GetType() + ".InternalLayout AvailHeight[" + state.AvailHeight + "] lines=[" + state.Lines + "] leadingRatio=[" + leadingRatio + "] lineHeighRatio=[" + lineHeightRatio + "]");
                        //P42.Utils.Debug.Message(Element, "AutoFit.Lines B (FIXED HT) tmpFontSize=[" + tmpFontSize + "]");
                    }
                }
                else if (state.AutoFit == AutoFit.Width)
                {
                    tmpFontSize = TextPaintExtensions.WidthFit(state.JavaText, new TextPaint(control.Paint), state.Lines, ModelMinFontSize, tmpFontSize, state.AvailWidth, state.AvailHeight);
                    //P42.Utils.Debug.Message(Element, "AutoFit.Width tmpFontSize=[" + tmpFontSize + "]");
                }
            }

            //P42.Utils.Debug.Message(Element, "Fit Complete: control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "]  element.Size=[" + element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            tmpFontSize = BoundTextSize(tmpFontSize);
            //P42.Utils.Debug.Message(Element, "Bound Complete: control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "]");

            // this is the optimal font size.  Let it be known!
            if (System.Math.Abs(tmpFontSize - element.FittedFontSize) > 0.1)
            {
                if (Element != null && control != null)  // multipicker test was getting here with Element and control both null
                {
                    if (System.Math.Abs(tmpFontSize - element.FontSize) < 0.1 || (element.FontSize < 0 && System.Math.Abs(tmpFontSize - F9PTextView.DefaultTextSize) < 0.1))
                        element.FittedFontSize = -1;
                    else
                        element.FittedFontSize = tmpFontSize;
                }
                //P42.Utils.Debug.Message(Element, "element.FittedFontSize=[" + tmpFontSize + "]");
            }

            var syncFontSize = (float)((ILabel)Element).SynchronizedFontSize;
            if (syncFontSize >= 0 && System.Math.Abs(tmpFontSize - syncFontSize) > 0.1)
            {
                tmpFontSize = syncFontSize;
                //P42.Utils.Debug.Message(Element, "syncFontSize=[" + syncFontSize + "]");
            }

            control.TextSize = tmpFontSize;
            state.RenderedFontSize = tmpFontSize;
            var layout = TextExtensions.StaticLayout(state.JavaText, new TextPaint(control.Paint), state.AvailWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            //P42.Utils.Debug.Message(Element, "Post STATIC LAYOUT element.Size=[" + element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");

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
                //P42.Utils.Debug.Message(Element, "lines=[" + lines + "]");
            }
            if (layout.Height > state.AvailHeight || (lines > 0 && layout.LineCount > lines))
            {
                if (state.Lines == 1)
                {
                    control.SetSingleLine(true);
                    control.SetMaxLines(1);
                    control.Ellipsize = state.LineBreakMode.ToEllipsize();
                    //P42.Utils.Debug.Message(Element, "Ellipsize");
                }
                else
                {
                    layout = TextPaintExtensions.Truncate(state.Text, element.F9PFormattedString, new TextPaint(control.Paint), state.AvailWidth, state.AvailHeight, element.AutoFit, element.LineBreakMode, ref lines, ref text);
                    //P42.Utils.Debug.Message(Element, "Truncate");
                }
            }
            lines = lines > 0 ? System.Math.Min(lines, layout.LineCount) : layout.LineCount;
            for (int i = 0; i < lines; i++)
            {
                tmpHt = layout.GetLineBottom(i);
                var width = layout.GetLineWidth(i);
                //P42.Utils.Debug.Message(Element, "Line[" + i + "] w:[" + width + "] h:[" + layout.GetLineBottom(i) + "] layout.LineMax[" + layout.GetLineMax(i) + "] layout.Height[" + layout.Height + "]");
                //System.Diagnostics.Debug.WriteLine("\t\tright=["+right+"]");
                if (width > tmpWd)
                    tmpWd = (int)System.Math.Ceiling(width);
            }
            //P42.Utils.Debug.Message(Element, "tmpWd=[" + tmpWd + "] layout.Width=[" + layout.Width + "]");
            if (state.AutoFit == AutoFit.None && state.Lines > 0)
                control.SetMaxLines(state.Lines);

            //System.Diagnostics.Debug.WriteLine("\tLabelRenderer.GetDesiredSize\ttmp.size=[" + tmpWd + ", " + tmpHt + "]");
            if (element.IsDynamicallySized && state.Lines > 0 && state.AutoFit == AutoFit.Lines)
            {
                fontMetrics = control.Paint.GetFontMetrics();
                fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
                fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
                tmpHt = (int)(fontLineHeight * state.Lines + fontLeading * (state.Lines - 1));
                //P42.Utils.Debug.Message(Element, "DynamicallySized: tmpHt=[" + tmpHt + "]");
            }

            control.Gravity = element.HorizontalTextAlignment.ToHorizontalGravityFlags() | element.VerticalTextAlignment.ToVerticalGravityFlags();

            if (element.Text != null)
            {
                control.Text = text.ToString();
                //P42.Utils.Debug.Message(Element, "control.Text=" + control.Text);
            }
            else
            {
                control.TextFormatted = text;
                //P42.Utils.Debug.Message(Element, "control.TextFormatted=" + control.TextFormatted);
            }


            var result = new SizeRequest(new Xamarin.Forms.Size(System.Math.Ceiling((double)tmpWd), System.Math.Ceiling((double)tmpHt)), new Xamarin.Forms.Size(10, System.Math.Ceiling((double)tmpHt)));
            /*
            if (P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false)
            {
                //control.SetWidth((int)_lastSizeRequest.Value.Request.Width);
                //control.SetHeight((int)_lastSizeRequest.Value.Request.Height);

                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t[" + elementText + "] ");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize(" + (state.AvailWidth > int.MaxValue / 3 ? "infinity" : state.AvailWidth.ToString()) + "," + (state.AvailHeight > int.MaxValue / 3 ? "infinity" : state.AvailHeight.ToString()) + ") exit (" + result + ")");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Visibility=[" + control.Visibility + "]");
                //System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.TextFormatted=[" + control.TextFormatted + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.TextSize=[" + control.TextSize + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.ClipBounds=[" + control.ClipBounds?.Width() + "," + control.ClipBounds?.Height() + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Width[" + control.Width + "]  .Height=[" + control.Height + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.GetX[" + control.GetX() + "]  .GetY[" + control.GetY() + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Alpha[" + control.Alpha + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Background[" + control.Background + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Elevation[" + control.Elevation + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Enabled[" + control.Enabled + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.Error[" + control.Error + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.IsOpaque[" + control.IsOpaque + "]");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t\tControl.IsShown[" + control.IsShown + "]");
                //control.BringToFront();
                //System.Diagnostics.Debug.WriteLine("\t\t");
            }
            */

            if (element.LineBreakMode == LineBreakMode.NoWrap)
                control.SetSingleLine(true);
            /*
            if (P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Pink);
                //P42.Utils.Debug.Message(Element, "Visibility = " + Control.Visibility);
            }
            */
            //P42.Utils.Debug.Message(Element, "EXIT _lastSizeRequest=[" + result + "]  element.Size=[" + element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            control.IsNativeDrawEnabled = true;
            if (control == Control)
                control.RequestLayout();

            return result;
        }
        #endregion


        #region Element Change Handler

        readonly object _lock = new object();

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            //lock (_lock)
            {

                //P42.Utils.Debug.Message(Element, "ENTER e.OldElement=[" + e.OldElement + "] Bounds=[" + e.OldElement?.Bounds + "] e.NewElement=[" + e.NewElement + "] e.NewElement.Bounds=[" + e.NewElement?.Bounds + "] Width=[" + Width + "] Height=[" + Height + "]");
                base.OnElementChanged(e);

                if (Control == null)
                {
                    var view = new F9PTextView(Context);
                    _labelTextColorDefault = view.TextColors;
                    SetNativeControl(view);
                }
                Control.IsNativeDrawEnabled = false;

                if (e.OldElement != null)
                {
                    e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
                    e.OldElement.RendererSizeForWidthAndFontSize -= LabelF9pSize;
                    e.OldElement.Draw -= DrawLabel;

                    Control.SkipNextInvalidate();
                }
                if (e.NewElement != null)
                {
                    e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                    e.NewElement.RendererSizeForWidthAndFontSize += LabelF9pSize;
                    e.NewElement.Draw += DrawLabel;
                    _currentDrawState = new TextControlState
                    {
                        Lines = Element.Lines,
                        AutoFit = Element.AutoFit,
                        LineBreakMode = Element.LineBreakMode,
                        SyncFontSize = (float)Element.SynchronizedFontSize,
                    };
                    _lastMeasureState = null;
                    _lastDrawResult = null;
                    _lastMeasureResult = null;
                    _measureControl?.Dispose();
                    _measureControl = null;

                    UpdateColor(Control);
                    UpdateFont(Control);
                    UpdateFontSize(Control);
                    UpdateAlignment(Control);
                    UpdateText(Control);

                    if (Element.Width > 0 && Element.Height > 0)
                    {
                        var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
                        DrawLabel(Element.Width * displayScale, Element.Height * displayScale);
                    }
                }
                Control.IsNativeDrawEnabled = true;

                //P42.Utils.Debug.Message(Element, "EXIT  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
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
            //P42.Utils.Debug.Message(Element, "ENTER  [" + e.PropertyName + "]  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
            {
                UpdateAlignment(Control);
            }
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateColor(Control);
            }
            else if (e.PropertyName == Label.FontSizeProperty.PropertyName)
            {
                UpdateFontSize(Control);
            }
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
            {
                if (Control.TextSize < Element.MinFontSize)
                    RequestLayout();
            }
            else if (e.PropertyName == Label.FontProperty.PropertyName || e.PropertyName == Label.FontFamilyProperty.PropertyName || e.PropertyName == Label.FontAttributesProperty.PropertyName)
            {
                UpdateFont(Control);
            }
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
            {
                UpdateLineBreakMode();
                RequestLayout();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.HtmlTextProperty.PropertyName)
            {
                UpdateText(Control);
            }
            else if (e.PropertyName == Label.AutoFitProperty.PropertyName)
            {
                UpdateFit();
                RequestLayout();
            }
            else if (e.PropertyName == Label.LinesProperty.PropertyName)
            {
                UpdateLines();
                RequestLayout();
            }
            else if (e.PropertyName == Label.SynchronizedFontSizeProperty.PropertyName)
            {
                _currentDrawState.SyncFontSize = (float)Element.SynchronizedFontSize;
                RequestLayout();
            }
            //P42.Utils.Debug.Message(Element, "EXIT [" + e.PropertyName + "]  Element.Size=[" + Element.Bounds.Size + "] Width=[" + Width + "] Height=[" + Height + "]");
        }

        void UpdateAlignment(F9PTextView control)
        {
            if (Element is Forms9Patch.Label element)
                //lock (_lock)
                control.Gravity = element.HorizontalTextAlignment.ToHorizontalGravityFlags() | element.VerticalTextAlignment.ToVerticalGravityFlags();
        }

        void UpdateLineBreakMode()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            _currentDrawState.LineBreakMode = Element.LineBreakMode;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateFontSize(F9PTextView control)
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            _currentDrawState.TextSize = (float)Element.FontSize;
            //lock (_lock)
            control.TextSize = _currentDrawState.TextSize;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateFit()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            _currentDrawState.AutoFit = Element.AutoFit;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateLines()
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            _currentDrawState.Lines = Element.Lines;
            //P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateColor(F9PTextView control)
        {
            if (_currentDrawState.TextColor == Element.TextColor)
                return;
            _currentDrawState.TextColor = Element.TextColor;
            if (_currentDrawState.TextColor == Xamarin.Forms.Color.Default)
                control?.SetTextColor(_labelTextColorDefault);
            else
                control?.SetTextColor(_currentDrawState.TextColor.ToAndroid());
        }

        bool _updateFontPending;
        void UpdateFont(F9PTextView control)
        {
            _updateFontPending = false;
#pragma warning disable CS0618 // Type or member is obsolete
            _currentDrawState.Typeface = FontManagment.TypefaceForFontFamily(Element.FontFamily) ?? Element.Font.ToTypeface();
#pragma warning restore CS0618 // Type or member is obsolete
            control.Typeface = _currentDrawState.Typeface;
            return;
        }

        bool UpdateText(F9PTextView control)
        {
            //P42.Utils.Debug.Message(Element, "ENTER");
            if (Element.F9PFormattedString != null)
            {
                _currentDrawState.TextFormatted = Element.F9PFormattedString.ToSpannableString(noBreakSpace: Element.LineBreakMode == LineBreakMode.CharacterWrap);
                control.TextFormatted = _currentDrawState.TextFormatted;
            }
            else
            {
                var text = Element.Text;
                if (Element.LineBreakMode == LineBreakMode.CharacterWrap)
                    text = Element.Text.Replace(' ', '\u00A0');
                _currentDrawState.Text = text;
                control.Text = _currentDrawState.Text;
            }
            //P42.Utils.Debug.Message(Element, "EXIT");
            return true;
        }
        #endregion


        #region Touch to Index
        int IndexAtPoint(Xamarin.Forms.Point p)
            => Control.IndexForPoint(p.ToNativePoint());
        #endregion


        #region FontSize helpers
        float BoundTextSize(double textSize) => BoundTextSize((float)textSize);

        float BoundTextSize(float textSize)
        {
            if (textSize < 0.0001)
#pragma warning disable CS0618 // Type or member is obsolete
                textSize = (float)(F9PTextView.DefaultTextSize * System.Math.Abs(Element.FontSize));
#pragma warning restore CS0618 // Type or member is obsolete
            if (textSize > Element.FontSize)
                return (float)Element.FontSize;
            if (textSize < ModelMinFontSize)
                textSize = ModelMinFontSize;
            return textSize;
        }

        float ModelMinFontSize
        {
            get
            {
                var minFontSize = (float)Element.MinFontSize;
                if (minFontSize < 0)
                    minFontSize = 4;// Element.FontSize > 0 ? (float)Element.FontSize : (float)(F9PTextView.DefaultTextSize * System.Math.Abs(Element.FontSize));
                return minFontSize;
            }
        }

        #endregion


    }
}