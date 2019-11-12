using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Model for Segment.
    /// </summary>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(HtmlText))]
    public class Segment : Element, ISegment, IDisposable
    {
        #region Obsolete Properties
        /// <summary>
        /// OBSOLETE: Use TextColorProperty
        /// </summary>
        [Obsolete("Use TextColorProperty")]
        public static readonly BindableProperty FontColorProperty = BindableProperty.Create(nameof(FontColor), typeof(Color), typeof(Segment), Color.Default);
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
        /// Backing store for the Image bindable property.
        /// </summary>
        [Obsolete("Use IconImageProperty instead")]
        public static BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(Xamarin.Forms.ImageSource), typeof(Segment), null);
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

        #endregion


        #region ISegment properties

        #region IconImage property
        /// <summary>
        /// Bindable Property for the IconImage property
        /// </summary>
        public static BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(Forms9Patch.Image), typeof(Segment), null);
        /// <summary>
        /// Gets or sets the icon image for this <see cref="Segment"/>.  Alternative to IconText prop
        /// </summary>
        public Forms9Patch.Image IconImage
        {
            get => (Forms9Patch.Image)GetValue(IconImageProperty);
            set => SetValue(IconImageProperty, value);
        }
        #endregion IconImage property

        #region IconText property
        /// <summary>
        /// The icon text property backing store.
        /// </summary>
        public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(Segment), default(string));
        /// <summary>
        /// Gets or sets the icon text - alternative to ImageSource.
        /// </summary>
        /// <value>The icon text.</value>
        public string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }
        #endregion IconText property

        #region Text property
        /// <summary>
        /// Backing store for the Text bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Segment), null);
        /// <summary>
        /// Gets or sets the text for this <see cref="Segment"/>.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region HtmlText property
        /// <summary>
        /// Backing store for the formatted text property.
        /// </summary>
        public static readonly BindableProperty HtmlTextProperty = BindableProperty.Create(nameof(HtmlText), typeof(string), typeof(Segment), null,
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
            get => (string)GetValue(HtmlTextProperty);
            set => SetValue(HtmlTextProperty, value);
        }
        #endregion HtmlText property

        #region TextColor property
        /// <summary>
        /// Backing store for the Segment.TextColor bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Segment), Color.Default);
        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion

        #region FontAttributes property
        internal bool FontAttributesSet;
        /// <summary>
        /// Backing store for the Segment.FontAttributes bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(Segment), FontAttributes.None);
        /// <summary>
        /// Gets or sets the font attributes.
        /// </summary>
        /// <value>The font attributes.</value>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
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
        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(Segment), true);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Segment"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }
        #endregion IsEnabled property

        #region IsSelected property
        /// <summary>
        /// The backing store for the is selected property.
        /// </summary>
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(Segment), false, BindingMode.TwoWay);
        /// <summary>
        /// 
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        #endregion IsSelected property

        #region Orientation property
        /// <summary>
        /// The backing store for the orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(Segment), StackOrientation.Horizontal);
        /// <summary>
        /// Gets or sets the image/lable orientation.
        /// </summary>
        /// <value>The iamge/label orientation.</value>
        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion Orientation property

        #region ICommand property
        /// <summary>
        /// Backing store for the Segment.Command bindable property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Segment), null, BindingMode.OneWay, null,
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
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion ICommand property

        #region ICommandParameter property
        /// <summary>
        /// Backing store for the Segment.CommandParameter bindable property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Segment), null, BindingMode.OneWay, null,
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
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        #endregion ICommandParameter property

        #region VisualElement property
        /// <summary>
        /// Gets the visual element used to render the Segment (to support Bubble Popup).
        /// </summary>
        /// <value>The visual element.</value>
        public VisualElement VisualElement => _button;

        #endregion

        #endregion ISegment properties


        #region Event Handlers

        #region Tapped event handler
        /// <summary>
        /// Occurs when Segment is tapped.
        /// </summary>
        public event EventHandler Tapped
        {
            add { _button.Tapped += value; }
            remove { _button.Tapped -= value; }
        }
        #endregion Tapped eventhandler

        #region Selected event handler
        /// <summary>
        /// Occurs when Segment is selected.
        /// </summary>
        public event EventHandler Selected
        {
            add { _button.Selected += value; }
            remove { _button.Selected -= value; }
        }
        #endregion Selected event handler

        #region LongPressing event handlre
        /// <summary>
        /// Occurs when long pressing.
        /// </summary>
        public event EventHandler LongPressing
        {
            add { _button.LongPressing += value; }
            remove { _button.LongPressing -= value; }
        }
        #endregion LongPressing event handlre

        #region LongPressed event handler
        /// <summary>
        /// Occurs when long pressed.
        /// </summary>
        public event EventHandler LongPressed
        {
            add { _button.LongPressed += value; }
            remove { _button.LongPressed -= value; }
        }
        #endregion LongPressed event handler

        #endregion Event Handlers


        #region Fields
        internal GroupToggleBehavior _toggleBehavior;
        internal readonly SegmentButton _button;
        #endregion Fields


        #region Constuctors
        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        public Segment()
        {
            P42.Utils.Debug.AddToCensus(this);

            _button = new SegmentButton();
            _button.PropertyChanged += OnButtonPropertyChanged;
        }


        /// <summary>
        /// Instantiates an new Segment and sets its Text and imageSource properties
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        public Segment(string text, Forms9Patch.Image image) : this()
        {
            Text = text;
            if (image != null)
                IconImage = image;
        }

        /// <summary>
        /// Instantiates an new Segment and sets its Text property and sets the IconImage to a new image, created using the provided ImageSource
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imageSource"></param>
        public Segment(string text, Xamarin.Forms.ImageSource imageSource = null) : this()
        {
            Text = text;
            if (imageSource != null)
                IconImage = new Forms9Patch.Image(imageSource);
        }

        /// <summary>
        /// Instantiates a new Segment and sets its Text and either its IconText or its IconImage, created using the provided embedded resource id 
        /// </summary>
        /// <param name="text">Segment's text</param>
        /// <param name="icon">Segments's icon (either EmbeddedResourceId or HtmlText)</param>
        /// <param name="assembly">Assembly that has EmbeddedResource used for Icon</param>
        public Segment(string text, string icon, Assembly assembly = null) : this()
        {
            Text = text;
            var isIconText = false;


            assembly = assembly ?? AssemblyExtensions.AssemblyFromResourceId(icon);
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, Array.Empty<object>());

            var match = Forms9Patch.ImageSource.BestEmbeddedMultiResourceMatch(icon, assembly);

            //if (icon.Contains("<") && icon.Contains("/>"))
            if (match == null)
            {
                var sansStarts = icon.Replace("<", "");
                var starts = icon.Length - sansStarts.Length;
                var sansEnds = icon.Replace(">", "");
                var ends = icon.Length - sansEnds.Length;
                isIconText = starts == ends;
            }
            if (isIconText)
                IconText = icon;
            else
            {
                IconImage = new Forms9Patch.Image(icon, assembly);
            }
        }

        private bool _disposed;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;

                if (Command != null)
                    Command.CanExecuteChanged -= CommandCanExecuteChanged;

                _button.PropertyChanged -= OnButtonPropertyChanged;
                _button.IconImage = null;
                _button.BackgroundImage = null;
                _button.Dispose();

                IconImage?.Dispose();
                IconImage = null;

                P42.Utils.Debug.RemoveFromCensus(this);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Constuctor


        #region PropertyChanged responder
        void OnButtonPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IconImage):
                    IconImage = _button.IconImage;
                    break;
                case nameof(IconText):
                    IconText = _button.IconText;
                    break;
                case nameof(Text):
                    Text = _button.Text;
                    break;
                case nameof(HtmlText):
                    HtmlText = _button.HtmlText;
                    break;
                case nameof(TextColor):
                    TextColor = _button.TextColor;
                    break;
                case nameof(FontAttributes):
                    FontAttributes = _button.FontAttributes;
                    break;
                case nameof(IsEnabled):
                    IsEnabled = _button.IsEnabled;
                    break;
                case nameof(IsSelected):
                    IsSelected = _button.IsSelected;
                    break;
                case nameof(Orientation):
                    Orientation = _button.Orientation;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <returns>The property changed.</returns>
        /// <param name="propertyName">Property name.</param>
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

            switch (propertyName)
            {
                case nameof(IconImage):
                    _button.IconImage = IconImage;
                    break;
                case nameof(IconText):
                    _button.IconText = IconText;
                    break;
                case nameof(Text):
                    _button.Text = Text;
                    break;
                case nameof(HtmlText):
                    _button.HtmlText = HtmlText;
                    break;
                case nameof(TextColor):
                    _button.TextColor = TextColor;
                    break;
                case nameof(FontAttributes):
                    _button.FontAttributes = FontAttributes;
                    break;
                case nameof(IsEnabled):
                    _button.IsEnabled = IsEnabled;
                    break;
                case nameof(IsSelected):
                    _button.IsSelected = IsSelected;
                    break;
                case nameof(Orientation):
                    _button.Orientation = Orientation;
                    break;
                case nameof(Command):
                    System.Diagnostics.Debug.WriteLine("COMMAND PROPERTY CHANGED");
                    break;
                default:
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
            _button.Tap();
        }

        /// <summary>
        /// Programmically click the segement
        /// </summary>
        public void SendClicked()
        {
            _button.SendClicked();
        }
        #endregion


        #region Property Change management
        void OnCommandChanged()
        {
            _button.Command = Command;
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
            _button.CommandParameter = CommandParameter;
            var command = Command;
            if (command == null)
                return;
            //IsEnabledCore = command.CanExecute(CommandParameter);
        }
        #endregion Property Change management
    }
}

