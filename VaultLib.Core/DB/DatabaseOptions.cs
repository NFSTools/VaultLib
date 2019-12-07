namespace VaultLib.Core.DB
{
    public enum DatabaseType
    {
        /// <summary>
        /// 32-bit hashes
        /// </summary>
        X86Database,

        /// <summary>
        /// 64-bit hashes
        /// </summary>
        X64Database
    }

    public class DatabaseOptions
    {
        public DatabaseOptions(string gameId, DatabaseType type)
        {
            GameId = gameId;
            Type = type;
        }

        public string GameId { get; }

        public DatabaseType Type { get; }
    }
}