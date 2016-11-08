using Xamarin.Forms;
using System;
using FormsGestures;

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
		internal static readonly BindableProperty SeparatorIsVisibleProperty = ItemWrapper.SeparatorIsVisibleProperty;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.BaseCellView"/> separator is visible.
		/// </summary>
		/// <value><c>true</c> if separator is visible; otherwise, <c>false</c>.</value>
		internal bool SeparatorIsVisible
		{
			get { return (bool)GetValue(SeparatorIsVisibleProperty); }
			set
			{
				SetValue(SeparatorIsVisibleProperty, value);
			}
		}

		/// <summary>
		/// The separator color property.
		/// </summary>
		internal static readonly BindableProperty SeparatorColorProperty = ItemWrapper.SeparatorColorProperty;
		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		internal Color SeparatorColor
		{
			get { return (Color)GetValue(SeparatorColorProperty); }
			set { SetValue(SeparatorColorProperty, value); }
		}

		/// <summary>
		/// The separator height property.
		/// </summary>
		public static readonly BindableProperty SeparatorHeightProperty = ItemWrapper.SeparatorHeightProperty;
		/// <summary>
		/// Gets or sets the height of the separator.
		/// </summary>
		/// <value>The height of the separator.</value>
		public double SeparatorHeight
		{
			get { return (double)GetValue(SeparatorHeightProperty); }
			set { SetValue(SeparatorHeightProperty, value); }
		}

		/// <summary>
		/// The separator left indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorLeftIndentProperty = ItemWrapper.SeparatorLeftIndentProperty;
		/// <summary>
		/// Gets or sets the separator left indent.
		/// </summary>
		/// <value>The separator left indent.</value>
		public double SeparatorLeftIndent
		{
			get { return (double)GetValue(SeparatorLeftIndentProperty); }
			set { SetValue(SeparatorLeftIndentProperty, value); }
		}

		/// <summary>
		/// The separator right indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorRightIndentProperty = ItemWrapper.SeparatorRightIndentProperty;
		/// <summary>
		/// Gets or sets the separator right indent.
		/// </summary>
		/// <value>The separator right indent.</value>
		public double SeparatorRightIndent
		{
			get { return (double)GetValue(SeparatorRightIndentProperty); }
			set { SetValue(SeparatorRightIndentProperty, value); }
		}
		#endregion

		#region Accessory appearance
		/// <summary>
		/// The start accessory property.
		/// </summary>
		public static readonly BindableProperty StartAccessoryProperty = ItemWrapper.StartAccessoryProperty;
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
		public static readonly BindableProperty EndAccessoryProperty = ItemWrapper.EndAccessoryProperty;
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
		static int _instances;
		internal int ID;

		#region Accessories
		Label _startAccessory = new Label
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			TextColor = Color.Black
		};
		Label _endAccessory = new Label
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			TextColor = Color.Black
		};
		#endregion


		#region Swipe Actions
		readonly Frame _insetFrame = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			HasShadow = true,
			ShadowInverted = true,
			BackgroundColor = Color.FromRgb(200,200,200),
			Padding = 0,
			Margin = 0,
			OutlineWidth = 0,
		};

		readonly Frame _action1Frame = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		readonly Frame _action2Frame = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		readonly Frame _action3Frame = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		static readonly Frame _touchBlocker = new Frame
		{
			BackgroundColor = Color.FromRgba(0, 0, 0, 1),
		};

		readonly MaterialButton _action1Button = new MaterialButton { WidthRequest = 50, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };
		readonly MaterialButton _action2Button = new MaterialButton { WidthRequest = 44, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };
		readonly MaterialButton _action3Button = new MaterialButton { WidthRequest = 44, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };

		#endregion

		#endregion


		#region Constructor
		/// <summary>
		/// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BaseCellView"/> class.
		/// </summary>
		public BaseCellView () {
			ID = _instances++;
			Padding = new Thickness(0,1,0,1);
			ColumnSpacing = 0;
			RowSpacing = 0;
			Margin = 0;

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

			var thisListener = new Listener(this);
			thisListener.Tapped += OnTapped;
			thisListener.LongPressed += OnLongPressed;
			thisListener.LongPressing += OnLongPressing;
			thisListener.Panned += OnPanned;
			thisListener.Panning += OnPanning;

			_action1Frame.Content = _action1Button;
			_action2Frame.Content = _action2Button;
			_action3Frame.Content = _action3Button;

			//var blockerListener = new Listener(_touchBlocker);
			//blockerListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("BLOCKER");;

			_action1Button.Tapped += OnActionButtonTapped;
			_action2Button.Tapped += OnActionButtonTapped;
			_action3Button.Tapped += OnActionButtonTapped;
		}
		#endregion


		#region Pan Manager
		bool _settingup;

		double ChildrenX
		{
			get
			{
				return Content.TranslationX;
			}
			set
			{
				Content.TranslationX = value;
				_startAccessory.TranslationX = value;
				_endAccessory.TranslationX = value;
			}
		}

		void TranslateChildrenTo(double x, double y, uint milliseconds, Easing easing)
		{
			Content.TranslateTo(x,y,milliseconds,easing);
			_startAccessory.TranslateTo(x,y,milliseconds,easing);
			_endAccessory.TranslateTo(x,y,milliseconds,easing);
		}

		void OnPanned(object sender, PanEventArgs e)
		{
			if (_endButtons > 0)
			{
				System.Diagnostics.Debug.WriteLine("ChildrenX=[" + ChildrenX + "]");
				if (e.TotalDistance.X > 20 || ChildrenX > -60)
				{
					// put it away
					TranslateChildrenTo(0, 0, 300, Easing.Linear);
					_action1Frame.TranslateTo(Width, 0, 400, Easing.Linear);
					if (_endButtons > 1)
						_action2Frame.TranslateTo(Width, 0, 400, Easing.Linear);
					if (_endButtons > 2)
						_action3Frame.TranslateTo(Width, 0, 400, Easing.Linear);
					_insetFrame.TranslateTo(Width, 0, 400, Easing.Linear);
				}
				else if (ChildrenX < -210 && ((ICellContentView)Content).EndSwipeActions[0].SwipeExecutable)
				{
					// execute full swipe
					TranslateChildrenTo(0, 0, 300, Easing.Linear);
					_action1Frame.TranslateTo(0, 0, 300, Easing.Linear);
					if (_endButtons > 1)
						_action2Frame.TranslationX = Width;
					if (_endButtons > 2)
						_action3Frame.TranslationX = Width;
					_insetFrame.TranslationX = Width;
				}
				else
				{
					// display 3 buttons
					TranslateChildrenTo(-180, 0, 300, Easing.Linear);
					_action1Frame.TranslateTo(Width - 60, 0, 300, Easing.Linear);
					if (_endButtons > 1)
						_action2Frame.TranslateTo(Width - 120, 0, 300, Easing.Linear);
					if (_endButtons > 2)
						_action3Frame.TranslateTo(Width - 180, 0, 300, Easing.Linear);
					_insetFrame.TranslateTo(Width - 180, 0, 300, Easing.Linear);
					translateOnUp = -180;
					return;
				}
				translateOnUp = 0;
				_endButtons = 0;
				Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
				 {
					 _touchBlocker.IsVisible = false;
					 return false;
				 });
			}
		}

		void OnPanning(object sender, PanEventArgs e)
		{
			double distance = e.TotalDistance.X + translateOnUp;
			System.Diagnostics.Debug.WriteLine("eb=["+_endButtons+"] sb=["+startButtons+"] Distance=["+distance+"] translateOnUp=["+translateOnUp+"]");
			if (_settingup)
				return;
			if (_endButtons > 0)
			{
				if (distance <= -180)
				{
					// we're beyond the limit of presentation of the buttons
					ChildrenX = -180;
					if (distance <= -210 && e.DeltaDistance.X <= 0 && ((ICellContentView)Content).EndSwipeActions[0].SwipeExecutable)
						_action1Frame.TranslateTo(0,0,200,Easing.Linear);
					else
						_action1Frame.TranslateTo(Width-60,0,200,Easing.Linear);
					if (_endButtons > 1)
						_action2Frame.TranslationX = Width -120;
					if (_endButtons > 2)
						_action3Frame.TranslationX = Width -180;
					_insetFrame.TranslationX = Width - 180;
					return;
				}
				if (distance > 1)
				{
					// we keep the endButtons going so as to not allow for the startButtons to appear
					ChildrenX = 0;
					return;
				}
				ChildrenX = distance;
				_action1Frame.TranslationX = Width + distance / 3.0;
				_action2Frame.TranslationX = Width + 2 * distance / 3.0;
				_action3Frame.TranslationX = Width + distance;
				_insetFrame.TranslationX = Width + distance;
			}
			else if (startButtons > 0)
			{
			}
			else if (distance < 0)
			{
				var rightActions = ((ICellContentView)Content)?.EndSwipeActions;
				if (rightActions != null && rightActions.Count > 0)
				{
					_settingup = true;

					Children.Add(_touchBlocker, 0, 0);
					SetColumnSpan(_touchBlocker, 3);
					_touchBlocker.IsVisible = true;

					Children.Add(_insetFrame, 0, 0);
					SetColumnSpan(_insetFrame, 3);
					_insetFrame.TranslationX = Width;

					// setup buttons
					_endButtons = 1;
					translateOnUp = 0;
					_action1Button.HorizontalOptions = LayoutOptions.Start;
					_action1Frame.BackgroundColor = rightActions[0].BackgroundColor;
					_action1Button.HtmlText = rightActions[0].Text;
					_action1Button.IconText = rightActions[0].IconText;
					_action1Button.FontColor = rightActions[0].TextColor;
					if (rightActions.Count > 1)
					{
						_endButtons = 2;
						_action2Button.HorizontalOptions = LayoutOptions.Start;
						_action2Frame.BackgroundColor = rightActions[1].BackgroundColor;
						_action2Button.HtmlText = rightActions[1].Text;
						_action2Button.IconText = rightActions[1].IconText;
						_action2Button.FontColor = rightActions[1].TextColor;
						if (rightActions.Count > 2)
						{
							_endButtons = 3;
							_action3Button.HorizontalOptions = LayoutOptions.Start;
							if (rightActions.Count > 3)
							{
								_action3Frame.BackgroundColor = Color.Gray;
								_action3Button.HtmlText = "More";
								_action3Button.IconText = "•••";
								_action3Button.FontColor = Color.White;
							}
							else
							{
								_action3Frame.BackgroundColor = rightActions[2].BackgroundColor;
								_action3Button.HtmlText = rightActions[2].Text;
								_action3Button.IconText = rightActions[2].IconText;
								_action3Button.FontColor = rightActions[2].TextColor;
							}
							Children.Add(_action3Frame, 0, 0);
							SetColumnSpan(_action3Frame, 3);
							_action3Frame.TranslationX = Width;
						}
						Children.Add(_action2Frame, 0, 0);
						SetColumnSpan(_action2Frame, 3);
						_action2Frame.TranslationX = Width - distance / 3.0;
					}
					Children.Add(_action1Frame, 0, 0);
					SetColumnSpan(_action1Frame, 3);
					_action1Frame.TranslationX = Width - 2 * distance / 3.0;
					_settingup = false;
				}
			}
		}

		#endregion


		#region ActionButton 
		int _endButtons;
		int startButtons;
		double translateOnUp;

		void OnActionButtonTapped(object sender, EventArgs e)
		{
			int index = 0;
			if (sender == _action2Button)
				index = 1;
			else if (sender == _action3Button)
				index = 2;
			if (_endButtons > 0)
				((ICellContentView)Content).EndSwipeActions[index].Action?.Invoke(this, ((ItemWrapper)BindingContext).Source);
			else
				((ICellContentView)Content).StartSwipeActions[index].Action?.Invoke(this, ((ItemWrapper)BindingContext).Source);
			System.Diagnostics.Debug.WriteLine("SwipeAction[" + (index + 1) + "]");
		}

		#endregion


		#region Gestures
		void OnTapped(object sender, TapEventArgs e)
		{
			if (_endButtons==0 && startButtons ==0)
				((ItemWrapper)BindingContext)?.OnTapped(this, new ItemWrapperTapEventArgs((ItemWrapper)BindingContext));
		}

		void OnLongPressed(object sender, LongPressEventArgs e)
		{
			if (_endButtons == 0 && startButtons == 0)
				((ItemWrapper)BindingContext)?.OnLongPressed(this, new ItemWrapperLongPressEventArgs((ItemWrapper)BindingContext));
		}

		void OnLongPressing(object sender, LongPressEventArgs e)
		{
			if (_endButtons == 0 && startButtons == 0)
				((ItemWrapper)BindingContext)?.OnLongPressing(this, new ItemWrapperLongPressEventArgs((ItemWrapper)BindingContext));
		}
		#endregion


		#region change management

		/// <summary>
		/// Triggered by a change in the binding context
		/// </summary>
		protected override void OnBindingContextChanged () {
			//System.Diagnostics.Debug.WriteLine("BaseCellView.OnBindingContextChanged");
			if (BindingContext == null)
				return;
			var item = BindingContext as ItemWrapper;
			if (item != null)
			{
				item.BaseCellView = this;
				item.PropertyChanged += OnItemPropertyChanged;
				if (/*!item.HasUnevenRows && */item.RowHeight > 0)
				{
					HeightRequest = item.RowHeight;
					Content.HeightRequest = item.RowHeight;
				}
				else
				{
					HeightRequest = -1;
					Content.HeightRequest = -1;
				}
				UpdateUnBoundProperties();
			}
			else
				System.Diagnostics.Debug.WriteLine("");
			var type = BindingContext?.GetType ();
			if (type == typeof(NullItemWrapper) || type == typeof(BlankItemWrapper))
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
			//System.Diagnostics.Debug.WriteLine("BaseCellView.OnPropertyChanging("+propertyName+")");
			base.OnPropertyChanging(propertyName);
			if (propertyName == BindingContextProperty.PropertyName)
			{
				var item = BindingContext as ItemWrapper;
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
			//System.Diagnostics.Debug.WriteLine("BaseCellView.OnPropertyChanged(" + propertyName + ")");
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
			//System.Diagnostics.Debug.WriteLine("BaseCellView.OnItemPropertyChanging(" + e.PropertyName + ")");
			if (e.PropertyName == ItemWrapper.IsSelectedProperty.PropertyName 
			    || e.PropertyName == ItemWrapper.CellBackgroundColorProperty.PropertyName 
			    || e.PropertyName == ItemWrapper.SelectedCellBackgroundColorProperty.PropertyName
			    || e.PropertyName == ItemWrapper.IndexProperty.PropertyName
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
		bool _freshContent;
		void UpdateLayout()
		{
			//System.Diagnostics.Debug.WriteLine("BaseCellView.UpdateLayout");
			string startAccessoryText = ((ItemWrapper)BindingContext)?.StartAccessory?.TextFunction?.Invoke((ItemWrapper)BindingContext);
			//if (startAccessoryText != _startAccessory.HtmlText)
				_startAccessory.HtmlText = startAccessoryText;
			string endAccessoryText = ((ItemWrapper)BindingContext)?.EndAccessory?.TextFunction?.Invoke((ItemWrapper)BindingContext);
			//if (endAccessoryText != _endAccessory.HtmlText)
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
			//System.Diagnostics.Debug.WriteLine("BaseCellView.UpdateUnBoundProperties");
			var item = BindingContext as ItemWrapper;
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

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("BaseCellView.LayoutChildren");
			base.LayoutChildren(x, y, width, height);
		}
		#endregion
	}
}

