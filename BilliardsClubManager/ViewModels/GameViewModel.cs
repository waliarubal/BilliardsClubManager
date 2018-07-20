using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.ViewModels
{
    class GameViewModel: ViewModelBase, IRecordEditor
    {
        GameModel _record;

        public GameViewModel()
        {
            Record = new GameModel();
        }

        #region properties

        public IRecord Record
        {
            get => _record;
            set => Set(nameof(Record), ref _record, value as GameModel);
        }

        public bool IsNewAllowed => false;

        public bool IsSaveAllowed => false;

        public bool IsDeleteAllowed => false;

        #endregion
    }
}
