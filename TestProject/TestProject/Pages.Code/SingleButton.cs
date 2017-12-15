using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    public class SingleButton : ContentPage
    {
        public SingleButton()
        {
            var button = new Forms9Patch.Button { Text = "Click me", ToggleBehavior = true, BackgroundColor = Color.NavajoWhite, OutlineRadius = 5 };

            var hasShadowSwitch = new Switch();
            hasShadowSwitch.Toggled += (sender, e) => button.HasShadow = e.Value;

            var hasOutlineSwitch = new Switch();
            hasOutlineSwitch.Toggled += (sender, e) => button.OutlineWidth = e.Value ? 1 : 0;


            Padding = 20;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "HasShadow" },
                    hasShadowSwitch,
                    new Label { Text = "HasOutline"},
                    hasOutlineSwitch,

                    button,
                }
            };
        }

    }
}