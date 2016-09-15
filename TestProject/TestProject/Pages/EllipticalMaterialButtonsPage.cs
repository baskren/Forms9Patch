using System;

using Xamarin.Forms;
using System.Windows.Input;

namespace TestProject
{
	public class EllipticalMaterialButtonsPage : ContentPage
	{
		readonly ICommand _trueCommand = new Command (parameter => System.Diagnostics.Debug.WriteLine ("_simpleCommand Parameter[" + parameter + "]"), parameter=>true );

		readonly ICommand _falseCommand = new Command (parameter => System.Diagnostics.Debug.WriteLine ("_commandB [" + parameter + "]"), parameter => false);


		static void OnSegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		static void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		static void OnSegmentLongPressing(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		static void OnSegmentLongPressed(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		static void OnMaterialButtonTapped(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		static void OnMaterialButtonSelected(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		static void OnMaterialButtonLongPressing(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		static void OnMaterialButtonLongPressed(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}


		public EllipticalMaterialButtonsPage ()
		{
			var infoIcon =  Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.Info");
			var arrowIcon = Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.ArrowR");

			#region Material Buttons
			var grid = new Grid {
				RowDefinitions = {
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};



			var mb1 = new Forms9Patch.MaterialButton {
				Text = "",
				ImageSource = arrowIcon,
				IsElliptical = true
			};
			mb1.Tapped += OnMaterialButtonTapped;
			mb1.Selected += OnMaterialButtonSelected;
			mb1.LongPressing += OnMaterialButtonLongPressing;
			mb1.LongPressed += OnMaterialButtonLongPressed;
			var mb2 = new Forms9Patch.MaterialButton {
				//Text = "toggle",
				ToggleBehavior = true,
				ImageSource = infoIcon,
				IsElliptical = true
			};
			mb2.Tapped += OnMaterialButtonTapped;
			mb2.Selected += OnMaterialButtonSelected;
			mb2.LongPressing += OnMaterialButtonLongPressing;
			mb2.LongPressed += OnMaterialButtonLongPressed;
			var mb3 = new Forms9Patch.MaterialButton {
				//Text = "disabled",
				ToggleBehavior = true,
				IsEnabled = false,
				ImageSource = arrowIcon,
				IsElliptical = true
			};
			mb3.Tapped += OnMaterialButtonTapped;
			mb3.Selected += OnMaterialButtonSelected;
			mb3.LongPressing += OnMaterialButtonLongPressing;
			mb3.LongPressed += OnMaterialButtonLongPressed;
			var mb4 = new Forms9Patch.MaterialButton {
				//Text = "selected disabled",
				IsEnabled = false,
				IsSelected = true,
				ImageSource = infoIcon,
				IsElliptical = true
			};
			mb4.Tapped += OnMaterialButtonTapped;
			mb4.Selected += OnMaterialButtonSelected;
			mb4.LongPressing += OnMaterialButtonLongPressing;
			mb4.LongPressed += OnMaterialButtonLongPressed;


			var label1 = new Label {
				Text = "Gesture Label",
				BackgroundColor = Color.Blue,
				HeightRequest = 50
			};

			var label1Listener = new FormsGestures.Listener (label1);
			label1Listener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine($"Tapped:{((Label)sender).Text}");
			label1Listener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine($"DoubleTapped:{((Label)sender).Text}");
			label1Listener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine($"LongPressing:{((Label)sender).Text}");
			// How to remove a listener!
			label1Listener.LongPressed += (sender, e) => {
				label1Listener.Dispose();
				System.Diagnostics.Debug.WriteLine("Removed FormsGestures.Listener");
			};


			grid.Children.Add (new StackLayout {
				BackgroundColor = Color.FromHex("#33FF33"),
				Padding = new Thickness(10),
				Children = {

					new Label {
						Text = "Default, Light",
						TextColor = Color.Black
					},
					mb1,mb2, mb3, mb4,

					new Label {
						Text = "Outline, Light",
						TextColor = Color.Black
					},
					new Forms9Patch.MaterialButton {
						Text = "",
						ImageSource = arrowIcon,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						//Text = "toggle",
						ToggleBehavior = true,
						ImageSource = infoIcon,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						//Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						ImageSource = arrowIcon,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						//Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						ImageSource = infoIcon,
						OutlineWidth = 0,
						IsElliptical = true
					},

					new Label {
						Text = "Background Color, Light Theme",
						TextColor = Color.Black
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						BackgroundColor = Color.FromHex("#E0E0E0"),
						ImageSource = arrowIcon,
						Orientation = StackOrientation.Vertical,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						ImageSource = infoIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						ImageSource = arrowIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						ImageSource = infoIcon,
						IsElliptical = true
					},	

					new Label {
						Text = "Shadow, Light Theme",
						TextColor = Color.Black
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						HasShadow = true,
						ImageSource = infoIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						HasShadow = true,
						ImageSource = arrowIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						HasShadow = true,
						ImageSource = infoIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						HasShadow = true,
						ImageSource = arrowIcon,
						IsElliptical = true
					},

					new Label {
						Text = "Shadow Background Color, Light Theme",
						TextColor = Color.Black
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						ImageSource = infoIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						ImageSource = arrowIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						ImageSource = infoIcon,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						ImageSource = arrowIcon,
						IsElliptical = true
					},	

				},
			}, 0, 0);


			grid.Children.Add(new StackLayout {
				Padding = new Thickness(10),
				BackgroundColor = Color.FromHex("#003"),
				Children = {
					new Label {
						Text = "Default, Dark Theme",
						TextColor = Color.White
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						DarkTheme = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
						IsElliptical = true
					},

					new Label {
						Text = "Outline, Dark Theme",
						TextColor = Color.White
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						DarkTheme = true,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
						OutlineWidth = 0,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
						OutlineWidth = 0,
						IsElliptical = true
					},

					new Label {
						Text = "Background Color, Dark Theme",
						TextColor = Color.White
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						ImageSource = arrowIcon,
						Orientation = StackOrientation.Vertical,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						IsElliptical = true
					},

					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						IsElliptical = true
					},

					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						IsElliptical = true
					},
					new Label {
						Text = "Shadow, Dark Theme",
						TextColor = Color.White
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
					new Label {
						Text = "Shadow Background Color, Dark Theme",
						TextColor = Color.White
					},
					new Forms9Patch.MaterialButton {
						Text = "default",
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
					new Forms9Patch.MaterialButton {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},

					new Forms9Patch.MaterialButton {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},

					new Forms9Patch.MaterialButton {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
						IsElliptical = true
					},
				},
			},1,0);
			#endregion


			Padding = 20;
			Content = new ScrollView { 
				Content = new StackLayout{
					Children = {
						grid,
					},
				},
			};
		}
	}
}


