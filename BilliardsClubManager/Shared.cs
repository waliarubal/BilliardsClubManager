﻿using BilliardsClubManager.Models;
using Devart.Data.SQLite;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System.Data;
using System.IO;
using System.Windows;

namespace BilliardsClubManager
{
    class Shared: NotificationBase
    {
        const string PASSWORD = "!Control*88";

        static Shared _instance;
        PlayerModel _defaultFirstPlayer, _defaultSecondPlayer;
        GameStyleModel _defaultGameStyle;

        private Shared()
        {
            StartupDirectory = Application.Current.GetStartupDirectory();
            SettingsFile = Path.Combine(StartupDirectory, "Assets", "Settings.aes");
            DatabaseFile = Path.Combine(StartupDirectory, "Assets", "Database.sqlite3");
        }

        #region properties

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

        public void LoadSettings()
        {
            var settings = new SettingsManager();
            settings.Load(SettingsFile, PASSWORD);
            DefaultFirstPlayer = new PlayerModel().Get(settings.GetValue<long>(nameof(DefaultFirstPlayer), 1)) as PlayerModel;
            DefaultSecondPlayer = new PlayerModel().Get(settings.GetValue<long>(nameof(DefaultSecondPlayer), 2)) as PlayerModel;
            DefaultGameStyle = new GameStyleModel().Get(settings.GetValue<long>(nameof(DefaultGameStyle), 1)) as GameStyleModel;
        }

        public void SaveSettings()
        {
            var settings = new SettingsManager();
            settings.SetValue(nameof(DefaultFirstPlayer), DefaultFirstPlayer.Id);
            settings.SetValue(nameof(DefaultSecondPlayer), DefaultSecondPlayer.Id);
            settings.SetValue(nameof(DefaultGameStyle), DefaultGameStyle.Id);
            settings.Save(SettingsFile, PASSWORD);
        }
    }
}
