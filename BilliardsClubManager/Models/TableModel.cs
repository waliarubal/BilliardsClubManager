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
    [Table("Tables")]
    class TableModel: NotificationBase, IRecord
    {
        long _id;
        string _name;
        decimal _pricePerMinute;
        int _switch;

        public TableModel()
        {
            Id = Switch = -1;
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

        [DisplayName("Charge (₹ per minute)")]
        public decimal PricePerMinute
        {
            get => _pricePerMinute;
            set => Set(nameof(PricePerMinute), ref _pricePerMinute, value);
        }

        [DisplayName("Switch")]
        public int Switch
        {
            get => _switch;
            set => Set(nameof(Switch), ref _switch, value);
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
            sqlbuilder.AppendLineFormatted("FROM [Tables]");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  [Name] LIKE '%{0}%'", searchKeywoard);

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Query<TableModel>(sqlbuilder.ToString());
            }
        }

        public IRecord New()
        {
            return new TableModel();
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(Name))
                return "Table name not specified.";
            if (PricePerMinute <= 0)
                return "Price (per minute) not specified.";
            if (Switch < 0)
                return "Switch not specified.";

            using (var connection = Shared.Instance.GetConnection())
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT");
                sqlBuilder.AppendLine(" COUNT(Id)");
                sqlBuilder.AppendLine("FROM [Tables]");
                sqlBuilder.AppendLine("WHERE ");
                sqlBuilder.AppendLine(" Id != {0} AND", Id);
                sqlBuilder.AppendLine(" Switch = {1}", Switch);

                if (connection.ExecuteScalar<int>(sqlBuilder.ToString()) != 0)
                    return string.Format("Switch {0} is already assigned to another table.", Switch);

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
    }
}
