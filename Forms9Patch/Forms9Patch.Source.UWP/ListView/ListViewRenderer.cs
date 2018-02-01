using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Forms9Patch;

[assembly: ExportRenderer(typeof(Forms9Patch.ListView), typeof(Forms9Patch.UWP.ListViewRenderer))]
namespace Forms9Patch.UWP
{
    class ListViewRenderer : Xamarin.Forms.Platform.UWP.ListViewRenderer
    {
        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is ListView oldElement)
            {
                oldElement.RendererFindItemDataUnderRectangle -= FindItemDataUnderRectangle;
                oldElement.RendererScrollBy -= ScrollBy;
                oldElement.RendererScrollToPos -= ScrollToItem;
            }
            if (e.NewElement is ListView newElement)
            {
                newElement.RendererFindItemDataUnderRectangle += FindItemDataUnderRectangle;
                newElement.RendererScrollBy += ScrollBy;
                newElement.RendererScrollToPos += ScrollToItem;
                /*
                if (newElement.IsScrollListening)
                    Control.Delegate = new ScrollDelegate(newElement);  // why does this cause headers to not appear?
                                                                        //Control.Delegate = null;
                 */
                SetCellStyle();
                
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
            /*
            if (e.PropertyName == ListView.ScrollEnabledProperty.PropertyName)
            {
                Control.ScrollEnabled = (bool)Element.GetValue(ListView.ScrollEnabledProperty);
            }
            else if (e.PropertyName == ListView.IsScrollListeningProperty.PropertyName && !Element.IsGroupingEnabled && Control != null)
                Control.Delegate = new ScrollDelegate(Element as Forms9Patch.ListView);  // why does this cause headers to not appear?
                */
        }

        void SetCellStyle()
        {
            var listView = Control as Windows.UI.Xaml.Controls.ListView;
            if (listView == null)
                return;

            listView.ShowsScrollingPlaceholders = true;
            

            /* */

            var style = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["Forms9PatchListViewItem"];
            listView.ItemContainerStyle = style;

            /*
            var scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
            if (scrollViewer != null)
            {
                scrollViewer.VerticalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Enabled;
                scrollViewer.VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            }
            */
        }

        bool _viewChangeEventSet;
        void SetViewChangedEvent()
        {
            if (_viewChangeEventSet)
                return;
            var listView = Control as Windows.UI.Xaml.Controls.ListView;
            if (listView == null)
                return;

            var scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
            if (scrollViewer == null)
                return;


            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            _viewChangeEventSet = true;
        }

        void UnsetViewChangedEvent()
        {
            if (_viewChangeEventSet)
            {
                var listView = Control as Windows.UI.Xaml.Controls.ListView;
                if (listView != null)
                {
                    var scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
                    if (scrollViewer != null)
                        scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                }
                _viewChangeEventSet = false;
            }
        }

        private void ScrollViewer_ViewChanged(object sender, Windows.UI.Xaml.Controls.ScrollViewerViewChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CHANGED");
        }

        /*
        bool Extended(double delta)
        {
            return MinScroll(delta) || MaxScroll(delta);
        }

        bool MinScroll(double delta)
        {
            return (delta < 0 && Control.ContentOffset.Y + delta <= 0);
        }

        bool MaxScroll(double delta)
        {
            return (delta > 0 && Control.ContentOffset.Y + delta + Control.Bounds.Height > Control.ContentSize.Height);
        }
        */

        bool ScrollBy(double delta)
        {
            if (Control is Windows.UI.Xaml.Controls.ListView listView)
            {
                var scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
                if (scrollViewer != null)
                    return scrollViewer.ChangeView(null, delta, null);
            }
            return false;
        }

        /*
        internal DragEventArgs ItemAtPoint(Point p)
        {
            var visibleIndexPath = Control.IndexPathsForVisibleRows;
            var offset = Control.ContentOffset.ToPoint();
            foreach (NSIndexPath indexPath in visibleIndexPath)
            {
                var cellFrame = Control.RectForRowAtIndexPath(indexPath).ToRectangle();
                var cellViewFrame = new Rectangle(cellFrame.Location - (Size)offset, cellFrame.Size);
                //System.Diagnostics.Debug.WriteLine ("ip:" + indexPath + " offset:"+offset  + " cellFrame:" + cellFrame + " viewFrame:"+viewFrame);
                //UITableViewCell cell;
                if (cellViewFrame.Contains(p))
                    return ItemAt(indexPath);
            }
            return null;
        }
        */

