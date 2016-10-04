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
		/// <summary>
		/// The start accessory property.
		/// </summary>
		public static readonly BindableProperty StartAccessoryProperty = Item.StartAccessoryProperty;
		/// <summary>
		/// Gets or sets the start accessory.
		/// </summary>
		/// <value>The start accessory.</value>
		public CellAccessory StartAccessory
		{
			get { return (CellAccessory)GetValue(StartAccessoryProperty); }
			set { SetValue(StartAccessoryProperty, value); }
		}

		/// <summary>
		/// The end accessory property.
		/// </summary>
		public static readonly BindableProperty EndAccessoryProperty = Item.EndAccessoryProperty;
		/// <summary>
		/// Gets or sets the end accessory.
		/// </summary>
		/// <value>The end accessory.</value>
		public CellAccessory EndAccessory
		{
			get { return (CellAccessory)GetValue(EndAccessoryProperty); }
			set { SetValue(EndAccessoryProperty, value); }
		}
		#endregion

		#endregion


		#region Fields
		static int _instances = 0;
		internal int ID;
		Label _startAccessory = new Label
		{
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill
		};
		Label _endAccessory = new Label
		{
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill
		};
		#endregion


		#region Constructor
		/// <summary>
		/// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BaseCellView"/> class.
		/// </summary>
		public BaseCellView () {
			ID = _instances++;
			Padding = new Thickness(0,1,0,1);
			ColumnSpacing = 0;

			this.SetBinding(SeparatorIsVisibleProperty, SeparatorIsVisibleProperty.PropertyName);
			this.SetBinding(SeparatorColorProperty, SeparatorColorProperty.PropertyName);
			this.SetBinding(SeparatorHeightProperty, SeparatorHeightProperty.PropertyName);
			this.SetBinding(SeparatorLeftIndentProperty, SeparatorLeftIndentProperty.PropertyName);
			this.SetBinding(SeparatorRightIndentProperty, SeparatorRightIndentProperty.PropertyName);

			this.SetBinding(StartAccessoryProperty, StartAccessoryProperty.PropertyName);
			this.SetBinding(EndAccessoryProperty, EndAccessoryProperty.PropertyName);

			_startAccessory.SetBinding(Label.HorizontalTextAlignmentProperty,CellAccessory.HorizonatalAlignmentProperty.PropertyName);
			_startAccessory.SetBinding(Label.VerticalTextAlignmentProperty, CellAccessory.HorizonatalAlignmentProperty.PropertyName);

			_endAccessory.SetBinding(Label.HorizontalTextAlignmentProperty, CellAccessory.HorizonatalAlignmentProperty.PropertyName);
			_endAccessory.SetBinding(Label.VerticalTextAlignmentProperty, CellAccessory.HorizonatalAlignmentProperty.PropertyName);


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
			UpdateLayout();
			//System.Diagnostics.Debug.WriteLine("OnBindingContextChanged");
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
				_startAccessory.HtmlText = null;
				_endAccessory.HtmlText = null;
			}
			else if (propertyName == StartAccessoryProperty.PropertyName && StartAccessory != null)
				StartAccessory.PropertyChanged -= OnStartAccessoryPropertyChanged;
			else if (propertyName == EndAccessoryProperty.PropertyName && EndAccessory != null)
				EndAccessory.PropertyChanged -= OnEndAccessoryPropertyChanged;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == StartAccessoryProperty.PropertyName)
			{
				_startAccessory.BindingContext = StartAccessory;
				if (StartAccessory != null)
				{
					StartAccessory.PropertyChanged += OnStartAccessoryPropertyChanged;
					UpdateStartAccessoryWidth();
				}
			}
			else if (propertyName == EndAccessoryProperty.PropertyName)
			{
				_endAccessory.BindingContext = EndAccessory;
				if (EndAccessory != null)
				{
					EndAccessory.PropertyChanged += OnEndAccessoryPropertyChanged;
					UpdateEndAccessoryWidth();
				}
			}



			_freshContent = (_freshContent || propertyName == ContentProperty.PropertyName);
			//System.Diagnostics.Debug.WriteLine("OnPropertyChanged ["+propertyName+"] Item.Source=["+((Item)BindingContext)?.Source+"]");
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
			
			//System.Diagnostics.Debug.WriteLine("OnItemPropertyChanged");
			_freshContent = (_freshContent || e.PropertyName == ContentProperty.PropertyName);
			UpdateLayout();
		}

		void OnStartAccessoryPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CellAccessory.WidthProperty.PropertyName)
				UpdateStartAccessoryWidth();
			else if (e.PropertyName == CellAccessory.TextFunctionProperty.PropertyName)
				UpdateLayout();
		}

		void OnEndAccessoryPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CellAccessory.WidthProperty.PropertyName)
				UpdateEndAccessoryWidth();
			else if (e.PropertyName == CellAccessory.TextFunctionProperty.PropertyName)
				UpdateLayout();
		}

		void UpdateStartAccessoryWidth()
		{
			double width = StartAccessory.Width < 1 ? Width * StartAccessory.Width : StartAccessory.Width;
			ColumnDefinitions[0] = new ColumnDefinition { Width = new GridLength(width, GridUnitType.Absolute) };
		}

		void UpdateEndAccessoryWidth()
		{
			double width = EndAccessory.Width < 1 ? Width * EndAccessory.Width : EndAccessory.Width;
			ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(width, GridUnitType.Absolute) };
		}

		bool _lastStartAccessoryActive;
		bool _lastEndAccesoryActive;
		bool _freshContent = false;
		void UpdateLayout()
		{
			string startAccessoryText = ((Item)BindingContext)?.StartAccessory?.TextFunction?.Invoke((Item)BindingContext);
			if (startAccessoryText != _startAccessory.HtmlText)
				_startAccessory.HtmlText = startAccessoryText;
			string endAccessoryText = ((Item)BindingContext)?.EndAccessory?.TextFunction?.Invoke((Item)BindingContext);
			if (endAccessoryText != _endAccessory.HtmlText)
				_endAccessory.HtmlText = endAccessoryText;

			var startAccessoryActive = (_startAccessory.HtmlText != null);
			var endAccessoryActive = (_endAccessory.HtmlText != null);

			if (startAccessoryActive != _lastStartAccessoryActive || endAccessoryActive != _lastEndAccesoryActive || _freshContent) 
			{
				if (_startAccessory != null && Children.Contains(_startAccessory))
					Children.Remove(_startAccessory);
				if (Content != null && Children.Contains(Content))
					Children.Remove(Content);
				if (_endAccessory != null && Children.Contains(_endAccessory))
					Children.Remove(_endAccessory);
				if (startAccessoryActive)
				{
					Children.Add(_startAccessory, 0, 0);
					if (endAccessoryActive)
					{
						Children.Add(Content, 1, 0);
						Children.Add(_endAccessory, 2, 0);
					}
					else
					{
						Children.Add(Content, 1, 3, 0, 1);
						_endAccessory.HtmlText = null;
					}
				}
				else 
				{
					_startAccessory.HtmlText = null;
					if (endAccessoryActive)
					{
						Children.Add(Content, 0, 2, 0, 1);
						Children.Add(_endAccessory, 2, 0);
					}
					else
					{
						Children.Add(Content, 0, 3, 0, 1);
						_endAccessory.HtmlText = null;
					}
				}
				_lastStartAccessoryActive = startAccessoryActive;
				_lastEndAccesoryActive = endAccessoryActive;
				_freshContent = false;
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

