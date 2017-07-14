using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Image button.
    /// </summary>
    public class ImageButton : MaterialButton
    {

        #region Properties
        /// <summary>
        /// Backing store for the DefaultState bindable property.
        /// </summary>
        public static BindableProperty DefaultStateProperty = BindableProperty.Create("DefaultState", typeof(ImageButtonState), typeof(ImageButton), null);
        /// <summary>
        /// Gets or sets the ImageButton's properties for the default button state.
        /// </summary>
        /// <value>The ImageButtonState structure for the default button state.</value>
        public ImageButtonState DefaultState
        {
            get { return (ImageButtonState)GetValue(DefaultStateProperty); }
            set { SetValue(DefaultStateProperty, value); }
        }

        /// <summary>
        /// Backing store for the PressingState bindable property.
        /// </summary>
        public static BindableProperty PressingStateProperty = BindableProperty.Create("PressingState", typeof(ImageButtonState), typeof(Button), null);
        /// <summary>
        /// Gets or sets the ImageButton's properties for the pressing button state.
        /// </summary>
        /// <value>The ImageButtonState structure for the pressing button state.</value>
        public ImageButtonState PressingState
        {
            get { return (ImageButtonState)GetValue(PressingStateProperty); }
            set { SetValue(PressingStateProperty, value); }
        }

        /// <summary>
        /// Backing store for the SelectedState bindable property.
        /// </summary>
        public static BindableProperty SelectedStateProperty = BindableProperty.Create("SelectedState", typeof(ImageButtonState), typeof(ImageButton), null);
        /// <summary>
        /// Gets or sets the ImageButton's properties for the selected button state.
        /// </summary>
        /// <value>The ImageButtonState structure for the selected button state.</value>
        public ImageButtonState SelectedState
        {
            get { return (ImageButtonState)GetValue(SelectedStateProperty); }
            set { SetValue(SelectedStateProperty, value); }
        }

        /// <summary>
        /// Backing store for the DisabledState bindable property.
        /// </summary>
        public static BindableProperty DisabledStateProperty = BindableProperty.Create("DisabledState", typeof(ImageButtonState), typeof(ImageButton), null);
        /// <summary>
        /// Gets or sets the ImageButton's properties for the disabled button state.
        /// </summary>
        /// <value>The ImageButtonState structure for the disabled button state.</value>
        public ImageButtonState DisabledState
        {
            get { return (ImageButtonState)GetValue(DisabledStateProperty); }
            set { SetValue(DisabledStateProperty, value); }
        }

        /// <summary>
        /// Backing store for the DisabledAndSelectedState bindable property.
        /// </summary>
        public static BindableProperty DisabledAndSelectedStateProperty = BindableProperty.Create("DisabledAndSelectedState", typeof(ImageButtonState), typeof(ImageButton), null);
        /// <summary>
        /// Gets or sets the ImageButton's properties for the disabled and selected button state.
        /// </summary>
        /// <value>The ImageButtonState structure for the disabled and selected button state.</value>
        public ImageButtonState DisabledAndSelectedState
        {
            get { return (ImageButtonState)GetValue(DisabledAndSelectedStateProperty); }
            set { SetValue(DisabledAndSelectedStateProperty, value); }
        }
        #endregion



        #region Fields
        bool _noUpdate = true;
        ImageButtonState _currentState;
        //Xamarin.Forms.StackLayout _stackLayout;
        //Xamarin.Forms.Image _image;
        //Label _label;
        //FormsGestures.Listener _gestureListener;
        #endregion




        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.ImageButton"/> class.
        /// </summary>
        public ImageButton() : base()
        {
            _constructing = true;
            DefaultState = new ImageButtonState();

            _noUpdate = false;
            ShowState(DefaultState);

            _gestureListener.Up += OnUp;
            _gestureListener.Down += OnDown;

            _gestureListener.LongPressed += OnLongPressed;
            _gestureListener.LongPressing += OnLongPressing;

            _constructing = false;
        }
        #endregion


        #region IDisposable Support

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _gestureListener.Up -= OnUp;
                    _gestureListener.Down -= OnDown;
                    _gestureListener.LongPressed -= OnLongPressed;
                    _gestureListener.LongPressing -= OnLongPressing;
                    //_gestureListener.Dispose();
                    //_gestureListener = null;
                    _currentState.PropertyChanged -= OnStatePropertyChanged;
                    _currentState = null;
                    disposedValue = true;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

            }
            base.Dispose(disposing);
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageButton() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Releases all resource used by the <see cref="T:Forms9Patch.ImageButton"/> object.
        /// </summary>
        /// <remarks>Call <see cref="O:Forms9Patch.ImageButton.Dispose"/> when you are finished using the <see cref="T:Forms9Patch.ImageButton"/>. The
        /// <see cref="O:Forms9Patch.ImageButton.Dispose"/> method leaves the <see cref="T:Forms9Patch.ImageButton"/> in an unusable state. After
        /// calling <see cref="O:Forms9Patch.ImageButton.Dispose"/>, you must release all references to the <see cref="T:Forms9Patch.ImageButton"/> so
        /// the garbage collector can reclaim the memory that the <see cref="T:Forms9Patch.ImageButton"/> was occupying.</remarks>
        public new void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion



        #region State Change responders
        /// <summary>
        /// Redraws the button to the current state: Default, Selected, Disabled or DisabledAndSelected.
        /// </summary>
        public void UpdateState()
        {
            if (IsEnabled)
            {
                if (IsSelected)
                {
                    ShowState(SelectedState ?? DefaultState);
                    if (SelectedState == null)
                    {
                        _label.FontAttributes = FontAttributes.Bold;
                    }
                }
                else
                    ShowState(DefaultState);
                Opacity = 1.0;
            }
            else
            {
                if (IsSelected)
                {
                    ShowState(DisabledAndSelectedState ?? SelectedState ?? DisabledState ?? DefaultState);
                    if (DisabledAndSelectedState == null)
                    {
                        if (SelectedState != null)
                        {
                            Opacity = 0.75;
                        }
                        else if (DisabledState != null)
                        {
                            _label.FontAttributes = FontAttributes.Bold;
                        }
                        else
                        {
                            Opacity = 0.75;
                            _label.FontAttributes = FontAttributes.Bold;
                        }
                    }
                }
                else
                {
                    ShowState(DisabledState ?? DefaultState);
                    if (DisabledState == null)
                        Opacity = 0.75;
                }
            }
        }

        /// <summary>
        /// Redraws the button to a custom ImageButtonState
        /// </summary>
        /// <param name="newState">Custom ImageButtonState.</param>
        public void ShowState(ImageButtonState newState)
        {
            newState = newState ?? DefaultState;
            if (_currentState != null)
                _currentState.PropertyChanged -= OnStatePropertyChanged;
            _currentState = newState;
            var newBackgroundImage = _currentState.BackgroundImage ?? DefaultState.BackgroundImage;
            if (newBackgroundImage != BackgroundImage)
            {
                if (newBackgroundImage != null)
                    newBackgroundImage.Opacity = 1.0;
                BackgroundImage = newBackgroundImage;
            }
            //else if (!_constructing && Device.OS == TargetPlatform.Android)
            else if (!_constructing && Device.RuntimePlatform == Device.Android)
            {
                // this is a hack that compensates for a failure to resize the label when > 4 ImageButtons are on a ContentPage inside a NavigationPage
                BackgroundImage = null;
                if (newBackgroundImage != null)
                    Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                    {
                        BackgroundImage = newBackgroundImage;
                        return false;
                    });
            }
            BackgroundColor = _currentState.BackgroundColorSet ? _currentState.BackgroundColor : DefaultState.BackgroundColor;
            var newImage = _currentState.Image ?? DefaultState.Image;
            var htmlText = _currentState.HtmlText ?? DefaultState.HtmlText;
            var text = htmlText ?? _currentState.Text ?? DefaultState.Text;
            if (newImage == null)
            {
                // label only
                if (_image != null)
                {
                    var toDispose = _image;
                    if (toDispose != null)
                        Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                        {
                            toDispose.Opacity -= 0.25;
                            if (toDispose.Opacity > 0)
                                return true;
                            _stackLayout.Children.Remove(toDispose);
                            toDispose.Opacity = 1.0;
                            return false;
                        });
                }
                _image = null;
                if (!string.IsNullOrEmpty(text) && !_stackLayout.Children.Contains(_label))
                {
                    if (TrailingImage)
                        _stackLayout.Children.Insert(0, _label);
                    else
                        _stackLayout.Children.Add(_label);
                }
            }
            else
            {
                // there is an image
                newImage.Opacity = 1.0;
                if (_image != newImage)
                {
                    // if it is a new image
                    if (_image != null)
                        _stackLayout.Children.Remove(_image);
                    _image = newImage;
                    if (TrailingImage)
                        _stackLayout.Children.Add(_image);
                    else
                        _stackLayout.Children.Insert(0, _image);
                }
                if (string.IsNullOrEmpty(text))
                {
                    // no Label, just an image
                    if (_stackLayout.Children.Contains(_label))
                        _stackLayout.Children.Remove(_label);
                }
                else
                {
                    // image and label
                    if (!_stackLayout.Children.Contains(_label))
                    {
                        if (TrailingImage)
                            _stackLayout.Children.Insert(0, _label);
                        else
                            _stackLayout.Children.Add(_label);
                    }
                }
            }
            if (string.IsNullOrEmpty(htmlText))
                _label.Text = _currentState.Text ?? DefaultState.Text;
            else
                _label.HtmlText = htmlText;
            SetLabelState(_label, _currentState);
            _currentState.PropertyChanged += OnStatePropertyChanged;
            //InvalidateMeasure ();

        }

        void SetLabelState(Label label, ImageButtonState state)
        {
            //label.TextColor = (state.FontColorSet || state.FontColor != (Color)ImageButtonState.FontColorProperty.DefaultValue ? state.FontColor : (DefaultState.FontColorSet || DefaultState.FontColor != (Color)ImageButtonState.FontColorProperty.DefaultValue ? DefaultState.FontColor : ( Device.OS == TargetPlatform.iOS ? Color.Blue : Color.White)));
            label.TextColor = (state.FontColorSet || state.FontColor != (Color)ImageButtonState.FontColorProperty.DefaultValue ? state.FontColor : (DefaultState.FontColorSet || DefaultState.FontColor != (Color)ImageButtonState.FontColorProperty.DefaultValue ? DefaultState.FontColor : (Device.RuntimePlatform == Device.iOS ? Color.Blue : Color.White)));
            label.FontSize = (state.FontSizeSet || Math.Abs(state.FontSize - (double)ImageButtonState.FontSizeProperty.DefaultValue) > 0.01 ? state.FontSize : (DefaultState.FontSizeSet || Math.Abs(DefaultState.FontSize - (double)ImageButtonState.FontSizeProperty.DefaultValue) > 0.01 ? DefaultState.FontSize : Device.GetNamedSize(NamedSize.Medium, _label)));
            label.FontFamily = (state.FontFamilySet || state.FontFamily != (string)ImageButtonState.FontFamilyProperty.DefaultValue ? state.FontFamily : DefaultState.FontFamily);
            label.FontAttributes = (state.FontAttributesSet || state.FontAttributes != (FontAttributes)ImageButtonState.FontAttributesProperty.DefaultValue ? state.FontAttributes : DefaultState.FontAttributes);
        }
        #endregion


        #region Gesture Event Responders
        /// <summary>
        /// Tap this instance.
        /// </summary>
        public new void Tap()
        {
            base.Tap();
            OnUp(this, new FormsGestures.DownUpEventArgs(null, null));
        }

        void OnUp(object sender, FormsGestures.DownUpEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Up");
            if (IsEnabled)
            {
                KeyClicksService.Feedback(HapticEffect, HapticMode);

                System.Diagnostics.Debug.WriteLine("tapped");
                if (ToggleBehavior)
                {
                    IsSelected = !IsSelected;
                    UpdateState();
                    if (IsSelected)
                        //Selected?.Invoke(this, EventArgs.Empty);
                        InvokeSelected(this, EventArgs.Empty);
                    if (PressingState?.BackgroundImage == null)
                    {
                        Opacity = 0.5;
                        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                        {
                            Opacity += 0.1;
                            return Opacity < 1.0;
                        });
                    }
                }
                else
                {
                    if (PressingState != null && PressingState.Image != null)
                        Device.StartTimer(TimeSpan.FromMilliseconds(20), () =>
                        {
                            UpdateState();
                            return false;
                        });
                    else
                    {
                        Opacity = 0.5;
                        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                        {
                            Opacity += 0.1;
                            return Opacity < 1.0;
                        });
                    }
                }
                SendTapped();
            }
        }

        void OnDown(object sender, FormsGestures.DownUpEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Down");
            if (IsEnabled)
                ShowState(PressingState);
        }

        void OnLongPressing(object sender, FormsGestures.LongPressEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing");
            if (IsEnabled)
                //LongPressing?.Invoke(this, EventArgs.Empty);
                InvokeLongPressing(this, EventArgs.Empty);
        }

        void OnLongPressed(object sender, FormsGestures.LongPressEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed");
            if (IsEnabled)
            {
                //LongPressed?.Invoke(this, EventArgs.Empty);
                InvokeLongPressed(this, EventArgs.Empty);
                UpdateState();
            }
        }
        #endregion


        #region Change Handlers
        void OnStatePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnStatePropertyChanged");
            if (sender == _currentState)
            {
                System.Diagnostics.Debug.WriteLine("\t" + e.PropertyName);
                UpdateState();
                //if (e.PropertyName == ImageButtonState.TextProperty.PropertyName || e.PropertyName == ImageButtonState.HtmlTextProperty.PropertyName)
                //_stackLayout.
                //System.Diagnostics.Debug.WriteLine("");
                //_stackLayout.UpdateLayout();
            }
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (_noUpdate)
                return;

            if (propertyName == ImageButton.DefaultStateProperty.PropertyName && IsEnabled && !IsSelected)
                UpdateState();
            //          } else if (propertyName == Button.PressingStateProperty.PropertyName && PressingState != null) {
            //              PressingState.PropertyChanged += OnStatePropertyChanged;
            //              SetupPressingState ();
            else if (propertyName == IsSelectedProperty.PropertyName && IsEnabled && IsSelected)
                UpdateState();
            else if (propertyName == DisabledStateProperty.PropertyName && !IsEnabled && !IsSelected)
                UpdateState();
            else if (propertyName == DisabledStateProperty.PropertyName && !IsEnabled && IsSelected)
                UpdateState();
            //} else if (propertyName == Button.PaddingProperty.PropertyName) {
            //  _stackLayout.Padding = Padding;
            //else if (propertyName == ImageButton.AlignmentProperty.PropertyName)
            //   _stackLayout.HorizontalOptions = Alignment.ToLayoutOptions();
            else if (propertyName == IsEnabledProperty.PropertyName || propertyName == IsSelectedProperty.PropertyName)
                UpdateState();
            //else if (propertyName == ImageButton.LinesProperty.PropertyName)
            //    _label.Lines = Lines;
            //else if (propertyName == ImageButton.FitProperty.PropertyName)
            //    _label.Fit = Fit;
            //else if (propertyName == ImageButton.LineBreakModeProperty.PropertyName)
            //    _label.LineBreakMode = LineBreakMode;

            base.OnPropertyChanged(propertyName);

            if (propertyName == TextProperty.PropertyName)
            {
                DefaultState.Text = Text;
                UpdateState();
            }
            else if (propertyName == HtmlTextProperty.PropertyName)
            {
                DefaultState.HtmlText = HtmlText;
                UpdateState();
            }
            else if (propertyName == ImageSourceProperty.PropertyName)
            {
                DefaultState.Image = new Image
                {
                    Source = ImageSource
                };
                UpdateState();
            }
            else if (propertyName == FontColorProperty.PropertyName)
            {
                DefaultState.FontColor = FontColor;
                UpdateState();
            }
            else if (propertyName == FontAttributesProperty.PropertyName)
            {
                DefaultState.FontAttributes = FontAttributes;
                UpdateState();
            }
            else if (propertyName == FontSizeProperty.PropertyName)
            {
                DefaultState.FontSize = FontSize;
                UpdateState();
            }
            else if (propertyName == FontFamilyProperty.PropertyName)
            {
                DefaultState.FontFamily = FontFamily;
                UpdateState();
            }
            else if (propertyName == BackgroundColorProperty.PropertyName)
            {
                DefaultState.BackgroundColor = BackgroundColor;
                UpdateState();
            }
            else if (propertyName == BackgroundImageProperty.PropertyName)
            {
                DefaultState.BackgroundImage = BackgroundImage;
                UpdateState();
            }


        }

        #endregion



    }
}
