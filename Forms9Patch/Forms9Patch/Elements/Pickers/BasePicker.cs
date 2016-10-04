using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FormsGestures;
using System.Runtime.Serialization;

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
			set { 
				SetValue(ItemsSourceProperty, value); 
			}
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
		/// The start accessory property.
		/// </summary>
		public static readonly BindableProperty StartAccessoryProperty = Item.StartAccessoryProperty;
		/// <summary>
		/// Gets or sets the start accessory.
		/// </summary>
		/// <value>The start accessory.</value>
		public CellAccessory StartAccessory
		{
			get { return (CellAccessory)GetValue(StartAccessoryProperty); }
			set { SetValue(StartAccessoryProperty, value); }
		}

		/// <summary>
		/// The end accessory property.
		/// </summary>
		public static readonly BindableProperty EndAccessoryProperty = Item.EndAccessoryProperty;
		/// <summary>
		/// Gets or sets the end accessory.
		/// </summary>
		/// <value>The end accessory.</value>
		public CellAccessory EndAccessory
		{
			get { return (CellAccessory)GetValue(EndAccessoryProperty); }
			set { SetValue(EndAccessoryProperty, value); }
		}
		#endregion

		#endregion


		#region Fields
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
		bool _tapping;
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.BasePicker"/> class.
		/// </summary>
		public BasePicker()
		{
			BackgroundColor = Color.FromRgba(0.5,0.5,0.5,0.125);

			_listView.SetBinding(Xamarin.Forms.ListView.RowHeightProperty, RowHeightProperty.PropertyName);
			_listView.SetBinding(ListView.StartAccessoryProperty, StartAccessoryProperty.PropertyName);
			_listView.SetBinding(ListView.EndAccessoryProperty, EndAccessoryProperty.PropertyName);
			_listView.SetBinding(ListView.GroupToggleBehaviorProperty, GroupToggleBehaviorProperty.PropertyName);
			// TODO: Why doesn't the below binding work?
			//_listView.SetBinding(ListView.ItemsSourceProperty, ItemsSourceProperty.PropertyName);
			_listView.BindingContext = this;
			_listView.BackgroundColor = Color.Transparent;
			_listView.SelectedCellBackgroundColor = Color.Transparent;

			_listView.ItemAppearing += OnCellAppearing;
			_listView.ItemTapped += OnItemTapped;

			Content = _listView;

			var listener = new Listener(_listView);
			listener.Panned += OnPanned;
			listener.Panning += (sender, e) => _lastAppearance = DateTime.Now;

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
		if (propertyName == SelectedItemsProperty.PropertyName)
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
			if (propertyName == ItemsSourceProperty.PropertyName)
				System.Diagnostics.Debug.WriteLine("");
			base.OnPropertyChanged(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				//System.Diagnostics.Debug.WriteLine("BasePicker.OnPropertyChanged(ItemsSource) value=["+ItemsSource+"]");
				_listView.ItemsSource = ItemsSource;
				if (ItemsSource != null)
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
			if (ItemsSource == null)
				return;

			int count = 0;
			object firstItem=null;
			object indexItem=null;
			object lastItem=null;
			foreach (var item in ItemsSource)
			{
				if (count == 0)
					firstItem = item;
				if (count == index)
					indexItem = item;
				lastItem = item;
				count++;
			}
			if (count > 0)
			{
				if (index < 0)
					indexItem = firstItem;
				if (index > count - 1)
					indexItem = lastItem;
				if (indexItem != null)
				{
					_listView.ScrollTo(indexItem, ScrollToPosition.Center, true);
					if (SelectBy == SelectBy.Position)
					{
						Index = index;
						var selectedF9PItem = _listView.BaseItemsSource[index] as Item;
						if (selectedF9PItem != null)
							SelectedItem = selectedF9PItem.Source;
					}
				}
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
							{
								//Index = _col.Count - 1;
								if (ItemsSource == null)
									return false;
								int count = 0;
								foreach (var item in ItemsSource)
									count++;
								Index = count-1;
							}
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

