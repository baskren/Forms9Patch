using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class GestureTestPage : ContentPage
    {
        internal Xamarin.Forms.AbsoluteLayout absoluteLayout;
        internal Xamarin.Forms.RelativeLayout relativeLayout;

        void OnDown(object sender, FormsGestures.DownUpEventArgs e)
        {
        }

        public GestureTestPage()
        {
            #region Page

            //var pageListener = FormsGestures.Listener.For(this);

            //pageListener.LongPressing += (s, e) => System.Diagnostics.Debug.WriteLine("\tPAGE LONG PRESSING");
            //pageListener.LongPressed += (s, e) => System.Diagnostics.Debug.WriteLine("\tPAGE LONG PRESSED");


            #endregion

            #region Xamarin.Forms.Button
            var button = new Button
            {
                BackgroundColor = Color.Pink,
                Text = "Hello",
                BorderColor = Color.Blue,
                BorderWidth = 3,
            };
            button.Clicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON CLICKED!!!!");


            var buttonListener = FormsGestures.Listener.For(button);

            buttonListener.Down += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON DOWN [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements
            buttonListener.RightClicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            buttonListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            buttonListener.Panning += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                button.TranslationX += e.DeltaDistance.X;
                button.TranslationY += e.DeltaDistance.Y;
            };
            buttonListener.Pinching += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                button.Scale *= e.DeltaScale;
            };
            buttonListener.Rotating += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                button.Rotation += e.DeltaAngle;
            };

            buttonListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");      // does not work with UIControl derived elements
            buttonListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");  // does not work with UIControl derived elements
            buttonListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements

            buttonListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            buttonListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
            buttonListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON SWIPED!!! Velocity=[" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");

            buttonListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
            buttonListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATED TotalAngle=[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");

            #endregion


            #region Forms9Patch.Frame
            var frame = new Forms9Patch.Frame
            {
                BackgroundColor = Color.Orange,
                WidthRequest = 70,
                HeightRequest = 70,
                HasShadow = true,
                OutlineColor = Color.Green,
                OutlineRadius = 0,
                OutlineWidth = 1,
                Content = new Forms9Patch.Label {  Text = "pizza", TextColor = Color.White, BackgroundColor = Color.Blue, HorizontalOptions=LayoutOptions.Start, VerticalOptions = LayoutOptions.Start}
            };
            var frameListener = FormsGestures.Listener.For(frame);

            frameListener.Down += (sender, e) =>
            {
                e.Handled = true;
                System.Diagnostics.Debug.WriteLine("\tFRAME DOWN [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            frameListener.RightClicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            frameListener.Panning += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                frame.TranslationX += e.DeltaDistance.X;
                frame.TranslationY += e.DeltaDistance.Y;
            };

            frameListener.Pinching += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME PINCHING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaScale + "][" + e.ViewPosition + "]");
                frame.Scale *= e.DeltaScale;
            };
            frameListener.Rotating += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME ROTATING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaAngle + "][" + e.ViewPosition + "]");
                frame.Rotation += e.DeltaAngle;
            };

            frameListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            frameListener.Tapping += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME TAPPING #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            frameListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            frameListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            frameListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            frameListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
            frameListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME SWIPED!!! Velocity=" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");

            frameListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
            frameListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME ROTATED TotalAngle[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");
            #endregion


            #region Xamarin.Forms.BoxView
            var box = new BoxView
            {
                BackgroundColor = Color.Green,
            };
            var boxListener = FormsGestures.Listener.For(box);

            //boxListener.Down += OnDown;
            boxListener.Down += (sender, e) =>
            {
                e.Handled = true;
                System.Diagnostics.Debug.WriteLine("\tBOX DOWN [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            boxListener.LongPressing += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            boxListener.RightClicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            boxListener.Panning += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                box.TranslationX += e.DeltaDistance.X;
                box.TranslationY += e.DeltaDistance.Y;
            };

            boxListener.Pinching += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX PINCHING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaScale + "][" + e.ViewPosition + "]");
                box.Scale *= e.DeltaScale;
            };
            boxListener.Rotating += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX ROTATING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaAngle + "][" + e.ViewPosition + "]");
                box.Rotation += e.DeltaAngle;
            };

            boxListener.Up += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            boxListener.Tapping += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX TAPPING #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            boxListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            boxListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            boxListener.LongPressed += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            boxListener.Panned += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
            boxListener.Swiped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX SWIPED!!! Velocity=" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");

            boxListener.Pinched += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
            boxListener.Rotated += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX ROTATED TotalAngle[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");

            #endregion


            #region Looking for multiple taps
            var tapBox = new BoxView
            {
                BackgroundColor = Color.Blue,
            };
            var tapBoxListener = FormsGestures.Listener.For(tapBox);

            tapBoxListener.Tapping += (sender, e) => System.Diagnostics.Debug.WriteLine("\tTAPBOX TAPPING #[" + e.NumberOfTaps + "]");
            tapBoxListener.Tapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tTAPBOX TAPPED #[" + e.NumberOfTaps + "]");
            tapBoxListener.DoubleTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("\tTAPBOX DOUBLE TAPPED #[" + e.NumberOfTaps + "]");

            #endregion


            #region RelativeLayout
            relativeLayout = new Xamarin.Forms.RelativeLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0),
                BackgroundColor = Color.Transparent,
            };
            relativeLayout.Children.Clear();

            relativeLayout.Children.Add(button,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.X + parent.Width * 3 / 4; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height * 3 / 4; }),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 3; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 3; })
            );
            relativeLayout.Children.Add(box,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.X + parent.Width / 4; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height / 4; }),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 2; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 2; })
            );
            relativeLayout.Children.Add(tapBox,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.X + parent.Width / 8; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height / 8; }),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 16; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Width / 16; })
            );
            relativeLayout.Children.Add(frame,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.X + parent.Width * 1/4 ; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return parent.Y + parent.Height / 8; })
            );

            #endregion


            #region AbsoluteLayout
            absoluteLayout = new Xamarin.Forms.AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                // Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 5),
                Padding = new Thickness(5, 5, 5, 5),                    // given tool bar don't need upper padding

                BackgroundColor = Color.White
            };
            absoluteLayout.Children.Add(relativeLayout,
                new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All
            );

            #endregion

            Content = absoluteLayout;
        }
    }
}

