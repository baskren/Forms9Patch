using System.Collections;
using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Single picker.
    /// </summary>
    [DesignTimeVisible(true)]
    public class SinglePicker : Grid
    {
        #region Properties
        /// <summary>
        /// The item templates property.
        /// </summary>
        public static readonly BindableProperty ItemTemplatesProperty = BindableProperty.Create(nameof(ItemTemplates), typeof(DataTemplateSelector), typeof(SinglePicker), null);
        /// <summary>
        /// Gets the item templates.
        /// </summary>
        /// <value>The item templates.</value>
        public DataTemplateSelector ItemTemplates
        {
            get => (DataTemplateSelector)GetValue(ItemTemplatesProperty);
            private set => SetValue(ItemTemplatesProperty, value);
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SinglePicker), null);
        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// The row height property.
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(SinglePicker), 30);
        /// <summary>
        /// Gets or sets the height of the row.
        /// </summary>
        /// <value>The height of the row.</value>
        public int RowHeight
        {
            get => (int)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }

        /// <summary>
        /// The index property.
        /// </summary>
        public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(SinglePicker), 0);
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
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SinglePicker), null);
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        #endregion

        #region Fields
        readonly internal BasePicker _basePicker = new BasePicker
        {
            IsSelectOnScrollEnabled = true
        };

        readonly internal Color _overlayColor = Color.FromRgb(0.85, 0.85, 0.85);

        readonly internal ColorGradientBox _upperGradient = new ColorGradientBox
        {
            Orientation = StackOrientation.Vertical
        };

        readonly internal ColorGradientBox _lowerGradient = new ColorGradientBox
        {
            Orientation = StackOrientation.Vertical
        };

        readonly internal BoxView _upperEdge = new BoxView
        {
            BackgroundColor = Color.Gray,
            HeightRequest = 1,
            VerticalOptions = LayoutOptions.End,
        };
        readonly internal BoxView _lowerEdge = new BoxView
        {
            BackgroundColor = Color.Gray,
            HeightRequest = 1,
            VerticalOptions = LayoutOptions.Start,
        };

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.SinglePicker"/> class.
        /// </summary>
        public SinglePicker()
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition{ Height = GridLength.Star },
                new RowDefinition{ Height = RowHeight },
                new RowDefinition{ Height = GridLength.Star }
            };

            Padding = new Thickness(0, 0, 0, 0);

            _basePicker._listView.SelectedCellBackgroundColor = Color.Transparent;
            _basePicker.ItemTemplates.RemoveFactoryDefaults();
            _basePicker.ItemTemplates.Add(typeof(string), typeof(SinglePickerCellContentView));

            _upperGradient.StartColor = _overlayColor;
            _upperGradient.EndColor = _overlayColor.WithAlpha(0.5);
            _lowerGradient.StartColor = _overlayColor.WithAlpha(0.5);
            _lowerGradient.EndColor = _overlayColor;

            Children.Add(_upperEdge);
            Children.Add(_lowerEdge, 0, 2);
            Children.Add(_basePicker);
            Grid.SetRowSpan(_basePicker, 3);
            Children.Add(_upperGradient);
            Children.Add(_lowerGradient, 0, 2);

            _basePicker.SelectBy = SelectBy.Position;

            _basePicker.RowHeight = RowHeight;

            VerticalOptions = LayoutOptions.FillAndExpand;

            _basePicker.PropertyChanged += BasePickerPropertyChanged;
        }

        #region change management
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

            if (propertyName == RowHeightProperty.PropertyName)
            {
                _basePicker.RowHeight = RowHeight;
                RowDefinitions[1].Height = RowHeight;
            }
            else if (propertyName == ItemsSourceProperty.PropertyName)
                _basePicker.ItemsSource = ItemsSource;
            else if (propertyName == IndexProperty.PropertyName)
                _basePicker.Index = Index;
            else if (propertyName == SelectedItemProperty.PropertyName)
                _basePicker.SelectedItem = SelectedItem;
        }
        #endregion

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            SelectedItem = null;
        }

        void BasePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BasePicker.SelectedItemProperty.PropertyName)
            {
                SelectedItem = _basePicker.SelectedItem;
                if (ItemsSource != null && ItemsSource.Count > 0)
                {
                    for (int i = 0; i < ItemsSource.Count; i++)
                    {
                        if (ItemsSource[i].Equals(SelectedItem))
                        {
                            Index = i;
                            break;
                        }
                    }
                    //_basePicker.SelectedItem = null;
                    //_basePicker._listView.SelectedItem = null;
                }
            }
            else if (e.PropertyName == BasePicker.IndexProperty.PropertyName)
            {
                Index = _basePicker.Index;
                if (ItemsSource != null && ItemsSource.Count > Index)
                    SelectedItem = ItemsSource[Index];
            }
        }
    }



    #region Cell Template
    class SinglePickerCellContentView : Grid, ICellContentView, IIsSelectedAble
    {
        #region Properties
        public double CellHeight { get; set; }

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(MultiPickerCellContentView), default(bool));
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        #endregion


        #region Fields
        /*
        readonly Label checkLabel = new Label
        {
            Text = "✓",
            TextColor = Color.Blue,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            IsVisible = false
        };
        */
        protected readonly Label itemLabel = new Label
        {
            TextColor = Color.Black,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Start
        };

        #endregion


        #region Constructors
        public SinglePickerCellContentView()
        {
            CellHeight = 50;
            Padding = new Thickness(5, 1, 5, 1);
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(0,GridUnitType.Absolute)},
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
            };
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = new GridLength(CellHeight - Padding.VerticalThickness, GridUnitType.Absolute)}
            };
            IgnoreChildren = true;
            ColumnSpacing = 0;

            Children.Add(itemLabel, 1, 0);
        }
        #endregion


        #region Change management
        protected override void OnBindingContextChanged()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                return;
            }

            base.OnBindingContextChanged();
            if (BindingContext != null)
                itemLabel.Text = BindingContext.ToString();
            else
                itemLabel.Text = null;

            var parent = Parent;
            while (parent != null)
            {
                if (parent is BasePicker picker)
                {
                    itemLabel.TextColor = picker.TextColor;
                    return;
                }
                parent = parent.Parent;
            }

        }

        #endregion


        #region Appearing / Disappearing Event Handlers
        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }
        #endregion



    }
    #endregion

}
