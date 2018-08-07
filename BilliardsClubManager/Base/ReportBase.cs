using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.DataStructures;
using System.Collections.Generic;
using System.Data;

namespace BilliardsClubManager.Base
{
    abstract class ReportBase: NotificationBase
    {
        readonly List<Doublet<string, object>> _parameters;
        string _name;

        protected ReportBase(string name)
        {
            _parameters = new List<Doublet<string, object>>();
            Name = name;
        }

        #region properties

        public IEnumerable<Doublet<string, object>> Parameters
        {
            get { return _parameters; }
        }

        public string Name
        {
            get { return _name; }
            private set { Set(nameof(Name), ref _name, value); }
        }

        #endregion

        protected void Add(string key, object value)
        {
            _parameters.Add(new Doublet<string, object>(key, value));
        }

        protected void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public abstract DataTable Generate();
    }
}
