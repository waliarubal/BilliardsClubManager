using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class DashboardViewModel : ViewModelBase
    {
        ICommand _refresh, _startGame, _endGame, _saveGame;

        ObservableCollection<GameModel> _games;
        IEnumerable<PlayerModel> _players;
        IEnumerable<GameStyleModel> _gameStyles;

        #region properties

        public ObservableCollection<GameModel> Games
        {
            get
            {
                if (_games == null)
                    _games = new ObservableCollection<GameModel>();

                return _games;
            }
        }

        public IEnumerable<PlayerModel> Players
        {
            get => _players;
            private set => Set(nameof(Players), ref _players, value);
        }

        public IEnumerable<GameStyleModel> GameStyles
        {
            get => _gameStyles;
            private set => Set(nameof(GameStyles), ref _gameStyles, value);
        }

        public GameStyleModel DefaultGameStyle
        {
            get => Shared.Instance.DefaultGameStyle;
        }

        public PlayerModel DefaultPlayer1
        {
            get => Shared.Instance.DefaultFirstPlayer;
        }

        public PlayerModel DefaultPlayer2
        {
            get => Shared.Instance.DefaultSecondPlayer;
        }

        #endregion

        #region commands

        public ICommand RefreshCommand
        {
            get
            {
                if (_refresh == null)
                    _refresh = new RelayCommand<object, IEnumerable<GameModel>>(Refresh, RefreshCallback);

                return _refresh;
            }
        }

        public ICommand StartGameCommand
        {
            get
            {
                if (_startGame == null)
                    _startGame = new RelayCommand<GameModel>(StartGame);

                return _startGame;
            }
        }

        public ICommand EndGameCommand
        {
            get
            {
                if (_endGame == null)
                    _endGame = new RelayCommand<GameModel>(EndGame);

                return _endGame;
            }
        }

        public ICommand SaveGameCommand
        {
            get
            {
                if (_saveGame == null)
                    _saveGame = new RelayCommand<GameModel>(SaveGame) { IsSynchronous = true };

                return _saveGame;
            }
        }

        #endregion

        void StartGame(GameModel game)
        {
            game.StartGame();
            Shared.Instance.Switch.Toggle(game.Table.Switch, true);
        }

        void EndGame(GameModel game)
        {
            game.EndGame();
            Shared.Instance.Switch.Toggle(game.Table.Switch, false);
        }

        void SaveGame(GameModel game)
        {
            if (game.Save() == null)
            {
                var table = game.Table;
                var newGame = new GameModel
                {
                    Table = table,
                    GameStyle = DefaultGameStyle,
                    Player1 = DefaultPlayer1,
                    Player2 = DefaultPlayer2
                };
                var index = Games.IndexOf(game);

                Games.RemoveAt(index);
                Games.Insert(index, newGame);
            }
        }

        IEnumerable<GameModel> Refresh(object argument)
        {
            Players = new PlayerModel().Get(string.Empty) as IEnumerable<PlayerModel>;
            GameStyles = new GameStyleModel().Get(string.Empty) as IEnumerable<GameStyleModel>;

            var gameCache = new Dictionary<long, GameModel>();
            foreach (GameModel game in new GameModel().Get(string.Empty, GameState.InProgress))
                gameCache.Add(game.Table.Id, game);

            var games = new List<GameModel>();
            foreach (TableModel table in new TableModel().Get(string.Empty))
            {
                if (gameCache.ContainsKey(table.Id))
                {
                    gameCache[table.Id].ResumeGame();
                    games.Add(gameCache[table.Id]);
                }
                else
                {
                    var game = new GameModel
                    {
                        Table = table,
                        GameStyle = DefaultGameStyle,
                        Player1 = DefaultPlayer1,
                        Player2 = DefaultPlayer2
                    };
                    games.Add(game);
                }
            }

            return games;
        }

        void RefreshCallback(IEnumerable<GameModel> games)
        {
            Games.Clear();
            foreach (var game in games)
                Games.Add(game);
        }
    }
}
