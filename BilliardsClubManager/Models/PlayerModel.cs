using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using BilliardsClubManager.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.Models
{
    [Table("Players")]
    class PlayerModel: NotificationBase, IRecord, IEquatable<PlayerModel>
    {
        long _id;
        int _played, _won;
        string _name, _phone, _email;

        public PlayerModel()
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

        [DisplayName("Full Name")]
        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        [DisplayName("Phone Number")]
        public string Phone
        {
            get => _phone;
            set => Set(nameof(Phone), ref _phone, value);
        }

        [DisplayName("E-mail Address")]
        public string Email
        {
            get => _email;
            set => Set(nameof(Email), ref _email, value);
        }

        [DisplayName("Games Played")]
        [Computed]
        public int GamesPlayed
        {
            get => _played;
            private set => Set(nameof(GamesPlayed), ref _played, value);
        }

        [DisplayName("Games Won")]
        [Computed]
        public int GamesWon
        {
            get => _won;
            private set => Set(nameof(GamesWon), ref _won, value);
        }

        [DisplayName("Games Lost")]
        [Computed]
        public int GamesLost
        {
            get => GamesPlayed - GamesWon;
        }

        #endregion

        int GetGamesPlayed(IDbConnection connection, long id)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  COUNT(Id)");
            sqlbuilder.AppendLineFormatted("FROM [Games]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [Player1Id] = {0} OR", id);
            sqlbuilder.AppendLineFormatted("  [Player2Id] = {0}", id);

            return connection.ExecuteScalar<int>(sqlbuilder.ToString());
        }

        int GetGamesWon(IDbConnection connection, long id)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  COUNT(Id)");
            sqlbuilder.AppendLineFormatted("FROM [Games]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [WinnerId] = {0}", id);

            return connection.ExecuteScalar<int>(sqlbuilder.ToString());
        }

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
                return connection.Get<TableModel>(id);
            }
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  *");
            sqlbuilder.AppendLineFormatted("FROM [Players]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [Name] LIKE '%{0}%'", searchKeywoard);

            var players = new List<PlayerModel>();
            using (var connection = Shared.Instance.GetConnection())
            {
                foreach(var player in connection.Query<PlayerModel>(sqlbuilder.ToString()))
                {
                    player.GamesPlayed = GetGamesPlayed(connection, player.Id);
                    player.GamesWon = GetGamesWon(connection, player.Id);
                    players.Add(player);
                } 
            }
            return players;
        }

        public IRecord New()
        {
            return new PlayerModel();
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(Name))
                return "Player name not specified.";

            using (var connection = Shared.Instance.GetConnection())
            {
                bool isSaved;
                if (Id < 0)
                    isSaved = connection.Insert(this) > -1;
                else
                    isSaved = connection.Update(this);

                return isSaved ? null : "Failed to save record.";
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(PlayerModel other)
        {
            return other != null && Id.Equals(other.Id);
        }
    }
}
