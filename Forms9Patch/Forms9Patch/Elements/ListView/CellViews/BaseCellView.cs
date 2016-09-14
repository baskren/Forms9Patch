using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// DO NOT USE: Used by Forms9Patch.ListView as a foundation for cells.
	/// </summary>
	class BaseCellView : Xamarin.Forms.Grid
	{
		#region Properties
		/// <summary>
		/// The content property.
		/// </summary>
		public static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(BaseCellView), default(View));
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		#region Separator appearance
		/// <summary>
		/// The separator is visible property.
		/// </summary>
		internal static readonly BindableProperty SeparatorIsVisibleProperty = Item.SeparatorIsVisibleProperty;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.BaseCellView"/> separator is visible.
		/// </summary>
		/// <value><c>true</c> if separator is visible; otherwise, <c>false</c>.</value>
		internal bool SeparatorIsVisible {
			get { return (bool)GetValue (SeparatorIsVisibleProperty); }
			set { 
				SetValue (SeparatorIsVisibleProperty, value); 
			}
		}

		/// <summary>
		/// The separator color property.
		/// </summary>
		internal static readonly BindableProperty SeparatorColorProperty = Item.SeparatorColorProperty;
		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		internal Color SeparatorColor {
			get { return (Color)GetValue (SeparatorColorProperty); }
			set { SetValue (SeparatorColorProperty, value); }
		}

		/// <summary>
		/// The separator height property.
		/// </summary>
		public static readonly BindableProperty SeparatorHeightProperty = Item.SeparatorHeightProperty;
		/// <summary>
		/// Gets or sets the height of the separator.
		/// </summary>
		/// <value>The height of the separator.</value>
		public double SeparatorHeight {
			get { return (double)GetValue (SeparatorHeightProperty); }
			set { SetValue (SeparatorHeightProperty, value); }
		}

		/// <summary>
		/// The separator left indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorLeftIndentProperty = Item.SeparatorLeftIndentProperty;
		/// <summary>
		/// Gets or sets the separator left indent.
		/// </summary>
		/// <value>The separator left indent.</value>
		public double SeparatorLeftIndent {
			get { return (double)GetValue (SeparatorLeftIndentProperty); }
			set { SetValue (SeparatorLeftIndentProperty, value); }
		}

		/// <summary>
		/// The separator right indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorRightIndentProperty = Item.SeparatorRightIndentProperty;
		/// <summary>
		/// Gets or sets the separator right indent.
		/// </summary>
		/// <value>The separator right indent.</value>
		public double SeparatorRightIndent {
			get { return (double)GetValue (SeparatorRightIndentProperty); }
			set { SetValue (SeparatorRightIndentProperty, value); }
		}
		#endregion

		#region Accessory appearance
		public static readonly BindableProperty AccessoryPositionProperty = Item.AccessoryPositionProperty;
		public AccessoryPosition AccessoryPosition
		{
			get { return (AccessoryPosition)GetValue(AccessoryPositionProperty); }
			set { SetValue(AccessoryPositionProperty, value); }
		}

		public static readonly BindableProperty AccessoryWidthProperty = Item.AccessoryWidthProperty;
		public double AccessoryWidth
		{
			get { return (double)GetValue(AccessoryWidthProperty); }
			set { SetValue(AccessoryWidthProperty, value); }
		}

		public static readonly BindableProperty AccessoryHorizonatalAlignmentProperty = Item.AccessoryHorizonatalAlignmentProperty;
		public TextAlignment AccessoryHorizontalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryHorizonatalAlignmentProperty); }
			set { SetValue(AccessoryHorizonatalAlignmentProperty, value); }
		}

		public static readonly BindableProperty AccessoryVerticalAlignmentProperty = Item.AccessoryVerticalAlignmentProperty;
		public TextAlignment AccessoryVerticalAlignment
		{
			get { return (TextAlignment)GetValue(AccessoryVerticalAlignmentProperty); }
			set { SetValue(AccessoryVerticalAlignmentProperty, value); }
		}
		#endregion

		#endregion


		#region Fields
		static int _instances = 0;
		internal int ID;
		Label _accessory = new Label
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand
		};
		#endregion


		#region Constructor
		/// <summary>
		/// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BaseCellView"/> class.
		/// </summary>
		public BaseCellView () {
			ID = _instances++;
			Padding = new Thickness(0,1,0,1);

			this.SetBinding(SeparatorIsVisibleProperty, SeparatorIsVisibleProperty.PropertyName);
			this.SetBinding(SeparatorColorProperty, SeparatorColorProperty.PropertyName);
			this.SetBinding(SeparatorHeightProperty, SeparatorHeightProperty.PropertyName);
			this.SetBinding(SeparatorLeftIndentProperty, SeparatorLeftIndentProperty.PropertyName);
			this.SetBinding(SeparatorRightIndentProperty, SeparatorRightIndentProperty.PropertyName);

			this.SetBinding(AccessoryPositionProperty, AccessoryPositionProperty.PropertyName);
			this.SetBinding(AccessoryWidthProperty,AccessoryWidthProperty.PropertyName);
			this.SetBinding(AccessoryHorizonatalAlignmentProperty, AccessoryHorizonatalAlignmentProperty.PropertyName);
			this.SetBinding(AccessoryVerticalAlignmentProperty, AccessoryVerticalAlignmentProperty.PropertyName);

			_accessory.SetBinding(Label.HorizontalTextAlignmentProperty,AccessoryHorizonatalAlignmentProperty.PropertyName);
			_accessory.SetBinding(Label.VerticalTextAlignmentProperty,AccessoryVerticalAlignmentProperty.PropertyName);



			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = new GridLength(30, GridUnitType.Absolute) },
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength(30, GridUnitType.Absolute) }
			};
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition { Height = GridLength.Auto }
			};

		}
		#endregion


		#region change management

		/// <summary>
		/// Triggered by a change in the binding context
		/// </summary>
		protected override void OnBindingContextChanged () {
			if (BindingContext == null)
				return;
			var item = BindingContext as Item;
			if (item != null)
			{
				item.BaseCellView = this;
				item.PropertyChanged += OnItemPropertyChanged;
				UpdateUnBoundProperties();
			}
			else
				System.Diagnostics.Debug.WriteLine("");
			var type = BindingContext?.GetType ();
			if (type == typeof(NullItem) || type == typeof(BlankItem))
				Content.BindingContext = item;
			else
			{
				Content.BindingContext = item?.Source;
				//System.Diagnostics.Debug.WriteLine("item.Index=[" + item.Index + "] item.Source=[" + item.Source + "] item.SeparatorIsVisible[" + item.SeparatorIsVisible + "]");
			}
			_freshContent = true;
			System.Diagnostics.Debug.WriteLine("OnBindingContextChanged");
			base.OnBindingContextChanged();
		}

		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == BindingContextProperty.PropertyName)
			{
				var item = BindingContext as Item;
				if (item != null)
					item.PropertyChanged -= OnItemPropertyChanged;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == AccessoryWidthProperty.PropertyName)
			{
				double width = AccessoryWidth < 1 ? Width * AccessoryWidth : AccessoryWidth;
				ColumnDefinitions[0] = new ColumnDefinition { Width = new GridLength(width, GridUnitType.Absolute) };
				ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(width, GridUnitType.Absolute) };
			}
			System.Diagnostics.Debug.WriteLine("OnPropertyChanged");
			UpdateLayout();
		}


		void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Item.IsSelectedProperty.PropertyName 
			    || e.PropertyName == Item.CellBackgroundColorProperty.PropertyName 
			    || e.PropertyName == Item.SelectedCellBackgroundColorProperty.PropertyName
			    || e.PropertyName == Item.IndexProperty.PropertyName
			   )
				UpdateUnBoundProperties();
			_freshContent = (_freshContent || e.PropertyName == "Renderer" || e.PropertyName == ContentProperty.PropertyName);
			System.Diagnostics.Debug.WriteLine("OnItemPropertyChanged");
			UpdateLayout();
		}

		AccessoryPosition _lastAccPos = AccessoryPosition.None;
		bool _lastAccActive = false;
		bool _freshContent = false;
		void UpdateLayout()
		{
			var _accActive = false;
			if (AccessoryPosition != AccessoryPosition.None)
			{
				string accText = ((Item)BindingContext)?.AccessoryText?.Invoke((Item)BindingContext);
				if (accText != _accessory.HtmlText)
				{
					_accessory.HtmlText = accText;
					_accActive = _accessory.HtmlText != null;
				}
			}
			if (_accActive != _lastAccActive || AccessoryPosition != _lastAccPos || _freshContent)
			{
				//if ( _freshContent)
				//{
					if (Content != null && Children.Contains(Content))
						Children.Remove(Content);
					if (_accessory != null && Children.Contains(_accessory))
						Children.Remove(_accessory);
					if (AccessoryPosition == AccessoryPosition.None || !_accActive)
					{
						if (Content != null)
							Children.Add(Content, 0, 3, 0, 1);
					}
					else if (AccessoryPosition == AccessoryPosition.Start)
					{
						if (Content != null)
							Children.Add(Content, 1, 3, 0, 1);
						Children.Add(_accessory, 0, 0);
					}
					else
					{
						if (Content != null)
							Children.Add(Content, 0, 2, 0, 1);
						Children.Add(_accessory, 2, 0);
					}
					_lastAccPos = AccessoryPosition;
				//}
				_freshContent = false;
				_lastAccActive = _accActive;
			}
		}

		void UpdateUnBoundProperties()
		{
			var item = BindingContext as Item;
			if (item != null)
			{
				BackgroundColor = item.IsSelected ? item.SelectedCellBackgroundColor : item.CellBackgroundColor;
				//System.Diagnostics.Debug.WriteLine("BaseCellView["+ID+"].BackgroundColor=["+BackgroundColor+"]");
				SeparatorIsVisible = item.Index > 0 && item.SeparatorIsVisible;
			}
			else
			{
				BackgroundColor = Color.Transparent;
				SeparatorIsVisible = false;
			}
			//System.Diagnostics.Debug.WriteLine("[" + ID + "] SeparatorColor=[" + SeparatorColor + "] SeparatorVisibility=[" + SeparatorIsVisible + "]");
		}
		#endregion
	}
}

