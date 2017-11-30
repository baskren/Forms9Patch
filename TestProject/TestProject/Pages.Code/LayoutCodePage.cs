using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class LayoutCodePage : MasterDetailPage
    {
        #region Shapes
        static string Rectangle = "▭";
        static string Square = "□";
        static string Circle = "○";
        static string Ellipse = "⬭";
        //static string Obround = "◖▮◗";
        #endregion

        Slider capsInsetsLeftSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsRightSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsTopSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsBottomSlider = new Slider(0, 100, 0.0);

        Forms9Patch.SliderStepSizeEffect capsInsetsLStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsRStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsTStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsBStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);

        Switch pixelCapsSwitch = new Switch();

        Forms9Patch.AbsoluteLayout absoluteLayout = new Forms9Patch.AbsoluteLayout
        {
            BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redGridBox"),
        };
        Forms9Patch.Frame frame = new Forms9Patch.Frame
        {
            BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redGridBox"),
            Content = new Forms9Patch.Label("CONTENT") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment=TextAlignment.Center, FontAttributes = FontAttributes.Bold },
        };
        Forms9Patch.Grid grid = new Forms9Patch.Grid
        {
            BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redGridBox"),
            ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                },
            RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)},
                },
        };
        Forms9Patch.RelativeLayout relativeLayout = new Forms9Patch.RelativeLayout
        {
            BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redGridBox"),
        };
        Forms9Patch.StackLayout stackLayout = new Forms9Patch.StackLayout
        {
            BackgroundImage = new Forms9Patch.Image("Forms9PatchDemo.Resources.redGridBox"),
            Children =
                {
                    new Forms9Patch.Label("Child 1") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment=TextAlignment.Center, FontAttributes=FontAttributes.Bold },
                    new Forms9Patch.Label("Child 2") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment=TextAlignment.Center, FontAttributes=FontAttributes.Bold }
                }
        };

        Xamarin.Forms.Slider outlineWidthSlider = new Xamarin.Forms.Slider
        {
            Minimum = 0,
            Maximum = 15,
            Value = 2
        };


        public LayoutCodePage()
        {
            var hzOptionSegmentedControl = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL")
                },
            };
            hzOptionSegmentedControl.SelectIndex(3);
            hzOptionSegmentedControl.SegmentTapped += HzOptionSegmentedControl_SegmentTapped;

            var vtOptionSegmentedControl = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL")
                }
            };
            vtOptionSegmentedControl.SelectIndex(3);
            vtOptionSegmentedControl.SegmentTapped += VtOptionSegmentedControl_SegmentTapped;


            var heightRequestSlider = new Slider(-1, 300, -1);
            heightRequestSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            heightRequestSlider.ValueChanged += HeightRequestSlider_ValueChanged;

            var fillSegmentedControl = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    //new Forms9Patch.Segment("NONE"),
                    new Forms9Patch.Segment("TILE"),
                    new Forms9Patch.Segment("ASPECTFILL"),
                    new Forms9Patch.Segment("ASPECTFIT"),
                    new Forms9Patch.Segment("FILL")
                }
            };
            fillSegmentedControl.SelectIndex(3);
            fillSegmentedControl.SegmentTapped += FillSegmentedControl_SegmentTapped;

            Forms9Patch.MaterialSegmentedControl shapesSelector = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                new Forms9Patch.Segment { Text = Rectangle },
                new Forms9Patch.Segment { Text = Square },
                new Forms9Patch.Segment { Text = Circle },
                new Forms9Patch.Segment { Text = Ellipse },
                new Forms9Patch.Segment { Text = "OBROUND" }
                }
            };
            shapesSelector.SelectIndex(0);
            shapesSelector.SegmentTapped += ShapesSelector_SegmentTapped;

            outlineWidthSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1.0 / Forms9Patch.Display.Scale));
            outlineWidthSlider.ValueChanged += OutlineWidthSlider_ValueChanged;

            var outlineRadiusSlider = new Xamarin.Forms.Slider
            {
                Minimum = 0,
                Maximum = 15,
                Value = 0
            };
            outlineRadiusSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            outlineRadiusSlider.ValueChanged += OutlineRadiusSlider_ValueChanged;

            var shapeAttributesSelector = new Forms9Patch.MaterialSegmentedControl
            {
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
                Segments =
                {
                    new Forms9Patch.Segment("BACKGROUND"),
                    new Forms9Patch.Segment("OUTLINE"),
                    new Forms9Patch.Segment("SHADOW"),
                    new Forms9Patch.Segment("INVERTED")
                }
            };
            shapeAttributesSelector.SegmentTapped += ShapeAttributesSelector_SegmentTapped;

            var backgroundImageSelector = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("NONE"),
                    new Forms9Patch.Segment("redGridBox"),
                    new Forms9Patch.Segment("button"),
                    new Forms9Patch.Segment("cat"),
                    new Forms9Patch.Segment("balloons"),
                    new Forms9Patch.Segment("image"),
                    new Forms9Patch.Segment("redribbon"),
                    new Forms9Patch.Segment("bubble"),
                    new Forms9Patch.Segment("bluebutton"),
                }
            };
            backgroundImageSelector.SelectIndex(1);
            backgroundImageSelector.SegmentTapped += BackgroundImageSelector_SegmentTapped;

            capsInsetsTopSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsTopSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsLeftSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsLeftSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsRightSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsRightSlider.Rotation = 180;
            capsInsetsRightSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsBottomSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsBottomSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsBottomSlider.Rotation = 180;

            var outlineGrid = new Xamarin.Forms.Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                },
            };
            outlineGrid.Children.Add(new Forms9Patch.Label("Forms9Patch.Image.OutlineWidth:"), 0, 0);
            outlineGrid.Children.Add(outlineWidthSlider, 0, 1);
            outlineGrid.Children.Add(new Forms9Patch.Label("Forms9Patch.Image.OutlineRadius:"), 1, 0);
            outlineGrid.Children.Add(outlineRadiusSlider, 1, 1);

            pixelCapsSwitch.Toggled += PixelCapsSwitch_Toggled;

            var capsInsetsGrid = new Xamarin.Forms.Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                },
            };
            capsInsetsGrid.Children.Add(new Forms9Patch.Label("CapsInsets.Top:"), 0, 0);
            capsInsetsGrid.Children.Add(capsInsetsTopSlider, 0, 1);
            capsInsetsGrid.Children.Add(new Forms9Patch.Label("CapsInsets.Bottom:"), 0, 2);
            capsInsetsGrid.Children.Add(capsInsetsBottomSlider, 0, 3);
            capsInsetsGrid.Children.Add(new Forms9Patch.Label("CapsInsets.Left:"), 1, 0);
            capsInsetsGrid.Children.Add(capsInsetsLeftSlider, 1, 1);
            capsInsetsGrid.Children.Add(new Forms9Patch.Label("CapsInsets.Right:"), 1, 2);
            capsInsetsGrid.Children.Add(capsInsetsRightSlider, 1, 3);

            var absoluteLayoutContent = new Forms9Patch.Label("CONTENT")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center ,
                FontAttributes = FontAttributes.Bold
            };
            AbsoluteLayout.SetLayoutBounds(absoluteLayoutContent, new Rectangle(0.5, 0.5, 1.0, 1.0));
            AbsoluteLayout.SetLayoutFlags(absoluteLayoutContent, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(absoluteLayoutContent);

            grid.Children.Add(new Forms9Patch.Label("C0R0") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 0, 0);
            grid.Children.Add(new Forms9Patch.Label("C0R1") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 0, 1);
            grid.Children.Add(new Forms9Patch.Label("C1R0") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 1, 0);
            grid.Children.Add(new Forms9Patch.Label("C1R1") { TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 1, 1);

            var cView = new Forms9Patch.Label("C")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var lView = new Forms9Patch.Label("L")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var rView = new Forms9Patch.Label("R")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var tView = new Forms9Patch.Label("T")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var bView = new Forms9Patch.Label("T")
            {
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };

            relativeLayout.Children.Add(cView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToParent((parent) => parent.Height / 2));
            relativeLayout.Children.Add(tView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToView(cView, (parent, sibling) => sibling.Y - parent.Height / 4));
            relativeLayout.Children.Add(bView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToView(cView, (parent, sibling) => sibling.Y + parent.Height / 4));
            relativeLayout.Children.Add(lView, Constraint.RelativeToView(cView, (parent, sibling) => sibling.X - parent.Width / 4), Constraint.RelativeToParent((parent) => parent.Height / 2));
            relativeLayout.Children.Add(rView, Constraint.RelativeToView(cView, (parent, sibling) => sibling.X + parent.Width / 4), Constraint.RelativeToParent((parent) => parent.Height / 2));


            var layoutTypeSegmentedController = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("ABSOLUTE"),
                    new Forms9Patch.Segment("FRAME"),
                    new Forms9Patch.Segment("GRID"),
                    new Forms9Patch.Segment("RELATIVE"),
                    new Forms9Patch.Segment("STACK")
                }
            };
            layoutTypeSegmentedController.SegmentTapped += (sender, e) =>
            {
                Layout layout = null;
                switch(e.Segment.Text)
                {
                    case "ABSOLUTE": layout = absoluteLayout; break;
                    case "FRAME": layout = frame; break;
                    case "GRID": layout = grid; break;
                    case "RELATIVE": layout = relativeLayout; break;
                    case "STACK": layout = stackLayout; break;
                }
                ((ContentPage)Detail).Content = layout;
                ((ContentPage)Detail).Title = e.Segment.Text;
            };

            //Padding = new Thickness(20, 20, 0, 20);
            Master = new ContentPage
            {
                Title = "Layout Settings",
                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 20,
                        Children =
                        {
                            new Forms9Patch.Label("Layout type:"),
                            layoutTypeSegmentedController,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Xamarin.Forms.VisualElement.HorizontalOptions (Alignment):"),
                            hzOptionSegmentedControl,
                            new Forms9Patch.Label("Xamarin.Forms.VisualElement.VerticalOptions (Alignment):"),
                            vtOptionSegmentedControl,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Forms9Patch.Image.Fill:"),
                            fillSegmentedControl,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Forms9Patch.Image.ElementShape:"),
                            shapesSelector,
                            new Forms9Patch.Label("Shape attributes (some value vs. default value)"),
                            shapeAttributesSelector,
                            outlineGrid,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Forms9Patch.Image.Source:"),
                            backgroundImageSelector,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("CapsInsets Pixels (ON) / Percent (OFF):"),
                            pixelCapsSwitch,
                            capsInsetsGrid,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Xamarin.Forms.VisualElement.HeightRequest:"),
                            heightRequestSlider,
                        },
                    },
                }
            };
            Detail = new ContentPage
            {
                Title = "ABSOLUTE",
                Content = absoluteLayout
            };


        }

        private void PixelCapsSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                capsInsetsLeftSlider.Maximum = 200;
                capsInsetsLeftSlider.Value = 0;
                capsInsetsRightSlider.Maximum = 200;
                capsInsetsRightSlider.Value = 0;
                capsInsetsTopSlider.Maximum = 200;
                capsInsetsTopSlider.Value = 0;
                capsInsetsBottomSlider.Maximum = 200;
                capsInsetsBottomSlider.Value = 0;
            }
            else
            {
                capsInsetsLeftSlider.Maximum = 100;
                capsInsetsLeftSlider.Value = 0;
                capsInsetsRightSlider.Maximum = 100;
                capsInsetsRightSlider.Value = 0;
                capsInsetsTopSlider.Maximum = 100;
                capsInsetsTopSlider.Value = 0;
                capsInsetsBottomSlider.Maximum = 100;
                capsInsetsBottomSlider.Value = 0;
            }
        }

        private void HzOptionSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            var layoutOption = LayoutOptions.Fill;
            if (e.Segment.Text == "START")
                layoutOption = LayoutOptions.Start;
            else if (e.Segment.Text == "CENTER")
                layoutOption = LayoutOptions.Center;
            else if (e.Segment.Text == "END")
                layoutOption = LayoutOptions.End;
            absoluteLayout.HorizontalOptions = layoutOption;
            frame.HorizontalOptions = layoutOption;
            grid.HorizontalOptions = layoutOption;
            relativeLayout.HorizontalOptions = layoutOption;
            stackLayout.HorizontalOptions = layoutOption;
        }

        private void VtOptionSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            var layoutOption = LayoutOptions.Fill;
            if (e.Segment.Text == "START")
                layoutOption = LayoutOptions.Start;
            else if (e.Segment.Text == "CENTER")
                layoutOption = LayoutOptions.Center;
            else if (e.Segment.Text == "END")
                layoutOption = LayoutOptions.End;
            absoluteLayout.VerticalOptions = layoutOption;
            frame.VerticalOptions = layoutOption;
            grid.VerticalOptions = layoutOption;
            relativeLayout.VerticalOptions = layoutOption;
            stackLayout.VerticalOptions = layoutOption;
        }

        private void HeightRequestSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            absoluteLayout.HeightRequest = e.NewValue;
            frame.HeightRequest = e.NewValue;
            grid.HeightRequest = e.NewValue;
            relativeLayout.HeightRequest = e.NewValue;
            stackLayout.HeightRequest = e.NewValue;
        }

        private void FillSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            var fill = Forms9Patch.Fill.Fill;
            if (e.Segment.Text == "TILE")
                fill = Forms9Patch.Fill.Tile;
            else if (e.Segment.Text == "ASPECTFILL")
                fill = Forms9Patch.Fill.AspectFill;
            else if (e.Segment.Text == "ASPECTFIT")
                fill = Forms9Patch.Fill.AspectFit;
            absoluteLayout.BackgroundImage.Fill = fill;
            frame.BackgroundImage.Fill = fill;
            grid.BackgroundImage.Fill = fill;
            relativeLayout.BackgroundImage.Fill = fill;
            stackLayout.BackgroundImage.Fill = fill;
        }

        private void ShapesSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            var shape = Forms9Patch.ElementShape.Rectangle;
            if (e.Segment.Text == Square)
                shape = Forms9Patch.ElementShape.Square;
            else if (e.Segment.Text == Circle)
                shape = Forms9Patch.ElementShape.Circle;
            else if (e.Segment.Text == Ellipse)
                shape = Forms9Patch.ElementShape.Elliptical;
            else if (e.Segment.Text == "OBROUND")
                shape = Forms9Patch.ElementShape.Obround;
            
            absoluteLayout.ElementShape = shape;
            frame.ElementShape = shape;
            grid.ElementShape = shape;
            relativeLayout.ElementShape = shape;
            stackLayout.ElementShape = shape;
        }

        private void OutlineWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            absoluteLayout.OutlineWidth = (float)e.NewValue;
            frame.OutlineWidth = (float)e.NewValue;
            grid.OutlineWidth = (float)e.NewValue;
            relativeLayout.OutlineWidth = (float)e.NewValue;
            stackLayout.OutlineWidth = (float)e.NewValue;
        }

        private void OutlineRadiusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            absoluteLayout.OutlineRadius = (float)e.NewValue;
            frame.OutlineRadius = (float)e.NewValue;
            grid.OutlineRadius = (float)e.NewValue;
            relativeLayout.OutlineRadius = (float)e.NewValue;
            stackLayout.OutlineRadius = (float)e.NewValue;
        }

        private void ShapeAttributesSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            switch (e.Segment.Text)
            {
                case "BACKGROUND":
                    var backgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    absoluteLayout.BackgroundColor = backgroundColor;
                    frame.BackgroundColor = backgroundColor;
                    grid.BackgroundColor = backgroundColor;
                    relativeLayout.BackgroundColor = backgroundColor;
                    stackLayout.BackgroundColor = backgroundColor;
                    break;
                case "OUTLINE":
                    var outlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    absoluteLayout.OutlineColor = outlineColor;
                    frame.OutlineColor = outlineColor;
                    grid.OutlineColor = outlineColor;
                    relativeLayout.OutlineColor = outlineColor;
                    stackLayout.OutlineColor = outlineColor;
                    var outlineWidth = e.Segment.IsSelected ? (float)Math.Max(outlineWidthSlider.Value, 2) : 0;
                    absoluteLayout.OutlineWidth = outlineWidth;
                    frame.OutlineWidth = outlineWidth;
                    grid.OutlineWidth = outlineWidth;
                    relativeLayout.OutlineWidth = outlineWidth;
                    stackLayout.OutlineWidth = outlineWidth;
                    break;
                case "SHADOW":
                    absoluteLayout.HasShadow = e.Segment.IsSelected;
                    frame.HasShadow = e.Segment.IsSelected;
                    grid.HasShadow = e.Segment.IsSelected;
                    relativeLayout.HasShadow = e.Segment.IsSelected;
                    stackLayout.HasShadow = e.Segment.IsSelected;
                    break;
                case "INVERTED":
                    absoluteLayout.ShadowInverted = e.Segment.IsSelected;
                    frame.ShadowInverted = e.Segment.IsSelected;
                    grid.ShadowInverted = e.Segment.IsSelected;
                    relativeLayout.ShadowInverted = e.Segment.IsSelected;
                    stackLayout.ShadowInverted = e.Segment.IsSelected;
                    break;
            }
        }

        private void BackgroundImageSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            if (e.Segment.Text=="NONE")
            {
                absoluteLayout.BackgroundImage = null;
                frame.BackgroundImage = null;
                grid.BackgroundImage = null;
                relativeLayout.BackgroundImage = null;
                stackLayout.BackgroundImage = null;
                return;
            }
            string embeddedResourceId = "Forms9PatchDemo.Resources." + e.Segment.Text;

            absoluteLayout.BackgroundImage = new Forms9Patch.Image(embeddedResourceId);
            frame.BackgroundImage = new Forms9Patch.Image(embeddedResourceId);
            grid.BackgroundImage = new Forms9Patch.Image(embeddedResourceId);
            relativeLayout.BackgroundImage = new Forms9Patch.Image(embeddedResourceId);
            stackLayout.BackgroundImage = new Forms9Patch.Image(embeddedResourceId);
    }

        private void CapsInsetsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double scale = pixelCapsSwitch.IsToggled ? 1 : 0.01;
            var capsInset = new Thickness(capsInsetsLeftSlider.Value * scale, capsInsetsTopSlider.Value * scale, capsInsetsRightSlider.Value * scale, capsInsetsBottomSlider.Value * scale);
            System.Diagnostics.Debug.WriteLine("CapsInset=[" + Forms9Patch.ThicknessExtension.Description(capsInset) + "]");
            if (absoluteLayout?.BackgroundImage?.Source != null)
            {
                absoluteLayout.BackgroundImage.CapInsets = capsInset;
                frame.BackgroundImage.CapInsets = capsInset;
                grid.BackgroundImage.CapInsets = capsInset;
                relativeLayout.BackgroundImage.CapInsets = capsInset;
                stackLayout.BackgroundImage.CapInsets = capsInset;
            }
        }
    }


}


