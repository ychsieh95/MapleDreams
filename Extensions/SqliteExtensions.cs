using Microsoft.Data.Sqlite;

namespace MapleDreams.Extensions
{
    public static class SqliteExtensions
    {
        /// <summary>
        /// 資料表結構，於資料庫或資料表不存在時建立
        /// </summary>
        private static readonly string[] tableSchemas =
        {
            "CREATE TABLE IF NOT EXISTS Catalysts (Name TEXT, Type INTEGER, SerialNumber INTEGER, Guid TEXT);",
            "CREATE TABLE IF NOT EXISTS Stones (Name TEXT, Type INTEGER, SerialNumber INTEGER, Guid TEXT);",
            "CREATE TABLE IF NOT EXISTS Monsters (Name TEXT, Level INTEGER, Location TEXT, Catalyst TEXT, Stone TEXT, Guid TEXT);"
        };

        /// <summary>
        /// 確保 SQLite 資料庫與其資料表存在，不存在時建立之
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="logger">記錄器，null 表示不記錄，預設為 null</param>
        public static void EnsureSqliteDatabaseCreated(this string connectionString, ILogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'MapleDreamsConnection' is not configured.");

            var builder = new SqliteConnectionStringBuilder(connectionString);
            bool onDisk = builder.Mode != SqliteOpenMode.Memory && !builder.DataSource.Equals(":memory:", StringComparison.OrdinalIgnoreCase);
            string dataSource = onDisk ? Path.GetFullPath(builder.DataSource) : null;

            // Create the directory holding the database file, SQLite only creates the file itself
            if (onDisk)
            {
                string directory = Path.GetDirectoryName(dataSource);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            bool created = onDisk && !File.Exists(dataSource);

            // Opening the connection creates the database file when it does not exist
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                foreach (string schema in tableSchemas)
                {
                    using var command = conn.CreateCommand();
                    command.CommandText = schema;
                    command.ExecuteNonQuery();
                }
            }

            if (created)
                logger?.LogInformation("Created a new SQLite database at {DataSource}.", dataSource);
        }
    }
}
