using System;

using Xamarin.Forms;
using System.Windows.Input;

namespace Forms9PatchDemo
{
	public class MaterialButtonsPage : ContentPage
	{

        readonly ICommand _trueCommand = new Command (parameter => System.Diagnostics.Debug.WriteLine ("_simpleCommand Parameter[" + parameter + "]"), parameter=>true );

		readonly ICommand _falseCommand = new Command (parameter => System.Diagnostics.Debug.WriteLine ("_commandB [" + parameter + "]"), parameter => false);

        #region Gesture Handlers
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
			System.Diagnostics.Debug.WriteLine ("Tapped Button Text=["+((Forms9Patch.Button)sender).Text+"]");
		}

		static void OnMaterialButtonSelected(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Button Text=["+((Forms9Patch.Button)sender).Text+"]");
		}

		static void OnMaterialButtonLongPressing(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Button Text=["+((Forms9Patch.Button)sender).Text+"]");
		}

		static void OnMaterialButtonLongPressed(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Button Text=["+((Forms9Patch.Button)sender).Text+"]");
		}
        #endregion


        #region Shapes
        static string Rectangle = "▭";
        static string Square = "□";
        static string Circle = "○";
        static string Ellipse = "⬭";
        static string Obround = "◖▮◗";
        #endregion

        #region Icons
        static string _infoIconText = "🛈";
        static string _arrowIconText = "⇨";
        static Xamarin.Forms.ImageSource _infoIconSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.Info");
        static Xamarin.Forms.ImageSource _arrowIconSource = Forms9Patch.ImageSource.FromMultiResource("Forms9PatchDemo.Resources.ArrowR");
        #endregion#

