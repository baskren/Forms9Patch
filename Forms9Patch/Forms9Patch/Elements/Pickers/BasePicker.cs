using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FormsGestures;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Forms9Patch
{
	/// <summary>
	/// Base picker.
	/// </summary>
	internal class BasePicker : ContentView
	{
		#region Properties

		#region Item Source & Templates
		/// <summary>
		/// Gets the item templates.
		/// </summary>
		/// <value>The item templates.</value>
		public DataTemplateSelector ItemTemplates
		{
			get { return _listView.ItemTemplates; }
			//set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// The items source property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BasePicker), null);
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		#endregion

		#region Position and Selection
		/// <summary>
		/// The index property.
		/// </summary>
		public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(BasePicker), -1, BindingMode.TwoWay);
		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		/// <summary>
		/// The selected item property.
		/// </summary>
		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(BasePicker), null, BindingMode.TwoWay);
		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { 
				SetValue(SelectedItemProperty, value); 
			}
		}


		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create("SelectedItems", typeof(ObservableCollection<object>), typeof(BasePicker), null);
		public ObservableCollection<object> SelectedItems
		{
			get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		/*
		public ObservableCollection<object> SelectedItems
		{
			get { return _listView.SelectedItems; }
			set
			{
				_listView.SelectedItems.Clear();
				foreach (var item in value)
					_listView.SelectedItems.Add(item);
			}
		}
		*/


		/// <summary>
		/// The backing store for the ListViews's GroupToggleBehavior property.
		/// </summary>
		public static readonly BindableProperty GroupToggleBehaviorProperty = ListView.GroupToggleBehaviorProperty;
		/// <summary>
		/// Gets or sets the ListViews's GroupToggle behavior.
		/// </summary>
		/// <value>The Toggle behavior (None, Radio, Multiselect).</value>
		public GroupToggleBehavior GroupToggleBehavior
		{
			get { return (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty); }
			set { SetValue(GroupToggleBehaviorProperty, value); }
		}
		#endregion

		#region Appearance
		/// <summary>
		/// The row height property.
		/// </summary>
		public static readonly BindableProperty RowHeightProperty = BindableProperty.Create("RowHeight", typeof(int), typeof(BasePicker), 30);
		/// <summary>
		/// Gets or sets the height of the row.
		/// </summary>
		/// <value>The height of the row.</value>
		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		/// <summary>
		/// The accessory position property.
		/// </summary>
		public static readonly BindableProperty AccessoryPositionProperty = Item.AccessoryPositionProperty;
		/// <summary>
		/// Gets or sets the accessory position.
		/// </summary>
		/// <value>The accessory position.</value>
		public AccessoryPosition AccessoryPosition
		{
			get { return (AccessoryPosition)GetValue(AccessoryPositionProperty); }
			set { SetValue(AccessoryPositionProperty, value); }
		}

		/// <summary>
		/// The accessory text property.
		/// </summary>
		public static readonly BindableProperty AccessoryTextProperty = Item.AccessoryTextProperty;
		/// <summary>
		/// Gets or sets the accessory text.
		/// </summary>
		/// <value>The accessory text.</value>
		public Func<IItem, string> AccessoryText
		{
			get { return (Func<IItem, string>)GetValue(AccessoryTextProperty); }
			set { SetValue(AccessoryTextProperty, value); }
		}

		/// <summary>
		/// The accessory width property.
		/// </summary>
		public static readonly BindableProperty AccessoryWidthProperty = Item.AccessoryWidthProperty;
		/// <summary>
		/// Gets or sets the width of the accessory.
		/// </summary>
		/// <value>The width of the accessory.</value>
		public double AccessoryWidth
		{
			get { return (double)GetValue(AccessoryWidthProperty); }
			set { SetValue(AccessoryWidthProperty, value); }
		}

		/// <summary>
		/// The accessory horizonatal alignment property.
		/// </summary>
		public static readonly BindableProperty AccessoryHorizonatalAlignmentProperty = Item.AccessoryHorizonatalAlignmentProperty;
		/// <summary>
		/// Gets or sets the accessory horizontal alignment.
		/// </summary>
		/// <value>The accessory horizontal alignment.</value>
		public TextAlignment AccessoryHorizontalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryHorizonatalAlignmentProperty); }
			set { SetValue(AccessoryHorizonatalAlignmentProperty, value); }
		}

		/// <summary>
		/// The accessory vertical alignment property.
		/// </summary>
		public static readonly BindableProperty AccessoryVerticalAlignmentProperty = Item.AccessoryVerticalAlignmentProperty;
		/// <summary>
		/// Gets or sets the accessory vertical alignment.
		/// </summary>
		/// <value>The accessory vertical alignment.</value>
		public TextAlignment AccessoryVerticalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryVerticalAlignmentProperty); }
			set { SetValue(AccessoryVerticalAlignmentProperty, value); }
		}
		#endregion

		#endregion


		#region Fields
		ObservableCollection<object> _col = new ObservableCollection<object>();

		readonly ListView _listView = new ListView
		{
			IsGroupingEnabled = false,
			SeparatorIsVisible = false,
			BackgroundColor = Color.Transparent
		};

		readonly BoxView _upperPadding = new BoxView
		{
			BackgroundColor = Color.Transparent
		};

		readonly BoxView _lowerPadding = new BoxView
		{
			BackgroundColor = Color.Transparent
		};

		//internal bool PositionToSelect;
		internal SelectBy SelectBy;
		bool _tapping = false;
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.BasePicker"/> class.
		/// </summary>
		public BasePicker()
		{
			BackgroundColor = Color.FromRgba(0.5,0.5,0.5,0.125);

			_listView.SetBinding(Xamarin.Forms.ListView.RowHeightProperty, RowHeightProperty.PropertyName);
			_listView.SetBinding(ListView.AccessoryTextProperty, AccessoryTextProperty.PropertyName);
			_listView.SetBinding(ListView.AccessoryPositionProperty, AccessoryPositionProperty.PropertyName);
			_listView.SetBinding(ListView.GroupToggleBehaviorProperty, GroupToggleBehaviorProperty.PropertyName);
			_listView.BindingContext = this;
			//UpdateUnboundListViewProperties();
			_listView.BackgroundColor = Color.Transparent;
			_listView.SelectedCellBackgroundColor = Color.Transparent;

			_listView.ItemAppearing += OnCellAppearing;
			_listView.ItemTapped += OnItemTapped;

			Content = _listView;

			var listener = new Listener(_listView);
			listener.Panned += OnPanned;
			listener.Panning += (sender, e) =>
			{
				//System.Diagnostics.Debug.WriteLine("Listener(_listView).Panning");
				_lastAppearance = DateTime.Now;
			};
			/*
			listener.Tapped += (object sender, TapEventArgs e) => 
			{
				_tapping = true;
				//System.Diagnostics.Debug.WriteLine("Listener(_listView).Tapped");
				var point = e.Touches[0];
				var indexPath = ListViewExtensions.IndexPathAtPoint(_listView,point);
				if (indexPath != null)
					Index = indexPath.Item2;
				_tapping = false;
			};
			*/

			_listView.TranslationY = Device.OnPlatform<double>(0, -7, 0);
			_listView.Header = _upperPadding;
			_listView.Footer = _lowerPadding;

			SelectedItems = _listView.SelectedItems;
		}
		#endregion


		#region Selection management
		protected virtual void OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			_tapping = true;
			//_listView.ScrollTo(e.Item, ScrollToPosition.Center, true);
			int index=0;
			foreach (var item in ItemsSource)
			{
				if (item == e.Item)
				{
					Index = index;
					break;
				}
				index++;
			}
			_tapping = false;
		}

		void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var item in e.NewItems)
						if (!_listView.SelectedItems.Contains(item))
							_listView.SelectedItems.Add(item);
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
					_listView.SelectedItems.Clear();
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var item in e.OldItems)
						if (_listView.SelectedItems.Contains(item))
							_listView.SelectedItems.Remove(item);
					break;
				case NotifyCollectionChangedAction.Replace:
					foreach (var item in e.OldItems)
						if (_listView.SelectedItems.Contains(item))
							_listView.SelectedItems.Remove(item);
					foreach (var item in e.NewItems)
						if (!_listView.SelectedItems.Contains(item))
							_listView.SelectedItems.Add(item);
					break;
			}
		}
		#endregion


		#region Property Change management
		/// <summary>
		/// Ons the property changing.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				var notifiableCollection = ItemsSource as INotifyCollectionChanged;
				if (notifiableCollection != null)
					notifiableCollection.CollectionChanged -= SourceCollectionChanged;
				_col.Clear();
			}
			else if (propertyName == SelectedItemsProperty.PropertyName)
			{
				if (SelectedItems != null && SelectedItems != _listView.SelectedItems)
					SelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
				_listView.SelectedItems.Clear();
			}
		}

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				var notifiableCollection = ItemsSource as INotifyCollectionChanged;
				if (notifiableCollection != null)
					notifiableCollection.CollectionChanged += SourceCollectionChanged;
				_col = new ObservableCollection<object>((System.Collections.Generic.IEnumerable<object>)ItemsSource);
				_listView.ItemsSource = _col;
				ScrollToIndex(Index);
			}
			else if (propertyName == HeightProperty.PropertyName || propertyName == RowHeightProperty.PropertyName)
			{
				_upperPadding.HeightRequest = (Height - RowHeight) / 2.0 + Device.OnPlatform(0, 8, 0);
				_lowerPadding.HeightRequest = (Height - RowHeight) / 2.0;
			}
			else if (propertyName == IndexProperty.PropertyName && (GroupToggleBehavior != GroupToggleBehavior.Multiselect || !_tapping))
				ScrollToIndex(Index);
			else if (propertyName == SelectedItemsProperty.PropertyName && SelectedItems != null && SelectedItems != _listView.SelectedItems)
			{
				foreach (var item in SelectedItems)
					_listView.SelectedItems.Add(item);
				SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
			}
		}

		/// <summary>
		/// Scrolls the index of the to.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual void ScrollToIndex(int index)
		{
			if (_col.Count > 0)
			{
				if (index < 0)
					index = 0;
				if (index > _col.Count - 1)
					index = _col.Count - 1;
				var item = _col[index];
				_listView.ScrollTo(item, ScrollToPosition.Center, true);
				if (SelectBy==SelectBy.Position)
				{
					//var selectedF9PItem = _listView.BaseItemsSource.ItemAtIndexPath(new Tuple<int, int>(0, index));
					Index = index;
					var selectedF9PItem = _listView.BaseItemsSource[index] as Item;
					if (selectedF9PItem != null)
						SelectedItem = selectedF9PItem.Source;
				}
			}
		}


		void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					for (int i = 0; i < e.NewItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex,e.NewItems[i]);
					if (e.NewStartingIndex <= Index)
						Index += e.NewItems.Count;
					break;
				case NotifyCollectionChangedAction.Move:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex, e.OldItems[i]);
					if (Index >= e.OldStartingIndex && Index < e.OldStartingIndex + e.OldItems.Count)
					{
						var offset = Index - e.OldStartingIndex;
						Index = e.NewStartingIndex + offset;
					}
					else if (Index < e.OldStartingIndex && Index > e.NewStartingIndex)
						Index += e.NewItems.Count;
					else if (Index < e.NewStartingIndex && Index < e.OldStartingIndex)
						Index -= e.NewItems.Count;
					break;
				case NotifyCollectionChangedAction.Reset:
					_col.Clear();
					Index = -1;
					break;
				case NotifyCollectionChangedAction.Remove:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					if (Index >= e.OldStartingIndex + e.OldItems.Count)
						Index -= e.OldItems.Count;
					else if (Index >= e.OldStartingIndex)
						Index = e.OldStartingIndex - 1;
					break;
				case NotifyCollectionChangedAction.Replace:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					for (int i = 0; i < e.NewItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex, e.NewItems[i]);
					if (Index >= e.OldStartingIndex + e.OldItems.Count)
						Index += (e.NewItems.Count - e.OldItems.Count);
					break;
			}
		}
		#endregion


		#region Snap to cell
		DateTime _lastAppearance = DateTime.Now;
		void OnCellAppearing(object sender, ItemVisibilityEventArgs e)
		{
			_lastAppearance = DateTime.Now;
		}

		bool _waitingForScrollToComplete;
		void OnPanned(object sender, PanEventArgs e)
		{
			if (!_waitingForScrollToComplete)
			{
				_waitingForScrollToComplete = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(50), () => 
				{
					if (DateTime.Now - _lastAppearance > TimeSpan.FromMilliseconds(350))
					{
						var indexPath = ListViewExtensions.IndexPathAtCenter(_listView);
						if (indexPath != null)
						{
							if (indexPath.Item1 != 0)
								throw new InvalidDataContractException("SinglePicker should not be grouped");
							Index = indexPath.Item2;
							ScrollToIndex(Index);
						}
						else 
						{
							if (_listView.HitTest(_listView.Bounds.Center, _lowerPadding))
								Index = _col.Count - 1;
							else if (_listView.HitTest(_listView.Bounds.Center, _upperPadding))
								Index = 0;
							if (SelectBy==SelectBy.Position)
							{
								var selectedF9PItem = _listView.BaseItemsSource[Index] as Item;
								if (selectedF9PItem != null)
									SelectedItem = selectedF9PItem.Source;
							}
						}
						_waitingForScrollToComplete = false;
						return false;
					}
					return true;
				});
			}
		}

		#endregion
	}
}

