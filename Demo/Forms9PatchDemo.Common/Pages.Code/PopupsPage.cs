using System;
using Xamarin.Forms;
using Forms9Patch;
using System.Collections.Generic;

namespace Forms9PatchDemo.Pages.Code
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PopupsPage : Xamarin.Forms.ContentPage
    {
        bool _hasShadow;
        bool _shadowInverted;
        bool _blueOutline;

        const string _hasShadowText = "HasShadow";
        const string _shadowInvertedText = "ShadowInverted";
        const string _blueOutlineText = "Blue Outline";

        const string bubbleLeftText = "← BubblePopup";
        const string bubbleRightText = "BubblePopup →";
        const string bubbleUpText = "↑ BubblePopup";
        const string bubbleDownText = "BubblePopup ↓";
        const string bubbleHzText = "← BubblePopup →";
        const string bubbleVtText = "↑ BubblePopup ↓";
        const string bubbleAnyText = "← ↑ BubblePopup → ↓";
        const string bubbleNoneText = " BubblePopup (none)";

        const string _layoutOptionStartText = "START";
        const string _layoutOptionCenterText = "CENTER";
        const string _layoutOptionEndText = "END";
        const string _layoutOptionFillText = "FILL";


        readonly SegmentedControl _decoration = new SegmentedControl
        {
            Segments = {
                new Segment(_hasShadowText),
                new Segment(_shadowInvertedText),
                new Segment(_blueOutlineText),
            },
            GroupToggleBehavior = GroupToggleBehavior.Multiselect,
            BackgroundColor = Color.White,
        };

        BubblePopup bubble;

        readonly SegmentedControl _hzLayoutOptions = new SegmentedControl
        {
            Segments = {
                new Segment(_layoutOptionStartText),
                new Segment(_layoutOptionCenterText),
                new Segment(_layoutOptionEndText),
                new Segment(_layoutOptionFillText),
            },
            GroupToggleBehavior = GroupToggleBehavior.Radio,
            BackgroundColor = Color.White,
        };
        readonly SegmentedControl _vtLayoutOptions = new SegmentedControl
        {
            Segments = {
                new Segment(_layoutOptionStartText),
                new Segment(_layoutOptionCenterText),
                new Segment(_layoutOptionEndText),
                new Segment(_layoutOptionFillText),
            },
            GroupToggleBehavior = GroupToggleBehavior.Radio,
            BackgroundColor = Color.White,
        };


        #region Modal Popup VisualElements
        readonly Forms9Patch.Button showModalButton = new Forms9Patch.Button("ModalPopup") { BackgroundColor = Color.White };
        static readonly Forms9Patch.Button popPushModalButton = new Forms9Patch.Button("POP & PUSH");
        static readonly Forms9Patch.Button cancelModalButton = new Forms9Patch.Button("CANCEL");

        readonly ModalPopup _modalPopup = new ModalPopup()
        {
            Content = new Xamarin.Forms.StackLayout
            {
                Children = {
                        new Forms9Patch.Label("ModalPopup") { FontAttributes=FontAttributes.Bold },
                        popPushModalButton,
                        cancelModalButton
                    },
                BackgroundColor = Color.Pink,
            },
            OutlineColor = Color.Blue,
        };
        #endregion

        Forms9Patch.Button showTargetedMenu;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //showTargetedMenu.Tap();
        }

        readonly Forms9Patch.ActivityIndicatorPopup _activity = new ActivityIndicatorPopup();

        public PopupsPage()
        {
            Padding = 20;
            _hzLayoutOptions.SelectIndex(1);
            _vtLayoutOptions.SelectIndex(1);

            #region ModalPopup
            cancelModalButton.Clicked += async (sender, e) => await _modalPopup.CancelAsync();
            popPushModalButton.Clicked += (sender, e) =>
            {
                _modalPopup.IsVisible = false;
                _modalPopup.IsVisible = true;
            };
            showModalButton.Clicked += (sender, e) =>
            {
                _modalPopup.HasShadow = _hasShadow;
                _modalPopup.ShadowInverted = _shadowInverted;
                _modalPopup.OutlineWidth = _blueOutline ? 1 : 0;
                _modalPopup.IsVisible = true;
                _modalPopup.HorizontalOptions = LayoutOption(_hzLayoutOptions);
                _modalPopup.VerticalOptions = LayoutOption(_vtLayoutOptions);
            };
            #endregion


            #region BubblePopups


            var showBubbleLeftButton = new Forms9Patch.Button(bubbleLeftText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleRightButton = new Forms9Patch.Button(bubbleRightText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleUpButton = new Forms9Patch.Button(bubbleUpText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleDownButton = new Forms9Patch.Button(bubbleDownText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleHzButton = new Forms9Patch.Button(bubbleHzText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleVtButton = new Forms9Patch.Button(bubbleVtText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleAnyButton = new Forms9Patch.Button(bubbleAnyText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };
            var showBubbleNoneButton = new Forms9Patch.Button(bubbleNoneText) { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center };

            var bubblePointerDirectionControl = new SegmentedControl
            {
                Segments =
                {
                    new Segment("←"),
                    new Segment("↑"),
                    new Segment("→"),
                    new Segment("↓"),
                    new Segment("↔"),
                    new Segment("↕"),
                    new Segment("↔ ↕"),
                    new Segment(" ")
                },
                SyncSegmentFontSizes = false,
            };

            var cancelBubbleButton = new Forms9Patch.Button("CANCEL");

            var bubbleTarget = new Forms9Patch.Frame
            {
                OutlineWidth = 1,
                OutlineColor = Color.Black,
                OutlineRadius = 4,
                Content = new Forms9Patch.Label("BUBBLE TARGET"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            bubble = new BubblePopup(bubbleTarget)
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children = {
                        new Forms9Patch.Label("BubblePopup") { FontAttributes=FontAttributes.Bold },
                        bubblePointerDirectionControl,
                        cancelBubbleButton
                    },
                },
            };

            bubblePointerDirectionControl.SegmentTapped += (sender, e) =>
            {
                switch (e.Segment.Text)
                {
                    case "←": bubble.PointerDirection = PointerDirection.Left; break;
                    case "↑": bubble.PointerDirection = PointerDirection.Up; break;
                    case "→": bubble.PointerDirection = PointerDirection.Right; break;
                    case "↓": bubble.PointerDirection = PointerDirection.Down; break;
                    case "↔": bubble.PointerDirection = PointerDirection.Horizontal; break;
                    case "↕": bubble.PointerDirection = PointerDirection.Vertical; break;
                    case "↔↕": bubble.PointerDirection = PointerDirection.Any; break;
                    case " ": bubble.PointerDirection = PointerDirection.None; break;
                }
            };

            cancelBubbleButton.Clicked += async (sender, e) => await bubble.CancelAsync();
            showBubbleLeftButton.Clicked += OnBubbleButtonClicked;
            showBubbleRightButton.Clicked += OnBubbleButtonClicked;
            showBubbleUpButton.Clicked += OnBubbleButtonClicked;
            showBubbleDownButton.Clicked += OnBubbleButtonClicked;
            showBubbleHzButton.Clicked += OnBubbleButtonClicked;
            showBubbleVtButton.Clicked += OnBubbleButtonClicked;
            showBubbleAnyButton.Clicked += OnBubbleButtonClicked;
            showBubbleNoneButton.Clicked += OnBubbleButtonClicked;
            #endregion


            #region ActivityPopup
            Forms9Patch.Button showActivityPopupButton = new Forms9Patch.Button("ActivityIndicatorPopup") { BackgroundColor = Color.White };
            showActivityPopupButton.Clicked += (sender, e) =>
            {
                //var activity = Forms9Patch.ActivityIndicatorPopup.Create();
                _activity.CancelOnPageOverlayTouch = true;
                _activity.IsVisible = true;
            };
            #endregion


            #region PermissionPopup
            var showPermissionButton = new Forms9Patch.Button("PermissionPopup") { BackgroundColor = Color.White };
            showPermissionButton.Clicked += (sender, e) =>
            {
                var permission = PermissionPopup.Create("PermissionPopup", "Do you agree?");
                permission.HorizontalOptions = LayoutOption(_hzLayoutOptions);
                permission.VerticalOptions = LayoutOption(_vtLayoutOptions);
                permission.OutlineColor = Color.Blue;
                permission.HasShadow = _hasShadow;
                permission.ShadowInverted = _shadowInverted;
                permission.OutlineWidth = _blueOutline ? 1 : 0;
            };
            #endregion


            #region Toast
            var showToastButton = new Forms9Patch.Button("Toast") { BackgroundColor = Color.White };
            showToastButton.Clicked += (sender, e) =>
            {
                var toast = Toast.Create("Toast", "... of the town!");
                toast.OutlineColor = Color.Blue;
                toast.HasShadow = _hasShadow;
                toast.ShadowInverted = _shadowInverted;
                toast.OutlineWidth = _blueOutline ? 1 : 0;
                toast.HorizontalOptions = LayoutOption(_hzLayoutOptions);
                toast.VerticalOptions = LayoutOption(_vtLayoutOptions);
            };
            #endregion


            #region TargetedToast
            var showTargetedToash = new Forms9Patch.Button("TargetedToast") { BackgroundColor = Color.White };
            showTargetedToash.Clicked += (sender, e) =>
            {
                var toast = TargetedToast.Create(showTargetedToash, "TargetedToast", "... has the far getted most!");
                toast.OutlineColor = Color.Blue;
                toast.HasShadow = _hasShadow;
                toast.ShadowInverted = _shadowInverted;
                toast.OutlineWidth = _blueOutline ? 1 : 0;
                toast.HorizontalOptions = LayoutOption(_hzLayoutOptions);
                toast.VerticalOptions = LayoutOption(_vtLayoutOptions);
            };
            #endregion


            #region TargetedMenu
            showTargetedMenu = new Forms9Patch.Button("TargetedMenu") { BackgroundColor = Color.White };
            var targetedMenu = new Forms9Patch.TargetedMenu(showTargetedMenu)
            {
                Segments =
                {
                    new Segment("Copy", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14D;</font>"),
                    new Segment("Cut", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14E;</font>"),
                    new Segment("Paste", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14F;</font>"),
                    new Segment("Segment A"),
                    new Segment("Segment B"),
                    new Segment("Segment C"),
                    new Segment("Segment D"),
                    new Segment("Segment E"),
                    new Segment("Segment F"),
                    new Segment("Segment G"),
                    new Segment("Segment H"),
                },
                TextColor = Color.White
            };
            showTargetedMenu.Clicked += (s, e) =>
            {
                targetedMenu.IsVisible = true;
                targetedMenu.HasShadow = _hasShadow;
                targetedMenu.ShadowInverted = _shadowInverted;
                targetedMenu.IsVisible = true;
            };
            targetedMenu.SegmentTapped += (s, e) => System.Diagnostics.Debug.WriteLine("TargetedMenu.SegmentTapped: " + e.Segment.Text);
            #endregion


            #region Vertical TargetedMenu
            var showVerticalTargetedMenu = new Forms9Patch.Button("Vertical TargetedMenu") { BackgroundColor = Color.White };
            var verticalTargetedMenu = new Forms9Patch.TargetedMenu(showVerticalTargetedMenu)
            {
                Orientation = StackOrientation.Vertical,
                Segments =
                {
                    new Segment("Copy", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14D;</font>"),
                    new Segment("Cut", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14E;</font>"),
                    new Segment("Paste", "<font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\">&#xE14F;</font>"),
                    new Segment("Segment A"),
                    new Segment("Segment B"),
                    new Segment("Segment C"),
                    new Segment("Segment D"),
                    new Segment("Segment E"),
                    new Segment("Segment F"),
                    new Segment("Segment G"),
                    new Segment("Segment H"),
                },
            };
            showVerticalTargetedMenu.Clicked += (s, e) =>
            {
                //verticalTargetedMenu.OutlineColor = Color.Blue;
                verticalTargetedMenu.IsVisible = true;
                verticalTargetedMenu.HasShadow = _hasShadow;
                verticalTargetedMenu.ShadowInverted = _shadowInverted;
                verticalTargetedMenu.IsVisible = true;
            };
            verticalTargetedMenu.SegmentTapped += (s, e) => System.Diagnostics.Debug.WriteLine("TargetedMenu.SegmentTapped: " + e.Segment.Text);
            #endregion


            #region SoftwareKeyboardTest

            var yearPicker = new Xamarin.Forms.Picker
            {
                Title = "SELECT YEAR",
                ItemsSource = new List<string> { "SELECT YEAR", "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2006", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020" },
                SelectedItem = "SELECT YEAR",
                SelectedIndex = 0,
                TextColor = Color.LightGray,
            };
            yearPicker.SelectedIndexChanged += (s, e) => yearPicker.TextColor = yearPicker.SelectedIndex == 0 ? Color.LightGray : Color.Blue;

            var monthPicker = new Xamarin.Forms.Picker
            {
                Title = "SELECT MONTH",
                ItemsSource = new List<string> { "SELECT MONTH", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                SelectedItem = "SELECT MONTH",
                TextColor = Color.LightGray,
            };
            monthPicker.SelectedIndexChanged += (s, e) => monthPicker.TextColor = monthPicker.SelectedIndex == 0 ? Color.LightGray : Color.Blue;


            var softwareKeyboardTestButton = new Forms9Patch.Button("Software Keyboard Test") { BackgroundColor = Color.White };
            var softwareKeyboardTestPopup = new ModalPopup
            {
                HorizontalOptions = LayoutOption(_hzLayoutOptions),
                VerticalOptions = LayoutOption(_vtLayoutOptions),
                Content =
                new Xamarin.Forms.StackLayout
                {
                    Children = {
                        yearPicker,
                        monthPicker,
                        new Xamarin.Forms.Entry
                        {
                            Placeholder = "ENTER FIRST NAME",
                            TextColor = Color.Blue,
                            PlaceholderColor = Color.LightGray,
                        },
                        new Xamarin.Forms.Entry
                        {
                            Placeholder = "ENTER LAST NAME",
                            TextColor = Color.Blue,
                            PlaceholderColor = Color.LightGray,
                        },
                    }
                }
            };
            softwareKeyboardTestButton.Clicked += (s, e) =>
            {
                softwareKeyboardTestPopup.OutlineColor = Color.Blue;
                softwareKeyboardTestPopup.HasShadow = _hasShadow;
                softwareKeyboardTestPopup.ShadowInverted = _shadowInverted;
                softwareKeyboardTestPopup.OutlineWidth = _blueOutline ? 1 : 0;
                softwareKeyboardTestPopup.IsVisible = true;
                softwareKeyboardTestPopup.HorizontalOptions = LayoutOption(_hzLayoutOptions);
                softwareKeyboardTestPopup.VerticalOptions = LayoutOption(_vtLayoutOptions);
            };
            #endregion

            Content = new Xamarin.Forms.ScrollView
            {
                Content = new Xamarin.Forms.StackLayout
                {
                    Children =
                    {
                        new BoxView { HeightRequest = 1},
                        _decoration,
                        new Forms9Patch.Label("HZ LAYOUT ALIGNMENT:"),
                        _hzLayoutOptions,
                        new Forms9Patch.Label("VT LAYOUT ALIGNMENT:"),
                        _vtLayoutOptions,
                        new BoxView { HeightRequest = 1},
                        showModalButton,
                        showBubbleLeftButton,
                        showBubbleRightButton,
                        showBubbleUpButton,
                        showBubbleDownButton,
                        showBubbleHzButton,
                        showBubbleVtButton,
                        showBubbleAnyButton,
                        showBubbleNoneButton,
                        bubbleTarget,
                        showActivityPopupButton,
                        showPermissionButton,
                        showToastButton,
                        showTargetedToash,
                        showTargetedMenu,
                        showVerticalTargetedMenu,
                        softwareKeyboardTestButton,
                        new BoxView { HeightRequest = 1},
                    }
                }
            };

            _decoration.SegmentTapped += (s, e) =>
            {
                switch (e.Segment.Text)
                {
                    case _hasShadowText:
                        _hasShadow = !_hasShadow;
                        break;
                    case _shadowInvertedText:
                        _shadowInverted = !_shadowInverted;
                        break;
                    case _blueOutlineText:
                        _blueOutline = !_blueOutline;
                        break;
                }
            };

            BackgroundColor = Color.LightSlateGray;
        }

        void OnBubbleButtonClicked(object sender, EventArgs e)
        {
            bubble.OutlineColor = Color.Blue;
            bubble.HasShadow = _hasShadow;
            bubble.ShadowInverted = _shadowInverted;
            bubble.OutlineWidth = _blueOutline ? 1 : 0;
            bubble.IsVisible = true;
            bubble.HorizontalOptions = LayoutOption(_hzLayoutOptions);
            bubble.VerticalOptions = LayoutOption(_vtLayoutOptions);

            switch (((Forms9Patch.Button)sender).Text)
            {
                case bubbleLeftText: bubble.PointerDirection = PointerDirection.Left; break;
                case bubbleRightText: bubble.PointerDirection = PointerDirection.Right; break;
                case bubbleUpText: bubble.PointerDirection = PointerDirection.Up; break;
                case bubbleDownText: bubble.PointerDirection = PointerDirection.Down; break;
                case bubbleHzText: bubble.PointerDirection = PointerDirection.Horizontal; break;
                case bubbleVtText: bubble.PointerDirection = PointerDirection.Vertical; break;
                case bubbleAnyText: bubble.PointerDirection = PointerDirection.Any; break;
                default: bubble.PointerDirection = PointerDirection.None; break;
            }
        }

        LayoutOptions LayoutOption(SegmentedControl control)
        {
            switch (control.SelectedSegments[0].Text)
            {
                case _layoutOptionStartText: return LayoutOptions.Start;
                case _layoutOptionCenterText: return LayoutOptions.Center;
                case _layoutOptionEndText: return LayoutOptions.End;
                case _layoutOptionFillText: return LayoutOptions.Fill;
            }
            return LayoutOptions.Center;
        }
    }
}
