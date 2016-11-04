using System;
using Xamarin.Forms;
using System.ComponentModel;
using FormsGestures;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView List view.
	/// </summary>
	public class ListView : Xamarin.Forms.ListView
	{

		#region Properties

		#region Item Source / CellTemplates
		/// <summary>
		/// The item template property.
		/// </summary>
		[Obsolete("Use Forms9Patch.ListView.ItemTemplates property instead.", true)]
		public static new readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(Xamarin.Forms.DataTemplate), typeof(ListView), null);
		/// <summary>
		/// The item template property.
		/// </summary>
		[Obsolete("Use Forms9Patch.ListView.ItemTemplates property instead.", true)]
		public new Xamarin.Forms.DataTemplateSelector ItemTemplate
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public Forms9Patch.DataTemplateSelector ItemTemplates
		{
			get { return (Forms9Patch.DataTemplateSelector)GetValue(Xamarin.Forms.ListView.ItemTemplateProperty); }
			private set { 
				SetValue(Xamarin.Forms.ListView.ItemTemplateProperty, value); 
			}
		}

		/// <summary>
		/// The source property map property.
		/// </summary>
		public static readonly BindableProperty SourcePropertyMapProperty = BindableProperty.Create("SourcePropertyMap", typeof(List<string>), typeof(ListView), default(List<string>));
		/// <summary>
		/// Gets or sets the source property map.  Used to map the properties in a heirachial ItemsSource used to make the heirachy and bind (as items) to the CellViews
		/// </summary>
		/// <value>The source property map.</value>
		public List<string> SourcePropertyMap
		{
			get { return (List<string>)GetValue(SourcePropertyMapProperty); }
			set { SetValue(SourcePropertyMapProperty, value); }
		}

		/// <summary>
		/// The items source property.
		/// </summary>
		public static new readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("F9PItemsSource", typeof(IEnumerable), typeof(ListView), null);
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public new IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { 
				SetValue(ItemsSourceProperty, value);  
			}
		}


		#endregion

		#region Cell decoration

		#region Separator appearance
		/// <summary>
		/// The separator visibility property.
		/// </summary>
		[Obsolete("Use SeparatorIsVisibleProperty")]
		public static new readonly BindableProperty SeparatorVisibilityProperty = BindableProperty.Create("SeparatorVisibility", typeof(SeparatorVisibility), typeof(ListView), SeparatorVisibility.Default);
		/// <summary>
		/// Gets the separator visibility.
		/// </summary>
		/// <value>The separator visibility.</value>
		[Obsolete("Use SeparatorIsVisible")]
		public new SeparatorVisibility SeparatorVisibility
		{
			get { return (SeparatorVisibility)GetValue(SeparatorVisibilityProperty); }
			private set { SetValue(SeparatorVisibilityProperty, value); }
		}


		/// <summary>
		/// The separator visibility property.
		/// </summary>
		public static readonly BindableProperty SeparatorIsVisibleProperty = Item.SeparatorIsVisibleProperty;
		/// <summary>
		/// Gets or sets the separator visibility.
		/// </summary>
		/// <value>The separator visibility.</value>
		public bool SeparatorIsVisible {
			get { return (bool)GetValue (SeparatorIsVisibleProperty); }
			set { SetValue (SeparatorIsVisibleProperty, value); }
		}

		/// <summary>
		/// The separator color property.
		/// </summary>
		public static new readonly BindableProperty SeparatorColorProperty = Item.SeparatorColorProperty;
		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		public new Color SeparatorColor
		{
			get { return (Color)GetValue(SeparatorColorProperty); }
			set { 
				SetValue(SeparatorColorProperty, value); 
				System.Diagnostics.Debug.WriteLine("ListView.SeparatorColor["+value+"]");
			}
		}
		#endregion

		#region Background appearance
		/// <summary>
		/// The cell background color property.
		/// </summary>
		public static readonly BindableProperty CellBackgroundColorProperty = Item.CellBackgroundColorProperty;
		/// <summary>
		/// Gets or sets the color of the cell background.
		/// </summary>
		/// <value>The color of the cell background.</value>
		public Color CellBackgroundColor {
			get { return (Color)GetValue (CellBackgroundColorProperty); }
			set { SetValue (CellBackgroundColorProperty, value); }
		}

		/// <summary>
		/// The selected cell background color property.
		/// </summary>
		public static readonly BindableProperty SelectedCellBackgroundColorProperty = Item.SelectedCellBackgroundColorProperty;
		/// <summary>
		/// Gets or sets the color of the selected cell background.
		/// </summary>
		/// <value>The color of the selected cell background.</value>
		public Color SelectedCellBackgroundColor
		{
			get { return (Color)GetValue(SelectedCellBackgroundColorProperty); }
			set { SetValue(SelectedCellBackgroundColorProperty, value);
			}
		}
		#endregion

		#region Accessory appearance
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

		#region Item Selection
		/// <summary>
		/// The backing store for the ListViews's GroupToggleBehavior property.
		/// </summary>
		public static readonly BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(ListView), GroupToggleBehavior.Radio);
		/// <summary>
		/// Gets or sets the ListViews's GroupToggle behavior.
		/// </summary>
		/// <value>The Toggle behavior (None, Radio, Multiselect).</value>
		public GroupToggleBehavior GroupToggleBehavior
		{
			get { return (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty); }
			set { SetValue(GroupToggleBehaviorProperty, value); }
		}

		/// <summary>
		/// The most recently selected item property.
		/// </summary>
		public static new readonly BindableProperty SelectedItemProperty = BindableProperty.Create("Forms9Patch.ListView.SelectedItem", typeof(object), typeof(ListView), null);
		/// <summary>
		/// Gets or sets the most recently selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public new object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		/// <summary>
		/// The selected items property.
		/// </summary>
		public static readonly BindablePropertyKey SelectedItemsPropertyKey = BindableProperty.CreateReadOnly("SelectedItems", typeof(ObservableCollection<object>), typeof(ListView), null);
		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public ObservableCollection<object> SelectedItems
		{
			get { return (ObservableCollection<object>)GetValue(SelectedItemsPropertyKey.BindableProperty); }
			private set { SetValue(SelectedItemsPropertyKey, value); }
		}
		#endregion

		/// <summary>
		/// The editable property.
		/// </summary>
		public static readonly BindableProperty EditableProperty = BindableProperty.Create("Editable", typeof(bool), typeof(ListView), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ListView"/> is editable - cells may be moved or deleted based upon the response from the CanDrag CanDrop CanDelete delegate methods.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		public bool Editable
		{
			get { return (bool)GetValue(EditableProperty); }
			set { SetValue(EditableProperty, value); }
		}

		Group _baseItemsSource;
		/// <summary>
		/// Group backing store for the ItemsSource 
		/// </summary>
		/// <value>The base items source.</value>
		internal Group BaseItemsSource { get { return _baseItemsSource; } }

		public static readonly BindableProperty VisibilityTestProperty = BindableProperty.Create("VisibilityTest", typeof(Func<object,bool>), typeof(ListView), null);
		public Func<object,bool> VisibilityTest
		{
			get { return (Func<object,bool>)GetValue(VisibilityTestProperty); }
			set { SetValue(VisibilityTestProperty, value); }
		}

		#endregion


		#region Events

		#region Selection Events
		/// <summary>
		/// Occurs when cell is selected.
		/// </summary>
		public new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		/// <summary>
		/// Occurs when cell is tapped.
		/// </summary>
		public new event EventHandler<ItemTappedEventArgs> ItemTapped;


		Item _selectedF9PItem;
		List<Item> _selectedF9PItems = new List<Item>();
		bool _processingItemTapped;
		void OnItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (!_processingItemTapped)
			{
				_processingItemTapped = true;
				base.SelectedItem = null;

				var tappedItem = e.Item as Item;
				var group = e.Group as Group ?? _baseItemsSource;

				if (tappedItem?.Source != null)
				{
					// null source items are not tappable or selectable
					ItemTapped?.Invoke(this, new ItemTappedEventArgs(group.Source, tappedItem.Source, tappedItem.CellView));
					switch (GroupToggleBehavior)
					{
						case GroupToggleBehavior.None:
							_internalAddRemove = true;
							SelectedItems.Clear();
							_internalAddRemove = false;
							SelectedItem = null;
							if (_selectedF9PItem != null)
								_selectedF9PItem.IsSelected = false;
							_selectedF9PItem = null;
							if (_selectedF9PItems.Count > 0)
							{
								foreach (var item in _selectedF9PItems)
									item.IsSelected = false;
								_selectedF9PItems.Clear();
							}
							break;
						case GroupToggleBehavior.Radio:
							if (tappedItem != _selectedF9PItem)
							{
								RemoveSelectedItem(_selectedF9PItem);
								AddSelectedItem(tappedItem);
								ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(group.Source, tappedItem.Source, tappedItem.CellView));
							}
							break;
						case GroupToggleBehavior.Multiselect:
							if (_selectedF9PItems.Contains(tappedItem))
							{
								if (base.SelectedItem == tappedItem)
									base.SelectedItem = null;
								RemoveSelectedItem(tappedItem);
							}
							else
							{
								AddSelectedItem(tappedItem);
								ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(group.Source, tappedItem.Source, tappedItem.CellView));
							}
							break;
					}
				}
				_processingItemTapped = false;
			}
		}
		#endregion

		#region VisibilityEvents
		/// <summary>
		/// Occurs when item is appearing.
		/// </summary>
		public new event EventHandler<ItemVisibilityEventArgs> ItemAppearing;

		/// <summary>
		/// Occurs when item is disappearing.
		/// </summary>
		public new event EventHandler<ItemVisibilityEventArgs> ItemDisappearing;

		void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("OnItemAppearing");
			var item = ((Item)e?.Item);
			var source = item?.Source;
			if (source != null)
				ItemAppearing?.Invoke(this, new ItemVisibilityEventArgs(source));
		}

		void OnItemDisappearing(object sender, ItemVisibilityEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("OnItemDisappearing");
			var source = ((Item)e?.Item)?.Source;
			if (source != null)
				ItemDisappearing?.Invoke(this, new ItemVisibilityEventArgs(source));
			if (source == SelectedItem)
				base.SelectedItem = null;
		}
		#endregion

		#endregion


		#region Constructor
		static int Count;
		int id;
		Listener _listener;

		readonly ModalPopup _popup = new ModalPopup {
			Padding = 3,
			HasShadow = true,
			OutlineRadius = 4
		};

		void init() {
			//this.DisableSelection();
			//Margin = new Thickness(5, 0, 5, 0);

			id = Count++;
			HasUnevenRows = false;
			BackgroundColor = Color.Transparent;

			base.SeparatorColor = Color.Transparent;
			base.SeparatorVisibility = SeparatorVisibility.None;

			base.ItemAppearing += OnItemAppearing;
			base.ItemDisappearing += OnItemDisappearing;
			base.ItemTapped += OnItemTapped;

			IsEnabled = true;
			_listener = new Listener (this);
			_listener.LongPressed += OnLongPressed;
			_listener.LongPressing += OnLongPressing;
			_listener.Panning += OnPanning;


			SelectedItems = new ObservableCollection<object>();
			SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;

			ItemTemplates = new DataTemplateSelector();
			/*
			Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
			{
				System.Diagnostics.Debug.WriteLine("\tbefore BaseItemsSource.AccessoryPosition=[" + BaseItemsSource.AccessoryPosition + "]");
				BaseItemsSource.AccessoryPosition = AccessoryPosition.Start;
				System.Diagnostics.Debug.WriteLine("\tafter BaseItemsSource.AccessoryPosition=[" + BaseItemsSource.AccessoryPosition + "]");
				return false;
			});
			*/
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
		/// </summary>
		/// <param name="strategy">Strategy.</param>
		public ListView(ListViewCachingStrategy strategy) : base (strategy){
			init ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
		/// </summary>
		public ListView() {
			init ();
		}

		/// <summary>
		/// Description this instance.
		/// </summary>
		public string Description() {
			return "ListView[" + id + "]";
		}
		#endregion


		#region SelectItems management
		// Assumption: SelectedItem(s) must be set AFTER ItemsSource and ItemsSourceMap has been set.  Otherwise, selected items will be culled

		bool _internalAddRemove = false;

		void AddSelection(Item f9pItem, object sourceItem)
		{
			_internalAddRemove = true;
			if (f9pItem == null || sourceItem == null)
				throw new InvalidOperationException("Cannot select null item");
			f9pItem.IsSelected = true;
			if (!_selectedF9PItems.Contains(f9pItem))
				_selectedF9PItems.Add(f9pItem);
			_selectedF9PItem = f9pItem;
			if (sourceItem != SelectedItem)
				SelectedItem = sourceItem;
			if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && !SelectedItems.Contains(sourceItem))
				SelectedItems.Add(sourceItem);
			_internalAddRemove = false;
		}

		void AddSelectedItem(Item f9pItem)
		{
			if (f9pItem == null)
				return;
			var sourceItem = f9pItem.Source;
			AddSelection(f9pItem, sourceItem);
		}

		void AddSelectedSourceItem(object sourceItem)
		{
			if (sourceItem == null)
				return;
			var f9pItem = _baseItemsSource.ItemWithSource(sourceItem);
			AddSelection(f9pItem, sourceItem);
		}

		void RemoveSelection(Item f9pItem, object sourceItem)
		{
			_internalAddRemove = true;
			if (f9pItem != null)
			{
				f9pItem.IsSelected = false;
				if (_selectedF9PItems.Contains(f9pItem))
					_selectedF9PItems.Remove(f9pItem);
			}
			if (SelectedItem == sourceItem)
			{
				SelectedItem = null;
				_selectedF9PItem = null;
			}
			if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && sourceItem != null && SelectedItems.Contains(sourceItem) )
				SelectedItems.Remove(sourceItem);
			_internalAddRemove = false;
		}

		void RemoveSelectedItem(Item f9pItem)
		{
			if (f9pItem == null)
				return;
			var sourceItem = f9pItem.Source;
			RemoveSelection(f9pItem,sourceItem);
		}

		void RemoveSelectedSourceItem(object sourceItem)
		{
			if (sourceItem == null)
				return;
			var f9pItem = _baseItemsSource.ItemWithSource(sourceItem);
			RemoveSelection(f9pItem, sourceItem);
		}

		void AddSelectedSourceItems(System.Collections.IList sourceItems)
		{
			if (sourceItems == null)
				return;
			foreach (var sourceItem in sourceItems)
				AddSelectedSourceItem(sourceItem);
		}

		void RemoveSelectedSourceItems(System.Collections.IList sourceItems)
		{
			if (sourceItems == null)
				return;
			for (int i = sourceItems.Count-1; i >=0 ;i--)
			{
				var sourceItem = sourceItems[i];
				RemoveSelectedSourceItem(sourceItem);
			}
		}

		void SelectedItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (_internalAddRemove)
				return;
			switch (e.Action)
			{
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					AddSelectedSourceItems(e.NewItems);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					RemoveSelectedSourceItems(e.OldItems);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
					RemoveSelectedSourceItems(e.OldItems);
					AddSelectedSourceItems(e.NewItems);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
					RemoveSelectedSourceItems(SelectedItems);
					break;
			}
		}

		void ReevaluateSelectedItems()
		{
			// remove any SelectedItems that are not a Item.Source in the _baseItemsSource
			_selectedF9PItem = null;
			_selectedF9PItems.Clear();
			if (GroupToggleBehavior == GroupToggleBehavior.Multiselect)
			{
				for (int i = SelectedItems.Count - 1; i >= 0; i--)
				{
					var sourceItem = SelectedItems[i];
					var item = _baseItemsSource.ItemWithSource(sourceItem);
					if (item == null)
						RemoveSelectedSourceItem(sourceItem);
					else
						AddSelectedSourceItem(sourceItem);
				}
			}
			else if (GroupToggleBehavior == GroupToggleBehavior.Radio)
			{
				var item = _baseItemsSource.ItemWithSource(SelectedItem);
				if (item == null)
					RemoveSelectedSourceItem(SelectedItem);
				else
					AddSelectedSourceItem(SelectedItem);
			}
		}

		#endregion


		#region ListView property change management
		/// <summary>
		/// Ons the property changing.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior == GroupToggleBehavior.Radio) {
				RemoveSelectedSourceItem(SelectedItem);
			}
			/*
			else if (propertyName == SelectedItemsProperty.PropertyName)
			{
				if (SelectedItems != null)
					SelectedItems.CollectionChanged -=  SelectedItemsCollectionChanged;
				RemoveSelectedSourceItems(SelectedItems);
			}
			*/
		}


		/// <summary>
		/// Trigged with a property has changed
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);
			/*
			if (propertyName == SeparatorColorProperty.PropertyName
				|| propertyName == SeparatorVisibilityProperty.PropertyName
				|| propertyName == CellBackgroundColorProperty.PropertyName
				|| propertyName == SelectedCellBackgroundColorProperty.PropertyName
			   )
				UpdateCellProperties();

			else*/
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				//System.Diagnostics.Debug.WriteLine("ListView.OnPropertyChanging(ItemsSource)");
				UpdateItemsSource();
			}
			else if (propertyName == SourcePropertyMapProperty.PropertyName)
				UpdateItemsSource();
			else if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
				AddSelectedSourceItem(SelectedItem);
			else if (BaseItemsSource != null)
			{
				if (propertyName == VisibilityTestProperty.PropertyName)
					BaseItemsSource.VisibilityTest = VisibilityTest;
				else if (propertyName == HasUnevenRowsProperty.PropertyName)
					BaseItemsSource.HasUnevenRows = HasUnevenRows;
				else if (propertyName == RowHeightProperty.PropertyName)
					BaseItemsSource.RowHeight = RowHeight;
			}
			/*
			else if (propertyName == SelectedItemsProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
			{
				AddSelectedSourceItems(SelectedItems);
				SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
			}
			*/
		}

		void UpdateItemsSource()
		{
			if (ItemsSource == null)
				return;
			//System.Diagnostics.Debug.WriteLine("UpdateItemsSource");
			//_baseItemsSource = new Group(ItemsSource, SourcePropertyMap);
			_baseItemsSource = new Group();
			_baseItemsSource.BindingContext = this;
			_baseItemsSource.SourceSubPropertyMap = SourcePropertyMap;
			_baseItemsSource.Source = ItemsSource;
			_baseItemsSource.VisibilityTest = VisibilityTest;
			_baseItemsSource.HasUnevenRows = HasUnevenRows;
			_baseItemsSource.RowHeight = RowHeight;
			base.ItemsSource = _baseItemsSource;
			IsGroupingEnabled = _baseItemsSource.ContentType == Group.GroupContentType.Lists;
			ReevaluateSelectedItems();
		}
		#endregion


		#region Item change management
		/// <summary>
		/// Occurs when a property of a ListViewItem is about to change.
		/// </summary>
		public event PropertyChangingEventHandler ItemPropertyChanging;
		void OnItemPropertyChanging(object sender, PropertyChangingEventArgs e) {
			//System.Diagnostics.Debug.WriteLine("OnItemPropertyChanging");
			ItemPropertyChanging?.Invoke(sender, e);
		}

		/// <summary>
		/// Occurs when property of a ListViewItem has changed.
		/// </summary>
		public event PropertyChangedEventHandler ItemPropertyChanged;
		void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine("OnItemPropertyChanged");
			ItemPropertyChanged?.Invoke(sender, e);
		}
		#endregion


		#region DragDrop support
		internal static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create("ScrollEnabled",typeof(bool),typeof(ListView),true);
		internal bool ScrollEnabled {
			get { return (bool)GetValue (ScrollEnabledProperty); }
			set { SetValue (ScrollEnabledProperty, value); }
		}


		internal Func<Rectangle, DragEventArgs> RendererFindItemDataUnderRectangle;
		internal DragEventArgs FindItemDataUnderRectangle(Rectangle rect) {
			return RendererFindItemDataUnderRectangle != null ? RendererFindItemDataUnderRectangle (rect) : null;
		}


		internal Func<double,bool> RendererScrollBy;
		internal Action<object, object, ScrollToPosition, bool> RendererScrollToPos;


		double _scrollSpeed;
		bool _scrolling;
		void ScrollSpeed(double speed) {
			if (!_scrolling &&  Math.Abs(_scrollSpeed) > 0) {
				_scrolling = true;
				Device.StartTimer (TimeSpan.FromMilliseconds (25), () => {
					_scrolling = RendererScrollBy(_scrollSpeed);
					_scrolling &= Math.Abs(_scrollSpeed) > 0;
					return _scrolling;
				});
			}
			_scrollSpeed = speed;
		}


		/// <summary>
		/// Scroll the ListView by so many DIPs
		/// </summary>
		/// <returns><c>true</c>, if by was scrolled, <c>false</c> otherwise.</returns>
		/// <param name="delta">Delta.</param>
		public bool ScrollBy(double delta) {
			return RendererScrollBy (delta);
		}



		/// <summary>
		/// Scrolls to.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="group">Group.</param>
		/// <param name="position">Position.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public new void ScrollTo(object item, object group, ScrollToPosition position, bool animated)
		{
			var itemGroup = _baseItemsSource.ItemWithSource(group) as Group;
			if (itemGroup != null)
			{
				var itemItem = itemGroup.ItemWithSource(item);
				Device.StartTimer(TimeSpan.FromMilliseconds(150), () => {
					if (Device.OS == TargetPlatform.Android)
					{
						if (IsGroupingEnabled)
							RendererScrollToPos?.Invoke(itemItem, group, position, animated);
						else
							RendererScrollToPos?.Invoke(itemItem, null, position, animated);
					}
					else 
					{ 
						if (IsGroupingEnabled) 
							base.ScrollTo(itemItem, itemGroup, position, animated); 
						else 
							base.ScrollTo(itemItem, position, animated); 
					}
					return false;
				});
			}
		}

		/// <summary>
		/// Scrolls to.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="position">Position.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public new void ScrollTo(object item, ScrollToPosition position, bool animated)
		{
			var itemItem = _baseItemsSource.ItemWithSource(item);
			if (itemItem != null)
				// required because of race condition: Xamarin.Forms ListView doesn't scroll to index if it's still digesting a new ItemsSource?
				Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
				{
					if (Device.OS == TargetPlatform.Android)
					{
						if (IsGroupingEnabled)
							RendererScrollToPos?.Invoke(itemItem, BaseItemsSource, position, animated);
	                    else
							RendererScrollToPos?.Invoke(itemItem, null, position, animated);
					}
						//RendererScrollToItemInGroup(item, group, position, animated);
					else
					{
						base.ScrollTo(itemItem, position, animated);
					}
					 return false;
				 });
		}
		#endregion


		#region Drag/Drop
		/// <summary>
		/// Delegate function that will be called to query if a item (at a deep index location) can be dragged
		/// </summary>
		public Func<ListView, object, int[],bool> CanDrag;
		/// <summary>
		/// Delegate function that will be called to query if a item being dragged can be dropped at location specified by a deep index.
		/// </summary>
		public Func<ListView, object, int[],bool> CanDrop;

		DragEventArgs _longPress;

		readonly NullItem _nullItem = new NullItem ();
		readonly BlankItem _blankItem = new BlankItem();

		Rectangle _nativeFrame;

		void OnLongPressing(object sender, LongPressEventArgs e) {
			if (!Editable)
				return;
			// will be called when the _listener (attached to this) detects a long press
			if (_longPress != null)
				return;

			// we need to know what item is being pressed and it's corresponding view
			//System.Diagnostics.Debug.WriteLine ("LONGPRESSING ["+e.Listener.Element+"]");

			_longPress = DependencyService.Get<IListItemLocation>().DragEventArgsForItemAtPoint(this,e.Center);
			if (_longPress != null && _longPress.Item.BaseCellView != null) {
				// can we drag this Item?
				bool canDrag = true;
				if (CanDrag != null)
					canDrag = CanDrag (this,_longPress.Item.Source, _longPress.DeepIndex);
				if (!canDrag) {
					_longPress = null;
					return;
				}
				SelectedItem = null;
				_nativeFrame = _longPress.Item.BaseCellView.BoundsToWinCoord();

				// need a null item to fill the void 
				_nullItem.RequestedHeight = _nativeFrame.Height;
				_nullItem.CellBackgroundColor = BackgroundColor;

				_baseItemsSource.NotifySourceOfChanges = false;
				_baseItemsSource.DeepSwapItems (_longPress.Item, _nullItem);

				_longPress.Item.SeparatorIsVisible = false;
				var contentView = ItemTemplates.MakeContentView (_longPress.Item);
				contentView.WidthRequest = _nativeFrame.Width;
				contentView.HeightRequest = _nativeFrame.Height;
				contentView.BackgroundColor = Color.Transparent;
				//_popup.BackgroundColor = contentView.BackgroundColor == Color.Transparent ? Color.White : contentView.BackgroundColor;\
				_popup.BackgroundColor = CellBackgroundColor;
				_popup.Content = contentView;
				_popup.Location = _nativeFrame.Location;
				_popup.IsVisible = true;

				ScrollEnabled = false;
			} else {
				_longPress = null;
			}
		}


		void OnPanning(object sender, PanEventArgs e) {
			if (_longPress != null) {
				//_longPressPan = true;
				//System.Diagnostics.Debug.WriteLine("PAN ["+e.Listener.Element+"]");
				System.Diagnostics.Debug.WriteLine("LONGPRESS PANNING");
				_popup.TranslationX = e.TotalDistance.X;
				_popup.TranslationY = e.TotalDistance.Y;

				Point currentOrigin = _nativeFrame.Location + (Size)e.TotalDistance;
				var cellRect = new Rectangle (currentOrigin, _nativeFrame.Size);
				var currentDragOver = FindItemDataUnderRectangle(cellRect);
				if (currentDragOver != null && currentDragOver.Item != _nullItem && currentDragOver.Item!=null) {
					//System.Diagnostics.Debug.WriteLine ("current=[{0}]", currentDragOver.Item.Title);
					// can we drop here?
					bool canDrop = true;
					if (CanDrop != null)
						canDrop = CanDrop(this, currentDragOver.Item.Source, currentDragOver.DeepIndex);
					if (canDrop) {
						// yes: put the NullItem here
						_baseItemsSource.DeepRemove(_nullItem);
						_baseItemsSource.DeepInsert (currentDragOver.DeepIndex, _nullItem);
					} else if (_baseItemsSource.DeepContains(_nullItem) && !_baseItemsSource.DeepIndexOf (_nullItem).SequenceEqual (_longPress.DeepIndex)) {
						// no: put the NullItem at the location where we started 
						_baseItemsSource.DeepRemove(_nullItem);
						_baseItemsSource.DeepInsert (_longPress.DeepIndex, _nullItem);
					}
				}

				if (cellRect.Top < 40 && e.DeltaDistance.Y <= 2)
					ScrollSpeed (cellRect.Top - 40);
				else if (cellRect.Bottom + 40 > Height && e.DeltaDistance.Y >= -2)
					ScrollSpeed (cellRect.Bottom + 40 - Height);
				else
					ScrollSpeed (0);
			}
		}


		void OnLongPressed(object sender, LongPressEventArgs e) {
			//System.Diagnostics.Debug.WriteLine ("LONGPRESSED ["+e.Listener.Element+"]");

			if (_longPress != null) {
				ScrollSpeed(0);
				ScrollEnabled = true;

				// next two lines are for Android - without them, if you try to drag the same cell twice, the ModalPopup content is blank.
				var blankView = ItemTemplates.MakeContentView (_blankItem);
				_popup.Content = blankView;

				_popup.IsVisible = false;

				// return to our pre-drag state
				var nullIndex = _baseItemsSource.DeepIndexOf (_nullItem);
				_baseItemsSource.DeepRemove (_nullItem);
				_baseItemsSource.DeepInsert (_longPress.DeepIndex, _longPress.Item);
				_baseItemsSource.NotifySourceOfChanges = true;
				if (!nullIndex.SequenceEqual (_longPress.DeepIndex)) {
					// we made a allowed move, so make that move
					_baseItemsSource.DeepRemove(_longPress.Item);
					_baseItemsSource.DeepInsert (nullIndex, _longPress.Item);
				}
				_longPress = null;
				_popup.Content = null;
			}

		}
		#endregion
	}
}

