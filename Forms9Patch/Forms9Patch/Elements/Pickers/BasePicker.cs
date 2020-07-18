using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Base picker.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    class BasePicker : Xamarin.Forms.CollectionView
    {
        #region Properties

        #region Item Source & Templates
        /// <summary>
        /// Gets the item templates.
        /// </summary>
        /// <value>The item templates.</value>
        //public DataTemplateSelector ItemTemplates => _listView.ItemTemplates;
        public BasePickerDataTemplateSelector ItemTemplates { get; } = new BasePickerDataTemplateSelector();
        #endregion

        #region Position and Selection
        /// <summary>
        /// The index property.
        /// </summary>
        public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(BasePicker), -1, BindingMode.TwoWay);
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }


        #region IsSelectOnScrollEnabled Property
        /// <summary>
        /// BindableProperty for IsSelectOnScrollEnabled
        /// </summary>
        public static readonly BindableProperty IsSelectOnScrollEnabledProperty = BindableProperty.Create(nameof(IsSelectOnScrollEnabled), typeof(bool), typeof(BasePicker), true);
        /// <summary>
        /// Enables the ability to select an item when it is focused upon during a scroll
        /// </summary>
        public bool IsSelectOnScrollEnabled
        {
            get => (bool)GetValue(IsSelectOnScrollEnabledProperty);
            set => SetValue(IsSelectOnScrollEnabledProperty, value);
        }
        #endregion


        #endregion
        
        #region Appearance
        /// <summary>
        /// The row height property.
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(BasePicker), 40);
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
        
        #region TextColor property
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BasePicker), default(Color));
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion



        #endregion



        #region Fields
        readonly BoxView _upperPadding = new BoxView { Color = Color.Transparent };
        readonly BoxView _lowerPadding = new BoxView { Color = Color.Transparent };
        bool _isRendered;
        bool _waitingForIsRendered;
        IList<object> _pendingSelectedItems;
        object _pendingSelectedItem;
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
            IsGrouped = false;
            ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
            ItemTemplate = ItemTemplates;
            SelectionMode = SelectionMode.Single;
            BackgroundColor = Color.Transparent;

            Header = _upperPadding;
            Footer = _lowerPadding;

        }

        #endregion




        #region Property Change management
        
        protected virtual void OnRendered()
        {
            if (ElementExtensions.HasRenderer(this))
            {
                _isRendered = true;
                SelectedItems = _pendingSelectedItems;
                SelectedItem = _pendingSelectedItem;
            }
            _isRendered = false;
            /*
        if (ItemsSource == null || (ItemsSource.Cast<object>().ToArray() is object[] array && (array.Length > 0 || !array.Contains(SelectedItem)) ) )
            ScrollTo(0);
        if (SelectedItem != null)
            ScrollTo(SelectedItem);
        else if (SelectedItems.Count > 0)
            ScrollTo(SelectedItems[0]);
            */
        }



        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            /*
            if (!_isRendered)
            {
                if (propertyName == SelectedItemProperty.PropertyName)
                {
                    if (SelectedItem != null)
                        _pendingSelectedItem = SelectedItem;
                    SelectedItem = null;
                    return;
                }
                else if (propertyName == SelectedItemsProperty.PropertyName)
                {
                    if (SelectedItems != null)
                        _pendingSelectedItems = SelectedItems;
                    SelectedItems = null;
                    return;
                }
            }
            */
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            System.Diagnostics.Debug.WriteLine("BasePicker.OnPropertyChanged(" + propertyName + ") ["+DateTime.Now+"]");


            try
            {
                base.OnPropertyChanged(propertyName);
            }
            catch (Exception) { }



            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                //_listView.ItemsSource = ItemsSource;
                if (ItemsSource?.Cast<object>().ToArray() is object[] items && Index > -1 && Index < items.Length)
                    ScrollTo(Index, position: ScrollToPosition.Center);
                /*
                else
                {
                    SelectedItem = null;
                    SelectedItems.Clear();
                }
                */
            }
            else if (propertyName == HeightProperty.PropertyName)// || propertyName == RowHeightProperty.PropertyName)
            {
                _lowerPadding.HeightRequest = (Height - RowHeight) / 2.0;
                _upperPadding.HeightRequest = (Height - RowHeight) / 2.0;
            }
            else if (propertyName == IndexProperty.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("Index: " + Index);

                //if (!_scrolling)
                //    ScrollTo(Index, position: ScrollToPosition.Center);
                if (ItemsSource?.Cast<object>().ToList() is List<object> items)
                {
                    if (Index > -1 && Index < items.Count)
                        SelectedItem = items[Index];
                    if (!_scrolling)
                        ScrollTo(Index, -1, ScrollToPosition.Center, true);
                }
            }
            else if (propertyName == SelectedItemProperty.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("SelectedItem: " + SelectedItem);

                if (!_scrolling && ItemsSource?.Cast<object>().ToList() is List<object> items)
                    Index = items.IndexOf(SelectedItem);
            }
            else if (propertyName == SelectedItemsProperty.PropertyName)
            {
                if (!_scrolling && SelectedItems != null && SelectedItems.Count > 0 && ItemsSource?.Cast<object>().ToList() is List<object> items)
                {
                    ScrollTo(SelectedItems[0]);
                }
            }
            /*
            else if (propertyName == "Renderer")
            {
                if (ElementExtensions.HasRenderer(this))
                {
                    if (!_waitingForIsRendered)
                    {
                        _waitingForIsRendered = true;
                        Device.StartTimer(TimeSpan.FromMilliseconds(5000), () =>
                        {
                            if (_waitingForIsRendered && ElementExtensions.HasRenderer(this))
                            {
                                _waitingForIsRendered = false;
                                OnRendered();
                            }
                            return false;
                        });
                    }
                }
                else
                    _isRendered = false;
            }
            */
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child is VisualElement element)
                System.Diagnostics.Debug.WriteLine("CHILD HEIGHT: " + element.Height);
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
        }
        #endregion


        #region Snap to cell
        DateTime _lastScrollPoint = DateTime.MinValue;
        bool _scrolling;
        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            base.OnScrolled(e);

            //System.Diagnostics.Debug.WriteLine("Scrolled: dt["+(DateTime.Now - _lastScrollPoint).Milliseconds+"]  Delta["+e.VerticalDelta+"] offset["+e.VerticalOffset+"] first["+e.FirstVisibleItemIndex+"] center=["+e.CenterItemIndex+"] last=["+e.LastVisibleItemIndex+"]");
            _lastScrollPoint = DateTime.Now;
            if (!_scrolling)
            {
                _scrolling = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
                {
                    if ((DateTime.Now - _lastScrollPoint).Milliseconds >= 300)
                    {
                        _scrolling = false;
                        //if (Index > -1)
                        //    ScrollTo(Index, position: ScrollToPosition.Center);
                        return false;
                    }
                    return true;
                });
            }
        }
        #endregion



    }

    class BasePickerDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        Dictionary<Type, DataTemplate> Templates = new Dictionary<Type, DataTemplate>();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var type = item.GetType();
            if (TemplateForType(type) is DataTemplate template)
                return template;
            var baseType = type.GetTypeInfo().BaseType;
            if (TemplateForType(baseType) is DataTemplate template1)
                return template1;
            return null;
        }

        DataTemplate TemplateForType(Type type)
        {
            if (Templates.TryGetValue(type, out DataTemplate dataTemplate))
                return dataTemplate;
            if (type.IsConstructedGenericType && Templates.TryGetValue(type.GetGenericTypeDefinition(), out DataTemplate dataTemplate1))
                    return dataTemplate1;
            return null;
        }

        public void Add(Type itemType, Type templateType)
            => Templates[itemType] = new DataTemplate(templateType);

        public void Clear()
            => Templates.Clear();
    }
}

