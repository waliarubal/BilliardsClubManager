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
        int _played, _paid;
        string _name, _phone, _email;
        decimal _balance;

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

        [DisplayName("Games Paid")]
        [Computed]
        public int GamesPaid
        {
            get => _paid;
            private set => Set(nameof(GamesPaid), ref _paid, value);
        }

        [DisplayName("Balance")]
        [Computed]
        public decimal Balance
        {
            get => _balance;
            private set => Set(nameof(Balance), ref _balance, value);
        }

        #endregion

        #region private methods

        decimal GetBalance(IDbConnection connection, long id)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  SUM(Amount)");
            sqlbuilder.AppendLineFormatted("FROM [CreditNotes]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [PlayerId] = {0}", id);

            var credit = connection.ExecuteScalar<decimal>(sqlbuilder.ToString());

            sqlbuilder.Clear();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  SUM(ChargeTotal)");
            sqlbuilder.AppendLineFormatted("FROM [Games]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  PaidById = {0}", id);

            var chargeTotal = connection.ExecuteScalar<decimal>(sqlbuilder.ToString());

            return credit - chargeTotal;
        }

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

        int GetGamesPaid(IDbConnection connection, long id)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  COUNT(Id)");
            sqlbuilder.AppendLineFormatted("FROM [Games]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [PaidById] = {0}", id);

            return connection.ExecuteScalar<int>(sqlbuilder.ToString());
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
                    player.GamesPaid = GetGamesPaid(connection, player.Id);
                    player.Balance = GetBalance(connection, player.Id);
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
