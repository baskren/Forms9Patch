using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
	/// <summary>
	/// Multi picker.
	/// </summary>
	public class MultiPicker : SinglePicker
	{
		#region Properties
		/// <summary>
		/// The selected items property.
		/// </summary>
		public static readonly BindablePropertyKey SelectedItemsPropertyKey = BindableProperty.CreateReadOnly("SelectedItems", typeof(ObservableCollection<object>), typeof(MultiPicker), null);
		/// <summary>
		/// Gets or sets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public ObservableCollection<object> SelectedItems
		{
			get { return (ObservableCollection<object>)GetValue(SelectedItemsPropertyKey.BindableProperty); }
			private set { SetValue(SelectedItemsPropertyKey, value); }
		}

		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.MultiPicker"/> class.
		/// </summary>
		public MultiPicker()
		{
			//SelectedItems = new ObservableCollection<object>();
			_lowerGradient.StartColor = _overlayColor.WithAlpha(0);
			_upperGradient.EndColor = _overlayColor.WithAlpha(0);
			_basePicker.SelectBy = SelectBy.Default;
			_manLayout.Children.Remove(_lowerEdge);
			_manLayout.Children.Remove(_upperEdge);
			_basePicker.GroupToggleBehavior = GroupToggleBehavior.Multiselect;
			_basePicker.StartAccessory = new CellAccessory();
			_basePicker.StartAccessory.HorizontalAlignment = TextAlignment.End;
			_basePicker.StartAccessory.TextFunction = (IItem arg) => arg.IsSelected ? "✓" : "";

			SelectedItems = _basePicker.SelectedItems;

			SelectedItems.CollectionChanged += (sender, e) => OnPropertyChanged(SelectedItemsPropertyKey.BindableProperty.PropertyName);
		}
		#endregion


	}
}

