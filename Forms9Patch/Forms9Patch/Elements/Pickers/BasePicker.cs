using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FormsGestures;
using System.Runtime.Serialization;
namespace Forms9Patch
{
	/// <summary>
	/// Base picker.
	/// </summary>
	public class BasePicker : ContentView
	{
		#region Properties
		/// <summary>
		/// Gets the item templates.
		/// </summary>
		/// <value>The item templates.</value>
		public DataTemplateSelector ItemTemplates
		{
			get { return _listView.ItemTemplates; }
			//set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// The items source property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BasePicker), null);
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

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
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		/// <summary>
		/// The index property.
		/// </summary>
		public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(BasePicker), 0, BindingMode.TwoWay);
		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
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
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		#endregion


		#region Fields
		ObservableCollection<object> _col = new ObservableCollection<object>();

		readonly ListView _listView = new ListView
		{
			IsGroupingEnabled = false,
			SeparatorVisibility = SeparatorVisibility.None,
			BackgroundColor = Color.Transparent
		};

		readonly BoxView _upperPadding = new BoxView
		{
			BackgroundColor = Color.Transparent
		};

		readonly BoxView _lowerPadding = new BoxView
		{
			BackgroundColor = Color.Transparent
		};

		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.BasePicker"/> class.
		/// </summary>
		public BasePicker()
		{
			BackgroundColor = Color.FromRgba(0.5,0.5,0.5,0.125);

			_listView.BindingContext = this;
			_listView.SetBinding(Xamarin.Forms.ListView.RowHeightProperty, "RowHeight");
			_listView.BackgroundColor = Color.Transparent;
			_listView.SelectedCellBackgroundColor = Color.Transparent;

			_listView.ItemAppearing += OnCellAppearing;

			Content = _listView;

			var listener = new Listener(_listView);
			listener.Panned += OnPanned;
			listener.Panning += (sender, e) =>
			{
				//System.Diagnostics.Debug.WriteLine("Listener(_listView).Panning");
				_lastAppearance = DateTime.Now;
			};
			listener.Tapped += (object sender, TapEventArgs e) => 
			{
				//System.Diagnostics.Debug.WriteLine("Listener(_listView).Tapped");
				var point = e.Touches[0];
				var indexPath = ListViewExtensions.IndexPathAtPoint(_listView,point);
				if (indexPath != null)
				{
					Index = indexPath.Item2;
					//System.Diagnostics.Debug.WriteLine("Tapped point=["+e.Touches[0]+"] indexPath=[" + indexPath + "] width=["+_listView.Width+"] height=["+_listView.Height+"]");
					ScrollToIndex(Index);
					SelectedItem = _listView.BaseItemsSource.ItemAtIndexPath(indexPath);
				}
			};

			_listView.TranslationY = Device.OnPlatform<double>(0, -7, 0);
			_listView.Header = _upperPadding;
			_listView.Footer = _lowerPadding;
		}
		#endregion


		#region Property Change management
		/// <summary>
		/// Ons the property changing.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				var notifiableCollection = ItemsSource as INotifyCollectionChanged;
				if (notifiableCollection != null)
					notifiableCollection.CollectionChanged -= SourceCollectionChanged;
				_col.Clear();
			}
		}

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				var notifiableCollection = ItemsSource as INotifyCollectionChanged;
				if (notifiableCollection != null)
					notifiableCollection.CollectionChanged += SourceCollectionChanged;
				_col = new ObservableCollection<object>((System.Collections.Generic.IEnumerable<object>)ItemsSource);
				_listView.ItemsSource = _col;
			}
			/*
			if (propertyName == ItemTemplateProperty.PropertyName)
			{
				_listView.ItemTemplate = ItemTemplate;
			}
			*/
			if (propertyName == HeightProperty.PropertyName || propertyName == RowHeightProperty.PropertyName)
			{
				_upperPadding.HeightRequest = (Height - RowHeight) / 2.0 + Device.OnPlatform(0,8,0);
				_lowerPadding.HeightRequest = (Height - RowHeight) / 2.0;
			}
		}

		/// <summary>
		/// Scrolls the index of the to.
		/// </summary>
		/// <param name="index">Index.</param>
		public void ScrollToIndex(int index)
		{
			var item = _col[index];
			_listView.ScrollTo(item, ScrollToPosition.Center, true);
		}


		void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					for (int i = 0; i < e.NewItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex,e.NewItems[i]);
					if (e.NewStartingIndex <= Index)
						Index += e.NewItems.Count;
					break;
				case NotifyCollectionChangedAction.Move:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex, e.OldItems[i]);
					if (Index >= e.OldStartingIndex && Index < e.OldStartingIndex + e.OldItems.Count)
					{
						var offset = Index - e.OldStartingIndex;
						Index = e.NewStartingIndex + offset;
					}
					else if (Index < e.OldStartingIndex && Index > e.NewStartingIndex)
						Index += e.NewItems.Count;
					else if (Index < e.NewStartingIndex && Index < e.OldStartingIndex)
						Index -= e.NewItems.Count;
					break;
				case NotifyCollectionChangedAction.Reset:
					_col.Clear();
					Index = -1;
					break;
				case NotifyCollectionChangedAction.Remove:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					if (Index >= e.OldStartingIndex + e.OldItems.Count)
						Index -= e.OldItems.Count;
					else if (Index >= e.OldStartingIndex)
						Index = e.OldStartingIndex - 1;
					break;
				case NotifyCollectionChangedAction.Replace:
					for (int i = 0; i < e.OldItems.Count; i++)
						_col.RemoveAt(e.OldStartingIndex);
					for (int i = 0; i < e.NewItems.Count; i++)
						_col.Insert(i + e.NewStartingIndex, e.NewItems[i]);
					if (Index >= e.OldStartingIndex + e.OldItems.Count)
						Index += (e.NewItems.Count - e.OldItems.Count);
					break;
			}
		}
		#endregion


		#region Snap to cell
		DateTime _lastAppearance = DateTime.Now;
		void OnCellAppearing(object sender, ItemVisibilityEventArgs e)
		{
			_lastAppearance = DateTime.Now;
		}

		bool _waitingForScrollToComplete;
		void OnPanned(object sender, PanEventArgs e)
		{
			if (!_waitingForScrollToComplete)
			{
				_waitingForScrollToComplete = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(50), () => 
				{
					if (DateTime.Now - _lastAppearance > TimeSpan.FromMilliseconds(350))
					{
						var indexPath = ListViewExtensions.IndexPathAtCenter(_listView);
						if (indexPath != null)
						{
							if (indexPath.Item1 != 0)
								throw new InvalidDataContractException("SinglePicker should not be grouped");
							Index = indexPath.Item2;
							ScrollToIndex(Index);
						}
						_waitingForScrollToComplete = false;
						return false;
					}
					return true;
				});
			}
		}

		#endregion
	}
}

