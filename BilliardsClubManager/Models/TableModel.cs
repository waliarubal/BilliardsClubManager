using System.Collections.Generic;
using BilliardsClubManager.Base;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.Models
{
    [Table("Tables")]
    class TableModel: NotificationBase, IRecord
    {
        long _id;
        string _name;
        decimal _pricePerMinute;
        byte _switch;

        #region properties

        [Key]
        public long Id
        {
            get => _id;
            set => Set(nameof(Id), ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        public decimal PricePerMinute
        {
            get => _pricePerMinute;
            set => Set(nameof(PricePerMinute), ref _pricePerMinute, value);
        }

        public byte Switch
        {
            get => _switch;
            set => Set(nameof(Switch), ref _switch, value);
        }

        #endregion

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public string Delete()
        {
            throw new System.NotImplementedException();
        }

        public IRecord Get(long id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            throw new System.NotImplementedException();
        }

        public string Save()
        {
            throw new System.NotImplementedException();
        }


    }
}
