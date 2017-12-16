using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class BubbleLayoutTestPage : ContentPage
	{
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius",typeof(double), typeof(BubbleLayoutTestPage), 10.0);
		public double CornerRadius {
			get { return (double) GetValue(CornerRadiusProperty); }
			set { SetValue (CornerRadiusProperty, value); }
		}
			
		public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("PointerLength",typeof(double), typeof(BubbleLayoutTestPage), 10.0);
		public double PointerLength {
			get { return (double) GetValue(PointerLengthProperty); }
			set { SetValue (PointerLengthProperty, value); }
		}

		public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius",typeof(double), typeof(BubbleLayoutTestPage), 5.0);
		public double PointerTipRadius {
			get { return (double) GetValue(PointerTipRadiusProperty); }
			set { SetValue (PointerTipRadiusProperty, value); }
		}

		public static readonly BindableProperty PUDPaddingProperty = BindableProperty.Create("PUDPadding",typeof(double), typeof(BubbleLayoutTestPage), 10.0);
		public double PUDPadding {
			get { return (double) GetValue(PUDPaddingProperty); }
			set { 
				SetValue (PUDPaddingProperty, value); 
				PUPadding = new Thickness (value);
			}
		}

		public static readonly BindableProperty PUPaddingProperty = BindableProperty.Create("PUPadding",typeof(Thickness), typeof(BubbleLayoutTestPage), new Thickness(10));
		public Thickness PUPadding {
			get { return (Thickness) GetValue(PUPaddingProperty); }
			set { 
				SetValue (PUPaddingProperty, value); 
			}
		}

		public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position",typeof(double), typeof(BubbleLayoutTestPage), 0.5);
		public double Position {
			get { return (double) GetValue(PositionProperty); }
			set { SetValue (PositionProperty, value); }
		}

		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(BubbleLayoutTestPage), false);
		public bool HasShadow {
			get { return (bool) GetValue(HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}

		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(BubbleLayoutTestPage), false);
		public bool ShadowInverted {
			get { return (bool) GetValue(ShadowInvertedProperty); }
			set { SetValue (ShadowInvertedProperty, value); }
		}

		public BubbleLayoutTestPage ()
		{
			BackgroundColor = Color.White;

			var shadowToggle = new Switch {
			};
			shadowToggle.SetBinding (Switch.IsToggledProperty, "HasShadow");
			shadowToggle.BindingContext = this;

			var shadowInvertedToggle = new Switch {
			};
			shadowInvertedToggle.SetBinding (Switch.IsToggledProperty, "ShadowInverted");
			shadowInvertedToggle.BindingContext = this;

			var cornerRadiusSlider = new Slider {
				Maximum = 40,
				Minimum = 0,
				Value = 10,
				HeightRequest = 20,
			};
			cornerRadiusSlider.SetBinding (Slider.ValueProperty, "CornerRadius");
			cornerRadiusSlider.BindingContext = this;

			var pointerLengthSlider = new Slider {
				Maximum = 40,
				Minimum = 0,
				Value = 10,
				HeightRequest = 20,
			};
			pointerLengthSlider.SetBinding (Slider.ValueProperty, "PointerLength");
			pointerLengthSlider.BindingContext = this;

			var pointerTipRadiusSlider = new Slider {
				Maximum = 20,
				Minimum = 0,
				Value = 5,
				HeightRequest = 20,
			};
			pointerTipRadiusSlider.SetBinding (Slider.ValueProperty, "PointerTipRadius");
			pointerTipRadiusSlider.BindingContext = this;

			var paddingSlider = new Slider {
				Maximum = 100,
				Minimum = 0,
				Value = 10,
				HeightRequest = 20,
			};
			paddingSlider.SetBinding (Slider.ValueProperty, "PUDPadding");
			paddingSlider.BindingContext = this;

			var positionSlider = new Slider {
				Maximum = 1.0,
				Minimum = 0,
				Value = 0.5,
				HeightRequest = 20,
			};
			positionSlider.SetBinding (Slider.ValueProperty, "Position");
			positionSlider.BindingContext = this;

			var lDown = new Label {
				Text = "Pizza",
				TextColor = Color.White,
				BackgroundColor = Color.Green,
			};
			lDown.SetBinding (Label.TextProperty, "CornerRadius");
			lDown.BindingContext = this;
			var bDown = new Forms9Patch.BubbleLayout {
				BackgroundColor = Color.White,
				OutlineColor = Color.Black,
				OutlineRadius = 10,
				OutlineWidth = 1,
				PointerLength = 20,
				Content =new StackLayout {
					Children = {
						new Label { Text = "Extra" },
						lDown,
						new Forms9Patch.MaterialButton { Text = "no action" },
					}
				}
			};
			bDown.SetBinding (Forms9Patch.RoundedBoxBase.OutlineRadiusProperty, "CornerRadius");
			bDown.SetBinding (Forms9Patch.BubbleLayout.PointerLengthProperty, "PointerLength");
			bDown.SetBinding (Forms9Patch.BubbleLayout.PointerTipRadiusProperty, "PointerTipRadius");
			bDown.SetBinding (Forms9Patch.BubbleLayout.PaddingProperty, "PUPadding");
			bDown.SetBinding (Forms9Patch.BubbleLayout.PointerAxialPositionProperty, "Position");
			bDown.SetBinding (Forms9Patch.BubbleLayout.HasShadowProperty, "HasShadow");
			bDown.SetBinding (Forms9Patch.RoundedBoxBase.ShadowInvertedProperty, "ShadowInverted");
			bDown.BindingContext = this;

			var lUp = new Label {
				Text = "Pizza",
				TextColor = Color.White,
				BackgroundColor = Color.Green,
			};
			lUp.SetBinding (Label.TextProperty, "CornerRadius");
			lUp.BindingContext = this;
			var bUp = new Forms9Patch.BubbleLayout {
				BackgroundColor = Color.White,
				OutlineColor = Color.Black,
				OutlineRadius = 10,
				OutlineWidth = 1,
				PointerLength = 20,
				PointerDirection = Forms9Patch.PointerDirection.Up,
				Content = new StackLayout {
					Children = {
						new Label { Text = "Extra" },
						lUp,
						new Forms9Patch.MaterialButton { Text = "no action" },
					}
				}
			};
			bUp.SetBinding (Forms9Patch.RoundedBoxBase.OutlineRadiusProperty, "CornerRadius");
			bUp.SetBinding (Forms9Patch.BubbleLayout.PointerLengthProperty, "PointerLength");
			bUp.SetBinding (Forms9Patch.BubbleLayout.PointerTipRadiusProperty, "PointerTipRadius");
			bUp.SetBinding (Forms9Patch.BubbleLayout.PaddingProperty, "PUPadding");
			bUp.SetBinding (Forms9Patch.BubbleLayout.PointerAxialPositionProperty, "Position");
			bUp.SetBinding (Forms9Patch.BubbleLayout.HasShadowProperty, "HasShadow");
			bUp.SetBinding (Forms9Patch.RoundedBoxBase.ShadowInvertedProperty, "ShadowInverted");
			bUp.BindingContext = this;

			var lLeft = new Label {
				Text = "Pizza",
				TextColor = Color.White,
				BackgroundColor = Color.Green,
			};
			lLeft.SetBinding (Label.TextProperty, "CornerRadius");
			lLeft.BindingContext = this;
			var bLeft = new Forms9Patch.BubbleLayout {
				BackgroundColor = Color.White,
				OutlineColor = Color.Black,
				OutlineRadius = 10,
				OutlineWidth = 1,
				PointerLength = 20,
				PointerDirection = Forms9Patch.PointerDirection.Left,
				Content = new StackLayout {
					Children = {
						new Label { Text = "Extra" },
						lLeft,
						new Forms9Patch.MaterialButton { Text = "no action" },
					}
				}
			};
			bLeft.SetBinding (Forms9Patch.RoundedBoxBase.OutlineRadiusProperty, "CornerRadius");
			bLeft.SetBinding (Forms9Patch.BubbleLayout.PointerLengthProperty, "PointerLength");
			bLeft.SetBinding (Forms9Patch.BubbleLayout.PointerTipRadiusProperty, "PointerTipRadius");
			bLeft.SetBinding (Forms9Patch.BubbleLayout.PaddingProperty, "PUPadding");
			bLeft.SetBinding (Forms9Patch.BubbleLayout.PointerAxialPositionProperty, "Position");
			bLeft.SetBinding (Forms9Patch.BubbleLayout.HasShadowProperty, "HasShadow");
			bLeft.SetBinding (Forms9Patch.RoundedBoxBase.ShadowInvertedProperty, "ShadowInverted");
			bLeft.BindingContext = this;

			var lRight = new Label {
				Text = "Pizza",
				TextColor = Color.White,
				BackgroundColor = Color.Green,
			};
			lRight.SetBinding (Label.TextProperty, "CornerRadius");
			lRight.BindingContext = this;
			var bRight = new Forms9Patch.BubbleLayout {
				BackgroundColor = Color.White,
				OutlineColor = Color.Black,
				OutlineRadius = 10,
				OutlineWidth = 1,
				PointerLength = 20,
				PointerDirection = Forms9Patch.PointerDirection.Right,
				Content = new StackLayout {
					Children = {
						new Label { Text = "Extra" },
						lRight,
						new Forms9Patch.MaterialButton { Text = "no action" },
					}
				}
			};
			bRight.SetBinding (Forms9Patch.RoundedBoxBase.OutlineRadiusProperty, "CornerRadius");
			bRight.SetBinding (Forms9Patch.BubbleLayout.PointerLengthProperty, "PointerLength");
			bRight.SetBinding (Forms9Patch.BubbleLayout.PointerTipRadiusProperty, "PointerTipRadius");
			bRight.SetBinding (Forms9Patch.BubbleLayout.PaddingProperty, "PUPadding");
			bRight.SetBinding (Forms9Patch.BubbleLayout.PointerAxialPositionProperty, "Position");
			bRight.SetBinding (Forms9Patch.BubbleLayout.HasShadowProperty, "HasShadow");
			bRight.SetBinding (Forms9Patch.RoundedBoxBase.ShadowInvertedProperty, "ShadowInverted");
			bRight.BindingContext = this;



			Padding = 40;
			Content = new StackLayout { 
				//BackgroundColor = Color.Blue,


				Children = {
					new Label { Text = "Corner Radius:" },
					cornerRadiusSlider,
					new Label { Text = "Pointer Length:" },
					pointerLengthSlider,
					new Label { Text = "Pointer Tip Radius:" },
					pointerTipRadiusSlider,
					new Label { Text = "Padding:" },
					paddingSlider,
					new Label { Text = "Axial Position" },
					positionSlider,
					new Label { Text = "Has Shadow" },
					shadowToggle,
					new Label { Text = "Inset Shadow" },
					shadowInvertedToggle,
					bDown,
					bUp,
					bLeft,
					bRight,
				}
			};
		}
	}
}


