using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;

[assembly: ExportRenderer(typeof(Forms9Patch.ListView), typeof(Forms9Patch.iOS.ListViewRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// List view renderer.
	/// </summary>
	public class ListViewRenderer : Xamarin.Forms.Platform.iOS.ListViewRenderer
	{
		/// <summary>
		/// Ons the element changed.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged(e);
			var oldElement = e.OldElement as ListView;
			if (oldElement != null) {
				oldElement.RendererFindItemDataUnderRectangle -= FindItemDataUnderRectangle;
				oldElement.RendererScrollBy -= ScrollBy;
			}
			var newElement = e.NewElement as ListView;
			if (newElement != null) {
				newElement.RendererFindItemDataUnderRectangle += FindItemDataUnderRectangle;
				newElement.RendererScrollBy += ScrollBy;
				if (newElement.IsScrollListening)
				  Control.Delegate = new ScrollDelegate(newElement);  // why does this cause headers to not appear?
				//Control.Delegate = null;
			}
		}

		/// <summary>
		/// Ons the element property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == ListView.ScrollEnabledProperty.PropertyName) {
				Control.ScrollEnabled = (bool) Element.GetValue (ListView.ScrollEnabledProperty);
			}
			else if (e.PropertyName == ListView.IsScrollListeningProperty.PropertyName && !Element.IsGroupingEnabled && Control!=null)
				Control.Delegate = new ScrollDelegate(Element as Forms9Patch.ListView);  // why does this cause headers to not appear?
		}

		bool Extended(double delta) {
			return MinScroll (delta) || MaxScroll (delta);
		}

		bool MinScroll(double delta) {
			return (delta < 0 && Control.ContentOffset.Y + delta <= 0);
		}

		bool MaxScroll(double delta) {
			return (delta > 0 && Control.ContentOffset.Y + delta + Control.Bounds.Height > Control.ContentSize.Height);
		}

		bool ScrollBy(double delta) {
			if (delta < 0 && Control.ContentOffset.Y + delta <= 0) {
				Control.ContentOffset = new CoreGraphics.CGPoint (Control.ContentOffset.X, 0);
				return false;
			}
			if (delta > 0 && Control.ContentOffset.Y + delta + Control.Bounds.Height > Control.ContentSize.Height) {
				Control.ContentOffset = new CoreGraphics.CGPoint(Control.ContentOffset.X,Control.ContentSize.Height - Control.Bounds.Height);
				return false;
			}
			//System.Diagnostics.Debug.WriteLine ("delta=["+delta+"]");
			Control.ContentOffset = new CoreGraphics.CGPoint(Control.ContentOffset.X,Control.ContentOffset.Y + delta);
			return true;
		}

		internal DragEventArgs ItemAtPoint(Point p) {
			var visibleIndexPath = Control.IndexPathsForVisibleRows;
			var offset = Control.ContentOffset.ToPoint();
			foreach (NSIndexPath indexPath in visibleIndexPath) {
				var cellFrame = Control.RectForRowAtIndexPath(indexPath).ToRectangle();
				var cellViewFrame = new Rectangle (cellFrame.Location - (Size) offset, cellFrame.Size);
				//System.Diagnostics.Debug.WriteLine ("ip:" + indexPath + " offset:"+offset  + " cellFrame:" + cellFrame + " viewFrame:"+viewFrame);
				//UITableViewCell cell;
				if (cellViewFrame.Contains (p)) 
					return  ItemAt (indexPath);
			}
			return null;
		}

		DragEventArgs FindItemDataUnderRectangle(Rectangle rect) {
			// return the best candidate to be replaced item (that is being hovered over by the frame represented by rect)
			double left = Math.Max(rect.Left,Control.Frame.Left);
			double top = Math.Max(rect.Top,Control.Frame.Top);
			double right = Math.Min(rect.Right,Control.Frame.Right);
			double bottom = Math.Min(rect.Bottom,Control.Frame.Bottom);
			double vCenter = (top + bottom) / 2.0;
			double hCenter = (left + right) / 2.0;


			var visibleIndexPath = Control.IndexPathsForVisibleRows;
			var offset = Control.ContentOffset.ToPoint();
			foreach (NSIndexPath indexPath in visibleIndexPath) {
				var cellFrame = Control.RectForRowAtIndexPath(indexPath).ToRectangle();
				var cellViewFrame = new Rectangle (cellFrame.Location - (Size) offset, cellFrame.Size);
				//System.Diagnostics.Debug.WriteLine ("ip:" + indexPath + " offset:"+offset  + " cellFrame:" + cellFrame + " viewFrame:"+viewFrame);
				DragEventArgs result;
				if (cellViewFrame.Contains (new Point (left, top))) {
					result = ItemAt (indexPath);
					if (result != null) {
						result.Alignment = HoverOverAlignment.Center;
						return result;
					}
				}
				if (cellViewFrame.Contains(new Point (hCenter, vCenter))) {
					result = ItemAt (indexPath);
					if (result != null) {
						result.Alignment = HoverOverAlignment.After;
						return result;
					}
				}
				if (cellViewFrame.Contains(new Point (right, bottom))) {
					result = ItemAt (indexPath);
					if (result != null) {
						result.Alignment = HoverOverAlignment.Before;
						return result;
					}
				}
			}
			//System.Diagnostics.Debug.WriteLine ("");
			return null;
		}

		DragEventArgs ItemAt(NSIndexPath indexPath) {
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

			GroupWrapper group = ((ListView)Element).BaseItemsSource;
			if (group != null) {
				var displayDeepIndex = new [] { indexPath.Section, indexPath.Row };
				var item = group.ItemAtDeepIndex (displayDeepIndex);
				var sourceDeepIndex = group.DeepSourceIndexOf (item);
				return new DragEventArgs { DeepIndex = sourceDeepIndex, Item = item };
			}
			return null;
		}

		Point ConvertToWindow(Point p) {
			return NativeView.ConvertPointToView (new CoreGraphics.CGPoint (p.X, p.Y), null).ToPoint();
		}


	}

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
}