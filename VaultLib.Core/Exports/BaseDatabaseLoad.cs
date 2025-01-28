using System;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Exports;

public abstract class BaseDatabaseLoad<TKey> : BaseExport<TKey> where TKey : IKey<TKey>
{
    public abstract override TKey GetExportId();

    public override string GetTypeId()
    {
        return "Attrib::DatabaseLoadData";
    }
}