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
		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create("SelectedItems", typeof(ObservableCollection<object>), typeof(MultiPicker), null);
		/// <summary>
		/// Gets or sets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public ObservableCollection<object> SelectedItems
		{
			get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.MultiPicker"/> class.
		/// </summary>
		public MultiPicker()
		{
			_lowerGradient.StartColor = _overlayColor.WithAlpha(0);
			_upperGradient.EndColor = _overlayColor.WithAlpha(0);
			_basePicker.SelectBy = SelectBy.Default;
			_manLayout.Children.Remove(_lowerEdge);
			_manLayout.Children.Remove(_upperEdge);
			_basePicker.AccessoryPosition = AccessoryPosition.Start;
			_basePicker.GroupToggleBehavior = GroupToggleBehavior.Multiselect;
			_basePicker.AccessoryText = (IItem arg) => arg.IsSelected ? "✓" : null;
			_basePicker.SetBinding(BasePicker.SelectedItemsProperty,"SelectedItems");
		}
		#endregion

	}
}

