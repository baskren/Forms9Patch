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
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class SinglePicker : BasePicker
    {
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

        #region Fields
        /// <summary>
        /// Internal use
        /// </summary>
        internal protected Type PlainTextCellType = typeof(SinglePickerCellContentView);
        /// <summary>
        /// Internal Use
        /// </summary>
        internal protected Type HtmlTextCellType = typeof(SinglePickerHtmlCellContentView);
        #endregion


        static SinglePicker()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.SinglePicker"/> class.
        /// </summary>
        public SinglePicker()
        {
            ItemTemplates.Clear();
            ItemTemplates.Add(typeof(string), PlainTextCellType);

            VerticalOptions = LayoutOptions.Fill;
        }

        #region change management
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == IsHtmlTextProperty.PropertyName)
                {
                    var itemsSource = ItemsSource;
                    ItemsSource = null;
                    ItemTemplates.Clear();
                    var template = IsHtmlText
                        ? HtmlTextCellType
                        : PlainTextCellType;
                    ItemTemplates.Add(typeof(string), template);
                    ItemsSource = itemsSource;
                }
            });
        }
        #endregion

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            SelectedItem = null;
        }


        #region Cell Template
        /// <summary>
        /// Contents of a SinglePicker HTML cell
        /// </summary>
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class SinglePickerHtmlCellContentView : SinglePickerCellContentView
        {
            /// <summary>
            /// Constructor for a SinglePicker HTML cell
            /// </summary>
            public SinglePickerHtmlCellContentView()
            {
                itemLabel.TextType = TextType.Html;
            }

            /// <summary>
            /// Same as it ever was
            /// </summary>
            protected override void OnBindingContextChanged()
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    base.OnBindingContextChanged();

                    if (BindingContext is IHtmlString htmlObject)
                        itemLabel.HtmlText = htmlObject.ToHtml();
                    else if (BindingContext != null)
                        itemLabel.HtmlText = BindingContext?.ToString();
                    else
                        itemLabel.HtmlText = itemLabel.Text = null;
                });
            }
        }

        /// <summary>
        /// Contents of a SinglePicker Cell
        /// </summary>
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class SinglePickerCellContentView : ViewCell, IDisposable // Xamarin.Forms.Grid, IDisposable
        {
            #region Properties
            /// <summary>
            /// Backing store key for IsSelected property
            /// </summary>
            public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SinglePickerCellContentView), default(bool));
            /// <summary>
            /// Is this cell selected?
            /// </summary>
            public bool IsSelected
            {
                get => (bool)GetValue(IsSelectedProperty);
                set => SetValue(IsSelectedProperty, value);
            }

            #endregion


            #region Fields
            /// <summary>
            /// For internal use
            /// </summary>
            protected readonly Forms9Patch.Label itemLabel = new Forms9Patch.Label
            {
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                TextType = TextType.Text
            };

            protected readonly Grid grid = new Grid
            {
                Padding = new Thickness(5, 1, 5, 1),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                },
                ColumnSpacing = 0
            };

            #endregion


            #region Constructors
            /// <summary>
            /// Constructs a Forms9Patch.SinglePickerCellContentView
            /// </summary>
            public SinglePickerCellContentView()
            {

                var cellHeight = 40; // (int)BasePicker.RowHeightProperty.DefaultValue;
                grid.RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(cellHeight - grid.Padding.VerticalThickness, GridUnitType.Absolute)}
                };

                grid.Children.Add(itemLabel, 1, 0);

                View = grid;
            }

            private bool _disposed;
            /// <summary>
            /// Same as it ever was
            /// </summary>
            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    _disposed = true;
                    if (Parent is CollectionView collectionView)
                    {
                        collectionView.SelectionChanged -= CollectionView_SelectionChanged;
                    }

                }
            }

            /// <summary>
            /// Same as it ever was
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion


            #region Change management
            /// <summary>
            /// Same as it ever was
            /// </summary>
            protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
            {
                base.OnPropertyChanging(propertyName);
                if (propertyName == BindingContextProperty.PropertyName)
                {
                    UpdateSelected();
                }
                else if (propertyName == "Parent")
                {
                    UpdateSelected();
                    if (Parent is CollectionView collectionView)
                    {
                        collectionView.SelectionChanged -= CollectionView_SelectionChanged;
                    }
                }
            }

            /// <summary>
            /// Same as it ever was
            /// </summary>
            protected override void OnBindingContextChanged()
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {

                    base.OnBindingContextChanged();
                    if (BindingContext != null)
                        itemLabel.Text = BindingContext.ToString();
                    else
                        itemLabel.Text = null;
                    UpdateSelected();
                });
            }

            /// <summary>
            /// Same as it ever was
            /// </summary>
            /// <param name="propertyName"></param>
            protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                base.OnPropertyChanged(propertyName);
                if (propertyName == "Parent")
                {
                    UpdateSelected();
                    if (Parent is CollectionView collectionView)
                    {
                        collectionView.SelectionChanged += CollectionView_SelectionChanged;
                    }
                }
            }

            private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                UpdateSelected();
            }

            void UpdateSelected()
            {
                if (BindingContext != null && Parent is CollectionView collectionView)
                {
                    if ((collectionView.SelectedItem?.Equals(BindingContext) ?? false) || (collectionView.SelectedItems?.Contains(BindingContext) ?? false))
                    {
                        // the below works for iOS but ACTUALLY DOES THE OPOSITE for Android.  Once again, Android.
                        //VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Selected);


                        // to address the Android fail
                        grid.BackgroundColor = Color.LightGray;

                        /* or, if you want to burn some mips
                        if (VisualStateManager.GetVisualStateGroups(this).FirstOrDefault(g=>g.Name == "CommonStates") is VisualStateGroup commonStates)
                        {
                            if (commonStates.States.FirstOrDefault(s=>s.Name == VisualStateManager.CommonStates.Selected && s.TargetType == GetType()) is VisualState selectedState)
                            {
                                if (selectedState.Setters.FirstOrDefault(t => t.Property == BackgroundColorProperty) is Setter backgroundSetter)
                                    BackgroundColor = (Color)backgroundSetter.Value;
                            }
                        }
                        */

                    }
                    else
                        grid.BackgroundColor = Color.Transparent;
                }
            }
            #endregion
        }
        #endregion
    }

}
