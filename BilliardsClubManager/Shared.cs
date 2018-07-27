using BilliardsClubManager.Models;
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
        static Shared _instance;
        PlayerModel _defaultFirstPlayer, _defaultSecondPlayer;
        GameStyleModel _defaultGameStyle;

        private Shared()
        {
            StartupDirectory = Application.Current.GetStartupDirectory();
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
                DataSource = Path.Combine(StartupDirectory, "Assets", "Database.sqlite3"),
                FailIfMissing = true,
                ConnectionTimeout = 20,
                Locking = LockingMode.Exclusive,
                AutoVacuum = AutoVacuumMode.Full,

            };

            var connection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
