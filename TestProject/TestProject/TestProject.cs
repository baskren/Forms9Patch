using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;

namespace TestProject
{
	public class App : Application
	{

		ICommand _trueCommand = new Command ((parameter) => System.Diagnostics.Debug.WriteLine ("_simpleCommand Parameter[" + parameter + "]"), parameter=>true );

		ICommand _falseCommand = new Command (parameter => System.Diagnostics.Debug.WriteLine ("_commandB [" + parameter + "]"), parameter => false);


		void OnSegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		void OnSegmentLongPressing(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		void OnSegmentLongPressed(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Segment[" + e.Index + "] Text=["+e.Segment.Text+"]");
		}

		void OnMaterialButtonTapped(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		void OnMaterialButtonSelected(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		void OnMaterialButtonLongPressing(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		void OnMaterialButtonLongPressed(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Button Text=["+((Forms9Patch.MaterialButton)sender).Text+"]");
		}

		void OnImageButtonTapped(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		void OnImageButtonSelected(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		void OnImageButtonLongPressing(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		void OnImageButtonLongPressed(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		const bool debugProperties = true;
		//static bool debugCollections = true;

		NavigationPage navPage = new NavigationPage(new HomePage());
		//NavigationPage navPage = new NavigationPage(new ImageButtonPage());


		void OnPagePopped(object sender, NavigationEventArgs e)
		{
			IDisposable displosable = e.Page as IDisposable;
			System.Diagnostics.Debug.WriteLine("");
		}

		public App ()
		{

			navPage.Popped += OnPagePopped;

			if (true) {
				//MainPage = new MaterialSegmentedControlPage();
				//MainPage = new NinePatchButtonPage();
				//MainPage = new ModalPopupTestPage();
				//MainPage = new BubbleLayoutTestPage();
				//MainPage = new ImageBenchmarkPage();
				//MainPage = new BubblePopupTestPage();
				//MainPage = new ChatListPage();
				//MainPage = new ImageButtonPage();
				//MainPage = new HtmlButtonsPage();
				//MainPage = new HtmlLabelPage();
				//MainPage = new ImageCodePage();

				//MainPage = new ZenmekPage();
				MainPage = navPage;

			} else {
				// The root page of your application
				/*
			Label label = new Label {
				XAlign = TextAlignment.Center,
				Text = "Welcome to Xamarin Forms!"
			};

			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						label
					}
				}
			};
			System.Diagnostics.Debug.WriteLine ("label.Text = " + label.Text);
			*/

				/*
				// Gesture recognition test code
				var fancyLabel = new Label () {
					Text = "Hello Forms!",
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand
				};

				var tapGestureRecognizer = new TapGestureRecognizer ();
				tapGestureRecognizer.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("tapped: "+tapGestureRecognizer.State + " " + tapGestureRecognizer.Location);
				fancyLabel.GestureRecognizers.Add (tapGestureRecognizer);

				var swipeGestureRecognizer = new SwipeGestureRecognizer ();
				swipeGestureRecognizer.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine("swipped: "+swipeGestureRecognizer.State + " " + swipeGestureRecognizer.Location);
				fancyLabel.GestureRecognizers.Add (swipeGestureRecognizer);

				var longPressGestureRecognizer = new LongPressGestureRecognizer ();
				longPressGestureRecognizer.LongPressed += (sender, e) =>  System.Diagnostics.Debug.WriteLine("long pressed: "+longPressGestureRecognizer.State + " " + longPressGestureRecognizer.Location);
				fancyLabel.GestureRecognizers.Add (longPressGestureRecognizer);

				var panGestureRecongizer = new PanGestureRecognizer ();
				panGestureRecongizer.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("panned: "+panGestureRecongizer.State + " " + panGestureRecongizer.Location);
				fancyLabel.GestureRecognizers.Add (panGestureRecongizer);

				var doubleClickRecognizer = new TapGestureRecognizer() {
					NumberOfTapsRequired = 2
				};
				doubleClickRecognizer.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("double: "+doubleClickRecognizer.State + " " + doubleClickRecognizer.Location);
				fancyLabel.GestureRecognizers.Add (doubleClickRecognizer);

				MainPage = new ContentPage {
					Content = fancyLabel
				};
				*/





				#region Images
				var si50 = new Forms9Patch.Image {
					//var image = new Xamarin.Forms.Image() {
					WidthRequest = 100,
					HeightRequest = 100,
					Source = ImageSource.FromResource("TestProject.Resources.50x50.png"),
					CapInsets = new Thickness(20),
				};


				var si100 = new Forms9Patch.Image {
					//var image = new Xamarin.Forms.Image() {
					WidthRequest = 100,
					HeightRequest = 100,
					Source = ImageSource.FromResource("TestProject.Resources.100x100.png"),
					CapInsets = new Thickness(20),
				};

				var si200 = new Forms9Patch.Image {
					//var image = new Xamarin.Forms.Image() {
					WidthRequest = 100,
					HeightRequest = 100,
					Source = ImageSource.FromResource("TestProject.Resources.200x200.png"),
					CapInsets = new Thickness(20),
				};
				//var image = new Forms9Patch.Image() {
				var i50 = new Xamarin.Forms.Image {
					//WidthRequest = 50,
					//HeightRequest = 100,
					//Aspect = Aspect.,
					Source = ImageSource.FromResource("TestProject.Resources.50x50.png"),
				};
				var i100 = new Xamarin.Forms.Image {
					//WidthRequest = 50,
					//HeightRequest = 100,
					//Aspect = Aspect.,
					Source = ImageSource.FromResource("TestProject.Resources.100x100.png"),
				};
				var i200 = new Xamarin.Forms.Image {
					//WidthRequest = 50,
					//HeightRequest = 100,
					//Aspect = Aspect.,
					Source = ImageSource.FromResource("TestProject.Resources.200x200.png"),
				};


				var siPU = new Forms9Patch.Image {
					HeightRequest = 150,
					Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.PopupDM.png"),
				};

				var iPU = new Xamarin.Forms.Image {
					Source = ImageSource.FromResource("TestProject.Resources.PopupDM.png"),
				};

				var b1 = new Xamarin.Forms.Button {
					Text = "pizza cheese, smile, lkj, asdlfkjdslk",
					//Image = "five.png",
					BackgroundColor = Color.Red,
				};
				b1.Image = "five.png";
				#endregion

				#region ImageButtons
				var b2 = new Forms9Patch.ImageButton {
					DefaultState = new Forms9Patch.ImageButtonState {
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.button"),
						},
						Image = new Xamarin.Forms.Image {
							Source = ImageSource.FromFile("five.png"),
						},
						FontColor = Color.White,
						Text = "Sticky w/ SelectedState",
					},
					SelectedState = new Forms9Patch.ImageButtonState {
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.image"),
						},
						FontColor = Color.Red,
						Text = "Selected",
					},
					ToggleBehavior = true,
					HeightRequest = 50,
					Alignment = TextAlignment.Start,
				};
				b2.Tapped += OnImageButtonTapped;
				b2.LongPressing += OnImageButtonLongPressing;
				b2.LongPressed += OnImageButtonLongPressed;
				b2.Selected += OnImageButtonSelected;

				var b3 = new Forms9Patch.ImageButton {
					DefaultState = new Forms9Patch.ImageButtonState {
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.button"),
						},
						Image = new Xamarin.Forms.Image {
							Source = ImageSource.FromFile("five.png"),
						},
						FontColor = Color.FromRgb(0.0, 0.0, 0.8),
						Text = "Sticky w/o SelectedState",
					},
					PressingState = new Forms9Patch.ImageButtonState {
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redButton"),
						},
						Text = "Pressing",
					},
					//ToggleBehavior = true,
					HeightRequest = 50,
					Alignment = TextAlignment.Center,
				};
				b3.Tapped += OnImageButtonTapped;
				b3.LongPressing += OnImageButtonLongPressing;
				b3.LongPressed += OnImageButtonLongPressed;
				b3.Selected += OnImageButtonSelected;

				var b4 = new Forms9Patch.ImageButton {
					DefaultState = new Forms9Patch.ImageButtonState {
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.button"),
						},
						Image = new Xamarin.Forms.Image {
							Source = ImageSource.FromFile("five.png"),
						},
						FontColor = Color.White,
						Text = "Not toggle",
					},
					//ToggleBehavior = true,
					HeightRequest = 50,
					Alignment = TextAlignment.End,
				};
				b4.Tapped += OnImageButtonTapped;
				b4.LongPressing += OnImageButtonLongPressing;
				b4.LongPressed += OnImageButtonLongPressed;
				b4.Selected += OnImageButtonSelected;
				#endregion

				#region RelativeLayout
				var heading = new Xamarin.Forms.Label {
					Text = "RelativeLayout Example",
					TextColor = Color.Red,
				};

				var relativelyPositioned = new Xamarin.Forms.Label {
					Text = "Positioned relative to my parent."
				};

				var relativeLayout = new Forms9Patch.RelativeLayout {
					BackgroundImage = new Forms9Patch.Image {
						Source = Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.blackrocks"),
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

				var sourceMode = "resource"; // "file", "uri", or "resource"
				Xamarin.Forms.ImageSource source;
				switch (sourceMode) {
				case "resource":
					source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox");
					break;
				case "uri":
					source = ImageSource.FromUri(new Uri ("https://raw.githubusercontent.com/baskren/TestProject/master/TestProject/Resources/redGridBox.png"));
					break;
				default:
					source = ImageSource.FromFile("redGridBox.png");
					break;
				}

				var infoIcon =  Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.Info");
				var arrowIcon = Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.ArrowR");



				#region Material Buttons
				var grid = new Xamarin.Forms.Grid {
					RowDefinitions = {
						new RowDefinition { Height = GridLength.Auto },
					},
					ColumnDefinitions = {
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					},

				};

				var mb1 = new Forms9Patch.MaterialButton {
					Text = "",
					ImageSource = arrowIcon,
				};
				mb1.Tapped += OnMaterialButtonTapped;
				mb1.Selected += OnMaterialButtonSelected;
				var mb2 = new Forms9Patch.MaterialButton {
					//Text = "toggle",
					ToggleBehavior = true,
					ImageSource = infoIcon,
				};
				mb2.Tapped += OnMaterialButtonTapped;
				mb2.Selected += OnMaterialButtonSelected;
				var mb3 = new Forms9Patch.MaterialButton {
					//Text = "disabled",
					ToggleBehavior = true,
					IsEnabled = false,
					ImageSource = arrowIcon,
				};
				mb3.Tapped += OnMaterialButtonTapped;
				mb3.Selected += OnMaterialButtonSelected;
				var mb4 = new Forms9Patch.MaterialButton {
					//Text = "selected disabled",
					IsEnabled = false,
					IsSelected = true,
					ImageSource = infoIcon,
				};
				mb4.Tapped += OnMaterialButtonTapped;
				mb4.Selected += OnMaterialButtonSelected;


				grid.Children.Add (new Xamarin.Forms.StackLayout {
					BackgroundColor = Color.FromHex("#33FF33"),
					Padding = new Thickness(10),
					Children = {

						new Xamarin.Forms.Label {
							Text = "Default, Light",
							TextColor = Color.Black,
						},
						mb1,mb2, mb3, mb4,

						new Xamarin.Forms.Label {
							Text = "Outline, Light",
							TextColor = Color.Black,
						},
						new Forms9Patch.MaterialButton {
							Text = "",
							ImageSource = arrowIcon,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							//Text = "toggle",
							ToggleBehavior = true,
							ImageSource = infoIcon,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							//Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							ImageSource = arrowIcon,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							//Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							ImageSource = infoIcon,
							OutlineWidth = 0,
						},

						new Xamarin.Forms.Label {
							Text = "Background Color, Light Theme",
							TextColor = Color.Black,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							BackgroundColor = Color.FromHex("#E0E0E0"),
							ImageSource = arrowIcon,
							Orientation = StackOrientation.Vertical,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							ImageSource = infoIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							ImageSource = arrowIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							ImageSource = infoIcon,
						},	

						new Xamarin.Forms.Label {
							Text = "Shadow, Light Theme",
							TextColor = Color.Black,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							HasShadow = true,
							ImageSource = infoIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							HasShadow = true,
							ImageSource = arrowIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							HasShadow = true,
							ImageSource = infoIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							HasShadow = true,
							ImageSource = arrowIcon,
						},

						new Xamarin.Forms.Label {
							Text = "Shadow Background Color, Light Theme",
							TextColor = Color.Black,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							BackgroundColor = Color.FromHex("#E0E0E0"),
							HasShadow = true,
							ImageSource = infoIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							HasShadow = true,
							ImageSource = arrowIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							HasShadow = true,
							ImageSource = infoIcon,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							BackgroundColor = Color.FromHex("#E0E0E0"),
							HasShadow = true,
							ImageSource = arrowIcon,
						},	

					},
				}, 0, 0);


				grid.Children.Add(new Xamarin.Forms.StackLayout {
					Padding = new Thickness(10),
					BackgroundColor = Color.FromHex("#003"),
					Children = {
						new Xamarin.Forms.Label {
							Text = "Default, Dark Theme",
							TextColor = Color.White,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							DarkTheme = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							DarkTheme = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							DarkTheme = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							DarkTheme = true,
						},

						new Xamarin.Forms.Label {
							Text = "Outline, Dark Theme",
							TextColor = Color.White,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							DarkTheme = true,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							DarkTheme = true,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							DarkTheme = true,
							OutlineWidth = 0,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							DarkTheme = true,
							OutlineWidth = 0,
						},

						new Xamarin.Forms.Label {
							Text = "Background Color, Dark Theme",
							TextColor = Color.White,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
							ImageSource = arrowIcon,
							Orientation = StackOrientation.Vertical,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
						},

						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
						},

						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
						},
						new Xamarin.Forms.Label {
							Text = "Shadow, Dark Theme",
							TextColor = Color.White,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							DarkTheme = true,
							HasShadow = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							DarkTheme = true,
							HasShadow = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							DarkTheme = true,
							HasShadow = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							DarkTheme = true,
							HasShadow = true,
						},
						new Xamarin.Forms.Label {
							Text = "Shadow Background Color, Dark Theme",
							TextColor = Color.White,
						},
						new Forms9Patch.MaterialButton {
							Text = "default",
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
							HasShadow = true,
						},
						new Forms9Patch.MaterialButton {
							Text = "toggle",
							ToggleBehavior = true,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
							HasShadow = true,
						},

						new Forms9Patch.MaterialButton {
							Text = "disabled",
							ToggleBehavior = true,
							IsEnabled = false,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
							HasShadow = true,
						},

						new Forms9Patch.MaterialButton {
							Text = "selected disabled",
							IsEnabled = false,
							IsSelected = true,
							BackgroundColor = Color.FromHex("#1194F6"),
							DarkTheme = true,
							HasShadow = true,
						},
					},
				},1,0);
				#endregion


				#region Light SegmentedControl

				var sc1 = new Forms9Patch.MaterialSegmentedControl {
					Segments = {

						new Forms9Patch.Segment {
							Text = "A",
							ImageSource = arrowIcon,
							Command = _trueCommand,
							CommandParameter = "sc1 A",
						},
						new Forms9Patch.Segment {
							//Text = "B",
							IsSelected = true,
							ImageSource = arrowIcon,
							Command = _trueCommand,
							CommandParameter = "sc1 B",
						},
						new Forms9Patch.Segment {
							Text = "C",
							Command = _trueCommand,
							CommandParameter = "sc1 C",
						},

						new Forms9Patch.Segment {
							Text = "D",
							//IsEnabled = false,
							Command = _falseCommand,
							CommandParameter = "sc1 D",
						},
					},
				};
				sc1.SegmentSelected += OnSegmentSelected;
				sc1.SegmentTapped += OnSegmentTapped;
				sc1.SegmentLongPressing += OnSegmentLongPressing;
				sc1.SegmentLongPressed += OnSegmentLongPressed;

				var seg1 = new Forms9Patch.Segment {
					//Text = "A",
					ImageSource = arrowIcon,
				};
				seg1.Tapped += OnMaterialButtonTapped;
				seg1.Selected += OnMaterialButtonTapped;
				seg1.LongPressing += OnMaterialButtonLongPressing;
				seg1.LongPressed += OnMaterialButtonLongPressed;

				var seg2 = new Forms9Patch.Segment {
					Text = "B",
					IsSelected = true,
				};
				seg2.Tapped += OnMaterialButtonTapped;
				seg2.Selected += OnMaterialButtonTapped;
				seg2.LongPressing += OnMaterialButtonLongPressing;
				seg2.LongPressed += OnMaterialButtonLongPressed;
				var seg3 = new Forms9Patch.Segment {
					Text = "C",
				};
				seg3.Tapped += OnMaterialButtonTapped;
				seg3.Selected += OnMaterialButtonTapped;
				seg3.LongPressing += OnMaterialButtonLongPressing;
				seg3.LongPressed += OnMaterialButtonLongPressed;
				var seg4 = new Forms9Patch.Segment {
					Text = "D",
					IsEnabled = false,
				};
				seg4.Tapped += OnMaterialButtonTapped;
				seg4.Selected += OnMaterialButtonTapped;
				seg4.LongPressing += OnMaterialButtonLongPressing;
				seg4.LongPressed += OnMaterialButtonLongPressed;


				var sc2 = new Forms9Patch.MaterialSegmentedControl {
					OutlineWidth = 0,
					Segments = {
						seg1, seg2, seg3, seg4,
					},
				};
				//sc2.SegmentSelected += OnSegmentSelected;
				//sc2.SegmentTapped += OnSegmentTapped;

				var sc3 = new Forms9Patch.MaterialSegmentedControl {
					//OutlineColor = Color.Transparent,
					BackgroundColor = Color.FromHex("#E0E0E0"),
					Segments = {
						new Forms9Patch.Segment {
							Text = "A",
						},
						new Forms9Patch.Segment {
							Text = "B",
							IsSelected = true,
						},
						new Forms9Patch.Segment {
							Text = "C",
						},
						new Forms9Patch.Segment {
							//Text = "D",
							IsEnabled = false,
							ImageSource = arrowIcon,
						},
					},
				};
				sc3.SegmentSelected += OnSegmentSelected;
				sc3.SegmentTapped += OnSegmentTapped;

				var sc4 = new Forms9Patch.MaterialSegmentedControl {
					BackgroundColor = Color.FromHex("#E0E0E0"),
					OutlineWidth = 0,
					SeparatorWidth = 1,
					GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.None,
					Segments = {
						new Forms9Patch.Segment {
							Text = "A",
							ImageSource = arrowIcon,
							Orientation = StackOrientation.Vertical,
						},
						new Forms9Patch.Segment {
							Text = "B",
							IsSelected = true,
							ImageSource = infoIcon,
							Orientation = StackOrientation.Vertical,
						},

						new Forms9Patch.Segment {
							Text = "C",
							ImageSource = arrowIcon,
						},
						new Forms9Patch.Segment {
							Text = "D",
							IsEnabled = false,
							ImageSource = infoIcon,
							Orientation = StackOrientation.Vertical,
						},

					},
				};
				sc4.SegmentSelected += OnSegmentSelected;
				sc4.SegmentTapped += OnSegmentTapped;

				var sc5 = new Forms9Patch.MaterialSegmentedControl {
					BackgroundColor = Color.FromHex("#E0E0E0"),
					HasShadow = true,
					//OutlineRadius = 0,
					//OutlineWidth = 0,
					Orientation = StackOrientation.Vertical,
					GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
					Segments = {

						new Forms9Patch.Segment {
							Text = "A",
							ImageSource = arrowIcon,
						},

						new Forms9Patch.Segment {
							Text = "B",
							IsSelected = true,
						},
						new Forms9Patch.Segment {
							Text = "C",
						},
						new Forms9Patch.Segment {
							Text = "D",
							IsEnabled = false,
						},

					},
				};
				sc5.SegmentSelected += OnSegmentSelected;
				sc5.SegmentTapped += OnSegmentTapped;

				var sc6 = new Forms9Patch.MaterialSegmentedControl {
					BackgroundColor = Color.FromHex("#E0E0E0"),
					HasShadow = true,
					//OutlineRadius = 0,
					OutlineWidth = 0,
					SeparatorWidth = 1,
					Orientation = StackOrientation.Vertical,
					GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
					Segments = {

						new Forms9Patch.Segment {
							Text = "A",
						},

						new Forms9Patch.Segment {
							Text = "B",
							IsSelected = true,
							ImageSource = arrowIcon,
							//Orientation = StackOrientation.Vertical,
						},
						new Forms9Patch.Segment {
							Text = "C",
						},
						new Forms9Patch.Segment {
							Text = "D",
							IsEnabled = false,
						},

					},
				};
				sc6.SegmentSelected += OnSegmentSelected;
				sc6.SegmentTapped += OnSegmentTapped;

				#endregion

				// Gestures
				var label1 = new Xamarin.Forms.Label {
					Text = "Gesture Label",
					BackgroundColor = Color.Blue,
					HeightRequest = 50,
				};

				var label1Listener = new FormsGestures.Listener (label1);
				label1Listener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine($"Tapped:{((Xamarin.Forms.Label)sender).Text}");
				label1Listener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine($"DoubleTapped:{((Xamarin.Forms.Label)sender).Text}");
				label1Listener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine($"LongPressed:{((Xamarin.Forms.Label)sender).Text}");
				//label1Listener.Tapped += (sender, e) => ((Forms9Patch.StackLayout)label1.Parent).Children.Remove(label1);
				label1Listener.Tapped += (sender, e) => label1Listener.Dispose();


				MainPage = new ContentPage {
					Padding = new Thickness (5, Xamarin.Forms.Device.OnPlatform (20, 0, 0), 5, 0),
					BackgroundColor = Color.White,
					Content = new Xamarin.Forms.ScrollView {
						Content = new Forms9Patch.StackLayout {
							/*
						BackgroundImage = new Forms9Patch.Image {
							Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.ghosts"),
							Fill = Forms9Patch.Fill.Tile,
						},
						*/
							//Orientation = StackOrientation.Horizontal,
							Children = { 

								label1,
								si50, si100, si200, i50, i100,i200,



							#region Image
							new Xamarin.Forms.Label {
								Text = "Forms9Patch.Image",
							},
							new Forms9Patch.Image {
								Source = Xamarin.Forms.ImageSource.FromResource("TestProject.Resources.water.gif"),
								//TintColor = Color.White,
								BackgroundColor = Color.Black,
							},
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFill,
							},
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFit,
							},
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Fill,
							},
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.Tile,
							},
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redGridBox"),
								Fill = Forms9Patch.Fill.AspectFill,
								CapInsets = new Thickness(10),
							},
							#endregion

							#region ContentView
							new Xamarin.Forms.Label {
								Text = "Forms9Patch.ContentView",
								TextColor = Color.Black,
								FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
							},
							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.AspectFill,
								},
								Content = new Xamarin.Forms.Label{
									Text = "ContentView AspectFill",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.Gray,
							},
							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.AspectFit,
								},
								Content = new Xamarin.Forms.Label{
									Text = "ContentView AspectFit",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.Gray,
							},
							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.Fill,
								},
								Content = new Xamarin.Forms.Label{
									Text = "ContentView Fill",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.Gray,
							},
							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.Tile,
								},
								Content = new Xamarin.Forms.Label{
									Text = "ContentView Tile",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.Gray,
							},
							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									CapInsets = new Thickness(10),
								},
								Content = new Xamarin.Forms.Label{
									Text = "ContentView scalable (CapInsets)",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.Gray,
							},
							new Xamarin.Forms.Label { 
								Text = "Forms9Patch.ImageSource.FromMultiSource >> Forms9Patch.ContentView", 
								FontSize = 10,
								HorizontalOptions = LayoutOptions.Center,
							},

