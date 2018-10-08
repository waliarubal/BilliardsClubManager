using BilliardsClubManager.Models;
using Microsoft.Win32;
using NullVoidCreations.Licensing;
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
        ICommand _initialize, _uninitialize, _save, _loadLicense;
        string _errorMessage, _serialKey, _activationKey;

        #region properties

        public string SerialKey
        {
            get => _serialKey;
            set => Set(nameof(SerialKey), ref _serialKey, value);
        }

        public string ActivationKey
        {
            get => _activationKey;
            set => Set(nameof(ActivationKey), ref _activationKey, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => Set(nameof(ErrorMessage), ref _errorMessage, value);
        }

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

        public bool IsMaximizedOnStart
        {
            get => Shared.Instance.IsMaximizedOnStart;
            set => Shared.Instance.IsMaximizedOnStart = value;
        }

        public bool IsSwitchControlledAutomatically
        {
            get => Shared.Instance.IsSwitchControlledAutomatically;
            set => Shared.Instance.IsSwitchControlledAutomatically = value;
        }

        public StrongLicense License
        {
            get => Shared.Instance.License;
        }

        public string SettingsFile
        {
            get => Shared.Instance.SettingsFile;
        }

        public string DatabaseFile
        {
            get => Shared.Instance.DatabaseFile;
        }

        public bool IsLicensed
        {
            get => Shared.Instance.IsLicensed;
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

        public ICommand UninitializeCommand
        {
            get
            {
                if (_uninitialize == null)
                    _uninitialize = new RelayCommand(Uninitialize);

                return _uninitialize;
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

        public ICommand LoadLicenseCommand
        {
            get
            {
                if (_loadLicense == null)
                    _loadLicense = new RelayCommand(LoadLicense) { IsSynchronous = true };

                return _loadLicense;
            }
        }

        #endregion

        void LoadLicense()
        {
            if (string.IsNullOrWhiteSpace(SerialKey))
                ErrorMessage = "Serial key not specified.";
            else if (string.IsNullOrWhiteSpace(ActivationKey))
                ErrorMessage = "Activation key not specified.";
            else
            {
                ErrorMessage = Shared.Instance.LoadLicense(SerialKey, ActivationKey);
                if (IsLicensed)
                {
                    Shared.Instance.LoadSettings();
                    ErrorMessage = Shared.Instance.Switch.Open();
                }
            }
        }

        void OnSharedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        void Initialize()
        {
            Shared.Instance.PropertyChanged += OnSharedPropertyChanged;

            Players = new PlayerModel().Get(string.Empty) as IEnumerable<PlayerModel>;
            GameStyles = new GameStyleModel().Get(string.Empty) as IEnumerable<GameStyleModel>;
        }

        void Uninitialize()
        {
            Shared.Instance.PropertyChanged -= OnSharedPropertyChanged;
        }

        void Save()
        {
            Shared.Instance.SaveSettings();
        }
    }
}
