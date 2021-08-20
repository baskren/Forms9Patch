using Android.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Runtime;

[assembly: ExportRenderer(typeof(Forms9Patch.EnhancedListView), typeof(Forms9Patch.Droid.EnhancedListViewRenderer))]
namespace Forms9Patch.Droid
{
    public class EnhancedListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer, IScrollView
    {
        #region Fields
        ScrollListener _scrollListener;
        ScrollListener ScrollListener
        {
            get
            {
                _scrollListener = _scrollListener ?? new ScrollListener();
                return _scrollListener;
            }
        }
        #endregion

        public EnhancedListViewRenderer(Android.Content.Context context) : base(context) { }

        public EnhancedListViewRenderer(Android.Content.Context context, object obj) : base(context) { }

#pragma warning disable CS0618 // Type or member is obsolete
        public EnhancedListViewRenderer(System.IntPtr intPtr, Android.Runtime.JniHandleOwnership owner) { }
#pragma warning restore CS0618 // Type or member is obsolete


        #region ElementChanged
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is EnhancedListView oldElement)
            {
                oldElement.PropertyChanging -= OnElementPropertyChanging;
                oldElement.Renderer = null;
                ScrollListener.Element = null;
            }
            if (e.NewElement is EnhancedListView newElement)
            {
                newElement.PropertyChanging += OnElementPropertyChanging;
                newElement.Renderer = this as IScrollView;

                ScrollListener.IsBuildingLayOut = true;
                ScrollListener.Element = newElement;
                Control.SetOnScrollListener(ScrollListener);
                ScrollListener.IsBuildingLayOut = false;

                if (newElement.FooterElement is VisualElement footer)
                    footer.PropertyChanged += Footer_PropertyChanged;

            }
            Control.Divider = null;
            Control.DividerHeight = 0;
            /* none of the following seem to do anything!
			Control.SetSelector(Android.Resource.Color.Transparent);
			Control.ChoiceMode = ChoiceMode.MultipleModal;
			Control.SetMultiChoiceModeListener(new ChoiceListener());
			*/
            //Control.Enabled = false;  // disables selection AND scrolling!
            //Control.Clickable = false;  // this will disable Click (tap) gesture
        }
        #endregion


        #region Element Property Change Responders
        void OnElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if ((e.PropertyName == Xamarin.Forms.ListView.FooterProperty.PropertyName || e.PropertyName == Xamarin.Forms.ListView.FooterTemplateProperty.PropertyName) && Element?.FooterElement is VisualElement footer)
                footer.PropertyChanged -= Footer_PropertyChanged;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if ((e.PropertyName == Xamarin.Forms.ListView.FooterProperty.PropertyName || e.PropertyName == Xamarin.Forms.ListView.FooterTemplateProperty.PropertyName) && Element?.FooterElement is VisualElement footer)
                footer.PropertyChanged += Footer_PropertyChanged;
        }

        void Footer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.HeightRequestProperty.PropertyName && Element?.FooterElement is VisualElement footer)
            {
                footer.InvalidateMeasureNonVirtual(Xamarin.Forms.Internals.InvalidationTrigger.MeasureChanged);
                var sizeRequest = footer.Measure(Element.Width, double.PositiveInfinity);
                if (footer.Bounds.Size != sizeRequest.Request && Platform.GetRenderer(footer) is IVisualElementRenderer footerRenderer)
                {
                    footer.Layout(new Rectangle(0, 0, sizeRequest.Request.Width, sizeRequest.Request.Height));
                    footerRenderer.UpdateLayout();
                }
            }
        }

        #endregion

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            ScrollListener.IsBuildingLayOut = true;
            base.OnLayout(changed, l, t, r, b);
            ScrollListener.IsBuildingLayOut = false;
        }

        #region Scrolling
        public bool ScrollBy(double delta, bool animated)
        {
            if (animated)
            {
                Control.SmoothScrollByOffset((int)delta);
                return true;
            }
            int index = Control.FirstVisiblePosition;
            var cellView = Control.GetChildAt(index);
            var offset = (cellView == null) ? 0 : (cellView.Top - Control.ListPaddingTop);
            offset += (int)(delta * Settings.Context.Resources.DisplayMetrics.Density);
            Control.SetSelectionFromTop(index, -offset);
            return true;
        }

        DateTime _lastScrollToDateTime = DateTime.MinValue.AddYears(1);
        int _lastScrollToOffset = 0;
        public bool ScrollTo(double offset, bool animated)
        {
            if (Element is EnhancedListView listView && listView.ItemsSource is GroupWrapper)
            {
                // Animation not supported on Android because SmoothScrollToTop fires a bunch of ScrollListener.OnScrolledStateChanged(ScrolledState.Idle) calls during animation.
                // There appears to be no way of knowing when the animation is on going without overriding the native Android.ListView's LinearLayoutManager.  Too tall of an order.

                _lastScrollToOffset = (int)Math.Round(-offset * Forms9Patch.Display.Scale);
                Control.SetSelectionFromTop(0, _lastScrollToOffset - 1);
                _lastScrollToDateTime = DateTime.Now;
                CatchMissingScroll();
                return true;
            }
            return false;
        }

        void CatchMissingScroll()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {

                if (DateTime.Now - _lastScrollToDateTime <= TimeSpan.FromSeconds(0.5))
                    return true;
                Control.SetSelectionFromTop(0, _lastScrollToOffset);
                return false;
            });
        }

        public double ScrollOffset
        {
            get
            {
                if (Element is EnhancedListView listView && listView.ItemsSource is GroupWrapper groupWrapper)
                {
                    var cellView = Control.GetChildAt(0);
                    if (cellView == null)
                        return -1;

                    int index = Control.FirstVisiblePosition;
                    if (index == 0)
                        return -cellView.Top / Forms9Patch.Display.Scale;

                    if (Control.HeaderViewsCount > 0)
                    {
                        index--;
                        if (Control.HeaderViewsCount > 1)
                            throw new NotSupportedException("EnhancedListViewRenderer only supports 0 or 1 ListView.Header");
                    }

                    var deepDataSource = groupWrapper.TwoDeepDataSetForFlatIndex(index);
                    return HeaderHeight + deepDataSource.Offset - cellView.Top / Forms9Patch.Display.Scale;
                }
                return 0;
            }
        }

        public bool IsScrollEnabled { get; set; } = true;

        /*
        public override bool OnInterceptTouchEvent(MotionEvent e) 
        {
            if (e.Action == MotionEventActions.Move && !IsScrollEnabled)
                return true;
            return base.OnInterceptTouchEvent(e);
        }
        */


        float downY = 0;
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
                downY = ev.GetY();

            //if (IsScrollEnabled || ev.Action != MotionEventActions.Move)
            //    return base.OnInterceptTouchEvent(ev);
            //return true;

            if (!IsScrollEnabled)
                ev.SetLocation(ev.GetX(), downY);
            return base.DispatchTouchEvent(ev);
        }

        #endregion

        #region HeaderHeight
        double NativeHeaderHeight()
        {
            if (Element?.HeaderElement is VisualElement headerElement)
            {
                if (Platform.GetRenderer(headerElement) is IVisualElementRenderer headerRenderer)
                    return headerRenderer.View.Height;
                throw new Exception("Element.HeaderElement should have a renderer");
            }
            return 0;
        }

        public double HeaderHeight => NativeHeaderHeight() / Forms9Patch.Display.Scale;
        #endregion

        #region FooterHeight
        double NativeFooterHeight()
        {
            if (Element?.FooterElement is VisualElement footerElement)
            {
                if (Platform.GetRenderer(footerElement) is IVisualElementRenderer footerRenderer)
                    return footerRenderer.View.Height;
                throw new Exception("Element.HeaderElement should have a renderer");
            }
            return 0;
        }

        double FooterHeight() => NativeFooterHeight() / Forms9Patch.Display.Scale;
        #endregion
    }

    #region ScrollListener 
    class ScrollListener : Java.Lang.Object, Android.Widget.ListView.IOnScrollListener
    {
        public Forms9Patch.EnhancedListView Element;
        public bool IsBuildingLayOut;

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (!IsBuildingLayOut)
                Element?.OnScrolling(this, EventArgs.Empty);
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            if (scrollState == ScrollState.Idle)
                Element?.OnScrolled(this, EventArgs.Empty);
        }
    }
    #endregion
}
