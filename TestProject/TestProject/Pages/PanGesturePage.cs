using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class PanGesturePage : ContentPage
	{
		internal Xamarin.Forms.AbsoluteLayout absoluteLayout;
		internal Xamarin.Forms.RelativeLayout relativeLayout;

		void OnDown(object sender, FormsGestures.DownUpEventArgs e)
		{
		}

		public PanGesturePage ()
		{

			var button = new Button {
				BackgroundColor = Color.Pink,
				Text = "Hello",
				BorderColor = Color.Blue,
				BorderWidth = 3,
			};
			button.Clicked += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON CLICKED!!!!");


			var buttonListener = FormsGestures.Listener.For(button);
			buttonListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON DOWN [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements

			buttonListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON LONG PRESSING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
			buttonListener.Panning += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBUTTON PANNING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
				button.TranslationX += e.DeltaDistance.X;
				button.TranslationY += e.DeltaDistance.Y;
			};
			buttonListener.Pinching += (sender, e) =>  {
				System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
				button.Scale *= e.DeltaScale;
			};
			buttonListener.Rotating += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
				button.Rotation += e.DeltaAngle;
			};

			buttonListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON UP [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");		 // does not work with UIControl derived elements
			buttonListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON TAPPED #["+e.NumberOfTaps+"] [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");  // does not work with UIControl derived elements
			buttonListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements

			buttonListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON LONG PRESSED [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
			buttonListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON PANNED TotalDistance=["+e.TotalDistance+ "][" + e.Center + "][" + e.ViewPosition + "]");
			buttonListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON SWIPED!!! Velocity=["+e.VelocityX+","+e.VelocityY+ "][" + e.Center + "][" + e.ViewPosition + "]");

			buttonListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON PINCHED TotalScale=["+e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
			buttonListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON ROTATED TotalAngle=["+e.TotalAngle+ "][" + e.Center + "][" + e.ViewPosition + "]");


			var box = new BoxView {
				BackgroundColor = Color.Green,
			};
			var boxListener = FormsGestures.Listener.For(box);

			//boxListener.Down += OnDown;
			boxListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOWN [" + e.Center + "][" + e.Touches[0]+"]["+e.ViewPosition+"]"); 

			boxListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");

			boxListener.Panning += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBOX PANNING [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
				box.TranslationX += e.DeltaDistance.X;
				box.TranslationY += e.DeltaDistance.Y;
			};

			boxListener.Pinching += (sender, e) =>  {
				System.Diagnostics.Debug.WriteLine("\tBOX PINCHING [" + e.Center + "][" + e.Touches[0]+"]["+e.DeltaScale+ "][" + e.ViewPosition + "]");
				box.Scale *= e.DeltaScale;
			};
			boxListener.Rotating += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBOX ROTATING [" + e.Center + "][" + e.Touches[0]+"]["+e.DeltaAngle+ "][" + e.ViewPosition + "]");
				box.Rotation += e.DeltaAngle;
			};

			boxListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX UP [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");		 
			boxListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX TAPPED #["+e.NumberOfTaps+"] [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");  
			boxListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");

			boxListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSED [" + e.Center + "][" + e.Touches[0]+ "][" + e.ViewPosition + "]");
			boxListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PANNED TotalDistance=["+e.TotalDistance+ "][" + e.Center + "][" + e.ViewPosition + "]");
			boxListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX SWIPED!!! Velocity="+e.VelocityX+","+e.VelocityY+ "][" + e.Center + "][" + e.ViewPosition + "]");

			boxListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PINCHED TotalScale=["+e.TotalScale+ "][" + e.Center + "][" + e.ViewPosition + "]");
			boxListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX ROTATED TotalAngle["+e.TotalAngle+ "][" + e.Center + "][" + e.ViewPosition + "]");

			relativeLayout = new Xamarin.Forms.RelativeLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(0),				
				BackgroundColor = Color.Transparent,
			};
			relativeLayout.Children.Clear ();

			relativeLayout.Children.Add(button, 
				xConstraint: 		Constraint.RelativeToParent ((parent) => { return parent.X + parent.Width/2;}),
				yConstraint: 		Constraint.RelativeToParent ((parent) => { return parent.Y + parent.Height/2;}),
				widthConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Width/8;}),
				heightConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Height/10;})
			);
			relativeLayout.Children.Add(box, 
				xConstraint: 		Constraint.RelativeToParent ((parent) => { return parent.X + parent.Width/4;}),
				yConstraint: 		Constraint.RelativeToParent ((parent) => { return parent.Y + parent.Height/4;}),
				widthConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Width/3;}),
				heightConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Height/4;})
			);

			absoluteLayout = new Xamarin.Forms.AbsoluteLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				// Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 5),
				Padding = new Thickness(5,5,5,5),					// given tool bar don't need upper padding

				BackgroundColor = Color.White
			};
			absoluteLayout.Children.Add ( relativeLayout, 
				new Rectangle(0,0,1,1), AbsoluteLayoutFlags.All
			);

			Content = absoluteLayout;
		}
	}
}

