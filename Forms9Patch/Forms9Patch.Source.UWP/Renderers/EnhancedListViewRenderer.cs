using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Forms9Patch;
using Windows.UI.Xaml.Controls;

[assembly: Xamarin.Forms.Platform.UWP.ExportRenderer(typeof(Forms9Patch.EnhancedListView), typeof(Forms9Patch.UWP.EnhancedListViewRenderer))]
namespace Forms9Patch.UWP
{
    class EnhancedListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {
        /*
        static Windows.UI.Xaml.ResourceDictionary _enhancedListViewResources;
        Windows.UI.Xaml.ResourceDictionary EnhancedListViewResources
        {
            get
            {
                if (_enhancedListViewResources != null)
                    return _enhancedListViewResources;
                var xaml = P42.Utils.ApplicationStorageExtensions.EmbeddedStoredText("Forms9Patch.UWP.EnhancedListView.Resources.xaml", GetType().GetTypeInfo().Assembly);
                _enhancedListViewResources = (Windows.UI.Xaml.ResourceDictionary)Windows.UI.Xaml.Markup.XamlReader.Load(xaml);
                return _enhancedListViewResources;
            }
        }
        */
        

        #region Fields
        Windows.UI.Xaml.Controls.ScrollViewer _scrollViewer = null;
        #endregion


        #region Element change setup / teardown
        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(Xamarin.Forms.Platform.UWP.ElementChangedEventArgs<Xamarin.Forms.ListView> e)
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

        class GroupStyleSelector : Windows.UI.Xaml.Controls.GroupStyleSelector
        {

            public Windows.UI.Xaml.Controls.GroupStyle GroupStyle { get; private set; }

            public GroupStyleSelector(Windows.UI.Xaml.Controls.GroupStyle style)
            {
                GroupStyle = style;
            }

            protected override GroupStyle SelectGroupStyleCore(object group, uint level) => GroupStyle;
        }

        void SetCellStyle()
        {
            var listView = Control as Windows.UI.Xaml.Controls.ListView;
            if (listView == null)
                return;

            listView.ShowsScrollingPlaceholders = true;

            var itemStyle = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["Forms9PatchListViewItem"];
            //var itemStyle = (Windows.UI.Xaml.Style)EnhancedListViewResources["Forms9PatchListViewItem"];
            listView.ItemContainerStyle = itemStyle;

            var forms9PatchListViewGroupStyle = (Windows.UI.Xaml.Controls.GroupStyle)Windows.UI.Xaml.Application.Current.Resources["Forms9PatchListViewGroupStyle"];
            //var forms9PatchListViewGroupStyle = (Windows.UI.Xaml.Controls.GroupStyle)EnhancedListViewResources["Forms9PatchListViewGroupStyle"];
            listView.GroupStyleSelector = new GroupStyleSelector(forms9PatchListViewGroupStyle);

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
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
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
