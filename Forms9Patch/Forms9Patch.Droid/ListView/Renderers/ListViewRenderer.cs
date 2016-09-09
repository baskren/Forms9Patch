using Android.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Forms9Patch.ListView), typeof(Forms9Patch.Droid.ListViewRenderer))]
namespace Forms9Patch.Droid
{
	public class ListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
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
			}
			Control.Divider = null;
			Control.DividerHeight = -1;
		}


		bool Extended(double delta) {
			return MinScroll (delta) || MaxScroll (delta);
		}

		bool MinScroll(double delta) {
			bool result = (delta < 0 && offset - delta <= 0);
			//System.Diagnostics.Debug.WriteLine ("\t\tMinScroll=["+result+"]");
			return result;
		}

		bool MaxScroll(double delta) {
			var lastPos = Control.LastVisiblePosition;
			if (delta > 0 && lastPos == Control.Adapter.Count - 1) {  
				var child = Control.GetChildAt (lastPos);
				if (child != null && child.Bottom <= Control.Height) {
					//System.Diagnostics.Debug.WriteLine ("MaxScroll=[true]");
					return true;
				}
				// else WTF Android!
			}
			//System.Diagnostics.Debug.WriteLine ("MaxScroll=[false]");
			return false;
		}

		int offset;
		int lastIndex=-1;
		bool ScrollBy(double delta) {
			int index = Control.FirstVisiblePosition;
			if (index != lastIndex) {
				var v = Control.GetChildAt (index);
				offset = (v == null) ? 0 : (v.Top - Control.ListPaddingTop);
				lastIndex = index;
			}
			//System.Diagnostics.Debug.WriteLine ("index["+index+"] offset=[" + offset + "] delta=["+delta+"]");

			//System.Diagnostics.Debug.WriteLine ("speed=["+speed+"] offset["+offset+"]");
			if (Extended(delta))
				return false;
			offset += (int)(delta * Forms.Context.Resources.DisplayMetrics.Density);
			Control.SetSelectionFromTop (index, -offset);
			return true;
		}


		internal Forms9Patch.DragEventArgs ItemAtPoint(Point p) {
			var scale = Forms.Context.Resources.DisplayMetrics.Density;
			var pos = Control.PointToPosition ((int)(p.X * scale), (int)(p.Y * scale));
			//System.Diagnostics.Debug.WriteLine ("p=["+p+"] pos=["+pos+"]" );
			return HoverOverForPosition (pos);
		}

		Forms9Patch.DragEventArgs FindItemDataUnderRectangle(Rectangle rect) {
			var listView = Control;

			double left = Math.Max(Forms.Context.ToPixels (rect.Left),listView.Left);
			double top = Math.Max(Forms.Context.ToPixels (rect.Top),listView.Top);
			double right = Math.Min(Forms.Context.ToPixels (rect.Right),listView.Right-Forms.Context.ToPixels (1.0));
			double bottom = Math.Min(Forms.Context.ToPixels (rect.Bottom),listView.Bottom-Forms.Context.ToPixels (1.0));
			double vCenter = (top + bottom) / 2.0;
			double hCenter = (left + right) / 2.0;

			var pCenter = listView.PointToPosition ((int)hCenter, (int)vCenter);
			var hoCenter = HoverOverForPosition (pCenter);
			if (hoCenter != null) {
				hoCenter.Alignment = HoverOverAlignment.Center;
				return hoCenter;
			}

			var pStart = listView.PointToPosition ((int)left, (int)top);
			var hoStart = HoverOverForPosition (pStart);
			if (hoStart != null) {
				hoStart.Alignment = HoverOverAlignment.After;
				return hoStart;
			}

			var pEnd = listView.PointToPosition ((int)right, (int)bottom);
			var hoEnd = HoverOverForPosition (pEnd);
			if (hoEnd != null) {
				hoEnd.Alignment = HoverOverAlignment.Before;
				return hoEnd;
			}
			return null;
		}

		internal Forms9Patch.DragEventArgs HoverOverForPosition(int pos) {
			object javaObj = Control.GetItemAtPosition (pos);
			if  (javaObj == null)
				return null;
			object obj = javaObj.GetType ().GetProperty ("Instance").GetValue (javaObj, null);
			return obj == null ? null : HoverOverForObject (obj);
			/*
					var limits = LimitsForCellAtPosition (p1);
					if (limits != null) {
						result.Start = limits.Item1;
						result.End = limits.Item2;
					}
					*/
		}

		bool _heightCalcReset = true;
		Dictionary<int,Tuple<double,double>> _calculatedHeights = new Dictionary<int, Tuple<double, double>>();
		Tuple<double,double> LimitsForCellAtPosition(int pos) {
			double listHeight = 0;
			int startingIndex = 0;
			if (_heightCalcReset) {
				_calculatedHeights.Clear ();
				_heightCalcReset = false;
			} else {
				if (_calculatedHeights.ContainsKey (pos))
					return _calculatedHeights [pos];
				for (int i = pos - 1; i >= 0; i--) {
					if (_calculatedHeights.ContainsKey (pos)) {
						startingIndex = i+1;
						listHeight = _calculatedHeights [pos].Item2;
					}
				}
			}
			var adapter = Control.Adapter;

			// note: only works with Vt scroll
			// can check via Control.CanScrollVertically() and Control.CanScrollHorizontally();

			for (int i = startingIndex; i <= pos; i++) {
				global::Android.Views.View cell = adapter.GetView (i, null, Control);
				if (cell != null) {
					cell.Measure (MeasureSpec.MakeMeasureSpec(Control.Width-Control.PaddingLeft-Control.PaddingRight,MeasureSpecMode.AtMost),0);
					var height = cell.MeasuredHeight/Forms.Context.ToPixels (1.0);
					//var width = cell.MeasuredWidth;
					//System.Console.WriteLine("i:"+i+" h:"+height);
					_calculatedHeights[i] = new Tuple<double,double> (listHeight, listHeight+height);
					if (i == pos)
						return _calculatedHeights [pos];
					listHeight += (double)height + Control.DividerHeight / Forms.Context.ToPixels (1.0); 
				}
			}

			//var cellView = this.Control.GetChildAt (pos);
			//System.Console.WriteLine("i:"+pos+" l:"+cellView.Left+" r:"+cellView.Right+" t:"+cellView.Top+" b:"+cellView.Bottom);
			return null;
		}



		Forms9Patch.DragEventArgs HoverOverForObject (object obj) {
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
							return new Forms9Patch.DragEventArgs {DeepIndex = new [] {section,row}, Item=obj as Item};
						row++;
					}
					section++;
				} else {
					if (item.Equals(obj))
						return new Forms9Patch.DragEventArgs {DeepIndex = new [] {section,row}, Item=obj as Item};
					row++;
				}
			}
			return null;
			*/

			Group group = ((ListView)Element).BaseItemsSource;
			if (group != null) {
				var item = obj as Item;
				var sourceDeepIndex = group.DeepSourceIndexOf (item);
				if (((ListView)Element).IsGroupingEnabled && sourceDeepIndex.Length>1)
				return new Forms9Patch.DragEventArgs { DeepIndex = sourceDeepIndex, Item = item };
			}
			return null;
		}

		Point ConvertToWindow(Point p) {
			var point = new [] { (int)p.X, (int)p.Y };
			Control.GetLocationInWindow(point);
			return new Point (point [0], point [1]);
		}
	}
}
