using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class LayoutsPage : ContentPage
	{
		public LayoutsPage ()
		{

			#region RelativeLayout
			var heading = new Label {
				Text = "RelativeLayout Example",
				TextColor = Color.Red,
			};

			var relativelyPositioned = new Label {
				Text = "Positioned relative to my parent."
			};

			var relativeLayout = new Forms9Patch.RelativeLayout {
				BackgroundImage = new Forms9Patch.Image {
					Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ghosts"),
					Fill = Forms9Patch.Fill.Tile,
				},
				BackgroundColor = Color.White,
				OutlineColor = Color.Green,
				OutlineWidth = 3,
				OutlineRadius = 2,
				HeightRequest = 100,
			};

			relativeLayout.Children.Add (heading, Constraint.RelativeToParent (parent => 0));

			relativeLayout.Children.Add (relativelyPositioned,
				Constraint.RelativeToParent (parent => parent.Width / 3),
				Constraint.RelativeToParent (parent => parent.Height / 2)
			);
			#endregion

			const double fontSize = 9;

			Content = new ScrollView {
				Content = new StackLayout { 
					Children = {

						#region ContentView
						new Label {
							Text = "Forms9Patch.ContentView",
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFill,
							},
							Content = new Label{
								Text = "ContentView AspectFill",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFit,
							},
							Content = new Label{
								Text = "ContentView AspectFit",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Fill,
							},
							Content = new Label{
								Text = "ContentView Fill",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Tile,
							},
							Content = new Label{
								Text = "ContentView Tile",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								CapInsets = new Thickness(10),
							},
							Content = new Label{
								Text = "ContentView scalable (CapInsets)",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						#endregion

						#region Frame
						new Label {
							Text = "Forms9Patch.Frame",
						},
						new Forms9Patch.Frame {
							Content = new Label {
								Text = "Frame OutlineWidth & OutlineRadius",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
							OutlineColor = Color.Blue,
							OutlineWidth = 1,
							OutlineRadius = 4,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFill,
							},
							Content = new Label {
								Text = "Frame AspectFill",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
							OutlineColor = Color.Blue,
							OutlineWidth = 1,
							OutlineRadius = 4,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFit,
							},
							Content = new Label {
								Text = "Frame AspectFit",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Fill,
							},
							Content = new Label {
								Text = "Frame Fill",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Tile,
							},
							Content = new Label {
								Text = "Frame Tile",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redGridBox"),
								CapInsets = new Thickness(10),
							},
							Content = new Label {
								Text = "Frame scalable (CapInsets)",
								TextColor = Color.Black,
								FontSize = 12,
								BackgroundColor = Color.Green,
							},
							Padding = new Thickness(10),
							BackgroundColor = Color.Gray,
						},
						#endregion

						#region CapsInset ContentView
						new Image { 
							Source = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.redribbon"), 
						},
						new Label { Text = "Forms9Patch.ImageSource.FromMultiSource >> Xamarin.Forms.Image", 
							FontSize = fontSize,
							HorizontalOptions = LayoutOptions.Center,
						},

						new Forms9Patch.Frame {
							BackgroundImage = new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("Forms9PatchDemo.Resources.redribbon"),
								CapInsets = new Thickness(30,-1,160,-1),
							},
							Content = new Label{
								Text = "ContentView scalable (CapInsets)",
								TextColor = Color.White,
								FontAttributes = FontAttributes.Bold,
								//BackgroundColor = Color.Gray,
								FontSize = 14,
								HorizontalOptions = LayoutOptions.Center,
								VerticalOptions = LayoutOptions.Center,
							},
							Padding = new Thickness(30,30,110,20),
							HeightRequest = 80,
						},

						new Label { Text = "Forms9atch.ImageSource.FromMultiSource >> Forms9Patch.ContentView", 
							FontSize = fontSize,
							HorizontalOptions = LayoutOptions.Center,
						},
						#endregion

						relativeLayout,
					},
				},
			};
		}
	}
}