        #region Fields
        Forms9Patch.SegmentedControl _shapesSelector = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment { Text = Rectangle },
                new Forms9Patch.Segment { Text = Square },
                new Forms9Patch.Segment { Text = Circle },
                new Forms9Patch.Segment { Text = Ellipse },
                new Forms9Patch.Segment { Text = Obround }
            }
        };
        Forms9Patch.SegmentedControl _iconSelector = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment { },
                new Forms9Patch.Segment { IconText = _infoIconText },
                new Forms9Patch.Segment { IconText = _arrowIconText },
                new Forms9Patch.Segment { IconImage = new Forms9Patch.Image { Source =_infoIconSource } },
                new Forms9Patch.Segment { IconImage = new Forms9Patch.Image { Source =_arrowIconSource } },
            }
        };
        Forms9Patch.SegmentedControl _horizontalTextAlignment = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment { Text = "LEFT" },
                new Forms9Patch.Segment { Text = "CENTER" },
                new Forms9Patch.Segment { Text = "RIGHT" },
            }
        };
        Forms9Patch.SegmentedControl _verticalTextAlignment = new Forms9Patch.SegmentedControl
        {
            Segments =
            {
                new Forms9Patch.Segment { Text = "TOP" },
                new Forms9Patch.Segment { Text = "CENTER" },
                new Forms9Patch.Segment { Text = "BOTTOM" },
            }
        };
        Xamarin.Forms.Entry _textEntry = new Entry { Text = "ButtonText "};
        #endregion


        public MaterialButtonsPage ()
		{

			#region Material Buttons
			var grid = new Grid {
				RowDefinitions = {
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				},

			};



			var mb1 = new Forms9Patch.Button {
				Text = "TI",
				IconImage = new Forms9Patch.Image { Source = _arrowIconSource },
				TintIcon = false,
			};
			mb1.Tapped += OnMaterialButtonTapped;
			mb1.Selected += OnMaterialButtonSelected;
			mb1.LongPressing += OnMaterialButtonLongPressing;
			mb1.LongPressed += OnMaterialButtonLongPressed;
			var mb2 = new Forms9Patch.Button {
				//Text = "toggle",
				ToggleBehavior = true,
				IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
			};
			mb2.Tapped += OnMaterialButtonTapped;
			mb2.Selected += OnMaterialButtonSelected;
			mb2.LongPressing += OnMaterialButtonLongPressing;
			mb2.LongPressed += OnMaterialButtonLongPressed;
			var mb3 = new Forms9Patch.Button {
				//Text = "disabled",
				ToggleBehavior = true,
				IsEnabled = false,
				IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
			};
			mb3.Tapped += OnMaterialButtonTapped;
			mb3.Selected += OnMaterialButtonSelected;
			mb3.LongPressing += OnMaterialButtonLongPressing;
			mb3.LongPressed += OnMaterialButtonLongPressed;
			var mb4 = new Forms9Patch.Button {
				//Text = "selected disabled",
				IsEnabled = false,
				IsSelected = true,
				IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
			};
			mb4.Tapped += OnMaterialButtonTapped;
			mb4.Selected += OnMaterialButtonSelected;
			mb4.LongPressing += OnMaterialButtonLongPressing;
			mb4.LongPressed += OnMaterialButtonLongPressed;


			var label1 = new Label {
				Text = "Gesture Label",
				BackgroundColor = Color.Blue,
				HeightRequest = 50,
			};

			var label1Listener = FormsGestures.Listener.For(label1);
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
						TextColor = Color.Black,
					},
					mb1,mb2, mb3, mb4,

					new Label {
						Text = "Outline, Light",
						TextColor = Color.Black,
					},
					new Forms9Patch.Button {
						Text = "",
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						//Text = "toggle",
						ToggleBehavior = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						//Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						//Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
						OutlineWidth = 0,
					},

					new Label {
						Text = "Background Color, Light Theme",
						TextColor = Color.Black,
					},
					new Forms9Patch.Button {
						Text = "default",
						BackgroundColor = Color.FromHex("#E0E0E0"),
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						Orientation = StackOrientation.Vertical,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},	

					new Label {
						Text = "Shadow, Light Theme",
						TextColor = Color.Black,
					},
					new Forms9Patch.Button {
						Text = "default",
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},

					new Label {
						Text = "Shadow Background Color, Light Theme",
						TextColor = Color.Black,
					},
					new Forms9Patch.Button {
						Text = "default",
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#E0E0E0"),
						HasShadow = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},	

				},
			}, 0, 0);


			grid.Children.Add(new StackLayout {
				Padding = new Thickness(10),
				BackgroundColor = Color.FromHex("#003"),
				Children = {
					new Label {
						Text = "Default, Dark Theme",
						TextColor = Color.White,
					},
					new Forms9Patch.Button {
						Text = "default",
						DarkTheme = true,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
					},

					new Label {
						Text = "Outline, Dark Theme",
						TextColor = Color.White,
					},
					new Forms9Patch.Button {
						Text = "default",
						DarkTheme = true,
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
						OutlineWidth = 0,
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
						OutlineWidth = 0,
					},

					new Label {
						Text = "Background Color, Dark Theme",
						TextColor = Color.White,
					},
					new Forms9Patch.Button {
						Text = "default",
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						Orientation = StackOrientation.Vertical,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
					},

					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
					},

					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
					},
					new Label {
						Text = "Shadow, Dark Theme",
						TextColor = Color.White,
					},
					new Forms9Patch.Button {
						Text = "default",
						DarkTheme = true,
						HasShadow = true,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						DarkTheme = true,
						HasShadow = true,
					},
					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						DarkTheme = true,
						HasShadow = true,
					},
					new Forms9Patch.Button {
						Text = "selected disabled",
						IsEnabled = false,
						IsSelected = true,
						DarkTheme = true,
						HasShadow = true,
					},
					new Label {
						Text = "Shadow Background Color, Dark Theme",
						TextColor = Color.White,
					},
					new Forms9Patch.Button {
						Text = "default",
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
					},
					new Forms9Patch.Button {
						Text = "toggle",
						ToggleBehavior = true,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
					},

					new Forms9Patch.Button {
						Text = "disabled",
						ToggleBehavior = true,
						IsEnabled = false,
						BackgroundColor = Color.FromHex("#1194F6"),
						DarkTheme = true,
						HasShadow = true,
					},

					new Forms9Patch.Button {
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

			var sc1 = new Forms9Patch.SegmentedControl {
				Padding = 3,
				Segments = {

					new Forms9Patch.Segment {
						Text = "A",
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						Command = _trueCommand,
						CommandParameter = "sc1 A",
					},
					new Forms9Patch.Segment {
						//Text = "B",
						IsSelected = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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
				IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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


			var sc2 = new Forms9Patch.SegmentedControl {
				OutlineWidth = 0,
				Padding = 3,
				Segments = {
					seg1, seg2, seg3, seg4,
				},
			};
			sc2.SegmentSelected += OnSegmentSelected;
			sc2.SegmentTapped += OnSegmentTapped;
			sc2.SegmentLongPressing += OnSegmentLongPressing;
			sc2.SegmentLongPressed += OnSegmentLongPressed;

			var sc3 = new Forms9Patch.SegmentedControl {
				//OutlineColor = Color.Transparent,
				BackgroundColor = Color.FromHex("#E0E0E0"),
				Padding = 3,
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
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},
				},
			};
			sc3.SegmentSelected += OnSegmentSelected;
			sc3.SegmentTapped += OnSegmentTapped;
			sc3.SegmentLongPressing += OnSegmentLongPressing;
			sc3.SegmentLongPressed += OnSegmentLongPressed;

			var sc4 = new Forms9Patch.SegmentedControl {
				BackgroundColor = Color.FromHex("#E0E0E0"),
				OutlineWidth = 0,
				SeparatorWidth = 1,
				GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.None,
				Padding = 3,
				Segments = {
					new Forms9Patch.Segment {
						Text = "A none",
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						Orientation = StackOrientation.Vertical,
					},
					new Forms9Patch.Segment {
						Text = "B none",
						IsSelected = true,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
						Orientation = StackOrientation.Vertical,
					},

					new Forms9Patch.Segment {
						Text = "C none",
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},
					new Forms9Patch.Segment {
						Text = "D none",
						IsEnabled = false,
						IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
						Orientation = StackOrientation.Vertical,
					},

				},
			};
			sc4.SegmentSelected += OnSegmentSelected;
			sc4.SegmentTapped += OnSegmentTapped;
			sc4.SegmentLongPressing += OnSegmentLongPressing;
			sc4.SegmentLongPressed += OnSegmentLongPressed;

			var sc5 = new Forms9Patch.SegmentedControl {
				BackgroundColor = Color.FromHex("#E0E0E0"),
				HasShadow = true,
				//OutlineRadius = 0,
				//OutlineWidth = 0,
				Orientation = StackOrientation.Vertical,
				GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
				Padding = 3,
				Segments = {

					new Forms9Patch.Segment {
						Text = "A multi",
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
					},

					new Forms9Patch.Segment {
						Text = "B multi",
						IsSelected = true,
					},
					new Forms9Patch.Segment {
						Text = "C multi",
					},
					new Forms9Patch.Segment {
						Text = "D multi",
						IsEnabled = false,
					},

				},
			};
			sc5.SegmentSelected += OnSegmentSelected;
			sc5.SegmentTapped += OnSegmentTapped;
			sc5.SegmentLongPressing += OnSegmentLongPressing;
			sc5.SegmentLongPressed += OnSegmentLongPressed;

			var sc6 = new Forms9Patch.SegmentedControl {
				BackgroundColor = Color.FromHex("#E0E0E0"),
				HasShadow = true,
				//OutlineRadius = 0,
				OutlineWidth = 0,
				SeparatorWidth = 1,
				Orientation = StackOrientation.Vertical,
				GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.None,
				Padding = 3,
				Segments = {

					new Forms9Patch.Segment {
						Text = "A none",
					},

					new Forms9Patch.Segment {
						Text = "B none",
						IsSelected = true,
						IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
						//Orientation = StackOrientation.Vertical,
					},
					new Forms9Patch.Segment {
						Text = "C none",
					},
					new Forms9Patch.Segment {
						Text = "D none",
						IsEnabled = false,
					},

				},
			};
			sc6.SegmentSelected += OnSegmentSelected;
			sc6.SegmentTapped += OnSegmentTapped;
			sc6.SegmentLongPressing += OnSegmentLongPressing;
			sc6.SegmentLongPressed += OnSegmentLongPressed;

			#endregion


			Padding = 20;
			Content = new ScrollView { 
				Content = new StackLayout{
					Children = {
						grid,

						#region MaterialSegmentControl


						#region Light
						new StackLayout {
							BackgroundColor = Color.Lime,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							Padding = new Thickness(10),
							Children = {
								new Label {
									Text = "Default, Light",
									TextColor = Color.Black,
								},

								sc1, sc2, sc3, sc4, sc5, sc6,

							},
						},
						#endregion

								#region Dark
								new StackLayout {
									BackgroundColor = Color.FromHex("#003"),
									HorizontalOptions = LayoutOptions.FillAndExpand,
									Padding = new Thickness(10),
									Children = {
										new Label {
											Text = "Default, Dark",
											TextColor = Color.White,
										},

										new Forms9Patch.SegmentedControl {
											//OutlineColor = Color.Transparent,
											DarkTheme = true,
											Padding = 3,
											Segments = {

												new Forms9Patch.Segment {
													Text = "A",
												},
												new Forms9Patch.Segment {
													//Text = "B",
													IsSelected = true,
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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

										new Forms9Patch.SegmentedControl {
											DarkTheme = true,
											OutlineWidth = 0,
											Padding = 3,
											Segments = {

												new Forms9Patch.Segment {
													//Text = "A",
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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

										new Forms9Patch.SegmentedControl {
											DarkTheme = true,
											BackgroundColor = Color.FromHex("#1194F6"),
											Padding = 3,
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
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
												},
											},
										},

										new Forms9Patch.SegmentedControl {
											DarkTheme = true,
											BackgroundColor = Color.FromHex("#1194F6"),
											OutlineWidth = 0,
											Padding = 3,
											Segments = {
												new Forms9Patch.Segment {
													Text = "A",
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
													Orientation = StackOrientation.Vertical,
												},
												new Forms9Patch.Segment {
													Text = "B",
													IsSelected = true,
													IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
													Orientation = StackOrientation.Vertical,
												},

												new Forms9Patch.Segment {
													Text = "C",
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
												},
												new Forms9Patch.Segment {
													Text = "D",
													IsEnabled = false,
													IconImage = new Forms9Patch.Image {  Source = _infoIconSource },
													Orientation = StackOrientation.Vertical,
												},

											},
										},

										new Forms9Patch.SegmentedControl {
											DarkTheme = true,
											BackgroundColor = Color.FromHex("#1194F6"),
											HasShadow = true,
											//OutlineRadius = 0,
											//OutlineWidth = 0,
											Orientation = StackOrientation.Vertical,
											GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
											Padding = 3,
											Segments = {

												new Forms9Patch.Segment {
													Text = "A",
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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

										new Forms9Patch.SegmentedControl {
											DarkTheme = true,
											BackgroundColor = Color.FromHex("#1194F6"),
											HasShadow = true,
											//OutlineRadius = 0,
											OutlineWidth = 0,
											SeparatorWidth = 1,
											Orientation = StackOrientation.Vertical,
											GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
											Padding = 3,
											Segments = {

												new Forms9Patch.Segment {
													Text = "A",
												},

												new Forms9Patch.Segment {
													Text = "B",
													IsSelected = true,
													IconImage = new Forms9Patch.Image {  Source = _arrowIconSource },
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

						#endregion

					},
				},
			};
		}
	}
}


