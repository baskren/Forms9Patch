using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
	/// <summary>
	/// Model for Segment.
	/// </summary>
	[ContentProperty("HtmlText")]
	public class Segment : BindableObject, ISegment
	{
        #region ISegment properties

        #region IconImage property
        /// <summary>
        /// Backing store for the Image bindable property.
        /// </summary>
        [Obsolete("Use IconImageProperty instead")]
        public static BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(Xamarin.Forms.ImageSource), typeof(Segment), null);
        /// <summary>
        /// Gets or sets the companion image for this this <see cref="Segment"/> - alternative to IconText.
        /// </summary>
        /// <value>The image.</value>
        [Obsolete("Use IconImage instead")]
        public Xamarin.Forms.ImageSource ImageSource
        {
            get { throw new NotSupportedException("Use IconImage instead"); }
            set { throw new NotSupportedException("Use IconImage instead"); }
        }


        /// <summary>
        /// Bindable Property for the IconImage property
        /// </summary>
        public static BindableProperty IconImageProperty = BindableProperty.Create("IconImage", typeof(Forms9Patch.Image), typeof(Segment), null);
        /// <summary>
        /// Gets or sets the icon image for this <see cref="Segment"/>.  Alternative to IconText prop
        /// </summary>
        public Forms9Patch.Image IconImage
        {
            get { return (Forms9Patch.Image)GetValue(IconImageProperty); }
            set
            {
                SetValue(IconImageProperty, value);
            }
        }
        #endregion IconImage property

        #region IconText property
        /// <summary>
        /// The icon text property backing store.
        /// </summary>
        public static readonly BindableProperty IconTextProperty = BindableProperty.Create("IconText", typeof(string), typeof(Segment), default(string));
        /// <summary>
        /// Gets or sets the icon text - alternative to ImageSource.
        /// </summary>
        /// <value>The icon text.</value>
        public string IconText
        {
            get { return (string)GetValue(IconTextProperty); }
            set { SetValue(IconTextProperty, value); }
        }
        #endregion IconText property

        #region Text property
        /// <summary>
        /// Backing store for the Text bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Segment), null);
        /// <summary>
        /// Gets or sets the text for this <see cref="Segment"/>.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region HtmlText property
        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create("HtmlText", typeof(string), typeof(Segment), null, 
            propertyChanged: (bindable, oldValue, newValue) => 
            {
                //System.Diagnostics.Debug.WriteLine("");
            });
        /// <summary>
        /// Gets or sets the formatted text.
        /// </summary>
        /// <value>The formatted text.</value>
        public string HtmlText
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set
            {
                SetValue(HtmlTextProperty, value);
            }
        }
        #endregion HtmlText property

        #region TextColor property
        /// <summary>
        /// OBSOLETE: Use TextColorProperty
        /// </summary>
        [Obsolete("Use TextColorProperty")]
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create("FontColor", typeof(Color), typeof(Segment), Color.Default);
        /// <summary>
        /// OBSOLETE: Use TextColor
        /// </summary>
        /// <value>The color of the font.</value>
        [Obsolete("Use TextColor")]
        public Color FontColor
        {
            get { throw new NotSupportedException("Use TextColor"); }
            set { throw new NotSupportedException("Use TextColor"); }
        }

        /// <summary>
        /// Backing store for the Segment.TextColor bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(Segment), Color.Default);
        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion

        #region FontAttributes property
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
            set
            {
                FontAttributesSet = true;
                SetValue(FontAttributesProperty, value);
            }
        }
        #endregion FontAttributes property

        #region IsEnabled property
        /// <summary>
        /// The backing store for the is enabled property.
        /// </summary>
        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create("IsEnabled", typeof(bool), typeof(Segment), true);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Segment"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }
        #endregion IsEnabled property

        #region IsSelected property
        /// <summary>
        /// The backing store for the is selected property.
        /// </summary>
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(Segment), false, BindingMode.TwoWay);
        /// <summary>
        /// 
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }
        #endregion IsSelected property

        #region Orientation property
        /// <summary>
        /// The backing store for the orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(StackOrientation), typeof(Segment), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the image/lable orientation.
        /// </summary>
        /// <value>The iamge/label orientation.</value>
        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion Orientation property

        #region ICommand property
        /// <summary>
        /// Backing store for the Segment.Command bindable property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(Segment), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((Segment)bo).OnCommandChanged()));
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
        #endregion ICommand property

        #region ICommandParameter property
        /// <summary>
        /// Backing store for the Segment.CommandParameter bindable property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Segment), null, BindingMode.OneWay, null,
            new BindableProperty.BindingPropertyChangedDelegate((bo, o, n) =>
                ((Segment)bo).CommandCanExecuteChanged(bo, EventArgs.Empty)));
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
        #endregion ICommandParameter property

        #region VisualElement property
        /// <summary>
        /// Gets the visual element used to render the Segment (to support Bubble Popup).
        /// </summary>
        /// <value>The visual element.</value>
        public VisualElement VisualElement
        {
            get { return MaterialButton; }
        }
        #endregion

        #endregion ISegment properties


        #region Event Handlers

        #region Tapped event handler
        /// <summary>
        /// Occurs when Segment is tapped.
        /// </summary>
        public event EventHandler Tapped
        {
            add { MaterialButton.Tapped += value; }
            remove { MaterialButton.Tapped -= value; }
        }
        #endregion Tapped eventhandler

        #region Selected event handler
        /// <summary>
        /// Occurs when Segment is selected.
        /// </summary>
        public event EventHandler Selected
        {
            add { MaterialButton.Selected += value; }
            remove { MaterialButton.Selected -= value; }
        }
        #endregion Selected event handler

        #region LongPressing event handlre
        /// <summary>
        /// Occurs when long pressing.
        /// </summary>
        public event EventHandler LongPressing
        {
            add { MaterialButton.LongPressing += value; }
            remove { MaterialButton.LongPressing -= value; }
        }
        #endregion LongPressing event handlre

        #region LongPressed event handler
        /// <summary>
        /// Occurs when long pressed.
        /// </summary>
        public event EventHandler LongPressed
        {
            add { MaterialButton.LongPressed += value; }
            remove { MaterialButton.LongPressed -= value; }
        }
        #endregion LongPressed event handler

        #endregion Event Handlers


        #region Fields
        internal GroupToggleBehavior ToggleBehavior;
        internal readonly MaterialButton MaterialButton;
        #endregion Fields





        #region Constuctors
        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        public Segment()
        {
            MaterialButton = new MaterialButton();
            MaterialButton.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                switch (e.PropertyName)
                {
                    case "IsEnabled":
                        IsEnabled = MaterialButton.IsEnabled;
                        break;
                    case "ImageSource":
                        IconImage = MaterialButton.IconImage;
                        break;
                    case "IconText":
                        IconText = MaterialButton.IconText;
                        break;
                    case "Text":
                        Text = MaterialButton.Text;
                        break;
                    case "HtmlText":
                        HtmlText = MaterialButton.HtmlText;
                        break;
                    case "FontColor":
                        TextColor = MaterialButton.TextColor;
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
        /// Instantiates an new Segment and sets its Text and imageSource properties
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public Segment(string text, Forms9Patch.Image image) : this()
        {
            Text = text;
            if (image!=null)
                IconImage = image;
        }

        /// <summary>
        /// Instantiates an new Segment and sets its Text property and sets the IconImage to a new image, created using the provided ImageSource
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imageSource"></param>
        public Segment(string text, Xamarin.Forms.ImageSource imageSource=null) : this()
        {
            Text = text;
            if (imageSource!=null)
                IconImage = new Forms9Patch.Image(imageSource);
        }

        /// <summary>
        /// Instantiates a new Segment and sets its Text and either its IconText or its IconImage, created using the provided embedded resource id 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        public Segment(string text, string icon, Assembly assembly=null) : this()
        {
            Text = text;
            bool isIconText=false;

            assembly = assembly ?? (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            var match = Forms9Patch.ImageSource.BestEmbeddedMultiResourceMatch(icon, assembly);

            //if (icon.Contains("<") && icon.Contains("/>"))
            if (match==null)
            {
                int opens=0, closes=0;
                for(int i=0;i<icon.Length;i++)
                {
                    if (icon[i] == '<')
                        opens++;
                    else if (icon[i]=='/' && (i+2)<icon.Length && icon[i+1]=='>')
                    {
                        closes++;
                        i++;
                    }
                }
                isIconText = opens > 0 && opens == closes;
            }
            if (isIconText)
                IconText = icon;
            else
            {
                IconImage = new Forms9Patch.Image(icon, assembly);
            }
        }
        #endregion Constuctor


        #region PropertyChanged responder
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
                case "IconImage":
                    MaterialButton.IconImage = IconImage;
                    break;
                case "IconText":
                    MaterialButton.IconText = IconText;
                    break;
                case "Text":
                    MaterialButton.Text = Text;
                    break;
                case "HtmlText":
                    MaterialButton.HtmlText = HtmlText;
                    break;
                case "TextColor":
                    MaterialButton.TextColor = TextColor;
                    break;
                case "FontAttributes":
                    MaterialButton.FontAttributes = FontAttributes;
                    break;
                case "IsEnabled":
					MaterialButton.IsEnabled = IsEnabled;
					break;
				case "IsSelected":
					MaterialButton.IsSelected = IsSelected;
					break;
				case "Orienation":
					MaterialButton.Orientation = Orientation;
					break;
			}
		}
        #endregion


        #region Programmical Gestures
        /// <summary>
        /// Tap this instance.
        /// </summary>
        public void Tap()
		{
			MaterialButton.Tap();
		}

        /// <summary>
        /// Programmically click the segement
        /// </summary>
        public void SendClicked()
        {
            MaterialButton.SendClicked();
        }
        #endregion


        #region Property Change management
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
        #endregion Property Change management




    }
}

