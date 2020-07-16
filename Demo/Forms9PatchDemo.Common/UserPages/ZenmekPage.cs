// /*******************************************************************
//  *
//  * Zenmek.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ZenmekPage : ContentPage
    {
        readonly Forms9Patch.StateButton ibStartStop;

        public ZenmekPage()
        {
            ibStartStop = new Forms9Patch.StateButton
            {
                DefaultState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromResource(BaseResource + ".Resources.button_01_default.png"),
                    },
                    TextColor = Color.Black,
                    Text = "Start",
                },
                PressingState = new Forms9Patch.ButtonState
                {
                    BackgroundImage = new Forms9Patch.Image
                    {
                        Source = Forms9Patch.ImageSource.FromResource(BaseResource + ".Resources.button_01_pressing.png"),
                    },
                },
                ToggleBehavior = true,
                HeightRequest = 50,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            StackLayout slMain = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                    {
                        ibStartStop
                    }
            };

            //ContentView fnpcvMain = new ContentView
            Forms9Patch.Frame fnpcvMain = new Forms9Patch.Frame
            {
                Content = slMain,
            };

            //Issue happens with this:
            Content = fnpcvMain;

            //It works fine with this:
            //Content = slMain;

        }

        private string _baseResource = null;
        public string BaseResource
        {
            get
            {
                if (_baseResource == null)
                {
                    _baseResource = "Forms9PatchDemo";
                    //_baseResource = Assembly.GetExecutingAssembly().FullName.Split(',').FirstOrDefault();
                }
                return _baseResource;
            }
        }
    }
}


