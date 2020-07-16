using System;
using Forms9Patch;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HardwareKeyPage1 : Forms9Patch.HardwareKeyPage
    {
        #region Visual Elements
        readonly Xamarin.Forms.Label _label = new Xamarin.Forms.Label { Text = "Xamarin.Forms.Label: ModalHardwareKeyPage" };

        readonly Xamarin.Forms.Button _button = new Xamarin.Forms.Button { Text = "Pop Page1" };

        readonly Xamarin.Forms.Editor _editor = new Xamarin.Forms.Editor { Text = "Xamarin.Forms.Editor: Page1" };

        readonly Xamarin.Forms.Entry _entry = new Xamarin.Forms.Entry { Text = "Xamarin.Forms.Entry: Page1" };

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

        public Xamarin.Forms.Button Button => _button;

        #endregion

        public HardwareKeyPage1(bool modal)
        {

            #region Hardware Key Listeners

            #region ... for this HardwareKeyPage
            this.AddHardwareKeyListener("ç", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("é", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("ф", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("A", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("`", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("~", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);


            this.AddHardwareKeyListener("1", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("!", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("2", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("@", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("3", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("#", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("4", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("$", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("%", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("6", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("^", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("7", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("&", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("8", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("*", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("9", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("(", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("0", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(")", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("-", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("_", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("+", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("=", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("0", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(")", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("[", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("{", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("]", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("}", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("\\", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("|", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener(";", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(":", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("'", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("\"", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener(",", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("<", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener(".", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(">", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);

            this.AddHardwareKeyListener("/", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("?", HardwareKeyModifierKeys.Any, OnHardwareKeyPressed);


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

            this.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.BackspaceDeleteKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.ForwardDeleteKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.InsertKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.TabKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EnterReturnKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.PageUpKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.PageDownKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.HomeKeyInput, OnHardwareKeyPressed);
            this.AddHardwareKeyListener(HardwareKey.EndKeyInput, OnHardwareKeyPressed);
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

            #region ... for _button
            _button.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _button.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _button.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _button.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _button.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);
            #endregion

            #endregion


            #region layout
            Padding = new Thickness(5, 25, 5, 5);

            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children = {
                    _label,
                    _editor,
                    _entry,
                    _button,
                    new Xamarin.Forms.Label { Text="Focus:"},
                    _segmentedControl,
                        new BoxView { Color = Color.Black, HeightRequest = 2 },
                        _focusLabel,
                        new BoxView { Color = Color.Black, HeightRequest = 2 },
                        _defaultFocusLabel,
                    new BoxView { Color = Color.Black, HeightRequest = 2 },
                    _inputLabel,
                    _modifiersLabel,
                    _keyboardType
                    }
                }
            };
            #endregion


            #region Event Handlers
            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            _segmentedControl.SegmentTapped += (sender, e) =>
            {
                switch (e.Segment.Text)
                {
                    case "label": _label.HardwareKeyFocus(); break;
                    case "editor": _editor.HardwareKeyFocus(); break;
                    case "entry": _entry.HardwareKeyFocus(); break;
                    case "button": _button.HardwareKeyFocus(); break;
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
                else if (sender == _button)
                    _segmentedControl.SelectIndex(3);
                else
                    _segmentedControl.DeselectAll();
            };



            _button.Clicked += async (object sender, EventArgs e) =>
            {
                if (modal)
                    await Navigation.PopModalAsync();
                else
                    await Navigation.PopAsync();
            };

            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            var labelGestureListener = FormsGestures.Listener.For(_label);
            labelGestureListener.Tapped += (s, e) => _label.HardwareKeyFocus();

            // NOTE: The only elements that change the focus are Entry and Editor.  As such, if we want to change focus when a user takes an action other then selecting an Editor or Entry, we have change the focus ourselves.
            var pageGestureListener = FormsGestures.Listener.For(this);
            pageGestureListener.Tapped += (s, e) => Forms9Patch.HardwareKeyPage.FocusedElement = null;

            #endregion
        }


        #region Event Handlers
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

