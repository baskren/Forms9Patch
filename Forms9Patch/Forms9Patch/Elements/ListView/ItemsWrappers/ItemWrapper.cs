using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Item.
	/// </summary>
	abstract class ItemWrapper : BindableObject, IItemWrapper {


		#region Fields
		protected static bool debugProperties;
		public readonly int ID;
		#endregion


		#region Properties
		/*
		#region Separator
		public static readonly BindableProperty SeparatorIsVisibleProperty  = BindableProperty.Create("SeparatorIsVisible",  typeof(bool), typeof(ItemWrapper), true);
		public bool SeparatorIsVisible {
			get { 
				return (bool)GetValue(SeparatorIsVisibleProperty); 
			}
			internal set { 
				SetValue (SeparatorIsVisibleProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SeparatorIsVisible=["+value+"]");
			}
		}		

		public static readonly BindableProperty SeparatorColorProperty  = BindableProperty.Create("SeparatorColor",  typeof(Color), typeof(ItemWrapper), Color.FromRgba(0,0,0,0.12));
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
		public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create("SeparatorHeight", typeof(double), typeof(ItemWrapper), 1.0);
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
		public static readonly BindableProperty SeparatorLeftIndentProperty = BindableProperty.Create("SeparatorLeftIndent", typeof(double), typeof(ItemWrapper), 20.0);
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
		public static readonly BindableProperty SeparatorRightIndentProperty = BindableProperty.Create("SeparatorRightIndent", typeof(double), typeof(ItemWrapper), 0.0);
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
		*/

		#region Background
		public static readonly BindableProperty CellBackgroundColorProperty  = BindableProperty.Create("CellBackgroundColor",  typeof(Color), typeof(ItemWrapper), Color.Transparent);
		public Color CellBackgroundColor {
			get { return (Color)GetValue(CellBackgroundColorProperty); }
			internal set { 
				SetValue (CellBackgroundColorProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.CellBackgroundColor=["+value+"]");
			}
		}	

		public static readonly BindableProperty SelectedCellBackgroundColorProperty = BindableProperty.Create("SelectedCellBackgroundColor", typeof(Color), typeof(ItemWrapper), Color.Gray);
		public Color SelectedCellBackgroundColor
		{
			get { return (Color)GetValue(SelectedCellBackgroundColorProperty); }
			set { 
				SetValue(SelectedCellBackgroundColorProperty, value); 
				//System.Diagnostics.Debug.WriteLine("Item.SelectedCellBackgroundColor=["+value+"]");
			}
		}
		#endregion

		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(ItemWrapper), false);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			internal set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(object), typeof(ItemWrapper), null);
		public object Source
		{
			get { return GetValue(SourceProperty); }
			internal set { SetValue(SourceProperty, value); }
		}

		public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(ItemWrapper), -1);
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			internal set { SetValue(IndexProperty, value); }
		}

		public View CellView
		{
			get { return BaseCellView.Content; }
		}

		public static readonly BindableProperty RowHeightProperty = Xamarin.Forms.ListView.RowHeightProperty;
		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		public static readonly BindableProperty ParentProperty = BindableProperty.Create("Parent", typeof(GroupWrapper), typeof(ItemWrapper), default(GroupWrapper));
		public GroupWrapper Parent
		{
			get { return (GroupWrapper)GetValue(ParentProperty); }
			set { SetValue(ParentProperty, value); }
		}



		/*
		public static readonly BindableProperty HasUnevenRowsProperty = Xamarin.Forms.ListView.HasUnevenRowsProperty;
		public bool HasUnevenRows
		{
			get { return (bool)GetValue(HasUnevenRowsProperty); }
			set { SetValue(HasUnevenRowsProperty, value); }
		}
*/
		#endregion


		#region Events
		// used by BaseCellView to communicate touch events to ListView via Group.
		public event EventHandler<ItemWrapperTapEventArgs> Tapped;
		public event EventHandler<ItemWrapperLongPressEventArgs> LongPressing;
		public event EventHandler<ItemWrapperLongPressEventArgs> LongPressed;
		public event EventHandler<SwipeMenuItemTappedArgs> SwipeMenuItemTapped;

		internal void OnTapped(object sender, ItemWrapperTapEventArgs e)
		{
			Tapped?.Invoke(sender, e);
		}

		internal void OnLongPressing(object sender, ItemWrapperLongPressEventArgs e)
		{
			LongPressing?.Invoke(sender, e);
		}

		internal void OnLongPressed(object sender, ItemWrapperLongPressEventArgs e)
		{
			LongPressed?.Invoke(sender, e);
		}

		internal void OnSwipeMenuItemTapped(object sender, SwipeMenuItemTappedArgs e)
		{
			SwipeMenuItemTapped?.Invoke(sender, e);
		}

		#endregion


		#region Constructors
		static int instantiations;

		protected ItemWrapper() 
		{
			ID = instantiations++;
		}

		#endregion


		#region Convenience
		internal void ShallowCopy(ItemWrapper other)
		{
			/*
			SeparatorIsVisible = other.SeparatorIsVisible;
			SeparatorColor = other.SeparatorColor;
			SeparatorHeight = other.SeparatorHeight;
			SeparatorLeftIndent = other.SeparatorLeftIndent;
			SeparatorRightIndent = other.SeparatorRightIndent;
			*/
			CellBackgroundColor = other.CellBackgroundColor;
			SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;

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

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			var iItemWrapper = BindingContext as IItemWrapper;
			if (iItemWrapper != null)
			{
				/*
				SeparatorIsVisible = iItemWrapper.SeparatorIsVisible;
				SeparatorColor = iItemWrapper.SeparatorColor;
				SeparatorHeight = iItemWrapper.SeparatorHeight;
				SeparatorLeftIndent = iItemWrapper.SeparatorLeftIndent;
				SeparatorRightIndent = iItemWrapper.SeparatorRightIndent;
				*/
				CellBackgroundColor = iItemWrapper.CellBackgroundColor;
				SelectedCellBackgroundColor = iItemWrapper.SelectedCellBackgroundColor;
				RowHeight = iItemWrapper.RowHeight;
			}
		}
		#endregion
	}


}


