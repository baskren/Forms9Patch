using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class LayoutCodePage : ContentPage
    {
        #region Shapes
        static string Rectangle = "▭";
        static string Square = "□";
        static string Circle = "○";
        static string Ellipse = "⬭";
        static string Obround = "◖▮◗";
        #endregion

        Slider capsInsetsLeftSlider = new Slider(0, 1, 0.0);
        Slider capsInsetsRightSlider = new Slider(0, 1, 0.0);
        Slider capsInsetsTopSlider = new Slider(0, 1, 0.0);
        Slider capsInsetsBottomSlider = new Slider(0, 1, 0.0);

        Forms9Patch.AbsoluteLayout absoluteLayout = new Forms9Patch.AbsoluteLayout
        {
            BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") },
        };
        Forms9Patch.Frame frame = new Forms9Patch.Frame
        {
            BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") },
            Content = new Forms9Patch.Label("CONTENT"),
        };
        Forms9Patch.Grid grid = new Forms9Patch.Grid
        {
            BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") },
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
            BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") },
        };
        Forms9Patch.StackLayout stackLayout = new Forms9Patch.StackLayout
        {
            BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") },
            Children =
                {
                    new Forms9Patch.Label("Child 1"),
                    new Forms9Patch.Label("Child 2")
                }
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
                    new Forms9Patch.Segment("gridBox","Forms9PatchDemo.Resources.redGridBox"),
                    new Forms9Patch.Segment("button", "Forms9PatchDemo.Resources.button"),
                    new Forms9Patch.Segment("cat"),
                    new Forms9Patch.Segment("balloons"),
                    new Forms9Patch.Segment("image.9"),
                }
            };
            backgroundImageSelector.SelectIndex(1);
            backgroundImageSelector.SegmentTapped += BackgroundImageSelector_SegmentTapped;

            capsInsetsTopSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.01));
            capsInsetsTopSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsLeftSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.01));
            capsInsetsLeftSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsRightSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.01));
            capsInsetsRightSlider.Rotation = 180;
            capsInsetsRightSlider.ValueChanged += CapsInsetsSlider_ValueChanged;
            capsInsetsBottomSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.01));
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

            var absoluteLayoutContent = new Forms9Patch.Label("CONTENT");
            AbsoluteLayout.SetLayoutFlags(absoluteLayoutContent, AbsoluteLayoutFlags.PositionProportional);


            grid.Children.Add(new Forms9Patch.Label("C0R0"), 0, 0);
            grid.Children.Add(new Forms9Patch.Label("C0R1"), 0, 1);
            grid.Children.Add(new Forms9Patch.Label("C1R0"), 1, 0);
            grid.Children.Add(new Forms9Patch.Label("C1R1"), 1, 1);

            relativeLayout.Children.Add(new Forms9Patch.Label("L"), Constraint.RelativeToParent((parent) => { return 0; }), Constraint.RelativeToParent((parent) => { return parent.Height/2; }));
            relativeLayout.Children.Add(new Forms9Patch.Label("R"), Constraint.RelativeToParent((parent) => { return parent.Width; }), Constraint.RelativeToParent((parent) => { return parent.Height / 2; }));
            relativeLayout.Children.Add(new Forms9Patch.Label("T"), Constraint.RelativeToParent((parent) => { return parent.Width/2; }), Constraint.RelativeToParent((parent) => { return 0; }));
            relativeLayout.Children.Add(new Forms9Patch.Label("B"), Constraint.RelativeToParent((parent) => { return parent.Width / 2; }), Constraint.RelativeToParent((parent) => { return parent.Height; }));



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
                        capsInsetsGrid,

                        new Forms9Patch.Label("Xamarin.Forms.VisualElement.HeightRequest:"),
                        heightRequestSlider,

                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                        new Label { Text = "AbsoluteLayout" },
                        absoluteLayout,
                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                        new Label { Text = "Frame" },
                        frame,
                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                        new Label { Text = "Grid" },
                        grid,
                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                        new Label { Text = "RelativeLayout" },
                        relativeLayout,
                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },

                        new Label { Text = "StackLayout" },
                        stackLayout,
                        new BoxView { HeightRequest = 1, BackgroundColor = Color.Black },
                    },
                },
            };
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
                    absoluteLayout.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    frame.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    grid.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    relativeLayout.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    stackLayout.BackgroundColor = e.Segment.IsSelected ? Color.Orange : Color.Default;
                    break;
                case "OUTLINE":
                    absoluteLayout.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    frame.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    grid.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    relativeLayout.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
                    stackLayout.OutlineColor = e.Segment.IsSelected ? Color.Blue : Color.Default;
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
            switch (e.Segment.Text)
            {
                case "NONE":
                    absoluteLayout.BackgroundImage = null;
                    frame.BackgroundImage = null;
                    grid.BackgroundImage = null;
                    relativeLayout.BackgroundImage = null;
                    stackLayout.BackgroundImage = null;
                    break;
                case "gridBox":
                    absoluteLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") };
                    frame.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") };
                    grid.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") };
                    relativeLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") };
                    stackLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redGridBox") };
                    break;
                case "button":
                    absoluteLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") };
                    frame.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") };
                    grid.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") };
                    relativeLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") };
                    stackLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button") };
                    break;
                case "cat":
                    absoluteLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("cat.jpg") };
                    frame.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("cat.jpg") };
                    grid.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("cat.jpg") };
                    relativeLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("cat.jpg") };
                    stackLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("cat.jpg") };
                    break;
                case "balloons":
                    absoluteLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("balloons.jpg") };
                    frame.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("balloons.jpg") };
                    grid.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("balloons.jpg") };
                    relativeLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("balloons.jpg") };
                    stackLayout.BackgroundImage = new Forms9Patch.Image { Source = ImageSource.FromFile("balloons.jpg") };
                    break;
                case "image9":
                    absoluteLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") };
                    frame.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") };
                    grid.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") };
                    relativeLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") };
                    stackLayout.BackgroundImage = new Forms9Patch.Image { Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") };
                    break;
            }
        }

        private void CapsInsetsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var capsInset = new Thickness(capsInsetsLeftSlider.Value, capsInsetsTopSlider.Value, capsInsetsRightSlider.Value, capsInsetsBottomSlider.Value);
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


