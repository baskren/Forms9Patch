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

        public bool ScrollBy(double delta, bool animated=true)
        {
            if (RendererScrollBy != null)
                return RendererScrollBy(delta, animated);
            return false;
        }

        public bool ScrollTo(double offset, bool animated=true)
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
            Scrolling?.Invoke(this, args);
        }

        internal void OnScrolled(object sender, EventArgs args)
        {
            Scrolled?.Invoke(this, args); 
        }
        #endregion
    }


}
