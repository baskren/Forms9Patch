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

			_basePicker.ItemTemplates.RemoveFactoryDefaults();
			_basePicker.ItemTemplates.Add(typeof(string), typeof(MultiPickerCellContentView));

			SelectedItems = _basePicker.SelectedItems;

			SelectedItems.CollectionChanged += (sender, e) => OnPropertyChanged(SelectedItemsPropertyKey.BindableProperty.PropertyName);
		}
		#endregion


	}



	#region Cell Template
	class MultiPickerCellContentView : Grid, ICellHeight, IIsSelectedAble
	{
		public double CellHeight { get; set; }

		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(MultiPickerCellContentView), default(bool));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		#endregion

		#region Fields
		readonly Label checkLabel = new Label
		{
			Text = "✓",
			TextColor = Color.Blue,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalTextAlignment = TextAlignment.Center,
			IsVisible = false
		};
		readonly Label itemLabel = new Label
		{
			TextColor = Color.Black,
			VerticalTextAlignment = TextAlignment.Center,
			HorizontalTextAlignment = TextAlignment.Start
		};

		#endregion

		#region Constructors
		public MultiPickerCellContentView()
		{
			CellHeight = 50;
			Padding = new Thickness(5, 1, 5, 1);
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = new GridLength(30,GridUnitType.Absolute)},
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
			};
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition { Height = new GridLength(CellHeight - Padding.VerticalThickness, GridUnitType.Absolute)}
			};
			IgnoreChildren = true;
			ColumnSpacing = 0;

			Children.Add(itemLabel, 1, 0);
			Children.Add(checkLabel, 0, 0);
		}
		#endregion


		#region Change management
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
				itemLabel.Text = BindingContext.ToString();
			else
				itemLabel.Text = null;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsSelectedProperty.PropertyName)
				checkLabel.IsVisible = IsSelected;
		}

		#endregion
	}
}

