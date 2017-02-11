// /*******************************************************************
//  *
//  * AsafTest.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;
using NumericalMethods;

namespace AsafTest
{
	class Page3 : ContentPage //BaseContentPage
	{
		Button b;
		static int _instances = 0;
		int _instance;

		~Page3()
		{
			System.Diagnostics.Debug.WriteLine("Page3[" + _instance + "].~Page3();");
			Stop();
		}

		void Stop()
		{
			//throw new Exception();
			//System.Diagnostics.Debug.WriteLine("{0}[{1}] ", PCL.Utils.ReflectionExtensions.CallerString(), GetType());
			System.Diagnostics.Debug.WriteLine("Page3[" + _instance + "].Stop()");
		}

		public void moveToPage()
		{
			//l.GestureRecognizers.(0);
			System.Diagnostics.Debug.WriteLine("Page3[" + _instance + "].moveToPage()");
			App.Current.MainPage.Navigation.InsertPageBefore(new Page1(), this);
			App.Current.MainPage.Navigation.PopAsync();
			GC.Collect();
		}

		public Page3()
		{
			_instance = _instances++;
			System.Diagnostics.Debug.WriteLine("Page3[" + _instance + "]..ctr()");
			b = new Button();
			b.Clicked += B_Clicked; ;
			b.Text = "33333";
			Content = b;
		}

		private void B_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Page3[" + _instance + "].B_Clicked()");
			moveToPage();
		}
	}

	class Page1 : ContentPage //BaseContentPage
	{
		Button b;
		static int _instances = 0;
		int _instance;

		~Page1()
		{
			System.Diagnostics.Debug.WriteLine("Page1[" + _instance + "].~Page1()");
			Stop();
		}

		void Stop()
		{
			System.Diagnostics.Debug.WriteLine("Page1[" + _instance + "].Stop()");
			throw new Exception();
		}

		public void moveToPage()
		{
			//l.GestureRecognizers.(0);
			System.Diagnostics.Debug.WriteLine("Page1[" + _instance + "].moveToPage()");
			App.Current.MainPage.Navigation.InsertPageBefore(new Page3(), this);
			App.Current.MainPage.Navigation.PopAsync();
			s1.BackgroundImage = null;
			s1 = null;
			GC.Collect();
		}

		Forms9Patch.StackLayout s1;

		public Page1()
		{
			_instance = _instances++;
			System.Diagnostics.Debug.WriteLine("Page1[" + _instance + "]..ctr()");
			b = new Button();
			b.Clicked += B_Clicked; ;
			b.Text = "111111";

			s1 = new Forms9Patch.StackLayout();

			Forms9Patch.Image image = new Forms9Patch.Image();
			image.Source = Forms9Patch.ImageSource.FromMultiResource("AsafTest.Green");
			image.Fill = Forms9Patch.Fill.Fill;

			// ben - this line cause the problem - if you remove it the destructor is called and 
			// the app exits, else ther constructor isn't called
			s1.BackgroundImage = image;


			s1.Children.Add(b);

			Content = s1;
		}

		private void B_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Page1[" + _instance + "].B_Clicked()");
			moveToPage();
		}
	}


	public class App : Application
	{
		public App()
		{
			System.Diagnostics.Debug.WriteLine("App..ctr()");
			int a = System.GC.MaxGeneration;
			// The root page of your application
			MainPage = new NavigationPage(new Page1());
		}
	}
}
