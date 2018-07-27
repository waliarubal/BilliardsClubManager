using BilliardsClubManager.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace BilliardsClubManager.ViewModels
{
    class SettingViewModel: ViewModelBase
    {
        IEnumerable<PlayerModel> _players;
        ICommand _initialize;

        public SettingViewModel()
        {
            Shared.Instance.PropertyChanged += OnSharedPropertyChanged;
        }

        ~SettingViewModel()
        {
            Shared.Instance.PropertyChanged -= OnSharedPropertyChanged;
        }

        #region properties

        public IEnumerable<PlayerModel> Players
        {
            get => _players;
            private set => Set(nameof(Players), ref _players, value);
        }

        public PlayerModel DefaultFirstPlayer
        {
            get => Shared.Instance.DefaultFirstPlayer;
            set => Shared.Instance.DefaultFirstPlayer = value;
        }

        public PlayerModel DefaultSecondPlayer
        {
            get => Shared.Instance.DefaultSecondPlayer;
            set => Shared.Instance.DefaultSecondPlayer = value;
        }

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

        void OnSharedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        void Initialize()
        {
            Players = new PlayerModel().Get(string.Empty) as IEnumerable<PlayerModel>;
        }
    }
}
