using Android.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Runtime;

[assembly: ExportRenderer(typeof(Forms9Patch.ListView), typeof(Forms9Patch.Droid.ListViewRenderer))]
namespace Forms9Patch.Droid
{
    public class ListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            var oldElement = e.OldElement as ListView;
            if (oldElement != null)
            {
                oldElement.RendererFindItemDataUnderRectangle -= FindItemDataUnderRectangle;
                oldElement.RendererScrollBy -= ScrollBy;
                oldElement.RendererScrollToPos -= ScrollToItem;
            }
            var newElement = e.NewElement as ListView;
            if (newElement != null)
            {
                newElement.RendererFindItemDataUnderRectangle += FindItemDataUnderRectangle;
                newElement.RendererScrollBy += ScrollBy;
                newElement.RendererScrollToPos += ScrollToItem;
                Control.SetOnScrollListener(new ScrollListener(newElement));
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

        bool Extended(double delta)
        {
            return MinScroll(delta) || MaxScroll(delta);
        }

        bool MinScroll(double delta)
        {
            bool result = (delta < 0 && offset - delta <= 0);
            //System.Diagnostics.Debug.WriteLine ("\t\tMinScroll=["+result+"]");
            return result;
        }

        bool MaxScroll(double delta)
        {
            var lastPos = Control.LastVisiblePosition;
            if (delta > 0 && lastPos == Control.Adapter.Count - 1)
            {
                var child = Control.GetChildAt(lastPos);
                if (child != null && child.Bottom <= Control.Height)
                {
                    //System.Diagnostics.Debug.WriteLine ("MaxScroll=[true]");
                    return true;
                }
                // else WTF Android!
            }
            //System.Diagnostics.Debug.WriteLine ("MaxScroll=[false]");
            return false;
        }

        int offset;
        int lastIndex = -1;
        bool ScrollBy(double delta)
        {
            int index = Control.FirstVisiblePosition;
            if (index != lastIndex)
            {
                var v = Control.GetChildAt(index);
                offset = (v == null) ? 0 : (v.Top - Control.ListPaddingTop);
                lastIndex = index;
            }
            //System.Diagnostics.Debug.WriteLine ("index["+index+"] offset=[" + offset + "] delta=["+delta+"]");

            //System.Diagnostics.Debug.WriteLine ("speed=["+speed+"] offset["+offset+"]");
            if (Extended(delta))
                return false;
            offset += (int)(delta * Settings.Context.Resources.DisplayMetrics.Density);
            Control.SetSelectionFromTop(index, -offset);
            return true;
        }

        void ScrollToItem(object reqItem, object reqGroup, ScrollToPosition scrollToPosition, bool animated)
        {
            ITemplatedItemsView<Cell> templatedItemsView = Element;
            if (Element == null)
                return;
            ITemplatedItemsList<Cell> templatedItems = templatedItemsView.TemplatedItems;
            Cell cell;
            int position;

            /*
			if (Element.IsGroupingEnabled)
			{
				var results = templatedItems.GetGroupAndIndexOfItem(reqGroup, reqItem);
				if (results.Item1 == -1 || results.Item2 == -1)
					return;

				var group = templatedItems.GetGroup(results.Item1);
				cell = group[results.Item2];

				position = templatedItems.GetGlobalIndexForGroup(group) + results.Item2 + 1;
			}
			else
			{
			*/
            position = templatedItems.GetGlobalIndexOfItem(reqItem);
            if (Element.IsGroupingEnabled)
            {
                int subIndex = 0;
                var groupIndex = templatedItems.GetGroupIndexFromGlobal(position, out subIndex);
                var group = templatedItems.GetGroup(groupIndex);
                subIndex = group.GetGlobalIndexOfItem(reqItem);
                cell = group[subIndex];
            }
            else
            {
                cell = templatedItems[position];
            }


            //cell = templatedItems[position];
            //}

            //Android offsets position of cells when using header
            int realPositionWithHeader = position + 1;

            if (scrollToPosition == ScrollToPosition.MakeVisible)
            {
                if (animated)
                    Control.SmoothScrollToPosition(realPositionWithHeader);
                else
                    Control.SetSelection(realPositionWithHeader);
                return;
            }

            //int height = Control.Height;
            var cellHeight = (int)cell.RenderHeight;
            if (cellHeight == -1)
            {
                int first = Control.FirstVisiblePosition;
                if (first <= position && position <= Control.LastVisiblePosition)
                    cellHeight = Control.GetChildAt(position - first).Height;
                else
                {
                    //var adapter = Control.Adapter as CellAdapter;

                    //Android.Views.View view = _adapter.GetView(position, null, null);
                    //view.Measure(MeasureSpecFactory.MakeMeasureSpec(Control.Width, MeasureSpecMode.AtMost), MeasureSpecFactory.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                    //cellHeight = view.MeasuredHeight;
                }
            }
            var scale = Settings.Context.Resources.DisplayMetrics.Density;
            cellHeight = (int)(cellHeight * scale);

            var y = 0;

            if (scrollToPosition == ScrollToPosition.Center)
                y = (int)(Control.Height / 2 - 0.75 * cellHeight / 2);
            else if (scrollToPosition == ScrollToPosition.End)
                y = (int)(Control.Height - cellHeight);

            //System.Diagnostics.Debug.WriteLine("==================================");
            //System.Diagnostics.Debug.WriteLine("y=["+y+"] ht=["+Control.Height+"] cellHt=["+cellHeight+"]");
            //System.Diagnostics.Debug.WriteLine(new System.Diagnostics.StackTrace());
            //System.Diagnostics.Debug.WriteLine("");

            if (animated)
                Control.SmoothScrollToPositionFromTop(realPositionWithHeader, y);
            else
                Control.SetSelectionFromTop(realPositionWithHeader, y);

            /*
			var cellView = Control.GetChildAt(pos);

			double cellHeight=50 * Forms9Patch.Display.Density;
			System.Diagnostics.Debug.WriteLine("cellHeight[" + cellHeight + "]");
			if (cellView == null)
			{
				for (int i = 0; i < Control.ChildCount; i++)
				{
					var child = Control.GetChildAt(i);
					if (child != null)
					{
						cellHeight = child.Height;
						System.Diagnostics.Debug.WriteLine("cellHeight from item [" + i + "]");
						break;
					}
				}
			}
			else
			{
				cellHeight = cellView.Height;
				System.Diagnostics.Debug.WriteLine("cellHeight from actual cell");
			}
			//double cellHeight = (cellView != null) ? cellView.Height : 40;
			System.Diagnostics.Debug.WriteLine("cellHeight["+cellHeight+"]");

			double offsetFromTop = 0;

			if (position == Xamarin.Forms.ScrollToPosition.MakeVisible)
			{
				if (pos < Control.FirstVisiblePosition)
					position = Xamarin.Forms.ScrollToPosition.Start;
				else if (pos > Control.LastVisiblePosition)
					position = Xamarin.Forms.ScrollToPosition.End;
				else
					return;
			}
			if (position == Xamarin.Forms.ScrollToPosition.Center)
				offsetFromTop = (Control.Height - cellHeight*0.75) / 2.0;
			else if (position == Xamarin.Forms.ScrollToPosition.End)
				offsetFromTop = (Control.Height);

			offsetFromTop += Control.Top - Control.ListPaddingTop;
			System.Diagnostics.Debug.WriteLine("pos=["+pos+"] offset=["+offsetFromTop+"]");



			if (animated)
				Control.SmoothScrollToPositionFromTop(pos, (int)offsetFromTop);
			else
				Control.SmoothScrollToPositionFromTop(pos, (int)offsetFromTop,0);
				*/
            /*
			if (cellView == null)
			{
				Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
				{
					ScrollToPosition(pos, position, animated);
					return false;
				});
			}
			*/
        }


        internal Forms9Patch.CellProximityEventArgs ItemAtPoint(Point p)
        {
            var scale = Settings.Context.Resources.DisplayMetrics.Density;
            var pos = Control.PointToPosition((int)(p.X * scale), (int)(p.Y * scale));
            //System.Diagnostics.Debug.WriteLine ("p=["+p+"] pos=["+pos+"]" );
            return HoverOverForPosition(pos);
        }

        Forms9Patch.CellProximityEventArgs FindItemDataUnderRectangle(Rectangle rect)
        {
            var listView = Control;

            double left = Math.Max(Settings.Context.ToPixels(rect.Left), listView.Left);
            double top = Math.Max(Settings.Context.ToPixels(rect.Top), listView.Top);
            double right = Math.Min(Settings.Context.ToPixels(rect.Right), listView.Right - Settings.Context.ToPixels(1.0));
            double bottom = Math.Min(Settings.Context.ToPixels(rect.Bottom), listView.Bottom - Settings.Context.ToPixels(1.0));
            double vCenter = (top + bottom) / 2.0;
            double hCenter = (left + right) / 2.0;

            var pCenter = listView.PointToPosition((int)hCenter, (int)vCenter);
            var hoCenter = HoverOverForPosition(pCenter);
            if (hoCenter != null)
            {
                hoCenter.Proximity = Proximity.Aligned;
                return hoCenter;
            }

            var pStart = listView.PointToPosition((int)left, (int)top);
            var hoStart = HoverOverForPosition(pStart);
            if (hoStart != null)
            {
                hoStart.Proximity = Proximity.After;
                return hoStart;
            }

            var pEnd = listView.PointToPosition((int)right, (int)bottom);
            var hoEnd = HoverOverForPosition(pEnd);
            if (hoEnd != null)
            {
                hoEnd.Proximity = Proximity.Before;
                return hoEnd;
            }
            return null;
        }

        internal Forms9Patch.CellProximityEventArgs HoverOverForPosition(int pos)
        {
            object javaObj = Control.GetItemAtPosition(pos);
            if (javaObj == null)
                return null;
            object obj = javaObj.GetType().GetProperty("Instance").GetValue(javaObj, null);
            return obj == null ? null : HoverOverForObject(obj);
            /*
					var limits = LimitsForCellAtPosition (p1);
					if (limits != null) {
						result.Start = limits.Item1;
						result.End = limits.Item2;
					}
					*/
        }

        bool _heightCalcReset = true;
        Dictionary<int, Tuple<double, double>> _calculatedHeights = new Dictionary<int, Tuple<double, double>>();
        Tuple<double, double> LimitsForCellAtPosition(int pos)
        {
            double listHeight = 0;
            int startingIndex = 0;
            if (_heightCalcReset)
            {
                _calculatedHeights.Clear();
                _heightCalcReset = false;
            }
            else
            {
                if (_calculatedHeights.ContainsKey(pos))
                    return _calculatedHeights[pos];
                for (int i = pos - 1; i >= 0; i--)
                {
                    if (_calculatedHeights.ContainsKey(pos))
                    {
                        startingIndex = i + 1;
                        listHeight = _calculatedHeights[pos].Item2;
                    }
                }
            }
            var adapter = Control.Adapter;

            // note: only works with Vt scroll
            // can check via Control.CanScrollVertically() and Control.CanScrollHorizontally();

            for (int i = startingIndex; i <= pos; i++)
            {
                global::Android.Views.View cell = adapter.GetView(i, null, Control);
                if (cell != null)
                {
                    cell.Measure(MeasureSpec.MakeMeasureSpec(Control.Width - Control.PaddingLeft - Control.PaddingRight, MeasureSpecMode.AtMost), 0);
                    var height = cell.MeasuredHeight / Settings.Context.ToPixels(1.0);
                    //var width = cell.MeasuredWidth;
                    //System.Console.WriteLine("i:"+i+" h:"+height);
                    _calculatedHeights[i] = new Tuple<double, double>(listHeight, listHeight + height);
                    if (i == pos)
                        return _calculatedHeights[pos];
                    listHeight += (double)height + Control.DividerHeight / Settings.Context.ToPixels(1.0);
                }
            }

            //var cellView = this.Control.GetChildAt (pos);
            //System.Console.WriteLine("i:"+pos+" l:"+cellView.Left+" r:"+cellView.Right+" t:"+cellView.Top+" b:"+cellView.Bottom);
            return null;
        }



        Forms9Patch.CellProximityEventArgs HoverOverForObject(object obj)
        {
            /*
			int section = 0;
			int row = 0;
			IEnumerable items = Element.ItemsSource;
			foreach (var item in items) {
				var group = item as IEnumerable;
				if (group != null) {
					if (group.Equals(obj))
						return null;
					row = 0;
					foreach (var groupItem in group) {
						if (groupItem.Equals(obj))
							return new Forms9Patch.CellProximityEventArgs {DeepIndex = new [] {section,row}, Item=obj as Item};
						row++;
					}
					section++;
				} else {
					if (item.Equals(obj))
						return new Forms9Patch.CellProximityEventArgs {DeepIndex = new [] {section,row}, Item=obj as Item};
					row++;
				}
			}
			return null;
			*/

            GroupWrapper group = ((ListView)Element).BaseItemsSource;
            if (group != null)
            {
                var item = obj as ItemWrapper;
                var sourceDeepIndex = group.DeepSourceIndexOf(item);
                if (((ListView)Element).IsGroupingEnabled && sourceDeepIndex.Length > 1)
                    return new Forms9Patch.CellProximityEventArgs { DeepIndex = sourceDeepIndex, Item = item };
            }
            return null;
        }

        Point ConvertToWindow(Point p)
        {
            var point = new[] { (int)p.X, (int)p.Y };
            Control.GetLocationInWindow(point);
            return new Point(point[0], point[1]);
        }
    }

