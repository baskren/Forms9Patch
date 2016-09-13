using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Item.
	/// </summary>
	abstract class Item : BindableObject, IItem {


		#region Fields
		protected static bool debugProperties = false;
		public readonly int ID;
		#endregion


		#region Properties

		#region Separator
		public static readonly BindableProperty SeparatorIsVisibleProperty  = BindableProperty.Create("SeparatorIsVisible",  typeof(bool), typeof(Item), true);
		public bool SeparatorIsVisible {
			get { 
				return (bool)GetValue(SeparatorIsVisibleProperty); 
			}
			internal set { 
				SetValue (SeparatorIsVisibleProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorIsVisible=["+value+"]");
			}
		}		

		public static readonly BindableProperty SeparatorColorProperty  = BindableProperty.Create("SeparatorColor",  typeof(Color), typeof(Item), Color.FromRgba(0,0,0,0.12));
		public Color SeparatorColor {
			get { return (Color)GetValue(SeparatorColorProperty); }
			internal set { 
				SetValue (SeparatorColorProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorColor=["+value+"]");
			}
		}

		/// <summary>
		/// The separator height property.
		/// </summary>
		public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create("SeparatorHeight", typeof(double), typeof(Item), 1.0);
		/// <summary>
		/// Gets or sets the height of the separator.
		/// </summary>
		/// <value>The height of the separator.</value>
		public double SeparatorHeight
		{
			get { return (double)GetValue(SeparatorHeightProperty); }
			set { 
				SetValue(SeparatorHeightProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorHeight=["+value+"]");
			}
		}

		/// <summary>
		/// The separator left indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorLeftIndentProperty = BindableProperty.Create("SeparatorLeftIndent", typeof(double), typeof(Item), 20.0);
		/// <summary>
		/// Gets or sets the separator left indent.
		/// </summary>
		/// <value>The separator left indent.</value>
		public double SeparatorLeftIndent
		{
			get { return (double)GetValue(SeparatorLeftIndentProperty); }
			set { 
				SetValue(SeparatorLeftIndentProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorLeftIndent=["+value+"]");
			}
		}

		/// <summary>
		/// The separator right indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorRightIndentProperty = BindableProperty.Create("SeparatorRightIndent", typeof(double), typeof(Item), 0.0);
		/// <summary>
		/// Gets or sets the separator right indent.
		/// </summary>
		/// <value>The separator right indent.</value>
		public double SeparatorRightIndent
		{
			get { return (double)GetValue(SeparatorRightIndentProperty); }
			set { 
				SetValue(SeparatorRightIndentProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorRightIndent=["+value+"]");
			}
		}
		#endregion

		#region Background
		public static readonly BindableProperty CellBackgroundColorProperty  = BindableProperty.Create("CellBackgroundColor",  typeof(Color), typeof(Item), Color.Transparent);
		public Color CellBackgroundColor {
			get { return (Color)GetValue(CellBackgroundColorProperty); }
			internal set { 
				SetValue (CellBackgroundColorProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.CellBackgroundColor=["+value+"]");
			}
		}	

		public static readonly BindableProperty SelectedCellBackgroundColorProperty = BindableProperty.Create("SelectedCellBackgroundColor", typeof(Color), typeof(Item), Color.Gray);
		public Color SelectedCellBackgroundColor
		{
			get { return (Color)GetValue(SelectedCellBackgroundColorProperty); }
			set { 
				SetValue(SelectedCellBackgroundColorProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SelectedCellBackgroundColor=["+value+"]");
			}
		}
		#endregion

		#region Accessory
		public static readonly BindableProperty AccessoryPositionProperty = BindableProperty.Create("AccessoryPosition", typeof(AccessoryPosition), typeof(Item), AccessoryPosition.None);
		public AccessoryPosition AccessoryPosition
		{
			get { return (AccessoryPosition)GetValue(AccessoryPositionProperty); }
			set { SetValue(AccessoryPositionProperty, value); }
		}

		public static readonly BindableProperty AccessoryTextProperty = BindableProperty.Create("AccessoryText", typeof(Func<Item, string>), typeof(Item), null);
		public Func<IItem, string> AccessoryText
		{
			get { return (Func<IItem, string>)GetValue(AccessoryTextProperty); }
			set { SetValue(AccessoryTextProperty, value); }
		}

		public static readonly BindableProperty AccessoryWidthProperty = BindableProperty.Create("AccessoryWidth", typeof(double), typeof(Item), 20.0);
		public double AccessoryWidth
		{
			get { return (double)GetValue(AccessoryWidthProperty); }
			set { SetValue(AccessoryWidthProperty, value); }
		}

		public static readonly BindableProperty AccessoryHorizonatalAlignmentProperty = BindableProperty.Create("AccessoryHorizontalAlignment", typeof(TextAlignment), typeof(Item), TextAlignment.Center);
		public TextAlignment AccessoryHorizontalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryHorizonatalAlignmentProperty); }
			set { SetValue(AccessoryHorizonatalAlignmentProperty, value); }
		}

		public static readonly BindableProperty AccessoryVerticalAlignmentProperty = BindableProperty.Create("AccessoryVerticalAlignment", typeof(TextAlignment), typeof(Item), TextAlignment.Center);
		public TextAlignment AccessoryVerticalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryVerticalAlignmentProperty); }
			set { SetValue(AccessoryVerticalAlignmentProperty, value); }
		}
		#endregion


		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(Item), false);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(object), typeof(Item), null);
		public object Source
		{
			get { return GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(Item), -1);
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}
		#endregion


		#region Constructors
		static int instantiations;

		protected Item() {
			ID = instantiations++;

			this.SetBinding(SeparatorIsVisibleProperty, SeparatorIsVisibleProperty.PropertyName);
			this.SetBinding(SeparatorColorProperty, SeparatorColorProperty.PropertyName);
			this.SetBinding(SeparatorHeightProperty, SeparatorHeightProperty.PropertyName);
			this.SetBinding(SeparatorLeftIndentProperty, SeparatorLeftIndentProperty.PropertyName);
			this.SetBinding(SeparatorRightIndentProperty, SeparatorRightIndentProperty.PropertyName);

			this.SetBinding(CellBackgroundColorProperty, CellBackgroundColorProperty.PropertyName);
			this.SetBinding(SelectedCellBackgroundColorProperty, SelectedCellBackgroundColorProperty.PropertyName);

			this.SetBinding(AccessoryPositionProperty, AccessoryPositionProperty.PropertyName);
			this.SetBinding(AccessoryTextProperty, AccessoryTextProperty.PropertyName);
			this.SetBinding(AccessoryWidthProperty, AccessoryWidthProperty.PropertyName);
			this.SetBinding(AccessoryHorizonatalAlignmentProperty, AccessoryHorizonatalAlignmentProperty.PropertyName);
			this.SetBinding(AccessoryVerticalAlignmentProperty, AccessoryVerticalAlignmentProperty.PropertyName);
		}

		#endregion


		#region Convenience
		internal void ShallowCopy(Item other)
		{
			SeparatorIsVisible = other.SeparatorIsVisible;
			SeparatorColor = other.SeparatorColor;
			SeparatorHeight = other.SeparatorHeight;
			SeparatorLeftIndent = other.SeparatorLeftIndent;
			SeparatorRightIndent = other.SeparatorRightIndent;

			CellBackgroundColor = other.CellBackgroundColor;
			SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;

			AccessoryPosition = other.AccessoryPosition;
			AccessoryText = other.AccessoryText;

			Source = other.Source;
		}

		public string Description ()
		{
			return string.Format("{0}[{1}]",GetType().Name,ID);
		}
		#endregion


		#region Reference to CellView bound to Item
		internal WeakReference _weakBaseCellView;
		internal BaseCellView BaseCellView {
			get { return (BaseCellView)(_weakBaseCellView != null && _weakBaseCellView.IsAlive ? _weakBaseCellView.Target : null); }
			set { _weakBaseCellView = (value==null ? null : new WeakReference (value)); }
		}
		#endregion


		#region Property change management
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsSelectedProperty.PropertyName)
			{
				var isSelectedSource = Source as IIsSelectedAble;
				if (isSelectedSource != null)
					isSelectedSource.IsSelected = IsSelected;
			}
		}
		#endregion
	}
}

