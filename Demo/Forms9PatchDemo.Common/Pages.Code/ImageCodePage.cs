using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ImageCodePage : MasterDetailPage
    {
        #region Shape Strings
        static string Rectangle = "▭";
        static string Square = "□";
        static string Circle = "○";
        static string Ellipse = "⬭";
        #endregion

        #region Global Elements
        Slider capsInsetsLeftSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsRightSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsTopSlider = new Slider(0, 100, 0.0);
        Slider capsInsetsBottomSlider = new Slider(0, 100, 0.0);

        Forms9Patch.SliderStepSizeEffect capsInsetsLStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsRStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsTStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);
        Forms9Patch.SliderStepSizeEffect capsInsetsBStepSizeEffect = new Forms9Patch.SliderStepSizeEffect(1);

        //Switch pixelCapsSwitch = new Switch();
        Forms9Patch.SegmentedControl capsUnitsSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment("PIXL"),
                new Forms9Patch.Segment("%")
            },
        };

        Switch antiAliasSwitch = new Switch();

        Xamarin.Forms.Slider outlineRadiusSlider = new Xamarin.Forms.Slider
        {
            Minimum = 0,
            Maximum = 15,
            Value = 2
        };

        Forms9Patch.Image f9pImage = new Forms9Patch.Image
        {
            Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox"),
        };

        Xamarin.Forms.Image xamarinImage = new Xamarin.Forms.Image
        {
            Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox"),
            Aspect = Aspect.Fill
        };

        //BoxView f9pImageWidthBox = new BoxView { Color = Color.Red, HeightRequest = 1 };

        Forms9Patch.SegmentedControl hzOptionSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL"),
                    new Forms9Patch.Segment("EXPAND")
                },
            GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
            Lines = 1,
            AutoFit = Forms9Patch.AutoFit.Width
        };
        Forms9Patch.SegmentedControl vtOptionSegmentedControl = new Forms9Patch.SegmentedControl
        {
            Segments =
                {
                    new Forms9Patch.Segment("START"),
                    new Forms9Patch.Segment("CENTER"),
                    new Forms9Patch.Segment("END"),
                    new Forms9Patch.Segment("FILL"),
                    new Forms9Patch.Segment("EXPAND")
                },
            GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect
        };
        #endregion

        public ImageCodePage()
        {
            Title = "ImageCodePage";
            IsPresented = true;

            #region Local Elements
            hzOptionSegmentedControl.SelectIndex(3);
            hzOptionSegmentedControl.SegmentTapped += HzOptionSegmentedControl_SegmentTapped;

            vtOptionSegmentedControl.SelectIndex(3);
            vtOptionSegmentedControl.SegmentTapped += VtOptionSegmentedControl_SegmentTapped;


            var heightRequestSlider = new Slider(-1, 300, -1);
            heightRequestSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            heightRequestSlider.ValueChanged += HeightRequestSlider_ValueChanged;

            var fillSegmentedControl = new Forms9Patch.SegmentedControl
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
            shapesSelector.SelectIndex(0);
            shapesSelector.SegmentTapped += ShapesSelector_SegmentTapped;

            var outlineWidthSlider = new Xamarin.Forms.Slider
            {
                Minimum = 0,
                Maximum = 15,
                Value = 2
            };
            outlineWidthSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1.0 / Forms9Patch.Display.Scale));
            outlineWidthSlider.ValueChanged += OutlineWidthSlider_ValueChanged;

            outlineRadiusSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            outlineRadiusSlider.ValueChanged += OutlineRadiusSlider_ValueChanged;

            var shapeAttributesSelector = new Forms9Patch.SegmentedControl
            {
                GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
                Segments =
                {
                    new Forms9Patch.Segment("BACKGROUND"),
                    new Forms9Patch.Segment("OUTLINE"),
                    new Forms9Patch.Segment("SHADOW"),
                    new Forms9Patch.Segment("INVERTED"),
                    new Forms9Patch.Segment("TINT")
                }
            };
            shapeAttributesSelector.SegmentTapped += ShapeAttributesSelector_SegmentTapped;

            var seg1 = new Forms9Patch.Segment("");
            var seg2 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.redGridBox");
            var seg3 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.button");
            var seg4 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.cat.jpg");
            var seg5 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.balloons.jpg");
            var seg6 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.image");
            var seg7 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.printerIcon");
            var seg8 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.bubble");
            var seg9 = new Forms9Patch.Segment(null, "Forms9PatchDemo.Resources.SvgSample.svg");

            var backgroundImageSelector = new Forms9Patch.SegmentedControl
            {
                HeightRequest = 40,
                HasTightSpacing = true,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TintIcon = false,
                Segments =
                {
                    seg1,
                    seg2,
                    seg3,
                    seg4,
                    seg5,
                    seg6,
                    seg7,
                    seg8,
                    seg9

                }
            };
            /*
            backgroundImageSelector.Segments.Add(seg1);
            backgroundImageSelector.Segments.Add(seg2);
            backgroundImageSelector.Segments.Add(seg3);
            backgroundImageSelector.Segments.Add(seg4);
            backgroundImageSelector.Segments.Add(seg5);
            backgroundImageSelector.Segments.Add(seg6);
            backgroundImageSelector.Segments.Add(seg7);
            backgroundImageSelector.Segments.Add(seg8);
            backgroundImageSelector.Segments.Add(seg9);
            */
            backgroundImageSelector.SelectIndex(1);
            backgroundImageSelector.SegmentTapped += BackgroundImageSelector_SegmentTapped;

            capsInsetsTopSlider.Effects.Add(capsInsetsTStepSizeEffect);
            capsInsetsTopSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsLeftSlider.Effects.Add(capsInsetsLStepSizeEffect);
            capsInsetsLeftSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsRightSlider.Effects.Add(capsInsetsRStepSizeEffect);
            capsInsetsRightSlider.Rotation = 180;
            capsInsetsRightSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsBottomSlider.Effects.Add(capsInsetsBStepSizeEffect);
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
            outlineGrid.Children.Add(new Forms9Patch.Label("OutlineWidth:"), 0, 0);
            outlineGrid.Children.Add(outlineWidthSlider, 0, 1);
            outlineGrid.Children.Add(new Forms9Patch.Label("OutlineRadius:"), 1, 0);
            outlineGrid.Children.Add(outlineRadiusSlider, 1, 1);

            //pixelCapsSwitch.Toggled += PixelCapsSwitch_Toggled;

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
            capsUnitsSegmentedControl.SegmentTapped += CapsUnitsSegmentedControl_SegmentTapped;
            antiAliasSwitch.Toggled += AntiAliasSwitch_Toggled;
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

            #endregion

            #region Content
            Master = new ContentPage
            {
                Title = "Controls",
                BackgroundColor = Color.LightGray,
                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = new Thickness(10, 10, 20, 10),
                        Children = {

                        new Forms9Patch.Label("HorizontalOptions:"),
                        hzOptionSegmentedControl,
                        new Forms9Patch.Label("VerticalOptions:"),
                        vtOptionSegmentedControl,
                        new Forms9Patch.Label("Fill / Aspect:"),
                        fillSegmentedControl,
                        new Forms9Patch.Label("ElementShape:"),
                        shapesSelector,
                        new Forms9Patch.Label("Shape attributes:"),
                        shapeAttributesSelector,
                        outlineGrid,

                        new Forms9Patch.Label("ImageSource:"),
                        backgroundImageSelector,

                        capsInsetsAndAntiAliasGrid,
                        capsInsetsGrid,

                        new Forms9Patch.Label("HeightRequest:"),
                        heightRequestSlider,


                            new Xamarin.Forms.Label { Text="Display.Scale=["+Forms9Patch.Display.Scale+"]" }


                    },
                    },
                }
            };
            Detail = new ContentPage
            {
                Title = "Images",

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                            new Forms9Patch.Label("Forms9Patch.Image:"),
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                            f9pImage,
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                            new Forms9Patch.Label("Xamarin.Forms.Image:"),
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                            xamarinImage,
                            new BoxView { Color=Color.Black, HeightRequest = 1},
                        }
                    }
                }
            };

            #endregion

        }

        #region Event Handlers
        private void AntiAliasSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            f9pImage.AntiAlias = e.Value;
        }


        private void CapsUnitsSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            if (e.Index == 0)
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
            switch (e.Segment.Text)
            {
                case "START":
                    hzOptionSegmentedControl.SelectIndex(0);
                    f9pImage.HorizontalOptions = (hzOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.StartAndExpand : LayoutOptions.Start);
                    hzOptionSegmentedControl.DeselectIndex(1);
                    hzOptionSegmentedControl.DeselectIndex(2);
                    hzOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "CENTER":
                    hzOptionSegmentedControl.SelectIndex(1);
                    f9pImage.HorizontalOptions = (hzOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.CenterAndExpand : LayoutOptions.Center);
                    hzOptionSegmentedControl.DeselectIndex(0);
                    hzOptionSegmentedControl.DeselectIndex(2);
                    hzOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "END":
                    hzOptionSegmentedControl.SelectIndex(2);
                    f9pImage.HorizontalOptions = (hzOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.EndAndExpand : LayoutOptions.End);
                    hzOptionSegmentedControl.DeselectIndex(0);
                    hzOptionSegmentedControl.DeselectIndex(1);
                    hzOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "FILL":

                    hzOptionSegmentedControl.SelectIndex(3);
                    f9pImage.HorizontalOptions = (hzOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.FillAndExpand : LayoutOptions.Fill);
                    hzOptionSegmentedControl.DeselectIndex(0);
                    hzOptionSegmentedControl.DeselectIndex(1);
                    hzOptionSegmentedControl.DeselectIndex(2);
                    break;
                case "EXPAND":
                    f9pImage.HorizontalOptions = new LayoutOptions(f9pImage.HorizontalOptions.Alignment, e.Segment.IsSelected);
                    break;
            }
            xamarinImage.HorizontalOptions = f9pImage.HorizontalOptions;
        }

        private void VtOptionSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            switch (e.Segment.Text)
            {
                case "START":
                    vtOptionSegmentedControl.SelectIndex(0);
                    f9pImage.VerticalOptions = (vtOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.StartAndExpand : LayoutOptions.Start);
                    vtOptionSegmentedControl.DeselectIndex(1);
                    vtOptionSegmentedControl.DeselectIndex(2);
                    vtOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "CENTER":
                    vtOptionSegmentedControl.SelectIndex(1);
                    f9pImage.VerticalOptions = (vtOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.CenterAndExpand : LayoutOptions.Center);
                    vtOptionSegmentedControl.DeselectIndex(0);
                    vtOptionSegmentedControl.DeselectIndex(2);
                    vtOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "END":
                    vtOptionSegmentedControl.SelectIndex(2);
                    f9pImage.VerticalOptions = (vtOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.EndAndExpand : LayoutOptions.End);
                    vtOptionSegmentedControl.DeselectIndex(0);
                    vtOptionSegmentedControl.DeselectIndex(1);
                    vtOptionSegmentedControl.DeselectIndex(3);
                    break;
                case "FILL":
                    vtOptionSegmentedControl.SelectIndex(3);
                    f9pImage.VerticalOptions = (vtOptionSegmentedControl.IsIndexSelected(4) ? LayoutOptions.FillAndExpand : LayoutOptions.Fill);
                    vtOptionSegmentedControl.DeselectIndex(0);
                    vtOptionSegmentedControl.DeselectIndex(1);
                    vtOptionSegmentedControl.DeselectIndex(2);
                    break;
                case "EXPAND":
                    f9pImage.VerticalOptions = new LayoutOptions(f9pImage.VerticalOptions.Alignment, e.Segment.IsSelected);
                    break;
            }
            xamarinImage.VerticalOptions = f9pImage.VerticalOptions;
        }

        private void HeightRequestSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            f9pImage.HeightRequest = e.NewValue;
            xamarinImage.HeightRequest = e.NewValue;
        }

        private void FillSegmentedControl_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            switch (e.Segment.Text)
            {
                case "TILE":
                    f9pImage.Fill = Forms9Patch.Fill.Tile;
                    xamarinImage.Aspect = Xamarin.Forms.Aspect.Fill;
                    break;
                case "ASPECTFILL":
                    f9pImage.Fill = Forms9Patch.Fill.AspectFill;
                    xamarinImage.Aspect = Xamarin.Forms.Aspect.AspectFill;
                    break;
                case "ASPECTFIT":
                    f9pImage.Fill = Forms9Patch.Fill.AspectFit;
                    xamarinImage.Aspect = Xamarin.Forms.Aspect.AspectFit;
                    break;
                default:
                    f9pImage.Fill = Forms9Patch.Fill.Fill;
                    xamarinImage.Aspect = Xamarin.Forms.Aspect.Fill;
                    break;
            }
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
            f9pImage.ElementShape = shape;
        }

        private void OutlineWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            f9pImage.OutlineWidth = (float)e.NewValue;
        }

        private void OutlineRadiusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            f9pImage.OutlineRadius = (float)e.NewValue;
        }

        private void ShapeAttributesSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            switch (e.Segment.Text)
            {
                case "BACKGROUND":
                    f9pImage.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    xamarinImage.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    break;
                case "OUTLINE":
                    f9pImage.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    f9pImage.OutlineWidth = e.Segment.IsSelected ? (float)Math.Max(outlineRadiusSlider.Value, 2) : 0;
                    break;
                case "SHADOW":
                    f9pImage.HasShadow = e.Segment.IsSelected;
                    break;
                case "INVERTED":
                    f9pImage.ShadowInverted = e.Segment.IsSelected;
                    break;
                case "TINT":
                    f9pImage.TintColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    break;
            }
        }

        private void BackgroundImageSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            f9pImage.Source = e.Segment.IconImage?.Source;
            xamarinImage.Source = e.Segment.IconImage?.Source;
        }

        private void CapsInsetsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double scale = capsUnitsSegmentedControl.IsIndexSelected(0) ? 1 : 0.01; // pixelCapsSwitch.IsToggled ? 1 : 0.01;
            var capsInset = new Thickness(capsInsetsLeftSlider.Value * scale, capsInsetsTopSlider.Value * scale, capsInsetsRightSlider.Value * scale, capsInsetsBottomSlider.Value * scale);
            //System.Diagnostics.Debug.WriteLine("CapsInset=[" + Forms9Patch.ThicknessExtension.Description(capsInset) + "]");
            f9pImage.CapInsets = capsInset;
        }
        #endregion

    }
}


