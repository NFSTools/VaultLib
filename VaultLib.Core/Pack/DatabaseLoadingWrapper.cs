using System;
using VaultLib.Core.DB;

namespace VaultLib.Core.Pack;

/// <summary>
/// Simple disposable class that calls <see cref="Database.CompleteLoad"/> so you don't have to.
/// </summary>
public class DatabaseLoadingWrapper<TKey> : IDisposable
{
    private readonly Database<TKey> _database;

    public DatabaseLoadingWrapper(Database<TKey> database)
    {
        _database = database;
    }

    public void Dispose()
    {
        _database.CompleteLoad();
    }
}