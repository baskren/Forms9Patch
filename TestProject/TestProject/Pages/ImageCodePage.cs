using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class ImageCodePage : ContentPage
	{
		public ImageCodePage ()
		{
			const double fontSize = 9;
			Padding = 20;
			Content = new ScrollView
			{
				Content = new StackLayout
				{
					Children = {

                       

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
							CapInsets = new Thickness(23.0/308.0,-1,111.0/308.0,-1),
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

						#region Image


                        new Label { Text = "f9p: multiresource: redGridBox fill:None" },
                        new Forms9Patch.Image {
                            Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
                            Fill = Forms9Patch.Fill.None,
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
						#endregion
                        
                        
                        
                        

					},
				},
			};
		}
	}
}


