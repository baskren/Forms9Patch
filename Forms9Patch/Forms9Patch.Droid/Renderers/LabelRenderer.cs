using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using Android.Views;
using Android.Util;
using System;
using FormsGestures.Droid;
using Java.Interop;
using System.Runtime.InteropServices;
using Android.Drm;
using Android.OS;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(Forms9Patch.Label), typeof(Forms9Patch.Droid.LabelRenderer))]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// The Forms9Patch Label renderer.
    /// </summary>
    public class LabelRenderer : ViewRenderer<Label, F9PTextView>
    {

        #region Debug support
        /*
        bool DebugCondition //=> false;// (Element?.HtmlText ?? Element?.Text)?.Equals("5") ?? false;
        => (Element?.HtmlText ?? Element?.Text)?.ToLower().StartsWith("heights") ?? false;

        void DebugMessage(string message, [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            if (DebugCondition)
            {
                System.Diagnostics.Debug.IndentSize = 4;
                if (message?.Contains("ENTER") ?? false)
                {
                    if (System.Diagnostics.Debug.IndentLevel == 0)
                        System.Diagnostics.Debug.WriteLine("=========================================================");
                }
                if (message?.Contains("EXIT") ?? false)
                {
                    System.Diagnostics.Debug.Unindent();
                }
                System.Diagnostics.Debug.WriteLine(methodName + ": " + message);
                if (message?.Contains("ENTER") ?? false)
                    System.Diagnostics.Debug.Indent();
                if (message?.Contains("EXIT") ?? false)
                {
                    if (System.Diagnostics.Debug.IndentLevel == 0)
                        System.Diagnostics.Debug.WriteLine("=========================================================");
                }
            }
        }
        */



        void DebugGetDesiredRequest(string mark, double widthConstraint, double heightConstraint, SizeRequest request, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null)
        {
            P42.Utils.Debug.Message(mark + " Constr=[" + widthConstraint + "," + heightConstraint + "] " + DebugControlSizes() + " result=[" + request + "]", callerName);
        }

        string DebugControlSizes()
        {
            if (Control != null)
            {
                var result = "Ctrl.MaxLines=[" + Control.MaxLines + "] .Width=[" + Control?.Width + "] .Height=[" + Control?.Height + "] ";
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



        static int _instances = 0;
        int _instance;
        //double _fittedFontSize = -1;

        ColorStateList _labelTextColorDefault;

        static float _aStupidWayToImplementFontScaling = 1.0f;

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRenderer"/> class.
        /// </summary>
        public LabelRenderer()
        {
            _instance = _instances++;
            AutoPackage = false;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public LabelRenderer(Android.Content.Context context) : base(context)
        {
            _instance = _instances++;
            AutoPackage = false;
        }


        bool showDebugMsg
        {
            get
            {
                return false;
            }
        }

        string elementText
        {
            get
            {
                return (Element.HtmlText ?? Element.Text ?? "");
            }
        }

        Xamarin.Forms.Size LabelXamarinSize(double widthConstraint, double fontSize)
        {
            var layout = LabelLayout((widthConstraint > double.MaxValue / 3 ? int.MaxValue / 2 : (int)widthConstraint), (float)fontSize);
            if (layout == null)
                return Xamarin.Forms.Size.Zero;
            P42.Utils.Debug.Message(Element, "ENTER widthConstraint=[" + widthConstraint + "] fontSize=[" + fontSize + "]");
            float width = 0;
            float height = 0;
            for (int i = 0; i < layout.LineCount; i++)
            {
                var lineWidth = layout.GetLineWidth(i);
                if (width < lineWidth)
                    width = lineWidth;
                var blockHeight = layout.GetLineBottom(i);
                if (height < blockHeight)
                    height = blockHeight;
            }
            var result = new Xamarin.Forms.Size(System.Math.Ceiling(width), System.Math.Ceiling(height));
            P42.Utils.Debug.Message(Element, "EXIT result=[" + result + "]");
            return result;
        }

        StaticLayout LabelLayout(int widthConstraint, float fontSize)
        {
            if (Element == null)
                return null;
            if (_currentDrawState?.JavaText == null)
                return null;
            P42.Utils.Debug.Message(Element, "ENTER widthConstraint=[" + widthConstraint + "] fontSize=[" + fontSize + "]");
            if (fontSize < 0.001)
                fontSize = F9PTextView.DefaultTextSize;
            var paint = new TextPaint(Control.Paint);
            paint.TextSize = fontSize * _aStupidWayToImplementFontScaling;
            var layout = TextExtensions.StaticLayout(_currentDrawState.JavaText, paint, widthConstraint, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);
            P42.Utils.Debug.Message(Element, "EXIT paint.TextSize=[" + paint.TextSize + "] fontSize=[" + fontSize + "] size=[" + layout.Width + ", " + layout.Height + "]");
            return layout;
        }

        /*
        void LayoutForSize(int width, int height)
        {
            P42.Utils.Debug.Message(Element, "ENTER width=[" + width + "] height=[" + height + "]");
            //var widthConstraint = MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.AtMost);
            //var heightConstraint = MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost);
            //var result = GetDesiredSize(widthConstraint, heightConstraint);
            var result = DrawLabel(width, height);
            P42.Utils.Debug.Message(Element, "EXIT result=[" + result + "]");
        }
        */


        F9PTextView _measureControl;
        TextControlState _currentDesiredSizeState;
        SizeRequest? _lastDesiredSizeResult;
        TextControlState _lastDesiredSizeState;

        TextControlState _currentDrawState;
        SizeRequest? _lastDrawResult;
        TextControlState _lastDrawState;



        SizeRequest DrawLabel(double width, double height)
        {
            if (_currentDrawState.IsNullOrEmpty || Control == null || Element == null)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            if (width < 0 || height < 0)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            //_currentDrawState = _currentDrawState ?? new TextControlState(_currentDesiredSizeState);
            var displayScale = (float)Resources.DisplayMetrics.DensityDpi / (float)Android.Util.DisplayMetricsDensity.Default;
            _currentDrawState.AvailWidth = (int)System.Math.Floor(width * displayScale);
            _currentDrawState.AvailHeight = (int)System.Math.Floor(height * displayScale);

            P42.Utils.Debug.Message(Element, "ENTER  _currentDrawState.AvailWidth=[" + _currentDrawState.AvailWidth + "]  _currentDrawState.AvailHeight=[" + _currentDrawState.AvailHeight + "]");
            P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");

            if (_currentDrawState == _lastDrawState && _lastDrawResult.HasValue)
            {
                P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDesiredSizeResult.Value + "]");
                return _lastDesiredSizeResult.Value;
            }

            _lastDrawResult = InternalLayout(Control, _currentDrawState);
            _lastDrawState = new TextControlState(_currentDrawState);

            P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastDrawResult + "]");
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
            if (_currentDrawState.IsNullOrEmpty || Control == null || Element == null)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            var width = MeasureSpec.GetSize(widthConstraint);
            if (MeasureSpec.GetMode(widthConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                width = int.MaxValue / 2;
            var height = MeasureSpec.GetSize(heightConstraint);
            if (MeasureSpec.GetMode(heightConstraint) == Android.Views.MeasureSpecMode.Unspecified)
                height = int.MaxValue / 2;

            if (width <= 0 || height <= 0)
                return new SizeRequest(Xamarin.Forms.Size.Zero);

            _currentDesiredSizeState = new TextControlState(_currentDrawState)
            {
                AvailWidth = width,
                AvailHeight = height
            };

            P42.Utils.Debug.Message(Element, "ENTER  _currentControlState.AvailWidth=[" + _currentDesiredSizeState.AvailWidth + "]  _currentControlState.AvailHeight=[" + _currentDesiredSizeState.AvailHeight + "]");
            //P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");

            if (_currentDesiredSizeState == _lastDesiredSizeState && _lastDesiredSizeResult.HasValue)
            {
                P42.Utils.Debug.Message(Element, "EXIT reuse _lastSizeRequest=[" + _lastDesiredSizeResult.Value + "]");
                return _lastDesiredSizeResult.Value;
            }

            _measureControl = _measureControl ?? new F9PTextView(Settings.Context);

            _lastDesiredSizeResult = InternalLayout(_measureControl, _currentDesiredSizeState);
            _lastDesiredSizeState = new TextControlState(_currentDesiredSizeState);

            P42.Utils.Debug.Message(Element, "EXIT result = [" + _lastDesiredSizeResult + "]");
            return _lastDesiredSizeResult.Value;
        }

        SizeRequest InternalLayout(F9PTextView control, TextControlState currentState)
        {

            P42.Utils.Debug.Message(Element, "ENTER  _currentControlState.AvailWidth=[" + currentState.AvailWidth + "]  _currentControlState.AvailHeight=[" + currentState.AvailHeight + "]");
            P42.Utils.Debug.Message(Element, "Control.TextSize=[" + Control.TextSize + "] Element.FontSize=[" + Element.FontSize + "]");


            ICharSequence text = currentState.JavaText;
            var tmpFontSize = BoundTextSize(Element.FontSize);
            control.Typeface = currentState.Typeface;
            control.SetTextColor(currentState.TextColor.ToAndroid());
            control.TextSize = tmpFontSize;
            _aStupidWayToImplementFontScaling = control.TextSize / tmpFontSize;
            //control.TextSize = tmpFontSize / _aStupidWayToImplementFontScaling;

            if (P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false)
            {
                var metrics = Resources.DisplayMetrics;
                var density = metrics.DensityDpi;
                var displayScale = (float)density / (float)Android.Util.DisplayMetricsDensity.Default;// DisplayMetrics.DensityDefault;

                P42.Utils.Debug.Message(Element, "control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "] fontScale=[" + _aStupidWayToImplementFontScaling + "] displayScale=[" + displayScale + "]");
            }

            control.SetSingleLine(false);
            control.SetMaxLines(int.MaxValue / 2);
            control.SetIncludeFontPadding(false);
            control.Ellipsize = null;

            int tmpHt = -1;
            int tmpWd = -1;

            var fontMetrics = control.Paint.GetFontMetrics();
            var fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
            var fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);

            P42.Utils.Debug.Message(Element, "Element.Line=[" + Element.Lines + "] _currentControlState.Linst=[" + currentState.Lines + "]");

            if (currentState.Lines == 0)
            {
                if (currentState.AvailHeight < int.MaxValue / 3)
                {
                    tmpFontSize = F9PTextView.ZeroLinesFit(currentState.JavaText, new TextPaint(control.Paint), ModelMinFontSize, tmpFontSize, currentState.AvailWidth, currentState.AvailHeight);
                    P42.Utils.Debug.Message(Element, "ZeroLinesFit tmpFontSize=[" + tmpFontSize + "]");
                }
            }
            else
            {
                if (currentState.AutoFit == AutoFit.Lines)
                {
                    if (currentState.AvailHeight > int.MaxValue / 3)
                    {
                        tmpHt = (int)System.Math.Round(currentState.Lines * fontLineHeight + (currentState.Lines - 1) * fontLeading);
                        P42.Utils.Debug.Message(Element, "AutoFit.Lines A (MAX HEIGHT) tmpHt=[" + tmpHt + "]");
                    }
                    else
                    {
                        var fontPointSize = tmpFontSize;
                        var lineHeightRatio = fontLineHeight / fontPointSize;
                        var leadingRatio = fontLeading / fontPointSize;
                        tmpFontSize = ((currentState.AvailHeight / (currentState.Lines + leadingRatio * (currentState.Lines - 1))) / lineHeightRatio - 0.1f);
                        P42.Utils.Debug.Message(Element, "AutoFit.Lines B (FIXED HT) tmpFontSize=[" + tmpFontSize + "]");
                    }
                }
                else if (currentState.AutoFit == AutoFit.Width)
                {
                    tmpFontSize = F9PTextView.WidthFit(currentState.JavaText, new TextPaint(control.Paint), currentState.Lines, ModelMinFontSize, tmpFontSize, currentState.AvailWidth, currentState.AvailHeight);
                    P42.Utils.Debug.Message(Element, "AutoFit.Width tmpFontSize=[" + tmpFontSize + "]");
                }
            }

            P42.Utils.Debug.Message(Element, "Fit Complete: control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "]");
            tmpFontSize = BoundTextSize(tmpFontSize);
            P42.Utils.Debug.Message(Element, "Bound Complete: control.TextSize=[" + control.TextSize + "] tmpFontSize=[" + tmpFontSize + "]");

            // this is the optimal font size.  Let it be known!
            if (System.Math.Abs(tmpFontSize - Element.FittedFontSize) > 0.1)
            {
                if (Element != null && control != null)  // multipicker test was getting here with Element and control both null
                {
                    if (System.Math.Abs(tmpFontSize - Element.FontSize) < 0.1 || (Element.FontSize < 0 && System.Math.Abs(tmpFontSize - F9PTextView.DefaultTextSize) < 0.1))
                        Element.FittedFontSize = -1;
                    else
                        Element.FittedFontSize = tmpFontSize;
                }
                P42.Utils.Debug.Message(Element, "Element.FittedFontSize=[" + tmpFontSize + "]");
            }

            var syncFontSize = (float)((ILabel)Element).SynchronizedFontSize;
            if (syncFontSize >= 0 && System.Math.Abs(tmpFontSize - syncFontSize) > 0.1)
            {
                tmpFontSize = syncFontSize;
                P42.Utils.Debug.Message(Element, "syncFontSize=[" + syncFontSize + "]");
            }

            control.TextSize = tmpFontSize;

            var layout = TextExtensions.StaticLayout(currentState.JavaText, new TextPaint(control.Paint), currentState.AvailWidth, Android.Text.Layout.Alignment.AlignNormal, 1.0f, 0.0f, true);

            int lines = currentState.Lines;
            if (lines == 0 && currentState.AutoFit == AutoFit.None)
            {
                for (int i = 0; i < layout.LineCount; i++)
                {
                    if (layout.GetLineBottom(i) <= currentState.AvailHeight - layout.TopPadding - layout.BottomPadding)
                        lines++;
                    else
                        break;
                }
                P42.Utils.Debug.Message(Element, "lines=[" + lines + "]");
            }
            if (layout.Height > currentState.AvailHeight || (lines > 0 && layout.LineCount > lines))
            {
                if (currentState.Lines == 1)
                {
                    control.SetSingleLine(true);
                    control.SetMaxLines(1);
                    control.Ellipsize = currentState.LineBreakMode.ToEllipsize();
                    P42.Utils.Debug.Message(Element, "Ellipsize");
                }
                else
                {
                    layout = F9PTextView.Truncate(currentState.Text, Element.F9PFormattedString, new TextPaint(control.Paint), currentState.AvailWidth, currentState.AvailHeight, Element.AutoFit, Element.LineBreakMode, ref lines, ref text);
                    P42.Utils.Debug.Message(Element, "Truncate");
                }
            }
            lines = lines > 0 ? System.Math.Min(lines, layout.LineCount) : layout.LineCount;
            for (int i = 0; i < lines; i++)
            {
                tmpHt = layout.GetLineBottom(i);
                var width = layout.GetLineWidth(i);
                P42.Utils.Debug.Message(Element, "Line[" + i + "] w:[" + width + "] h:[" + layout.GetLineBottom(i) + "]");
                //System.Diagnostics.Debug.WriteLine("\t\tright=["+right+"]");
                if (width > tmpWd)
                    tmpWd = (int)System.Math.Ceiling(width);
            }
            P42.Utils.Debug.Message(Element, "tmpWd=[" + tmpWd + "] layout.Width=[" + layout.Width + "]");
            if (currentState.AutoFit == AutoFit.None && currentState.Lines > 0)
                control.SetMaxLines(currentState.Lines);

            //System.Diagnostics.Debug.WriteLine("\tLabelRenderer.GetDesiredSize\ttmp.size=[" + tmpWd + ", " + tmpHt + "]");
            if (Element.IsDynamicallySized && currentState.Lines > 0 && currentState.AutoFit == AutoFit.Lines)
            {
                fontMetrics = control.Paint.GetFontMetrics();
                fontLineHeight = fontMetrics.Descent - fontMetrics.Ascent;
                fontLeading = System.Math.Abs(fontMetrics.Bottom - fontMetrics.Descent);
                tmpHt = (int)(fontLineHeight * currentState.Lines + fontLeading * (currentState.Lines - 1));
                P42.Utils.Debug.Message(Element, "DynamicallySized: tmpHt=[" + tmpHt + "]");
            }

            control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();

            if (Element.Text != null)
                control.Text = text.ToString();
            else
                control.TextFormatted = text;

            var result = new SizeRequest(new Xamarin.Forms.Size(System.Math.Ceiling((double)tmpWd), System.Math.Ceiling((double)tmpHt)), new Xamarin.Forms.Size(10, System.Math.Ceiling((double)tmpHt)));
            if (P42.Utils.Debug.ConditionFunc?.Invoke(Element) ?? false)
            {
                //control.SetWidth((int)_lastSizeRequest.Value.Request.Width);
                //control.SetHeight((int)_lastSizeRequest.Value.Request.Height);

                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize \t[" + elementText + "] ");
                System.Diagnostics.Debug.WriteLine(GetType() + ".GetDesiredSize(" + (currentState.AvailWidth > int.MaxValue / 3 ? "infinity" : currentState.AvailWidth.ToString()) + "," + (currentState.AvailHeight > int.MaxValue / 3 ? "infinity" : currentState.AvailHeight.ToString()) + ") exit (" + result + ")");
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

            if (Element.LineBreakMode == LineBreakMode.NoWrap)
                control.SetSingleLine(true);

            P42.Utils.Debug.Message(Element, "EXIT _lastSizeRequest=[" + result + "]");
            return result;
        }




        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var view = new F9PTextView(Context);
                _labelTextColorDefault = view.TextColors;
                SetNativeControl(view);
            }

            if (e.OldElement != null)
            {
                e.OldElement.RendererIndexAtPoint -= IndexAtPoint;
                e.OldElement.RendererSizeForWidthAndFontSize -= LabelXamarinSize;
                e.OldElement.Draw -= DrawLabel;
            }

            if (e.NewElement != null)
            {
                e.NewElement.RendererIndexAtPoint += IndexAtPoint;
                e.NewElement.RendererSizeForWidthAndFontSize += LabelXamarinSize;
                e.NewElement.Draw += DrawLabel;
                _currentDrawState = new TextControlState
                {
                    Lines = Element.Lines,
                    AutoFit = Element.AutoFit,
                    LineBreakMode = Element.LineBreakMode,
                    SyncFontSize = (float)Element.SynchronizedFontSize,
                };
                _lastDesiredSizeState = null;

                if (Control != null)
                {
                    if (Looper.MyLooper() == Looper.MainLooper)
                    {
                        Control.SetTextColor(_labelTextColorDefault);
                        Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
                        UpdateText();
                        UpdateColor();
                        UpdateFont();
                    }
                    else
                    {
                        var activity = Settings.Context as Android.App.Activity;
                        activity.RunOnUiThread(() =>
                        {
                            Control.SetTextColor(_labelTextColorDefault);
                            Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
                            UpdateText();
                            UpdateColor();
                            UpdateFont();
                        });
                    }
                }
                //Control.SkipNextInvalidate();
            }
        }



        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
                if (Looper.MyLooper() == Looper.MainLooper)
                    Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
                else
                    ((Android.App.Activity)Settings.Context).RunOnUiThread(() =>
                {
                    Control.Gravity = Element.HorizontalTextAlignment.ToHorizontalGravityFlags() | Element.VerticalTextAlignment.ToVerticalGravityFlags();
                });
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
                UpdateColor();
            else if (e.PropertyName == Label.FontSizeProperty.PropertyName)
                UpdateFontSize();
            else if (e.PropertyName == Forms9Patch.Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize();
            else if (e.PropertyName == Label.FontProperty.PropertyName || e.PropertyName == Label.FontFamilyProperty.PropertyName || e.PropertyName == Label.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
                UpdateLineBreakMode();
            else if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.HtmlTextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Label.AutoFitProperty.PropertyName)
                UpdateFit();
            else if (e.PropertyName == Label.LinesProperty.PropertyName)
                UpdateLines();
            else if (e.PropertyName == Label.MinFontSizeProperty.PropertyName)
                UpdateMinFontSize();
            else if (e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                /*
				//TODO: EVALUATE THE NECESSITY AND EFFICACY OF THIS BLOCK
				if (Element.Width > -1 && Element.Height > -1 && Element.IsVisible)
					if (Element.Width != _lastControlState.AvailWidth || Element.Height != _lastControlState.AvailHeight)
					{
						_lastSizeRequest = null;
						LayoutForSize((int)(Element.Width * Forms9Patch.Display.Scale), (int)(Element.Height * Forms9Patch.Display.Scale));
					}
				*/
                Layout();
            }
            else if (e.PropertyName == Label.SynchronizedFontSizeProperty.PropertyName)
            {
                //if (_currentControlState.TextSize > 0)
                //{
                //    var syncFontSize = ((ILabel)Element).SynchronizedFontSize;
                //    if (syncFontSize >= 0 && _currentControlState.TextSize != syncFontSize)
                //    {
                //        _currentControlState.TextSize = -200;
                _currentDrawState.SyncFontSize = (float)Element.SynchronizedFontSize;
                Layout();
                //    }
                //}
            }
        }

        void Layout()
        {
            P42.Utils.Debug.Message(Element, "ENTER");
            if (Element.IsVisible)
            {
                if (Element.Width > -1 && Element.Height > -1 && (_lastDesiredSizeState == null || System.Math.Abs(Element.Width - _lastDesiredSizeState.AvailWidth) > 1 || System.Math.Abs(Element.Height - _lastDesiredSizeState.AvailHeight) > 1))
                    //LayoutForSize((int)(Element.Width * Forms9Patch.Display.Scale), (int)(Element.Height * Forms9Patch.Display.Scale));
                    //LayoutForSize(Element.Width, Element.Height);
                    DrawLabel(Element.Width, Element.Height);
                else
                    RequestLayout();
            }
            P42.Utils.Debug.Message(Element, "EXIT");
        }

        void UpdateLineBreakMode()
        {
            _currentDrawState.LineBreakMode = Element.LineBreakMode;
            UpdateText();
            Layout();
        }

        void UpdateFontSize()
        {
            _currentDrawState.TextSize = (float)Element.FontSize;
            Layout();
        }

        void UpdateMinFontSize()
        {
            _currentDrawState.TextSize = BoundTextSize(Element.FontSize);
            Layout();
        }

        void UpdateFit()
        {
            _currentDrawState.AutoFit = Element.AutoFit;
            Layout();
        }

        void UpdateLines()
        {
            _currentDrawState.Lines = Element.Lines;
            Layout();
        }

        //void UpdateColor([System.Runtime.CompilerServices.CallerMemberName] string callerName = null, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        void UpdateColor()
        {
            if (_currentDrawState.TextColor == Element.TextColor)
                return;
            _currentDrawState.TextColor = Element.TextColor;
            if (Looper.MyLooper() == Looper.MainLooper)
            {
                if (_currentDrawState.TextColor == Xamarin.Forms.Color.Default)
                    Control.SetTextColor(_labelTextColorDefault);
                else
                    Control.SetTextColor(_currentDrawState.TextColor.ToAndroid());
            }
            else
                ((Android.App.Activity)Settings.Context).RunOnUiThread(() =>
                {
                    if (_currentDrawState.TextColor == Xamarin.Forms.Color.Default)
                        Control.SetTextColor(_labelTextColorDefault);
                    else
                        Control.SetTextColor(_currentDrawState.TextColor.ToAndroid());
                });
        }

        void UpdateFont()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _currentDrawState.Typeface = FontManagment.TypefaceForFontFamily(Element.FontFamily) ?? Element.Font.ToTypeface();
#pragma warning restore CS0618 // Type or member is obsolete
            if (_currentDrawState.Typeface == Control.Typeface)
                return;
            //Android.App.LocalActivityManager.CurrentActivity.RunOnUiThread(()=>
            if (Looper.MyLooper() == Looper.MainLooper)
                Control.Typeface = _currentDrawState.Typeface;
            else
                ((Android.App.Activity)Settings.Context).RunOnUiThread(() =>
                {
                    Control.Typeface = _currentDrawState.Typeface;
                });
            Layout();
        }

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
                    minFontSize = 4;
                return minFontSize;
            }
        }

        #endregion

        void UpdateText()
        {

            if (Element.F9PFormattedString != null)
                _currentDrawState.TextFormatted = Element.F9PFormattedString.ToSpannableString(noBreakSpace: Element.LineBreakMode == LineBreakMode.CharacterWrap);
            else
            {
                var text = Element.Text;
                if (Element.LineBreakMode == LineBreakMode.CharacterWrap)
                    text = Element.Text.Replace(' ', '\u00A0');
                _currentDrawState.Text = text;
            }

            UpdateColor();
            Layout();
        }

        int IndexAtPoint(Xamarin.Forms.Point p)
        {
            return Control.IndexForPoint(p.ToNativePoint());
        }
    }
}