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
            Clear();
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

        [DisplayName("Price (per minute)")]
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

        public void Clear()
        {
            Name = null;
            PricePerMinute = 0;
            Switch = -1;
        }

        public string Delete()
        {
            throw new System.NotImplementedException();
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

            var sqlBuilder = new StringBuilder();
            if (Id < 0)
            {
                sqlBuilder.AppendLineFormatted("INSERT INTO [Tables] ([Name], [PricePerMinute], [Switch])");
                sqlBuilder.AppendLineFormatted("VALUES ('{0}', {1}, {2})", Name, PricePerMinute, Switch);
            }
            else
            {
                sqlBuilder.AppendLineFormatted("UPDATE [Tables]");
                sqlBuilder.AppendLineFormatted("SET");
                sqlBuilder.AppendLineFormatted("  [Name] = '{0}',", Name);
                sqlBuilder.AppendLineFormatted("  [PricePerMinute] = {0},", PricePerMinute);
                sqlBuilder.AppendLineFormatted("  [Switch] = {0}", Switch);
                sqlBuilder.AppendLineFormatted("WHERE [Id] = {0}", Id);
            }

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Execute(sqlBuilder.ToString()) == 0 ? null : "Failed to save record.";
            }
        }


    }
}
