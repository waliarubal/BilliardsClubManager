using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class DashboardViewModel: ViewModelBase
    {
        ICommand _refresh, _startGame, _endGame, _saveGame;

        ObservableCollection<GameModel> _games;
        IEnumerable<PlayerModel> _players;

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
        }

        void EndGame(GameModel game)
        {
            game.EndGame();
        }

        void SaveGame(GameModel game)
        {
            if(game.Save() == null)
            {
                var table = game.Table;
                var index = Games.IndexOf(game);
                Games.RemoveAt(index);
                Games.Insert(index, new GameModel { Table = table });
            }
        }

        IEnumerable<GameModel> Refresh(object argument)
        {
            Players = new PlayerModel().Get(string.Empty) as IEnumerable<PlayerModel>;

            var games = new List<GameModel>();
            var tables = new TableModel().Get(string.Empty);
            foreach (TableModel table in tables)
            {
                var game = new GameModel { Table = table };
                games.Add(game);
            }
            return games;
        }

        void RefreshCallback(IEnumerable<GameModel> games)
        {
            Games.Clear();
            foreach(var game in games)
                Games.Add(game);
        }
    }
}
