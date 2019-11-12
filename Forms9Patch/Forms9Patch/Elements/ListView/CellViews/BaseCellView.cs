using Xamarin.Forms;
using System;
using FormsGestures;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using P42.Utils;


namespace Forms9Patch
{
    /// <summary>
    /// DO NOT USE: Used by Forms9Patch.ListView as a foundation for cells.
    /// </summary>
    [DesignTimeVisible(true)]
    class BaseCellView : Xamarin.Forms.Grid, IDisposable, ICellContentView  // why grid?  because you can put more than one view in the same place at the same time
    {

        #region debug convenience
        static bool Debug
        {
            get
            {
                //return (BindingContext is ItemWrapper<string> itemWrapper && itemWrapper.Index == 0);
                //return false;
                //return (BindingContext is GroupWrapper wrapper && wrapper.Index == 0);

                //return BindingContext is GroupWrapper wrapper && wrapper.Source.GetType().ToString() == "HeightsAndAreas.FrontageSegment";
                return false;
            }
        }


        void DebugMessage(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            if (Debug)
                System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + callerName + ":" + lineNumber + "][" + InstanceId + "] " + message);
        }
        #endregion


        #region Properties
        /// <summary>
        /// The content property.
        /// </summary>
        public static readonly BindableProperty ContentViewProperty = BindableProperty.Create(nameof(ContentView), typeof(View), typeof(BaseCellView), default(View));
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public View ContentView
        {
            get => (View)GetValue(ContentViewProperty);
            set => SetValue(ContentViewProperty, value);
        }

        internal bool IsHeader;


        // ViewHeight priority : (1) value of View.CellHeight (2) TemplateSelectorBase.SetHeight() can try to set (if View.CellHeight doesn't exist or is -1).
        internal double RowHeight
        {
            get
            {
                //if (IsHeader)
                //    System.Diagnostics.Debug.WriteLine("");
                if (BindingContext is IItemWrapper wrapper)
                {
                    if (ContentView is ICellContentView contentView && contentView.CellHeight > -1)
                        wrapper.RenderedRowHeight = contentView.CellHeight;
                    else if (IsHeader && BindingContext is GroupWrapper groupWrapper)
                        wrapper.RenderedRowHeight = groupWrapper.RequestedGroupHeaderRowHeight;

                    return wrapper.BestGuessItemRowHeight();
                }
                return 40.0;
            }
        }

        // SeparatorHeight setter priority: (1) TemplateSelectorBase
        internal double SeparatorHeight
        {
            get
            {
                if (IsHeader)
                    return 0;
                if (BindingContext is IItemWrapper itemWrapper)
                    //return itemWrapper.RenderedSeparatorHeight;
                    return itemWrapper.RequestedSeparatorHeight;
                return 1;
            }
        }
        #endregion


        #region Fields
        static int _instances;
        internal int InstanceId;
        #endregion


        #region VisualElements
        readonly BoxView _separator = new BoxView
        {
            Color = Color.Black,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Margin = 0,
        };

