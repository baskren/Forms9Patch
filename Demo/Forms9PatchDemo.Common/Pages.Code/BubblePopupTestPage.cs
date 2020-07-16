using System;

using Xamarin.Forms;
using P42.Utils;
using System.Linq;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class BubblePopupTestPage : ContentPage
    {
        #region Properties
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(BubblePopupTestPage), 4.0);
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create(nameof(PointerCornerRadius), typeof(double), typeof(BubblePopupTestPage), 4.0);
        public double PointerCornerRadius
        {
            get { return (double)GetValue(PointerCornerRadiusProperty); }
            set { SetValue(PointerCornerRadiusProperty, value); }
        }

        public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create(nameof(PointerLength), typeof(double), typeof(BubblePopupTestPage), 10.0);
        public double PointerLength
        {
            get { return (double)GetValue(PointerLengthProperty); }
            set { SetValue(PointerLengthProperty, value); }
        }

        public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create(nameof(PointerTipRadius), typeof(double), typeof(BubblePopupTestPage), 0.0);
        public double PointerTipRadius
        {
            get { return (double)GetValue(PointerTipRadiusProperty); }
            set { SetValue(PointerTipRadiusProperty, value); }
        }

        public static readonly BindableProperty PUDPaddingProperty = BindableProperty.Create(nameof(PUDPadding), typeof(double), typeof(BubblePopupTestPage), 10.0);
        public double PUDPadding
        {
            get { return (double)GetValue(PUDPaddingProperty); }
            set
            {
                SetValue(PUDPaddingProperty, value);
                PUPadding = new Thickness(value);
            }
        }

        public static readonly BindableProperty PUPaddingProperty = BindableProperty.Create(nameof(PUPadding), typeof(Thickness), typeof(BubblePopupTestPage), new Thickness((double)BubblePopupTestPage.PUDPaddingProperty.DefaultValue));
        public Thickness PUPadding
        {
            get { return (Thickness)GetValue(PUPaddingProperty); }
            set { SetValue(PUPaddingProperty, value); }
        }

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(double), typeof(BubblePopupTestPage), 0.5);
        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(BubblePopupTestPage), false);
        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }

        public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create(nameof(ShadowInverted), typeof(bool), typeof(BubblePopupTestPage), false);
        public bool ShadowInverted
        {
            get { return (bool)GetValue(ShadowInvertedProperty); }
            set { SetValue(ShadowInvertedProperty, value); }
        }

        public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create(nameof(PointerDirection), typeof(Forms9Patch.PointerDirection), typeof(BubblePopupTestPage), Forms9Patch.PointerDirection.Any);
        public Forms9Patch.PointerDirection PointerDirection
        {
            get { return (Forms9Patch.PointerDirection)GetValue(PointerDirectionProperty); }
            set
            {
                SetValue(PointerDirectionProperty, value);
            }
        }
        #endregion


        #region Fields
        VisualElement _lastChanged;
        #endregion


        #region Construction
        public BubblePopupTestPage()
        {

            BackgroundColor = Color.White;

            var shadowToggle = new Switch();
            shadowToggle.SetBinding(Switch.IsToggledProperty, "HasShadow");
            shadowToggle.BindingContext = this;

            var shadowInvertedToggle = new Switch();
            shadowInvertedToggle.SetBinding(Switch.IsToggledProperty, "ShadowInverted");
            shadowInvertedToggle.BindingContext = this;

            var cornerRadiusSlider = new Slider
            {
                Maximum = 40,
                Minimum = 0,
                HeightRequest = 20,
            };
            cornerRadiusSlider.SetBinding(Slider.ValueProperty, "CornerRadius");
            cornerRadiusSlider.BindingContext = this;

            var pointerLengthSlider = new Slider
            {
                Maximum = 40,
                Minimum = 0,
                HeightRequest = 20,
            };
            pointerLengthSlider.SetBinding(Slider.ValueProperty, "PointerLength");
            pointerLengthSlider.BindingContext = this;

            var pointerTipRadiusSlider = new Slider
            {
                Maximum = 20,
                Minimum = 0,
                HeightRequest = 20,
            };
            pointerTipRadiusSlider.SetBinding(Slider.ValueProperty, "PointerTipRadius");
            pointerTipRadiusSlider.BindingContext = this;

            var paddingSlider = new Slider
            {
                Maximum = 100,
                Minimum = 0,
                HeightRequest = 20,
            };
            paddingSlider.SetBinding(Slider.ValueProperty, "PUDPadding");
            paddingSlider.BindingContext = this;

            var pointerCornerRadiusSlider = new Slider
            {
                Maximum = 20,
                Minimum = 0,
                HeightRequest = 20,
            };
            pointerCornerRadiusSlider.SetBinding(Slider.ValueProperty, "PointerCornerRadius");
            pointerCornerRadiusSlider.BindingContext = this;

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
            upSeg.Tapped += (sender, e) =>
                _lastChanged = upSeg.VisualElement;
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

            PointerDirection = Forms9Patch.PointerDirection.Left;
            var directionSegmentControl = new Forms9Patch.SegmentedControl
            {
                Segments = { leftSeg, upSeg, rightSeg, downSeg, },
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
            };
            directionSegmentControl.SegmentTapped += (sender, e) =>
            {
                var dir = Forms9Patch.PointerDirection.None;
                dir |= (leftSeg.IsSelected ? Forms9Patch.PointerDirection.Left : Forms9Patch.PointerDirection.None);
                dir |= (rightSeg.IsSelected ? Forms9Patch.PointerDirection.Right : Forms9Patch.PointerDirection.None);
                dir |= (upSeg.IsSelected ? Forms9Patch.PointerDirection.Up : Forms9Patch.PointerDirection.None);
                dir |= (downSeg.IsSelected ? Forms9Patch.PointerDirection.Down : Forms9Patch.PointerDirection.None);
                PointerDirection = dir;
                System.Diagnostics.Debug.WriteLine("Direction changed");
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

            var addItemButton = new Forms9Patch.Button
            {
                Text = "Add Item",
                OutlineColor = Color.Black,
                OutlineWidth = 1,
                TextColor = Color.Red
            };

            var removeItemButton = new Forms9Patch.Button
            {
                Text = "Remove Item",
                OutlineColor = Color.Black,
                OutlineWidth = 1,
                TextColor = Color.Red
            };

            var enlargeItemsButton = new Forms9Patch.Button
            {
                Text = "Englarge Items",
                OutlineColor = Color.Black,
                OutlineWidth = 1,
                TextColor = Color.Black
            };

            var shrinkItems = new Forms9Patch.Button
            {
                Text = "Shrink Items",
                OutlineColor = Color.Black,
                OutlineWidth = 1,
                TextColor = Color.Black
            };

            var stackLayout = new Forms9Patch.StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
            };
            //stackLayout.Children.Add(new Label { Text = "X,", TextColor = Color.Green });

            var contentView = new Xamarin.Forms.ContentView
            {
                Content = stackLayout,
                HorizontalOptions = LayoutOptions.Start
            };

            var bubble = new Forms9Patch.BubblePopup(this)
            {
                //BackgroundColor = Color.Green,
                //OutlineColor = Color.Black,
                //OutlineWidth = 1,
                PointerCornerRadius = 0,
                Content = new StackLayout
                {
                    Children = {
                        bubbleLabel,
                        contentView,
                        new Label { Text = "Padding:", FontSize=10, TextColor=Color.Black },
                        paddingSlider,
                        new Label { Text = "Pointer Length:", FontSize=10, TextColor=Color.Black },
                        pointerLengthSlider,
                        new Label { Text = "Pointer Tip Radius:", FontSize=10, TextColor=Color.Black },
                        pointerTipRadiusSlider,
                        new Label { Text = "Corner Radius:" , FontSize=10, TextColor=Color.Black },
                        cornerRadiusSlider,
                        new Label { Text = "Pointer Corner Radius:" , FontSize=10, TextColor=Color.Black },
                        pointerCornerRadiusSlider,
                        bubbleButton,
                        addItemButton,
                        removeItemButton,
                        enlargeItemsButton,
                        shrinkItems
                    }
                },
            };
            bubble.SetBinding(Forms9Patch.BubblePopup.OutlineRadiusProperty, "CornerRadius");
            bubble.SetBinding(Forms9Patch.BubblePopup.PointerLengthProperty, "PointerLength");
            bubble.SetBinding(Forms9Patch.BubblePopup.PointerTipRadiusProperty, "PointerTipRadius");
            bubble.SetBinding(Forms9Patch.BubblePopup.PaddingProperty, "PUPadding");
            bubble.SetBinding(Forms9Patch.BubblePopup.HasShadowProperty, "HasShadow");
            bubble.SetBinding(Forms9Patch.BubblePopup.ShadowInvertedProperty, "ShadowInverted");
            bubble.SetBinding(Forms9Patch.BubblePopup.PointerDirectionProperty, "PointerDirection");
            bubble.SetBinding(Forms9Patch.BubblePopup.PointerCornerRadiusProperty, "PointerCornerRadius");
            bubble.BindingContext = this;

            //const bool Relayout = false;


            bubbleButton.Tapped += (sender, e) => bubble.IsVisible = false;
            addItemButton.Tapped += (sender, e) =>
            {
                //var count = stackLayout.Children.Count();
                //stackLayout.Children.Clear();
                //for (int i = 0; i <= count; i++)
                stackLayout.Children.Add(new Label { Text = "X,", TextColor = Color.Green });
                //bubble.WidthRequest = 1;
                stackLayout.WidthRequest = -1;
                var size = stackLayout.Measure(double.MaxValue, double.MinValue);
                System.Diagnostics.Debug.WriteLine("WIDTH=[" + stackLayout.Width + "] size.Request.Width=[" + size.Request.Width + "]");
                //stackLayout.WidthRequest = size.Request.Width + 1;
                //this.CallMethod("InvalidateMeasure", new object[] {});
                //contentView.WidthRequest = size.Request.Width;
                stackLayout.Children.Last().IsVisible = true;
            };

            enlargeItemsButton.Tapped += (sender, e) =>
            {
                var count = stackLayout.Children.Count();
                var width = 1;
                if (count > 0)
                    width = ((Label)stackLayout.Children[0]).Text.Length;
                string text = "";
                for (int i = 0; i < width; i++)
                    text += "X";
                text += ",";
                stackLayout.Children.Clear();
                for (int i = 0; i < count; i++)
                    stackLayout.Children.Add(new Label { Text = text, TextColor = Color.Green });
                //bubble.WidthRequest = 1;
                stackLayout.WidthRequest = -1;
                var size = stackLayout.Measure(double.MaxValue, double.MinValue);
                System.Diagnostics.Debug.WriteLine("WIDTH=[" + stackLayout.Width + "] size.Request.Width=[" + size.Request.Width + "]");
                //stackLayout.WidthRequest = size.Request.Width + 1;
                //this.CallMethod("InvalidateMeasure", new object[] {});
                //contentView.WidthRequest = size.Request.Width;
            };

            shrinkItems.Tapped += (sender, e) =>
            {
                var count = stackLayout.Children.Count();
                var width = 1;
                if (count > 0)
                    width = ((Label)stackLayout.Children[0]).Text.Length;
                string text = "";
                for (int i = 0; i < width - 2; i++)
                    text += "X";
                text += ",";
                System.Diagnostics.Debug.WriteLine("Text=[" + text + "]");
                stackLayout.Children.Clear();
                for (int i = 0; i < count; i++)
                    stackLayout.Children.Add(new Label { Text = text, TextColor = Color.Green });
                //bubble.WidthRequest = 1;
                stackLayout.WidthRequest = -1;
                var size = stackLayout.Measure(double.MaxValue, double.MinValue);
                System.Diagnostics.Debug.WriteLine("WIDTH=[" + stackLayout.Width + "] size.Request.Width=[" + size.Request.Width + "]");
            };

            removeItemButton.Tapped += (sender, e) =>
            {
                if (stackLayout.Children.Count > 0)
                    stackLayout.Children.Remove(stackLayout.Children.Last());
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
                Padding = 20,
                Children = {
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            new StackLayout {
                                Children = {
                                    new Label { Text = "Has Shadow", FontSize=10, TextColor=Color.Black },
                                    shadowToggle,
                                },
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                            },
                            new StackLayout {
                                Children = {
                                    new Label { Text = "Inset Shadow", FontSize=10, TextColor=Color.Black },
                                    shadowInvertedToggle,
                                },
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                            }
                        }
                    },
                    //new Label { Text = "Padding:", FontSize=10, TextColor=Color.Black },
                    //paddingSlider,
                    new Label { Text = "Pointer Direction", FontSize=10,  TextColor=Color.Black},
                    directionSegmentControl,
                    showButton,
                    new Label { Text = "Arrows choose the BubblePopup's allowed pointer direction.  Bubble's pointer will point at the last selected arrow-segment-button.", TextColor=Color.Black },
                }
                
            };
        }
        #endregion
    }
}


