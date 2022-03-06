using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using P42.Utils;
using System;

namespace Forms9Patch
{
    /// <summary>
    /// Multi picker.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class MultiPicker : SinglePicker
    {
        #region Properties

        /// <summary>
        /// The indexes currently selected
        /// </summary>
        public List<int> SelectedIndexes
        {
            get
            {
                var result = new List<int>();
                var itemsSource = ItemsSource.Cast<object>().ToArray();
                for (int i = 0; i < itemsSource.Length; i++)
                {
                    //if (SelectedItems.Contains(itemsSource[i]))
                    //if (SelectedIndexes.Contains(i))
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
            //SelectionMode = ListViewSelectionMode.

            ItemTemplates.Clear();
            ItemTemplates.Add(typeof(string), PlainTextCellType);
        }
        #endregion


        #region Cell Template
        /// <summary>
        /// MultiPicker HTML cell content view
        /// </summary>
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class MultiPickerHtmlCellContentView : MultiPickerCellContentView
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public MultiPickerHtmlCellContentView()
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
                        itemLabel.Text = htmlObject.ToHtml();
                    else if (BindingContext != null)
                        itemLabel.Text = BindingContext?.ToString();
                    else
                        itemLabel.Text = itemLabel.Text = null;
                });
            }
        }

        /// <summary>
        /// MultiPicker cell content view
        /// </summary>
        [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
        protected class MultiPickerCellContentView : SinglePickerCellContentView
        {
            #region Fields
            /// <summary>
            /// Protected use
            /// </summary>
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
            /// <summary>
            /// Constructor for MultiPicker Cell content view
            /// </summary>
            public MultiPickerCellContentView()
            {
                grid.ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(30,GridUnitType.Absolute)},
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                };
                grid.Children.Add(checkLabel, 0, 0);
            }
            #endregion


            #region Change management
            /// <summary>
            /// Same as it ever was
            /// </summary>
            protected override void OnPropertyChanged(string propertyName = null)
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {

                    base.OnPropertyChanged(propertyName);

                    if (propertyName == IsSelectedProperty.PropertyName)
                        checkLabel.IsVisible = IsSelected;
                });
            }

            #endregion

        }
        #endregion
    }
}

