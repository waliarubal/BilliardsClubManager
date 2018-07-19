using Devart.Data.SQLite;
using NullVoidCreations.WpfHelpers;
using System.Data;
using System.IO;
using System.Windows;

namespace BilliardsClubManager
{
    class Shared
    {
        static Shared _instance;

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