							#endregion


							#region Frame
							new Xamarin.Forms.Label {
								Text = "Forms9Patch.Frame",
							},
							new Forms9Patch.Frame {
								Content = new Xamarin.Forms.Label {
									Text = "Frame OutlineRadius & Shadow",
									TextColor = Color.Black,
									FontSize = 12,
								},
								Padding = new Thickness(10),
								BackgroundColor = Color.FromHex("#FAFAFA"),
								//OutlineColor = Color.Blue,
								//OutlineWidth = 1,
								OutlineRadius = 2,
								HasShadow = true,
							},
							new Forms9Patch.Frame {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.AspectFill,
								},
								Content = new Xamarin.Forms.Label {
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
									Source = source,
									Fill = Forms9Patch.Fill.AspectFit,
								},
								Content = new Xamarin.Forms.Label {
									Text = "Frame AspectFit",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
							},
							new Forms9Patch.Frame {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.Fill,
								},
								Content = new Xamarin.Forms.Label {
									Text = "Frame Fill",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
							},
							new Forms9Patch.Frame {
								BackgroundImage = new Forms9Patch.Image {
									Source = source,
									Fill = Forms9Patch.Fill.Tile,
								},
								Content = new Xamarin.Forms.Label {
									Text = "Frame Tile",
									TextColor = Color.Black,
									FontSize = 12,
									BackgroundColor = Color.Green,
								},
								Padding = new Thickness(10),
							},

