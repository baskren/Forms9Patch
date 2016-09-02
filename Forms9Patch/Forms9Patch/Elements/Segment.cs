using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.Generic;

namespace Forms9Patch
{
	/// <summary>
	/// Model for Segment.
	/// </summary>
	[ContentProperty("HtmlText")]
	public class Segment : BindableObject
	{
		#region Properties
		/// <summary>
		/// Backing store for the Image bindable property.
		/// </summary>
		public static BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(Xamarin.Forms.ImageSource), typeof(Segment), null);
		/// <summary>
		/// Gets or sets the companion image for this this <see cref="Segment"/>.
		/// </summary>
		/// <value>The image.</value>
		public Xamarin.Forms.ImageSource ImageSource {
			get { return (Xamarin.Forms.ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		/// <summary>
		/// Backing store for the Text bindable property.
		/// </summary>
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Segment), null);
		/// <summary>
		/// Gets or sets the text for this <see cref="Segment"/>.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Backing store for the formatted text property.
		/// </summary>
		public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create("HtmlText", typeof(string), typeof(Segment), null);
		/// <summary>
		/// Gets or sets the formatted text.
		/// </summary>
		/// <value>The formatted text.</value>
		public string HtmlText
		{
			get { return (string)GetValue(HtmlTextProperty); }
			set { SetValue(HtmlTextProperty, value); }
		}



		/// <summary>
		/// Backing store for the Segment.FontColor bindable property.
		/// </summary>
		public static readonly BindableProperty FontColorProperty = BindableProperty.Create("FontColor", typeof(Color), typeof(Segment), Color.Default);
		/// <summary>
		/// Gets or sets the color of the font.
		/// </summary>
		/// <value>The color of the font.</value>
		public Color FontColor
		{
			get { return (Color)GetValue(FontColorProperty); }
			set { SetValue(FontColorProperty, value); }
		}

		internal bool FontAttributesSet;
		/// <summary>
		/// Backing store for the Segment.FontAttributes bindable property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(Segment), FontAttributes.None);
		  /// <summary>
		  /// Gets or sets the font attributes.
		  /// </summary>
		  /// <value>The font attributes.</value>
		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set {
				FontAttributesSet = true;
				SetValue(FontAttributesProperty, value); 
			}
		}



