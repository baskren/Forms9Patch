﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Forms9PatchDemo.Pages
{
    public class SingleSegmentedController : ContentPage
    {
        public SingleSegmentedController()
        {
            var segementControl = new Forms9Patch.MaterialSegmentedControl
            {
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
                BackgroundColor = Color.NavajoWhite,
                OutlineRadius = 5,

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


            Padding = 20;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "HasShadow" },
                    hasShadowSwitch,
                    new Label { Text = "HasOutline"},
                    hasOutlineSwitch,

                    segementControl,
                }
            };
        }
    }
}