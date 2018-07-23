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
    [Table("GameStyles")]
    class GameStyleModel: NotificationBase, IRecord, IEquatable<GameStyleModel>
    {
        long _id;
        string _name;

        public GameStyleModel()
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

        [DisplayName("Name")]
        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
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
                return connection.Get<GameStyleModel>(id);
            }
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  *");
            sqlbuilder.AppendLineFormatted("FROM [GameStyles]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [Name] LIKE '%{0}%'", searchKeywoard);

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Query<GameStyleModel>(sqlbuilder.ToString());
            }
        }

        public IRecord New()
        {
            return new GameStyleModel();
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(Name))
                return "Game style not specified.";

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

        public bool Equals(GameStyleModel other)
        {
            return other != null && Id == other.Id;
        }

    }
}
