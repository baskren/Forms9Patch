// /*******************************************************************
//  *
//  * EmbeddedResourceFontEffectPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using Forms9Patch;
namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class EmbeddedResourceFontEffectPage : Forms9Patch.HardwareKeyPage
    {
        Xamarin.Forms.Label _label = new Xamarin.Forms.Label
        {
            Text = "Xamarin.Forms.Label",
            FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
        };

        Xamarin.Forms.Editor _editor = new Xamarin.Forms.Editor
        {
            Text = "Xamarin.Forms.Editor",
            FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
        };

        Xamarin.Forms.Entry _entry = new Xamarin.Forms.Entry
        {
            Text = "Xamarin.Forms.Entry",
            FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf"
        };

        public EmbeddedResourceFontEffectPage()
        {
            Forms9Patch.EmbeddedResourceFontEffect.ApplyTo(_label);
            _editor.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());
            _entry.Effects.Add(new Forms9Patch.EmbeddedResourceFontEffect());


            Forms9Patch.SegmentedControl segmentedControl = new SegmentedControl
            {
                Margin = new Thickness(10, 0),
                FontFamily = "Forms9PatchDemo.Resources.Fonts.Pacifico.ttf",
                Segments =
                {
                    new Forms9Patch.Segment("label"),
                    new Forms9Patch.Segment("editor"),
                    new Forms9Patch.Segment("entry"),
                }
            };

            Content = new Xamarin.Forms.StackLayout
            {
                Children =
                {
                    _label,
                    _editor,
                    _entry,
                    new Xamarin.Forms.Label { Text="Focus:"},
                    segmentedControl
                }
            };

            segmentedControl.SegmentTapped += (sender, e) =>
            {
                switch (e.Segment.Text)
                {
                    case "label": _label.HardwareKeyFocus(); break;
                    case "editor": _editor.HardwareKeyFocus(); break;
                    case "entry": _entry.HardwareKeyFocus(); break;
                }
            };

            HardwareKeyPage.FocusedElementChanged += (sender, e) =>
            {
                if (sender == _label)
                    segmentedControl.SelectIndex(0);
                else if (sender == _editor)
                    segmentedControl.SelectIndex(1);
                else if (sender == _entry)
                    segmentedControl.SelectIndex(2);
                else
                    segmentedControl.DeselectAll();
            };

            _label.Focused += (sender, e) => System.Diagnostics.Debug.WriteLine("label.Focused " + e.IsFocused);
            _editor.Focused += (sender, e) => System.Diagnostics.Debug.WriteLine("editor.Focused " + e.IsFocused);
            _entry.Focused += (sender, e) => System.Diagnostics.Debug.WriteLine("entry.Focused " + e.IsFocused);

            _label.Unfocused += (sender, e) => System.Diagnostics.Debug.WriteLine("label.Unfocused " + e.IsFocused);
            _editor.Unfocused += (sender, e) => System.Diagnostics.Debug.WriteLine("editor.Unfocused " + e.IsFocused);
            _entry.Unfocused += (sender, e) => System.Diagnostics.Debug.WriteLine("entry.Unfocused " + e.IsFocused);

            _label.FocusChangeRequested += (object sender, FocusRequestArgs e) => System.Diagnostics.Debug.WriteLine("label.FocusChangeRequested ");
            _editor.FocusChangeRequested += (object sender, FocusRequestArgs e) => System.Diagnostics.Debug.WriteLine("editor.FocusChangeRequested ");
            _entry.FocusChangeRequested += (object sender, FocusRequestArgs e) => System.Diagnostics.Debug.WriteLine("entry.FocusChangeRequested ");

            this.AddHardwareKeyListener("ç", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("é", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("ф", OnHardwareKeyPressed);

            this.AddHardwareKeyListener("A", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.PlatformKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Control, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Alternate, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.CapsLock, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.FunctionKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Shift, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Shift | HardwareKeyModifierKeys.Alternate, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Shift | HardwareKeyModifierKeys.Control, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Shift | HardwareKeyModifierKeys.PlatformKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Control | HardwareKeyModifierKeys.Alternate, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("a", HardwareKeyModifierKeys.Control | HardwareKeyModifierKeys.PlatformKey, OnHardwareKeyPressed);


            this.AddHardwareKeyListener("5", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.NumericPadKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.PlatformKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.Control, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.Alternate, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.CapsLock, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.FunctionKey, OnHardwareKeyPressed);
            this.AddHardwareKeyListener("5", HardwareKeyModifierKeys.Shift, OnHardwareKeyPressed);



            this.AddHardwareKeyListener("/", OnHardwareKeyPressed);
            this.AddHardwareKeyListener("/", HardwareKeyModifierKeys.Alternate, OnHardwareKeyPressed);

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

            _entry.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _entry.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);

            _editor.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _editor.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);

            _label.AddHardwareKeyListener(HardwareKey.UpArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.DownArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.LeftArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.RightArrowKeyInput, OnHardwareKeyPressed);
            _label.AddHardwareKeyListener(HardwareKey.EscapeKeyInput, OnHardwareKeyPressed);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HardwareKeyPage.DefaultFocusedElement = this;
        }

        void OnHardwareKeyPressed(object sender, HardwareKeyEventArgs e) => System.Diagnostics.Debug.WriteLine("FocusedElement=[" + Forms9Patch.HardwareKeyPage.FocusedElement + "] KeyInput=[" + e.HardwareKey.KeyInput + "] ModifierKeys=[" + e.HardwareKey.ModifierKeys + "]");

    }
}

