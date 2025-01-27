using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core;

public abstract class BaseGameModule<TKey> where TKey : IKey<TKey>
{
    public abstract void RegisterTypes(TypeRegistry<TKey> typeRegistry);
    
    public abstract ExportFactory<TKey> CreateExportFactory();
}