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
            throw new System.NotImplementedException();
        }


    }
}
