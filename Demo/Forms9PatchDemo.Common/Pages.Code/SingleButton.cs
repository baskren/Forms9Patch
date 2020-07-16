using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SingleButton : ContentPage
    {
        public SingleButton()
        {
            var button = new Forms9Patch.Button { Text = "Click me", ToggleBehavior = true, BackgroundColor = Color.NavajoWhite, OutlineRadius = 5 };
            var footerButtonA = new Forms9Patch.Button
            {
                FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                Text = "",
                Margin = 5,
                Padding = 0,
                BackgroundColor = Color.White,
                OutlineWidth = 0,
            };

            var footerButtonB = new Forms9Patch.Button
            {
                IconFontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                IconText = "",
                Margin = 5,
                Padding = 0,
                BackgroundColor = Color.White,
                OutlineWidth = 0,
            };


            var hasShadowSwitch = new Switch();
            hasShadowSwitch.Toggled += (sender, e) =>
            {
                button.HasShadow = e.Value;
                footerButtonA.HasShadow = e.Value;
                footerButtonB.HasShadow = e.Value;
            };
            var hasOutlineSwitch = new Switch();

            hasOutlineSwitch.Toggled += (sender, e) =>
            {
                button.OutlineWidth = e.Value ? 1 : 0;
                footerButtonA.OutlineWidth = e.Value ? 1 : 0;
                footerButtonB.OutlineWidth = e.Value ? 1 : 0;
            };

            //var longPressSwitch = new Switch();
            //longPressSwitch.Toggled += (s, e) => button.IsLongPressEnabled = e.Value;

            Padding = 0;

            var stack = new StackLayout
            {
                Padding = 20,
                Children = {
                    new Label { Text = "HasShadow" },
                    hasShadowSwitch,
                    new Label { Text = "HasOutline"},
                    hasOutlineSwitch,
                    //new Label { Text = "Long Press Enabled"},
                    //longPressSwitch,

                    button,
                }
            };

            var footer = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 50 },
                    new ColumnDefinition { Width = 50 },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = 50 },
                    new ColumnDefinition { Width = 50 },
                }
            };

            footer.Children.Add(footerButtonA, 3, 0);
            footer.Children.Add(footerButtonB, 4, 0);

            var grid = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = 50 }
                }
            };

            grid.Children.Add(stack);
            grid.Children.Add(new BoxView { Color = Color.DarkGray }, 0, 1);
            grid.Children.Add(footer, 0, 1);


            Content = grid;


            //button.IsLongPressEnabled = longPressSwitch.IsToggled;
            button.HasShadow = hasShadowSwitch.IsToggled;
            footerButtonA.HasShadow = hasShadowSwitch.IsToggled;
            footerButtonB.HasShadow = hasShadowSwitch.IsToggled;
            button.OutlineWidth = hasOutlineSwitch.IsToggled ? 1 : 0;
            footerButtonA.OutlineWidth = hasOutlineSwitch.IsToggled ? 1 : 0;
            footerButtonB.OutlineWidth = hasOutlineSwitch.IsToggled ? 1 : 0;

            button.Tapped += (object sender, EventArgs e) => Forms9Patch.Toast.Create(null, "Tapped: ");

            button.Clicked += (object sender, EventArgs e) => Forms9Patch.Toast.Create(null, "Clicked");

            button.Selected += (object sender, EventArgs e) => Forms9Patch.Toast.Create(null, "Selected");

            button.LongPressed += (object sender, EventArgs e) => Forms9Patch.Toast.Create(null, "Long Pressed");

            button.LongPressing += (object sender, EventArgs e) => Forms9Patch.Toast.Create(null, "Long Pressing");

        }

    }
}