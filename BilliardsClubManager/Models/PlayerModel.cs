using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BilliardsClubManager.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.Models
{
    [Table("Players")]
    class PlayerModel: NotificationBase, IRecord, IEqualityComparer<PlayerModel>
    {
        long _id;
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

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Query<PlayerModel>(sqlbuilder.ToString());
            }
        }

        public int GetHashCode(PlayerModel obj)
        {
            throw new System.NotImplementedException();
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

        public bool Equals(PlayerModel first, PlayerModel second)
        {
            return first != null && second != null && first.Id == second.Id;
        }
    }
}
