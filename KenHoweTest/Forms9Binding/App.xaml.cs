using Xamarin.Forms;

namespace Forms9Binding
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new Forms9BindingPage();
			MainPage = Forms9Patch.RootPage.Create(new Forms9BindingPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
