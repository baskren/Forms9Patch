using System;
using System.Collections.Generic;
using PCL.Utils;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace Forms9Patch
{
	public class MultiComponentPicker : ContentView
	{
		#region Properties
		public static readonly BindableProperty ComponentsProperty = BindableProperty.Create("Components", typeof(ObservableCollection<ObservableCollection<object>>), typeof(MultiComponentPicker), null);
		public ObservableCollection<ObservableCollection<object>> Components
		{
		    get { return (ObservableCollection<ObservableCollection<object>>)GetValue(ComponentsProperty); }
		    set { SetValue(ComponentsProperty, value); }
		}

		public static readonly BindableProperty RowSizesProperty = BindableProperty.Create("RowSizes", typeof(List<double>), typeof(MultiComponentPicker), null);
		public List<double> RowSizes
		{
			get { return (List<double>)GetValue(RowSizesProperty); }
			set { SetValue(RowSizesProperty, value); }
		}

		public static readonly BindableProperty RowHeightProperty = BindableProperty.Create("RowHeight", typeof(int), typeof(MultiComponentPicker), -1);
		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		#endregion


		#region selection changed event
		public class SelectionChangedEventArgs : EventArgs
		{
			public int Category;
		}

		WeakEventManager _eventManager;
		public event EventHandler<SelectionChangedEventArgs> SelectionChanged
		{
			add
			{
				if (_eventManager == null)
					_eventManager = WeakEventManager.GetWeakEventManager(this);
				_eventManager.AddEventHandler("SelectionChanged", value);
			}
			remove
			{
				_eventManager?.RemoveEventHandler("SelectionChanged", value);
			}
		}
		#endregion


		#region Fields
		//List<Xamarin.Forms.ListView> _listViews = new List<ListView>();
		ObservableCollection<ObservableCollection<string>> _components = new ObservableCollection<ObservableCollection<string>>();
		Grid _grid = new Grid();
		#endregion

		public MultiComponentPicker()
		{
		}


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

