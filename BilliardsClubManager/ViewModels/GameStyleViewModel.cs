using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.ViewModels
{
    class GameStyleViewModel: ViewModelBase, IRecordEditor
    {
        GameStyleModel _record;

        public GameStyleViewModel()
        {
            Record = new GameStyleModel();
        }

        #region properties

        public IRecord Record
        {
            get => _record;
            set => Set(nameof(Record), ref _record, value as GameStyleModel);
        }

        public bool IsNewAllowed => true;

        public bool IsSaveAllowed => true;

        public bool IsDeleteAllowed => true;

        #endregion
    }
}
