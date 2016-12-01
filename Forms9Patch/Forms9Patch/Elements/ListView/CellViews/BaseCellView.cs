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


		#region Swipe Menu
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
		readonly Frame _swipeFrame1 = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		readonly Frame _swipeFrame2 = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		readonly Frame _swipeFrame3 = new Frame
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = new Thickness(-1)
		};
		readonly Frame _touchBlocker = new Frame
		{
			BackgroundColor = Color.FromRgba(0, 0, 0, 1),
		};

		readonly MaterialButton _swipeButton1 = new MaterialButton { WidthRequest = 50, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };
		readonly MaterialButton _swipeButton2 = new MaterialButton { WidthRequest = 44, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };
		readonly MaterialButton _swipeButton3 = new MaterialButton { WidthRequest = 44, OutlineWidth = 0, OutlineRadius = 0, Orientation = StackOrientation.Vertical };

		#endregion

		#endregion


		#region Constructor
		/// <summary>
		/// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BaseCellView"/> class.
		/// </summary>
		public BaseCellView () {
			ID = _instances++;
			Padding = 0; // new Thickness(0,1,0,1);
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
				new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute) },
				new RowDefinition { Height = GridLength.Auto }
			};

			var thisListener = new Listener(this);
			thisListener.Tapped += OnTapped;
			thisListener.LongPressed += OnLongPressed;
			thisListener.LongPressing += OnLongPressing;
			thisListener.Panned += OnPanned;
			thisListener.Panning += OnPanning;

			_swipeFrame1.Content = _swipeButton1;
			_swipeFrame2.Content = _swipeButton2;
			_swipeFrame3.Content = _swipeButton3;

			//var blockerListener = new Listener(_touchBlocker);
			//blockerListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("BLOCKER");;

			_swipeButton1.Tapped += OnSwipeButtonTapped;
			_swipeButton2.Tapped += OnSwipeButtonTapped;
			_swipeButton3.Tapped += OnSwipeButtonTapped;
		}
		#endregion


		#region Swipe Menu
		enum Side
		{
			Start = -1,
			End = 1
		} 
		bool _settingup;
		int _endButtons;
		int _startButtons;
		double _translateOnUp;

		double ContentX
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
			_panVt = false;
			_panHz = false;
			var iCellContentView = Content as ICellContentView;
			if (iCellContentView != null )
			{
				double distance = e.TotalDistance.X + _translateOnUp;
				if (_endButtons + _startButtons > 0)
				{
					var side = _startButtons > 0 ? Side.Start : Side.End;
					//System.Diagnostics.Debug.WriteLine("ChildrenX=[" + ChildrenX + "]");
					if ((_endButtons>0 && side == Side.End && (e.TotalDistance.X > 20 || ContentX > -60)) || 
					    (_startButtons>0 && side == Side.Start && (e.TotalDistance.X < -20 || ContentX < 60)))
					{
						PutAwaySwipeButtons(true);
						return;
					}
					if ((  _endButtons > 0 && side == Side.End   && /*_swipeFrame1.TranslationX < Width - 210 */ distance <= -210 && ((ICellContentView)Content).EndSwipeMenu!=null   && ((ICellContentView)Content).EndSwipeMenu.Count > 0   && ((ICellContentView)Content).EndSwipeMenu[0].SwipeActivated) || 
					    (_startButtons > 0 && side == Side.Start && /*_swipeFrame1.TranslationX > 210 - Width */ distance >=  210 && ((ICellContentView)Content).StartSwipeMenu!=null && ((ICellContentView)Content).StartSwipeMenu.Count > 0 && ((ICellContentView)Content).StartSwipeMenu[0].SwipeActivated))
					{
						// execute full swipe
						_swipeFrame1.TranslateTo(0, 0, 250, Easing.Linear);
						OnSwipeButtonTapped(_swipeButton1, EventArgs.Empty);
						Device.StartTimer(TimeSpan.FromMilliseconds(400), () =>
						{
							PutAwaySwipeButtons(false);
							return false;
						});
					}
					else
					{
						// display 3 buttons
						TranslateChildrenTo(-(int)side * (60 * (_endButtons + _startButtons)), 0, 300, Easing.Linear);
						_swipeFrame1.TranslateTo((int)side * (Width - 60), 0, 300, Easing.Linear);
						if (_endButtons + _startButtons > 1)
							_swipeFrame2.TranslateTo((int)side * (Width - 120), 0, 300, Easing.Linear);
						if (_endButtons + _startButtons > 2)
							_swipeFrame3.TranslateTo((int)side * (Width - 180), 0, 300, Easing.Linear);
						_insetFrame.TranslateTo((int)side * (Width - (60 * (_endButtons + _startButtons))), 0, 300, Easing.Linear);
						_translateOnUp = (int)side * -180;
						return;
					}
				}

			}
		}

		bool _panHz, _panVt;
		void OnPanning(object sender, PanEventArgs e)
		{
			if (!_panVt && !_panVt)
			{
				if (Math.Abs(e.TotalDistance.X) > 10)
					_panHz = true;
				else if (Math.Abs(e.TotalDistance.Y) > 10)
					_panVt = true;
				else
					return;
			}
			double distance = e.TotalDistance.X + _translateOnUp;
			//System.Diagnostics.Debug.WriteLine("eb=["+_endButtons+"] sb=["+startButtons+"] Distance=["+distance+"] translateOnUp=["+translateOnUp+"]");
			if (_settingup)
				return;
			if (_endButtons + _startButtons > 0)
			{
				var side = _startButtons > 0 ? Side.Start : Side.End;
				if ((side == Side.End && distance <= -60 * _endButtons) || (side == Side.Start && distance >= 60 * _startButtons))
				{
					// we're beyond the limit of presentation of the buttons
					ContentX = (int)side * -180;
					if (     side == Side.End   && distance <= -210 && e.DeltaDistance.X <= 0 && ((ICellContentView)Content).EndSwipeMenu!=null   && ((ICellContentView)Content).EndSwipeMenu.Count> 0 && ((ICellContentView)Content).EndSwipeMenu[0].SwipeActivated)
						_swipeFrame1.TranslateTo(0, 0, 200, Easing.Linear);
					else if (side == Side.Start && distance >=  210 && e.DeltaDistance.X >= 0 && ((ICellContentView)Content).StartSwipeMenu!=null && ((ICellContentView)Content).StartSwipeMenu.Count>0 && ((ICellContentView)Content).StartSwipeMenu[0].SwipeActivated)
						_swipeFrame1.TranslateTo(0, 0, 200, Easing.Linear);
					else
						_swipeFrame1.TranslateTo((int)side * (Width - (int)side * 60), 0, 200, Easing.Linear);
					if (_endButtons + _startButtons > 1)
						_swipeFrame2.TranslationX = (int)side * (Width - (int)side * 120);
					if (_endButtons + _startButtons > 2)
						_swipeFrame3.TranslationX = (int)side * (Width - (int)side * 180);
					_insetFrame.TranslationX = (int)side * (Width + (int)side * distance);
					return;
				}
				if ((side == Side.End && distance > 1) || (side == Side.Start && distance < 1))
				{
					// we keep the endButtons going so as to not allow for the startButtons to appear
					ContentX = 0;
					return;
				}
				ContentX = distance;
				_swipeFrame1.TranslationX = (int)side * (Width + (int)side * distance / (_endButtons + _startButtons));
				_swipeFrame2.TranslationX = (int)side * (Width + (int)side * 2 * distance / (_endButtons + _startButtons));
				_swipeFrame3.TranslationX = (int)side * (Width + (int)side * distance);
				_insetFrame.TranslationX = (int)side * (Width + (int)side * distance);
			}
			else if (Math.Abs(distance) > 0.1)
			{
				// setup end SwipeMenu
				var side = distance < 0 ? Side.End : Side.Start;
				var iCellContenveView = Content as ICellContentView;
				if (iCellContenveView != null)
				{
					var swipeMenu = side == Side.End ? iCellContenveView.EndSwipeMenu : iCellContenveView.StartSwipeMenu;
					if (swipeMenu != null && swipeMenu.Count > 0)
					{
						_settingup = true;

						Children.Add(_touchBlocker, 0, 1);
						SetColumnSpan(_touchBlocker, 3);
						_touchBlocker.IsVisible = true;

						Children.Add(_insetFrame, 0, 1);
						SetColumnSpan(_insetFrame, 3);
						_insetFrame.TranslationX = (int)side * Width;

						// setup buttons
						if (side == Side.End)
						{
							_endButtons = 1;
							_swipeButton1.HorizontalOptions = LayoutOptions.Start;
						}
						else
						{
							_startButtons = 1;
							_swipeButton1.HorizontalOptions = LayoutOptions.End;
						}
						_translateOnUp = 0;
						_swipeFrame1.BackgroundColor = swipeMenu[0].BackgroundColor;
						_swipeButton1.HtmlText = swipeMenu[0].Text;
						_swipeButton1.IconText = swipeMenu[0].IconText;
						_swipeButton1.FontColor = swipeMenu[0].TextColor;
						if (swipeMenu.Count > 1)
						{
							if (side == Side.End)
							{
								_endButtons = 2;
								_swipeButton2.HorizontalOptions = LayoutOptions.Start;
							}
							else
							{
								_startButtons = 2;
								_swipeButton2.HorizontalOptions = LayoutOptions.End;
							}
							_swipeFrame2.BackgroundColor = swipeMenu[1].BackgroundColor;
							_swipeButton2.HtmlText = swipeMenu[1].Text;
							_swipeButton2.IconText = swipeMenu[1].IconText;
							_swipeButton2.FontColor = swipeMenu[1].TextColor;
							if (swipeMenu.Count > 2)
							{
								if (side == Side.End)
								{
									_endButtons = 3;
									_swipeButton3.HorizontalOptions = LayoutOptions.Start;
								}
								else
								{
									_startButtons = 3;
									_swipeButton3.HorizontalOptions = LayoutOptions.End;
								}
								if (swipeMenu.Count > 3)
								{
									_swipeFrame3.BackgroundColor = Color.Gray;
									_swipeButton3.HtmlText = "More";
									_swipeButton3.IconText = "•••";
									_swipeButton3.FontColor = Color.White;
								}
								else
								{
									_swipeFrame3.BackgroundColor = swipeMenu[2].BackgroundColor;
									_swipeButton3.HtmlText = swipeMenu[2].Text;
									_swipeButton3.IconText = swipeMenu[2].IconText;
									_swipeButton3.FontColor = swipeMenu[2].TextColor;
								}
								Children.Add(_swipeFrame3, 0, 1);
								SetColumnSpan(_swipeFrame3, 3);
								_swipeFrame3.TranslationX = (int)side * Width;
							}
							Children.Add(_swipeFrame2, 0, 1);
							SetColumnSpan(_swipeFrame2, 3);
							_swipeFrame2.TranslationX = (int)side * (Width - distance / 3.0);
						}
						Children.Add(_swipeFrame1, 0, 1);
						SetColumnSpan(_swipeFrame1, 3);
						_swipeFrame1.TranslationX = (int)side * (Width - 2 * distance / 3.0);
						_settingup = false;
					}

				}
			}
		}

		void PutAwaySwipeButtons(bool animated)
		{
			var parkingX = _endButtons > 0 ? Width : -Width;
			if (animated)
			{
				TranslateChildrenTo(0, 0, 300, Easing.Linear);
				_swipeFrame1.TranslateTo(parkingX, 0, 400, Easing.Linear);
				if (_endButtons + _startButtons > 1)
					_swipeFrame2.TranslateTo(parkingX, 0, 400, Easing.Linear);
				if (_endButtons + _startButtons > 2)
					_swipeFrame3.TranslateTo(parkingX, 0, 400, Easing.Linear);
				_insetFrame.TranslateTo(parkingX, 0, 400, Easing.Linear);
				Device.StartTimer(TimeSpan.FromMilliseconds(400), () =>
				{
					_touchBlocker.IsVisible = false;
					return false;
				});
			}
			else
			{
				ContentX = 0;
				_swipeFrame1.TranslationX = parkingX;
				if (_endButtons + _startButtons > 1)
					_swipeFrame2.TranslationX = parkingX;
				if (_endButtons + _startButtons > 2)
					_swipeFrame3.TranslationX = parkingX;
				_insetFrame.TranslationX = parkingX;
				_touchBlocker.IsVisible = false;
			}
			_translateOnUp = 0;
			_endButtons = 0;
			_startButtons = 0;
		}

		void OnSwipeButtonTapped(object sender, EventArgs e)
		{
			int index = 0;
			if (sender == _swipeButton2)
				index = 1;
			else if (sender == _swipeButton3)
				index = 2;
			var swipeMenu = _endButtons > 0 ? ((ICellContentView)Content).EndSwipeMenu : ((ICellContentView)Content).StartSwipeMenu;
			if (index == 2 && _endButtons + _startButtons > 2)
			{
				// show remaining menu items in a modal list

				var segmentedController = new MaterialSegmentedControl
				{
					Orientation = StackOrientation.Vertical,
					BackgroundColor = Settings.ListViewCellSwipePopupMenuButtonColor,
					FontSize = Settings.ListViewCellSwipePopupMenuFontSize,
					FontColor = Settings.ListViewCellSwipePopupMenuFontColor,
					OutlineColor = Settings.ListViewCellSwipePopupMenuButtonOutlineColor,
					OutlineWidth = Settings.ListViewCellSwipePopupMenuButtonOutlineWidth,
					SeparatorWidth = Settings.ListViewCellSwipePopupMenuButtonSeparatorWidth,
					OutlineRadius = Settings.ListViewCellSwipePopupMenuButtonOutlineRadius,
					Padding = 5,
					WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
				};
				var cancelButton = new MaterialButton
				{
					Text = "Cancel",
					FontAttributes = FontAttributes.Bold,
					BackgroundColor = Settings.ListViewCellSwipePopupMenuButtonColor,
					FontSize = Settings.ListViewCellSwipePopupMenuFontSize,
					FontColor = Settings.ListViewCellSwipePopupMenuFontColor,
					OutlineColor = Settings.ListViewCellSwipePopupMenuButtonOutlineColor,
					OutlineWidth = Settings.ListViewCellSwipePopupMenuButtonOutlineWidth,
					SeparatorWidth = Settings.ListViewCellSwipePopupMenuButtonSeparatorWidth,
					OutlineRadius = Settings.ListViewCellSwipePopupMenuButtonOutlineRadius,
					Padding = 5,
					WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
				};
				var stack = new StackLayout
				{
					Orientation = StackOrientation.Vertical,
					WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
					Children = { segmentedController, cancelButton }
				};
				var modal = new Forms9Patch.ModalPopup(this)
				{
					BackgroundColor = Color.Transparent,
					OutlineWidth = 0,
					WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
					Content = stack
				};
				cancelButton.Tapped += (s, arg) => modal.Cancel();
				for (int i = 2; i < swipeMenu.Count; i++)
				{
					var menuItem = swipeMenu[i];
					var segment = new Segment
					{
						Text = menuItem.Text,
						IconText = menuItem.IconText,
						ImageSource = menuItem.ImageSource,
					};
					segment.Tapped += (s, arg) =>
					{
						modal.Cancel();
						((ItemWrapper)BindingContext)?.OnSwipeMenuItemTapped(this, new SwipeMenuItemTappedArgs((ICellContentView)Content, (ItemWrapper)BindingContext, menuItem));
						//System.Diagnostics.Debug.WriteLine("SwipeMenu[" + menuItem.Key + "]");
					};
					segmentedController.Segments.Add(segment);
				}
				modal.IsVisible = true;
				//System.Diagnostics.Debug.WriteLine("SwipeMenu[More]");
			}
			else
			{
				PutAwaySwipeButtons(false);
				((ItemWrapper)BindingContext)?.OnSwipeMenuItemTapped(this, new SwipeMenuItemTappedArgs((ICellContentView)Content,(ItemWrapper)BindingContext,swipeMenu[index]));
				//System.Diagnostics.Debug.WriteLine("SwipeMenu[" + swipeMenu[index].Key + "]");
			}
		}

		#endregion


		#region Cell Gestures
		void OnTapped(object sender, TapEventArgs e)
		{
			if (_endButtons + _startButtons ==0)
				((ItemWrapper)BindingContext)?.OnTapped(this, new ItemWrapperTapEventArgs((ItemWrapper)BindingContext));
		}

		void OnLongPressed(object sender, LongPressEventArgs e)
		{
			if (_endButtons + _startButtons == 0)
				((ItemWrapper)BindingContext)?.OnLongPressed(this, new ItemWrapperLongPressEventArgs((ItemWrapper)BindingContext));
		}

		void OnLongPressing(object sender, LongPressEventArgs e)
		{
			if (_endButtons + _startButtons == 0)
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
				UpdateUnBoundProperties();
				SetHeights();
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
				PutAwaySwipeButtons(false);
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

			if (e.PropertyName == ItemWrapper.RowHeightProperty.PropertyName)
				SetHeights();
				
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
					Children.Add(_startAccessory, 0, 1);
					if (endAccessoryActive)
					{
						Children.Add(Content, 1, 1);
						Children.Add(_endAccessory, 2, 1);
					}
					else
					{
						Children.Add(Content, 1, 3, 1, 2);
						_endAccessory.HtmlText = null;
					}
				}
				else 
				{
					_startAccessory.HtmlText = null;
					if (endAccessoryActive)
					{
						Children.Add(Content, 0, 2, 1, 2);
						Children.Add(_endAccessory, 2, 1);
					}
					else
					{
						Children.Add(Content, 0, 3, 1, 2);
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
				SeparatorIsVisible = item.Index > 0 && item.SeparatorIsVisible;
			}
			else
			{
				BackgroundColor = Color.Transparent;
				SeparatorIsVisible = false;
			}
		}

		void SetHeights()
		{
			var content = Content as ICellContentView;
			if (content != null)
			{
				if (/*!item.HasUnevenRows && */content.RowHeight > 0)
				{
					HeightRequest = content.RowHeight + 1;

					//Content.HeightRequest = itemWrapper.RowHeight;
					//_insetFrame.HeightRequest = itemWrapper.RowHeight - Padding.Bottom;
					//_swipeFrame1.HeightRequest = itemWrapper.RowHeight - Padding.Bottom;
					//_swipeFrame2.HeightRequest = itemWrapper.RowHeight - Padding.Bottom;
					//_swipeFrame3.HeightRequest = itemWrapper.RowHeight - Padding.Bottom;
					//_touchBlocker.HeightRequest = itemWrapper.RowHeight - Padding.Bottom;
					RowDefinitions[1] = new RowDefinition { Height = new GridLength(content.RowHeight, GridUnitType.Absolute) };
				}
				else
				{
					var itemWrapper = BindingContext as ItemWrapper;
					if (itemWrapper != null)
					{
						HeightRequest = itemWrapper.RowHeight + 1;
						RowDefinitions[1] = new RowDefinition { Height = new GridLength(itemWrapper.RowHeight, GridUnitType.Absolute) };
					}
					else
					{
						HeightRequest = -1;
						Content.HeightRequest = -1;
						RowDefinitions[1] = new RowDefinition { Height = GridLength.Auto };
					}
				}
			}
			System.Diagnostics.Debug.WriteLine("HeightRequest = [" + HeightRequest + "]");
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//System.Diagnostics.Debug.WriteLine("BaseCellView.LayoutChildren");
			base.LayoutChildren(x, y, width, height);
		}
		#endregion
	}
}

