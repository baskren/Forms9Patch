using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class GestureTestPage : ContentPage
    {
        internal Xamarin.Forms.AbsoluteLayout absoluteLayout;
        internal Xamarin.Forms.RelativeLayout relativeLayout;



        public GestureTestPage()
        {
            #region Page

            //var pageListener = FormsGestures.Listener.For(this);

            //pageListener.LongPressing += (s, e) => System.Diagnostics.Debug.WriteLine("\tPAGE LONG PRESSING");
            //pageListener.LongPressed += (s, e) => System.Diagnostics.Debug.WriteLine("\tPAGE LONG PRESSED");


            #endregion


            #region Delta vs Total Switch
            var totalSwitch = new Xamarin.Forms.Switch();
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

            buttonListener.Down += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON DOWN [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements
            };
            buttonListener.Up += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");      // does not work with UIControl derived elements
            };
            buttonListener.Tapping += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON TAPPING #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            buttonListener.Tapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");  // does not work with UIControl derived elements
            };
            buttonListener.DoubleTapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]"); // does not work with UIControl derived elements
            };
            buttonListener.LongPressing += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            buttonListener.LongPressed += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };

            buttonListener.RightClicked += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };

            buttonListener.Panning += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                if (totalSwitch.IsToggled)
                {
                    button.TranslationX = e.TotalDistance.X;
                    button.TranslationY = e.TotalDistance.Y;
                }
                else
                {
                    button.TranslationX += e.DeltaDistance.X;
                    button.TranslationY += e.DeltaDistance.Y;
                }
            };
            buttonListener.Panned += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
                if (totalSwitch.IsToggled)
                {
                    button.TranslationX = 0;
                    button.TranslationY = 0;
                }
            };
            buttonListener.Swiped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON SWIPED!!! Velocity=[" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");
            };

            buttonListener.Pinching += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                button.Scale *= e.DeltaScale;
            };
            buttonListener.Pinched += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
            };
            buttonListener.Rotating += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                button.Rotation += e.DeltaAngle;
            };
            buttonListener.Rotated += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBUTTON ROTATED TotalAngle=[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");
            };
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
            frameListener.Up += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.Tapping += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME TAPPING #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.Tapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.DoubleTapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.LongPressing += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };
            frameListener.LongPressed += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
            };

            frameListener.RightClicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tFRAME RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            frameListener.Panning += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                if (totalSwitch.IsToggled)
                {
                    frame.TranslationX = e.TotalDistance.X;
                    frame.TranslationY = e.TotalDistance.Y;
                }
                else
                {
                    frame.TranslationX += e.DeltaDistance.X;
                    frame.TranslationY += e.DeltaDistance.Y;
                }
            };
            frameListener.Panned += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
                if (totalSwitch.IsToggled)
                {
                    frame.TranslationX = 0;
                    frame.TranslationY = 0;
                }
            };
            frameListener.Swiped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME SWIPED!!! Velocity=" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");
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
            frameListener.Pinched += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
            };
            frameListener.Rotated += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tFRAME ROTATED TotalAngle[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");
            };

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
                //System.Diagnostics.Debug.WriteLine("\tBOX DOWN [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Red);
            };
            boxListener.Up += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX UP [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Blue);
            };

            boxListener.Tapping += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX TAPPING #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Yellow);
            };
            boxListener.Tapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX TAPPED #[" + e.NumberOfTaps + "] [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Green);
            };
            boxListener.DoubleTapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX DOUBLE TAPPED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Orange);
            };
            boxListener.LongPressing += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX LONG PRESSING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Magenta);
            };
            boxListener.LongPressed += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX LONG PRESSED [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Purple);
            };


            boxListener.RightClicked += (sender, e) => System.Diagnostics.Debug.WriteLine("\tBOX RIGHT CLICK[" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");

            boxListener.Panning += (sender, e) =>
            {
                //System.Diagnostics.Debug.WriteLine("\tBOX PANNING [" + e.Center + "][" + e.Touches[0] + "][" + e.ViewPosition + "]");
                if (totalSwitch.IsToggled)
                {
                    box.TranslationX = e.TotalDistance.X;
                    box.TranslationY = e.TotalDistance.Y;
                }
                else
                {
                    box.TranslationX += e.DeltaDistance.X;
                    box.TranslationY += e.DeltaDistance.Y;
                }
                ShowTouches(box, e, Color.Pink);
            };
            boxListener.Panned += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX PANNED TotalDistance=[" + e.TotalDistance + "][" + e.Center + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.MistyRose);
                if (totalSwitch.IsToggled)
                {
                    box.TranslationX = 0;
                    box.TranslationY = 0;
                }
            };
            boxListener.Swiped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX SWIPED!!! Velocity=" + e.VelocityX + "," + e.VelocityY + "][" + e.Center + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.Cyan);
            };

            boxListener.Pinching += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX PINCHING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaScale + "][" + e.ViewPosition + "]");
                box.Scale *= e.DeltaScale;
                ShowTouches(box, e, Color.LightSeaGreen);
            };
            boxListener.Pinched += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX PINCHED TotalScale=[" + e.TotalScale + "][" + e.Center + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.MintCream);
            };
            boxListener.Rotating += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX ROTATING [" + e.Center + "][" + e.Touches[0] + "][" + e.DeltaAngle + "][" + e.ViewPosition + "]");
                box.Rotation += e.DeltaAngle;
                ShowTouches(box, e, Color.SandyBrown);
            };
            boxListener.Rotated += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("\tBOX ROTATED TotalAngle[" + e.TotalAngle + "][" + e.Center + "][" + e.ViewPosition + "]");
                ShowTouches(box, e, Color.RosyBrown);
            };
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

            relativeLayout.Children.Add(totalSwitch,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.X; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return parent.Y; })
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

            totalSwitch.Toggled += (s, e) =>
            {
                box.TranslationX = 0;
                box.TranslationY = 0;
                frame.TranslationX = 0;
                frame.TranslationY = 0;
                button.TranslationX = 0;
                button.TranslationY = 0;
            };
        }

        List<BoxView> _touchIndicators = new List<BoxView>();
        public void ShowTouches(View element, FormsGestures.BaseGestureEventArgs e, Color color)
        {
            foreach (var box in _touchIndicators)
                relativeLayout.Children.Remove(box);

            //for (var i = 0; i < e.Touches.Length; i++)
            //    ShowBoxRelativeTo(element, e.Touches[i], color);
        }

        void ShowBoxRelativeTo(View source, Point point, Color color)
        {
            var box = new BoxView
            {
                HeightRequest = 40,
                WidthRequest = 40,
                Color = color,
                TranslationX = -20,
                TranslationY = -20
            };
            _touchIndicators.Add(box);
            relativeLayout.Children.Add(box, xConstraint: Constraint.RelativeToView(source, (layout, view) => 0.0), yConstraint: Constraint.RelativeToView(source, (layout, view) => 0.0));
        }
    }
}

