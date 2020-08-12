// /*******************************************************************
//  *
//  * AutofitTextView.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using Android.Content;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Android.Text;
using Xamarin.Forms;
using System;
using Android.Runtime;
using Android.Graphics;
using Android.Views;

namespace Forms9Patch.Droid
{
    public class F9PTextView : TextView, Forms9Patch.IText
    {
        internal delegate bool BoolDelegate();



        internal static float DefaultTextSize { get; private set; }
        internal static int DefaultTextColorArgbInt { get; private set; }
        internal static Android.Graphics.Color DefaultTextColor { get; private set; }
        internal static float DefaultLineSpacingExtra { get; private set; }
        internal static float DefaultLineSpacingMultiplier { get; private set; }

        static F9PTextView()
        {
            using (var architypeTextView = new TextView(Forms9Patch.Droid.Settings.Context))
            {
                var systemFontSize = architypeTextView.TextSize;
                DefaultTextSize = systemFontSize / Settings.Context.Resources.DisplayMetrics.Density;
                DefaultTextColorArgbInt = architypeTextView.CurrentTextColor;
                DefaultTextColor = new Android.Graphics.Color(DefaultTextColorArgbInt);
                DefaultLineSpacingExtra = architypeTextView.LineSpacingExtra;
                DefaultLineSpacingMultiplier = architypeTextView.LineSpacingMultiplier;
            }
        }

        #region Fields
        static int _instances;
        internal int _instanceId;
        #endregion

        #region Construction / Disposal
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public F9PTextView(Context context) : base(context)
        //=> Init(context, null, 0);
        { _instanceId = _instances++; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        public F9PTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        //=> Init(context, attrs, 0);
        { _instanceId = _instances++; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        /// <param name="defStyle">Def style.</param>
        public F9PTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        //=> Init(context, attrs, defStyle);
        { _instanceId = _instances++; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="javaReference">Java reference.</param>
        /// <param name="transfer">Transfer.</param>
        public F9PTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { _instanceId = _instances++; }



        bool _disposePending;
        private void OnInNativeLayoutComplete(object sender, EventArgs e)
        {
            if (_disposePending)
                Dispose();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (InNativeLayout)
            {
                _disposePending = true;
                return;
            }
            if (!_disposed && disposing)
            {
                _disposed = true;
                //InNativeLayoutComplete -= OnInNativeLayoutComplete;
            }
            base.Dispose(disposing);
        }

        #endregion


        #region Properties
        internal new float TextSize
        {
            get => base.TextSize / Settings.Context.Resources.DisplayMetrics.Density;
            set
            {
                if (System.Math.Abs(TextSize - value) < 0.001)
                    return;
                if (value < 0)
                {
                    if (System.Math.Abs(TextSize - DefaultTextSize) < 0.001)
                        return;
                    base.TextSize = DefaultTextSize;
                }
                else
                    base.TextSize = value;
            }
        }

        #endregion


        #region update
        public void UpdateFrom(F9PTextView control)
        {
            if (control != null && control != this)
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr1)
                {
                    LayoutDirection = control.LayoutDirection;
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                    {
                        FontFeatureSettings = control.FontFeatureSettings;
                        LetterSpacing = control.LetterSpacing;
                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.P)
                        {
                            FallbackLineSpacing = control.FallbackLineSpacing;
                            LineHeight = control.LineHeight;
                        }
                    }
                }
                LayoutParameters = control.LayoutParameters;
                PaintFlags = control.PaintFlags;


            }
        }
        #endregion

        #region Touch to Index
        internal int IndexForPoint(Android.Graphics.Point p)
        {
            if (_disposed)
                return -1;
            var line = Layout.GetLineForVertical(p.Y);
            var offset = Layout.GetOffsetForHorizontal(line, p.X);
            return offset;
        }
        #endregion


        #region Skip First Invalidation

        public bool IsNativeDrawEnabled { get; set; } = true;


        public override void RequestLayout()
        {
            if (IsNativeDrawEnabled)
                base.RequestLayout();
        }

        //bool _skip;
        bool invalidating = false;
        bool pendingInvalidate = false;
        public override void Invalidate()
        {
            if (IsNativeDrawEnabled)
            {
                /*
                if (!_skip)
                    base.Invalidate();
                _skip = false;
                */
                if (invalidating)
                {
                    pendingInvalidate = true;
                    System.Diagnostics.Debug.WriteLine("pending");
                    return;
                }
                invalidating = true;
                //Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                //{
                    base.Invalidate();
                    invalidating = false;
                    if (pendingInvalidate)
                    {
                        pendingInvalidate = false;
                        Invalidate();
                    }
                //});

            }
        }

        public void SkipNextInvalidate()
        { }
        //=> _skip = true;

        #endregion


        // I don't know why, but this #region seems to help mitigate a "using JNI after critical get in call to DeleteGlobalRef"
        // crash in ConnectionCalc results, when scrolling up/down multiple times
        //
        // don't remove without extensive testing
        #region Android Layout
        event EventHandler InNativeLayoutComplete;
        int _inNativeLayout;
        internal bool InNativeLayout
        {
            get => _inNativeLayout > 0;
            set
            {
                if (value)
                    _inNativeLayout++;
                else
                    _inNativeLayout--;
                if (_inNativeLayout == 0)
                    InNativeLayoutComplete?.Invoke(null, null);
            }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (_disposed)
                return;

            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            if (MeasureSpec.GetMode(widthMeasureSpec) == Android.Views.MeasureSpecMode.Unspecified)
                width = int.MaxValue / 2;
            if (width <= 0)
                width = 10;
            var height = MeasureSpec.GetSize(heightMeasureSpec);
            var heightMode = MeasureSpec.GetMode(widthMeasureSpec);
            if (MeasureSpec.GetMode(heightMeasureSpec) == Android.Views.MeasureSpecMode.Unspecified)
                height = int.MaxValue / 2;
            if (height <= 0)
                height = 10;

            widthMeasureSpec = MeasureSpec.MakeMeasureSpec(width, widthMode);
            heightMeasureSpec = MeasureSpec.MakeMeasureSpec(height, heightMode);

            InNativeLayout = true;
            try
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
            catch (Java.Lang.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName(), " exception: " + e);
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName(), " exception: " + e);
            }
            InNativeLayout = false;
        }
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (_disposed)
                return;
            InNativeLayout = true;
            base.OnLayout(changed, left, top, right, bottom);
            InNativeLayout = false;
        }
        #endregion


        #region  get better crash diagnostics!
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        => base.OnSizeChanged(w, h, oldw, oldh);

        public override void OnDrawForeground(Canvas canvas)
            => base.OnDrawForeground(canvas);

        protected override void OnDraw(Canvas canvas)
            => base.OnDraw(canvas);

        public override bool OnPreDraw()
            => base.OnPreDraw();
        #endregion

    }
}

