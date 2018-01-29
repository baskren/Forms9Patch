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

            var style = (Windows.UI.Xaml.Style)Windows.UI.Xaml.Application.Current.Resources["Forms9PatchListViewItem"];
            listView.ItemContainerStyle = style;
            //Windows.UI.Xaml.VisualStateManager.GoToState(listView, "PointerOver", false);

            /*
            var controlTemplate = new ControlTemplate();

            var group = new Windows.UI.Xaml.VisualStateGroup();
            var state = group.CurrentState;

            var groups = Windows.UI.Xaml.VisualStateManager.GetVisualStateGroups(listView);
           // groups.Add()
            var x = new Windows.UI.Xaml.VisualStateManager();
            Windows.UI.Xaml.VisualStateManager.SetCustomVisualStateManager(listView, x);
            //x.RegisterPropertyChangedCallback
            //listView.ItemTemplate = new Windows.UI.Xaml.DataTemplate();
            //listView.Style = null;

            //var style = listView.ItemContainerStyle;
            //var setters = style.Setters;
            //var template = style.GetValue(Windows.UI.Xaml.Controls.ListViewItem.TemplateProperty);

            /* doesn't work!
            listView.Resources["ListViewItemBackgroundPointerOver"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            listView.Resources["ListViewItemBackgroundPressed"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            listView.Resources["ListViewItemBackgroundSelected"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            listView.Resources["ListViewItemBackgroundSelectedPointerOver"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            listView.Resources["ListViewItemBackgroundSelectedPressed"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
            listView.Resources["ListViewItemBackgroundPressed"] = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);

            var template = listView.ItemTemplate;

            var style = listView.Style;
            // var containerStyle = listView.ItemContainerStyle;
            //var controlTemplate = (ControlTemplate)containerStyle.GetValue(Windows.UI.Xaml.Controls.ListViewItem.TemplateProperty);

            //containerStyle.SetValue(Windows.UI.Xaml.Controls.ListViewItem.TemplateProperty, new ControlTemplate(typeof(Windows.UI.Xaml.Controls.ListViewItem)));
            */

            /*
            var listViewItemPresenter = new Windows.UI.Xaml.Controls.Primitives.ListViewItemPresenter
            {

            };

            var listViewItemTemplate = new ControlTemplate(typeof(Windows.UI.Xaml.Controls.ListViewItem));
            //listViewItemTemplate.CreateContent();
            listView.ItemContainerStyle = new Windows.UI.Xaml.Style(typeof(Windows.UI.Xaml.Controls.ListViewItem));
            listView.ItemContainerStyle.SetValue(Windows.UI.Xaml.Controls.ListViewItem.TemplateProperty, listViewItemTemplate);
            */
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
            /*
            if (delta < 0 && Control.ContentOffset.Y + delta <= 0)
            {
                Control.ContentOffset = new CoreGraphics.CGPoint(Control.ContentOffset.X, 0);
                return false;
            }
            if (delta > 0 && Control.ContentOffset.Y + delta + Control.Bounds.Height > Control.ContentSize.Height)
            {
                Control.ContentOffset = new CoreGraphics.CGPoint(Control.ContentOffset.X, Control.ContentSize.Height - Control.Bounds.Height);
                return false;
            }
            //System.Diagnostics.Debug.WriteLine ("delta=["+delta+"]");
            Control.ContentOffset = new CoreGraphics.CGPoint(Control.ContentOffset.X, Control.ContentOffset.Y + delta);
            */
            return true;
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
            /*
            GroupWrapper group = ((ListView)Element).BaseItemsSource;
            //if (reqItem is ItemWrapper)
            //System.Diagnostics.Debug.WriteLine("   itemWrapper.ID=[" + ((ItemWrapper)reqItem).ID + "]  group.ID=[" + group.ID + "]");
            var path = group.DeepIndexOf(reqItem as ItemWrapper);

            //System.Diagnostics.Debug.WriteLine("path.Length=["+path.Length+"]["+path[0]+"]");
            if (path.Length < 1)
                return;
            nint section = (path.Length > 1 ? path[0] : 0);
            nint row = (path.Length > 1 ? path[1] : path[0]);

            UITableViewScrollPosition pos = UITableViewScrollPosition.None;
            switch (scrollToPosition)
            {
                case ScrollToPosition.Start:
                    pos = UITableViewScrollPosition.Top;
                    break;
                case ScrollToPosition.Center:
                    pos = UITableViewScrollPosition.Middle;
                    break;
                case ScrollToPosition.End:
                    pos = UITableViewScrollPosition.Bottom;
                    break;
            }

            Control.ScrollToRow(NSIndexPath.FromItemSection(row, section), pos, animated);
            */
        }

    }

    /*
    class ScrollDelegate : UITableViewDelegate
    {
        public Forms9Patch.ListView Element;

        public ScrollDelegate(Forms9Patch.ListView element) : base()
        {
            Element = element;
        }

        bool _scrolling;

        public override void DraggingStarted(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate DraggingStarted");
            Element?.OnScrolling(this, EventArgs.Empty);
            _scrolling = true;
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate DraggingEnded");
            if (!willDecelerate)
            {
                _scrolling = false;
                Element?.OnScrolled(this, EventArgs.Empty);
            }
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate DecelerationEnded");
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                _scrolling = false;
                Element?.OnScrolled(this, EventArgs.Empty);
                return false;
            });
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate Scrolled");
            if (_scrolling)
                Element?.OnScrolling(this, EventArgs.Empty);
        }


    }
    */
}
