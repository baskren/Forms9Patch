using System;
using Xamarin.Forms;
using System.ComponentModel;
using FormsGestures;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
        public static new readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(ListView), null);
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
        public DataTemplateSelector ItemTemplates
        {
            get { return (DataTemplateSelector)GetValue(ItemsView<Cell>.ItemTemplateProperty); }
            private set
            {
                SetValue(ItemsView<Cell>.ItemTemplateProperty, value);
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
        public static readonly BindableProperty F9PItemsSourceProperty = BindableProperty.Create("F9PItemsSource", typeof(IEnumerable), typeof(ListView), null);
        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable F9PItemsSource
        {
            get { return (IEnumerable)GetValue(F9PItemsSourceProperty); }
            set
            {
                SetValue(F9PItemsSourceProperty, value);
            }
        }


        #endregion

        #region Cell decoration

        #region Background appearance
        /// <summary>
        /// The cell background color property.
        /// </summary>
        public static readonly BindableProperty CellBackgroundColorProperty = ItemWrapper.CellBackgroundColorProperty;
        /// <summary>
        /// Gets or sets the color of the cell background.
        /// </summary>
        /// <value>The color of the cell background.</value>
        public Color CellBackgroundColor
        {
            get { return (Color)GetValue(CellBackgroundColorProperty); }
            set { SetValue(CellBackgroundColorProperty, value); }
        }

        /// <summary>
        /// The selected cell background color property.
        /// </summary>
        public static readonly BindableProperty SelectedCellBackgroundColorProperty = ItemWrapper.SelectedCellBackgroundColorProperty;
        /// <summary>
        /// Gets or sets the color of the selected cell background.
        /// </summary>
        /// <value>The color of the selected cell background.</value>
        public Color SelectedCellBackgroundColor
        {
            get { return (Color)GetValue(SelectedCellBackgroundColorProperty); }
            set
            {
                SetValue(SelectedCellBackgroundColorProperty, value);
            }
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
        public static new readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(ListView), null);
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

        GroupWrapper _baseItemsSource;
        /// <summary>
        /// Group backing store for the ItemsSource 
        /// </summary>
        /// <value>The base items source.</value>
        internal GroupWrapper BaseItemsSource { get { return _baseItemsSource; } }

        /// <summary>
        /// The cell visibility test property.
        /// </summary>
        public static readonly BindableProperty VisibilityTestProperty = BindableProperty.Create("VisibilityTest", typeof(Func<object, bool>), typeof(ListView), null);
        /// <summary>
        /// Gets or sets the cell visibility test.
        /// </summary>
        /// <value>The visibility test.</value>
        public Func<object, bool> VisibilityTest
        {
            get { return (Func<object, bool>)GetValue(VisibilityTestProperty); }
            set { SetValue(VisibilityTestProperty, value); }
        }

        /// <summary>
        /// The sub group type property backing store.
        /// </summary>
        public static readonly BindableProperty SubGroupTypeProperty = BindableProperty.Create("SubGroupType", typeof(Type), typeof(ListView), null);
        /// <summary>
        /// Gets or sets the type for list subgroups.  If set, allows non-SubGroupType IEnumerables to be items rather than SubGroups.
        /// </summary>
        /// <value>The type of the sub group.</value>
        public Type SubGroupType
        {
            get { return (Type)GetValue(SubGroupTypeProperty); }
            set { SetValue(SubGroupTypeProperty, value); }
        }

        internal static readonly BindableProperty IsScrollListeningProperty = BindableProperty.Create("IsScrollListening", typeof(bool), typeof(ListView), default(bool));
        internal bool IsScrollListening
        {
            get { return (bool)GetValue(IsScrollListeningProperty); }
            set { SetValue(IsScrollListeningProperty, value); }
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

        /// <summary>
        /// Occurs when ItemsSource setting has completed.
        /// </summary>
        public event EventHandler ItemsSourceSet;


        /// <summary>
        /// Taps the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void TapItem(object item)
        {
            var itemWrapper = BaseItemsSource.WrapperForSource(item);
            var args = new ItemWrapperTapEventArgs(itemWrapper);
            OnItemTapped(this, args);
        }

        ItemWrapper _selectedF9PItem;
        List<ItemWrapper> _selectedF9PItems = new List<ItemWrapper>();
        bool _processingItemTapped;
        //void OnItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        void OnItemTapped(object sender, ItemWrapperTapEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ITEM TAPPED");
            if (!_processingItemTapped)
            {
                _processingItemTapped = true;
                base.SelectedItem = null;

                //var tappedItem = e.Item as ItemWrapper;
                var tappedItem = e.ItemWrapper;
                //var group = e.Group as GroupWrapper ?? _baseItemsSource;
                var group = tappedItem.Parent ?? _baseItemsSource;

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
            var item = e?.Item as ItemWrapper;
            var source = item?.Source;
            if (source != null)
                ItemAppearing?.Invoke(this, new ItemVisibilityEventArgs(source));
        }

        void OnItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnItemDisappearing");
            var source = ((ItemWrapper)e?.Item)?.Source;
            if (source != null)
                ItemDisappearing?.Invoke(this, new ItemVisibilityEventArgs(source));
            if (source == SelectedItem)
                base.SelectedItem = null;
        }


        #endregion

        #region Gestures
        /// <summary>
        /// Occurs when the panning has completed.
        /// </summary>
        public event EventHandler Panning;
        /// <summary>
        /// Occurs when the listview is panned.
        /// </summary>
        public event EventHandler Panned;
        #endregion


        #region Scroll
        /// <summary>
        /// Occurs when scroll is started and (depending on the platform) may continue until the scroll is completed.
        /// </summary>
        internal event EventHandler Scrolling;
        /// <summary>
        /// Occurs when scroll is completed.
        /// </summary>
        internal event EventHandler Scrolled;
        #endregion

        #endregion


        #region Constructor
        static int Count;
        int id;
        Listener _listener;

        readonly ModalPopup _popup;

        void init()
        {
            //this.DisableSelection();
            //Margin = new Thickness(5, 0, 5, 0);

            id = Count++;
            HasUnevenRows = false;
            BackgroundColor = Color.Transparent;

            //base.SeparatorColor = Color.White;
            //base.SeparatorVisibility = SeparatorVisibility.None;

            base.ItemAppearing += OnItemAppearing;
            base.ItemDisappearing += OnItemDisappearing;
            //base.ItemTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("ListView base.ItemTapped");

            IsEnabled = true;
            _listener = FormsGestures.Listener.For(this);
            //_listener.LongPressed += OnLongPressed;
            //_listener.LongPressing += OnLongPressing;
            _listener.Panning += OnPanning;
            _listener.Panned += OnPanned;

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
        public ListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            _popup = new ModalPopup()
            {
                Padding = 3,
                HasShadow = true,
                OutlineRadius = 4
            };

            init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
        /// </summary>
        public ListView()
        {
            _popup = new ModalPopup()
            {
                Padding = 3,
                HasShadow = true,
                OutlineRadius = 4
            };

            init();
        }

        /// <summary>
        /// Description this instance.
        /// </summary>
        public string Description()
        {
            return "ListView[" + id + "]";
        }
        #endregion


        #region SelectItems management
        // Assumption: SelectedItem(s) must be set AFTER ItemsSource and ItemsSourceMap has been set.  Otherwise, selected items will be culled

        bool _internalAddRemove;

        void AddSelection(ItemWrapper f9pItem, object sourceItem)
        {
            _internalAddRemove = true;
            if (f9pItem == null || sourceItem == null)
                throw new InvalidOperationException("Cannot select null item");
            f9pItem.IsSelected = true;
            if (!_selectedF9PItems.Contains(f9pItem))
                _selectedF9PItems.Add(f9pItem);
            _selectedF9PItem = f9pItem;
            //if (sourceItem != SelectedItem)
            SelectedItem = sourceItem;
            if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && !SelectedItems.Contains(sourceItem))
                SelectedItems.Add(sourceItem);
            _internalAddRemove = false;
        }

        void AddSelectedItem(ItemWrapper f9pItem)
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
            var f9pItem = _baseItemsSource.WrapperForSource(sourceItem);
            AddSelection(f9pItem, sourceItem);
        }

        void RemoveSelection(ItemWrapper f9pItem, object sourceItem)
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
            if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && sourceItem != null && SelectedItems.Contains(sourceItem))
                SelectedItems.Remove(sourceItem);
            _internalAddRemove = false;
        }

        void RemoveSelectedItem(ItemWrapper f9pItem)
        {
            if (f9pItem == null)
                return;
            var sourceItem = f9pItem.Source;
            RemoveSelection(f9pItem, sourceItem);
        }

        void RemoveSelectedSourceItem(object sourceItem)
        {
            if (sourceItem == null)
                return;
            var f9pItem = _baseItemsSource.WrapperForSource(sourceItem);
            RemoveSelection(f9pItem, sourceItem);
        }

        void AddSelectedSourceItems(IList sourceItems)
        {
            if (sourceItems == null)
                return;
            foreach (var sourceItem in sourceItems)
                AddSelectedSourceItem(sourceItem);
        }

        void RemoveSelectedSourceItems(IList sourceItems)
        {
            if (sourceItems == null)
                return;
            for (int i = sourceItems.Count - 1; i >= 0; i--)
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
                    var item = _baseItemsSource.WrapperForSource(sourceItem);
                    if (item == null)
                        RemoveSelectedSourceItem(sourceItem);
                    else
                        AddSelectedSourceItem(sourceItem);
                }
            }
            else if (GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
                var item = _baseItemsSource.WrapperForSource(SelectedItem);
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
            if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
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
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == F9PItemsSourceProperty.PropertyName)
            {
                //System.Diagnostics.Debug.WriteLine("ListView.OnPropertyChanging(ItemsSource)");
                UpdateItemsSource();
                return;
            }
            else if (propertyName == SourcePropertyMapProperty.PropertyName)
            {
                UpdateItemsSource();
                return;
            }
            base.OnPropertyChanged(propertyName);
            /*
			if (propertyName == SeparatorColorProperty.PropertyName
				|| propertyName == SeparatorVisibilityProperty.PropertyName
				|| propertyName == CellBackgroundColorProperty.PropertyName
				|| propertyName == SelectedCellBackgroundColorProperty.PropertyName
			   )
				UpdateCellProperties();

			else*/
            if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
                AddSelectedSourceItem(SelectedItem);
            else if (BaseItemsSource != null)
            {
                if (propertyName == VisibilityTestProperty.PropertyName)
                    BaseItemsSource.VisibilityTest = VisibilityTest;
                //else if (propertyName == HasUnevenRowsProperty.PropertyName)
                //	BaseItemsSource.HasUnevenRows = HasUnevenRows;
                else if (propertyName == RowHeightProperty.PropertyName)
                    BaseItemsSource.RowHeight = RowHeight;
                else if (propertyName == CellBackgroundColorProperty.PropertyName)
                    BaseItemsSource.CellBackgroundColor = CellBackgroundColor;
                else if (propertyName == SelectedCellBackgroundColorProperty.PropertyName)
                    BaseItemsSource.SelectedCellBackgroundColor = SelectedCellBackgroundColor;
            }
            /*
			else if (propertyName == SelectedItemsProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
			{
				AddSelectedSourceItems(SelectedItems);
				SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
			}
			*/
        }

        object _updateItemsSourceBlock = new object();

        void UpdateItemsSource()
        {
            //Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            //{
            if (F9PItemsSource == null)
                return;
            //Task.Run(() =>  // creates a race condition on picker view (Heights and Areas, Advanced, new OccupancyArea, select Occupancy
            //{
            if (_baseItemsSource != null)
            {
                _baseItemsSource.Tapped -= OnItemTapped;
                _baseItemsSource.SwipeMenuItemTapped -= OnSwipeMenuItemTapped;
                _baseItemsSource.LongPressed -= OnLongPressed;
                _baseItemsSource.LongPressing -= OnLongPressing;
            }
            //System.Diagnostics.Debug.WriteLine("UpdateItemsSource");
            //_baseItemsSource = new Group(ItemsSource, SourcePropertyMap);
            _baseItemsSource = new GroupWrapper();

            _baseItemsSource.Tapped += OnItemTapped;
            _baseItemsSource.LongPressed += OnLongPressed;
            _baseItemsSource.LongPressing += OnLongPressing;
            _baseItemsSource.SwipeMenuItemTapped += OnSwipeMenuItemTapped;

            _baseItemsSource.BindingContext = this;
            _baseItemsSource.SourceSubPropertyMap = SourcePropertyMap;
            _baseItemsSource.SubGroupType = SubGroupType;
            _baseItemsSource.VisibilityTest = VisibilityTest;
            //_baseItemsSource.HasUnevenRows = HasUnevenRows;
            _baseItemsSource.RowHeight = RowHeight;
            _baseItemsSource.CellBackgroundColor = CellBackgroundColor;
            _baseItemsSource.SelectedCellBackgroundColor = SelectedCellBackgroundColor;

            _baseItemsSource.Source = F9PItemsSource;
            Device.BeginInvokeOnMainThread(() =>
            {
                base.ItemsSource = _baseItemsSource;

                IsGroupingEnabled = _baseItemsSource.ContentType == GroupWrapper.GroupContentType.Lists;
                ReevaluateSelectedItems();
                ItemsSourceSet?.Invoke(this, EventArgs.Empty);
            });
            //});
            //return false;
            //});
        }
        #endregion


        #region Swipe Menu events
        /// <summary>
        /// Occurs when a swipe menu item has been tapped.
        /// </summary>
        public event EventHandler<SwipeMenuItemTappedArgs> SwipeMenuItemTapped;
        /// <summary>
        /// Called when a swipe menu items has been tapped
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnSwipeMenuItemTapped(object sender, SwipeMenuItemTappedArgs e)
        {
            SwipeMenuItemTapped?.Invoke(sender, e);
        }
        #endregion


        #region Item change management
        /// <summary>
        /// Occurs when a property of a ListViewItem is about to change.
        /// </summary>
        public event PropertyChangingEventHandler ItemPropertyChanging;
        void OnItemPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnItemPropertyChanging");
            ItemPropertyChanging?.Invoke(sender, e);
        }

        /// <summary>
        /// Occurs when property of a ListViewItem has changed.
        /// </summary>
        public event PropertyChangedEventHandler ItemPropertyChanged;
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnItemPropertyChanged");
            ItemPropertyChanged?.Invoke(sender, e);
        }
        #endregion


        #region DragDrop support
        internal static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create("ScrollEnabled", typeof(bool), typeof(ListView), true);
        internal bool ScrollEnabled
        {
            get { return (bool)GetValue(ScrollEnabledProperty); }
            set { SetValue(ScrollEnabledProperty, value); }
        }


        internal Func<Rectangle, DragEventArgs> RendererFindItemDataUnderRectangle;
        internal DragEventArgs FindItemDataUnderRectangle(Rectangle rect)
        {
            return RendererFindItemDataUnderRectangle != null ? RendererFindItemDataUnderRectangle(rect) : null;
        }


        internal Func<double, bool> RendererScrollBy;
        internal Action<object, object, ScrollToPosition, bool> RendererScrollToPos;


        double _scrollSpeed;
        bool _scrolling;
        void ScrollSpeed(double speed)
        {
            if (!_scrolling && Math.Abs(_scrollSpeed) > 0)
            {
                _scrolling = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
                {
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
        public bool ScrollBy(double delta)
        {
            return RendererScrollBy(delta);
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
            var itemGroup = _baseItemsSource.WrapperForSource(group) as GroupWrapper;
            if (itemGroup != null)
            {
                var itemWrapper = itemGroup.WrapperForSource(item);
                if (itemWrapper == null)
                {
                    // this will happen when item is inside of a group that is not of SubGroupType type 
                    ItemWrapperForSourceItem(item, group as IEnumerable, out itemWrapper);
                }
                Device.StartTimer(TimeSpan.FromMilliseconds(150), () =>
                {
                    //if (Device.OS == TargetPlatform.Android)
                    //{
                    if (IsGroupingEnabled)
                        RendererScrollToPos?.Invoke(itemWrapper, group, position, animated);
                    else
                        RendererScrollToPos?.Invoke(itemWrapper, null, position, animated);
                    /*
					}
					else 
					{ 
						if (IsGroupingEnabled) 
							base.ScrollTo(itemWrapper, itemGroup, position, animated); 
						else 
							base.ScrollTo(itemWrapper, position, animated); 
					}
					*/
                    return false;
                });
            }
        }

        int _scrollToInvocations;

        /// <summary>
        /// Scrolls to.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="position">Position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public new void ScrollTo(object item, ScrollToPosition position, bool animated)
        {
            _scrollToInvocations++;
            var itemWrapper = _baseItemsSource.WrapperForSource(item);
            if (itemWrapper == null)
                // this will happen when item is inside of a group that is not of SubGroupType type 
                ItemWrapperForSourceItem(item, F9PItemsSource, out itemWrapper);
            if (itemWrapper == null)
            {
                itemWrapper = _baseItemsSource.WrapperForSource(item);
                if (itemWrapper == null)
                    ItemWrapperForSourceItem(item, F9PItemsSource, out itemWrapper);
                //throw new InvalidDataContractException("Should not have any cell that doesn't have an items source!");
            }
            //else
            //{
            // required because of race condition: Xamarin.Forms ListView doesn't scroll to index if it's still digesting a new ItemsSource?
            System.Diagnostics.Debug.WriteLine("A invocation=[" + _scrollToInvocations + "] itemWrapper.ID=[" + itemWrapper.ID + "]  _baseItemsSource.ID=[" + _baseItemsSource.ID + "]");

            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                //if (Device.OS == TargetPlatform.Android)
                //{
                System.Diagnostics.Debug.WriteLine("B invocation=[" + _scrollToInvocations + "] itemWrapper.ID=[" + itemWrapper.ID + "]  _baseItemsSource.ID=[" + _baseItemsSource.ID + "]");
                if (IsGroupingEnabled)
                    RendererScrollToPos?.Invoke(itemWrapper, BaseItemsSource, position, animated);
                else
                    RendererScrollToPos?.Invoke(itemWrapper, null, position, animated);
                /*
                    }
                    //RendererScrollToItemInGroup(item, group, position, animated);
                    else
                    {
                        base.ScrollTo(itemWrapper, position, animated);
                    }
                    */
                return false;
            });
            //}
        }


        bool ItemWrapperForSourceItem(object sourceItem, IEnumerable sourceGroup, out ItemWrapper itemWrapper)
        {
            itemWrapper = null;
            foreach (var item in sourceGroup)
            {
                if (item == sourceItem)
                {
                    itemWrapper = BaseItemsSource.WrapperForSource(item);
                    // we are in this function because item isn't in BaseItemsSource so we will now return so the recursion can find the item's parent that is in BaseItemsSource
                    return true;
                }
                var enumerable = item as IEnumerable;
                if (enumerable != null && ItemWrapperForSourceItem(sourceItem, enumerable, out itemWrapper))
                {
                    // source is a child of this object!
                    itemWrapper = itemWrapper ?? BaseItemsSource.WrapperForSource(enumerable);
                    return true;
                }
            }
            return false;
        }


        #endregion




        #region Drag/Drop
        /// <summary>
        /// Delegate function that will be called to query if a item (at a deep index location) can be dragged
        /// </summary>
        public Func<ListView, object, int[], bool> CanDrag;
        /// <summary>
        /// Delegate function that will be called to query if a item being dragged can be dropped at location specified by a deep index.
        /// </summary>
        public Func<ListView, object, int[], bool> CanDrop;

        DragEventArgs _longPress;
        // TODO:  MAJOR TODO !!! NEED TO REFACTOR _longPress TO WORK WITH BETTER INFORMATION PROVIDED BY ItemWrapperLongPressEventArgs

        readonly NullItemWrapper _nullItem = new NullItemWrapper();
        readonly BlankItemWrapper _blankItem = new BlankItemWrapper();

        Rectangle _nativeFrame;

        //void OnLongPressing(object sender, LongPressEventArgs e) {
        void OnLongPressing(object sender, ItemWrapperLongPressEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ListView.OnLongPressing");
            if (!Editable)
                return;
            // will be called when the _listener (attached to this) detects a long press
            if (_longPress != null)
                return;

            // we need to know what item is being pressed and it's corresponding view
            //System.Diagnostics.Debug.WriteLine ("LONGPRESSING ["+e.Listener.Element+"]");

            _longPress = DependencyService.Get<IListItemLocation>().DragEventArgsForItemAtPoint(this, e.Center);
            if (_longPress != null && _longPress.Item.BaseCellView != null)
            {
                // can we drag this Item?
                bool canDrag = true;
                if (CanDrag != null)
                    canDrag = CanDrag(this, _longPress.Item.Source, _longPress.DeepIndex);
                if (!canDrag)
                {
                    _longPress = null;
                    return;
                }
                SelectedItem = null;
                _nativeFrame = _longPress.Item.BaseCellView.BoundsToWinCoord();

                // need a null item to fill the void 
                _nullItem.RequestedHeight = _nativeFrame.Height;
                _nullItem.CellBackgroundColor = BackgroundColor;

                _baseItemsSource.NotifySourceOfChanges = false;
                _baseItemsSource.DeepSwapItems(_longPress.Item, _nullItem);

                //_longPress.Item.SeparatorIsVisible = false;
                var contentView = ItemTemplates.MakeContentView(_longPress.Item);
                contentView.WidthRequest = _nativeFrame.Width;
                contentView.HeightRequest = _nativeFrame.Height;
                contentView.BackgroundColor = Color.Transparent;
                //_popup.BackgroundColor = contentView.BackgroundColor == Color.Transparent ? Color.White : contentView.BackgroundColor;\
                _popup.BackgroundColor = CellBackgroundColor;
                _popup.Content = contentView;
                _popup.Location = _nativeFrame.Location;
                _popup.IsVisible = true;

                ScrollEnabled = false;
            }
            else
            {
                _longPress = null;
            }
        }


        void OnPanning(object sender, PanEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ListView.OnPanning");
            Panning?.Invoke(this, EventArgs.Empty);
            if (_longPress != null)
            {
                //_longPressPan = true;
                //System.Diagnostics.Debug.WriteLine("PAN ["+e.Listener.Element+"]");
                System.Diagnostics.Debug.WriteLine("LONGPRESS PANNING");
                _popup.TranslationX = e.TotalDistance.X;
                _popup.TranslationY = e.TotalDistance.Y;

                Point currentOrigin = _nativeFrame.Location + (Size)e.TotalDistance;
                var cellRect = new Rectangle(currentOrigin, _nativeFrame.Size);
                var currentDragOver = FindItemDataUnderRectangle(cellRect);
                if (currentDragOver != null && currentDragOver.Item != _nullItem && currentDragOver.Item != null)
                {
                    //System.Diagnostics.Debug.WriteLine ("current=[{0}]", currentDragOver.Item.Title);
                    // can we drop here?
                    bool canDrop = true;
                    if (CanDrop != null)
                        canDrop = CanDrop(this, currentDragOver.Item.Source, currentDragOver.DeepIndex);
                    if (canDrop)
                    {
                        // yes: put the NullItem here
                        _baseItemsSource.DeepRemove(_nullItem);
                        _baseItemsSource.DeepInsert(currentDragOver.DeepIndex, _nullItem);
                    }
                    else if (_baseItemsSource.DeepContains(_nullItem) && !_baseItemsSource.DeepIndexOf(_nullItem).SequenceEqual(_longPress.DeepIndex))
                    {
                        // no: put the NullItem at the location where we started 
                        _baseItemsSource.DeepRemove(_nullItem);
                        _baseItemsSource.DeepInsert(_longPress.DeepIndex, _nullItem);
                    }
                }

                if (cellRect.Top < 40 && e.DeltaDistance.Y <= 2)
                    ScrollSpeed(cellRect.Top - 40);
                else if (cellRect.Bottom + 40 > Height && e.DeltaDistance.Y >= -2)
                    ScrollSpeed(cellRect.Bottom + 40 - Height);
                else
                    ScrollSpeed(0);
            }
        }

        void OnPanned(object sender, PanEventArgs e)
        {
            Panned?.Invoke(this, EventArgs.Empty);
        }

        internal void OnScrolling(object sender, EventArgs e)
        {
            Scrolling?.Invoke(this, EventArgs.Empty);
        }

        internal void OnScrolled(object sender, EventArgs e)
        {
            Scrolled?.Invoke(this, EventArgs.Empty);
        }

        //void OnLongPressed(object sender, LongPressEventArgs e) {
        void OnLongPressed(object sender, ItemWrapperLongPressEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine ("ListView.LONGPRESSED ["+e.Listener.Element+"]");
            //System.Diagnostics.Debug.WriteLine("ListView.LONGPRESSED [" + e.ItemWrapper.Source + "]");

            if (_longPress != null)
            {
                ScrollSpeed(0);
                ScrollEnabled = true;

                // next two lines are for Android - without them, if you try to drag the same cell twice, the ModalPopup content is blank.
                var blankView = ItemTemplates.MakeContentView(_blankItem);
                _popup.Content = blankView;

                _popup.IsVisible = false;

                // return to our pre-drag state
                var nullIndex = _baseItemsSource.DeepIndexOf(_nullItem);
                _baseItemsSource.DeepRemove(_nullItem);
                _baseItemsSource.DeepInsert(_longPress.DeepIndex, _longPress.Item);
                _baseItemsSource.NotifySourceOfChanges = true;
                if (!nullIndex.SequenceEqual(_longPress.DeepIndex))
                {
                    // we made a allowed move, so make that move
                    _baseItemsSource.DeepRemove(_longPress.Item);
                    _baseItemsSource.DeepInsert(nullIndex, _longPress.Item);
                }
                _longPress = null;
                _popup.Content = null;
            }

        }
        #endregion
    }
}