							#endregion

							#region CapsInset ContentView
							new Xamarin.Forms.Image { 
								Source = Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.redribbon"), 
								//HeightRequest = 80,
							},
							new Xamarin.Forms.Label { Text = "Forms9Patch.ImageSource.FromMultiSource >> Xamarin.Forms.Image", 
								FontSize = 12,
								HorizontalOptions = LayoutOptions.Center,

							},

							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redribbon"),
								CapInsets = new Thickness(30,0,110,0),
								//HeightRequest = 80,
							},

							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redribbon"),
									CapInsets = new Thickness(30,0,110,0),
								},
								Content = new Xamarin.Forms.Label{
									Text = "pizza",
									TextColor = Color.White,
									FontAttributes = FontAttributes.Bold,
									BackgroundColor = Color.Gray,
									FontSize = 16,

								},
								Padding = new Thickness(30,26,110,13),
								//HeightRequest = 80,
							},

							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redribbon"),
									CapInsets = new Thickness(30,-1,160,-1),
								},
								Content = new Xamarin.Forms.Label{
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

							new Forms9Patch.ContentView {
								BackgroundImage = new Forms9Patch.Image {
									Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.redribbon"),
									CapInsets = new Thickness(30,0,110,0),
								},
								Content = new Xamarin.Forms.StackLayout {
									Children = {
										new Xamarin.Forms.Label{
											Text = "pizza",
											TextColor = Color.White,
											FontAttributes = FontAttributes.Bold,
											BackgroundColor = Color.Gray,
											FontSize = 16,
										},				
										new Xamarin.Forms.Label{
											Text = "pizza",
											TextColor = Color.White,
											FontAttributes = FontAttributes.Bold,
											BackgroundColor = Color.Gray,
											FontSize = 16,

										},									
										new Xamarin.Forms.Label{
											Text = "pizza",
											TextColor = Color.White,
											FontAttributes = FontAttributes.Bold,
											BackgroundColor = Color.Gray,
											FontSize = 16,

										},									
									},
								},
								Padding = new Thickness(30,26,110,13),
								//HeightRequest = 80,
							},
							new Xamarin.Forms.Label { Text = "Forms9atch.ImageSource.FromMultiSource >> Forms9Patch.ContentView", 
								FontSize = 12,
								HorizontalOptions = LayoutOptions.Center,
							},
							#endregion

							#region RelativeLayout
							relativeLayout,
							#endregion


								#region MaterialSegmentControl

								new Xamarin.Forms.StackLayout {
									Orientation = StackOrientation.Horizontal,
									Children = {

										#region Light
										new Xamarin.Forms.StackLayout {
											BackgroundColor = Color.Lime,
											HorizontalOptions = LayoutOptions.FillAndExpand,
											Padding = new Thickness(10),
											Children = {
												new Xamarin.Forms.Label {
													Text = "Default, Light",
													TextColor = Color.Black,
												},

												sc1, sc2, sc3, sc4, sc5, sc6,

											},
										},
										#endregion


										#region Dark
										new Xamarin.Forms.StackLayout {
											BackgroundColor = Color.FromHex("#003"),
											HorizontalOptions = LayoutOptions.FillAndExpand,
											Padding = new Thickness(10),
											Children = {
												new Xamarin.Forms.Label {
													Text = "Default, Dark",
													TextColor = Color.White,
												},

												new Forms9Patch.MaterialSegmentedControl {
													//OutlineColor = Color.Transparent,
													DarkTheme = true,
													Segments = {

														new Forms9Patch.Segment {
															Text = "A",
														},
														new Forms9Patch.Segment {
															//Text = "B",
															IsSelected = true,
															ImageSource = arrowIcon,
														},
														new Forms9Patch.Segment {
															Text = "C",
														},

														new Forms9Patch.Segment {
															Text = "D",
															IsEnabled = false,
														},
													},
												},

												new Forms9Patch.MaterialSegmentedControl {
													DarkTheme = true,
													OutlineWidth = 0,
													Segments = {

														new Forms9Patch.Segment {
															//Text = "A",
															ImageSource = arrowIcon,
														},
														new Forms9Patch.Segment {
															Text = "B",
															IsSelected = true,
														},
														new Forms9Patch.Segment {
															Text = "C",
														},

														new Forms9Patch.Segment {
															Text = "D",
															IsEnabled = false,
														},
													},
												},

												new Forms9Patch.MaterialSegmentedControl {
													DarkTheme = true,
													BackgroundColor = Color.FromHex("#1194F6"),
													Segments = {
														new Forms9Patch.Segment {
															Text = "A",
														},
														new Forms9Patch.Segment {
															Text = "B",
															IsSelected = true,
														},
														new Forms9Patch.Segment {
															Text = "C",
														},
														new Forms9Patch.Segment {
															//Text = "D",
															IsEnabled = false,
															ImageSource = arrowIcon,
														},
													},
												},

												new Forms9Patch.MaterialSegmentedControl {
													DarkTheme = true,
													BackgroundColor = Color.FromHex("#1194F6"),
													OutlineWidth = 0,
													Segments = {
														new Forms9Patch.Segment {
															Text = "A",
															ImageSource = arrowIcon,
															Orientation = StackOrientation.Vertical,
														},
														new Forms9Patch.Segment {
															Text = "B",
															IsSelected = true,
															ImageSource = infoIcon,
															Orientation = StackOrientation.Vertical,
														},

														new Forms9Patch.Segment {
															Text = "C",
															ImageSource = arrowIcon,
														},
														new Forms9Patch.Segment {
															Text = "D",
															IsEnabled = false,
															ImageSource = infoIcon,
															Orientation = StackOrientation.Vertical,
														},

													},
												},

												new Forms9Patch.MaterialSegmentedControl {
													DarkTheme = true,
													BackgroundColor = Color.FromHex("#1194F6"),
													HasShadow = true,
													//OutlineRadius = 0,
													//OutlineWidth = 0,
													Orientation = StackOrientation.Vertical,
													GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
													Segments = {

														new Forms9Patch.Segment {
															Text = "A",
															ImageSource = arrowIcon,
														},

														new Forms9Patch.Segment {
															Text = "B",
															IsSelected = true,
														},
														new Forms9Patch.Segment {
															Text = "C",
														},
														new Forms9Patch.Segment {
															Text = "D",
															IsEnabled = false,
														},

													},
												},

												new Forms9Patch.MaterialSegmentedControl {
													DarkTheme = true,
													BackgroundColor = Color.FromHex("#1194F6"),
													HasShadow = true,
													//OutlineRadius = 0,
													OutlineWidth = 0,
													SeparatorWidth = 1,
													Orientation = StackOrientation.Vertical,
													GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
													Segments = {

														new Forms9Patch.Segment {
															Text = "A",
														},

														new Forms9Patch.Segment {
															Text = "B",
															IsSelected = true,
															ImageSource = arrowIcon,
														},
														new Forms9Patch.Segment {
															Text = "C",
														},
														new Forms9Patch.Segment {
															Text = "D",
															IsEnabled = false,
														},

													},
												},
											},
										},
										#endregion

									},
								},

								#endregion

								grid,


								/*
						new Xamarin.Forms.Button {
							Text = "pizza",
							Image = "five.png",
							BackgroundColor = Color.Red,
							//HeightRequest = 100,
						},
						

							new BC.FormsExt.Button {
								ToggleBehavior = true,
								DefaultState = new BC.FormsExt.Button.State {
									Text = "pizza",
									Image = new Forms9Patch.Image {
										Source = ImageSource.FromFile("five.png"),
										TintColor = Color.Red,
									},
									BackgroundColor = Color.Green,
									BorderColor = Color.Red,
									BorderWidth = 2,
									BorderRadius = 5,
								},
								SelectedState = new BC.FormsExt.Button.State {
									Text = "pizza",
									//Image = null,
									BackgroundColor = Color.Red,
									BorderColor = Color.Black,
								},
								//PressingState = new BC.FormsExt.Button.State {
								//	Text = "pizza",
									//Image = new Xamarin.Forms.Image {
									//	Source = ImageSource.FromFile("fivePlus.png"),
									//},
								//},
								HeightRequest = 100,
								Alignment = TextAlignment.Center,
							},
							*/
							b2,
							b3,
							b4,

						


							b1,
							new Forms9Patch.Image {
								Source = Forms9Patch.ImageSource.FromMultiResource("TestProject.Resources.image"),
							},
							b2,
							
							},
							//Children = { siPU },
							//HeightRequest = 50,
							//BackgroundColor = Color.White
						},
					}
				};

				/*
			b2.DefaultState.Text = "trees";

			b2.DisabledState = new BC.FormsExt.Button.State {
				BackgroundColor = Color.Gray,
				Text = "disabled",
			};

			b2.ToggleBehavior = true;
			*/

				//items.CollectionChanged += OnCollectionChanged;


				var button = new Xamarin.Forms.Button  {
					Text = "Click me",
					BackgroundColor = Color.White,
					TextColor = Color.Blue
				};
				/*
				popup = new PopupView (MainPage as BC.FormsExt.ContentPage) {
					Content = button,
					PositionCoordinateSystem = PopupView.CoordinateSystems.Relative,
					XPositionRequest = 0.5,
					YPositionRequest = 0.5,
				};

				button.Clicked += (sender, eventArgs) => popup.Hide ();
				*/


				// dynamically update the cells
				//Device.StartTimer(TimeSpan.FromSeconds(1),this.UpdateAnItem);

				// dynamically move cells
				//Device.StartTimer(TimeSpan.FromSeconds(1),this.MoveAnItem);

				// dynamicall reverse visibility of cells
				//Device.StartTimer(TimeSpan.FromSeconds(1), this.ReverseVisibilityOfAnItem);
				//Device.StartTimer(TimeSpan.FromSeconds(2), this.ReverseItem5);


				/*
			MainPage = new Xamarin.Forms.ContentPage {
				Content = new Xamarin.Forms.ScrollView {
					Content = new Xamarin.Forms.StackLayout {							
						Children = {
						},
					},
				},
			};
			*/			}

		}


		/*
		bool ReverseItem5() {
			var group0 = (Group) groups [0];
			var item5 = group0[5];
			item5.IsVisible = !item5.IsVisible;
			System.Diagnostics.Debug.WriteLine ("item5.IsVisible=" + item5.IsVisible);
			//if (BcGlobal.timerTrippedCount > 1)
			//	return false;
			return true;
		}
		*/
		/*
		private bool ReverseVisibilityOfAnItem() {
			// randomly pick an item
			Random rand = new Random();
			int index = (int)(rand.NextDouble () * items.Count);
			object item = items [index];

			baseObject baseItem = item as baseObject;
			if (baseItem != null) {
				baseItem.Title = baseItem.Title + "+";
				baseItem.IsVisible = !baseItem.IsVisible;
				return true;
			}
			return false;
		}

		private bool UpdateAnItem() {
			// randomly pick an item
			Random rand = new Random();
			int index = (int)(rand.NextDouble () * items.Count);
			object item = items [index];

			baseObject baseItem = item as baseObject;
			if (baseItem != null) {
				baseItem.Title = baseItem.Title + "+";
			}

			// update it
			boolObject boolItem = item as boolObject;
			if (boolItem != null) 
				boolItem.Value = !boolItem.Value;
			stringObject stringItem = item as stringObject;
			if (stringItem != null) 
				stringItem.Value = words [(int)(rand.NextDouble () * words.Count)];
			colorObject colorItem = item as colorObject;
			if (colorItem != null)
				colorItem.Value = Color.FromRgb ((int)(rand.NextDouble () * 255), (int)(rand.NextDouble () * 255), (int)(rand.NextDouble () * 255));
			doubleObject doubleItem = item as doubleObject;
			if (doubleItem != null)
				doubleItem.Value = rand.NextDouble ();
			return true;
		}

		private bool MoveAnItem() {
			Random rand = new Random ();
			int oldIndex = (int)(rand.NextDouble () * items.Count);
			int newIndex = (int)(rand.NextDouble () * items.Count);
			items.Move (oldIndex, newIndex);

			object item = items [newIndex];

			baseObject baseItem = item as baseObject;
			if (baseItem != null) {
				baseItem.Title = baseItem.Title + "+";
			}

			return true;
		}
*/

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

