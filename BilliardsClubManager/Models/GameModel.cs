using BilliardsClubManager.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;

namespace BilliardsClubManager.Models
{
    public enum GameState: byte
    {
        NotStarted,
        InProgress,
        Finished
    }

    [Table("Games")]
    class GameModel : NotificationBase, IRecord
    {
        long _id;
        TableModel _table;
        PlayerModel _player1, _player2, _winner;
        DateTime? _start, _end;
        string _errorMessage;
        GameState _state;
        TimeSpan _runningTime;
        Timer _timer;
        decimal _charge;

        public GameModel()
        {
            Id = -1;
            UpdateState();
        }

        #region properties

        [Key]
        public long Id
        {
            get => _id;
            set => Set(nameof(Id), ref _id, value);
        }

        [DisplayName("Table")]
        public TableModel Table
        {
            get => _table;
            set => Set(nameof(Table), ref _table, value);
        }

        [DisplayName("First Player (or Team)")]
        public PlayerModel Player1
        {
            get => _player1;
            set => Set(nameof(Player1), ref _player1, value);
        }

        [DisplayName("Second Player (or Team)")]
        public PlayerModel Player2
        {
            get => _player2;
            set => Set(nameof(Player2), ref _player2, value);
        }

        [DisplayName("Winner Player (or Team)")]
        public PlayerModel Winner
        {
            get => _winner;
            set => Set(nameof(Winner), ref _winner, value);
        }

        [DisplayName("Start (Date & Time)")]
        public DateTime? Start
        {
            get => _start;
            set
            {
                if (Set(nameof(Start), ref _start, value))
                    UpdateState();
            }
        }

        [DisplayName("End (Date & Time)")]
        public DateTime? End
        {
            get => _end;
            set
            {
                if (Set(nameof(End), ref _end, value))
                    UpdateState();
            }
        }

        [Computed]
        public TimeSpan Time
        {
            get => _runningTime;
            private set => Set(nameof(Time), ref _runningTime, value);
        }

        [Computed]
        public decimal Charge
        {
            get => _charge;
            private set => Set(nameof(Charge), ref _charge, value);
        }

        [Computed]
        public GameState State
        {
            get => _state;
            private set => Set(nameof(State), ref _state, value);
        }

        [Computed]
        public bool IsNotStarted
        {
            get => State == GameState.NotStarted;
        }

        [Computed]
        public bool IsInProgress
        {
            get => State == GameState.InProgress;
        }

        [Computed]
        public bool IsFinished
        {
            get => State == GameState.Finished;
        }

        [Computed]
        public string ErrorMessage
        {
            get => _errorMessage;
            private set => Set(nameof(ErrorMessage), ref _errorMessage, value);
        }

        #endregion

        void UpdateState()
        {
            if (Start == null)
                State = GameState.NotStarted;
            else if (Start != null && End == null)
                State = GameState.InProgress;
            else if (Start != null && End != null)
                State = GameState.Finished;

            RaisePropertyChanged(nameof(IsNotStarted));
            RaisePropertyChanged(nameof(IsInProgress));
            RaisePropertyChanged(nameof(IsFinished));
        }

        void Compute(DateTime? currentTime)
        {
            Time = currentTime.Value.Subtract(Start.Value);
            Charge = (decimal)Time.TotalMinutes * Table.PricePerMinute;
        }

        public bool StartGame()
        {
            Start = DateTime.Now;

            ErrorMessage = Save();
            if (ErrorMessage != null)
            {
                Start = null;
                return false;
            }

            _timer = new Timer(1000);
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => Compute(DateTime.Now);
            _timer.Start();

            return true;
        }

        public void EndGame()
        {
            ErrorMessage = null;

            _timer.Stop();
            _timer.Dispose();

            End = DateTime.Now;
            Compute(End);
        }

        public string Delete()
        {
            using (var connection = Shared.Instance.GetConnection())
            {
                ErrorMessage = connection.Delete(this) ? null : "Failed to delete record.";
            }
            return ErrorMessage;
        }

        public IRecord Get(long id)
        {
            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Get<GameModel>(id);
            }
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLineFormatted("SELECT");
            sqlBuilder.AppendLineFormatted(" * ");
            sqlBuilder.AppendLineFormatted("FROM [Games] AS G");
            sqlBuilder.AppendLineFormatted("INNER JOIN [Tables] AS T ON G.TableId = T.Id");
            sqlBuilder.AppendLineFormatted("INNER JOIN [Players] AS P1 ON G.Player1Id = P1.Id");
            sqlBuilder.AppendLineFormatted("LEFT JOIN [Players] AS P2 ON G.Player2Id = P2.Id");
            sqlBuilder.AppendLineFormatted("LEFT JOIN [Players] AS W ON G.WinnerId = W.Id");

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Query<GameModel, TableModel, PlayerModel, PlayerModel, PlayerModel, GameModel>(sqlBuilder.ToString(), MapResult);
            }
        }

        GameModel MapResult(GameModel game, TableModel table, PlayerModel player1, PlayerModel player2, PlayerModel winner)
        {
            game.Table = table;
            game.Player1 = player1;
            game.Player2 = Player2;
            game.Winner = winner;
            return game;
        }

        public IRecord New()
        {
            return new GameModel();
        }

        public string Save()
        {
            ErrorMessage = null;
            if (Table == null)
                ErrorMessage = "Table not specified.";
            else if (Player1 == null)
                ErrorMessage = "First player (or team) not specified.";
            else if (Player2 == null)
                ErrorMessage = "Second player (or team) not specified.";
            else if (Player1.Equals(Player2))
                ErrorMessage = "First player (or team) and second player (or team) can't be same.";
            else if (Start == null)
                ErrorMessage = "Game start date & time not specified.";
            if (Id > -1)
            {
                if (End == null)
                    ErrorMessage = "Game end date & time not specified.";
                else if (Winner == null)
                    ErrorMessage = "Winner player (or team) not specified.";
                else if (!Winner.Equals(Player1) && Winner.Equals(Player2))
                    ErrorMessage = "Winner must be one of the first or second player (or team).";
            }
            if (ErrorMessage != null)
                return ErrorMessage;

            using (var connection = Shared.Instance.GetConnection())
            {
                bool isSaved;
                var sqlBuilder = new StringBuilder();
                if (Id == -1)
                {
                    sqlBuilder.AppendLineFormatted("INSERT INTO [Games] (TableId, Player1Id, Player2Id, Start)");
                    sqlBuilder.AppendLineFormatted("VALUES ({0}, {1}, {2}, '{3}');", Table.Id, Player1.Id, Player2.Id, Start.Value);
                    sqlBuilder.AppendLineFormatted("SELECT last_insert_rowid();");

                    Id = connection.ExecuteScalar<long>(sqlBuilder.ToString());
                    isSaved = Id  > -1;
                }
                else
                {
                    sqlBuilder.AppendLineFormatted("UPDATE [Games]");
                    sqlBuilder.AppendLineFormatted("SET");
                    sqlBuilder.AppendLineFormatted("  WinnerId = {0},", Winner.Id);
                    sqlBuilder.AppendLineFormatted("  End = '{0}'", End.Value);
                    sqlBuilder.AppendLineFormatted("WHERE Id = {0};", Id);

                    isSaved = connection.Execute(sqlBuilder.ToString()) > 0;
                }

                ErrorMessage = isSaved ? null : "Failed to save record.";
                return ErrorMessage;
            }
        }
    }
}
