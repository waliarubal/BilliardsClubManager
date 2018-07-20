using BilliardsClubManager.Base;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers.Base;
using System;
using System.Collections.Generic;

namespace BilliardsClubManager.Models
{
    [Table("Games")]
    class GameModel: NotificationBase, IRecord
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

        public TableModel Table
        {
            get => _table;
            set => Set(nameof(Table), ref _table, value);
        }

        public PlayerModel Player1
        {
            get => _player1;
            set => Set(nameof(Player1), ref _player1, value);
        }

        public PlayerModel Player2
        {
            get => _player2;
            set => Set(nameof(Player2), ref _player2, value);
        }

        public PlayerModel Winner
        {
            get => _winner;
            set => Set(nameof(Winner), ref _winner, value);
        }

        public DateTime Start
        {
            get => _start;
            set => Set(nameof(Start), ref _start, value);
        }

        public DateTime End
        {
            get => _end;
            set => Set(nameof(End), ref _end, value);
        }

        #endregion

        public string Delete()
        {
            throw new NotImplementedException();
        }

        public IRecord Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            throw new NotImplementedException();
        }

        public IRecord New()
        {
            throw new NotImplementedException();
        }

        public string Save()
        {
            throw new NotImplementedException();
        }
    }
}
