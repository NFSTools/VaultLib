using System;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Exports;

public abstract class BaseDatabaseLoad<TKey> : BaseExport<TKey> where TKey : IKey<TKey>
{
    public override TKey GetExportId()
    {
        throw new NotImplementedException();
        // return 0xF1DFAC8D; // constant, probably doesn't matter but I don't know the text it comes from
    }

    public override string GetTypeId()
    {
        return "Attrib::DatabaseLoadData";
    }
}