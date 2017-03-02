using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class NestedBubblePopupPage : ContentPage
	{

		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius",typeof(double), typeof(NestedBubblePopupPage), 4.0);
		public double CornerRadius {
			get { return (double) GetValue(CornerRadiusProperty); }
			set { SetValue (CornerRadiusProperty, value); }
		}

		public static readonly BindableProperty PointerCornerRadiusProperty = BindableProperty.Create("PointerCornerRadius",typeof(double), typeof(NestedBubblePopupPage), 4.0);
		public double PointerCornerRadius {
			get { return (double) GetValue(PointerCornerRadiusProperty); }
			set { SetValue (PointerCornerRadiusProperty, value); }
		}

		public static readonly BindableProperty PointerLengthProperty = BindableProperty.Create("PointerLength",typeof(double), typeof(NestedBubblePopupPage), 10.0);
		public double PointerLength {
			get { return (double) GetValue(PointerLengthProperty); }
			set { SetValue (PointerLengthProperty, value); }
		}

		public static readonly BindableProperty PointerTipRadiusProperty = BindableProperty.Create("PointerTipRadius",typeof(double), typeof(NestedBubblePopupPage), 0.0);
		public double PointerTipRadius {
			get { return (double) GetValue(PointerTipRadiusProperty); }
			set { SetValue (PointerTipRadiusProperty, value); }
		}

		public static readonly BindableProperty PUDPaddingProperty = BindableProperty.Create("PUDPadding",typeof(double), typeof(NestedBubblePopupPage), 10.0);
		public double PUDPadding {
			get { return (double) GetValue(PUDPaddingProperty); }
			set { 
				SetValue (PUDPaddingProperty, value); 
				PUPadding = new Thickness (value);
			}
		}

		public static readonly BindableProperty PUPaddingProperty = BindableProperty.Create("PUPadding",typeof(Thickness), typeof(NestedBubblePopupPage), new Thickness((double)NestedBubblePopupPage.PUDPaddingProperty.DefaultValue));
		public Thickness PUPadding {
			get { return (Thickness) GetValue(PUPaddingProperty); }
			set { SetValue (PUPaddingProperty, value); }
		}

		public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position",typeof(double), typeof(NestedBubblePopupPage), 0.5);
		public double Position {
			get { return (double) GetValue(PositionProperty); }
			set { SetValue (PositionProperty, value); }
		}

		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(NestedBubblePopupPage), false);
		public bool HasShadow {
			get { return (bool) GetValue(HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}

		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(NestedBubblePopupPage), false);
		public bool ShadowInverted {
			get { return (bool) GetValue(ShadowInvertedProperty); }
			set { SetValue (ShadowInvertedProperty, value); }
		}

		public static readonly BindableProperty PointerDirectionProperty = BindableProperty.Create("PointerDirection", typeof(Forms9Patch.PointerDirection), typeof(NestedBubblePopupPage), Forms9Patch.PointerDirection.Any);
		public Forms9Patch.PointerDirection PointerDirection {
			get { return (Forms9Patch.PointerDirection) GetValue(PointerDirectionProperty); }
			set { SetValue(PointerDirectionProperty, value); }
		}

		VisualElement _lastChanged;

		public NestedBubblePopupPage ()
		{
			BackgroundColor = Color.White;
			Padding = new Thickness(20,Device.OnPlatform(20,0,0),20,20);

			var shadowToggle = new Switch ();
			shadowToggle.SetBinding (Switch.IsToggledProperty, "HasShadow");
			shadowToggle.BindingContext = this;

			var shadowInvertedToggle = new Switch ();
			shadowInvertedToggle.SetBinding (Switch.IsToggledProperty, "ShadowInverted");
			shadowInvertedToggle.BindingContext = this;

			var cornerRadiusSlider = new Slider {
				Maximum = 40,
				Minimum = 0,
				HeightRequest = 20,
			};
			cornerRadiusSlider.SetBinding (Slider.ValueProperty, "CornerRadius");
			cornerRadiusSlider.BindingContext = this;

			var pointerLengthSlider = new Slider {
				Maximum = 40,
				Minimum = 0,
				HeightRequest = 20,
			};
			pointerLengthSlider.SetBinding (Slider.ValueProperty, "PointerLength");
			pointerLengthSlider.BindingContext = this;

			var pointerTipRadiusSlider = new Slider {
				Maximum = 20,
				Minimum = 0,
				HeightRequest = 20,
			};
			pointerTipRadiusSlider.SetBinding (Slider.ValueProperty, "PointerTipRadius");
			pointerTipRadiusSlider.BindingContext = this;

			var paddingSlider = new Slider {
				Maximum = 100,
				Minimum = 0,
				HeightRequest = 20,
			};
			paddingSlider.SetBinding (Slider.ValueProperty, "PUDPadding");
			paddingSlider.BindingContext = this;

			var pointerCornerRadiusSlider = new Slider {
				Maximum = 20,
				Minimum = 0,
				HeightRequest = 20,
			};
			pointerCornerRadiusSlider.SetBinding (Slider.ValueProperty, "PointerCornerRadius");
			pointerCornerRadiusSlider.BindingContext = this;

			var leftSeg = new Forms9Patch.Segment {
				Text = "⬅︎",
				IsSelected = true,
			};
			leftSeg.Tapped += (sender, e) => _lastChanged = leftSeg.VisualElement;
			var upSeg = new Forms9Patch.Segment {
				Text = "⬆︎",
			};
			upSeg.Tapped += (sender, e) => _lastChanged = upSeg.VisualElement;
			var rightSeg = new Forms9Patch.Segment {
				Text = "➡︎",
			};
			rightSeg.Tapped += (sender, e) => _lastChanged = rightSeg.VisualElement;
			var downSeg = new Forms9Patch.Segment {
				Text = "⬇︎",
			};
			downSeg.Tapped += (sender, e) => _lastChanged = downSeg.VisualElement;

			PointerDirection = Forms9Patch.PointerDirection.Left;
			var directionSegmentControl = new Forms9Patch.MaterialSegmentedControl {
				Segments = { leftSeg, upSeg, rightSeg, downSeg, },
				GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.Multiselect,
			};
			directionSegmentControl.SegmentTapped += (sender,e) => {
				var dir = Forms9Patch.PointerDirection.None;
				dir |= (leftSeg.IsSelected ? Forms9Patch.PointerDirection.Left : Forms9Patch.PointerDirection.None);
				dir |= (rightSeg.IsSelected ? Forms9Patch.PointerDirection.Right : Forms9Patch.PointerDirection.None);
				dir |= (upSeg.IsSelected ? Forms9Patch.PointerDirection.Up : Forms9Patch.PointerDirection.None);
				dir |= (downSeg.IsSelected ? Forms9Patch.PointerDirection.Down : Forms9Patch.PointerDirection.None);
				PointerDirection = dir;
				System.Diagnostics.Debug.WriteLine("Direction changed");
			};

			var bubbleLabel = new Label {
				Text = "Forms9Patch.BubblePopup",
				TextColor = Color.Black,
				//BackgroundColor = Color.Green,
			};
			var bubbleButton = new Forms9Patch.MaterialButton {
				Text = "Close",
				//BackgroundColor = Color.Blue,
				OutlineColor = Color.Blue,
				FontColor = Color.Blue,
			};
			//bubbleLabel.SetBinding (Label.TextProperty, "CornerRadius");
			bubbleLabel.BindingContext = this;

			var bubble = new Forms9Patch.BubblePopup(this) {
				//BackgroundColor = Color.Green,
				//OutlineColor = Color.Black,
				//OutlineWidth = 1,
				PointerCornerRadius = 0,
				Content = new StackLayout {
					Children = {
						//bubbleLabel,
						new Label { Text = "Pointer Length:", FontSize=10, },
						pointerLengthSlider,
						new Label { Text = "Pointer Tip Radius:", FontSize=10, },
						pointerTipRadiusSlider,
						new Label { Text = "Corner Radius:" , FontSize=10, },
						cornerRadiusSlider,
						new Label { Text = "Pointer Corner Radius:" , FontSize=10, },
						pointerCornerRadiusSlider,
						bubbleButton,
					}
				},
			};
			bubble.SetBinding (Forms9Patch.BubblePopup.OutlineRadiusProperty, "CornerRadius");
			bubble.SetBinding (Forms9Patch.BubblePopup.PointerLengthProperty, "PointerLength");
			bubble.SetBinding (Forms9Patch.BubblePopup.PointerTipRadiusProperty, "PointerTipRadius");
			bubble.SetBinding (Forms9Patch.BubblePopup.PaddingProperty, "Padding");
			bubble.SetBinding (Forms9Patch.BubblePopup.HasShadowProperty, "HasShadow");
			bubble.SetBinding (Forms9Patch.BubblePopup.ShadowInvertedProperty, "ShadowInverted");
			bubble.SetBinding (Forms9Patch.BubblePopup.PointerDirectionProperty, "PointerDirection");
			bubble.SetBinding (Forms9Patch.BubblePopup.PointerCornerRadiusProperty, "PointerCornerRadius");
			bubble.BindingContext = this;


			bubbleButton.Tapped += (sender, e) => bubble.IsVisible = false;
			bubble.Cancelled += (sender, e) =>
			{
				var newbubble = new Forms9Patch.BubblePopup(this)
				{
					//BackgroundColor = Color.Green,
					//OutlineColor = Color.Black,
					//OutlineWidth = 1,
					Target = bubbleButton,
					PointerCornerRadius = 0,
					Content = new StackLayout
					{
						Children = {
							//bubbleLabel,
							new Label { Text = "Pointer Length:", FontSize=10, },
							pointerLengthSlider,
							new Label { Text = "Pointer Tip Radius:", FontSize=10, },
							pointerTipRadiusSlider,
							new Label { Text = "Corner Radius:" , FontSize=10, },
							cornerRadiusSlider,
							new Label { Text = "Pointer Corner Radius:" , FontSize=10, },
							pointerCornerRadiusSlider,
							bubbleButton,
						}
					},
				};

				newbubble.IsVisible = true;
			};

			var showButton = new Forms9Patch.MaterialButton {
				Text = "Show BubblePopup",
				OutlineColor = Color.Blue,
				FontColor = Color.Blue,
			};
			showButton.Tapped += (object sender, EventArgs e) => {
				bubble.Target = _lastChanged;
				bubble.IsVisible = true;
			};

			_lastChanged = leftSeg.VisualElement;

			Content = new StackLayout { 
				Children = {
					new StackLayout {
						Orientation = StackOrientation.Horizontal,
						Children = {
							new StackLayout {
								Children = {
									new Label { Text = "Has Shadow", FontSize=10, },
									shadowToggle,
								},
								HorizontalOptions = LayoutOptions.StartAndExpand,
							},
							new StackLayout {
								Children = {
									new Label { Text = "Inset Shadow", FontSize=10, },
									shadowInvertedToggle,
								},
								HorizontalOptions = LayoutOptions.EndAndExpand,
							}
						}
					},
					new Label { Text = "Padding:", FontSize=10, },
					paddingSlider,
					new Label { Text = "Pointer Direction", FontSize=10, },
					directionSegmentControl,
					showButton,
					new Label { Text = "Arrows choose the BubblePopup's allowed pointer direction.  Bubble's pointer will point at the last selected arrow-segment-button." },
				}
			};
		}
	}
}


