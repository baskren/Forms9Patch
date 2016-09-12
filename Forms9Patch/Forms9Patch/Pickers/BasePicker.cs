using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FormsGestures;
using System.Runtime.Serialization;
namespace Forms9Patch
{
	public class BasePicker : ContentView
	{
		#region Properties
		public Forms9Patch.DataTemplateSelector ItemTemplate
		{
			get { return _listView.ItemTemplates; }
			//set { SetValue(ItemTemplateProperty, value); }
		}


		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BasePicker), null);
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}


		public static readonly BindableProperty RowHeightProperty = BindableProperty.Create("RowHeight", typeof(int), typeof(BasePicker), 30);
		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(BasePicker), 0);
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		#endregion


		#region Fields
		ObservableCollection<object> _col = new ObservableCollection<object>();

		readonly Forms9Patch.ListView _listView = new Forms9Patch.ListView
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

		readonly Xamarin.Forms.AbsoluteLayout _absLayout = new Xamarin.Forms.AbsoluteLayout()
		{
		};

		readonly BoxView _boxView = new BoxView()
		{
			BackgroundColor = Color.Gray
		};
		#endregion


		#region Constructor
		bool _processingSelection;

		public BasePicker()
		{
			BackgroundColor = Color.FromRgba(0.5,0.5,0.5,0.125);

			_listView.BindingContext = this;
			_listView.SetBinding(ListView.RowHeightProperty, "RowHeight");
			_listView.BackgroundColor = Color.Transparent;
			_listView.SelectedCellBackgroundColor = Color.Transparent;

			_listView.ItemAppearing += OnCellAppearing;

			//Content = _listView;
			AbsoluteLayout.SetLayoutFlags(_listView,AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(_boxView,AbsoluteLayoutFlags.WidthProportional| AbsoluteLayoutFlags.YProportional);
			AbsoluteLayout.SetLayoutBounds(_listView, new Rectangle(0,0,1,1));
			AbsoluteLayout.SetLayoutBounds(_boxView, new Rectangle(0,0.5,1,30));
			_absLayout.Children.Add(_boxView);
			_absLayout.Children.Add(_listView);
			Content = _absLayout;

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
				var indexPath = Forms9Patch.ListViewExtensions.IndexPathAtPoint(_listView,point);
				if (indexPath != null)
				{
					Index = indexPath.Item2;
					//System.Diagnostics.Debug.WriteLine("Tapped point=["+e.Touches[0]+"] indexPath=[" + indexPath + "] width=["+_listView.Width+"] height=["+_listView.Height+"]");
					ScrollToIndex(Index);
				}
			};

			_listView.TranslationY = Device.OnPlatform<double>(0, -7, 0);
			_listView.Header = _upperPadding;
			_listView.Footer = _lowerPadding;
		}
		#endregion


		#region Property Change management
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

		void ScrollToIndex(int index)
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
			//System.Diagnostics.Debug.WriteLine("Listener(_listView).Panned");
			if (!_waitingForScrollToComplete)
			{
				_waitingForScrollToComplete = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(50), () => 
				{
					if (DateTime.Now - _lastAppearance > TimeSpan.FromMilliseconds(350))
					{
						var indexPath = Forms9Patch.ListViewExtensions.IndexPathAtCenter(_listView);
						if (indexPath == null)
						{
							/*
							if (_listView.HitTest(_listView.Bounds.Center, _lowerPadding))
								Index = _col.Count - 1;
							else if (_listView.HitTest(_listView.Bounds.Center, _upperPadding))
								Index = 0;
								*/
							_waitingForScrollToComplete = false;
							return false;
						}
						else
						{
							if (indexPath.Item1 != 0)
								throw new InvalidDataContractException("SinglePicker should not be grouped");
							Index = indexPath.Item2;
						}
						ScrollToIndex(Index);
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

