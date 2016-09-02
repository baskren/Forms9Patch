using System;

using Xamarin.Forms;

namespace TestProject
{
	public class HtmlButtonsPage : ContentPage
	{
		public HtmlButtonsPage ()
		{
			Padding = 20;
			BackgroundColor = Color.White;

			#region Material Button
			var mb1 = new Forms9Patch.MaterialButton {
				HtmlText = "<i>Markup</i> button <font size=\"4\" face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font>",
				//Text = "Pizza",
				BackgroundColor = Color.FromRgb(200,200,200),
				FontColor = Color.Blue,
				HasShadow = true,
				StickyBehavior = true,
			};
			mb1.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("Tapped");
			mb1.Selected += (sender, e) => System.Diagnostics.Debug.WriteLine ("Selected");
			mb1.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("Long Pressing");
			mb1.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("Long Pressed");
			#endregion

			#region Segmented Button
			var sc1 = new Forms9Patch.MaterialSegmentedControl {
				HasShadow = true,
				BackgroundColor = Color.FromRgb(200,200,200),
				//FontColor = Color.Blue,
				Segments = {

					new Forms9Patch.Segment {
						HtmlText = "<font size=\"4\" face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> Cart",
					},
					new Forms9Patch.Segment {
						HtmlText = "<font size=\"4\" face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> Pay",
					},
					new Forms9Patch.Segment {
						HtmlText = "<font size=\"4\" face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> Ship",
					},
					new Forms9Patch.Segment {
						HtmlText = "<font size=\"4\" face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> Email",
					},
				},
			};
			sc1.SegmentSelected += OnSegmentSelected;
			sc1.SegmentTapped += OnSegmentTapped;
			sc1.SegmentLongPressing += OnSegmentLongPressing;
			sc1.SegmentLongPressed += OnSegmentLongPressed;
			#endregion

			#region Image Button
			var ib1 = new Forms9Patch.ImageButton {
				DefaultState = new Forms9Patch.ImageButtonState {
					BackgroundImage = new Forms9Patch.Image {
						Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.button"),
					},
					Image = new Image {
						Source = ImageSource.FromFile("five.png"),
					},
					FontColor = Color.White,
					//Text = "Sticky w/ SelectedState",
					HtmlText = "<b>Sticky</b> with <i>SelectedState</i>",
				},
				SelectedState = new Forms9Patch.ImageButtonState {
					BackgroundImage = new Forms9Patch.Image {
						Source = Forms9Patch.ImageSource.FromMultiResource ("TestProject.Resources.image"),
					},
					FontColor = Color.Red,
					//Text = "Selected",
					HtmlText = "<b><i>Selected</i></b>",

				},
				StickyBehavior = true,
				HeightRequest = 50,
				Alignment = TextAlignment.Start,
			};
			ib1.Tapped += OnImageButtonTapped;
			ib1.Selected += OnImageButtonSelected;
			ib1.LongPressing += OnImageButtonLongPressing;
			ib1.LongPressed += OnImageButtonLongPressed;
			#endregion

			Content = new StackLayout { 
				Children = {
					new Forms9Patch.Label { HtmlText = "<b>HTML Buttons</b>" },
					mb1,
					sc1,
					ib1,
				}
			};
		}

		#region Touch event responders
		static void OnSegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Segment[" + e.Index + "] Text=["+e.Segment.HtmlText+"]");
		}

		static void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Segment[" + e.Index + "] Text=["+e.Segment.HtmlText+"]");
		}

		static void OnSegmentLongPressing(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Segment[" + e.Index + "] Text=["+e.Segment.HtmlText+"]");
		}

		static void OnSegmentLongPressed(object sender, Forms9Patch.SegmentedControlEventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Segment[" + e.Index + "] Text=["+e.Segment.HtmlText+"]");
		}

		static void OnImageButtonTapped(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Tapped Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		static void OnImageButtonSelected(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("Selected Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		static void OnImageButtonLongPressing(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressing Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}

		static void OnImageButtonLongPressed(object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("LongPressed Button Text=["+((Forms9Patch.ImageButton)sender).Text+"]");
		}
		#endregion


	}
}