        #region Swipe Menu
        readonly Frame _touchBlocker = new Frame
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            BackgroundColor = Color.FromRgba(0, 0, 0, 1)
        };

        readonly Frame _swipeFrame1 = new Frame
        {
            Padding = 0,
            VerticalOptions = LayoutOptions.FillAndExpand,
            //Padding = new Thickness(-1)
        };
        readonly Frame _swipeFrame2 = new Frame
        {
            Padding = 0,
            VerticalOptions = LayoutOptions.FillAndExpand,
            //Padding = new Thickness(-1)
        };
        readonly Frame _swipeFrame3 = new Frame
        {
            Padding = 0,
            VerticalOptions = LayoutOptions.FillAndExpand,
            //Padding = new Thickness(-1)
        };

        readonly Button _swipeButton1 = new Button
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            WidthRequest = 50,
            OutlineWidth = 0,
            OutlineRadius = 0,
            Orientation = StackOrientation.Vertical,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            HasTightSpacing = true,
            Spacing = 5,
        };
        readonly Button _swipeButton2 = new Button
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            WidthRequest = 44,
            OutlineWidth = 0,
            OutlineRadius = 0,
            Orientation = StackOrientation.Vertical,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            HasTightSpacing = true,
            Spacing = 5,
        };
        readonly Button _swipeButton3 = new Button
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            WidthRequest = 44,
            OutlineWidth = 0,
            OutlineRadius = 0,
            Orientation = StackOrientation.Vertical,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            HasTightSpacing = true,
            Spacing = 5,
        };
        readonly SegmentedControl _swipeSegmentedController = new SegmentedControl
        {
            Orientation = StackOrientation.Vertical,
            BackgroundColor = Settings.ListViewCellSwipePopupMenuButtonColor,
            FontSize = Settings.ListViewCellSwipePopupMenuFontSize,
            TextColor = Settings.ListViewCellSwipePopupMenuTextColor,
            OutlineColor = Settings.ListViewCellSwipePopupMenuButtonOutlineColor,
            OutlineWidth = Settings.ListViewCellSwipePopupMenuButtonOutlineWidth,
            SeparatorWidth = Settings.ListViewCellSwipePopupMenuButtonSeparatorWidth,
            OutlineRadius = Settings.ListViewCellSwipePopupMenuButtonOutlineRadius,
            Padding = 5,
            WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest
        };
        readonly Button _swipePopupCancelButton = new Button
        {
            Text = "Cancel",
            FontAttributes = FontAttributes.Bold,
            BackgroundColor = Settings.ListViewCellSwipePopupMenuButtonColor,
            FontSize = Settings.ListViewCellSwipePopupMenuFontSize,
            TextColor = Settings.ListViewCellSwipePopupMenuTextColor,
            OutlineColor = Settings.ListViewCellSwipePopupMenuButtonOutlineColor,
            OutlineWidth = Settings.ListViewCellSwipePopupMenuButtonOutlineWidth,
            OutlineRadius = Settings.ListViewCellSwipePopupMenuButtonOutlineRadius,
            Padding = 5,
            WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest
        };
        readonly StackLayout _swipePopupStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
        };
        readonly ModalPopup _swipePopup = new ModalPopup
        {
            BackgroundColor = Color.Transparent,
            OutlineWidth = 0,
            WidthRequest = Settings.ListViewCellSwipePopupMenuWidthRequest,
        };

        #endregion

        readonly FormsGestures.Listener _thisListener;

        #endregion


        #region Constructor
        /// <summary>
        /// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BaseCellView"/> class.
        /// </summary>
        public BaseCellView()
        {
            P42.Utils.Debug.AddToCensus(this);

            InstanceId = _instances++;
            Padding = 0; // new Thickness(0,1,0,1);
            ColumnSpacing = 0;
            RowSpacing = 0;
            Margin = 0;


            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            };
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute)}
            };

            _thisListener = FormsGestures.Listener.For(this);
            _thisListener.Tapped += OnTapped;
            _thisListener.LongPressed += OnLongPressed;
            _thisListener.LongPressing += OnLongPressing;
            _thisListener.Panned += OnPanned;
            _thisListener.Panning += OnPanning;
            _thisListener.RightClicked += OnRightClicked;
            //thisListener.Swiped += OnSwipe;


            _swipeFrame1.Content = _swipeButton1;
            _swipeFrame2.Content = _swipeButton2;
            _swipeFrame3.Content = _swipeButton3;

            _swipeButton1.Tapped += OnSwipeButtonTapped;
            _swipeButton2.Tapped += OnSwipeButtonTapped;
            _swipeButton3.Tapped += OnSwipeButtonTapped;

            Children.Add(_separator, 0, 1);

            _swipePopupStackLayout.Children.Add(_swipeSegmentedController);
            _swipePopupStackLayout.Children.Add(_swipePopupCancelButton);
            _swipePopup.Content = _swipePopupStackLayout;
        }

        bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                _thisListener.Tapped -= OnTapped;
                _thisListener.LongPressed -= OnLongPressed;
                _thisListener.LongPressing -= OnLongPressing;
                _thisListener.Panned -= OnPanned;
                _thisListener.Panning -= OnPanning;
                _thisListener.RightClicked -= OnRightClicked;
                _thisListener.Dispose();

                _swipeButton1.Tapped -= OnSwipeButtonTapped;
                _swipeButton2.Tapped -= OnSwipeButtonTapped;
                _swipeButton3.Tapped -= OnSwipeButtonTapped;

                _swipePopupCancelButton.Tapped -= OnSwipePopupCancelButtonTapped;

                _swipeButton1.Dispose();
                _swipeButton2.Dispose();
                _swipeButton3.Dispose();
                _swipeSegmentedController.Dispose();
                _swipePopupCancelButton.Dispose();
                _swipePopup.Dispose();

                if (BindingContext is ItemWrapper itemWrapper)
                {
                    itemWrapper.BaseCellView = null;
                    itemWrapper.PropertyChanged -= OnItemPropertyChanged;
                }

                if (ContentView != null)
                {
                    ContentView.PropertyChanged -= OnContentViewPropertyChanged;
                    FocusMonitor.Stop(ContentView);
                    Children.Remove(ContentView);
                    ContentView.BindingContext = null;
                    if (ContentView is IDisposable contentView)
                        contentView.Dispose();
                }

                _touchBlocker.Dispose();
                _swipeFrame1.Dispose();
                _swipeFrame2.Dispose();
                _swipeFrame3.Dispose();
                _swipePopupStackLayout.Dispose();

                P42.Utils.Debug.RemoveFromCensus(this);

            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region Swipe Menu

        private static void OnSwipe(object sender, SwipeEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Swiped:" + e);
        }




        enum Side
        {
            Start = -1,
            End = 1
        }
        bool _settingup;
        int _endButtons;
        int _startButtons;
        double _translateOnUp;

        double ContentViewX
        {
            get
            {
                return ContentView.TranslationX;
            }
            set
            {
                ContentView.TranslationX = value;
            }
        }

        void TranslateContentViewTo(double x, double y, uint milliseconds, Easing easing)
        {

            ContentView.TranslateTo(x, y, milliseconds, easing);
        }

        void OnPanned(object sender, PanEventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPanned(sender, e));
                return;
            }

            //if (Debug)
            //    System.Diagnostics.Debug.WriteLine(P42.Utils.ReflectionExtensions.CallerMemberName() + "(" + sender + ", " + e + ")");
            ((ItemWrapper)BindingContext)?.OnPanned(this, new ItemWrapperPanEventArgs((ItemWrapper)BindingContext, e));
            if (_panVt)
            {
                _panVt = false;
                return;
            }
            _panHz = false;
            if (ContentView is ICellSwipeMenus iCellSwipeMenus)
            {
                var distance = e.TotalDistance.X + _translateOnUp;
                if (_endButtons + _startButtons > 0)
                {
                    var side = _startButtons > 0 ? Side.Start : Side.End;
                    //System.Diagnostics.Debug.WriteLine("ChildrenX=[" + ChildrenX + "]");
                    if ((_endButtons > 0 && side == Side.End && (e.TotalDistance.X > 20 || ContentViewX > -60)) ||
                        (_startButtons > 0 && side == Side.Start && (e.TotalDistance.X < -20 || ContentViewX < 60)))
                    {
                        PutAwaySwipeButtons(true);
                        return;
                    }
                    if ((_endButtons > 0 && side == Side.End && /*_swipeFrame1.TranslationX < Width - 210 */ distance <= -210 && ((ICellSwipeMenus)ContentView)?.EndSwipeMenu != null && ((ICellSwipeMenus)ContentView).EndSwipeMenu.Count > 0 && ((ICellSwipeMenus)ContentView).EndSwipeMenu[0].IsTriggeredOnFullSwipe) ||
                        (_startButtons > 0 && side == Side.Start && /*_swipeFrame1.TranslationX > 210 - Width */ distance >= 210 && ((ICellSwipeMenus)ContentView)?.StartSwipeMenu != null && ((ICellSwipeMenus)ContentView).StartSwipeMenu.Count > 0 && ((ICellSwipeMenus)ContentView).StartSwipeMenu[0].IsTriggeredOnFullSwipe))
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
                        //TranslateContentViewTo(-(int)side * (60 * (_endButtons + _startButtons)), 0, 200, Easing.Linear);
                        ContentViewX = -(int)side * (60 * (_endButtons + _startButtons));
                        //_swipeFrame1.TranslateTo((int)side * (Width - 60), 0, 200, Easing.Linear);
                        _swipeFrame1.TranslationX = (int)side * (Width - 60);
                        if (_endButtons + _startButtons > 1)
                            //_swipeFrame2.TranslateTo((int)side * (Width - 120), 0, 200, Easing.Linear);
                            _swipeFrame2.TranslationX = (int)side * (Width - 120);
                        if (_endButtons + _startButtons > 2)
                            //_swipeFrame3.TranslateTo((int)side * (Width - 180), 0, 200, Easing.Linear);
                            _swipeFrame3.TranslationX = (int)side * (Width - 180);
                        //_insetFrame.TranslateTo((int)side * (Width - (60 * (_endButtons + _startButtons))), 0, 300, Easing.Linear);
                        _translateOnUp = (int)side * -180;
                        return;
                    }
                }

            }
        }

        bool _panHz, _panVt;
        double _homeOffset;
        void OnPanning(object sender, PanEventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPanning(sender, e));
                return;
            }

            ((ItemWrapper)BindingContext)?.OnPanning(this, new ItemWrapperPanEventArgs((ItemWrapper)BindingContext, e));
            //if (Debug)
            //    System.Diagnostics.Debug.WriteLine(P42.Utils.ReflectionExtensions.CallerMemberName() + "(" + sender + ", " + e + ")");
            if (_panVt)
                return;
            if (!_panVt && !_panHz)
            {
                if (Math.Abs(e.TotalDistance.Y) > 10)
                {
                    _panVt = true;
                    return;
                }
                if (Math.Abs(e.TotalDistance.X) > 10)
                    _panHz = true;
                else
                    return;
            }

            e.Handled = _panHz;
            var listView = this.Ancestor<ListView>();
            if (listView != null)
                listView.IsScrollEnabled = false;

            var distance = e.TotalDistance.X + _translateOnUp;
            if (_settingup)
                return;
            if (_endButtons + _startButtons > 0)
            {
                var side = _startButtons > 0 ? Side.Start : Side.End;

                if ((side == Side.End && distance <= _homeOffset) || (side == Side.Start && distance >= _homeOffset))
                {

                    //System.Diagnostics.Debug.WriteLine("=S=t=r=e=t=c=h=");
                    var parkedPosition = -(int)side * 60 * (_endButtons + _startButtons);
                    var panOffset = distance - parkedPosition;
                    ContentViewX = parkedPosition + panOffset / 2;
                    _swipeFrame1.TranslationX = (int)side * (Width + (int)side * -60 + panOffset / 3);
                    _swipeFrame2.TranslationX = (int)side * (Width + (int)side * -120 + panOffset / 6);
                    _swipeFrame3.TranslationX = (int)side * (Width + (int)side * -180 + panOffset / 6);
                    return;
                }
                if ((side == Side.End && distance > 1) || (side == Side.Start && distance < 1))
                {
                    // hey, the user is panning too far in the wrong direction
                    ContentViewX = 0;
                    return;
                }
                //System.Diagnostics.Debug.WriteLine("slide");
                ContentViewX = distance;
                _swipeFrame1.TranslationX = (int)side * (Width + (int)side * distance / (_endButtons + _startButtons));
                _swipeFrame2.TranslationX = (int)side * (Width + (int)side * 2 * distance / (_endButtons + _startButtons));
                _swipeFrame3.TranslationX = (int)side * (Width + (int)side * distance);
            }
            else if (Math.Abs(distance) > 0.1)
            {
                // setup end SwipeMenu
                var side = distance < 0 ? Side.End : Side.Start;
                _homeOffset = 0;
                if (ContentView is ICellSwipeMenus iCellSwipeMenus)
                {
                    var swipeMenu = side == Side.End ? iCellSwipeMenus.EndSwipeMenu : iCellSwipeMenus.StartSwipeMenu;
                    if (swipeMenu != null && swipeMenu.Count > 0)
                    {
                        _settingup = true;
                        _homeOffset -= _swipeButton1.Width * (int)side;

                        Children.Add(_touchBlocker, 0, 0);
                        _touchBlocker.IsVisible = true;

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
                        _swipeButton1.TextColor = swipeMenu[0].TextColor;

                        //_swipeFrame2.IsVisible = false;
                        //_swipeFrame3.IsVisible = false;

                        System.Diagnostics.Debug.WriteLine("BaseCellView.OnPanning swipeMenu.Count=[" + swipeMenu.Count + "]");
                        if (swipeMenu.Count > 1)
                        {
                            _homeOffset -= _swipeButton2.Width * (int)side;
                            System.Diagnostics.Debug.WriteLine("BaseCellView.OnPanning _swipeButton2.Width=[" + _swipeButton2.Width + "]");
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
                            _swipeButton2.TextColor = swipeMenu[1].TextColor;
                            if (swipeMenu.Count > 2)
                            {
                                _homeOffset -= _swipeButton3.Width * (int)side;
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
                                    _swipeButton3.TextColor = Color.White;
                                }
                                else
                                {
                                    _swipeFrame3.BackgroundColor = swipeMenu[2].BackgroundColor;
                                    _swipeButton3.HtmlText = swipeMenu[2].Text;
                                    _swipeButton3.IconText = swipeMenu[2].IconText;
                                    _swipeButton3.TextColor = swipeMenu[2].TextColor;
                                }
                                Children.Add(_swipeFrame3, 0, 0);
                                RaiseChild(_swipeFrame3);
                                _swipeFrame3.TranslationX = (int)side * Width;
                                _swipeFrame3.IsVisible = true;
                                _swipeButton3.IsVisible = true;
                            }
                            else
                                _swipeFrame3.IsVisible = false;

                            Children.Add(_swipeFrame2, 0, 0);
                            RaiseChild(_swipeFrame2);
                            _swipeFrame2.TranslationX = (int)side * (Width - distance / 3.0);
                            _swipeFrame2.IsVisible = true;
                            _swipeButton2.IsVisible = true;
                        }
                        else
                        {
                            _swipeFrame2.IsVisible = false;
                            _swipeFrame3.IsVisible = false;
                        }
                        Children.Add(_swipeFrame1, 0, 0);
                        RaiseChild(_swipeFrame1);
                        _swipeFrame1.TranslationX = (int)side * (Width - 2 * distance / 3.0);
                        _swipeFrame1.IsVisible = true;
                        _swipeButton1.IsVisible = true;
                        _settingup = false;
                    }

                }
            }
        }

        void PutAwaySwipeButtons(bool animated)
        {
            var listView = this.Ancestor<ListView>();
            if (listView != null)
                listView.IsScrollEnabled = true;
            if (_startButtons + _endButtons > 0)
            {
                var parkingX = _endButtons > 0 ? Width : -Width;
                TranslateContentViewTo(0, 0, 300, Easing.Linear);
                if (animated)
                {
                    _swipeFrame1.TranslateTo(parkingX, 0, 400, Easing.Linear);
                    if (_endButtons + _startButtons > 1)
                        _swipeFrame2.TranslateTo(parkingX, 0, 400, Easing.Linear);
                    if (_endButtons + _startButtons > 2)
                        _swipeFrame3.TranslateTo(parkingX, 0, 400, Easing.Linear);
                    //_insetFrame.TranslateTo(parkingX, 0, 400, Easing.Linear);
                    Device.StartTimer(TimeSpan.FromMilliseconds(400), () =>
                    {
                        _touchBlocker.IsVisible = false;
                        _swipeFrame1.HorizontalOptions = LayoutOptions.Fill;
                        _swipeFrame2.HorizontalOptions = LayoutOptions.Fill;
                        _swipeFrame3.HorizontalOptions = LayoutOptions.Fill;
                        _swipeFrame1.IsVisible = false;
                        _swipeFrame2.IsVisible = false;
                        _swipeFrame3.IsVisible = false;
                        return false;
                    });
                }
                else
                {
                    ContentViewX = 0;
                    _swipeFrame1.TranslationX = parkingX;
                    if (_endButtons + _startButtons > 1)
                        _swipeFrame2.TranslationX = parkingX;
                    if (_endButtons + _startButtons > 2)
                        _swipeFrame3.TranslationX = parkingX;
                    //_insetFrame.TranslationX = parkingX;
                    _touchBlocker.IsVisible = false;
                }
                _translateOnUp = 0;
                _endButtons = 0;
                _startButtons = 0;
            }
        }

        void OnSwipeButtonTapped(object sender, EventArgs e)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnSwipeButtonTapped(sender, e));
                return;
            }


            var listView = this.Ancestor<ListView>();
            if (listView != null)
                listView.IsScrollEnabled = true;
            var index = 0;
            if (sender == _swipeButton2)
                index = 1;
            else if (sender == _swipeButton3)
                index = 2;
            var swipeMenu = _endButtons > 0 ? ((ICellSwipeMenus)ContentView)?.EndSwipeMenu : ((ICellSwipeMenus)ContentView)?.StartSwipeMenu;
            if (index == 2 && _endButtons + _startButtons > 2)
            {
                // show remaining menu items in a modal list
                PutAwaySwipeButtons(false);
                _swipePopupCancelButton.Tapped += OnSwipePopupCancelButtonTapped;
                for (int i = 2; i < swipeMenu.Count; i++)
                {
                    var menuItem = swipeMenu[i];
                    var segment = new Segment
                    {
                        Text = menuItem.Text,
                        IconText = menuItem.IconText,
                        IconImage = menuItem.IconImage
                    };
                    segment.Tapped += OnSwipeSegmentTapped;
                    _swipeSegmentedController.Segments.Add(segment);
                }
                _swipePopup.IsVisible = true;
                //System.Diagnostics.Debug.WriteLine("SwipeMenu[More]");
            }
            else
            {
                PutAwaySwipeButtons(false);
                var args = new SwipeMenuItemTappedArgs((ICellSwipeMenus)ContentView, (ItemWrapper)BindingContext, swipeMenu[index]);
                ((ICellSwipeMenus)ContentView)?.OnSwipeMenuItemButtonTapped(this.BindingContext, args);
                ((ItemWrapper)BindingContext)?.OnSwipeMenuItemTapped(this, args);
                //System.Diagnostics.Debug.WriteLine("SwipeMenu[" + swipeMenu[index].Key + "]");
            }
        }

        async void OnSwipeSegmentTapped(object sender, EventArgs e)
        {
            if (sender is Segment segment)
            {
                await _swipePopup.CancelAsync(segment);
                var swipeMenu = _endButtons > 0 ? ((ICellSwipeMenus)ContentView)?.EndSwipeMenu : ((ICellSwipeMenus)ContentView)?.StartSwipeMenu;
                if (swipeMenu?.FirstOrDefault((m) => m.Text == segment.Text) is SwipeMenuItem menuItem)
                {
                    var args = new SwipeMenuItemTappedArgs((ICellSwipeMenus)ContentView, (ItemWrapper)BindingContext, menuItem);
                    ((ICellSwipeMenus)ContentView)?.OnSwipeMenuItemButtonTapped(this, args);
                    ((ItemWrapper)BindingContext)?.OnSwipeMenuItemTapped(this, args);
                    //System.Diagnostics.Debug.WriteLine("SwipeMenu[" + menuItem.Key + "]");
                }
                foreach (var seg in _swipeSegmentedController.Segments)
                    seg.Tapped -= OnSwipeSegmentTapped;
            }
        }

        async void OnSwipePopupCancelButtonTapped(object sender, EventArgs e)
            => await _swipePopup.CancelAsync(_swipePopupCancelButton);
        #endregion


        #region Cell Gestures
        void OnTapped(object sender, TapEventArgs e)
        {
            //if (Math.Abs(ContentViewX) > 4)
            //    return;
            PutAwaySwipeButtons(true);
            if (_endButtons + _startButtons == 0)
                ((ItemWrapper)BindingContext)?.OnTapped(this, new ItemWrapperTapEventArgs((ItemWrapper)BindingContext));
        }

        void OnLongPressed(object sender, LongPressEventArgs e)
        {
            PutAwaySwipeButtons(true);
            if (_endButtons + _startButtons == 0)
                ((ItemWrapper)BindingContext)?.OnLongPressed(this, new ItemWrapperLongPressEventArgs((ItemWrapper)BindingContext));
        }

        void OnLongPressing(object sender, LongPressEventArgs e)
        {
            if (_endButtons + _startButtons == 0)
                ((ItemWrapper)BindingContext)?.OnLongPressing(this, new ItemWrapperLongPressEventArgs((ItemWrapper)BindingContext));
        }

        async void OnRightClicked(object sender, RightClickEventArgs e)
        {
            if (_startButtons + _endButtons > 0)
                PutAwaySwipeButtons(true);
            else
            {

                if (ContentView is ICellSwipeMenus contentView)
                {
                    var segments = new List<Segment>();

                    if (contentView.EndSwipeMenu is List<SwipeMenuItem> endMenu)
                    {
                        foreach (var item in endMenu)
                        {
                            var segment = new Segment();
                            if (item.IconText != null)
                                segment.IconText = item.IconText;
                            if (item.IconImage?.Source != null)
                                segment.IconImage = item.IconImage;
                            if (item.Text != null)
                                segment.Text = item.Text;
                            if (item.HtmlText != null)
                                segment.HtmlText = item.HtmlText;
                            segment.CommandParameter = item;
                            segments.Add(segment);
                        }
                    }
                    if (contentView.StartSwipeMenu is List<SwipeMenuItem> startMenu)
                    {
                        foreach (var item in startMenu)
                        {
                            var segment = new Segment();
                            if (item.IconText != null)
                                segment.IconText = item.IconText;
                            if (item.IconImage?.Source != null)
                                segment.IconImage = item.IconImage;
                            if (item.Text != null)
                                segment.Text = item.Text;
                            if (item.HtmlText != null)
                                segment.HtmlText = item.HtmlText;
                            segment.CommandParameter = item;
                            segments.Add(segment);
                        }
                    }
                    if (segments.Count > 0)
                    {

                        using (var menu = new Forms9Patch.TargetedMenu(this, e.Center)
                        {
                            Segments = segments
                        })
                        {
                            menu.IsVisible = true;
                            await menu.WaitForPoppedAsync();
                            if (menu.SelectedSegment is Segment segment)
                            {
                                if (segment.CommandParameter is SwipeMenuItem menuItem)
                                {
                                    var args = new SwipeMenuItemTappedArgs((ICellSwipeMenus)ContentView, (ItemWrapper)BindingContext, menuItem);
                                    ((ICellSwipeMenus)ContentView)?.OnSwipeMenuItemButtonTapped(BindingContext, args);
                                    ((ItemWrapper)BindingContext)?.OnSwipeMenuItemTapped(this, args);
                                }
                            }
                        }
                    }
                }

            }
        }

        #endregion


        #region change management

        /// <summary>
        /// Triggered by a change in the binding context
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                return;
            }

            //DebugMessage("Enter BindingContext=[" + BindingContext + "]");
            if (BindingContext == null)
                return;

            if (BindingContext is ItemWrapper itemWrapper)
            {
                itemWrapper.BaseCellView = this;
                itemWrapper.PropertyChanged += OnItemPropertyChanged;
                ContentView.BindingContext = itemWrapper.Source;
                UpdateBackground();
                UpdateHeights();
                UpdateSeparator();

                if (ContentView is IIsSelectedAble contentView)
                    contentView.IsSelected = itemWrapper.IsSelected;
            }
            else
                HeightRequest = -1;

            base.OnBindingContextChanged();

            //DebugMessage("Exit");
        }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanging(propertyName));
                return;
            }

            base.OnPropertyChanging(propertyName);

            if (propertyName == BindingContextProperty.PropertyName)
            {
                if (BindingContext is ItemWrapper itemWrapper)
                {
                    itemWrapper.BaseCellView = null;
                    itemWrapper.PropertyChanged -= OnItemPropertyChanged;
                    HeightRequest = -1;
                }
                PutAwaySwipeButtons(false);
            }
            else if (propertyName == ContentViewProperty.PropertyName && ContentView != null)
            {
                ContentView.PropertyChanged -= OnContentViewPropertyChanged;
                FocusMonitor.Stop(ContentView);
                Children.Remove(ContentView);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            try
            {
                base.OnPropertyChanged(propertyName);
            }
            catch (Exception) { }

            if (propertyName == ContentViewProperty.PropertyName && ContentView != null)
            {
                FocusMonitor.Start(ContentView);
                Children.Add(ContentView, 0, 0);
                ContentView.PropertyChanged += OnContentViewPropertyChanged;
            }
        }

        private void OnContentViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == HeightRequestProperty.PropertyName || e.PropertyName == nameof(ICellContentView.CellHeight))
                UpdateHeights();
        }

        void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ItemWrapper.CellBackgroundColorProperty.PropertyName || e.PropertyName == ItemWrapper.SelectedCellBackgroundColorProperty.PropertyName)
                UpdateBackground();

            else if (e.PropertyName == ItemWrapper.IndexProperty.PropertyName || e.PropertyName == ItemWrapper.ParentProperty.PropertyName)
            {
                UpdateVisibility();
                UpdateBackground();
                UpdateHeights();
                UpdateSeparator();
            }

            else if (e.PropertyName == ItemWrapper.IsSelectedProperty.PropertyName)
            {
                UpdateBackground();
                if (ContentView is IIsSelectedAble view)
                    view.IsSelected = ((ItemWrapper)BindingContext).IsSelected;
            }

            else if (e.PropertyName == ItemWrapper.RequestedRowHeightProperty.PropertyName)
                UpdateHeights();

            else if (e.PropertyName == ItemWrapper.SeparatorVisibilityProperty.PropertyName || e.PropertyName == ItemWrapper.RequestedSeparatorHeightProperty.PropertyName || e.PropertyName == ItemWrapper.IsLastItemProperty.PropertyName)
            {
                UpdateHeights();
                UpdateSeparator();
            }

            else if (e.PropertyName == ItemWrapper.SeparatorColorProperty.PropertyName || e.PropertyName == ItemWrapper.SeparatorLeftIndentProperty.PropertyName || e.PropertyName == ItemWrapper.SeparatorRightIndentProperty.PropertyName)
                UpdateSeparator();
            //System.Diagnostics.Debug.WriteLine("OnItemPropertyChanged");
            //_freshContent = (_freshContent || e.PropertyName == ContentProperty.PropertyName);
            //UpdateLayout();
        }

        void UpdateVisibility()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (BindingContext is IItemWrapper itemWrapper && itemWrapper.Parent != null)
                        this.FadeTo(1.0);
                    else
                        this.FadeTo(0.0);
                }
            }
            else
                Device.BeginInvokeOnMainThread(UpdateVisibility);
        }

        void UpdateBackground()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (BindingContext is IItemWrapper item && item.IsSelected)
                    BackgroundColor = item.SelectedCellBackgroundColor;
                else
                    BackgroundColor = Color.Transparent;
            }
            else
                Device.BeginInvokeOnMainThread(UpdateBackground);
        }

        void UpdateHeights()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                //if (IsHeader)
                //    System.Diagnostics.Debug.WriteLine("");
                var rowHeight = RowHeight;
                if (Math.Abs(RowDefinitions[0].Height.Value - rowHeight) > 0.1)
                    RowDefinitions[0] = new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) };
                HeightRequest = rowHeight + SeparatorHeight;
                if (Parent is ICell_T_Height cell)
                    cell.Height = HeightRequest;
            }
            else
                Device.BeginInvokeOnMainThread(UpdateBackground);
        }

        void UpdateSeparator()
        {
            if (P42.Utils.Environment.IsOnMainThread)
            {
                if (BindingContext is IItemWrapper itemWrapper)
                {
                    var separatorHeight = SeparatorHeight;
                    if (Math.Abs(RowDefinitions[1].Height.Value - separatorHeight) > 0.1)
                        RowDefinitions[1] = new RowDefinition { Height = new GridLength(separatorHeight, GridUnitType.Absolute) };
                    _separator.IsVisible = !itemWrapper.IsLastItem;
                    // yes, we are keeping the height the same because, if not, the time to  refresh after ForceCellHeight on iOS gets longer and longer
                    HeightRequest = RowHeight + separatorHeight;
                    _separator.Color = itemWrapper.SeparatorColor;
                    _separator.Margin = new Thickness(itemWrapper.SeparatorLeftIndent, 0, itemWrapper.SeparatorRightIndent, 0);
                    _separator.HeightRequest = separatorHeight;
                }
            }
            else
                Device.BeginInvokeOnMainThread(UpdateBackground);
        }

        public double CellHeight
        {
            get
            {
                if (ContentView is ICellContentView contentView)
                    return contentView.CellHeight;
                return 90;
            }
        }
        #endregion


        #region Appearing / Dissapearing Handlers
        public void OnAppearing()
        {
            if (ContentView is ICellContentView contentView)
                contentView.OnAppearing();
        }

        public void OnDisappearing()
        {
            if (ContentView is ICellContentView contentView)
                contentView.OnDisappearing();
        }
        #endregion
    }
}