    class ScrollListener : Java.Lang.Object, Android.Widget.ListView.IOnScrollListener
    {
        Forms9Patch.ListView Element;

        public ScrollListener(Forms9Patch.ListView element)
        {
            Element = element;
        }

        //bool _scrolling;
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            //System.Diagnostics.Debug.WriteLine("SCROLLING");
            IVisualElementRenderer renderer = Platform.GetRenderer(Element);
            if (renderer != null)
                Element.OnScrolling(this, EventArgs.Empty);
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            //System.Diagnostics.Debug.WriteLine("SCROLL STATE=["+scrollState+"]");
            IVisualElementRenderer renderer = Platform.GetRenderer(Element);
            if (renderer != null)
            {
                if (scrollState == ScrollState.Idle)
                    Element.OnScrolled(this, EventArgs.Empty);
            }
        }
    }

    class ChoiceListener : Java.Lang.Object, Android.Widget.AbsListView.IMultiChoiceModeListener
    {
        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            throw new NotImplementedException();
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            throw new NotImplementedException();
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            throw new NotImplementedException();
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            throw new NotImplementedException();
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support

        /*
		private bool disposedValue = false; // To detect redundant calls
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposedValue)
			{
				if (disposing)
				{
					throw new NotImplementedException();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ChoiceListener() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		*/
        #endregion
    }
}
