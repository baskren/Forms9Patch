using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch ImageButton.
	/// </summary>
	public class ImageButton : ContentView, IDisposable
	{
		
		#region Properties
		/*
		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		/// <remarks></remarks>
		new public static readonly BindableProperty PaddingProperty = BindableProperty.Create ("Padding", typeof(Thickness), typeof(Button), default(Thickness));
		/// <summary>
		/// Gets or sets the content (image and text) padding for the button.
		/// </summary>
		/// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
		new public Thickness Padding {
			get { return (Thickness)GetValue (PaddingProperty); }
			set { SetValue (PaddingProperty, value); }
		}
		*/


		/// <summary>
		/// Backing store for the Command bindable property.
		/// </summary>
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof (ICommand), typeof (ImageButton), (object) null, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate) null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((ImageButton)bo).OnCommandChanged ()), 
			(BindableProperty.BindingPropertyChangingDelegate) null, 
			(BindableProperty.CoerceValueDelegate) null, 
			(BindableProperty.CreateDefaultValueDelegate) null);

		/// <summary>
		/// Backing store for the CommandParameter bindable property.
		/// </summary>
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof (object), typeof (ImageButton), (object) null, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate) null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((ImageButton)bo).CommandCanExecuteChanged (bo, EventArgs.Empty)), 
			(BindableProperty.BindingPropertyChangingDelegate) null, (BindableProperty.CoerceValueDelegate) null, (BindableProperty.CreateDefaultValueDelegate) null);

		/// <summary>
		/// Backing store for the IsSelected bindable property.
		/// </summary>
		public static BindableProperty IsSelectedProperty = BindableProperty.Create ("IsSelected", typeof(bool), typeof(ImageButton), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Button"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool IsSelected {
			get { return (bool)GetValue (IsSelectedProperty); }
			set { SetValue (IsSelectedProperty, value); }
		}

		/// <summary>
		/// Backing store for the DefaultState bindable property.
		/// </summary>
		public static BindableProperty DefaultStateProperty = BindableProperty.Create ("DefaultState", typeof(ImageButtonState), typeof(ImageButton), null);
		/// <summary>
		/// Gets or sets the ImageButton's properties for the default button state.
		/// </summary>
		/// <value>The ImageButtonState structure for the default button state.</value>
		public ImageButtonState DefaultState {
			get { return (ImageButtonState)GetValue (DefaultStateProperty);}
			set { SetValue (DefaultStateProperty, value); }
		}

		/// <summary>
		/// Backing store for the PressingState bindable property.
		/// </summary>
		public static BindableProperty PressingStateProperty = BindableProperty.Create ("PressingState", typeof(ImageButtonState), typeof(Button), null);
		/// <summary>
		/// Gets or sets the ImageButton's properties for the pressing button state.
		/// </summary>
		/// <value>The ImageButtonState structure for the pressing button state.</value>
		public ImageButtonState PressingState {
			get { return (ImageButtonState)GetValue (PressingStateProperty);}
			set { SetValue (PressingStateProperty, value); }
		}

		/// <summary>
		/// Backing store for the SelectedState bindable property.
		/// </summary>
		public static BindableProperty SelectedStateProperty = BindableProperty.Create ("SelectedState", typeof(ImageButtonState), typeof(ImageButton), null);
		/// <summary>
		/// Gets or sets the ImageButton's properties for the selected button state.
		/// </summary>
		/// <value>The ImageButtonState structure for the selected button state.</value>
		public ImageButtonState SelectedState {
			get { return (ImageButtonState)GetValue (SelectedStateProperty);}
			set { SetValue (SelectedStateProperty, value); }
		}

		/// <summary>
		/// Backing store for the DisabledState bindable property.
		/// </summary>
		public static BindableProperty DisabledStateProperty = BindableProperty.Create ("DisabledState", typeof(ImageButtonState), typeof(ImageButton), null);
		/// <summary>
		/// Gets or sets the ImageButton's properties for the disabled button state.
		/// </summary>
		/// <value>The ImageButtonState structure for the disabled button state.</value>
		public ImageButtonState DisabledState {
			get { return (ImageButtonState)GetValue (DisabledStateProperty);}
			set { SetValue (DisabledStateProperty, value); }
		}

		/// <summary>
		/// Backing store for the DisabledAndSelectedState bindable property.
		/// </summary>
		public static BindableProperty DisabledAndSelectedStateProperty = BindableProperty.Create ("DisabledAndSelectedState", typeof(ImageButtonState), typeof(ImageButton), null);
		/// <summary>
		/// Gets or sets the ImageButton's properties for the disabled and selected button state.
		/// </summary>
		/// <value>The ImageButtonState structure for the disabled and selected button state.</value>
		public ImageButtonState DisabledAndSelectedState {
			get { return (ImageButtonState)GetValue (DisabledAndSelectedStateProperty);}
			set { SetValue (DisabledAndSelectedStateProperty, value); }
		}


		/// <summary>
		/// OBSOLETE: Use ToggleBehaviorProperty instead.
		/// </summary>
		[Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead")]
		public static BindableProperty StickyBehaviorProperty = null;

		/// <summary>
		/// OBSOLETE: Use ToggleBehavior instead.
		/// </summary>
		[Obsolete("StickyBehavior property is obsolete, use ToggleBehavior instead")]
		public bool StickyBehavior
		{
			get { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead");  }
			set { throw new NotSupportedException("StickyBehavior property is obsolete, use ToggleBehavior instead");  }
		}


		/// <summary>
		/// Backing store for the ToggleBehavior bindable property.
		/// </summary>
		public static BindableProperty ToggleBehaviorProperty = BindableProperty.Create ("ToggleBehavior", typeof(bool), typeof(ImageButton), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Button"/> will stay selected or unselected after a tap.
		/// </summary>
		/// <value><c>true</c> if Toggle; otherwise, <c>false</c>.</value>
		public bool ToggleBehavior {
			get { return (bool)GetValue (ToggleBehaviorProperty); }
			set { SetValue (ToggleBehaviorProperty, value); }
		}

		/// <summary>
		/// The alignment of the image and text.
		/// </summary>
		public static BindableProperty AlignmentProperty = BindableProperty.Create ("Alignment", typeof(TextAlignment), typeof(ImageButton), TextAlignment.Center);
		/// <summary>
		/// Gets or sets the alignment of the image and text.
		/// </summary>
		/// <value>The alignment (left, center, right).</value>
		public TextAlignment Alignment {
			get { return (TextAlignment)GetValue (AlignmentProperty); }
			set { SetValue (AlignmentProperty, value); }
		}

		/// <summary>
		/// Backing store for the lines property.
		/// </summary>
		public static readonly BindableProperty LinesProperty = BindableProperty.Create("Lines", typeof(int), typeof(ImageButton), 1);
		/// <summary>
		/// Gets or sets the lines of text in ImageButton.
		/// </summary>
		/// <value>The lines (default 1)</value>
		public int Lines
		{
			get { return (int)GetValue(LinesProperty); }
			set { SetValue(LinesProperty, value); }
		}

		/// <summary>
		/// Backing store for the fit property.
		/// </summary>
		public static readonly BindableProperty FitProperty = BindableProperty.Create("Fit", typeof(LabelFit), typeof(ImageButton), LabelFit.None);
		/// <summary>
		/// Gets or sets the fit of the label.
		/// </summary>
		/// <value>None, Width or Lines</value>
		public LabelFit Fit
		{
			get { return (LabelFit)GetValue(FitProperty); }
			set { SetValue(FitProperty, value); }
		}

		/// <summary>
		/// Backing store for the line break mode property.
		/// </summary>
		public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create("BreakMode", typeof(LineBreakMode), typeof(ImageButton), LineBreakMode.TailTruncation);
		/// <summary>
		/// Gets or sets the line break mode.
		/// </summary>
		/// <value>The break mode (default=TailTruncation)</value>
		public LineBreakMode LineBreakMode
		{
			get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
			set { SetValue(LineBreakModeProperty, value); }
		}

		/// <summary>
		/// Backing store for the trailing image property.
		/// </summary>
		public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create("TrailingImage", typeof(bool), typeof(ImageButton), default(bool));
		/// <summary>
		/// Gets or sets if the image is to be rendered after the text.
		/// </summary>
		/// <value>default=false</value>
		public bool TrailingImage
		{
			get { return (bool)GetValue(TrailingImageProperty); }
			set { SetValue(TrailingImageProperty, value); }
		}



		#endregion


		#region Fields
		bool _noUpdate = true;
		ImageButtonState _currentState;
		Xamarin.Forms.StackLayout _stackLayout;
		Xamarin.Forms.Image _image;
		Label _label;
		FormsGestures.Listener _gestureListener;
		#endregion


		#region Constructor
		bool _constructing;

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.ImageButton"/> class.
		/// </summary>
		public ImageButton ()
		{
			_constructing = true;
			Padding = new Thickness (5);
			_label = new Label {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				//VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				LineBreakMode = LineBreakMode.TailTruncation,
				Lines = 1,
				Fit = LabelFit.None,
				//BackgroundColor = Color.Pink
			};
			DefaultState = new ImageButtonState () ;
			_stackLayout = new Xamarin.Forms.StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = Alignment.ToLayoutOptions (),
				//BackgroundColor = Color.Red,
			};
			_stackLayout.Children.Add (_label);
			Content = _stackLayout;
			_noUpdate = false;
			ShowState (DefaultState);


			_gestureListener = new FormsGestures.Listener (this);


			_gestureListener.Up += OnUp;
			_gestureListener.Down += OnDown;

			_gestureListener.LongPressed += OnLongPressed;
			_gestureListener.LongPressing += OnLongPressing;
			_constructing = false;
		}


		#endregion


		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">Disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_gestureListener.Up -= OnUp;
					_gestureListener.Down -= OnDown;
					_gestureListener.LongPressed -= OnLongPressed;
					_gestureListener.LongPressing -= OnLongPressing;
					_gestureListener.Dispose();
					_gestureListener = null;
					_image = null;
					_label = null;
					_stackLayout = null;
					_currentState.PropertyChanged -= OnStatePropertyChanged;
					_currentState = null;
					if (Command != null)
						Command.CanExecuteChanged -= CommandCanExecuteChanged;
					disposedValue = true;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

			}
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
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion


		#region Gesture Event Responders
		public void Tap()
		{
			OnUp(this,new FormsGestures.DownUpEventArgs(null,null));
		}

		void OnUp(object sender, FormsGestures.DownUpEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Up");
			if (IsEnabled)
			{
				Debug.WriteLine("tapped");
				if (ToggleBehavior)
				{
					IsSelected = !IsSelected;
					UpdateState();
					if (IsSelected)
						Selected?.Invoke(this, EventArgs.Empty);
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
				else {
					if (PressingState != null && PressingState.Image!=null)
						Device.StartTimer(TimeSpan.FromMilliseconds(20), () =>
						{
							UpdateState();
							return false;
						});
					else {
						Opacity = 0.5;
						Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
						{
							Opacity += 0.1;
							return Opacity < 1.0;
						});
					}
				}
				SendClicked();
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
				LongPressing?.Invoke(this, EventArgs.Empty);
		}

		void OnLongPressed(object sender, FormsGestures.LongPressEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("LongPressed");
			if (IsEnabled)
			{
				LongPressed?.Invoke(this, EventArgs.Empty);
				UpdateState();
			}
		}
		#endregion


		#region State Change responders
		/// <summary>
		/// Redraws the button to the current state: Default, Selected, Disabled or DisabledAndSelected.
		/// </summary>
		public void UpdateState() {
			if (IsEnabled) {
				if (IsSelected) {
					ShowState (SelectedState ?? DefaultState);
					if (SelectedState == null) {
						_label.FontAttributes = FontAttributes.Bold;
					}
				} else 
					ShowState (DefaultState);
			} else {
				if (IsSelected) {
					ShowState (DisabledAndSelectedState ?? SelectedState ?? DisabledState ?? DefaultState);
					if (DisabledAndSelectedState == null) {
						if (SelectedState != null) {
							Opacity = 0.75;
						} else if (DisabledState != null) {
							_label.FontAttributes = FontAttributes.Bold;
						} else {
							Opacity = 0.75;
							_label.FontAttributes = FontAttributes.Bold;
						}
					}
				} else {
					ShowState (DisabledState ?? DefaultState);
					if (DisabledState == null)
						Opacity = 0.75;
				}
			}
		}

		/// <summary>
		/// Redraws the button to a custom ImageButtonState
		/// </summary>
		/// <param name="newState">Custom ImageButtonState.</param>
		public void ShowState(ImageButtonState newState) {
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
			else if (!_constructing && Device.OnPlatform(false, true, false)) 
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
			if (newImage == null) {
				// label only
				if (_image != null) {
					var toDispose = _image;
					if (toDispose != null)
						Device.StartTimer (TimeSpan.FromMilliseconds (10), () => {
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
			} else {
				// there is an image
				newImage.Opacity = 1.0;
				if (_image != newImage) {
					// if it is a new image
					if (_image != null)
						_stackLayout.Children.Remove (_image);
					_image = newImage;
					if (TrailingImage)
						_stackLayout.Children.Add(_image);
					else
						_stackLayout.Children.Insert (0, _image);
				}
				if (string.IsNullOrEmpty (text)) {
					// no Label, just an image
					if (_stackLayout.Children.Contains(_label))
						_stackLayout.Children.Remove (_label);
				} else {
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
			SetLabelState (_label, _currentState);
			_currentState.PropertyChanged += OnStatePropertyChanged;
			//InvalidateMeasure ();
		
		}

		void SetLabelState(Label label, ImageButtonState state) {
			label.TextColor = (state.FontColorSet ? state.FontColor : (DefaultState.FontColorSet ? DefaultState.FontColor : Device.OnPlatform(Color.Blue, Color.White, Color.White)));
			label.FontSize = (state.FontSizeSet ? state.FontSize : (DefaultState.FontSizeSet ? DefaultState.FontSize : Device.GetNamedSize (NamedSize.Medium, _label)));
			label.FontFamily = (state.FontFamilySet ? state.FontFamily : DefaultState.FontFamily);
			label.FontAttributes = (state.FontAttributesSet ? state.FontAttributes : DefaultState.FontAttributes);
		}
		#endregion


		#region CurrentProperties
		/// <summary>
		/// Gets or sets the command to invoke when the button is activated. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// A command to invoke when the button is activated. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks>
		/// This property is used to associate a command with an instance of a button. This property is most often set in the MVVM pattern to bind callbacks back into the ViewModel. <see cref="P:Xamarin.Forms.VisualElement.IsEnabled"/> is controlled by the Command if set.
		/// </remarks>
		public ICommand Command
		{
			get { return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the parameter to pass to the Command property. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// A object to pass to the command property. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks/>
		public object CommandParameter
		{
			get { return GetValue (CommandParameterProperty); }
			set { SetValue (CommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets the current Text displayed as the content of the button. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// The text currently displayed in the button. The default value is <see langword="null"/>.
		/// </value>
		public string Text
		{
			get { return _label.Text; }
		}

		/// <summary>
		/// Gets the formatted text.
		/// </summary>
		/// <value>The formatted text.</value>
		public string FormattedText 
		{
			get { return _label.HtmlText; }
		}

		/// <summary>
		/// Gets the <see cref="T:Xamarin.Forms.Color"/> for the text of the button. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// The <see cref="T:Xamarin.Forms.Color"/> value.
		/// </value>
		public Color TextColor
		{
			get { return _label.TextColor; }
		}

		/// <summary>
		/// Gets the font family to which the font for the button text belongs.
		/// </summary>
		/// 
		/// <value>
		/// To be added.
		/// </value>
		/// 
		/// <remarks>
		/// To be added.
		/// </remarks>
		public string FontFamily
		{
			get { return _label.FontFamily; }
		}

		/// <summary>
		/// Gets or sets the size of the font of the button text.
		/// </summary>
		/// 
		/// <value>
		/// To be added.
		/// </value>
		/// 
		/// <remarks>
		/// To be added.
		/// </remarks>
		[Xamarin.Forms.TypeConverter(typeof (FontSizeConverter))]
		public double FontSize
		{
			get { return _label.FontSize; }
		}

		/// <summary>
		/// Gets a value that indicates whether the font for the button text is bold, italic, or neither.
		/// </summary>
		/// 
		/// <value>
		/// To be added.
		/// </value>
		/// 
		/// <remarks>
		/// To be added.
		/// </remarks>
		public FontAttributes FontAttributes
		{
			get { return _label.FontAttributes; }
		}

		/// <summary>
		/// Gets or sets the optional image source to display next to the text in the Button. This is a bindable property.
		/// </summary>
		/// 
		/// <value>
		/// To be added.
		/// </value>
		/// 
		/// <remarks>
		/// To be added.
		/// </remarks>
		public Xamarin.Forms.Image Image
		{
			get { return _image; }
		}


		bool IsEnabledCore
		{
			set
			{
				//this.SetValueCore(VisualElement.IsEnabledProperty, (object) (bool) (value ? true : false), Xamarin.Forms.BindableObject.SetValueFlags.None);
				SetValue(VisualElement.IsEnabledProperty, (object) value );
			}
		}


		/// <summary>
		/// Occurs when the Button is clicked.
		/// </summary>
		/// 
		/// <remarks>
		/// The user may be able to raise the clicked event using accessibility or keyboard controls when the Button has focus.
		/// </remarks>
		public event EventHandler Tapped;

		/// <summary>
		/// Occurs when the Button is long pressed.
		/// </summary>
		public event EventHandler LongPressed;

		/// <summary>
		/// Occurs when the Button is being pressed long enough to be considered a long pressing.
		/// </summary>
		public event EventHandler LongPressing;

		/// <summary>
		/// Occurs when button transitions to the selected state.
		/// </summary>
		public event EventHandler Selected;
		#endregion


		#region Change Handlers

		void OnStatePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Debug.WriteLine ("OnStatePropertyChanged");
			if (sender == _currentState)
			{
				System.Diagnostics.Debug.WriteLine("\t"+e.PropertyName);
				UpdateState();
				//if (e.PropertyName == ImageButtonState.TextProperty.PropertyName || e.PropertyName == ImageButtonState.HtmlTextProperty.PropertyName)
					//_stackLayout.
					//System.Diagnostics.Debug.WriteLine("");
					//_stackLayout.UpdateLayout();
			}
		}


		/// <param name="propertyName">The name of the changed property.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		/// 
		/// <remarks>
		/// A Button triggers this by itself. An inheritor only need to call this for properties without BindableProperty as backend store.
		/// </remarks>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			if (_noUpdate)
				return;
			if (propertyName == ImageButton.CommandProperty.PropertyName) {
				ICommand command = Command;
				if (command != null) {
					command.CanExecuteChanged -= CommandCanExecuteChanged;
				//} else if (propertyName == Button.PressingStateProperty.PropertyName && PressingState != null) {
				//	PressingState.PropertyChanged -= OnStatePropertyChanged;
				}
			}
			base.OnPropertyChanging(propertyName);
		}


		/// <param name="propertyName">The name of the changed property.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		/// 
		/// <remarks>
		/// A Button triggers this by itself. An inheritor only need to call this for properties without BindableProperty as backend store.
		/// </remarks>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			if (_noUpdate)
				return;

			if (propertyName == ImageButton.DefaultStateProperty.PropertyName && IsEnabled && !IsSelected)
				UpdateState();
			//			} else if (propertyName == Button.PressingStateProperty.PropertyName && PressingState != null) {
			//				PressingState.PropertyChanged += OnStatePropertyChanged;
			//				SetupPressingState ();
			else if (propertyName == ImageButton.IsSelectedProperty.PropertyName && IsEnabled && IsSelected)
				UpdateState();
			else if (propertyName == ImageButton.DisabledStateProperty.PropertyName && !IsEnabled && !IsSelected)
				UpdateState();
			else if (propertyName == ImageButton.DisabledStateProperty.PropertyName && !IsEnabled && IsSelected)
				UpdateState();
			//} else if (propertyName == Button.PaddingProperty.PropertyName) {
			//	_stackLayout.Padding = Padding;
			else if (propertyName == ImageButton.AlignmentProperty.PropertyName)
				_stackLayout.HorizontalOptions = Alignment.ToLayoutOptions();
			else if (propertyName == IsEnabledProperty.PropertyName || propertyName == IsSelectedProperty.PropertyName)
				UpdateState();
			else if (propertyName == ImageButton.LinesProperty.PropertyName)
				_label.Lines = Lines;
			else if (propertyName == ImageButton.FitProperty.PropertyName)
				_label.Fit = Fit;
			else if (propertyName == ImageButton.LineBreakModeProperty.PropertyName)
				_label.LineBreakMode = LineBreakMode;
				
			base.OnPropertyChanged(propertyName);

		}

		void OnCommandChanged()
		{
			if (Command != null)
			{
				Command.CanExecuteChanged += CommandCanExecuteChanged;
				CommandCanExecuteChanged(this, EventArgs.Empty);
			}
			else
				IsEnabledCore = true;
		}

		void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
		{
			ICommand command = Command;
			if (command == null)
				return;
			IsEnabledCore = command.CanExecute(CommandParameter);
		}

		void SendClicked()
		{
			ICommand command = Command;
			if (command != null)
				command.Execute(CommandParameter);
			// ISSUE: reference to a compiler-generated field
			EventHandler eventHandler = Tapped;
			if (eventHandler != null)
				eventHandler(this, EventArgs.Empty);
			//Debug.WriteLine ("Clicked");

		}
		#endregion
	}
}

