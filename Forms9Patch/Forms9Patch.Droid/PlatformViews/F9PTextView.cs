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

        static bool _defaultTextSizeInitialized;
        static float _defaultTextSize;
        internal static float DefaultTextSize
        {
            get
            {
                if (!_defaultTextSizeInitialized)
                {
                    var systemFontSize = (new TextView(Forms9Patch.Droid.Settings.Context)).TextSize;
                    _defaultTextSize = systemFontSize / Settings.Context.Resources.DisplayMetrics.Density;
                    _defaultTextSizeInitialized = true;
                }
                return _defaultTextSize;
            }
        }

        #region Construction / Disposal
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public F9PTextView(Context context) : base(context)
        //=> Init(context, null, 0);
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        public F9PTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        //=> Init(context, attrs, 0);
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        /// <param name="defStyle">Def style.</param>
        public F9PTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        //=> Init(context, attrs, defStyle);
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="javaReference">Java reference.</param>
        /// <param name="transfer">Transfer.</param>
        public F9PTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.Droid.F9PTextView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        /// <param name="defStyleAttr">Def style attr.</param>
        /// <param name="defStyleRes">Def style res.</param>
        public F9PTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
            => Init(context, attrs, 0);
            */



            /*
        void Init(Context context, IAttributeSet attrs, int defStyle)
        {
            if (DefaultTextSize <= 0)
            {
                var systemFontSize = (new TextView(context)).TextSize;
                DefaultTextSize = systemFontSize / Settings.Context.Resources.DisplayMetrics.Density;
            }
            //InNativeLayoutComplete += OnInNativeLayoutComplete;
        }
        */

        
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
                if (System.Math.Abs(base.TextSize - value) < float.Epsilon * 5)
                    return;
                base.TextSize = value;
            }
        }
        #endregion


        #region Touch to Index
        internal int IndexForPoint(Android.Graphics.Point p)
        {
            //if (_disposed)
            //    return -1;
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

        bool _skip;
        public override void Invalidate()
        {
            if (IsNativeDrawEnabled)
            {
                if (!_skip)
                    base.Invalidate();
                _skip = false;
            }
        }

        public void SkipNextInvalidate()
        {
            _skip = true;
        }
        #endregion

        
        // I don't know why, but this #region seems to help mitigate a "using JNI after critical get in call to DeleteGlobalRef"
        // crash in ConnectionCalc results, when scrolling up/down multiple times
        //
        // don't remove without extensive testing
        #region Android Layout
        event EventHandler InNativeLayoutComplete;
        int _inNativeLayout;
        bool InNativeLayout
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
            /*
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            if (MeasureSpec.GetMode(widthMeasureSpec) == Android.Views.MeasureSpecMode.Unspecified)
                width = int.MaxValue / 2;

            if (width <= 0)
                return;
            var height = MeasureSpec.GetSize(heightMeasureSpec);
            if (MeasureSpec.GetMode(heightMeasureSpec) == Android.Views.MeasureSpecMode.Unspecified)
                height = int.MaxValue / 2;
            if (height <= 0)
                return;
                */

            InNativeLayout = true;
            try
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
            catch (Java.Lang.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName()," exception: "+e);
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
        
    }
}

