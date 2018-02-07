using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Item.
	/// </summary>
	abstract class ItemWrapper : BindableObject, IItemWrapper {


		#region Fields
		public readonly int ID;
		#endregion


		#region Properties
		
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
		public static readonly BindableProperty RequestedSeparatorHeightProperty = BindableProperty.Create("RequestedSeparatorHeight", typeof(int), typeof(ItemWrapper), 1);
		/// <summary>
		/// Gets or sets the height of the separator.
		/// </summary>
		/// <value>The height of the separator.</value>
		public int RequestedSeparatorHeight
		{
			get { return (int)GetValue(RequestedSeparatorHeightProperty); }
			set {  SetValue(RequestedSeparatorHeightProperty, value); }
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

        #region CellView convenience property
        public View CellView
		{
			get { return BaseCellView.ContentView; }
		}
        #endregion

        #region RequestedRowHeight
        public static readonly BindableProperty RequestedRowHeightProperty = BindableProperty.Create("RequestedRowHeight", typeof(int), typeof(ItemWrapper), 40);
        public int RequestedRowHeight
		{
			get { return (int)GetValue(RequestedRowHeightProperty); }
			set { SetValue(RequestedRowHeightProperty, value); }
		}
        #endregion


        #region RenderedRowHeight property
        /// <summary>
        /// backing store for RenderedRowHeight property
        /// </summary>
        public static readonly BindableProperty RenderedRowHeightProperty = BindableProperty.Create("RenderedRowHeight", typeof(int), typeof(ItemWrapper), -1);
        /// <summary>
        /// Gets/Sets the RenderedRowHeight property
        /// </summary>
        public int RenderedRowHeight
        {
            get { return (int)GetValue(RenderedRowHeightProperty); }
            set { SetValue(RenderedRowHeightProperty, value); }
        }
        #endregion RenderedRowHeight property



        #region Parent property
        public static readonly BindableProperty ParentProperty = BindableProperty.Create("Parent", typeof(GroupWrapper), typeof(ItemWrapper), default(GroupWrapper));
		public GroupWrapper Parent
		{
			get { return (GroupWrapper)GetValue(ParentProperty); }
			set { SetValue(ParentProperty, value); }
		}
        #endregion



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
			
			SeparatorIsVisible = other.SeparatorIsVisible;
			SeparatorColor = other.SeparatorColor;
			RequestedSeparatorHeight = other.RequestedSeparatorHeight;
			SeparatorLeftIndent = other.SeparatorLeftIndent;
			SeparatorRightIndent = other.SeparatorRightIndent;
			
			CellBackgroundColor = other.CellBackgroundColor;
			SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;

			Source = other.Source;
		}

		public string Description ()
		{
			return string.Format("{0}[{1}]",GetType().Name,ID);
		}

        public bool IsLastChild
        {
            get
            {
                if (Parent is GroupWrapper parent)
                {
                    return Index == Parent.Count - 1;
                }
                return false;
            }
        }

        public bool ShouldRenderSeparator => SeparatorIsVisible && !IsLastChild;

        public int SeparatorHeight => ShouldRenderSeparator ? RequestedSeparatorHeight : 0;

        public int BestGuessItemRowHeight()
        {
            if (RenderedRowHeight >= 0)
                return RenderedRowHeight;
            if (RequestedRowHeight >= 0)
                return RequestedRowHeight;
            if (Parent is GroupWrapper parent && parent.RequestedRowHeight >= 0)
                return parent.RequestedRowHeight;
            return 40;
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

        internal void SignalPropertyChanged(string propertyName=null)
        {
            OnPropertyChanged(propertyName);
        }

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsSelectedProperty.PropertyName)
			{
                if (Source is IIsSelectedAble isSelectedSource)
                    isSelectedSource.IsSelected = IsSelected;
            }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
            if (BindingContext is IItemWrapper iItemWrapper)
            {
                CellBackgroundColor = iItemWrapper.CellBackgroundColor;
                SelectedCellBackgroundColor = iItemWrapper.SelectedCellBackgroundColor;
                RequestedRowHeight = iItemWrapper.RequestedRowHeight;

                SeparatorIsVisible = iItemWrapper.SeparatorIsVisible;
                SeparatorColor = iItemWrapper.SeparatorColor;
                RequestedSeparatorHeight = iItemWrapper.RequestedSeparatorHeight;
                SeparatorLeftIndent = iItemWrapper.SeparatorLeftIndent;
                SeparatorRightIndent = iItemWrapper.SeparatorRightIndent;

            }
        }
		#endregion
	}


}


