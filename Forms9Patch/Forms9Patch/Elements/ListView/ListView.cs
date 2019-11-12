using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace Forms9Patch
{
    /// <summary>
    /// FormsDragNDropListView List view.
    /// </summary>
    [DesignTimeVisible(true)]
    public class ListView : Forms9Patch.Frame, IElement //  Forms9Patch.ManualLayout, IElement
    {
        #region Properties

        #region Obsolete properties
        /// <summary>
        /// There is nothing to see here.  Move on.
        /// </summary>
        [Obsolete("Invalid property", true)]
        public static new readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ListView), default(View));
        /// <summary>
        /// There is nothing to see here.  Move on.
        /// </summary>
        /// <value>The content.</value>
        [Obsolete("Invalid property", true)]
        public new View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region Forms9Patch Cell Decoration properties

        #region GroupHeaderRowHeight
        /// <summary>
        /// backing store for GroupHeaderRowHeight property
        /// </summary>
        public static readonly BindableProperty GroupHeaderRowHeightProperty = GroupWrapper.RequestedGroupHeaderRowHeightProperty;
        /// <summary>
        /// Gets/Sets the GroupHeaderRowHeight property
        /// </summary>
        public double GroupHeaderRowHeight
        {
            get => (double)GetValue(GroupHeaderRowHeightProperty);
            set => SetValue(GroupHeaderRowHeightProperty, value);
        }
        #endregion GroupHeaderRowHeight property

        #region BackgroundColor properties

        #region SelectedCellBackgroundColor
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
            get => (Color)GetValue(SelectedCellBackgroundColorProperty);
            set => SetValue(SelectedCellBackgroundColorProperty, value);
        }
        #endregion SelectedCellBackgroundColor property

        #region GroupHeaderBackgroundColor
        /// <summary>
        /// The group header background color property backing store
        /// </summary>
        public static readonly BindableProperty GroupHeaderBackgroundColorProperty = GroupWrapper.GroupHeaderBackgroundColorProperty;
        /// <summary>
        /// Gets or sets the color of the group header background.
        /// </summary>
        /// <value>The color of the group header background.</value>
        public Color GroupHeaderBackgroundColor
        {
            get => (Color)GetValue(GroupHeaderBackgroundColorProperty);
            set => SetValue(GroupHeaderBackgroundColorProperty, value);
        }
        #endregion GroupHeaderBackgroundColor property

        #endregion

        #region Separator properties

        #region SeparatorVisibility property
        /// <summary>
        /// backing store for SeparatorVisibility property
        /// </summary>
        public static readonly BindableProperty SeparatorVisibilityProperty = ItemWrapper.SeparatorVisibilityProperty;
        /// <summary>
        /// Gets/Sets the SeparatorVisibility property
        /// </summary>
        public Xamarin.Forms.SeparatorVisibility SeparatorVisibility
        {
            get => (Xamarin.Forms.SeparatorVisibility)GetValue(SeparatorVisibilityProperty);
            set => SetValue(SeparatorVisibilityProperty, value);
        }
        #endregion SeparatorVisibility property

        #region SeparatorColor property
        /// <summary>
        /// backing store for SeparatorColor property
        /// </summary>
        public static readonly BindableProperty SeparatorColorProperty = ItemWrapper.SeparatorColorProperty;
        /// <summary>
        /// Gets/Sets the SeparatorColor property
        /// </summary>
        public Color SeparatorColor
        {
            get => (Color)GetValue(SeparatorColorProperty);
            set => SetValue(SeparatorColorProperty, value);
        }
        #endregion SeparatorColor property

        #region SeparatorLeftIndent property
        /// <summary>
        /// backing store for SeparatorLeftIndent property
        /// </summary>
        public static readonly BindableProperty SeparatorLeftIndentProperty = ItemWrapper.SeparatorLeftIndentProperty;
        /// <summary>
        /// Gets/Sets the SeparatorLeftIndent property
        /// </summary>
        public double SeparatorLeftIndent
        {
            get => (double)GetValue(SeparatorLeftIndentProperty);
            set => SetValue(SeparatorLeftIndentProperty, value);
        }
        #endregion SeparatorLeftIndent property

        #region SeparatorRightIndent property
        /// <summary>
        /// backing store for SeparatorRightIndent property
        /// </summary>
        public static readonly BindableProperty SeparatorRightIndentProperty = ItemWrapper.SeparatorRightIndentProperty;
        /// <summary>
        /// Gets/Sets the SeparatorRightIndent property
        /// </summary>
        public double SeparatorRightIndent
        {
            get => (double)GetValue(SeparatorRightIndentProperty);
            set => SetValue(SeparatorRightIndentProperty, value);
        }
        #endregion SeparatorRightIndent property

        #region SeparatorHeight property
        /// <summary>
        /// backing store for SeparatorHeight property
        /// </summary>
        public static readonly BindableProperty SeparatorHeightProperty = ItemWrapper.RequestedSeparatorHeightProperty;
        /// <summary>
        /// Gets/Sets the SeparatorHeight property
        /// </summary>
        public double SeparatorHeight
        {
            get => (double)GetValue(SeparatorHeightProperty);
            set => SetValue(SeparatorHeightProperty, value);
        }
        #endregion SeparatorHeight property

        #endregion


        #endregion

        #region Data mapping and filtering properties

        #region SourcePropertyMap
        /// <summary>
        /// The source property map property.
        /// </summary>
        public static readonly BindableProperty SourcePropertyMapProperty = BindableProperty.Create(nameof(SourcePropertyMap), typeof(List<string>), typeof(ListView), default(List<string>));
        /// <summary>
        /// Gets or sets the source property map.  Used to map the properties in a hierarchical ItemsSource used to make the hierarchy and bind (as items) to the CellViews
        /// </summary>
        /// <value>The source property map.</value>
        public List<string> SourcePropertyMap
        {
            get => (List<string>)GetValue(SourcePropertyMapProperty);
            set => SetValue(SourcePropertyMapProperty, value);
        }
        #endregion SourcePropertyMap

        #region VisibilityTest
        /// <summary>
        /// The cell visibility test property.
        /// </summary>
        public static readonly BindableProperty VisibilityTestProperty = BindableProperty.Create(nameof(VisibilityTest), typeof(Func<object, bool>), typeof(ListView), null);
        /// <summary>
        /// Gets or sets the cell visibility test.
        /// </summary>
        /// <value>The visibility test.</value>
        public Func<object, bool> VisibilityTest
        {
            get => (Func<object, bool>)GetValue(VisibilityTestProperty);
            set => SetValue(VisibilityTestProperty, value);
        }
        #endregion VisiblilityTest

        #region SubGroupType
        /// <summary>
        /// The sub group type property backing store.
        /// </summary>
        public static readonly BindableProperty SubGroupTypeProperty = BindableProperty.Create(nameof(SubGroupType), typeof(Type), typeof(ListView), null);
        /// <summary>
        /// Gets or sets the type for list subgroups.  If set, allows non-SubGroupType IEnumerables to be items rather than SubGroups.
        /// </summary>
        /// <value>The type of the sub group.</value>
        public Type SubGroupType
        {
            get => (Type)GetValue(SubGroupTypeProperty);
            set => SetValue(SubGroupTypeProperty, value);
        }
        #endregion

        #endregion

        #region Row Selection properties

        #region GroupToggleBehavior
        /// <summary>
        /// The backing store for the ListViews's GroupToggleBehavior property.
        /// </summary>
        public static readonly BindableProperty GroupToggleBehaviorProperty = BindableProperty.Create(nameof(GroupToggleBehavior), typeof(GroupToggleBehavior), typeof(ListView), GroupToggleBehavior.Radio);
        /// <summary>
        /// Gets or sets the ListViews's GroupToggle behavior.
        /// </summary>
        /// <value>The Toggle behavior (None, Radio, Multiselect).</value>
        public GroupToggleBehavior GroupToggleBehavior
        {
            get => (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty);
            set => SetValue(GroupToggleBehaviorProperty, value);
        }
        #endregion GroupToggleBehavior property

        #region SelectedItems
        /// <summary>
        /// The selected items property.
        /// </summary>
        public static readonly BindablePropertyKey SelectedItemsPropertyKey = BindableProperty.CreateReadOnly(nameof(SelectedItems), typeof(ObservableCollection<object>), typeof(ListView), null);
        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public ObservableCollection<object> SelectedItems
        {
            get => (ObservableCollection<object>)GetValue(SelectedItemsPropertyKey.BindableProperty);
            private set => SetValue(SelectedItemsPropertyKey, value);
        }
        #endregion SelectdItems property

        #endregion row selection properties

        #region Editable
        /// <summary>
        /// The editable property backing store.
        /// </summary>
        public static readonly BindableProperty EditableProperty = BindableProperty.Create(nameof(Editable), typeof(bool), typeof(ListView), false);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ListView"/> is editable - cells may be moved or deleted based upon the response from the CanDrag CanDrop CanDelete delegate methods.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable
        {
            get => (bool)GetValue(EditableProperty);
            set => SetValue(EditableProperty, value);
        }
        #endregion Editable

        #region Xamarin.Forms.ListView analogs

        #region Header / Footer properties

        #region Header
        /// <summary>
        /// backing store for Header property
        /// </summary>
        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(object), typeof(ListView), default);
        /// <summary>
        /// Gets/Sets the Header property
        /// </summary>
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        #endregion Header

        #region HeaderTemplate
        /// <summary>
        /// backing store for HeaderTemplate property
        /// </summary>
        public static readonly BindableProperty HeaderTemplateProperty = BindableProperty.Create(nameof(HeaderTemplate), typeof(Xamarin.Forms.DataTemplate), typeof(ListView), default(Xamarin.Forms.DataTemplate));
        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>The header template.</value>
        public Xamarin.Forms.DataTemplate HeaderTemplate
        {
            get => (Xamarin.Forms.DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }
        #endregion HeaderTemplate 

        #region Footer
        /// <summary>
        /// backing store for Footer property
        /// </summary>
        public static readonly BindableProperty FooterProperty = BindableProperty.Create(nameof(Footer), typeof(object), typeof(ListView), default);
        /// <summary>
        /// Gets/Sets the Footer property
        /// </summary>
        public object Footer
        {
            get => GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }
        #endregion Footer

        #region FooterTemplate
        /// <summary>
        /// backing store for FooterTemplate property
        /// </summary>
        public static readonly BindableProperty FooterTemplateProperty = BindableProperty.Create(nameof(FooterTemplate), typeof(DataTemplate), typeof(ListView), default(DataTemplate));
        /// <summary>
        /// Gets/Sets the FooterTemplate property
        /// </summary>
        public DataTemplate FooterTemplate
        {
            get => (DataTemplate)GetValue(FooterTemplateProperty);
            set => SetValue(FooterTemplateProperty, value);
        }
        #endregion FooterTemplate

        #endregion Header / Footer properties

        #region SelectedItem
        /// <summary>
        /// backing store for SelectedItem property
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ListView), default);
        /// <summary>
        /// Gets/Sets the SelectedItem property
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        #endregion SelectedItem

        #region Row Height properties

        // HasUnevenRows not supported because Forms9Patch.ListView assumes all lists have uneven rows

        #region RowHeight
        /// <summary>
        /// backing store for RowHeight property
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(ListView), 40);
        /// <summary>
        /// Gets/Sets the RowHeight property
        /// </summary>
        public int RowHeight
        {
            get => (int)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }
        #endregion RowHeight

        #endregion RowHeight properties

        #region Group behavior properties

        #region GroupHeaderTemplate
        /// <summary>
        /// backing store for GroupHeaderTemplate property
        /// </summary>
        public static readonly BindableProperty GroupHeaderTemplateProperty = BindableProperty.Create(nameof(GroupHeaderTemplate), typeof(Forms9Patch.GroupHeaderTemplate), typeof(ListView), default(Forms9Patch.GroupHeaderTemplate));
        /// <summary>
        /// Gets/Sets the GroupHeaderTemplate property
        /// </summary>
        public Forms9Patch.GroupHeaderTemplate GroupHeaderTemplate
        {
            get => (Forms9Patch.GroupHeaderTemplate)GetValue(GroupHeaderTemplateProperty);
            set => SetValue(GroupHeaderTemplateProperty, value);
        }
        #endregion GroupHeaderTemplate

        #region IsGroupingEnabled
        /// <summary>
        /// backing store for IsGroupingEnabled property
        /// </summary>
        public static readonly BindableProperty IsGroupingEnabledProperty = BindableProperty.Create(nameof(IsGroupingEnabled), typeof(bool), typeof(ListView), default(bool));
        /// <summary>
        /// Gets/Sets the IsGroupingEnabled property
        /// </summary>
        public bool IsGroupingEnabled
        {
            get => (bool)GetValue(IsGroupingEnabledProperty);
            set => SetValue(IsGroupingEnabledProperty, value);
        }
        #endregion IsGroupingEnabled

        #endregion

        #region Xamarin.Forms.ItemsView analogs

        #region ItemsSource
        /// <summary>
        /// backing store for ItemsSource property
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ListView), default(IEnumerable));
        /// <summary>
        /// Gets/Sets the ItemsSource property
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        #endregion ItemsSource

        #region ItemTemplates
        /// <summary>
        /// backing store for ItemTemplates property
        /// </summary>
        public static readonly BindableProperty ItemTemplatesProperty = BindableProperty.Create(nameof(ItemTemplates), typeof(Forms9Patch.DataTemplateSelector), typeof(ListView), default(Forms9Patch.DataTemplateSelector));
        /// <summary>
        /// Gets/Sets the ItemTemplates property
        /// </summary>
        public Forms9Patch.DataTemplateSelector ItemTemplates
        {
            get => (Forms9Patch.DataTemplateSelector)GetValue(ItemTemplatesProperty);
            set => SetValue(ItemTemplatesProperty, value);
        }

        #endregion ItemTemplates

        #endregion Xamarin.Forms.ItemsView analogs

        #endregion Xamarin.Forms.ListView analogs


        #endregion


        #region Events

        #region Selection Events
        /// <summary>
        /// Occurs when cell is selected.
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        /// <summary>
        /// Occurs when cell is tapped.
        /// </summary>
        public event EventHandler<ItemTappedEventArgs> ItemTapped;

        /// <summary>
        /// Occurs when ItemsSource setting has completed.
        /// </summary>
        public event EventHandler ItemsSourceSet;

        /// <summary>
        /// Occures when the ListView has appeared
        /// </summary>
        public event EventHandler Appeared;
        #endregion

        #region VisibilityEvents
        /// <summary>
        /// Occurs when item is appearing.
        /// </summary>
        public event EventHandler<ItemVisibilityEventArgs> ItemAppearing;

        /// <summary>
        /// Occurs when item is disappearing.
        /// </summary>
        public event EventHandler<ItemVisibilityEventArgs> ItemDisappearing;
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


        #region Private Properties
        /*
        ModalPopup _popup;
        ModalPopup Popup
        {
            get
            {
                _popup = _popup ?? new ModalPopup
                {
                    Padding = 3,
                    HasShadow = true,
                    OutlineRadius = 4
                };
                return _popup;
            }
        }
        */

        internal GroupWrapper BaseItemsSource { get; set; }
        #endregion


        #region Fields

        readonly EnhancedListView _listView;
        //readonly Listener _listener;
        bool _resetScrollToSelected;
        DateTime _scrollResetAt;
        DateTime _scrollCompletedAt = DateTime.MinValue;
        #endregion


        #region Constructor / Disposer
        static ListView()
        {
            Settings.ConfirmInitialization();
        }

        void Init()
        {
            P42.Utils.Debug.AddToCensus(this);

            Padding = 0;
            Margin = 0;

            //Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ListView.SetIsFastScrollEnabled(_listView, true);
            _listView.HasUnevenRows = true;
            _listView.ItemAppearing += OnItemAppearing;
            _listView.ItemDisappearing += OnItemDisappearing;
            _listView.SeparatorVisibility = Xamarin.Forms.SeparatorVisibility.None;


            _listView.Scrolling += OnScrolling;
            _listView.Scrolled += OnScrolled;


            SelectedItems = new ObservableCollection<object>();
            SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;

            ItemTemplates = new DataTemplateSelector();

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            IgnoreChildren = false;

            //Children.Add(_listView);
            base.Content = _listView;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
        /// </summary>
        public ListView()
        {
            _listView = new EnhancedListView();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
        /// </summary>
        /// <param name="cachingStrategy">Caching strategy.</param>
        public ListView(ListViewCachingStrategy cachingStrategy)
        {
            _listView = new EnhancedListView(cachingStrategy);
            Init();
        }


        private bool _disposed;

        /// <summary>
        /// Dispose this ListView and its children
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    ItemSelected = null;
                    ItemTapped = null;
                    ItemsSourceSet = null;
                    Appeared = null;
                    ItemAppearing = null;
                    ItemDisappearing = null;
                    Scrolling = null;
                    Scrolled = null;

                    SwipeMenuItemTapped = null;
                    ItemPropertyChanging = null;
                    ItemPropertyChanged = null;

                    _listView.ItemAppearing -= OnItemAppearing;
                    _listView.ItemDisappearing -= OnItemDisappearing;

                    _listView.Scrolling -= OnScrolling;
                    _listView.Scrolled -= OnScrolled;

                    SelectedItems.CollectionChanged -= SelectedItemsCollectionChanged;
                    SelectedItems = null;

                    ItemsSource = null; // this removes the group wrapper events;
                    //ItemTemplates = null;  causes crash and is not necessary
                    VisibilityTest = null;

                    _listView.Dispose();

                    P42.Utils.Debug.RemoveFromCensus(this);
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }


        #endregion


        #region Layout
        /// <summary>
        /// Layouts the children.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            LayoutChildIntoBoundingRegion(_listView, new Rectangle(x, y, width, height));
        }
        #endregion


        #region ListView property change management
        /// <summary>
        /// Ons the property changing.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanging(propertyName));
                return;
            }

            base.OnPropertyChanging(propertyName);


            if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
                RemoveSelectedItem(SelectedItem);
            }
        }


        /// <summary>
        /// Trigged with a property has changed
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer")
            {
                _resetScrollToSelected = true;
                _scrollResetAt = DateTime.Now;
            }

            if (BaseItemsSource != null)
            {
                #region Cell decoration
                // going to use Transparent
                //if (propertyName == CellBackgroundColorProperty.PropertyName)
                //    BaseItemsSource.CellBackgroundColor = CellBackgroundColor;
                //else 
                if (propertyName == SelectedCellBackgroundColorProperty.PropertyName)
                    BaseItemsSource.SelectedCellBackgroundColor = SelectedCellBackgroundColor;
                else if (propertyName == GroupHeaderRowHeightProperty.PropertyName)
                    BaseItemsSource.RequestedGroupHeaderRowHeight = GroupHeaderRowHeight;
                else if (propertyName == RowHeightProperty.PropertyName)
                    // note that _listView.RowHeight is set in the below _listView!=null section
                    BaseItemsSource.RequestedRowHeight = RowHeight;
                #endregion

                #region Data mapping and filtering
                else if (propertyName == SourcePropertyMapProperty.PropertyName || propertyName == VisibilityTestProperty.PropertyName || propertyName == SubGroupTypeProperty.PropertyName)
                    UpdateBaseItemsSource();
                #endregion

                #region Row selection properties
                else if (propertyName == SelectedItemProperty.PropertyName && GroupToggleBehavior != GroupToggleBehavior.None)
                    AddSelectedItem(SelectedItem);
                // SelectedItems cannot be changed
                #endregion


                #region Separator Properties
                else if (propertyName == SeparatorVisibilityProperty.PropertyName)
                    BaseItemsSource.SeparatorVisibility = SeparatorVisibility;
                else if (propertyName == SeparatorLeftIndentProperty.PropertyName)
                    BaseItemsSource.SeparatorLeftIndent = SeparatorLeftIndent;
                else if (propertyName == SeparatorRightIndentProperty.PropertyName)
                    BaseItemsSource.SeparatorRightIndent = SeparatorRightIndent;
                else if (propertyName == SeparatorHeightProperty.PropertyName)
                    BaseItemsSource.RequestedSeparatorHeight = SeparatorHeight;
                else if (propertyName == SeparatorColorProperty.PropertyName)
                    BaseItemsSource.SeparatorColor = SeparatorColor;
                #endregion
            }

            // Drag/Drop properties (Editable) are managed privately 

            if (_listView != null)
            {

                #region Xamarin.Forms.ListView analogs

                #region Header / Footer properties
                if (propertyName == HeaderProperty.PropertyName)
                    _listView.Header = Header;
                else if (propertyName == HeaderTemplateProperty.PropertyName)
                    _listView.HeaderTemplate = HeaderTemplate;
                else if (propertyName == FooterProperty.PropertyName)
                    _listView.Footer = Footer;
                else if (propertyName == FooterTemplateProperty.PropertyName)
                    _listView.FooterTemplate = FooterTemplate;
                #endregion

                // SelectedItem handled above in BaseItemsSource!=null section

                #region RowHeight properties
                else if (propertyName == RowHeightProperty.PropertyName)
                    // note that BaseItemsSource.RowHeight is set in the above BaseItemsSource!=null section
                    _listView.RowHeight = RowHeight;


                // HasUnevenRows ... we are assuming this is always the case
                #endregion

                #region Group behavior properties
                else if (propertyName == GroupHeaderTemplateProperty.PropertyName)
                    _listView.GroupHeaderTemplate = GroupHeaderTemplate;
                else if (propertyName == IsGroupingEnabledProperty.PropertyName)
                    _listView.IsGroupingEnabled = IsGroupingEnabled;
                #endregion

                #region Separator properties
                // _listView.SeparatorVisibility is set to None in Init();
                //else if (propertyName == IsSeparatorVisibleProperty.PropertyName)
                //    _listView.SeparatorVisibility = IsSeparatorVisible ? Xamarin.Forms.SeparatorVisibility.Default : Xamarin.Forms.SeparatorVisibility.None;
                else if (propertyName == SeparatorColorProperty.PropertyName)
                    _listView.SeparatorColor = SeparatorColor;
                #endregion

                #region Xamarin.Forms.ItemsView analogs
                else if (propertyName == ItemsSourceProperty.PropertyName)
                    UpdateBaseItemsSource();
                else if (propertyName == ItemTemplatesProperty.PropertyName)
                    _listView.ItemTemplate = ItemTemplates;

                #endregion Xamarin.Forms.ItemsView analogs

                #endregion Xamarin.Forms.ListView analogs
            }
        }
        #endregion


        #region Gesture Manipulators
        /// <summary>
        /// Taps the item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void TapItem(object item)
        {
            var itemWrapper = BaseItemsSource.ItemWrapperForSource(item);
            var args = new ItemWrapperTapEventArgs(itemWrapper);
            OnItemTapped(this, args);
        }

        /// <summary>
        /// Simulates a tap of the item at point in this ListView.
        /// </summary>
        /// <param name="p">P.</param>
        public void TapItemAtPoint(Point p)
        {
            var dataSet = TwoDeepDataSetAtPoint(p);
            if (dataSet != null && dataSet.ItemWrapper != null)
            {
                var args = new ItemWrapperTapEventArgs(dataSet.ItemWrapper);
                OnItemTapped(this, args);
            }
        }
        #endregion


        #region Gesture Handlers

        ItemWrapper _selectedItemWrapper;
        readonly List<ItemWrapper> _selectedItemWrappers = new List<ItemWrapper>();
        bool _processingItemTapped;

        void OnItemTapped(object sender, ItemWrapperTapEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ITEM TAPPED _scrolling " + _itemPanning + " " + (DateTime.Now - _scrollCompletedAt).Milliseconds);
            if (DateTime.Now - _scrollCompletedAt < TimeSpan.FromMilliseconds(400))
                return;
            if (!_processingItemTapped)
            {
                _processingItemTapped = true;
                _listView.SelectedItem = null;

                var tappedItemWrapper = e.ItemWrapper;
                var group = tappedItemWrapper.Parent ?? (GroupWrapper)_listView.ItemsSource;

                if (tappedItemWrapper?.Source != null)
                {
                    // null source items are not tappable or selectable
                    var itemTappedArgs = new ItemTappedEventArgs(tappedItemWrapper);
                    ItemTapped?.Invoke(this, itemTappedArgs);
                    e.Handled = itemTappedArgs.Handled;
                    switch (GroupToggleBehavior)
                    {
                        case GroupToggleBehavior.None:
                            _internalAddRemove = true;
                            SelectedItems.Clear();
                            _internalAddRemove = false;
                            SelectedItem = null;
                            if (_selectedItemWrapper != null)
                                _selectedItemWrapper.IsSelected = false;
                            _selectedItemWrapper = null;
                            if (_selectedItemWrappers.Count > 0)
                            {
                                foreach (var item in _selectedItemWrappers)
                                    item.IsSelected = false;
                                _selectedItemWrappers.Clear();
                            }
                            break;
                        case GroupToggleBehavior.Radio:
                            if (tappedItemWrapper != _selectedItemWrapper)
                            {
                                RemoveSelectedItemWrapper(_selectedItemWrapper);
                                AddSelectedItemWrapper(tappedItemWrapper);
                                ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(tappedItemWrapper));
                            }
                            break;
                        case GroupToggleBehavior.Multiselect:
                            if (_selectedItemWrappers.Contains(tappedItemWrapper))
                            {
                                if (_listView.SelectedItem == tappedItemWrapper)
                                    _listView.SelectedItem = null;
                                RemoveSelectedItemWrapper(tappedItemWrapper);
                            }
                            else
                            {
                                AddSelectedItemWrapper(tappedItemWrapper);
                                ItemSelected?.Invoke(this, new SelectedItemChangedEventArgs(tappedItemWrapper));
                            }
                            break;
                    }
                }
                _processingItemTapped = false;
            }
        }

        bool _itemPanning;
        bool _itemVerticalPanning;
        void OnItemPanning(object sender, ItemWrapperPanEventArgs e)
        {
            _itemPanning = true;
            _itemVerticalPanning = _itemVerticalPanning || Math.Abs(e.TotalDistance.Y) > 10;
            ScrollBy(-e.DeltaDistance.Y, false);
            if (_itemVerticalPanning)
                Scrolling?.Invoke(this, EventArgs.Empty);
        }

        void OnItemPanned(object sender, ItemWrapperPanEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ListView.OnItemPanned " + DateTime.Now);
            ScrollBy(-e.DeltaDistance.Y, false);
            if (_itemVerticalPanning)
            {
                Scrolled?.Invoke(this, EventArgs.Empty);
                _scrollCompletedAt = DateTime.Now;
            }
            _itemVerticalPanning = false;
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
             {
                 _itemPanning = false;
                 return false;
             });
        }

        #endregion


        #region Cell Visibility Handlers

        void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e?.Item is ItemWrapper itemWrapper)
            {
                //System.Diagnostics.Debug.WriteLine("APPEARING: " + itemWrapper.Source);
                if (!_visibleItemWrappers.Contains(itemWrapper))
                    _visibleItemWrappers.Add(itemWrapper);
                if (itemWrapper.Source != null)
                    ItemAppearing?.Invoke(this, new ItemVisibilityEventArgs(itemWrapper.Source, itemWrapper.Index));
            }
            if (_resetScrollToSelected)
            {
                _resetScrollToSelected = false;
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    if (DateTime.Now - _scrollResetAt > TimeSpan.FromMilliseconds(100))
                    {
                        Appeared?.Invoke(this, EventArgs.Empty);
                        return false;
                    }
                    return true;
                });
            }
            _scrollResetAt = DateTime.Now;
        }

        void OnItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e?.Item is ItemWrapper itemWrapper)
            {
                //System.Diagnostics.Debug.WriteLine("DISAPPEARING: " + itemWrapper.Source);
                if (_visibleItemWrappers.Contains(itemWrapper))
                    _visibleItemWrappers.Remove(itemWrapper);
                if (itemWrapper.Source != null)
                {
                    ItemDisappearing?.Invoke(this, new ItemVisibilityEventArgs(itemWrapper.Source, itemWrapper.Index));
                    if (itemWrapper.Source == SelectedItem)
                        _listView.SelectedItem = null;
                }
            }

        }

        readonly List<ItemWrapper> _visibleItemWrappers = new List<ItemWrapper>();

        internal List<ItemWrapper> VisibleItemWrappers => new List<ItemWrapper>(_visibleItemWrappers);

        /// <summary>
        /// Gets a list of the ListView's visible indexes.
        /// </summary>
        /// <value>The visible indexes.</value>
        public List<int[]> VisibleIndexes
        {
            get
            {
                var result = new List<int[]>();
                if (_listView.ItemsSource is GroupWrapper itemsSource)
                {
                    for (int i = 0; i < itemsSource.Count; i++)
                    {
                        var itemWrapper = itemsSource[i];
                        if (_visibleItemWrappers.Contains(itemWrapper))
                            result.Add(new int[] { i });
                        if (itemWrapper is GroupWrapper gr)
                        {
                            for (int j = 0; j < gr.Count; j++)
                            {
                                var subItemWrapper = gr[j];
                                if (_visibleItemWrappers.Contains(subItemWrapper))
                                    result.Add(new int[] { i, j });
                            }
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Gets a list of the ListView's visible items.
        /// </summary>
        /// <value>The visible items.</value>
        public List<object> VisibleItems
        {
            get
            {
                var result = new List<object>();
                foreach (var itemWrapper in _visibleItemWrappers)
                    result.Add(itemWrapper.Source);
                return result;
            }
        }

        #endregion


        #region Selection Management
        // Assumption: SelectedItem(s) must be set AFTER ItemsSource and ItemsSourceMap has been set.  Otherwise, selected items will be culled

        bool _internalAddRemove;

        void AddSelection(ItemWrapper itemWrapper, object item)
        {
            _internalAddRemove = true;
            if (itemWrapper is null || item is null)
                throw new InvalidOperationException("Cannot select null item");
            itemWrapper.IsSelected = true;
            if (!_selectedItemWrappers.Contains(itemWrapper))
                _selectedItemWrappers.Add(itemWrapper);
            _selectedItemWrapper = itemWrapper;
            //if (sourceItem != SelectedItem)
            SelectedItem = item;
            if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && !SelectedItems.Contains(item))
                SelectedItems.Add(item);
            _internalAddRemove = false;
        }

        void AddSelectedItemWrapper(ItemWrapper itemWrapper)
        {
            if (itemWrapper is null)
                return;
            var sourceItem = itemWrapper.Source;
            AddSelection(itemWrapper, sourceItem);
        }

        void AddSelectedItem(object item)
        {
            if (item is null)
                return;
            var itemWrapper = BaseItemsSource.ItemWrapperForSource(item);
            AddSelection(itemWrapper, item);
        }

        void RemoveSelection(ItemWrapper itemWrapper, object item)
        {
            _internalAddRemove = true;
            if (itemWrapper != null)
            {
                itemWrapper.IsSelected = false;
                if (_selectedItemWrappers.Contains(itemWrapper))
                    _selectedItemWrappers.Remove(itemWrapper);
            }
            //if (SelectedItem == item)
            if (SelectedItem != null && SelectedItem.Equals(item))
            {
                SelectedItem = null;
                _selectedItemWrapper = null;
            }
            if (GroupToggleBehavior == GroupToggleBehavior.Multiselect && item != null && SelectedItems.Contains(item))
                SelectedItems.Remove(item);
            _internalAddRemove = false;
        }

        void RemoveSelectedItemWrapper(ItemWrapper itemWrapper)
        {
            if (itemWrapper is null)
                return;
            var sourceItem = itemWrapper.Source;
            RemoveSelection(itemWrapper, sourceItem);
        }

        void RemoveSelectedItem(object item)
        {
            if (item is null)
                return;
            if (BaseItemsSource?.ItemWrapperForSource(item) is ItemWrapper itemWrapper)
                RemoveSelection(itemWrapper, item);
        }

        void AddSelectedItems(IList items)
        {
            if (items is null)
                return;
            foreach (var item in items)
                AddSelectedItem(item);
        }

        void RemoveSelectedItems(IList items)
        {
            if (items is null)
                return;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                RemoveSelectedItem(item);
            }
        }

        void SelectedItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_internalAddRemove)
                return;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    AddSelectedItems(e.NewItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    RemoveSelectedItems(e.OldItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    RemoveSelectedItems(e.OldItems);
                    AddSelectedItems(e.NewItems);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    RemoveSelectedItems(SelectedItems);
                    break;
            }
        }
        /*
        void ReevaluateSelectedItems()
        {
            // remove any SelectedItems that are not a Item.Source in the _baseItemsSource
            _selectedItemWrapper = null;
            _selectedItemWrappers.Clear();
            if (GroupToggleBehavior == GroupToggleBehavior.Multiselect)
            {
                for (int i = SelectedItems.Count - 1; i >= 0; i--)
                {
                    var item = SelectedItems[i];
                    var itemWrapper = BaseItemsSource.ItemWrapperForSource(item);
                    if (itemWrapper == null)
                        RemoveSelectedItem(item);
                    else
                        AddSelectedItem(item);
                }
            }
            else if (GroupToggleBehavior == GroupToggleBehavior.Radio)
            {
                var itemWrapper = BaseItemsSource.ItemWrapperForSource(SelectedItem);
                if (itemWrapper == null)
                    RemoveSelectedItem(SelectedItem);
                else
                    AddSelectedItem(SelectedItem);
            }
        }
        */
        #endregion


        #region Update BaseItemsSource
        void UpdateBaseItemsSource()
        {
            if (_listView.ItemsSource is GroupWrapper groupWrapper)
            {
                groupWrapper.Tapped -= OnItemTapped;
                groupWrapper.SwipeMenuItemTapped -= OnSwipeMenuItemTapped;
                if (Device.RuntimePlatform == Device.UWP)
                {
                    groupWrapper.Panning -= OnItemPanning;
                    groupWrapper.Panned -= OnItemPanned;
                }
                //groupWrapper.LongPressed -= OnLongPressed;
                //groupWrapper.LongPressing -= OnLongPressing;
            }

            if (ItemsSource != null)
            {

                groupWrapper = new GroupWrapper();

                #region Gestures
                groupWrapper.Tapped += OnItemTapped;
                groupWrapper.SwipeMenuItemTapped += OnSwipeMenuItemTapped;
                if (Device.RuntimePlatform == Device.UWP)
                {
                    groupWrapper.Panning += OnItemPanning;
                    groupWrapper.Panned += OnItemPanned;
                }
                //groupWrapper.LongPressed += OnLongPressed;
                //groupWrapper.LongPressing += OnLongPressing;
                #endregion

                #region Data mapping and filtering
                groupWrapper.BindingContext = this;
                groupWrapper.SourceSubPropertyMap = SourcePropertyMap;
                groupWrapper.SubGroupType = SubGroupType;
                groupWrapper.VisibilityTest = VisibilityTest;
                #endregion

                #region RowHeight Properties
                groupWrapper.RequestedGroupHeaderRowHeight = GroupHeaderRowHeight;
                groupWrapper.RequestedRowHeight = RowHeight;
                #endregion

                #region Separator properties
                groupWrapper.SeparatorVisibility = SeparatorVisibility;
                groupWrapper.SeparatorLeftIndent = SeparatorLeftIndent;
                groupWrapper.SeparatorRightIndent = SeparatorRightIndent;
                groupWrapper.RequestedSeparatorHeight = SeparatorHeight;
                groupWrapper.SeparatorColor = SeparatorColor;
                #endregion

                #region Background Colors
                //groupWrapper.CellBackgroundColor = Color.Transparent;
                groupWrapper.GroupHeaderBackgroundColor = GroupHeaderBackgroundColor;
                groupWrapper.SelectedCellBackgroundColor = SelectedCellBackgroundColor;
                #endregion

                groupWrapper.Source = ItemsSource;

                BaseItemsSource = groupWrapper;
            }
            else
                BaseItemsSource = null;

            if (P42.Utils.Environment.IsOnMainThread)
                Update_listViewItemsSourceAction();
            else
                Device.BeginInvokeOnMainThread(Update_listViewItemsSourceAction);
            _resetScrollToSelected = true;
            _scrollResetAt = DateTime.Now;
        }

        void Update_listViewItemsSourceAction()
        {
            _listView.SelectedItem = null;  // why is base.SelectedItem getting selected?  I don't know.  But let's stop this!
            _listView.ItemsSource = BaseItemsSource;
            IsGroupingEnabled = BaseItemsSource != null && BaseItemsSource.ContentType == GroupWrapper.GroupContentType.Lists;
            ItemsSourceSet?.Invoke(this, EventArgs.Empty);
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


        #region Item change handlers
        /// <summary>
        /// Occurs when a property of a ListViewItem is about to change.
        /// </summary>
        public event Xamarin.Forms.PropertyChangingEventHandler ItemPropertyChanging;
        void OnItemPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
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


        #region Scrolling
        /*
        internal Func<double, bool> RendererScrollBy;
        internal Action<object, object, ScrollToPosition, bool> RendererScrollToPos;
        internal Func<double> RendererScrollOffset;
        */


        internal void OnScrolling(object sender, EventArgs e)
        {
            if (!_itemPanning)
            {
                // let OnPanning handle this
                //System.Diagnostics.Debug.WriteLine("==SCROLLING==");
                Scrolling?.Invoke(this, EventArgs.Empty);
            }
        }


        internal void OnScrolled(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("ListView.OnScrolled: _itemPanning=" + _itemPanning + " " + DateTime.Now);
            if (!_itemPanning)
            {
                //System.Diagnostics.Debug.WriteLine("~~SCROLLED~~");
                // let OnPanned handle this
                Scrolled?.Invoke(this, EventArgs.Empty);
                _scrollCompletedAt = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ListView"/> is scroll enabled.
        /// </summary>
        /// <value><c>true</c> if is scroll enabled; otherwise, <c>false</c>.</value>
        public bool IsScrollEnabled
        {
            get => _listView.IsScrollEnabled;
            set => _listView.IsScrollEnabled = value;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Forms9Patch.ListView"/> is actively scrolling.
        /// </summary>
        /// <value><c>true</c> if is actively scrolling; otherwise, <c>false</c>.</value>
        public bool IsScrolling => _listView.IsScrolling;

        /// <summary>
        /// Gets the scroll offset (current position) of ListView.
        /// </summary>
        /// <value>The scroll offset.</value>
        public double ScrollOffset => _listView.ScrollOffset;

        /// <summary>
        /// Scrolls ListView to a position (offset) in pixels from the start
        /// </summary>
        /// <returns><c>true</c>, if to was scrolled, <c>false</c> otherwise.</returns>
        /// <param name="offset">Offset (position) to scroll to.</param>
        /// <param name="animiated">If set to <c>true</c> animiated.</param>
        public bool ScrollTo(double offset, bool animiated = true) => _listView.ScrollTo(offset, animiated);

        internal static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create(nameof(ScrollEnabled), typeof(bool), typeof(ListView), true);
        internal bool ScrollEnabled
        {
            get => (bool)GetValue(ScrollEnabledProperty);
            set => SetValue(ScrollEnabledProperty, value);
        }

        /// <summary>
        /// Scrolls the ListView by delta pixels.
        /// </summary>
        /// <returns><c>true</c>, if by was scrolled, <c>false</c> otherwise.</returns>
        /// <param name="delta">Delta to move list relative to current position</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public bool ScrollBy(double delta, bool animated = true) => _listView.ScrollBy(delta, animated);


        /// <summary>
        /// Scrolls to item in group
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="group">Group.</param>
        /// <param name="position">Position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public bool ScrollTo(object item, object group, ScrollToPosition position, bool animated = true) => ScrollTo(BaseItemsSource.TwoDeepDataSet(group, item), position, animated);

        /// <summary>
        /// Scrolls to item
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="position">Position.</param>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public bool ScrollTo(object item, ScrollToPosition position, bool animated = true) => ScrollTo(BaseItemsSource.TwoDeepDataSet(item), position, animated);

        /// <summary>
        /// Scrolls to item at index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public bool ScrollTo(int[] index, ScrollToPosition position, bool animated = true) => ScrollTo(BaseItemsSource.TwoDeepDataSet(index), position, animated);


        bool ScrollTo(DeepDataSet dataSet, ScrollToPosition position, bool animated = true)
        {
            if (dataSet == null)
                return false;
            var offset = dataSet.Offset + _listView.HeaderHeight;
            if (position == ScrollToPosition.Start)
                return _listView.ScrollTo(offset, animated);

            var cellHeight = dataSet.CellHeight;

            if (_listView.Height < 0)
                return false
                    ;
            if (position == ScrollToPosition.Center)
            {
                offset += -_listView.Height / 2 + cellHeight / 2;
                return _listView.ScrollTo(offset, animated);
            }

            offset += -_listView.Height + cellHeight;
            return _listView.ScrollTo(offset, animated);
        }
        #endregion


        #region Item Finding

        bool ItemWrapperForSourceItem(object sourceItem, IEnumerable sourceGroup, out ItemWrapper itemWrapper)
        {
            itemWrapper = null;
            foreach (var item in sourceGroup)
            {
                //if (item == sourceItem)
                if (item.Equals(sourceItem))
                {
                    itemWrapper = BaseItemsSource.ItemWrapperForSource(item);
                    // we are in this function because item isn't in BaseItemsSource so we will now return so the recursion can find the item's parent that is in BaseItemsSource
                    return true;
                }
                if (item is IEnumerable enumerable && ItemWrapperForSourceItem(sourceItem, enumerable, out itemWrapper))
                {
                    // source is a child of this object!
                    itemWrapper = itemWrapper ?? BaseItemsSource.ItemWrapperForSource(enumerable);
                    return true;
                }
            }
            return false;
        }

        internal DeepDataSet TwoDeepDataSetAtPoint(Point p)
        {
            if (p.Y < Bounds.Top || p.Y > Bounds.Bottom)
                return null;
            var offset = _listView.ScrollOffset - _listView.HeaderHeight + p.Y;
            var result = BaseItemsSource?.TwoDeepDataSetForOffset(offset);
            return result;
        }


        #endregion


        /*
        #region Item Finding

        internal int[] DeepIndexAtOffset(double offset)
        {
            double calcOffset = 0;

            if (Header is VisualElement header)
                calcOffset = header.Height;

            if (offset < calcOffset)
                return null;

            //foreach (var topItem in BaseItemsSource)
            for (int i=0; i< BaseItemsSource.Count; i++)
            {
                var topItem = BaseItemsSource[i];
                calcOffset += topItem.RowHeight < 0 ? RowHeight : topItem.RowHeight;
                if (offset <= calcOffset)
                    return new int[] { i };
                if (topItem is GroupWrapper gr)
                {
                    //foreach (var subItem in gr)
                    for (int j=0; j< gr.Count; j++)
                    {
                        var subItem = gr[j];
                        calcOffset += subItem.RowHeight < 0 ? RowHeight : subItem.RowHeight;
                        if (offset <= calcOffset)
                            return new int[] { i, j };
                    }
                }
            }
            return null;
        }


        internal ItemWrapper ItemWrapperAtCenter()
        {
            var centerOffset = ScrollOffset + Bounds.Height / 2;
            return ItemWrapperAtOffset(centerOffset);
        }


        internal ItemWrapper ItemWrapperAtOffset(double offset)
        {
            double calcOffset = 0;

            if (Header is VisualElement header)
                calcOffset = header.Height;

            if (offset < calcOffset)
                return null;

            foreach (var topItem in BaseItemsSource)
            {
                calcOffset += topItem.RowHeight < 0 ? RowHeight : topItem.RowHeight;
                if (offset <= calcOffset)
                    return topItem;
                if (topItem is GroupWrapper gr)
                {
                    foreach (var subItem in gr)
                    {
                        calcOffset += subItem.RowHeight < 0 ? RowHeight : subItem.RowHeight;
                        if (offset <= calcOffset)
                            return subItem;
                    }
                }
            }
            return null;
        }

        public object ItemAtOffset(double offset)
        {
            return ItemWrapperAtOffset(offset)?.Source;
        }

        CellProximityEventArgs CellUnderRectangle(Rectangle rect)
        {
            if (rect.Right < Bounds.Left || rect.Left > Bounds.Right || rect.Bottom < Bounds.Top || rect.Top > Bounds.Bottom)
                return CellProximityEventArgs.None;

            var hitRect = new Rectangle
            {
                Left = Math.Max(Bounds.Left, rect.Left),
                Top = Math.Max(Bounds.Top, rect.Top),
                Right = Math.Min(Bounds.Right, rect.Right),
                Bottom = Math.Min(Bounds.Bottom, rect.Bottom)
            };

            double calcOffset = 0;
            var offset = ScrollOffset;

            if (Header is VisualElement header)
                calcOffset = header.Height;

            if (offset + hitRect.Bottom < calcOffset)
                return null;
            

            foreach (var topItem in BaseItemsSource)
            {
                calcOffset += topItem.RowHeight < 0 ? RowHeight : topItem.RowHeight;
                if (offset + hitRect.Top <= calcOffset)
                    return new CellProximityEventArgs(BaseItemsSource, topItem, Proximity.Aligned);
                if (offset + hitRect.Center.Y <= calcOffset)
                    return new CellProximityEventArgs(BaseItemsSource, topItem, Proximity.After);
                if (offset + hitRect.Bottom <= calcOffset)
                    return new CellProximityEventArgs(BaseItemsSource, topItem, Proximity.Before);
                if (topItem is GroupWrapper gr)
                {
                    foreach (var subItem in gr)
                    {
                        calcOffset += subItem.RowHeight < 0 ? RowHeight : subItem.RowHeight;
                        if (offset + hitRect.Top <= calcOffset)
                            return new CellProximityEventArgs(BaseItemsSource, subItem, Proximity.Aligned);
                        if (offset + hitRect.Center.Y <= calcOffset)
                            return new CellProximityEventArgs(BaseItemsSource, subItem, Proximity.After);
                        if (offset + hitRect.Bottom <= calcOffset)
                            return new CellProximityEventArgs(BaseItemsSource, subItem, Proximity.Before);
                    }
                }
            }
            return null;
        }


        #endregion
                */


        /*
        #region Drag/Drop
        /// <summary>
        /// Delegate function that will be called to query if a item (at a deep index location) can be dragged
        /// </summary>
        public Func<ListView, object, int[], bool> CanDrag;
        /// <summary>
        /// Delegate function that will be called to query if a item being dragged can be dropped at location specified by a deep index.
        /// </summary>
        public Func<ListView, object, int[], bool> CanDrop;

        //CellProximityEventArgs _longPress;
        DeepDataSet _longPress;
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

            //_longPress = DependencyService.Get<IListItemLocation>().CellProximityEventArgsForItemAtPoint(this, e.Center);
            _longPress = TwoDeepDataSetAtPoint(e.Center);
            if (_longPress != null && _longPress.ItemWrapper.BaseCellView != null)
            {
                // can we drag this Item?
                bool canDrag = true;
                if (CanDrag != null)
                    canDrag = CanDrag(this, _longPress.ItemWrapper.Source, _longPress.Index);
                if (!canDrag)
                {
                    _longPress = null;
                    return;
                }
                SelectedItem = null;
                _nativeFrame = _longPress.ItemWrapper.BaseCellView.BoundsToWinCoord();

                // need a null item to fill the void 
                _nullItem.RequestedHeight = _nativeFrame.Height;
                _nullItem.CellBackgroundColor = BackgroundColor;

                BaseItemsSource.NotifySourceOfChanges = false;
                BaseItemsSource.DeepSwap(_longPress.ItemWrapper, _nullItem);

                //_longPress.Item.SeparatorIsVisible = false;
                var contentView = ItemTemplates.MakeContentView(_longPress.ItemWrapper);
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
                //System.Diagnostics.Debug.WriteLine("LONGPRESS PANNING");
                _popup.TranslationX = e.TotalDistance.X;
                _popup.TranslationY = e.TotalDistance.Y;

                Point currentOrigin = _nativeFrame.Location + (Size)e.TotalDistance;
                var cellRect = new Rectangle(currentOrigin, _nativeFrame.Size);
                var currentDragOver = CellUnderRectangle(cellRect);
                if (currentDragOver != null && currentDragOver.ItemWrapper != _nullItem && currentDragOver.ItemWrapper != null)
                {
                    //System.Diagnostics.Debug.WriteLine ("current=[{0}]", currentDragOver.Item.Title);
                    // can we drop here?
                    bool canDrop = true;
                    if (CanDrop != null)
                        canDrop = CanDrop(this, currentDragOver.ItemWrapper.Source, currentDragOver.DeepIndex);
                    if (canDrop)
                    {
                        // yes: put the NullItem here
                        BaseItemsSource.DeepRemove(_nullItem);
                        BaseItemsSource.DeepInsert(currentDragOver.DeepIndex, _nullItem);
                    }
                    else if (BaseItemsSource.DeepContains(_nullItem) && !BaseItemsSource.DeepIndexOf(_nullItem).SequenceEqual(_longPress.Index))
                    {
                        // no: put the NullItem at the location where we started 
                        BaseItemsSource.DeepRemove(_nullItem);
                        BaseItemsSource.DeepInsert(_longPress. Index, _nullItem);
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
                _baseItemsSource.DeepInsert(_longPress.DeepIndex, _longPress.ItemWrapper);
                _baseItemsSource.NotifySourceOfChanges = true;
                if (!nullIndex.SequenceEqual(_longPress.DeepIndex))
                {
                    // we made a allowed move, so make that move
                    _baseItemsSource.DeepRemove(_longPress.ItemWrapper);
                    _baseItemsSource.DeepInsert(nullIndex, _longPress.ItemWrapper);
                }
                _longPress = null;
                _popup.Content = null;
            }

        }
        #endregion
        */
    }
}

