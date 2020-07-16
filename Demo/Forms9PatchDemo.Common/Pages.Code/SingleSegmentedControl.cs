using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class SingleSegmentedControl : ContentPage
    {
        public SingleSegmentedControl()
        {
            var segementControl = new Forms9Patch.SegmentedControl
            {
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
                BackgroundColor = Color.NavajoWhite,
                OutlineRadius = 5,
                OutlineWidth = 0,
                Segments =
                {
                    new Forms9Patch.Segment { Text = "Start"},
                    new Forms9Patch.Segment { Text = "Mid"},
                    new Forms9Patch.Segment { Text = "End"}
                }
            };

            var hasShadowSwitch = new Switch();
            hasShadowSwitch.Toggled += (sender, e) => segementControl.HasShadow = e.Value;

            var hasOutlineSwitch = new Switch();
            hasOutlineSwitch.Toggled += (sender, e) => segementControl.OutlineWidth = e.Value ? 1 : 0;

            var hasSeparatorsSwitch = new Switch();
            hasSeparatorsSwitch.Toggled += (sender, e) => segementControl.SeparatorWidth = e.Value ? 1 : 0;


            Padding = 20;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "HasShadow" },
                    hasShadowSwitch,
                    new Label { Text = "HasOutline"},
                    hasOutlineSwitch,
                    new Forms9Patch.Label("HasSeparators"),
                    hasSeparatorsSwitch,
                    segementControl,
                }
            };

            hasSeparatorsSwitch.IsToggled = Math.Abs(segementControl.SeparatorWidth) > 0.01;
            hasShadowSwitch.IsToggled = segementControl.HasShadow;
            hasOutlineSwitch.IsToggled = segementControl.OutlineWidth > 0;
        }
    }
}