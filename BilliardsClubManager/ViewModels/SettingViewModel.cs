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
        IEnumerable<GameStyleModel> _styles;
        ICommand _initialize, _save;

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

        public IEnumerable<GameStyleModel> GameStyles
        {
            get => _styles;
            private set => Set(nameof(GameStyles), ref _styles, value);
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

        public GameStyleModel DefaultGameStyle
        {
            get => Shared.Instance.DefaultGameStyle;
            set => Shared.Instance.DefaultGameStyle = value;
        }

        public string SettingsFile
        {
            get => Shared.Instance.SettingsFile;
        }

        public string DatabaseFile
        {
            get => Shared.Instance.DatabaseFile;
        }

        public string LicenseFile
        {
            get => Shared.Instance.LicenseFile;
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

        public ICommand SaveCommand
        {
            get
            {
                if (_save == null)
                    _save = new RelayCommand(Save);

                return _save;
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
            GameStyles = new GameStyleModel().Get(string.Empty) as IEnumerable<GameStyleModel>;
        }

        void Save()
        {
            Shared.Instance.SaveSettings();
        }
    }
}
