using BilliardsClubManager.Base;
using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class CreditNoteViewModel: ViewModelBase, IRecordEditor
    {
        CreditNoteModel _record;
        IEnumerable<PlayerModel> _players;

        ICommand _initialize;

        public CreditNoteViewModel()
        {
            Record = new CreditNoteModel();
        }

        #region properties

        public IEnumerable<PlayerModel> Players
        {
            get => _players;
            private set => Set(nameof(Players), ref _players, value);
        }

        public IRecord Record
        {
            get => _record;
            set => Set(nameof(Record), ref _record, value as CreditNoteModel);
        }

        public bool IsNewAllowed => true;

        public bool IsSaveAllowed => true;

        public bool IsDeleteAllowed => false;

        #endregion

        #region commands

        public ICommand InitializeCommand
        {
            get
            {
                if (_initialize == null)
                    _initialize = new RelayCommand(Initialize);

                return _initialize;
            }
        }

        #endregion

        void Initialize()
        {
            Players = new PlayerModel().Get(string.Empty) as IEnumerable<PlayerModel>;
        }
    }
}
