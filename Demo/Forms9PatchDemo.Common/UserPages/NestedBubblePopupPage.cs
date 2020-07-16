using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class NestedBubblePopupPage : ContentPage
    {

        VisualElement _lastChanged;

        public NestedBubblePopupPage()
        {
            BackgroundColor = Color.White;
            //Padding = new Thickness(20, Device.RuntimePlatform == Device.iOS ? 20 : 0, 20, 20);

            var shadowToggle = new Switch();

            var shadowInvertedToggle = new Switch();

            var cornerRadiusSlider = new Slider
            {
                Maximum = 40,
                Minimum = 0,
                HeightRequest = 20,
            };

            var pointerLengthSlider = new Slider
            {
                Maximum = 40,
                Minimum = 0,
                HeightRequest = 20,
            };

            var pointerTipRadiusSlider = new Slider
            {
                Maximum = 20,
                Minimum = 0,
                HeightRequest = 20,
            };

            var paddingSlider = new Slider
            {
                Maximum = 100,
                Minimum = 0,
                HeightRequest = 20,
            };

            var pointerCornerRadiusSlider = new Slider
            {
                Maximum = 20,
                Minimum = 0,
                HeightRequest = 20,
            };

            var leftSeg = new Forms9Patch.Segment
            {
                Text = "⬅︎",
                IsSelected = true,
            };
            leftSeg.Tapped += (sender, e) => _lastChanged = leftSeg.VisualElement;
            var upSeg = new Forms9Patch.Segment
            {
                Text = "⬆︎",
            };
            upSeg.Tapped += (sender, e) => _lastChanged = upSeg.VisualElement;
            var rightSeg = new Forms9Patch.Segment
            {
                Text = "➡︎",
            };
            rightSeg.Tapped += (sender, e) => _lastChanged = rightSeg.VisualElement;
            var downSeg = new Forms9Patch.Segment
            {
                Text = "⬇︎",
            };
            downSeg.Tapped += (sender, e) => _lastChanged = downSeg.VisualElement;

            var directionSegmentControl = new Forms9Patch.SegmentedControl
            {
                Segments = { leftSeg, upSeg, rightSeg, downSeg, },
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
            };

            var bubbleLabel = new Label
            {
                Text = "Forms9Patch.BubblePopup",
                TextColor = Color.Black,
                //BackgroundColor = Color.Green,
            };
            var bubbleButton = new Forms9Patch.Button
            {
                Text = "Close",
                //BackgroundColor = Color.Blue,
                OutlineColor = Color.Blue,
                TextColor = Color.Blue,
            };
            //bubbleLabel.SetBinding (Label.TextProperty, "CornerRadius");
            bubbleLabel.BindingContext = this;

            var childBubbleButton = new Forms9Patch.Button("Show Child");

            var bubble = new Forms9Patch.BubblePopup(this)
            {
                //BackgroundColor = Color.Green,
                //OutlineColor = Color.Black,
                //OutlineWidth = 1,
                Padding = 0,
                PointerCornerRadius = 0,
                Content = new StackLayout
                {
                    Padding = 5,
                    Children = {
						//bubbleLabel,
                        childBubbleButton,
                        bubbleButton,
                    }
                },
            };

            cornerRadiusSlider.ValueChanged += (s, e) => bubble.OutlineRadius = (float)cornerRadiusSlider.Value;
            pointerLengthSlider.ValueChanged += (s, e) => bubble.PointerLength = (float)pointerLengthSlider.Value;
            pointerTipRadiusSlider.ValueChanged += (s, e) => bubble.PointerTipRadius = (float)pointerTipRadiusSlider.Value;
            shadowToggle.Toggled += (s, e) => bubble.HasShadow = shadowToggle.IsToggled;
            shadowInvertedToggle.Toggled += (s, e) => bubble.ShadowInverted = shadowInvertedToggle.IsToggled;
            pointerCornerRadiusSlider.ValueChanged += (s, e) => bubble.PointerCornerRadius = (float)pointerCornerRadiusSlider.Value;
            paddingSlider.ValueChanged += (s, e) => bubble.Padding = paddingSlider.Value;
            directionSegmentControl.SegmentTapped += (sender, e) =>
            {
                var dir = Forms9Patch.PointerDirection.None;
                dir |= (leftSeg.IsSelected ? Forms9Patch.PointerDirection.Left : Forms9Patch.PointerDirection.None);
                dir |= (rightSeg.IsSelected ? Forms9Patch.PointerDirection.Right : Forms9Patch.PointerDirection.None);
                dir |= (upSeg.IsSelected ? Forms9Patch.PointerDirection.Up : Forms9Patch.PointerDirection.None);
                dir |= (downSeg.IsSelected ? Forms9Patch.PointerDirection.Down : Forms9Patch.PointerDirection.None);
                bubble.PointerDirection = dir;
                System.Diagnostics.Debug.WriteLine("Direction changed");
            };


            bubbleButton.Tapped += (sender, e) => bubble.IsVisible = false;
            childBubbleButton.Clicked += (sender, e) =>
            {
                var newBubbleCancelButton = new Forms9Patch.Button
                {
                    Text = "Close",
                    OutlineColor = Color.Blue,
                    TextColor = Color.Blue,
                };

                var newbubble = new Forms9Patch.BubblePopup(bubble)
                {
                    //BackgroundColor = Color.Green,
                    //OutlineColor = Color.Black,
                    //OutlineWidth = 1,
                    //Target = childBubbleButton,
                    //PointerCornerRadius = 0,
                    PointerDirection = Forms9Patch.PointerDirection.Any,
                    Content = new StackLayout
                    {
                        Children = {
							//bubbleLabel,
							new Label { Text = "Pointer Length:", FontSize=10, },
                            pointerLengthSlider,
                            new Label { Text = "Pointer Tip Radius:", FontSize=10, },
                            pointerTipRadiusSlider,
                            new Label { Text = "Corner Radius:" , FontSize=10, },
                            cornerRadiusSlider,
                            new Label { Text = "Pointer Corner Radius:" , FontSize=10, },
                            pointerCornerRadiusSlider,
                            newBubbleCancelButton,
                        }
                    },
                };

                newbubble.IsVisible = true;
                newBubbleCancelButton.Clicked += async (s1, e1) =>
                {
                    await newbubble.CancelAsync();
                };
            };

            var showButton = new Forms9Patch.Button
            {
                Text = "Show BubblePopup",
                OutlineColor = Color.Blue,
                TextColor = Color.Blue,
            };
            showButton.Tapped += (object sender, EventArgs e) =>
            {
                bubble.Target = _lastChanged;
                bubble.IsVisible = true;
            };

            _lastChanged = leftSeg.VisualElement;

            Content = new StackLayout
            {
                Children = {
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            new StackLayout {
                                Children = {
                                    new Label { Text = "Has Shadow", FontSize=10, },
                                    shadowToggle,
                                },
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                            },
                            new StackLayout {
                                Children = {
                                    new Label { Text = "Inset Shadow", FontSize=10, },
                                    shadowInvertedToggle,
                                },
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                            }
                        }
                    },
                    new Label { Text = "Padding:", FontSize=10, },
                    paddingSlider,
                    new Label { Text = "Pointer Direction", FontSize=10, },
                    directionSegmentControl,
                    showButton,
                    new Label { Text = "Arrows choose the BubblePopup's allowed pointer direction.  Bubble's pointer will point at the last selected arrow-segment-button." },
                }
            };
        }
    }
}


