using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class ModalPopupTestPage : MasterDetailPage
	{
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius",typeof(double), typeof(ModalPopupTestPage), 4.0);
		public double CornerRadius {
			get { return (double) GetValue(CornerRadiusProperty); }
			set { SetValue (CornerRadiusProperty, value); }
		}

		public static readonly BindableProperty PUDPaddingProperty = BindableProperty.Create("PUDPadding",typeof(double), typeof(ModalPopupTestPage), 10.0);
		public double PUDPadding {
			get { return (double) GetValue(PUDPaddingProperty); }
			set { 
				SetValue (PUDPaddingProperty, value); 
				PUPadding = new Thickness (value);
			}
		}

		public static readonly BindableProperty PUPaddingProperty = BindableProperty.Create("PUPadding",typeof(Thickness), typeof(ModalPopupTestPage), new Thickness((double)ModalPopupTestPage.PUDPaddingProperty.DefaultValue));
		public Thickness PUPadding {
			get { return (Thickness) GetValue(PUPaddingProperty); }
			set { SetValue (PUPaddingProperty, value); }
		}

		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(ModalPopupTestPage), false);
		public bool HasShadow {
			get { return (bool) GetValue(HasShadowProperty); }
			set { SetValue (HasShadowProperty, value); }
		}

		public static readonly BindableProperty ShadowInvertedProperty = BindableProperty.Create("ShadowInverted", typeof(bool), typeof(ModalPopupTestPage), false);
		public bool ShadowInverted {
			get { return (bool) GetValue(ShadowInvertedProperty); }
			set { SetValue (ShadowInvertedProperty, value); }
		}

		public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create("OutlineWidth", typeof(float), typeof(ModalPopupTestPage), 0f);
		public float OutlineWidth {
			get { return (float) GetValue(OutlineWidthProperty); }
			set { SetValue (OutlineWidthProperty, value); }
		}

		public static readonly BindableProperty CancelOnBackgroundTouchProperty = BindableProperty.Create("CancelOnBackgroundTouch", typeof(bool), typeof(ModalPopupTestPage), true);
		public bool CancelOnBackgroundTouch
		{
			get { return (bool)GetValue(CancelOnBackgroundTouchProperty); }
			set { SetValue(CancelOnBackgroundTouchProperty, value); }
		}


		public ModalPopupTestPage ()
		{
			BackgroundColor = Color.White;
			Padding = new Thickness(20,Device.RuntimePlatform==Device.iOS ? 20:0,20,20);


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

			var paddingSlider = new Slider {
				Maximum = 100,
				Minimum = 0,
				HeightRequest = 20,
			};
			paddingSlider.SetBinding (Slider.ValueProperty, "PUDPadding");
			paddingSlider.BindingContext = this;

			//var listener = new FormsOrientation.Listener ();

			var hidePopupButton = new Forms9Patch.Button {
				Text = "DONE",
				BackgroundColor = Color.Blue,
				DarkTheme = true,
			};
			var showPopupButton = new Forms9Patch.Button {
				Text = "Show Modal popup",
				BackgroundColor = Color.Blue,
				DarkTheme = true,
				WidthRequest = 150,
				HorizontalOptions = LayoutOptions.Center,
			};

			var cancelOnBackgroundTouchButton = new Forms9Patch.Button
			{
				Text = "Cancel on Background touch",
				ToggleBehavior = true,
				IsSelected = true
			};
			cancelOnBackgroundTouchButton.SetBinding(Forms9Patch.Button.IsSelectedProperty,"CancelOnBackgroundTouch");
			cancelOnBackgroundTouchButton.BindingContext = this;


			var blackSegment = new Forms9Patch.Segment { HtmlText = "<font color=\"#000000\">Black</font>"};
			var redSegment = new Forms9Patch.Segment { HtmlText = "<font color=\"#FF0000\">Red</font>" };
			var greenSegment = new Forms9Patch.Segment { HtmlText = "<font color=\"#00FF00\">Green</font>" };
			var blueSegment = new Forms9Patch.Segment { HtmlText = "<font color=\"#0000FF\">Blue</font>" };

			var overlayColorSelector = new Forms9Patch.SegmentedControl
			{
				Segments = {
					blackSegment,
					redSegment,
					greenSegment,
					blueSegment,
				},
				BackgroundColor = Color.White,
				FontSize = 10,
			};

			var modal = new Forms9Patch.ModalPopup() {
				Content = new StackLayout {
					Children = {
						new Label { 
							Text = "Hello Modal popup!", 
							TextColor = Color.Black,},
						new Label { Text = "Padding:", FontSize=10, },
						paddingSlider,
						new Label { Text = "Corner Radius:" , FontSize=10, },
						cornerRadiusSlider,
						hidePopupButton,
						new Label { Text = "PageOverlayColor:" , FontSize=10, },
						overlayColorSelector
					},
					//BackgroundColor = Color.FromRgb(100,100,100),
					//Padding = 20,
				},
				OutlineRadius = 4,
				OutlineWidth = 1,
				OutlineColor = Color.Black,
				BackgroundColor = Color.Aqua,
				HasShadow = true,
				HeightRequest = 200,
				WidthRequest = 200,
				Margin = 0,
			};
			modal.SetBinding (Forms9Patch.PopupBase.OutlineRadiusProperty, "CornerRadius");
			modal.SetBinding (Forms9Patch.PopupBase.OutlineWidthProperty, "OutlineWidth");
			modal.SetBinding (Forms9Patch.PopupBase.PaddingProperty, "PUPadding");
			modal.SetBinding (Forms9Patch.PopupBase.HasShadowProperty, "HasShadow");
			modal.SetBinding (Forms9Patch.PopupBase.ShadowInvertedProperty, "ShadowInverted");
			modal.SetBinding (Forms9Patch.PopupBase.CancelOnPageOverlayTouchProperty, "CancelOnBackgroundTouch");
			modal.BindingContext = this;


			showPopupButton.Tapped += (sender, e) => {
				modal.IsVisible = true;
			};
			hidePopupButton.Tapped += (sender, e) => {
				modal.IsVisible = false;
				System.Diagnostics.Debug.WriteLine("button "+showPopupButton.Text);
			};

			blackSegment.Selected += (sender, e) => { modal.PageOverlayColor = Color.FromRgba(0, 0, 0, 128);};
			redSegment.Selected += (sender, e) => { modal.PageOverlayColor = Color.FromRgba(255, 0, 0, 128); };
			greenSegment.Selected += (sender, e) => { modal.PageOverlayColor = Color.FromRgba(0, 255, 0, 128); };
			blueSegment.Selected += (sender, e) => { modal.PageOverlayColor = Color.FromRgba(0, 0, 255, 128); };

			Detail = new ContentPage {
				BackgroundColor = Color.White,
				Title = "PopupTestPage - Detail",
				Content = new StackLayout { 
					Children = {
						new Label { 
							Text = "Hello ContentPage",
							TextColor = Color.Black,
						},
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
						cancelOnBackgroundTouchButton,
						showPopupButton,
					},
					Padding = 20,
				},
			};	

			Master = new ContentPage {
				Title = "PopupTestPage - Master",
				Content = new Label {
					Text = "Master Page",
				},
				BackgroundColor = Color.Gray,
			};
		}
	}
}


