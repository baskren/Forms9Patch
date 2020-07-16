using System;

using Xamarin.Forms;
using System.Linq;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
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

        //Switch pixelCapsSwitch = new Switch();

        /*
        */


        View Content => ((ContentPage)Detail).Content;

        Forms9Patch.SegmentedControl hzOptionSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL")
                },
        };
        Forms9Patch.SegmentedControl vtOptionSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL")
                }
        };
        Slider heightRequestSlider = new Slider(-1, 300, -1);
        Forms9Patch.SegmentedControl fillSegmentedControl = new Forms9Patch.SegmentedControl
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

        Xamarin.Forms.Slider outlineWidthSlider = new Xamarin.Forms.Slider
        {
            Minimum = 0,
            Maximum = 15,
            Value = 2
        };
        Forms9Patch.SegmentedControl shapesSelector = new Forms9Patch.SegmentedControl
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
        Forms9Patch.SegmentedControl shapeAttributesSelector = new Forms9Patch.SegmentedControl
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
        Forms9Patch.SegmentedControl backgroundImageSelector = new Forms9Patch.SegmentedControl
        {
            HeightRequest = 40,
            HasTightSpacing = true,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            TintIcon = false,
            Segments =
                {
                    new Forms9Patch.Segment(""),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.redGridBox"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.button"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.image"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.redribbon"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.bubble"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.bluebutton"),
                    new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.SvgSample2.svg"),
                }
        };


        Slider outlineRadiusSlider = new Xamarin.Forms.Slider
        {
            Minimum = 0,
            Maximum = 15,
            Value = 0
        };

        Forms9Patch.SegmentedControl capsUnitsSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment("PIXL"),
                new Forms9Patch.Segment("%")
            },
        };

        Switch antiAliasSwitch = new Switch();


        public LayoutCodePage()
        {
            hzOptionSegmentedControl.SelectIndex(3);
            hzOptionSegmentedControl.SegmentTapped += SetHzLayoutOptions;

            vtOptionSegmentedControl.SelectIndex(3);
            vtOptionSegmentedControl.SegmentTapped += SetVtLayoutOptions;

            heightRequestSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            heightRequestSlider.ValueChanged += SetHeightRequest;

            fillSegmentedControl.SelectIndex(3);
            fillSegmentedControl.SegmentTapped += SetFill;

            shapesSelector.SelectIndex(0);
            shapesSelector.SegmentTapped += SetShape;

            outlineWidthSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1.0 / Forms9Patch.Display.Scale));
            outlineWidthSlider.ValueChanged += SetOutlineWidth;

            outlineRadiusSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            outlineRadiusSlider.ValueChanged += SetOutlineRadius;

            shapeAttributesSelector.SegmentTapped += SetShapeAttributes;

            backgroundImageSelector.SelectIndex(1);
            backgroundImageSelector.SegmentTapped += SetBackgroundImage;

            capsInsetsTopSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsTopSlider.ValueChanged += SetCapsInsets;
            capsInsetsLeftSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsLeftSlider.ValueChanged += SetCapsInsets;
            capsInsetsRightSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsRightSlider.Rotation = 180;
            capsInsetsRightSlider.ValueChanged += SetCapsInsets;
            capsInsetsBottomSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsBottomSlider.ValueChanged += SetCapsInsets;
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
            outlineGrid.Children.Add(new Forms9Patch.Label("OutlineWidth:"), 0, 0);
            outlineGrid.Children.Add(outlineWidthSlider, 0, 1);
            outlineGrid.Children.Add(new Forms9Patch.Label("OutlineRadius:"), 1, 0);
            outlineGrid.Children.Add(outlineRadiusSlider, 1, 1);

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

            capsUnitsSegmentedControl.SelectIndex(0);
            capsUnitsSegmentedControl.SegmentTapped += SetCapsUnits;
            antiAliasSwitch.Toggled += SetAntiAlias;
            var capsInsetsAndAntiAliasGrid = new Grid
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
            capsInsetsAndAntiAliasGrid.Children.Add(new Forms9Patch.Label("CapsInsets Units:"), 0, 0);
            capsInsetsAndAntiAliasGrid.Children.Add(capsUnitsSegmentedControl, 0, 1);
            capsInsetsAndAntiAliasGrid.Children.Add(new Forms9Patch.Label("AntiAlias:"), 1, 0);
            capsInsetsAndAntiAliasGrid.Children.Add(antiAliasSwitch, 1, 1);



            var layoutTypeSegmentedController = new Forms9Patch.SegmentedControl
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
                switch (e.Segment.Text)
                {
                    case "ABSOLUTE": layout = GenerateAbsoluteLayout(); break;
                    case "FRAME": layout = GenerateFrame(); break;
                    case "GRID": layout = GenerateGrid(); break;
                    case "RELATIVE": layout = GenerateRelativeLayout(); break;
                    case "STACK": layout = GenerateStackLayout(); break;
                }
                ((ContentPage)Detail).Content = layout;
                SetLayoutProperties();
                ((ContentPage)Detail).Title = e.Segment.Text;
            };

            //Padding = new Thickness(20, 20, 0, 20);
            Master = new ContentPage
            {
                Title = "Layout Settings",
                BackgroundColor = Color.LightGray,
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

                            capsInsetsAndAntiAliasGrid,
                            capsInsetsGrid,

                            new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                            new Forms9Patch.Label("Xamarin.Forms.VisualElement.HeightRequest:"),
                            heightRequestSlider,
                            new Xamarin.Forms.Label { Text="Display.Scale=["+Forms9Patch.Display.Scale+"]" }
                        },
                    },
                }
            };
            Detail = new ContentPage
            {
                Title = "ABSOLUTE",
                Content = GenerateAbsoluteLayout()
            };
            SetLayoutProperties();

            IsPresented = true;
            MasterBehavior = MasterBehavior.Default;
        }

        void SetAntiAlias(object sender = null, ToggledEventArgs e = null)
        {
            if (((Forms9Patch.ILayout)Content).BackgroundImage != null)
                ((Forms9Patch.ILayout)Content).BackgroundImage.AntiAlias = antiAliasSwitch.IsToggled;
        }


        void SetCapsUnits(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            if (capsUnitsSegmentedControl.IsIndexSelected(0))
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
            SetCapsInsets();
        }

        void SetHzLayoutOptions(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            var text = hzOptionSegmentedControl.SelectedSegments.First().Text;
            var layoutOption = LayoutOptions.Fill;
            if (text == "START")
                layoutOption = LayoutOptions.Start;
            else if (text == "CENTER")
                layoutOption = LayoutOptions.Center;
            else if (text == "END")
                layoutOption = LayoutOptions.End;
            Content.HorizontalOptions = layoutOption;
            /*
            absoluteLayout.HorizontalOptions = layoutOption;
            frame.HorizontalOptions = layoutOption;
            grid.HorizontalOptions = layoutOption;
            relativeLayout.HorizontalOptions = layoutOption;
            stackLayout.HorizontalOptions = layoutOption;
            */
        }

        void SetVtLayoutOptions(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            var text = vtOptionSegmentedControl.SelectedSegments.First().Text;
            var layoutOption = LayoutOptions.Fill;
            if (text == "START")
                layoutOption = LayoutOptions.Start;
            else if (text == "CENTER")
                layoutOption = LayoutOptions.Center;
            else if (text == "END")
                layoutOption = LayoutOptions.End;
            Content.VerticalOptions = layoutOption;
            /*
            absoluteLayout.VerticalOptions = layoutOption;
            frame.VerticalOptions = layoutOption;
            grid.VerticalOptions = layoutOption;
            relativeLayout.VerticalOptions = layoutOption;
            stackLayout.VerticalOptions = layoutOption;
            */
        }

        void SetHeightRequest(object sender = null, ValueChangedEventArgs args = null)
        {
            Content.HeightRequest = heightRequestSlider.Value;
            /*
            absoluteLayout.HeightRequest = e.NewValue;
            frame.HeightRequest = e.NewValue;
            grid.HeightRequest = e.NewValue;
            relativeLayout.HeightRequest = e.NewValue;
            stackLayout.HeightRequest = e.NewValue;
            */
        }

        void SetFill(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            if (((Forms9Patch.ILayout)Content).BackgroundImage != null)
            {
                var text = fillSegmentedControl.SelectedSegments.First().Text;

                var fill = Forms9Patch.Fill.Fill;
                if (text == "TILE")
                    fill = Forms9Patch.Fill.Tile;
                else if (text == "ASPECTFILL")
                    fill = Forms9Patch.Fill.AspectFill;
                else if (text == "ASPECTFIT")
                    fill = Forms9Patch.Fill.AspectFit;
                ((Forms9Patch.ILayout)Content).BackgroundImage.Fill = fill;
                /*
                absoluteLayout.BackgroundImage.Fill = fill;
                frame.BackgroundImage.Fill = fill;
                grid.BackgroundImage.Fill = fill;
                relativeLayout.BackgroundImage.Fill = fill;
                stackLayout.BackgroundImage.Fill = fill;*/
            }
        }

        void SetShape(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            var text = shapesSelector.SelectedSegments.First().Text;
            var shape = Forms9Patch.ElementShape.Rectangle;
            if (text == Square)
                shape = Forms9Patch.ElementShape.Square;
            else if (text == Circle)
                shape = Forms9Patch.ElementShape.Circle;
            else if (text == Ellipse)
                shape = Forms9Patch.ElementShape.Elliptical;
            else if (text == "OBROUND")
                shape = Forms9Patch.ElementShape.Obround;
            ((Forms9Patch.ILayout)Content).ElementShape = shape;
            /*
            absoluteLayout.ElementShape = shape;
            frame.ElementShape = shape;
            grid.ElementShape = shape;
            relativeLayout.ElementShape = shape;
            stackLayout.ElementShape = shape;
            */
        }

        void SetOutlineWidth(object sender = null, ValueChangedEventArgs agrs = null)
        {
            ((Forms9Patch.ILayout)Content).OutlineWidth = (float)outlineWidthSlider.Value;
            /*
            absoluteLayout.OutlineWidth = (float)e.NewValue;
            frame.OutlineWidth = (float)e.NewValue;
            grid.OutlineWidth = (float)e.NewValue;
            relativeLayout.OutlineWidth = (float)e.NewValue;
            stackLayout.OutlineWidth = (float)e.NewValue;
            */
        }

        void SetOutlineRadius(object sender = null, ValueChangedEventArgs args = null)
        {
            ((Forms9Patch.ILayout)Content).OutlineRadius = (float)outlineRadiusSlider.Value;
            /*
            absoluteLayout.OutlineRadius = (float)e.NewValue;
            frame.OutlineRadius = (float)e.NewValue;
            grid.OutlineRadius = (float)e.NewValue;
            relativeLayout.OutlineRadius = (float)e.NewValue;
            stackLayout.OutlineRadius = (float)e.NewValue;
            */
        }

        void SetShapeAttributes(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            foreach (var segment in shapeAttributesSelector.Segments)
                switch (segment.Text)
                {
                    case "BACKGROUND":
                        var backgroundColor = segment.IsSelected ? Color.Green : Color.Default;
                        ((Forms9Patch.ILayout)Content).BackgroundColor = backgroundColor;
                        /*
                        absoluteLayout.BackgroundColor = backgroundColor;
                        frame.BackgroundColor = backgroundColor;
                        grid.BackgroundColor = backgroundColor;
                        relativeLayout.BackgroundColor = backgroundColor;
                        stackLayout.BackgroundColor = backgroundColor;
                        */
                        break;
                    case "OUTLINE":
                        var outlineColor = segment.IsSelected ? Color.Blue : Color.Default;
                        ((Forms9Patch.ILayout)Content).OutlineColor = outlineColor;
                        /*
                        absoluteLayout.OutlineColor = outlineColor;
                        frame.OutlineColor = outlineColor;
                        grid.OutlineColor = outlineColor;
                        relativeLayout.OutlineColor = outlineColor;
                        stackLayout.OutlineColor = outlineColor;
                        */
                        var outlineWidth = segment.IsSelected ? (float)Math.Max(outlineWidthSlider.Value, 2) : 0;
                        ((Forms9Patch.ILayout)Content).OutlineWidth = outlineWidth;
                        /*
                        absoluteLayout.OutlineWidth = outlineWidth;
                        frame.OutlineWidth = outlineWidth;
                        grid.OutlineWidth = outlineWidth;
                        relativeLayout.OutlineWidth = outlineWidth;
                        stackLayout.OutlineWidth = outlineWidth;
                        */
                        break;
                    case "SHADOW":
                        ((Forms9Patch.ILayout)Content).HasShadow = segment.IsSelected;
                        /*
                        absoluteLayout.HasShadow = segment.IsSelected;
                        frame.HasShadow = segment.IsSelected;
                        grid.HasShadow = segment.IsSelected;
                        relativeLayout.HasShadow = segment.IsSelected;
                        stackLayout.HasShadow = segment.IsSelected;
                        */
                        break;
                    case "INVERTED":
                        ((Forms9Patch.ILayout)Content).ShadowInverted = segment.IsSelected;
                        /*
                        absoluteLayout.ShadowInverted = segment.IsSelected;
                        frame.ShadowInverted = segment.IsSelected;
                        grid.ShadowInverted = segment.IsSelected;
                        relativeLayout.ShadowInverted = segment.IsSelected;
                        stackLayout.ShadowInverted = segment.IsSelected;
                        */
                        break;
                }
        }

        void SetBackgroundImage(object sender = null, Forms9Patch.SegmentedControlEventArgs args = null)
        {
            var segment = backgroundImageSelector.SelectedSegments.First();
            if (segment.Text == "NONE")
            {
                ((Forms9Patch.ILayout)Content).BackgroundImage = null;
                /*
                absoluteLayout.BackgroundImage = null;
                frame.BackgroundImage = null;
                grid.BackgroundImage = null;
                relativeLayout.BackgroundImage = null;
                stackLayout.BackgroundImage = null;
                */
                return;
            }
            ((Forms9Patch.ILayout)Content).BackgroundImage = new Forms9Patch.Image((ImageSource)segment.IconImage?.Source);
            /*
            absoluteLayout.BackgroundImage = new Forms9Patch.Image(segment.IconImage?.Source);
            frame.BackgroundImage = new Forms9Patch.Image(segment.IconImage?.Source);
            grid.BackgroundImage = new Forms9Patch.Image(segment.IconImage?.Source);
            relativeLayout.BackgroundImage = new Forms9Patch.Image(segment.IconImage?.Source);
            stackLayout.BackgroundImage = new Forms9Patch.Image(segment.IconImage?.Source);
            */
            SetFill();
            SetCapsInsets();
            SetAntiAlias();
        }

        void SetCapsInsets(object sender = null, ValueChangedEventArgs args = null)
        {
            if (((Forms9Patch.ILayout)Content)?.BackgroundImage?.Source != null)
            {
                double scale = capsUnitsSegmentedControl.IsIndexSelected(0) ? 1 : 0.01;
                var capsInset = new Thickness(capsInsetsLeftSlider.Value * scale, capsInsetsTopSlider.Value * scale, capsInsetsRightSlider.Value * scale, capsInsetsBottomSlider.Value * scale);
                //System.Diagnostics.Debug.WriteLine("CapsInset=[" + Forms9Patch.ThicknessExtension.Description(capsInset) + "]");
                ((Forms9Patch.ILayout)Content).BackgroundImage.CapInsets = capsInset;
                /*
                absoluteLayout.BackgroundImage.CapInsets = capsInset;
                frame.BackgroundImage.CapInsets = capsInset;
                grid.BackgroundImage.CapInsets = capsInset;
                relativeLayout.BackgroundImage.CapInsets = capsInset;
                stackLayout.BackgroundImage.CapInsets = capsInset;
                */
            }
        }

        void SetLayoutProperties()
        {
            SetHzLayoutOptions();
            SetVtLayoutOptions();
            SetHeightRequest();
            SetShape();
            SetOutlineWidth();
            SetOutlineRadius();
            SetShapeAttributes();
            SetBackgroundImage();
        }

        Forms9Patch.AbsoluteLayout GenerateAbsoluteLayout()
        {
            var absoluteLayout = new Forms9Patch.AbsoluteLayout();
            var absoluteLayoutContent = new Forms9Patch.Label("CONTENT")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            AbsoluteLayout.SetLayoutBounds(absoluteLayoutContent, new Rectangle(0.5, 0.5, 1.0, 1.0));
            AbsoluteLayout.SetLayoutFlags(absoluteLayoutContent, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(absoluteLayoutContent);
            return absoluteLayout;
        }

        Forms9Patch.Frame GenerateFrame()
        {
            var frame = new Forms9Patch.Frame
            {
                Content = new Forms9Patch.Label("CONTENT") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
            };
            return frame;
        }

        Forms9Patch.Grid GenerateGrid()
        {
            var grid = new Forms9Patch.Grid
            {
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
            grid.Children.Add(new Forms9Patch.Label("C0R0") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 0, 0);
            grid.Children.Add(new Forms9Patch.Label("C0R1") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 0, 1);
            grid.Children.Add(new Forms9Patch.Label("C1R0") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 1, 0);
            grid.Children.Add(new Forms9Patch.Label("C1R1") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold }, 1, 1);
            return grid;
        }

        Forms9Patch.RelativeLayout GenerateRelativeLayout()
        {
            var relativeLayout = new Forms9Patch.RelativeLayout();
            var cView = new Forms9Patch.Label("C")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var lView = new Forms9Patch.Label("L")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var rView = new Forms9Patch.Label("R")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var tView = new Forms9Patch.Label("T")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            var bView = new Forms9Patch.Label("T")
            {
                TextColor = Color.Orange,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            relativeLayout.Children.Add(cView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToParent((parent) => parent.Height / 2));
            relativeLayout.Children.Add(tView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToView(cView, (parent, sibling) => sibling.Y - parent.Height / 4));
            relativeLayout.Children.Add(bView, Constraint.RelativeToParent((parent) => parent.Width / 2), Constraint.RelativeToView(cView, (parent, sibling) => sibling.Y + parent.Height / 4));
            relativeLayout.Children.Add(lView, Constraint.RelativeToView(cView, (parent, sibling) => sibling.X - parent.Width / 4), Constraint.RelativeToParent((parent) => parent.Height / 2));
            relativeLayout.Children.Add(rView, Constraint.RelativeToView(cView, (parent, sibling) => sibling.X + parent.Width / 4), Constraint.RelativeToParent((parent) => parent.Height / 2));
            return relativeLayout;
        }

        Forms9Patch.StackLayout GenerateStackLayout()
        {
            var stackLayout = new Forms9Patch.StackLayout
            {
                Children =
                {
                    new Forms9Patch.Label("Child 1") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment=TextAlignment.Center, FontAttributes=FontAttributes.Bold },
                    new Forms9Patch.Label("Child 2") { TextColor = Color.Orange, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment=TextAlignment.Center, FontAttributes=FontAttributes.Bold }
                }
            };
            return stackLayout;
        }
    }


}


