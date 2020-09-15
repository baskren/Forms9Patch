using System;
using System.ComponentModel;
using System.Collections.Generic;
using P42.Utils;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace Forms9Patch
{
    /// <summary>
    /// Multi component picker.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class MultiComponentPicker : Xamarin.Forms.ContentView
    {
        #region Properties
        /// <summary>
        /// The components property.
        /// </summary>
        public static readonly BindableProperty ComponentsProperty = BindableProperty.Create(nameof(Components), typeof(ObservableCollection<ObservableCollection<object>>), typeof(MultiComponentPicker), null);
        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <value>The components.</value>
        public ObservableCollection<ObservableCollection<object>> Components
        {
            get => (ObservableCollection<ObservableCollection<object>>)GetValue(ComponentsProperty);
            set => SetValue(ComponentsProperty, value);
        }

        /// <summary>
        /// The row sizes property.
        /// </summary>
        public static readonly BindableProperty RowSizesProperty = BindableProperty.Create(nameof(RowSizes), typeof(List<double>), typeof(MultiComponentPicker), null);
        /// <summary>
        /// Gets or sets the row sizes.
        /// </summary>
        /// <value>The row sizes.</value>
        public List<double> RowSizes
        {
            get => (List<double>)GetValue(RowSizesProperty);
            set => SetValue(RowSizesProperty, value);
        }

        /// <summary>
        /// The row height property.
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(MultiComponentPicker), -1);
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


        #region selection changed event
        /// <summary>
        /// Selection changed event arguments.
        /// </summary>
        public class SelectionChangedEventArgs : EventArgs
        {
            /// <summary>
            /// The category.
            /// </summary>
            public int Category;
        }

        WeakEventManager _eventManager;
        /// <summary>
        /// Occurs when selection changed.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged
        {
            add
            {
                if (_eventManager == null)
                    _eventManager = WeakEventManager.GetWeakEventManager(this);
                _eventManager.AddEventHandler(nameof(SelectionChanged), value);
            }
            remove
            {
                _eventManager?.RemoveEventHandler(nameof(SelectionChanged), value);
            }
        }
        #endregion


        #region Fields
        //List<Xamarin.Forms.ListView> _listViews = new List<ListView>();
        //ObservableCollection<ObservableCollection<string>> _components = new ObservableCollection<ObservableCollection<string>>();
        //Grid _grid = new Grid();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.MultiComponentPicker"/> class.
        /// </summary>
        public MultiComponentPicker() { }


        #region Change management
        /*
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == ComponentsProperty.PropertyName && Components != null)
			{
				for (int i = 0; i < Components.Count; i++)
					_listViews[i].BindingContext = null;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == ComponentsProperty.PropertyName && Components != null)
			{
				for (int i = 0; i < Components.Count; i++)
				{
					if (_listViews.Count <= i)
						_listViews.Add(new ListView());
					_listViews[i].BindingContext = null;
				}
			}

		}
*/
        #endregion
    }



}

