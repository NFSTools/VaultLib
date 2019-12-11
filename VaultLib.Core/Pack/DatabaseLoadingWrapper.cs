using System;
using VaultLib.Core.DB;

namespace VaultLib.Core.Pack
{
    /// <summary>
    /// Simple disposable class that calls <see cref="Database.CompleteLoad"/> so you don't have to.
    /// </summary>
    public class DatabaseLoadingWrapper : IDisposable
    {
        private readonly Database _database;

        public DatabaseLoadingWrapper(Database database)
        {
            _database = database;
        }

        public void Dispose()
        {
            _database.CompleteLoad();
        }
    }
}