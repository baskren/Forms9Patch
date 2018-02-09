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
    public class EnhancedListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer
    {
        /*
        ScrollListener _scrollListener = null;
        ScrollListener ScrollListener
        {
            get
            {
                _scrollListener = _scrollListener ?? new ScrollListener();
                return _scrollListener;
            }
        }
        */

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is EnhancedListView oldElement)
            {
                oldElement.RendererScrollBy -= ScrollBy;
                oldElement.RendererScrollTo -= ScrollTo;
                oldElement.RendererScrollOffset -= ScrollOffset;
                oldElement.RendererHeaderHeight -= HeaderHeight;
                //ScrollListener.Element = null;
            }
            if (e.NewElement is EnhancedListView newElement)
            {
                newElement.RendererScrollBy += ScrollBy;
                newElement.RendererScrollTo += ScrollTo;
                newElement.RendererScrollOffset += ScrollOffset;
                newElement.RendererHeaderHeight += HeaderHeight;

                //ScrollListener.Element = newElement;
                //Control.SetOnScrollListener(ScrollListener);

                Control.ScrollStateChanged += Control_ScrollStateChanged;

            }
            Control.Divider = null;
            Control.DividerHeight = -1;
            /* none of the following seem to do anything!
			Control.SetSelector(Android.Resource.Color.Transparent);
			Control.ChoiceMode = ChoiceMode.MultipleModal;
			Control.SetMultiChoiceModeListener(new ChoiceListener());
			*/
            //Control.Enabled = false;  // disables selection AND scrolling!
            Control.Clickable = false;  // appears to do nothing?
        }

        //int offset;
        //int lastIndex = -1;
        bool ScrollBy(double delta, bool animated)
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


        bool ScrollTo(double offset, bool animated)
        {
            if (Element is EnhancedListView listView && listView.ItemsSource is GroupWrapper groupWrapper)
            {
                var deepDataSource = groupWrapper.TwoDeepDataSetForOffset(offset);
                var delta = offset - deepDataSource.Offset;
                if (animated)
                    Control.SmoothScrollToPositionFromTop(deepDataSource.FlatIndex, (int)delta);
                else
                    Control.SetSelectionFromTop(deepDataSource.FlatIndex, (int)delta);
                return true;
            }
            return false;
        }

        double ScrollOffset()
        {
            if (Element is EnhancedListView listView && listView.ItemsSource is GroupWrapper groupWrapper)
            {
                int index = Control.FirstVisiblePosition;
                var deepDataSource = groupWrapper.TwoDeepDataSetForFlatIndex(index);
                var cellView = Control.GetChildAt(index);
                if (cellView != null)
                {
                    var offset = HeaderHeight() + deepDataSource.Offset + cellView.Top - Control.ListPaddingTop;
                    return offset;
                }
            }
            return 0;
        }

        double HeaderHeight()
        {
            if (Element.HeaderElement is VisualElement headerElement)
            {
                if (Platform.GetRenderer(headerElement) is IVisualElementRenderer headerRenderer)
                    return headerRenderer.View.Height;
                throw new Exception("Element.HeaderElement should have a renderer");
            }
            return 0;
        }

        void Control_ScrollStateChanged(object sender, AbsListView.ScrollStateChangedEventArgs e)
        {
            if (e.ScrollState == ScrollState.Idle)
                ((EnhancedListView)Element).OnScrolled(this, EventArgs.Empty);
            else
                ((EnhancedListView)Element).OnScrolling(this, EventArgs.Empty);
        }
    }

    /*
    class ScrollListener : Java.Lang.Object, Android.Widget.ListView.IOnScrollListener
    {
        public Forms9Patch.ListView Element;

        public ScrollListener()
        {
        }

        //bool _scrolling;
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            //System.Diagnostics.Debug.WriteLine("SCROLLING");
            //IVisualElementRenderer renderer = Platform.GetRenderer(Element);
            //if (renderer != null)
            Element?.OnScrolling(this, EventArgs.Empty);
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            //System.Diagnostics.Debug.WriteLine("SCROLL STATE=["+scrollState+"]");
            //IVisualElementRenderer renderer = Platform.GetRenderer(Element);
            //if (renderer != null)
            {
                if (scrollState == ScrollState.Idle)
                    Element?.OnScrolled(this, EventArgs.Empty);
            }
        }
    }
    */
}
