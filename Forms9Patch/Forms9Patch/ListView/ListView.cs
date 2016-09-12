using System;
using Xamarin.Forms;
using System.ComponentModel;
using FormsGestures;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView List view.
	/// </summary>
	public class ListView : Xamarin.Forms.ListView
	{

		#region Properties

		#region Item Source / CellTemplates
		[Obsolete("Use Forms9Patch.ListView.ItemTemplates property instead.", true)]
		/// <summary>
		/// The item template property.
		/// </summary>
		public static new readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(Xamarin.Forms.DataTemplate), typeof(ListView), null);
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
		public new Forms9Patch.DataTemplateSelector ItemTemplates
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
		public static new readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("Forms9Patch.ListView.ItemsSource", typeof(object), typeof(ListView), null);
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public new object ItemsSource
		{
			get { return GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value);  }
		}


		#endregion

		#region Cell decoration
		/// <summary>
		/// The separator visibility property.
		/// </summary>
		public static new readonly BindableProperty SeparatorVisibilityProperty = BindableProperty.Create ("Forms9Patch.ListView.SeparatorVisibility", typeof(SeparatorVisibility), typeof(ListView), default(SeparatorVisibility));
		/// <summary>
		/// Gets or sets the separator visibility.
		/// </summary>
		/// <value>The separator visibility.</value>
		public new SeparatorVisibility SeparatorVisibility {
			get { return (SeparatorVisibility)GetValue (SeparatorVisibilityProperty); }
			set { SetValue (SeparatorVisibilityProperty, value); }
		}

		/// <summary>
		/// The separator color property.
		/// </summary>
		public static new readonly BindableProperty SeparatorColorProperty = BindableProperty.Create("Forms9Patch.ListView.SeparatorColor", typeof(Color), typeof(ListView), Color.Gray);
		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		public new Color SeparatorColor
		{
			get { return (Color)GetValue(SeparatorColorProperty); }
			set { SetValue(SeparatorColorProperty, value); }
		}

		/// <summary>
		/// The cell background color property.
		/// </summary>
		public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create ("CellBackgroundColor", typeof(Color), typeof(ListView), Color.Transparent);
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
		public static readonly BindableProperty SelectedCellBackgroundColorProperty = BindableProperty.Create("SelectedCellBackgroundColor", typeof(Color), typeof(ListView), Color.FromRgba(200, 200, 200, 255));
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

		#region Item Selection
		/// <summary>
		/// The backing store for the ListViews's GroupToggleBehavior property.
		/// </summary>
		public static readonly BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create("GroupToggleBehavior", typeof(GroupToggleBehavior), typeof(MaterialSegmentedControl), GroupToggleBehavior.Radio);
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
		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create("SelectedItems", typeof(ObservableCollection<object>), typeof(ListView), null);
		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public ObservableCollection<object> SelectedItems
		{
			get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
			private set { SetValue(SelectedItemsProperty, value); }
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
		#endregion


		#region Events

		#region Selection Events
		/// <summary>
		/// Occurs when item is selected.
		/// </summary>
		public new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		/// <summary>
		/// Occurs when item is tapped.
		/// </summary>
		public new event EventHandler<ItemTappedEventArgs> ItemTapped;

		void OnItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			//if (GroupToggleBehavior == GroupToggleBehavior.None)
			//	base.SelectedItem = null;
		}

		Item _selectedF9PItem;
		List<Item> _selectedF9PItems = new List<Item>();
		void OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("Forms9Patch.ListView.OnItemTapped");
			var tappedItem = ((Item)e?.Item);
			var group = (Group)((BindableObject)e.Group)?.GetValue(Xamarin.Forms.ListView.ItemsSourceProperty);
			if (tappedItem?.Source != null)
			{
				// null source items are not tappable or selectable
				ItemTapped?.Invoke(this, new ItemTappedEventArgs(group, tappedItem.Source));
				switch (GroupToggleBehavior)
				{
					case GroupToggleBehavior.None:
						SelectedItems.Clear();
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
							ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(group, tappedItem.Source));
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
							ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(group, tappedItem.Source));
						}
					break;
				}
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

			id = Count++;
			HasUnevenRows = false;
			BackgroundColor = Color.Transparent;

			base.SeparatorColor = Color.Transparent;
			base.SeparatorVisibility = SeparatorVisibility.None;

			//base.ItemSelected += OnItemSelected;
			//base.ItemTapped += OnItemTapped;
			base.ItemAppearing += OnItemAppearing;
			base.ItemDisappearing += OnItemDisappearing;

			IsEnabled = true;
			_listener = new Listener (this);
			_listener.LongPressed += OnLongPressed;
			_listener.LongPressing += OnLongPressing;
			_listener.Panning += OnPanning;

			_listener.Tapped += (object sender, TapEventArgs e) =>
			{
				base.SelectedItem = null;

				//System.Diagnostics.Debug.WriteLine("Tapped location=["+e.Touches[0]+"]");
				if (e.Touches.Count() == 1)
				{
					var indexPath = this.IndexPathAtPoint(e.Touches[0]);
					if (indexPath == null)
						return;
					if (indexPath.Item2 < 0)
						return;
					//System.Diagnostics.Debug.WriteLine("indexPath=["+indexPath+"]");
					Item item=null;
					Group group = null;
					if (IsGroupingEnabled)
					{
						group = _baseItemsSource[indexPath.Item1] as Group;
						item = group[indexPath.Item2];
					}
					else
						item = _baseItemsSource[indexPath.Item2];
					var args = new ItemTappedEventArgs(group, item);
					OnItemTapped(this,args);
				}
			};


			SelectedItems = new ObservableCollection<object>();
			SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;

			ItemTemplates = new Forms9Patch.DataTemplateSelector();
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

		void AddSelection(Item f9pItem, object sourceItem)
		{
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
				for (int i = SelectedItems.Count - 1; i >= 0; i++)
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
			else if (propertyName == SelectedItemsProperty.PropertyName)
			{
				if (SelectedItems != null)
					SelectedItems.CollectionChanged -=  SelectedItemsCollectionChanged;
				RemoveSelectedSourceItems(SelectedItems);
			}
		}


		/// <summary>
		/// Trigged with a property has changed
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == SeparatorColorProperty.PropertyName
				|| propertyName == SeparatorVisibilityProperty.PropertyName
				|| propertyName == CellBackgroundColorProperty.PropertyName
				|| propertyName == SelectedCellBackgroundColorProperty.PropertyName
			   )
				UpdateCellProperties();
			else if (propertyName == ItemsSourceProperty.PropertyName)
				UpdateItemsSource();
			else if (propertyName == SourcePropertyMapProperty.PropertyName)
				UpdateItemsSource();
			else if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
				AddSelectedSourceItem(SelectedItem);
			else if (propertyName == SelectedItemsProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
			{
				AddSelectedSourceItems(SelectedItems);
				SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
			}
		}

		void UpdateItemsSource()
		{
			// be ready to set / reset IsGroupingEnabled as items are added or removed
			/* need to use Group to determine if grouping is enabled.
			var observableCollection = value as INotifyCollectionChanged;
			if (observableCollection != null) {
				observableCollection.CollectionChanged += (sender, e) => {
					switch (e.Action) {
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems.Count==ItemsSource.Count) 
							IsGroupingEnabled = value [0] is IList;
						break;
					case NotifyCollectionChangedAction.Move:
						break;
					case NotifyCollectionChangedAction.Remove:
						IsGroupingEnabled &= ItemsSource.Count > 0;
						break;
					case NotifyCollectionChangedAction.Replace:
						if (ItemsSource.Count==1)
							IsGroupingEnabled = value [0] is IList;
						break;
					case NotifyCollectionChangedAction.Reset:
						if (ItemsSource.Count > 0)
							IsGroupingEnabled = value [0] is IList;
						break;
					}
				};
			}
			*/
			// This listView.ItemsSource is a target, so validity testing is to facility drag/drop of items

			// unselected everything when Source is changed

			if (ItemsSource == null)
				return;
			//System.Diagnostics.Debug.WriteLine("UpdateItemsSource");
			_baseItemsSource = new Group(ItemsSource, SourcePropertyMap);
			base.ItemsSource = _baseItemsSource;
			IsGroupingEnabled = _baseItemsSource.ContentType == Group.GroupContentType.Lists;
			ReevaluateSelectedItems();
			UpdateCellProperties();
		}

		void UpdateCellProperties()
		{
			if (_baseItemsSource != null)
			{
				//System.Diagnostics.Debug.WriteLine("UpdateCellProperties");
				_baseItemsSource.SeparatorIsVisible = SeparatorVisibility != SeparatorVisibility.None;
				_baseItemsSource.BackgroundColor = CellBackgroundColor;
				_baseItemsSource.SeparatorColor = SeparatorColor;
				_baseItemsSource.SelectedBackgroundColor = SelectedCellBackgroundColor;
				//  System.Diagnostics.Debug.WriteLine("SeparatorColor=["+_baseItemsSource.SeparatorColor+"] SeparatorVisibility=["+_baseItemsSource.SeparatorIsVisible+"]");
			}
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

		public new void ScrollTo(object item, object group, ScrollToPosition position, bool animated)
		{
			var itemGroup = _baseItemsSource.ItemWithSource(group) as Group;
			if (itemGroup != null)
			{
				var itemItem = itemGroup.ItemWithSource(item);
				base.ScrollTo(itemItem,itemGroup,position,animated);
			}
		}

		public new void ScrollTo(object item, ScrollToPosition position, bool animated)
		{
			var itemItem = _baseItemsSource.ItemWithSource(item);
			if (itemItem != null)
				base.ScrollTo(itemItem, position, animated);
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
				_nullItem.BackgroundColor = BackgroundColor;

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