        DragEventArgs FindItemDataUnderRectangle(Rectangle rect)
        {
            /*
            // return the best candidate to be replaced item (that is being hovered over by the frame represented by rect)
            double left = Math.Max(rect.Left, Control.Frame.Left);
            double top = Math.Max(rect.Top, Control.Frame.Top);
            double right = Math.Min(rect.Right, Control.Frame.Right);
            double bottom = Math.Min(rect.Bottom, Control.Frame.Bottom);
            double vCenter = (top + bottom) / 2.0;
            double hCenter = (left + right) / 2.0;


            var visibleIndexPath = Control.IndexPathsForVisibleRows;
            var offset = Control.ContentOffset.ToPoint();
            foreach (NSIndexPath indexPath in visibleIndexPath)
            {
                var cellFrame = Control.RectForRowAtIndexPath(indexPath).ToRectangle();
                var cellViewFrame = new Rectangle(cellFrame.Location - (Size)offset, cellFrame.Size);
                //System.Diagnostics.Debug.WriteLine ("ip:" + indexPath + " offset:"+offset  + " cellFrame:" + cellFrame + " viewFrame:"+viewFrame);
                DragEventArgs result;
                if (cellViewFrame.Contains(new Point(left, top)))
                {
                    result = ItemAt(indexPath);
                    if (result != null)
                    {
                        result.Alignment = HoverOverAlignment.Center;
                        return result;
                    }
                }
                if (cellViewFrame.Contains(new Point(hCenter, vCenter)))
                {
                    result = ItemAt(indexPath);
                    if (result != null)
                    {
                        result.Alignment = HoverOverAlignment.After;
                        return result;
                    }
                }
                if (cellViewFrame.Contains(new Point(right, bottom)))
                {
                    result = ItemAt(indexPath);
                    if (result != null)
                    {
                        result.Alignment = HoverOverAlignment.Before;
                        return result;
                    }
                }
            }
            //System.Diagnostics.Debug.WriteLine ("");
            */
            return null;

        }

        /*
        DragEventArgs ItemAt(NSIndexPath indexPath)
        {
            /*
			int section = 0;
			int row = 0;
			IEnumerable items = Element.ItemsSource;
			foreach (var item in items) {
				var group = item as IEnumerable;
				if (group != null) {
					if (section == indexPath.Section) {
						foreach (var groupItem in group) {
							if (row == indexPath.Row) {
								return new DragEventArgs { DeepIndex = new []  {section,row}, Item = groupItem as Item };
							}
							row++;
						}
					}
					section++;
				} else if (indexPath.Section > 0) {
					throw new NotSupportedException ("ListViewRenderer.CellAt: attempt to get cell from group>0 from 1D ItemsSource");
				} else {
					if (row == indexPath.Row) {
						return new DragEventArgs { DeepIndex = new [] {section,row}, Item = item as Item};
					}
					row++;
				}
			}
			return null;
			*/
        /*
        GroupWrapper group = ((ListView)Element).BaseItemsSource;
        if (group != null)
        {
            var displayDeepIndex = new[] { indexPath.Section, indexPath.Row };
            var item = group.ItemAtDeepIndex(displayDeepIndex);
            var sourceDeepIndex = group.DeepSourceIndexOf(item);
            return new DragEventArgs { DeepIndex = sourceDeepIndex, Item = item };
        }
        return null;
    }
    */
        /*
    Point ConvertToWindow(Point p)
    {
        return NativeView.ConvertPointToView(new CoreGraphics.CGPoint(p.X, p.Y), null).ToPoint();
    }
    */

        void ScrollToItem(object reqItem, object reqGroup, ScrollToPosition scrollToPosition, bool animated)
        {
            if (Control is Windows.UI.Xaml.Controls.ListView listView)
            {
                var scrollViewer = listView.GetChild<Windows.UI.Xaml.Controls.ScrollViewer>();
                if (scrollViewer != null)
                {
                    //System.Diagnostics.Debug.WriteLine("ScrollOffet["+scrollViewer.VerticalOffset+"] A");

                    var headerHeight = 0.0;
                    if (listView.Header is Xamarin.Forms.VisualElement xfHeader)
                        headerHeight = xfHeader.Height;
                    else if (listView.Header is Windows.UI.Xaml.FrameworkElement header)
                        headerHeight = header.Height;
                    var footerHeight = 0.0;
                    if (listView.Footer is Xamarin.Forms.VisualElement xfFooter)
                        footerHeight = xfFooter.Height;
                    else if (listView.Footer is Windows.UI.Xaml.FrameworkElement footer)
                        footerHeight = footer.Height;

                    //System.Diagnostics.Debug.WriteLine("\t headerHeight=["+headerHeight+"]");
                    //System.Diagnostics.Debug.WriteLine("\t OffsetFromTop=["+ ((Forms9Patch.ListView)Element).OffsetFromTop(reqItem) + "]");
                    //System.Diagnostics.Debug.WriteLine("\t CellHeight=[" + ((Forms9Patch.ListView)Element).CellHeightForItem(reqItem) + "]");
                    //System.Diagnostics.Debug.WriteLine("\t viewportHeight=["+scrollViewer.ViewportHeight+"]");

                    var targetPosition = headerHeight + ((Forms9Patch.ListView)Element).OffsetFromTop(reqItem);
                    if (scrollToPosition == ScrollToPosition.Center)
                        targetPosition += ((Forms9Patch.ListView)Element).CellHeightForItem(reqItem)/2;
                    else if (scrollToPosition == ScrollToPosition.End)
                        targetPosition += ((Forms9Patch.ListView)Element).CellHeightForItem(reqItem);

                    if (scrollToPosition == ScrollToPosition.Center)
                        targetPosition -= scrollViewer.ViewportHeight / 2;
                    else if (scrollToPosition == ScrollToPosition.End)
                        targetPosition -= scrollViewer.ViewportHeight;
                    //System.Diagnostics.Debug.WriteLine("\t targetPosition=["+targetPosition+"]");

                    scrollViewer.ChangeView(null, targetPosition, null, !animated);
                    //System.Diagnostics.Debug.WriteLine("ScrollOffet[" + scrollViewer.VerticalOffset + "] B");
                }
            }
        }


    }
}
