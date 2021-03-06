﻿using BilliardsClubManager.Models;
using Devart.Data.SQLite;
using NullVoidCreations.Licensing;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace BilliardsClubManager
{
    class Shared: NotificationBase
    {
        const string PASSWORD = "!Control*88";

        static Shared _instance;
        bool _isMaximizedOnStart, _isSwitchControlledAutomatically;
        PlayerModel _defaultFirstPlayer, _defaultSecondPlayer;
        GameStyleModel _defaultGameStyle;
        StrongLicense _license;
        Switch _switch;

        private Shared()
        {
            StartupDirectory = Application.Current.GetStartupDirectory();
            SettingsFile = Path.Combine(StartupDirectory, "Assets", "Settings.aes");
            DatabaseFile = Path.Combine(StartupDirectory, "Assets", "Database.sqlite3");
        }

        #region properties

        internal string SerialKey { get; set; }

        internal string ActivationKey { get; set; }

        public static Shared Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Shared();

                return _instance;
            }
        }

        public string StartupDirectory { get; }

        public string SettingsFile { get; }

        public string DatabaseFile { get; }

        public PlayerModel DefaultFirstPlayer
        {
            get => _defaultFirstPlayer;
            set => Set(nameof(DefaultFirstPlayer), ref _defaultFirstPlayer, value);
        }

        public PlayerModel DefaultSecondPlayer
        {
            get => _defaultSecondPlayer;
            set => Set(nameof(DefaultSecondPlayer), ref _defaultSecondPlayer, value);
        }

        public GameStyleModel DefaultGameStyle
        {
            get => _defaultGameStyle;
            set => Set(nameof(DefaultGameStyle), ref _defaultGameStyle, value);
        }

        public bool IsMaximizedOnStart
        {
            get => _isMaximizedOnStart;
            set => Set(nameof(IsMaximizedOnStart), ref _isMaximizedOnStart, value);
        }

        public bool IsSwitchControlledAutomatically
        {
            get => _isSwitchControlledAutomatically;
            set => Set(nameof(IsSwitchControlledAutomatically), ref _isSwitchControlledAutomatically, value);
        }

        public StrongLicense License
        {
            get => _license;
            private set
            {
                if(Set(nameof(License), ref _license, value))
                    RaisePropertyChanged(nameof(IsLicensed));
            }
        }

        public bool IsLicensed
        {
            get { return License != null && License.IsActivated; }
        }

        public Switch Switch
        {
            get
            {
                if (_switch == null)
                    _switch = new Switch();

                return _switch;
            }
        }

        #endregion

        public IDbConnection GetConnection()
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = DatabaseFile,
                FailIfMissing = true,
                ConnectionTimeout = 20,
                Locking = LockingMode.Exclusive,
                AutoVacuum = AutoVacuumMode.Full,

            };

            var connection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }

        public string LoadLicense(string serialKey, string activationKey)
        {
            string errorMessage;
            try
            {
                License = StrongLicense.Load(serialKey, activationKey, out errorMessage);
                SerialKey = serialKey;
                ActivationKey = activationKey;
            }
            catch(Exception ex)
            {
                errorMessage = string.Format("Failed to load license. {0}", ex.Message);
            }

            return errorMessage;
        }

        public void LoadSettings()
        {
            var settings = new SettingsManager();
            settings.Load(SettingsFile, PASSWORD);
            DefaultFirstPlayer = new PlayerModel().Get(settings.GetValue<long>(nameof(DefaultFirstPlayer), 1)) as PlayerModel;
            DefaultSecondPlayer = new PlayerModel().Get(settings.GetValue<long>(nameof(DefaultSecondPlayer), 2)) as PlayerModel;
            DefaultGameStyle = new GameStyleModel().Get(settings.GetValue<long>(nameof(DefaultGameStyle), 1)) as GameStyleModel;
            IsMaximizedOnStart = settings.GetValue(nameof(IsMaximizedOnStart), true);
            SerialKey = settings.GetValue(nameof(SerialKey), default(string));
            ActivationKey = settings.GetValue(nameof(ActivationKey), default(string));
        }

        public void SaveSettings()
        {
            var settings = new SettingsManager();
            settings.SetValue(nameof(DefaultFirstPlayer), DefaultFirstPlayer.Id);
            settings.SetValue(nameof(DefaultSecondPlayer), DefaultSecondPlayer.Id);
            settings.SetValue(nameof(DefaultGameStyle), DefaultGameStyle.Id);
            settings.SetValue(nameof(IsMaximizedOnStart), IsMaximizedOnStart);
            settings.SetValue(nameof(SerialKey), SerialKey);
            settings.SetValue(nameof(ActivationKey), ActivationKey);
            settings.Save(SettingsFile, PASSWORD);
        }
    }
}
