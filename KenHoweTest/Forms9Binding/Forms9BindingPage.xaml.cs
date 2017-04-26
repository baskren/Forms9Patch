using Xamarin.Forms;
using Forms9Patch;

namespace Forms9Binding
{
	public partial class Forms9BindingPage : ContentPage
	{
		private GameState state;

		public Forms9BindingPage()
		{
			InitializeComponent();

			state = new GameState();

			state.AddPlayer().Status = GameStatePlayerStatus.Resting;

			tblPlayers.ItemsSource = state.multiPlayers;
		}

		void Handle_Tapped(object sender, System.EventArgs e)
		{
			btnEnableder.IsEnabled = !btnEnableder.IsEnabled;
		}
	}
}
