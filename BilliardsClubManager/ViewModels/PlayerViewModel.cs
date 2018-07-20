using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;

namespace BilliardsClubManager.ViewModels
{
    class PlayerViewModel: ViewModelBase, IRecordEditor
    {
        PlayerModel _record;

        public PlayerViewModel()
        {
            Record = new PlayerModel();
        }

        #region properties

        public IRecord Record
        {
            get => _record;
            set => Set(nameof(Record), ref _record, value as PlayerModel);
        }

        public bool IsNewAllowed => true;

        public bool IsSaveAllowed => true;

        public bool IsDeleteAllowed => true;

        #endregion
    }
}
