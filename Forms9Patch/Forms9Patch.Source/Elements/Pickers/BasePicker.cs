using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Base picker.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    class BasePicker : Xamarin.Forms.ContentView
    {
        #region Properties

        #region Item Source & Templates
        /// <summary>
        /// Gets the item templates.
        /// </summary>
        /// <value>The item templates.</value>
        public DataTemplateSelector ItemTemplates => _listView.ItemTemplates;

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(BasePicker), null);
        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
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
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
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
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// The selected items property key.
        /// </summary>
        public static readonly BindablePropertyKey SelectedItemsPropertyKey = BindableProperty.CreateReadOnly("SelectedItems", typeof(ObservableCollection<object>), typeof(BasePicker), null);
        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public ObservableCollection<object> SelectedItems
        {
            get => (ObservableCollection<object>)GetValue(SelectedItemsPropertyKey.BindableProperty);
            private set => SetValue(SelectedItemsPropertyKey, value);
        }


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
            get => (GroupToggleBehavior)GetValue(GroupToggleBehaviorProperty);
            set => SetValue(GroupToggleBehaviorProperty, value);
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
            get => (int)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }

        #endregion

        #endregion


        #region Fields
        internal readonly Forms9Patch.ListView _listView = new Forms9Patch.ListView();
        readonly BoxView _upperPadding = new BoxView { Color = Color.Transparent };
        readonly BoxView _lowerPadding = new BoxView { Color = Color.Transparent };

        internal SelectBy SelectBy;
        bool _tapping;
        bool _scrolling;
        #endregion


        #region Constructor
        static BasePicker()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.BasePicker"/> class.
        /// </summary>
        internal BasePicker()
        {
            BackgroundColor = Color.Transparent;
            _listView.RowHeight = RowHeight;
            _listView.GroupToggleBehavior = GroupToggleBehavior;
            _listView.BackgroundColor = Color.Transparent;
            _listView.SelectedCellBackgroundColor = Color.Transparent;

            _listView.IsGroupingEnabled = false;
            _listView.SeparatorVisibility = SeparatorVisibility.None;

            _listView.ItemTapped += OnItemTapped;


            _listView.Scrolled += OnScrolled;
            _listView.Scrolling += OnScrolling;


            _listView.Header = _upperPadding;
            _listView.Footer = _lowerPadding;

            SelectedItems = _listView.SelectedItems;
            Content = _listView;

            _listView.Appeared += OnAppeared;
        }
        #endregion


        #region Selection management
        protected void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_scrolling)
                return;
            if (_scrollCompleting)
            {
                _scrollCompleting = false;
                return;
            }
            _tapping = true;
            int index = 0;
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
            e.Handled = true;
        }

        /// <summary>
        /// Programmatically Taps the item at point.
        /// </summary>
        /// <param name="p">P.</param>
        public void TapItemAtPoint(Point p) => _listView.TapItemAtPoint(p);

        #endregion


        #region Property Change management

        void OnAppeared(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                ScrollTo(SelectedItem);
            else if (SelectedItems.Count > 0)
                ScrollTo(SelectedItems[0]);
        }



        /// <summary>
        /// Ons the property changed.
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

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                _listView.ItemsSource = ItemsSource;
                if (ItemsSource != null)
                    ScrollToIndex(Index);
                else
                {
                    SelectedItem = null;
                    SelectedItems.Clear();
                    _listView.SelectedItem = null;
                    _listView.SelectedItems.Clear();
                }
            }
            else if (propertyName == HeightProperty.PropertyName || propertyName == RowHeightProperty.PropertyName)
            {
                _lowerPadding.HeightRequest = (Height - RowHeight) / 2.0;
                _upperPadding.HeightRequest = (Height - RowHeight) / 2.0;
            }
            else if (propertyName == IndexProperty.PropertyName && !_scrolling && (GroupToggleBehavior != GroupToggleBehavior.Multiselect || !_tapping))
                ScrollToIndex(Index);
            else if (propertyName == SelectedItemProperty.PropertyName && !_scrolling)
            {
                //System.Diagnostics.Debug.WriteLine("BasePicker SELECTED ITEM: " + SelectedItem);
                if ((GroupToggleBehavior != GroupToggleBehavior.Multiselect || !_tapping) && ItemsSource != null)
                    ScrollTo(SelectedItem);
            }
            else if (propertyName == RowHeightProperty.PropertyName)
                _listView.RowHeight = RowHeight;
            else if (propertyName == GroupToggleBehaviorProperty.PropertyName)
                _listView.GroupToggleBehavior = GroupToggleBehavior;
        }

        public virtual void ScrollTo(object item)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => ScrollTo(item));
                return;
            }

            if (ItemsSource == null || item == null)
                return;

            _listView.SelectedItem = item;
            _listView.ScrollTo(item, ScrollToPosition.Center);
        }

        /// <summary>
        /// Scrolls to index
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="force">If set to <c>true</c> scroll to index even if already scrolling.</param>
        public virtual void ScrollToIndex(int index, bool force = false)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => ScrollToIndex(index, force));
                return;
            }

            if (ItemsSource == null)
                return;

            int count = 0;
            object firstItem = null;
            object indexItem = null;
            object lastItem = null;
            if (ItemsSource is IList list)
            {
                firstItem = list[0];
                lastItem = list[list.Count - 1];
                if (index > -1 && index < list.Count)
                    indexItem = list[index];
                count = list.Count;
            }
            else
            {
                foreach (var item in ItemsSource)
                {
                    if (count == 0)
                        firstItem = item;
                    if (count == index)
                        indexItem = item;
                    lastItem = item;
                    count++;
                }
            }
            if (count > 0)
            {
                if (index < 0)
                    indexItem = firstItem;
                if (index > count - 1)
                    indexItem = lastItem;
                if (indexItem != null && (!_scrolling || force))
                    _listView.ScrollTo(indexItem, ScrollToPosition.Center, true);
            }
        }
        #endregion


        #region Snap to cell
        void OnScrolling(object sender, EventArgs e)
        {
            _scrolling = true;
            var deepDataSet = _listView.TwoDeepDataSetAtPoint(Bounds.Center);
            if (deepDataSet?.Index != null && deepDataSet.Index.Length == 1)
            {
                Index = deepDataSet.Index[0];
                SelectedItem = ItemsSource[Index];
            }
        }

        bool _scrollCompleting;
        void OnScrolled(object sender, EventArgs e)
        {
            if (SelectBy == SelectBy.Position && Index >= 0 && Index < ItemsSource.Count)
            {
                SelectedItem = ItemsSource[Index];
                ScrollToIndex(Index, true);
            }
            _scrolling = false;
        }

        #endregion



    }


}

