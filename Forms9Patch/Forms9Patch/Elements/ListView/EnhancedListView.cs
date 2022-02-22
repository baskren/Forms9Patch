using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FormsGestures;
using P42.NumericalMethods;
using System.Linq;

namespace Forms9Patch
{
    /// <summary>
    /// Enable EmbeddedResource fonts to be used with Xamarin elements
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    internal class EnhancedListView : Xamarin.Forms.ListView, IScrollView, IDisposable
    {
        #region Properties


        #region IsScrollEnabled property
        /// <summary>
        /// Gets/Sets the IsScrollEnabled property
        /// </summary>
        public bool IsScrollEnabled
        {
            get
            {
                if (Renderer != null)
                    return Renderer.IsScrollEnabled;
                return true;
            }
            set
            {
                if (Renderer != null)
                    Renderer.IsScrollEnabled = value;
            }
        }
        #endregion IsScrollEnabled property



        #region ScrollOffset property
        /// <summary>
        /// Gets/Sets the ScrollOffset property
        /// </summary>
        public double ScrollOffset
        {
            get
            {
                if (Renderer != null)
                    return Renderer.ScrollOffset;
                return -1;
            }
        }

        bool _isScrolling;
        DateTime _lastScrolling = DateTime.MinValue.AddYears(1);
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
        public new event EventHandler Scrolled;
        #endregion


        #region Constructors / Disposer
        /// <summary>
        /// Constructor for Forms9Patch.EnhancedListView
        /// </summary>
        public EnhancedListView()
        {
        }

        /// <summary>
        /// Constructor for Forms9Patch.EnhancedListView
        /// </summary>
        /// <param name="cachingStrategy"></param>
        public EnhancedListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposed = true;

                    Scrolling = null;
                    //Scrolled = null;

                    var items = TemplatedItems.ToArray();

                    foreach (var item in items)
                        if (item is IDisposable disposable)
                        {
                            item.BindingContext = null;
                            disposable.Dispose();
                        }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion


        #region  Platform method delegates

        internal IScrollView Renderer;

        #endregion


        #region Methods

        public bool ScrollBy(double delta, bool animated = true)
        {
            if (Renderer != null)
                Renderer.ScrollBy(delta, animated);
            return false;
        }

        public bool ScrollTo(double offset, bool animated = true) => Renderer != null && Renderer.ScrollTo(offset, animated);


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
                if (Renderer != null)
                    return Renderer.HeaderHeight;
                return 0;
            }
        }

        #endregion


        #region Event Handles
        internal void OnScrolling(object sender, EventArgs args)
        {
            //Listener.CancelActiveGestures();   // this breaks UWP listview scrolling!!!
            //System.Diagnostics.Debug.WriteLine("scrolling");
            IsScrolling = true;
            Scrolling?.Invoke(this, args);
            //System.Diagnostics.Debug.WriteLine("EnhancedListView.OnScrolling: offset=[" + ScrollOffset + "]");
        }
        
        internal void OnScrolled(object sender, EventArgs args)
        {
            //Listener.CancelActiveGestures();  // this breaks UWP listview scrolling!!!
            //System.Diagnostics.Debug.WriteLine("!!! STOP !!!");
            IsScrolling = false;
            Scrolled?.Invoke(this, args);
            //System.Diagnostics.Debug.WriteLine("EnhancedListView.OnScrolled: offset=[" + ScrollOffset + "]");
        }
        


        #endregion
    }


}
