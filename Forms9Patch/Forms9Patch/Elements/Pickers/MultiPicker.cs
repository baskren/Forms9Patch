using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using P42.Utils;

namespace Forms9Patch
{
    /// <summary>
    /// Multi picker.
    /// </summary>
    [DesignTimeVisible(true)]
    public class MultiPicker : SinglePicker
    {
        #region Properties
        /// <summary>
        /// The selected items property.
        /// </summary>
        public static readonly BindablePropertyKey SelectedItemsPropertyKey = BindableProperty.CreateReadOnly(nameof(SelectedItems), typeof(ObservableCollection<object>), typeof(MultiPicker), null);
        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public ObservableCollection<object> SelectedItems
        {
            get => (ObservableCollection<object>)GetValue(SelectedItemsPropertyKey.BindableProperty);
            private set => SetValue(SelectedItemsPropertyKey, value);
        }

        /// <summary>
        /// The indexes currently selected
        /// </summary>
        public List<int> SelectedIndexes
        {
            get
            {
                var result = new List<int>();
                for (int i = 0; i < ItemsSource.Count; i++)
                {
                    if (SelectedItems.Contains(ItemsSource[i]))
                        result.Add(i);
                }
                return result;
            }
        }

        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.MultiPicker"/> class.
        /// </summary>
        public MultiPicker()
        {
            PlainTextCellType = typeof(MultiPickerCellContentView);
            HtmlTextCellType = typeof(MultiPickerHtmlCellContentView);

            //SelectedItems = new ObservableCollection<object>();
            _lowerGradient.StartColor = _overlayColor.WithAlpha(0);
            _upperGradient.EndColor = _overlayColor.WithAlpha(0);
            _basePicker.SelectBy = SelectBy.Default;
            Children.Remove(_lowerEdge);
            Children.Remove(_upperEdge);
            _basePicker.GroupToggleBehavior = GroupToggleBehavior.Multiselect;

            _basePicker.ItemTemplates.RemoveFactoryDefaults();
            _basePicker.ItemTemplates.Add(typeof(string), PlainTextCellType);

            SelectedItems = _basePicker.SelectedItems;

            SelectedItems.CollectionChanged += (sender, e) => OnPropertyChanged(SelectedItemsPropertyKey.BindableProperty.PropertyName);

            _basePicker.IsSelectOnScrollEnabled = false;
        }
        #endregion


        #region Cell Template
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class MultiPickerHtmlCellContentView : MultiPickerCellContentView
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
        protected class MultiPickerCellContentView : SinglePickerCellContentView
        {
            #region Fields
            protected readonly Label checkLabel = new Label
            {
                Text = "✓",
                TextColor = Color.Blue,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                IsVisible = false
            };
            #endregion


            #region Constructors
            public MultiPickerCellContentView()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(30,GridUnitType.Absolute)},
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
            };
                Children.Add(checkLabel, 0, 0);
            }
            #endregion


            #region Change management
            protected override void OnPropertyChanged(string propertyName = null)
            {
                if (!P42.Utils.Environment.IsOnMainThread)
                {
                    Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                    return;
                }

                base.OnPropertyChanged(propertyName);

                if (propertyName == IsSelectedProperty.PropertyName)
                    checkLabel.IsVisible = IsSelected;
            }

            #endregion


            #region Appearing / Disappearing Event Handlers
            public override void OnAppearing() { }

            public override void OnDisappearing() { }
            #endregion



        }
        #endregion


    }



}

