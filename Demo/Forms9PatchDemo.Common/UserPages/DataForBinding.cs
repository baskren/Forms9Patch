using System;
using System.Collections.ObjectModel;

namespace Forms9PatchDemo
{
    public enum GameType { QuickPlay, TeamPlay };
    public enum GameStatePlayerStatus { Playing, Resting };
    public enum GameStateMultiPlayerType { Individual, TwoTeams };

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class GameStatePlayer
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public GameStatePlayerStatus Status { get; set; }
        public int Team { get; set; }

        public bool IsPlaying
        {
            get
            {
                return Status == GameStatePlayerStatus.Playing;
            }
        }

        public string PlayerButton
        {
            get
            {
                return Avatar + " " + Name;
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

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class GameState
    {
        public static string[] PlayerAvatars = new string[] { "\ud83d\ude00" };
        public ObservableCollection<GameStatePlayer> multiPlayers;

        public GameState()
        {
            multiPlayers = new ObservableCollection<GameStatePlayer>
            {
                new GameStatePlayer("Player 1", PlayerAvatars[0])
            };
        }

        public GameType type { get; set; }
        //private GameStateMultiPlayerType _multiPlayerType;


        public GameStatePlayer AddPlayer()
        {
            GameStatePlayer p = new GameStatePlayer("Player " + (multiPlayers.Count + 1).ToString(), PlayerAvatars[0]);
            multiPlayers.Add(p);
            return p;
        }
    }

}
