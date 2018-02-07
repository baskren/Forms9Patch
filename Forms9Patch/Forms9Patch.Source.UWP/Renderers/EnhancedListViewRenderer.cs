using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Forms9Patch;

[assembly: ExportRenderer(typeof(Forms9Patch.EnhancedListView), typeof(Forms9Patch.UWP.EnhancedListViewRenderer))]
namespace Forms9Patch.UWP
{
    class EnhancedListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {


        #region Fields
        Windows.UI.Xaml.Controls.ScrollViewer _scrollViewer = null;
        #endregion


        #region Element change setup / teardown
        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is EnhancedListView oldElement)
            {
                oldElement.RendererScrollBy -= ScrollBy;
                oldElement.RendererScrollTo -= ScrollTo;
                oldElement.RendererScrollOffset -= ScrollOffset;
                oldElement.RendererHeaderHeight -= HeaderHeight;
                UnsetViewChangedEvent();
            }
            if (e.NewElement is EnhancedListView newElement)
            {
                newElement.RendererScrollBy += ScrollBy;
                newElement.RendererScrollTo += ScrollTo;
                newElement.RendererScrollOffset += ScrollOffset;
                newElement.RendererHeaderHeight += HeaderHeight;

                SetCellStyle();
                SetViewChangedEvent();


            }
        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        void SetCellStyle()
        {
            var listView = Control as Windows.UI.Xaml.Controls.ListView;
            if (listView == null)
                return;

            listView.ShowsScrollingPlaceholders = true;

            var style = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["Forms9PatchListViewItem"];
            listView.ItemContainerStyle = style;

        }

        bool _viewChangeEventSet;
        void UnsetViewChangedEvent()
        {
            if (!_viewChangeEventSet)
                return;

            if (_scrollViewer != null)
                _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            _viewChangeEventSet = false;
        }

        void SetViewChangedEvent()
        {
            if (!_viewChangeEventSet)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                 {
                     if (Control is Windows.UI.Xaml.Controls.ListView listView)
                     {
                         _scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
                         if (_scrollViewer != null)
                         {
                             _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                             return false;
                         }
                     }
                     return true;
                 });
                _viewChangeEventSet = true;
            }
        }
        #endregion


        #region Scrolling
        private double ScrollOffset()
        {
            if (_scrollViewer != null)
                return _scrollViewer.VerticalOffset;
            return 0;
        }


        private void ScrollViewer_ViewChanged(object sender, Windows.UI.Xaml.Controls.ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate)
                ((EnhancedListView)Element).OnScrolling(this, EventArgs.Empty);
            else
                ((EnhancedListView)Element).OnScrolled(this, EventArgs.Empty);
        }


        bool ScrollBy(double delta, bool animated)
        {
            if (_scrollViewer != null)
                return _scrollViewer.ChangeView(null, _scrollViewer.VerticalOffset + delta, null, !animated);
            return false;
        }

        bool ScrollTo(double offset, bool animated)
        {
            if (_scrollViewer != null)
                return _scrollViewer.ChangeView(null, offset, null, !animated);
            return false;
        }

        #endregion


        #region Header

        double HeaderHeight()
        {
            if (HeaderControl != null)
                return HeaderControl.RenderSize.Height;
            return 0;
        }

        Windows.UI.Xaml.Controls.ContentControl _headerControl;
        Windows.UI.Xaml.Controls.ContentControl HeaderControl
        {
            get
            {
                if (_headerControl == null)
                {
                    if (_scrollViewer == null)
                        return null;

                    var presenter = _scrollViewer.GetChild<Windows.UI.Xaml.Controls.ItemsPresenter>();
                    if (presenter == null)
                        return null;

                    _headerControl = presenter.GetChild<Windows.UI.Xaml.Controls.ContentControl>();
                }

                return _headerControl;
            }
        }
        #endregion
    }
}
