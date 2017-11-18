using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class ImageCodePage : ContentPage
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

        public ImageCodePage ()
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

            var outlineWidthSlider = new Xamarin.Forms.Slider
            {
                Minimum = 0,
                Maximum = 15,
                Value = 2
            };
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
                    new Forms9Patch.Segment("gridBox"),
                    new Forms9Patch.Segment("button"),
                    new Forms9Patch.Segment("cat"),
                    new Forms9Patch.Segment("balloons"),
                    new Forms9Patch.Segment("image"),
                    new Forms9Patch.Segment("redRibbon"),
                    new Forms9Patch.Segment("bubble"),
                    new Forms9Patch.Segment("blueButton")
                }
            };
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


            Padding = new Thickness(20, 20, 0, 20);
            Content = new ScrollView
            {
                Padding = new Thickness(0, 0, 20, 0),
                Content = new StackLayout
                {
                    Children = {

                        new Forms9Patch.Label("Xamarin.Forms.VisualElement.HorizontalOptions (Alignment):"),
                        hzOptionSegmentedControl,
                        new Forms9Patch.Label("Xamarin.Forms.VisualElement.VerticalOptions (Alignment):"),
                        vtOptionSegmentedControl,
                        new Forms9Patch.Label("Forms9Patch.Image.Fill:"),
                        fillSegmentedControl,
                        new Forms9Patch.Label("Forms9Patch.Image.ElementShape:"),
                        shapesSelector,
                        new Forms9Patch.Label("Shape attributes (some value vs. default value)"),
                        shapeAttributesSelector,
                        outlineGrid,

                        new Forms9Patch.Label("Forms9Patch.Image.Source:"),
                        backgroundImageSelector,
                        new Forms9Patch.Label("CapsInsets Pixels (ON) / Percent (OFF):"),
                        pixelCapsSwitch,
                        capsInsetsGrid,

                        new Forms9Patch.Label("Xamarin.Forms.VisualElement.HeightRequest:"),
                        heightRequestSlider,



                        new Label { Text = "f9p: multiresource: redGridBox fill:None HzOpt=Center" },
                        new Forms9Patch.Image {
                            Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
                        },


					},
				},
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
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.HorizontalOptions = layoutOption;
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
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.VerticalOptions = layoutOption;
        }

        private void HeightRequestSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.HeightRequest = e.NewValue;
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
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.Fill = fill;
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
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.ElementShape = shape;
        }

        private void OutlineWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.OutlineWidth = (float)e.NewValue;
        }

        private void OutlineRadiusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.OutlineRadius = (float)e.NewValue;
        }

        private void ShapeAttributesSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    switch (e.Segment.Text)
            {
                case "BACKGROUND":
                            f9pImage.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                            break;
                        case "OUTLINE":
                            f9pImage.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                            break;
                        case "SHADOW":
                            f9pImage.HasShadow = e.Segment.IsSelected;
                            break;
                        case "INVERTED":
                            f9pImage.ShadowInverted = e.Segment.IsSelected;
                            break;
            }
        }

        private void BackgroundImageSelector_SegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                {
                    switch (e.Segment.Text)
                    {
                        case "NONE":
                            f9pImage.Source = null;
                            break;
                        case "gridBox":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox");
                            break;
                        case "button":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button");
                            break;
                        case "cat":
                            f9pImage.Source = ImageSource.FromFile("cat.jpg");
                            break;
                        case "balloons":
                            f9pImage.Source = ImageSource.FromFile("balloons.jpg");
                            break;
                        case "image":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image");
                            break;
                        case "redRibbon":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redribbon");
                            break;
                        case "bubble":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.bubble");
                            break;
                        case "blueButton":
                            f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.bluebutton_psd");
                            break;
                    }
                }

        }

        private void CapsInsetsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double scale = pixelCapsSwitch.IsToggled ? 1 : 0.01;
            var capsInset = new Thickness(capsInsetsLeftSlider.Value * scale, capsInsetsTopSlider.Value * scale, capsInsetsRightSlider.Value * scale, capsInsetsBottomSlider.Value * scale);
            System.Diagnostics.Debug.WriteLine("CapsInset=["+Forms9Patch.ThicknessExtension.Description(capsInset)+"]");
            foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                if (child is Forms9Patch.Image f9pImage)
                    f9pImage.CapInsets = capsInset;
        }
    }
}


