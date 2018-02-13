using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enable EmbeddedResource fonts to be used with Xamarin elements
    /// </summary>
    internal class EnhancedListView : Xamarin.Forms.ListView
    {
        #region Properties


        #region IsScrollEnabled property
        /// <summary>
        /// backing store for IsScrollEnabled property
        /// </summary>
        public static readonly BindableProperty IsScrollEnabledProperty = BindableProperty.Create("IsScrollEnabled", typeof(bool), typeof(EnhancedListView), default(bool));
        /// <summary>
        /// Gets/Sets the IsScrollEnabled property
        /// </summary>
        public bool IsScrollEnabled
        {
            get { return (bool)GetValue(IsScrollEnabledProperty); }
            set { SetValue(IsScrollEnabledProperty, value); }
        }
        #endregion IsScrollEnabled property



        #region ScrollOffset property
        /// <summary>
        /// Gets/Sets the ScrollOffset property
        /// </summary>
        public double ScrollOffset => RendererScrollOffset.Invoke();

        bool _isScrolling;
        DateTime _lastScrolling = DateTime.MinValue;
        public bool IsScrolling
        {
            get
            {
                if (_isScrolling && DateTime.Now - _lastScrolling < TimeSpan.FromSeconds(3))
                    _isScrolling = false;
                return _isScrolling;
            }
            set
            {
                _isScrolling = value;
                if (_scrolling)
                    _lastScrolling = DateTime.Now;
            }
        }
        #endregion ScrollOffset property


        #endregion


        #region Events
        public event EventHandler Scrolling;
        public event EventHandler Scrolled;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor for Forms9Patch.EnhancedListView
        /// </summary>
        public EnhancedListView() : base() { }

        /// <summary>
        /// Constructor for Forms9Patch.EnhancedListView
        /// </summary>
        /// <param name="cachingStrategy"></param>
        public EnhancedListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) { }
        #endregion


        #region  Platform method delegates
        internal Func<double, bool, bool> RendererScrollBy;
        internal Func<double, bool, bool> RendererScrollTo;
        internal Func<double> RendererScrollOffset;
        internal Func<double> RendererHeaderHeight;
        #endregion


        #region Methods

        public bool ScrollBy(double delta, bool animated = true)
        {
            if (RendererScrollBy != null)
                return RendererScrollBy(delta, animated);
            return false;
        }

        public bool ScrollTo(double offset, bool animated = true)
        {
            if (RendererScrollTo != null)
                return RendererScrollTo(offset, animated);
            return false;
        }

        double _scrollSpeed;
        bool _scrolling;
        internal void ScrollSpeed(double speed)
        {
            if (!_scrolling && Math.Abs(_scrollSpeed) > 0)
            {
                _scrolling = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
                {
                    _scrolling = ScrollBy(_scrollSpeed);
                    _scrolling &= Math.Abs(_scrollSpeed) > 0;
                    return _scrolling;
                });
            }
            _scrollSpeed = speed;
        }

        public double HeaderHeight
        {
            get
            {
                if (RendererHeaderHeight != null)
                    return RendererHeaderHeight.Invoke();
                return 0;
            }
        }

        #endregion


        #region Event Handles
        internal void OnScrolling(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("scrolling");
            IsScrolling = true;
            Scrolling?.Invoke(this, args);
            //System.Diagnostics.Debug.WriteLine("EnhancedListView.OnScrolling: offset=[" + ScrollOffset + "]");
        }

        internal void OnScrolled(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("!!! STOP !!!");
            IsScrolling = false;
            Scrolled?.Invoke(this, args);
            //System.Diagnostics.Debug.WriteLine("EnhancedListView.OnScrolled: offset=[" + ScrollOffset + "]");
        }
        #endregion
    }


}