		/// <summary>
		/// The backing store for the is enabled property.
		/// </summary>
		public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create("IsEnabled", typeof(bool), typeof(Segment), true);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Segment"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled {
			get { return (bool)GetValue(IsEnabledProperty); }
			set { 
				SetValue(IsEnabledProperty, value); 
			}
		}

		/// <summary>
		/// The backing store for the is selected property.
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(Segment), false, BindingMode.TwoWay);
		/// <summary>
		/// 
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool IsSelected {
			get { return (bool)GetValue(IsSelectedProperty); }
			set { 
				SetValue(IsSelectedProperty, value); 
			}
		}

		/// <summary>
		/// The backing store for the orientation property.
		/// </summary>
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(Segment), StackOrientation.Horizontal);
		/// <summary>
		/// Gets or sets the image/lable orientation.
		/// </summary>
		/// <value>The iamge/label orientation.</value>
		public StackOrientation Orientation {
			get { return (StackOrientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty,value); }
		}
		 
		/// <summary>
		/// Occurs when Segment is tapped.
		/// </summary>
		public event EventHandler Tapped {
			add { MaterialButton.Tapped += value; }
			remove { MaterialButton.Tapped -= value; }
		}

		/// <summary>
		/// Occurs when Segment is selected.
		/// </summary>
		public event EventHandler Selected {
			add { MaterialButton.Selected += value; }
			remove { MaterialButton.Selected -= value; }
		}

		/// <summary>
		/// Occurs when long pressing.
		/// </summary>
		public event EventHandler LongPressing {
			add { MaterialButton.LongPressing += value; }
			remove { MaterialButton.LongPressing -= value; }
		}

		/// <summary>
		/// Occurs when long pressed.
		/// </summary>
		public event EventHandler LongPressed {
			add { MaterialButton.LongPressed += value; }
			remove { MaterialButton.LongPressed -= value; }
		}

		/// <summary>
		/// Backing store for the Segment.Command bindable property.
		/// </summary>
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof (ICommand), typeof (Segment), null, BindingMode.OneWay, null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((Segment)bo).OnCommandChanged ()));
		/// <summary>
		/// Gets or sets the command to invoke when the segment is selected.
		/// </summary>
		/// 
		/// <value>
		/// A command to invoke when the segment is selected. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks>
		/// This property is used to associate a command with an instance of a segment. This property is most often set in the MVVM pattern to bind callbacks back into the ViewModel. <see cref="P:Xamarin.Forms.VisualElement.IsEnabled"/> is controlled by the Command if set.
		/// </remarks>
		public ICommand Command
		{
			//get { return (ICommand)GetValue (CommandProperty); }
			//set { SetValue (CommandProperty, value); }
			get { return MaterialButton.Command; }
			set { MaterialButton.Command = value; }
		}

		/// <summary>
		/// Backing store for the Segment.CommandParameter bindable property.
		/// </summary>
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof (object), typeof (Segment), null, BindingMode.OneWay, null, 
			new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
				((Segment)bo).CommandCanExecuteChanged (bo, EventArgs.Empty)));
		/// <summary>
		/// Gets or sets the parameter to pass to the Command property. 
		/// </summary>
		/// 
		/// <value>
		/// A object to pass to the command property. The default value is <see langword="null"/>.
		/// </value>
		/// 
		/// <remarks/>
		public object CommandParameter
		{
			get { return MaterialButton.CommandParameter; }
			set { MaterialButton.CommandParameter = value; }
		}

		
		internal SegmentControlStickyBehavior StickyBehavior;

		#endregion

		#region Popup Support 
		/// <summary>
		/// Gets the visual element used to render the Segment.
		/// </summary>
		/// <value>The visual element.</value>
		public VisualElement VisualElement {
			get { return MaterialButton; }
		}
		#endregion

		internal readonly MaterialButton MaterialButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="Segment"/> class.
		/// </summary>
		public Segment ()
		{
			MaterialButton = new MaterialButton ();
			MaterialButton.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
			{
				switch (e.PropertyName)
				{
					case "IsEnabled":
						IsEnabled = MaterialButton.IsEnabled;
						break;
					case "ImageSource":
						ImageSource = MaterialButton.ImageSource;
						break;
					case "Text":
						Text = MaterialButton.Text;
						break;
					case "HtmlText":
						HtmlText = MaterialButton.HtmlText;
						break;
					case "FontColor":
						FontColor = MaterialButton.FontColor;
						break;
					case "FontAttributes":
						FontAttributes = MaterialButton.FontAttributes;
						break;
					case "IsSelected":
						IsSelected = MaterialButton.IsSelected;
						break;
					case "Orienation":
						Orientation = MaterialButton.Orientation;
						break;
				}
			};
		}

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <returns>The property changed.</returns>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			switch (propertyName)
			{
				case "IsEnabled":
					MaterialButton.IsEnabled = IsEnabled;
					break;
				case "ImageSource":
					MaterialButton.ImageSource = ImageSource;
					break;
				case "Text":
					MaterialButton.Text = Text;
					break;
				case "HtmlText":
					MaterialButton.HtmlText = HtmlText;
					break;
				case "FontColor":
					MaterialButton.FontColor = FontColor;
					break;
				case "FontAttributes":
					MaterialButton.FontAttributes = FontAttributes;
					break;
				case "IsSelected":
					MaterialButton.IsSelected = IsSelected;
					break;
				case "Orienation":
					MaterialButton.Orientation = Orientation;
					break;
			}
				
			
		}

		/// <summary>
		/// Ons the property changing.
		/// </summary>
		/// <returns>The property changing.</returns>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
		}

		void OnCommandChanged()
		{
			if (Command != null)
			{
				Command.CanExecuteChanged += CommandCanExecuteChanged;
				CommandCanExecuteChanged(this, EventArgs.Empty);
			}
			//else
			//	IsEnabledCore = true;
		}

		void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
		{
			ICommand command = Command;
			if (command == null)
				return;
			//IsEnabledCore = command.CanExecute(CommandParameter);
		}




	}
}

