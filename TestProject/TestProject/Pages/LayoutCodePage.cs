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

        public LayoutCodePage()
        {
            //const double fontSize = 9;


            var hzOptionSegmentedControl = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("HZ.START"),
                    new Forms9Patch.Segment("HZ.CENTER"),
                    new Forms9Patch.Segment("HZ.END"),
                    new Forms9Patch.Segment("HZ.FILL")
                },
            };
            hzOptionSegmentedControl.SelectIndex(3);
            hzOptionSegmentedControl.SegmentTapped += (sender0, e0) =>
            {
                var layoutOption = LayoutOptions.Fill;
                if (e0.Segment.Text == "HZ.START")
                    layoutOption = LayoutOptions.Start;
                else if (e0.Segment.Text == "HZ.CENTER")
                    layoutOption = LayoutOptions.Center;
                else if (e0.Segment.Text == "HZ.END")
                    layoutOption = LayoutOptions.End;
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.HorizontalOptions = layoutOption;
            };
            var vtOptionSegmentedControl = new Forms9Patch.MaterialSegmentedControl
            {
                Segments =
                {
                    new Forms9Patch.Segment("VT.START"),
                    new Forms9Patch.Segment("VT.CENTER"),
                    new Forms9Patch.Segment("VT.END"),
                    new Forms9Patch.Segment("VT.FILL")
                }
            };
            vtOptionSegmentedControl.SelectIndex(3);
            vtOptionSegmentedControl.SegmentTapped += (sender1, e1) =>
            {
                var layoutOption = LayoutOptions.Fill;
                if (e1.Segment.Text == "VT.START")
                    layoutOption = LayoutOptions.Start;
                else if (e1.Segment.Text == "VT.CENTER")
                    layoutOption = LayoutOptions.Center;
                else if (e1.Segment.Text == "VT.END")
                    layoutOption = LayoutOptions.End;
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.VerticalOptions = layoutOption;
            };
            var heightRequestSwitch = new Xamarin.Forms.Switch();
            heightRequestSwitch.Toggled += (sender2, e2) =>
            {
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.HeightRequest = e2.Value ? 75 : -1;
            };
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
            fillSegmentedControl.SegmentTapped += (sender3, e3) =>
            {
                var fill = Forms9Patch.Fill.Fill;
                if (e3.Segment.Text == "TILE")
                    fill = Forms9Patch.Fill.Tile;
                else if (e3.Segment.Text == "ASPECTFILL")
                    fill = Forms9Patch.Fill.AspectFill;
                else if (e3.Segment.Text == "ASPECTFIT")
                    fill = Forms9Patch.Fill.AspectFit;
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.Fill = fill;
            };
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
            shapesSelector.SegmentTapped += (sender4, e4) =>
            {
                var shape = Forms9Patch.ElementShape.Rectangle;
                if (e4.Segment.Text == Square)
                    shape = Forms9Patch.ElementShape.Square;
                else if (e4.Segment.Text == Circle)
                    shape = Forms9Patch.ElementShape.Circle;
                else if (e4.Segment.Text == Ellipse)
                    shape = Forms9Patch.ElementShape.Elliptical;
                else if (e4.Segment.Text == "OBROUND")
                    shape = Forms9Patch.ElementShape.Obround;
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.ElementShape = shape;
            };
            var outlineWidthSlider = new Xamarin.Forms.Slider
            {
                Minimum = 0,
                Maximum = 15,
                Value = 2
            };
            outlineWidthSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1.0 / Forms9Patch.Display.Scale));
            outlineWidthSlider.ValueChanged += (sender6, e6) =>
            {
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.OutlineWidth = (float)outlineWidthSlider.Value;
            };
            var outlineRadiusSlider = new Xamarin.Forms.Slider
            {
                Minimum = 0,
                Maximum = 15,
                Value = 0
            };
            outlineRadiusSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1));
            outlineRadiusSlider.ValueChanged += (sender7, e7) =>
            {
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                        f9pImage.OutlineRadius = (float)outlineRadiusSlider.Value;
            };
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
            shapeAttributesSelector.SegmentTapped += (sender5, e5) =>
            {
                if (e5.Segment.Text == "BACKGROUND")
                {
                    foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                        if (child is Forms9Patch.Image f9pImage)
                            f9pImage.BackgroundColor = e5.Segment.IsSelected ? Color.Orange : Color.Default;
                }
                else if (e5.Segment.Text == "OUTLINE")
                {
                    foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                        if (child is Forms9Patch.Image f9pImage)
                        {
                            f9pImage.OutlineColor = e5.Segment.IsSelected ? Color.Blue : Color.Default;
                            f9pImage.OutlineWidth = (float)outlineWidthSlider.Value;
                        }
                }
                else if (e5.Segment.Text == "SHADOW")
                {
                    foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                        if (child is Forms9Patch.Image f9pImage)
                            f9pImage.HasShadow = e5.Segment.IsSelected;
                }
                else if (e5.Segment.Text == "INVERTED")
                {
                    foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                        if (child is Forms9Patch.Image f9pImage)
                            f9pImage.ShadowInverted = e5.Segment.IsSelected;
                }
            };
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
            backgroundImageSelector.SegmentTapped += (sender8, e8) =>
            {
                foreach (var child in ((StackLayout)((ScrollView)Content).Content).Children)
                    if (child is Forms9Patch.Image f9pImage)
                    {
                        switch (e8.Segment.Text)
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
                            case "image9":
                                f9pImage.Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png");
                                break;
                        }
                    }
            };

            Padding = 20;
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children = {
                        new Forms9Patch.Label("HorizontalOptions (Alignment):"),
                        hzOptionSegmentedControl,
                        vtOptionSegmentedControl,
                        new Forms9Patch.Label("Fixed Height:"),
                        heightRequestSwitch,
                        fillSegmentedControl,
                        shapesSelector,
                        shapeAttributesSelector,
                        new Forms9Patch.Label("OutlineWidth:"),
                        outlineWidthSlider,
                        new Forms9Patch.Label("OutlineRadius:"),
                        outlineRadiusSlider,
                        new Forms9Patch.Label("Source:"),
                        backgroundImageSelector,
                       /*

                        new Label { Text = "f9p: multiresource: button"},
						new Forms9Patch.Image {
							//Source = Xamarin.Forms.ImageSource.FromFile("sampleFile.png"),
							//Source = Xamarin.Forms.ImageSource.FromFile("bubble.9.png"),
							//Source = Xamarin.Forms.ImageSource.FromFile("button.9.png"),
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
                        
                        
						new Label { Text = "f9p: multiresource: adsBttn29" },
						new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.adsBttn29"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        
                        new Label { Text = "f9p: multiresource: bluebutton_psd" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.bluebutton_psd"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

						#region Original Image

                            

						new Label { Text = "f9p: resource: bubble.9.png" },
                        new Forms9Patch.Image {
							Source = ImageSource.FromResource("Forms9PatchDemo.Resources.bubble.9.png"),
							HeightRequest = 110,
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        new Label { Text = "xf: multiresource: redribbon" },
                        new Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redribbon"), 
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
                        new Label { Text = "f9p: uri: redribbon + CAPSINSET" },
                        new Forms9Patch.Image {
							Source = ImageSource.FromUri(new Uri("http://buildcalc.com/forms9patch/demo/redribbon.png")),
							CapInsets = new Thickness(23,0,111,0),
                            //CapInsets = new Thickness(0.1,0, 0.1, 0)
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
                        new Label { Text = "f9p: mulitresource: button" },
                        new Forms9Patch.Image { 
							Source =Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"), 
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },


                        new Label { Text = "f9p: multiresource: button + CAPSINSET" },
                        new Forms9Patch.Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.button"), 
							CapInsets = new Thickness(111,3,4,5),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
                        new Label { Text = "f9p: multiresource: redribbon + CAPSINSET" },
                        new Forms9Patch.Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redribbon"), 
							CapInsets = new Thickness(23.0/308.0,0,111.0/308.0,0),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        
                        new Label { Text = "xf: multiresource: image.9.png" },
                        new Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png") },
                        new BoxView { HeightRequest = 1, Color = Color.Black },


                        new Label { Text = "f9p: multiresource: image.9.png" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image.9.png"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },


                        new Label { Text = "xf: multiresource: image" },
                        new Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image") 
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        new Label { Text = "f9p: multiresource: image" },
                        new Forms9Patch.Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.image") ,
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        

                        new Label { Text = "xf: file: cat.jpg" },
                        new Image { 
							Source = ImageSource.FromFile("cat.jpg"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        new Label { Text = "xf: file: balloons.jpg" },
                        new Image { 
							Source = ImageSource.FromFile("balloons.jpg"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        new Label { Text = "xf: file: image.png" },
                        new Image { 
							Source = ImageSource.FromFile("image.png"),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
						#endregion
                        */


						#region Image


                        new Label { Text = "f9p: multiresource: redGridBox fill:None HzOpt=Center" },
                        new Forms9Patch.Image {
                            Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
                            //Fill = Forms9Patch.Fill.None,
                            //HorizontalOptions = LayoutOptions.Center
                        },

                                                                        /*

                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        new Label { Text = "f9p: multiresource: redGridBox fill:None HzOpt=Fill (default)" },
                        new Forms9Patch.Image {
                            Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
                            Fill = Forms9Patch.Fill.None,
                            //HorizontalOptions = LayoutOptions.End
                        },
                        new BoxView { HeightRequest = 1, Color = Color.Black },
    
                        new Label { Text = "f9p: multiresource: redGridBox fill:AspectFill" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
							Fill = Forms9Patch.Fill.AspectFill,
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                        new Label { Text = "f9p: multiresource: redGridBox fill:AspectFit" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
							Fill = Forms9Patch.Fill.AspectFit,
						},


                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        new Label { Text = "f9p: multiresource: redGridBox Fill:fill" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
							Fill = Forms9Patch.Fill.Fill,
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        
                        new Label { Text = "f9p: multiresource: redGridBox Fill.Tile" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
							Fill = Forms9Patch.Fill.Tile,
						},
                        
                        new BoxView { HeightRequest = 1, Color = Color.Black },
                        new Label { Text = "f9p: multiresource: redGridBox Fill.AspectFill CAPINSETS:10" },
                        new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
							Fill = Forms9Patch.Fill.AspectFill,
							CapInsets = new Thickness(10),
						},
                        new BoxView { HeightRequest = 1, Color = Color.Black },

                                                */

						#endregion
                        
                        

					},
                },
            };
        }
    }
}


