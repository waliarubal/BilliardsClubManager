using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.ViewModels
{
    class TableViewModel : ViewModelBase, IRecordEditor
    {
        TableModel _record;

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

        #endregion
    }
}
