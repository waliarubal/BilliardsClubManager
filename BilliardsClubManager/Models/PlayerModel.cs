using System.Collections.Generic;
using BilliardsClubManager.Base;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.Models
{
    [Table("Players")]
    class PlayerModel: NotificationBase, IRecord
    {
        long _id;
        string _name, _phone, _email;

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

        public string Phone
        {
            get => _phone;
            set => Set(nameof(Phone), ref _phone, value);
        }

        public string Email
        {
            get => _email;
            set => Set(nameof(Email), ref _email, value);
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

        public IRecord New()
        {
            throw new System.NotImplementedException();
        }

        public string Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
