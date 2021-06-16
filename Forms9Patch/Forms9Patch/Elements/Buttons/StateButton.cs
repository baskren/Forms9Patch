using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Image button.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class StateButton : Button
    {

        #region Overridden Properties

        #region Text
        /// <summary>
        /// Backing store for the Button.Text bindable property.
        /// </summary>
        public static new readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Button), null);
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public new string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion Text

        #region HtmlText
        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static new readonly BindableProperty HtmlTextProperty = BindableProperty.Create(nameof(HtmlText), typeof(string), typeof(Button), null);
        /// <summary>
        /// Gets or sets the formatted text.
        /// </summary>
        /// <value>The formatted text.</value>
        public new string HtmlText
        {
            get => (string)GetValue(HtmlTextProperty);
            set => SetValue(HtmlTextProperty, value);
        }
        #endregion HtmlText

        #region IconImage
        /// <summary>
        /// Backing store for the IconImage property
        /// </summary>
        public static new BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(Forms9Patch.Image), typeof(Button), null);
        /// <summary>
        /// Gets or sets the icon image.  Alternatively, use IconText
        /// </summary>
        public new Forms9Patch.Image IconImage
        {
            get => (Forms9Patch.Image)GetValue(IconImageProperty);
            set => SetValue(IconImageProperty, value);
        }
        #endregion IconImage

        #region IconText
        /// <summary>
        /// The image text property backing store
        /// </summary>
        public static new readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(Button), default(string));
        /// <summary>
        /// Gets or sets the image text - use this to specify the image as an HTML markup string.
        /// </summary>
        /// <value>The image text.</value>
        public new string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }
        #endregion IconText

        #endregion


        #region State Properties
        /// <summary>
        /// Backing store for the DefaultState bindable property.
        /// </summary>
        public static BindableProperty DefaultStateProperty = BindableProperty.Create(nameof(DefaultState), typeof(ButtonState), typeof(StateButton), null);
        /// <summary>
        /// Gets or sets the StateButton's properties for the default button state.
        /// </summary>
        /// <value>The ButtonState structure for the default button state.</value>
        public ButtonState DefaultState
        {
            get => (ButtonState)GetValue(DefaultStateProperty);
            set => SetValue(DefaultStateProperty, value);
        }

        /// <summary>
        /// Backing store for the PressingState bindable property.
        /// </summary>
        public static BindableProperty PressingStateProperty = BindableProperty.Create(nameof(PressingState), typeof(ButtonState), typeof(Button), null);
        /// <summary>
        /// Gets or sets the StateButton's properties for the pressing button state.
        /// </summary>
        /// <value>The ButtonState structure for the pressing button state.</value>
        public ButtonState PressingState
        {
            get => (ButtonState)GetValue(PressingStateProperty);
            set => SetValue(PressingStateProperty, value);
        }

        /// <summary>
        /// Backing store for the SelectedState bindable property.
        /// </summary>
        public static BindableProperty SelectedStateProperty = BindableProperty.Create(nameof(SelectedState), typeof(ButtonState), typeof(StateButton), null);
        /// <summary>
        /// Gets or sets the StateButton's properties for the selected button state.
        /// </summary>
        /// <value>The ButtonState structure for the selected button state.</value>
        public ButtonState SelectedState
        {
            get => (ButtonState)GetValue(SelectedStateProperty);
            set => SetValue(SelectedStateProperty, value);
        }

        /// <summary>
        /// Backing store for the DisabledState bindable property.
        /// </summary>
        public static BindableProperty DisabledStateProperty = BindableProperty.Create(nameof(DisabledState), typeof(ButtonState), typeof(StateButton), null);
        /// <summary>
        /// Gets or sets the StateButton's properties for the disabled button state.
        /// </summary>
        /// <value>The ButtonState structure for the disabled button state.</value>
        public ButtonState DisabledState
        {
            get => (ButtonState)GetValue(DisabledStateProperty);
            set => SetValue(DisabledStateProperty, value);
        }

        /// <summary>
        /// Backing store for the DisabledAndSelectedState bindable property.
        /// </summary>
        public static BindableProperty DisabledAndSelectedStateProperty = BindableProperty.Create(nameof(DisabledAndSelectedState), typeof(ButtonState), typeof(StateButton), null);
        /// <summary>
        /// Gets or sets the StateButton's properties for the disabled and selected button state.
        /// </summary>
        /// <value>The ButtonState structure for the disabled and selected button state.</value>
        public ButtonState DisabledAndSelectedState
        {
            get => (ButtonState)GetValue(DisabledAndSelectedStateProperty);
            set => SetValue(DisabledAndSelectedStateProperty, value);
        }
        #endregion


        #region Fields
        readonly bool _noUpdate = true;
        ButtonState _currentState;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.StateButton"/> class.
        /// </summary>
        public StateButton()
        {
            _constructing = true;
            DefaultState = new ButtonState();
            _noUpdate = false;
            ShowState(DefaultState);
            _constructing = false;


        }
        #endregion


        #region IDisposable Support
        bool _disposed;
        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _currentState.PropertyChanged -= OnStatePropertyChanged;
                _currentState = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when the button is disposed
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region State Change responders
        /// <summary>
        /// Redraws the button to the current state: Default, Selected, Disabled or DisabledAndSelected.
        /// </summary>
        public void UpdateState()
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {

                if (IsEnabled)
                {
                    if (IsSelected)
                    {
                        ShowState(SelectedState ?? DefaultState);
                        if (SelectedState == null)
                            _label.FontAttributes = FontAttributes.Bold;
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
                                Opacity = 0.75;
                            else if (DisabledState != null)
                                _label.FontAttributes = FontAttributes.Bold;
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
            });
        }

        bool _showingState;
        /// <summary>
        /// Redraws the button to a custom ButtonState
        /// </summary>
        /// <param name="newState">Custom ButtonState.</param>
        public void ShowState(ButtonState newState)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                _showingState = true;

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
                else if (_currentState == DefaultState && DefaultState.BackgroundImage == null && BackgroundImage != null)
                    DefaultState.BackgroundImage = BackgroundImage;
                //else if (!_constructing && Device.OS == TargetPlatform.Android)
                /*  NOTE: Commented out on 12/18/17 because was causing flicker upon button press in Android.  Tested on XamlStateButtonsPage and did not see failure to resize.
                else if (!_constructing && Device.RuntimePlatform == Device.Android)
                {
                    // this is a hack that compensates for a failure to resize the label when > 4 ImageButtons are on a ContentPage inside a NavigationPage
                    BackgroundImage = null;
                    if (newBackgroundImage != null)
                        Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                        {
                            _showingState = true;
                            BackgroundImage = newBackgroundImage;
                            _showingState = false;
                            return false;
                        });
                }
                */
                var backgroundColor = _currentState.BackgroundColorSet ? _currentState.BackgroundColor : DefaultState.BackgroundColor;
                SetValue(ShapeBase.BackgroundColorProperty, backgroundColor);

                var newIconImage = _currentState.IconImage ?? DefaultState.IconImage;
                var htmlText = _currentState.HtmlText ?? DefaultState.HtmlText;
                var text = htmlText ?? _currentState.Text ?? DefaultState.Text;
                var iconText = _currentState.IconText ?? DefaultState.IconText;

                if (base.IconText == iconText && base.IconImage != newIconImage)
                    base.IconImage = newIconImage;
                else if (base.IconText != iconText && base.IconImage == newIconImage)
                    base.IconText = iconText;
                else if (base.IconText != iconText && base.IconImage != newIconImage)
                {
                    if (iconText != null)
                        base.IconText = iconText;
                    else if (newIconImage != null)
                        base.IconImage = newIconImage;
                    else
                    {
                        base.IconText = null;
                        base.IconImage = null;
                    }
                }

                base.HtmlText = htmlText;
                if (base.HtmlText == null)
                    base.Text = text;

                #region IButtonState

                TrailingIcon = _currentState.TrailingIconSet ? _currentState.TrailingIcon : DefaultState.TrailingIcon;

                UpdateIconTint();

                HasTightSpacing = _currentState.HasTightSpacingSet ? _currentState.HasTightSpacing : DefaultState.HasTightSpacing;

                Spacing = _currentState.SpacingSet ? _currentState.Spacing : DefaultState.Spacing;

                Orientation = _currentState.OrientationSet ? _currentState.Orientation : DefaultState.Orientation;

                ElementShape = _currentState.ElementShapeSet ? _currentState.ElementShape : DefaultState.ElementShape;

                #region IBackground

                // BackgroundImage handed above

                #region IShape

                // BackgroundColor handled above

                var hasShadow = _currentState.HasShadowSet ? _currentState.HasShadow : DefaultState.HasShadow;
                SetValue(ShapeBase.HasShadowProperty, hasShadow);

                ShadowInverted = _currentState.ShadowInvertedSet ? _currentState.ShadowInverted : DefaultState.ShadowInverted;

                var outlineColor = _currentState.OutlineColorSet ? _currentState.OutlineColor : DefaultState.OutlineColor;
                SetValue(ShapeBase.OutlineColorProperty, outlineColor);

                OutlineRadius = _currentState.OutlineRadiusSet ? _currentState.OutlineRadius : DefaultState.OutlineRadius;

                var outlineWidth = _currentState.OutlineWidthSet ? _currentState.OutlineWidth : DefaultState.OutlineWidth;
                SetValue(ShapeBase.OutlineWidthProperty, outlineWidth);

                // ExtendedElementShape is set by ElementShape setter, above

                #endregion IShape

                #endregion IBackground

                #region ILabel

                // Text handled above

                // HtmlText handled above

                _label.TextColor = (_currentState.TextColorSet || _currentState.TextColor != (Color)ButtonState.TextColorProperty.DefaultValue ? _currentState.TextColor : (DefaultState.TextColorSet || DefaultState.TextColor != (Color)ButtonState.TextColorProperty.DefaultValue ? DefaultState.TextColor : (Device.RuntimePlatform == Device.iOS ? Color.Blue : Color.White)));

                HorizontalTextAlignment = _currentState.HorizontalTextAlignmentSet ? _currentState.HorizontalTextAlignment : DefaultState.HorizontalTextAlignment;

                VerticalTextAlignment = _currentState.VerticalTextAlignmentSet ? _currentState.VerticalTextAlignment : DefaultState.VerticalTextAlignment;

                LineBreakMode = _currentState.LineBreakModeSet ? _currentState.LineBreakMode : DefaultState.LineBreakMode;

                AutoFit = _currentState.AutoFitSet ? _currentState.AutoFit : DefaultState.AutoFit;

                Lines = _currentState.LinesSet ? _currentState.Lines : DefaultState.Lines;

                MinFontSize = _currentState.MinFontSizeSet ? _currentState.MinFontSize : DefaultState.MinFontSize;

                #region IFontElement

                _label.FontSize = (_currentState.FontSizeSet || Math.Abs(_currentState.FontSize - (double)ButtonState.FontSizeProperty.DefaultValue) > 0.01 ? _currentState.FontSize : (DefaultState.FontSizeSet || Math.Abs(DefaultState.FontSize - (double)ButtonState.FontSizeProperty.DefaultValue) > 0.01 ? DefaultState.FontSize : Device.GetNamedSize(NamedSize.Medium, _label)));

                _label.FontFamily = (_currentState.FontFamilySet || _currentState.FontFamily != (string)ButtonState.FontFamilyProperty.DefaultValue ? _currentState.FontFamily : DefaultState.FontFamily);

                _label.FontAttributes = (_currentState.FontAttributesSet || _currentState.FontAttributes != (FontAttributes)ButtonState.FontAttributesProperty.DefaultValue ? _currentState.FontAttributes : DefaultState.FontAttributes);

                if (_iconLabel != null)
                {
                    _iconLabel.FontSize = (_currentState.IconFontSizeSet || Math.Abs(_currentState.IconFontSize - (double)ButtonState.IconFontSizeProperty.DefaultValue) > 0.01 ? _currentState.IconFontSize : (DefaultState.IconFontSizeSet || Math.Abs(DefaultState.IconFontSize - (double)ButtonState.FontSizeProperty.DefaultValue) > 0.01 ? DefaultState.IconFontSize : DefaultState.FontSize));

                    _iconLabel.FontFamily = (_currentState.IconFontFamilySet || _currentState.IconFontFamily != (string)ButtonState.IconFontFamilyProperty.DefaultValue ? _currentState.IconFontFamily : DefaultState.FontFamily);
                }

                #endregion IFontElement



                #endregion ILabel

                #endregion IButtonState

                _currentState.PropertyChanged += OnStatePropertyChanged;

                _showingState = false;
            });
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

        /// <summary>
        /// Called when the button is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUp(object sender, FormsGestures.DownUpEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            //base.OnUp(sender, e);
            //System.Diagnostics.Debug.WriteLine("Up");
            if (IsEnabled)
            {
                //KeyClicksService.Feedback(HapticEffect, HapticEffectMode);
                Feedback.Play(HapticEffect, HapticEffectMode);
                Feedback.Play(SoundEffect, SoundEffectMode);

                //System.Diagnostics.Debug.WriteLine("tapped");
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
                    if (PressingState != null && PressingState.IconImage != null)
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
                //SendTapped();  // handled by base class
            }
        }

        /// <summary>
        /// Called when the button is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDown(object sender, FormsGestures.DownUpEventArgs e)
        {
            base.OnDown(sender, e);
            //System.Diagnostics.Debug.WriteLine("Down");
            if (IsEnabled)
                ShowState(PressingState);
        }

        /// <summary>
        /// Called when the button is in long press state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLongPressing(object sender, FormsGestures.LongPressEventArgs e)
        {
            base.OnLongPressing(sender, e);
            //System.Diagnostics.Debug.WriteLine("LongPressing");
            if (IsEnabled && IsLongPressEnabled)
                //LongPressing?.Invoke(this, EventArgs.Empty);
                InvokeLongPressing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the button is released from a long press state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLongPressed(object sender, FormsGestures.LongPressEventArgs e)
        {
            base.OnLongPressed(sender, e);
            //System.Diagnostics.Debug.WriteLine("LongPressed");
            if (IsEnabled && IsLongPressEnabled)
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
            //System.Diagnostics.Debug.WriteLine("OnStatePropertyChanged");
            if (sender == _currentState)
            {
                //System.Diagnostics.Debug.WriteLine("\t" + e.PropertyName);
                UpdateState();
                //if (e.PropertyName == ButtonState.TextProperty.PropertyName || e.PropertyName == ButtonState.HtmlTextProperty.PropertyName)
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
            /*
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }
            */

            base.OnPropertyChanged(propertyName);

            if (_noUpdate)
                return;


            #region State changes
            if (propertyName == StateButton.DefaultStateProperty.PropertyName && IsEnabled && !IsSelected)
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
            else if (propertyName == IsEnabledProperty.PropertyName || propertyName == IsSelectedProperty.PropertyName)
                UpdateState();
            #endregion


            #region IButtonState

            if (propertyName == IconImageProperty.PropertyName && !_showingState)
            {
                DefaultState.IconImage = IconImage;
                UpdateState();
            }
            else if (propertyName == IconTextProperty.PropertyName && !_showingState)
            {
                DefaultState.IconText = IconText;
                UpdateState();
            }
            else if (propertyName == TrailingIconProperty.PropertyName && !_showingState)
            {
                DefaultState.TrailingIcon = TrailingIcon;
                UpdateState();
            }
            else if (propertyName == TintIconProperty.PropertyName && !_showingState)
            {
                DefaultState.TintIcon = TintIcon;
                UpdateState();
            }
            else if (propertyName == HasTightSpacingProperty.PropertyName && !_showingState)
            {
                DefaultState.HasTightSpacing = HasTightSpacing;
                UpdateState();
            }
            else if (propertyName == SpacingProperty.PropertyName && !_showingState)
            {
                DefaultState.Spacing = Spacing;
                UpdateState();
            }
            else if (propertyName == OrientationProperty.PropertyName && !_showingState)
            {
                DefaultState.Orientation = Orientation;
                UpdateState();
            }

            #region IBackground

            else if (propertyName == ShapeBase.BackgroundImageProperty.PropertyName && !_showingState)
            {
                DefaultState.BackgroundImage = BackgroundImage;
                UpdateState();
            }

            #region IShape
            else if (propertyName == BackgroundColorProperty.PropertyName && !_showingState)
            {
                DefaultState.BackgroundColor = BackgroundColor;
                UpdateState();
            }
            else if (propertyName == HasShadowProperty.PropertyName && !_showingState)
            {
                DefaultState.HasShadow = HasShadow;
                UpdateState();
            }
            else if (propertyName == ShadowInvertedProperty.PropertyName && !_showingState)
            {
                DefaultState.ShadowInverted = ShadowInverted;
                UpdateState();
            }
            else if (propertyName == OutlineColorProperty.PropertyName && !_showingState)
            {
                DefaultState.OutlineColor = OutlineColor;
                UpdateState();
            }
            else if (propertyName == OutlineRadiusProperty.PropertyName && !_showingState)
            {
                DefaultState.OutlineRadius = OutlineRadius;
                UpdateState();
            }
            else if (propertyName == OutlineWidthProperty.PropertyName && !_showingState)
            {
                DefaultState.OutlineWidth = OutlineWidth;
                UpdateState();
            }
            #endregion IShape

            #endregion IBackground

            #region ILabel
            else if (propertyName == TextProperty.PropertyName && !_showingState)
            {
                DefaultState.Text = Text;
                UpdateState();
            }
            else if (propertyName == HtmlTextProperty.PropertyName && !_showingState)
            {
                DefaultState.HtmlText = HtmlText;
                UpdateState();
            }
            else if (propertyName == TextColorProperty.PropertyName && !_showingState)
            {
                DefaultState.TextColor = TextColor;
                UpdateState();
            }
            if (propertyName == HorizontalTextAlignmentProperty.PropertyName && !_showingState)
            {
                DefaultState.HorizontalTextAlignment = HorizontalTextAlignment;
                UpdateState();
            }
            else if (propertyName == VerticalTextAlignmentProperty.PropertyName && !_showingState)
            {
                DefaultState.VerticalTextAlignment = VerticalTextAlignment;
                UpdateState();
            }
            if (propertyName == LineBreakModeProperty.PropertyName && !_showingState)
            {
                DefaultState.LineBreakMode = LineBreakMode;
                UpdateState();
            }
            else if (propertyName == AutoFitProperty.PropertyName && !_showingState)
            {
                DefaultState.AutoFit = AutoFit;
                UpdateState();
            }
            if (propertyName == LinesProperty.PropertyName && !_showingState)
            {
                DefaultState.Lines = Lines;
                UpdateState();
            }
            else if (propertyName == MinFontSizeProperty.PropertyName && !_showingState)
            {
                DefaultState.MinFontSize = MinFontSize;
                UpdateState();
            }

            #region IFontElement
            else if (propertyName == FontAttributesProperty.PropertyName && !_showingState)
            {
                DefaultState.FontAttributes = FontAttributes;
                UpdateState();
            }
            else if (propertyName == FontSizeProperty.PropertyName && !_showingState)
            {
                DefaultState.FontSize = FontSize;
                UpdateState();
            }
            else if (propertyName == FontFamilyProperty.PropertyName && !_showingState)
            {
                DefaultState.FontFamily = FontFamily;
                UpdateState();
            }
            else if (propertyName == IconFontFamilyProperty.PropertyName && !_showingState)
            {
                DefaultState.IconFontFamily = IconFontFamily;
                UpdateState();
            }
            else if (propertyName == IconFontSizeProperty.PropertyName && !_showingState)
            {
                DefaultState.IconFontSize = IconFontSize;
                UpdateState();
            }
            #endregion IFontElement

            #endregion ILabel

            #endregion IButtonState

        }
        #endregion

    }
}
