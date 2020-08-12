using Xamarin.Forms;
using System;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// FormsDragNDropListView Item.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    abstract class ItemWrapper : BindableObject, IItemWrapper
    {


        #region Fields
        public readonly int ID;
        #endregion


        #region Properties

        #region RowHeight Properties
        public static readonly BindableProperty RequestedRowHeightProperty = BindableProperty.Create(nameof(RequestedRowHeight), typeof(double), typeof(ItemWrapper), 40.0);
        public double RequestedRowHeight
        {
            get => (double)GetValue(RequestedRowHeightProperty);
            set => SetValue(RequestedRowHeightProperty, value);
        }

        /// <summary>
        /// backing store for RenderedRowHeight property
        /// </summary>
        public static readonly BindableProperty RenderedRowHeightProperty = BindableProperty.Create(nameof(RenderedRowHeight), typeof(double), typeof(ItemWrapper), -1.0);
        /// <summary>
        /// Gets/Sets the RenderedRowHeight property
        /// </summary>
        public double RenderedRowHeight
        {
            get => (double)GetValue(RenderedRowHeightProperty);
            set => SetValue(RenderedRowHeightProperty, value);
        }
        #endregion RowHeight properties

        #region Separator properties
        /// <summary>
        /// The separator visibility property.
        /// </summary>
        public static readonly BindableProperty SeparatorVisibilityProperty = Xamarin.Forms.ListView.SeparatorVisibilityProperty; //BindableProperty.Create("SeparatorVisibility", typeof(Xamarin.Forms.SeparatorVisibility), typeof(ItemWrapper), true);
        /// <summary>
        /// Gets or sets the separator visibility.
        /// </summary>
        /// <value>The separator visibility.</value>
        public Xamarin.Forms.SeparatorVisibility SeparatorVisibility
        {
            get => (Xamarin.Forms.SeparatorVisibility)GetValue(SeparatorVisibilityProperty);
            internal set => SetValue(SeparatorVisibilityProperty, value);
        }

        /// <summary>
        /// The separator color property.
        /// </summary>
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(ItemWrapper), Color.FromRgba(0, 0, 0, 0.12));
        /// <summary>
        /// Gets or sets the color of the separator.
        /// </summary>
        /// <value>The color of the separator.</value>
        public Color SeparatorColor
        {
            get => (Color)GetValue(SeparatorColorProperty);
            internal set => SetValue(SeparatorColorProperty, value);
        }

        /// <summary>
        /// The separator height property.
        /// </summary>
        public static readonly BindableProperty RequestedSeparatorHeightProperty = BindableProperty.Create(nameof(RequestedSeparatorHeight), typeof(double), typeof(ItemWrapper), 1.0);
        /// <summary>
        /// Gets or sets the height of the separator.
        /// </summary>
        /// <value>The height of the separator.</value>
        public double RequestedSeparatorHeight
        {
            get => (double)GetValue(RequestedSeparatorHeightProperty);
            set => SetValue(RequestedSeparatorHeightProperty, value);
        }

        /// <summary>
        /// The separator left indent property.
        /// </summary>
        public static readonly BindableProperty SeparatorLeftIndentProperty = BindableProperty.Create(nameof(SeparatorLeftIndent), typeof(double), typeof(ItemWrapper), 20.0);
        /// <summary>
        /// Gets or sets the separator left indent.
        /// </summary>
        /// <value>The separator left indent.</value>
        public double SeparatorLeftIndent
        {
            get => (double)GetValue(SeparatorLeftIndentProperty);
            set => SetValue(SeparatorLeftIndentProperty, value);
        }

        /// <summary>
        /// The separator right indent property.
        /// </summary>
        public static readonly BindableProperty SeparatorRightIndentProperty = BindableProperty.Create(nameof(SeparatorRightIndent), typeof(double), typeof(ItemWrapper), 0.0);
        /// <summary>
        /// Gets or sets the separator right indent.
        /// </summary>
        /// <value>The separator right indent.</value>
        public double SeparatorRightIndent
        {
            get => (double)GetValue(SeparatorRightIndentProperty);
            set => SetValue(SeparatorRightIndentProperty, value);
        }
        #endregion

        #region Background properties
        public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(ItemWrapper), Color.Transparent);
        public Color CellBackgroundColor
        {
            get => (Color)GetValue(CellBackgroundColorProperty);
            internal set => SetValue(CellBackgroundColorProperty, value);
        }

        public static readonly BindableProperty SelectedCellBackgroundColorProperty = BindableProperty.Create(nameof(SelectedCellBackgroundColor), typeof(Color), typeof(ItemWrapper), Color.Gray);
        public Color SelectedCellBackgroundColor
        {
            get => (Color)GetValue(SelectedCellBackgroundColorProperty);
            set => SetValue(SelectedCellBackgroundColorProperty, value);
        }
        #endregion

        #region IsSelected
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ItemWrapper), false);
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            internal set => SetValue(IsSelectedProperty, value);
        }
        #endregion IsSelected property

        #region Source
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(object), typeof(ItemWrapper), null);
        public object Source
        {
            get => GetValue(SourceProperty);
            internal set => SetValue(SourceProperty, value);
        }
        #endregion

        #region Index
        public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(ItemWrapper), -1);
        public int Index
        {
            get => (int)GetValue(IndexProperty);
            internal set => SetValue(IndexProperty, value);
        }
        #endregion

        #region CellView
        public View CellView => BaseCellView?.ContentView;
        #endregion

        #region Parent
        public static readonly BindableProperty ParentProperty = BindableProperty.Create(nameof(Parent), typeof(GroupWrapper), typeof(ItemWrapper), default(GroupWrapper));
        public GroupWrapper Parent
        {
            get => (GroupWrapper)GetValue(ParentProperty);
            set => SetValue(ParentProperty, value);
        }
        #endregion

        WeakReference _listViewWeakRef;
        public ListView ListView
        {
            get => _listViewWeakRef?.Target as ListView;
            set => _listViewWeakRef = new WeakReference(value, false);
        }

        #endregion


        #region Events
        // used by BaseCellView to communicate touch events to ListView via Group.
        public event EventHandler<ItemWrapperTapEventArgs> Tapped;
        public event EventHandler<ItemWrapperLongPressEventArgs> LongPressing;
        public event EventHandler<ItemWrapperLongPressEventArgs> LongPressed;
        public event EventHandler<SwipeMenuItemTappedArgs> SwipeMenuItemTapped;
        public event EventHandler<ItemWrapperPanEventArgs> Panning;
        public event EventHandler<ItemWrapperPanEventArgs> Panned;

        internal void OnTapped(object sender, ItemWrapperTapEventArgs e) => Tapped?.Invoke(sender, e);

        internal void OnLongPressing(object sender, ItemWrapperLongPressEventArgs e) => LongPressing?.Invoke(sender, e);

        internal void OnLongPressed(object sender, ItemWrapperLongPressEventArgs e) => LongPressed?.Invoke(sender, e);

        internal void OnSwipeMenuItemTapped(object sender, SwipeMenuItemTappedArgs e) => SwipeMenuItemTapped?.Invoke(sender, e);

        internal void OnPanning(object sender, ItemWrapperPanEventArgs e) => Panning?.Invoke(sender, e);

        internal void OnPanned(object sender, ItemWrapperPanEventArgs e) => Panned?.Invoke(sender, e);
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
            #region RowHeighty
            RequestedRowHeight = other.RequestedRowHeight;
            RenderedRowHeight = other.RenderedRowHeight;
            #endregion

            #region Separator
            SeparatorVisibility = other.SeparatorVisibility;
            SeparatorColor = other.SeparatorColor;
            RequestedSeparatorHeight = other.RequestedSeparatorHeight;
            SeparatorLeftIndent = other.SeparatorLeftIndent;
            SeparatorRightIndent = other.SeparatorRightIndent;
            #endregion

            #region Background
            CellBackgroundColor = other.CellBackgroundColor;
            SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;
            #endregion


            Source = other.Source;
        }

        public string Description()
        {
            return string.Format("{0}[{1}]", GetType().Name, ID);
        }

        internal static readonly BindableProperty IsLastItemProperty = BindableProperty.Create(nameof(IsLastItem), typeof(bool), typeof(ItemWrapper), false);
        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            set { SetValue(IsLastItemProperty, value); }
        }

        /*
        public bool ShouldRenderSeparator => SeparatorVisibility != SeparatorVisibility.None && !IsLastItem;
               
        public double RenderedSeparatorHeight
        {
            get
            {
                var result = ShouldRenderSeparator ? RequestedSeparatorHeight : 0;
                //System.Diagnostics.Debug.WriteLine("RendereredSeparatorHeight SeparatorVisibiity[" + SeparatorVisibility + "] IsLastChild[" + IsLastChild + "] [" + RequestedSeparatorHeight + "] => [" + result + "]");
                return result;
            }
        }
        */

        public double BestGuessItemRowHeight()
        {
            if (RenderedRowHeight >= 0)
                return RenderedRowHeight;
            if (RequestedRowHeight >= 0)
                return RequestedRowHeight;
            if (Parent is GroupWrapper parent && parent.RequestedRowHeight >= 0)
                return parent.RequestedRowHeight;
            return 40.0;
        }
        #endregion


        #region Reference to CellView bound to Item
        internal WeakReference _weakBaseCellView;
        internal BaseCellView BaseCellView
        {
            get => (BaseCellView)(_weakBaseCellView != null && _weakBaseCellView.IsAlive ? _weakBaseCellView.Target : null);
            set => _weakBaseCellView = (value == null ? null : new WeakReference(value));
        }
        #endregion


        #region Property change management

        internal void SignalPropertyChanged(string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            try
            {
                base.OnPropertyChanged(propertyName);
            }
            catch (Exception) { }

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

                SeparatorVisibility = iItemWrapper.SeparatorVisibility;
                SeparatorColor = iItemWrapper.SeparatorColor;
                RequestedSeparatorHeight = iItemWrapper.RequestedSeparatorHeight;
                SeparatorLeftIndent = iItemWrapper.SeparatorLeftIndent;
                SeparatorRightIndent = iItemWrapper.SeparatorRightIndent;

            }
        }
        #endregion
    }


}


