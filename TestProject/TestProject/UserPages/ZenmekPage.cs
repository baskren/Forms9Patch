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
	public class ZenmekPage : ContentPage
	{
		Forms9Patch.ImageButton ibStartStop;

		public ZenmekPage()
		{
			ibStartStop = new Forms9Patch.ImageButton
			{
				DefaultState = new Forms9Patch.ImageButtonState
				{
					BackgroundImage = new Forms9Patch.Image
					{
						Source = Forms9Patch.ImageSource.FromResource(BaseResource + ".Resources.button_01_default.png"),
					},
					FontColor = Color.Black,
					Text = "Start",
				},
				PressingState = new Forms9Patch.ImageButtonState
				{
					BackgroundImage = new Forms9Patch.Image
					{
						Source = Forms9Patch.ImageSource.FromResource(BaseResource + ".Resources.button_01_pressing.png"),
					},
				},
				ToggleBehavior = true,
				HeightRequest = 50,
				Alignment = TextAlignment.Center,
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
			Forms9Patch.ContentView fnpcvMain = new Forms9Patch.ContentView
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


