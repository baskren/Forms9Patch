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
			buttonListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON DOWN ["+e.Touches[0]+"]"); // does not work with UIControl derived elements

			buttonListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON LONG PRESSING ["+e.Touches[0]+"]");
			buttonListener.Panning += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBUTTON PANNING ["+e.Touches[0]+"]");
				button.TranslationX += e.DeltaDistance.X;
				button.TranslationY += e.DeltaDistance.Y;
			};
			buttonListener.Pinching += (sender, e) =>  {
				System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHING ["+e.Touches[0]+"]");
				button.Scale *= e.DeltaScale;
			};
			buttonListener.Rotating += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATING ["+e.Touches[0]+"]");
				button.Rotation += e.DeltaAngle;
			};

			buttonListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON UP ["+e.Touches[0]+"]");		 // does not work with UIControl derived elements
			buttonListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON TAPPED ["+e.Touches[0]+"]");  // does not work with UIControl derived elements
			buttonListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON DOUBLE TAPPED ["+e.Touches[0]+"]"); // does not work with UIControl derived elements

			buttonListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON LONG PRESSED ["+e.Touches[0]+"]");
			buttonListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON PANNED TotalDistance=["+e.TotalDistance+"]");
			buttonListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON SWIPED!!! Velocity=["+e.VelocityX+","+e.VelocityY+"]");

			buttonListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON PINCHED TotalScale=["+e.TotalScale +"]");
			buttonListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON ROTATED TotalAngle=["+e.TotalAngle+"]");


			var box = new BoxView {
				BackgroundColor = Color.Green,
			};
			var boxListener = FormsGestures.Listener.For(box);

			//boxListener.Down += OnDown;
			boxListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOWN ["+e.Touches[0]+"]"); 

			boxListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSING ["+e.Touches[0]+"]");

			boxListener.Panning += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBOX PANNING ["+e.Touches[0]+"]");
				box.TranslationX += e.DeltaDistance.X;
				box.TranslationY += e.DeltaDistance.Y;
			};

			boxListener.Pinching += (sender, e) =>  {
				System.Diagnostics.Debug.WriteLine("\tBOX PINCHING ["+e.Touches[0]+"]["+e.DeltaScale+"]");
				box.Scale *= e.DeltaScale;
			};
			boxListener.Rotating += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("\tBOX ROTATING ["+e.Touches[0]+"]["+e.DeltaAngle+"]");
				box.Rotation += e.DeltaAngle;
			};

			boxListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX UP ["+e.Touches[0]+"]");		 
			boxListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX TAPPED ["+e.Touches[0]+"]");  
			boxListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOUBLE TAPPED ["+e.Touches[0]+"]");

			boxListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSED ["+e.Touches[0]+"]");
			boxListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PANNED TotalDistance=["+e.TotalDistance+"]");
			boxListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX SWIPED!!! Velocity="+e.VelocityX+","+e.VelocityY+"]");

			boxListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PINCHED TotalScale=["+e.TotalScale+"]");
			boxListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX ROTATED TotalAngle["+e.TotalAngle+"]");

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

