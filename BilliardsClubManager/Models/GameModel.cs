using BilliardsClubManager.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BilliardsClubManager.Models
{
    [Table("Games")]
    class GameModel : NotificationBase, IRecord
    {
        long _id;
        TableModel _table;
        PlayerModel _player1, _player2, _winner;
        DateTime _start, _end;

        public GameModel()
        {
            Id = -1;
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
        public DateTime Start
        {
            get => _start;
            set => Set(nameof(Start), ref _start, value);
        }

        [DisplayName("End (Date & Time)")]
        public DateTime End
        {
            get => _end;
            set => Set(nameof(End), ref _end, value);
        }

        #endregion

        public string Delete()
        {
            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Delete(this) ? null : "Failed to delete record.";
            }
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
            sqlBuilder.AppendLineFormatted(" *");
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
            throw new NotImplementedException();
        }
    }
}
