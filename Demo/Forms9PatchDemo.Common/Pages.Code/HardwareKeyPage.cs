using System;
using Xamarin.Forms;
using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HardwareKeyPage : Forms9Patch.HardwareKeyPage
    {
        #region Visual Elements
        readonly Xamarin.Forms.Label _label = new Xamarin.Forms.Label { Text = "Xamarin.Forms.Label: HardwareKeyPage" };

        readonly Xamarin.Forms.Button _modalButton = new Xamarin.Forms.Button { Text = "Modal Push Page1" };
        readonly Xamarin.Forms.Button _navButton = new Xamarin.Forms.Button { Text = "Navigate Push Page1" };

        readonly Xamarin.Forms.Editor _editor = new Xamarin.Forms.Editor { Text = "Xamarin.Forms.Editor: RootPage" };

        readonly Xamarin.Forms.Entry _entry = new Xamarin.Forms.Entry { Text = "Xamarin.Forms.Entry: RootPage" };

        readonly Xamarin.Forms.Label _inputLabel = new Xamarin.Forms.Label { Text = "Hardware Keyboard Input" };
        readonly Xamarin.Forms.Label _modifiersLabel = new Xamarin.Forms.Label { Text = "Hardware Keyboard Modifiers" };
        readonly Xamarin.Forms.Label _keyboardType = new Xamarin.Forms.Label { Text = "Hardware Keyboard Type" };

        Forms9Patch.SegmentedControl _segmentedControl = new SegmentedControl
        {
            Margin = new Thickness(10, 0),
            FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf",
            Segments =
                {
                    new Forms9Patch.Segment("label"),
                    new Forms9Patch.Segment("editor"),
                    new Forms9Patch.Segment("entry"),
                    new Forms9Patch.Segment("button"),
                }
        };

        readonly Xamarin.Forms.Label _focusLabel = new Xamarin.Forms.Label { Text = "HardwareKeyPage.FocusedElement:" };

        readonly Xamarin.Forms.Label _defaultFocusLabel = new Xamarin.Forms.Label { Text = "HardwareKeyPage.DefaultFocusedElement:" };

        #endregion

        public HardwareKeyPage()
        {
            var popup = new ModalPopup
            {
                Content = new Xamarin.Forms.Label { Text = "Modal Popup!" },
            };
            var popupButton = new Xamarin.Forms.Button { Text = "Display Popup" };
            popupButton.Clicked += (sender, e) =>
            {
                popup.IsVisible = true;
            };

            popup.Appearing += (sender, e) => Forms9Patch.HardwareKeyPage.DefaultFocusedElement = popup;

            popup.Disappearing += (sender, e) => Forms9Patch.HardwareKeyPage.DefaultFocusedElement = this;

            /* IMPORTANT NOTE TO DEVELOPERS USING HARDWARE KEY LISTENING
             In order to make Hardware Key Listening to work, Forms9Patch needs
             to keep up with when a Xamarin.Forms.VisualElement has been focused
             or unfocused.  

             Unfortunately, Xamarin.Forms does not provide a global way 
             to monitor this so Forms9Patch uses the HardwareKeyPage element as
             a way to initiate the monitoring of focus changes on all of it's
             descendents in the visual tree.   To do this, Forms9Patch adds a Focused
             and Unfocused event handler to all these descendents.  For iOS and UWP, 
             this is no big deal.  For Android, like almost everything, this is a big 
             deal in that it can have a negative impact on performance.  If this is
             important to you, please read on.

             To address this performance issue, you will need to tweek the 
             Xmarin.Forms source code just a teeny-tiny bit adn then use your
             now customied code instead of the off-the-shelf NuGet packages.
             In particular, below are additions to the Xamarin.Forms.VisualElement class
             in the Xamarin.Forms.Core.VisualElement.cs file: 

                public static event EventHandler<VisualElement> FocusChanged;

                static VisualElement _currentlyFocused;
                public static VisualElement CurrentlyFocused
                {
                    get => _currentlyFocused;
                    private set
                    {
                        if (_currentlyFocused != value)
                        {
                            var wasFocused = _currentlyFocused;
                            _currentlyFocused = value;
                            FocusChanged?.Invoke(wasFocused, _currentlyFocused);
                        }
                    }
                }

                protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
                {
                    base.OnPropertyChanged(propertyName);
                    if (propertyName == IsFocusedProperty.PropertyName)
                    {
                        if (IsFocused)
                            CurrentlyFocused = this;
                        else if (CurrentlyFocused == this)
                            CurrentlyFocused = null;
                    }
                }

            Upon instantiation, Forms9Patch.HardwareKeyListener will use reflection
            to look for the above changes.  If it finds them, it will use the above
            code to monitor the focus, rather than adding a bunch of event listeneres.

                    */


            #region Hardware Key Listeners

            #region ... for this HardwareKeyPage
            this.AddHardwareKeyListener("ç", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("é", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("ф", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("A", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("Q", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("`", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("~", OnHardwareKeyPressed);


            this.AddHardwareKeyListener("1", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("!", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("2", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("@", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("3", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("#", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("4", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("$", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("5", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("%", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("6", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("^", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("7", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("&", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("8", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("*", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("9", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("(", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("0", OnHardwareKeyPressed);
            this.AddHardwareKeyListener(")", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("-", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("_", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("+", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("=", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("0", OnHardwareKeyPressed);
            this.AddHardwareKeyListener(")", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("[", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("{", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("]", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("}", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("\\", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("|", OnHardwareKeyPressed);

            this.AddHardwareKeyListener(";", OnHardwareKeyPressed);
            this.AddHardwareKeyListener(":", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("'", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("\"", OnHardwareKeyPressed);

            this.AddHardwareKeyListener(",", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("<", OnHardwareKeyPressed);

            this.AddHardwareKeyListener(".", OnHardwareKeyPressed);
            this.AddHardwareKeyListener(">", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("/", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("?", OnHardwareKeyPressed);

            this.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.BackspaceDeleteKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.ForwardDeleteKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.InsertKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.TabKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EnterReturnKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.PageUpKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.PageDownKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.HomeKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EndKeyInput, HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            #endregion

            #region ... for _entry
            _entry.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #region ... for _editor
            _editor.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #region ... for _label
            _label.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #region ... for _modal
            _modalButton.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _modalButton.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _modalButton.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _modalButton.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _modalButton.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #region ... for _modal
            popup.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            popup.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            popup.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            popup.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            popup.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #endregion


            #region Layout

            Padding = new Thickness(5, 25, 5, 5);

            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children =
                    {
                    _label,
                    _editor,
                    _entry,
                    _modalButton,
                    _navButton,
                    new Xamarin.Forms.Label { Text="Focus:"},
                    _segmentedControl,
                    new BoxView { Color = Color.Black, HeightRequest = 2 },
                        _focusLabel,
                        new BoxView { Color = Color.Black, HeightRequest = 2 },
                        _defaultFocusLabel,
                        new BoxView { Color = Color.Black, HeightRequest = 2 },
                    _inputLabel,
                    _modifiersLabel,
                    _keyboardType,
                        popupButton,
                    }
                }

            };

            _defaultFocusLabel.Text = "HardwareKeyPage.DefaultFocusedElement: " + Forms9Patch.HardwareKeyPage.DefaultFocusedElement;

            #endregion


            #region event handlers
            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            _segmentedControl.SegmentTapped += (sender, e) =>
            {
                switch (e.Segment.Text)
                {
                    case "label": _label.HardwareKeyFocus(); break;
                    case "editor": _editor.HardwareKeyFocus(); break;
                    case "entry": _entry.HardwareKeyFocus(); break;
                    case "button": _modalButton.HardwareKeyFocus(); break;
                }
            };

            Forms9Patch.HardwareKeyPage.FocusedElementChanged += (sender, e) =>
            {
                _defaultFocusLabel.Text = "HardwareKeyPage.DefaultFocusedElement: " + Forms9Patch.HardwareKeyPage.DefaultFocusedElement;
                _focusLabel.Text = "HardwareKeyPage.FocusedElement: " + sender;
                if (sender == _label)
                    _segmentedControl.SelectIndex(0);
                else if (sender == _editor)
                    _segmentedControl.SelectIndex(1);
                else if (sender == _entry)
                    _segmentedControl.SelectIndex(2);
                else if (sender == _modalButton)
                    _segmentedControl.SelectIndex(3);
                else
                    _segmentedControl.DeselectAll();
            };

            _modalButton.Clicked += async (object sender, EventArgs e) =>
                {
                    var page1 = new HardwareKeyPage1(true);
                    await Navigation.PushModalAsync(page1);
                    //await Navigation.PushAsync(page1);
                };

            _navButton.Clicked += async (object sender, EventArgs e) =>
            {
                var page1 = new HardwareKeyPage1(false);
                //await Navigation.PushModalAsync(page1);
                await Navigation.PushAsync(page1);
            };

            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            var labelGestureListener = FormsGestures.Listener.For(_label);
            labelGestureListener.Tapped += (s, e) =>
            {
                _label.HardwareKeyFocus();
                e.Handled = true;
            };

            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            var pageGestureListener = FormsGestures.Listener.For(this);
            pageGestureListener.Tapped += (s, e) => Forms9Patch.HardwareKeyPage.FocusedElement = null;
            #endregion

        }

        #region Handlers
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Forms9Patch.HardwareKeyPage.DefaultFocusedElement = this;
        }

        void OnHardwareKeyPressed(object sender, HardwareKeyEventArgs e)
        {
            _inputLabel.Text = e.HardwareKey.KeyInput;
            _modifiersLabel.Text = e.HardwareKey.ModifierKeys.ToString();
            _keyboardType.Text = Forms9Patch.KeyboardService.LanguageRegion;
            System.Diagnostics.Debug.WriteLine("FocusedElement=[" + Forms9Patch.HardwareKeyPage.FocusedElement + "] KeyInput=[" + e.HardwareKey.KeyInput + "] ModifierKeys=[" + e.HardwareKey.ModifierKeys + "] Layout=[" + Forms9Patch.KeyboardService.LanguageRegion + "]");
        }
        #endregion

    }
}
