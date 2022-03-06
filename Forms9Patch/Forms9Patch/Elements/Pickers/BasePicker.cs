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
    public abstract class BasePicker : Xamarin.Forms.ListView //Xamarin.Forms.CollectionView
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
        
        #endregion



        #region Fields
        readonly BoxView _upperPadding = new BoxView { Color = Color.Transparent };
        readonly BoxView _lowerPadding = new BoxView { Color = Color.Transparent };

        /// <summary>
        /// For debug purposes
        /// </summary>
        public int Instances { get; private set; }
        /// <summary>
        /// For debug purposes
        /// </summary>
        public int Instance { get; private set; }
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
            Instance = Instances++;
            ItemTemplate = ItemTemplates;
            SelectionMode = ListViewSelectionMode.Single;
            BackgroundColor = Color.Transparent;

            Header = _upperPadding;
            Footer = _lowerPadding;
        }

        #endregion


        #region Property Change management


        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    base.OnPropertyChanged(propertyName);
                }
                catch (Exception) { }


                if (propertyName == HeightProperty.PropertyName)
                {
                    _lowerPadding.HeightRequest = (Height - RowHeight) / 3.0;
                    _upperPadding.HeightRequest = (Height - RowHeight) / 3.0;
                }
                else if (propertyName == IndexProperty.PropertyName)
                {
                    if (ItemsSource?.Cast<object>().ToList() is List<object> items)
                    {
                        if (Index > -1 && Index < items.Count)
                        {
                            SelectedItem = items[Index];
                        }
                    }
                }
                else if (propertyName == SelectedItemProperty.PropertyName)
                {
                    //if (!_scrolling && ItemsSource?.Cast<object>().ToList() is List<object> items)
                    //    Index = items.IndexOf(SelectedItem);
                }
            });
        }

        #endregion

        /*
        #region Snap to cell
        DateTime _lastScrollPoint = DateTime.MinValue.AddYears(1);
        bool _scrolling;
        /// <summary>
        /// Used to determine when scrolling has stopped
        /// </summary>
        /// <param name="e"></param>
        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            base.OnScrolled(e);
            _lastScrollPoint = DateTime.Now;
            if (!_scrolling)
            {
                _scrolling = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
                {
                    if ((DateTime.Now - _lastScrollPoint).Milliseconds >= 300)
                    {
                        _scrolling = false;
                        return false;
                    }
                    return true;
                });
            }
        }
        #endregion
        */
    }

    /// <summary>
    /// Template Picker
    /// </summary>
    public class BasePickerDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        readonly Dictionary<Type, DataTemplate> Templates = new Dictionary<Type, DataTemplate>();

        /// <summary>
        /// Selects a template for an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var type = item.GetType();
            if (TemplateForType(type) is DataTemplate template)
                return template;
            var baseType = type.GetTypeInfo().BaseType;
            if (TemplateForType(baseType) is DataTemplate template1)
                return template1;
            return Templates.FirstOrDefault().Value;
        }

        DataTemplate TemplateForType(Type type)
        {
            if (Templates.TryGetValue(type, out DataTemplate dataTemplate))
                return dataTemplate;
            if (type.IsConstructedGenericType && Templates.TryGetValue(type.GetGenericTypeDefinition(), out DataTemplate dataTemplate1))
                    return dataTemplate1;
            return null;
        }

        /// <summary>
        /// Adds a new template
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="templateType"></param>
        public void Add(Type itemType, Type templateType)
            => Templates[itemType] = new DataTemplate(templateType);

        /// <summary>
        /// Clears the templates
        /// </summary>
        public void Clear()
            => Templates.Clear();
    }
}

