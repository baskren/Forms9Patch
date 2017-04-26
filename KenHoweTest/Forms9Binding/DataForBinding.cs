using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Forms9Binding
{
	public enum GameType { QuickPlay, TeamPlay };
	public enum GameStatePlayerStatus { Playing, Resting };
	public enum GameStateMultiPlayerType { Individual, TwoTeams };

	public class GameStatePlayer : Xamarin.Forms.BindableObject
	{
		public string Name { get; set; }
		public string Avatar { get; set; }
		public GameStatePlayerStatus Status { get; set; }
		public int Team { get; set; }
		//public int FontSize { get; set; }

		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(int), typeof(GameStatePlayer), default(int));
		public int FontSize
		{
			get { return (int)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		public bool IsPlaying
		{
			get
			{
				return Status == GameStatePlayerStatus.Playing;
			}
			set
			{
				Status = GameStatePlayerStatus.Playing;
			}
		}

		public bool IsResting
		{
			get
			{
				return Status == GameStatePlayerStatus.Resting;
			}
			set
			{
				Status = GameStatePlayerStatus.Resting;
			}
		}
		public string PlayerButton
		{
			get
			{
				return Avatar + " " + Name + FontSize.ToString();
			}
		}

		public GameStatePlayer()
		{
			Status = GameStatePlayerStatus.Playing;
		}

		public GameStatePlayer(string name, string avatar)
		{
			Name = name;
			Avatar = avatar;
			Status = GameStatePlayerStatus.Playing;
		}
	}

	public class GameState
	{
		public static string[] PlayerAvatars = new string[] { "\ud83d\ude00" };
		public ObservableCollection<GameStatePlayer> multiPlayers;

		public GameState()
		{
			multiPlayers = new ObservableCollection<GameStatePlayer>();

			// we always add a dummy player for the UI
			this.AddPlayer();
			this.AddPlayer();
			this.AddPlayer();
		}

		public GameType type { get; set; }
		private GameStateMultiPlayerType _multiPlayerType;


		public GameStatePlayer AddPlayer()
		{
			GameStatePlayer p = new GameStatePlayer("Player " + (multiPlayers.Count + 1).ToString(), PlayerAvatars[0]);
			p.FontSize = 12 + (multiPlayers.Count * 2);
			multiPlayers.Add(p);

			return p;
		}
	}

}
