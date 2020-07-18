using System.Collections;
using Xamarin.Forms;
using System.ComponentModel;
using P42.Utils;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

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
        public static readonly BindableProperty ItemTemplatesProperty = BasePicker.ItemTemplateProperty; 
        /// <summary>
        /// Gets the item templates.
        /// </summary>
        /// <value>The item templates.</value>
        public DataTemplateSelector ItemTemplates
        {
            get => (DataTemplateSelector)_basePicker.GetValue(ItemTemplatesProperty);
            private set => _basePicker.SetValue(ItemTemplatesProperty, value);
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BasePicker.ItemsSourceProperty; 
        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource
        {
            get => (IList)_basePicker.GetValue(ItemsSourceProperty);
            set => _basePicker.SetValue(ItemsSourceProperty, value);
        }


        /// <summary>
        /// The row height property.
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BasePicker.RowHeightProperty; 
        /// <summary>
        /// Gets or sets the height of the row.
        /// </summary>
        /// <value>The height of the row.</value>
        public int RowHeight
        {
            get => (int)_basePicker.GetValue(RowHeightProperty);
            set => _basePicker.SetValue(RowHeightProperty, value);
        }
        

        /// <summary>
        /// The index property.
        /// </summary>
        public static readonly BindableProperty IndexProperty = BasePicker.IndexProperty; 
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get => (int)_basePicker.GetValue(IndexProperty);
            set => _basePicker.SetValue(IndexProperty, value);
        }


        /// <summary>
        /// The selected item property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BasePicker.SelectedItemProperty; 
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get => _basePicker.GetValue(SelectedItemProperty);
            set => _basePicker.SetValue(SelectedItemProperty, value);
        }

        #region IsHtmlText
        /// <summary>
        /// Backing store for SinglePicker's IsHtmlText property
        /// </summary>
        public static readonly BindableProperty IsHtmlTextProperty = BindableProperty.Create(nameof(IsHtmlText), typeof(bool), typeof(SinglePicker), default);
        /// <summary>
        /// controls value of .IsHtmlText property
        /// </summary>
        public bool IsHtmlText
        {
            get => (bool)GetValue(IsHtmlTextProperty);
            set => SetValue(IsHtmlTextProperty, value);
        }
        #endregion

        #endregion

        #region Fields
        readonly internal BasePicker _basePicker = new BasePicker
        {
            IsSelectOnScrollEnabled = true
        };
        /*
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
        */
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        internal protected Type PlainTextCellType = typeof(SinglePickerCellContentView);
        internal protected Type HtmlTextCellType = typeof(SinglePickerHtmlCellContentView);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion


        #region Events
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged
        {
            add => _basePicker.SelectionChanged += value;
            remove => _basePicker.SelectionChanged -= value;
        }
        #endregion


        static SinglePicker()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.SinglePicker"/> class.
        /// </summary>
        public SinglePicker()
        {

            RowSpacing = 0;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition{ Height = GridLength.Star },
                new RowDefinition{ Height = (int)BasePicker.RowHeightProperty.DefaultValue },
                new RowDefinition{ Height = GridLength.Star }
            };

            Padding = new Thickness(0, 0, 0, 0);

            //_basePicker._listView.SelectedCellBackgroundColor = Color.Transparent;
            _basePicker.ItemTemplates.Clear();
            _basePicker.ItemTemplates.Add(typeof(string), PlainTextCellType);

            //_upperGradient.StartColor = _overlayColor;
            //_upperGradient.EndColor = _overlayColor.WithAlpha(0.5);
            //_lowerGradient.StartColor = _overlayColor.WithAlpha(0.5);
            //_lowerGradient.EndColor = _overlayColor;

            //Children.Add(_upperEdge);
            //Children.Add(_lowerEdge, 0, 2);
            Children.Add(_basePicker);
            Grid.SetRowSpan(_basePicker, 3);
            //Children.Add(_upperGradient);
            //Children.Add(_lowerGradient, 0, 2);

            //_basePicker.RowHeight = RowHeight;

            VerticalOptions = LayoutOptions.FillAndExpand;

            //BackgroundColor = Color.Green;
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

            //if (propertyName == RowHeightProperty.PropertyName)
            //{
            //_basePicker.RowHeight = RowHeight;
            //    RowDefinitions[1].Height = RowHeight;
            //}
            //else
            if (propertyName == ItemsSourceProperty.PropertyName)
                _basePicker.ItemsSource = ItemsSource;
            else if (propertyName == IndexProperty.PropertyName)
                _basePicker.Index = Index;
            else if (propertyName == SelectedItemProperty.PropertyName)
                _basePicker.SelectedItem = SelectedItem;
            else if (propertyName == IsHtmlTextProperty.PropertyName)
            {
                _basePicker.ItemsSource = null;
                _basePicker.ItemTemplates.Clear();
                var template = IsHtmlText
                    ? HtmlTextCellType
                    : PlainTextCellType;
                _basePicker.ItemTemplates.Add(typeof(string), template);
                _basePicker.ItemsSource = ItemsSource;
            }
            //else if (propertyName == BackgroundColorProperty.PropertyName)
            //    BackgroundColor = Color.Red;
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
            /*
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
                {
                    if (Index > -1)
                        SelectedItem = ItemsSource[Index];
                    else
                        SelectedItem = null;
                }
            }
            */

        }


        #region Cell Template
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class SinglePickerHtmlCellContentView : SinglePickerCellContentView
        {
            protected override void OnBindingContextChanged()
            {
                if (!P42.Utils.Environment.IsOnMainThread)
                {
                    Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                    return;
                }

                base.OnBindingContextChanged();

                if (BindingContext is IHtmlString htmlObject)
                    itemLabel.HtmlText = htmlObject.ToHtml();
                else if (BindingContext != null)
                    itemLabel.HtmlText = BindingContext?.ToString();
                else
                    itemLabel.HtmlText = itemLabel.Text = null;
            }
        }

        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class SinglePickerCellContentView : Xamarin.Forms.Grid, ICellContentView, IIsSelectedAble, IDisposable
        {
            #region Properties
            public double CellHeight { get; set; }

            public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SinglePickerCellContentView), default(bool));
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
                
                var vsgs = VisualStateManager.GetVisualStateGroups(this);

                if (!vsgs.Any())
                {
                    var vsg = new VisualStateGroup
                    {
                        Name = "CommonStates",

                    };
                    vsg.States.Add(new VisualState
                    {
                        Name = "Normal"
                    });

                    var selectedState = new VisualState
                    {
                        Name = "Selected",
                        TargetType = GetType(),
                    };
                    selectedState.Setters.Add(new Setter
                    {
                        Property = BackgroundColorProperty,
                        Value = Color.LightGray
                    });
                    vsg.States.Add(selectedState);
                    vsgs.Add(vsg);
                }
                /*
                foreach (var vsg in vsgs)
                {

                    System.Diagnostics.Debug.WriteLine("=========================");

                    System.Diagnostics.Debug.WriteLine("vsg.Name: " + vsg.Name);
                    System.Diagnostics.Debug.WriteLine("vsg.CurrentState: " + vsg.CurrentState);
                    System.Diagnostics.Debug.WriteLine("-------");

                    foreach (var vs in vsg.States)
                    {
                        System.Diagnostics.Debug.WriteLine("    vs.Name : " + vs.Name);
                        System.Diagnostics.Debug.WriteLine("    vs.TargetType : " + vs.TargetType);
                        foreach (var trigger in vs.StateTriggers)
                        {

                        }
                    }
                    System.Diagnostics.Debug.WriteLine("=========================");
                }
                */



                CellHeight = (int)BasePicker.RowHeightProperty.DefaultValue;
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
                //IgnoreChildren = true;
                ColumnSpacing = 0;

                Children.Add(itemLabel, 1, 0);

                //BackgroundColor = Color.Yellow;
            }

            private bool _disposed;
            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    _disposed = true;

                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion


            #region Change management
            protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
            {
                base.OnPropertyChanging(propertyName);
                if (propertyName == BindingContextProperty.PropertyName)
                {
                    System.Diagnostics.Debug.WriteLine("SinglePickerCellContentView");

                }
            }

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

                //VisualStateManager.CommonStates.Selected;

                var visualStateGroupName = "CommonStates";
                var myVsg = VisualStateManager.GetVisualStateGroups(this).FirstOrDefault(vsg => vsg.Name == visualStateGroupName);
            }

            #endregion


            #region Appearing / Disappearing Event Handlers
            public virtual void OnAppearing()
            {
                System.Diagnostics.Debug.WriteLine("SinglePickerCellContentView");

            }

            public virtual void OnDisappearing() { }
            #endregion



        }
        #endregion

    }




}
