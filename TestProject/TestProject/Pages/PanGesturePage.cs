using System;
using Xamarin.Forms;

namespace TestProject
{
	public class PanGesturePage : ContentPage
	{
		internal Xamarin.Forms.AbsoluteLayout absoluteLayout;
		internal Xamarin.Forms.RelativeLayout relativeLayout;


		public PanGesturePage ()
		{

			var button = new Button {
				BackgroundColor = Color.Pink,
				Text = "Hello",
				BorderColor = Color.Blue,
				BorderWidth = 3,
			};
			button.Clicked += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBUTTON CLICKED!!!!");


			var buttonListener = new FormsGestures.Listener(button);
			buttonListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER DOWN"); // does not work with UIControl derived elements

			buttonListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER LONG PRESSING");
			buttonListener.Panning += (sender, e) => {
				button.TranslationX += e.DeltaDistance.X;
				button.TranslationY += e.DeltaDistance.Y;
			};
			buttonListener.Pinching += (sender, e) =>  {
				button.Scale *= e.DeltaScale;
			};
			buttonListener.Rotating += (sender, e) => {
				button.Rotation += e.DeltaAngle;
			};

			buttonListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER UP");		 // does not work with UIControl derived elements
			buttonListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER TAPPED");  // does not work with UIControl derived elements
			buttonListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER DOUBLE TAPPED"); // does not work with UIControl derived elements

			buttonListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER LONG PRESSED");
			buttonListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER PANNED");
			buttonListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER SWIPED!!!");

			buttonListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER PINCHED");
			buttonListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tLISTENER ROTATED");


			var box = new BoxView {
				BackgroundColor = Color.Green,
			};
			var boxListener = new FormsGestures.Listener (box);
			boxListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOWN"); 

			boxListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSING");
			boxListener.Panning += (sender, e) => {
				box.TranslationX += e.DeltaDistance.X;
				box.TranslationY += e.DeltaDistance.Y;
			};
			boxListener.Pinching += (sender, e) =>  {
				box.Scale *= e.DeltaScale;
			};
			boxListener.Rotating += (sender, e) => {
				box.Rotation += e.DeltaAngle;
			};

			boxListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX UP");		 
			boxListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX TAPPED");  
			boxListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX DOUBLE TAPPED");

			boxListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX LONG PRESSED");
			boxListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PANNED");
			boxListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX SWIPED!!!");

			boxListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX PINCHED");
			boxListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine ("\tBOX ROTATED");


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
				widthConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Width/8;}),
				heightConstraint: 	Constraint.RelativeToParent ((parent) => { return parent.Height/10;})
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

