using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using CoreGraphics;

[assembly: ExportRenderer(typeof(Forms9Patch.EnhancedListView), typeof(Forms9Patch.iOS.EnhancedListViewRenderer))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// List view renderer.
    /// </summary>
    public class EnhancedListViewRenderer : ListViewRenderer, IScrollView
    {

        ScrollDelegate ScrollDelegate;

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement is EnhancedListView oldElement)
            {
                oldElement.Renderer = null;

                if (Control != null)
                    Control.Delegate = null;
                if (ScrollDelegate != null)
                {
                    ScrollDelegate.Source = null;
                    ScrollDelegate.Element = null;
                    ScrollDelegate.Dispose();
                    ScrollDelegate = null;
                }
            }
            if (e.NewElement is EnhancedListView newElement)
            {
                newElement.Renderer = this;

                ScrollDelegate = new ScrollDelegate(newElement, Control.Source);
                Control.Delegate = ScrollDelegate;
            }
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                ScrollDelegate.Source = null;
                ScrollDelegate.Element = null;
                ScrollDelegate?.Dispose();
                ScrollDelegate = null;
                Control.Delegate = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ListView.ScrollEnabledProperty.PropertyName)
                Control.ScrollEnabled = (bool)Element.GetValue(ListView.ScrollEnabledProperty);
            else if (e.PropertyName == Xamarin.Forms.ListView.HasUnevenRowsProperty.PropertyName)
                ScrollDelegate.Source = Control.Source;
        }

        #region Scrolling
        public double ScrollOffset => Control.ContentOffset.Y;

        public bool ScrollBy(double delta, bool animated)
        {
            if (delta < 0 && Control.ContentOffset.Y + delta <= 0)
            {
                Control.SetContentOffset(new CGPoint(Control.ContentOffset.X, 0), animated);
                return false;
            }
            if (delta > 0 && Control.ContentOffset.Y + delta > Control.ContentSize.Height - Control.Bounds.Height)
            {
                Control.SetContentOffset(new CGPoint(Control.ContentOffset.X, Control.ContentSize.Height - Control.Bounds.Height), animated);
                return false;
            }
            Control.SetContentOffset(new CGPoint(Control.ContentOffset.X, Control.ContentOffset.Y + delta), animated);
            return true;
        }

        public bool ScrollTo(double offset, bool animated)
        {
            Control.SetContentOffset(new CGPoint(Control.ContentOffset.X, offset), animated);
            return offset >= 0 && offset < Control.ContentSize.Height - Control.Bounds.Height;
        }

        public bool IsScrollEnabled
        {
            get => Control.ScrollEnabled;
            set
            {
                Control.ScrollEnabled = value;
            }
        }

        #endregion

        #region Header
        public double HeaderHeight
        {
            get
            {
                if (Control?.TableHeaderView != null)
                    return Control.TableHeaderView.Bounds.Height;
                return 0;
            }
        }

        #endregion
    }

    class ScrollDelegate : UITableViewDelegate
    {
        public EnhancedListView Element;
        public UITableViewSource Source;

        public ScrollDelegate(EnhancedListView element, UITableViewSource source)
        {
            Element = element;
            Source = source;
        }

        bool _scrolling;


        #region Scroll Methods
        //Xamarin.Forms.ListViewDataSource
        public override void DraggingStarted(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate DraggingStarted");
            if (Element?.ItemsSource != null)
                Element?.OnScrolling(this, EventArgs.Empty);
            _scrolling = true;
            Source?.DraggingStarted(scrollView);
        }

        //Xamarin.Forms.ListViewDataSource
        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate.DraggingEnded: willDecelerate=[" + willDecelerate + "]");
            if (!willDecelerate)
            {
                _scrolling = false;
                if (Element?.ItemsSource != null)
                    Element?.OnScrolled(this, EventArgs.Empty);
            }
            Source?.DraggingEnded(scrollView, willDecelerate);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate.DecelerationEnded");
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                _scrolling = false;
                if (Element?.ItemsSource != null)
                    Element?.OnScrolled(this, EventArgs.Empty);
                return false;
            });
        }

        //Xamarin.Forms.ListViewDataSource
        public override void Scrolled(UIScrollView scrollView)
        {
            //System.Diagnostics.Debug.WriteLine("ScrollDelegate.Scrolled");
            if (_scrolling && Element?.ItemsSource != null)
                Element?.OnScrolling(this, EventArgs.Empty);
            Source?.Scrolled(scrollView);
        }
        #endregion

        #region Pass-Through methods
        //public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath) => Source?.AccessoryButtonTapped(tableView, indexPath);

        //public override UITableViewCellAccessory AccessoryForRow(UITableView tableView, NSIndexPath indexPath) => Source.AccessoryForRow(tableView, indexPath);

        //public override bool CanFocusRow(UITableView tableView, NSIndexPath indexPath) => Source.CanFocusRow(tableView, indexPath);

        //public override bool CanPerformAction(UITableView tableView, ObjCRuntime.Selector action, NSIndexPath indexPath, NSObject sender) => Source.CanPerformAction(tableView, action, indexPath, sender);

        //public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath) => Source.CellDisplayingEnded(tableView, cell, indexPath);

        //public override NSIndexPath CustomizeMoveTarget(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath) => Source.CustomizeMoveTarget(tableView, sourceIndexPath, proposedIndexPath);

        //public override void DidEndEditing(UITableView tableView, NSIndexPath indexPath) => Source.DidEndEditing(tableView, indexPath);

        //public override void DidUpdateFocus(UITableView tableView, UITableViewFocusUpdateContext context, UIFocusAnimationCoordinator coordinator) => Source.DidUpdateFocus(tableView, context, coordinator);

        //public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath) => Source.EditActionsForRow(tableView, indexPath);

        //public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath) => Source.EditingStyleForRow(tableView, indexPath);

        //public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath) => Source.EstimatedHeight(tableView, indexPath);

        //public override nfloat EstimatedHeightForFooter(UITableView tableView, nint section) => Source.EstimatedHeightForFooter(tableView, section);

        //public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section) => Source.EstimatedHeightForHeader(tableView, section);

        //public override void FooterViewDisplayingEnded(UITableView tableView, UIView footerView, nint section) => Source.FooterViewDisplayingEnded(tableView, footerView, section);

        //public override nfloat GetHeightForFooter(UITableView tableView, nint section) => Source.GetHeightForFooter(tableView, section);

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => Source.GetHeightForHeader(tableView, section);

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => Source.GetHeightForRow(tableView, indexPath);

        //public override NSIndexPath GetIndexPathForPreferredFocusedView(UITableView tableView) => Source.GetIndexPathForPreferredFocusedView(tableView);

        //public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath) => Source.GetLeadingSwipeActionsConfiguration(tableView, indexPath);

        //public override UISwipeActionsConfiguration GetTrailingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath) => Source.GetTrailingSwipeActionsConfiguration(tableView, indexPath);

        //public override UIView GetViewForFooter(UITableView tableView, nint section) => Source.GetViewForFooter(tableView, section);

        public override UIView GetViewForHeader(UITableView tableView, nint section) => Source.GetViewForHeader(tableView, section);

        //public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section) => Source.HeaderViewDisplayingEnded(tableView, headerView, section);

        //public override nint IndentationLevel(UITableView tableView, NSIndexPath indexPath) => Source.IndentationLevel(tableView, indexPath);

        //public override void PerformAction(UITableView tableView, ObjCRuntime.Selector action, NSIndexPath indexPath, NSObject sender) => Source.PerformAction(tableView, action, indexPath, sender);

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath) => Source.RowDeselected(tableView, indexPath);

        //public override void RowHighlighted(UITableView tableView, NSIndexPath rowIndexPath) => Source.RowHighlighted(tableView, rowIndexPath);

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath) => Source.RowSelected(tableView, indexPath);

        //public override void RowUnhighlighted(UITableView tableView, NSIndexPath rowIndexPath) => Source.RowUnhighlighted(tableView, rowIndexPath);

        //public override bool ShouldHighlightRow(UITableView tableView, NSIndexPath rowIndexPath) => Source.ShouldHighlightRow(tableView, rowIndexPath);

        //public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath) => Source.ShouldIndentWhileEditing(tableView, indexPath);

        //public override bool ShouldShowMenu(UITableView tableView, NSIndexPath rowAtindexPath) => Source.ShouldShowMenu(tableView, rowAtindexPath);

        //public override bool ShouldSpringLoadRow(UITableView tableView, NSIndexPath indexPath, IUISpringLoadedInteractionContext context) => Source.ShouldSpringLoadRow(tableView, indexPath, context);

        //public override bool ShouldUpdateFocus(UITableView tableView, UITableViewFocusUpdateContext context) => Source.ShouldUpdateFocus(tableView, context);

        //public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath) => Source.TitleForDeleteConfirmation(tableView, indexPath);

        //public override void WillBeginEditing(UITableView tableView, NSIndexPath indexPath) => Source.WillBeginEditing(tableView, indexPath);

        //public override NSIndexPath WillDeselectRow(UITableView tableView, NSIndexPath indexPath) => Source.WillDeselectRow(tableView, indexPath);

        //public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath) => Source.WillDisplay(tableView, cell, indexPath);

        //public override void WillDisplayFooterView(UITableView tableView, UIView footerView, nint section) => Source.WillDisplayFooterView(tableView, footerView, section);

        //public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section) => Source.WillDisplayHeaderView(tableView, headerView, section);

        //public override NSIndexPath WillSelectRow(UITableView tableView, NSIndexPath indexPath) => Source.WillSelectRow(tableView, indexPath);


        #endregion
    }
}