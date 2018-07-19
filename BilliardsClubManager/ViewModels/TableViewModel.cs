using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using System.Collections.Generic;

namespace BilliardsClubManager.ViewModels
{
    class TableViewModel : ViewModelBase, IRecordEditor
    {
        TableModel _record;

        IEnumerable<int> _switches;

        public TableViewModel()
        {
            Record = new TableModel();
        }

        #region properties

        public IRecord Record
        {
            get => _record;
            set => Set(nameof(Record), ref _record, value as TableModel);
        }

        public IEnumerable<int> Switches
        {
            get
            {
                if(_switches == null)
                {
                    var switches = new List<int>();
                    for (int counter = 0; counter < 8; counter++)
                        switches.Add(counter);

                    _switches = switches;
                }

                return _switches;
            }
        }

        #endregion
    }
}
