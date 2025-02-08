using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core;

public abstract class BaseGameModule<TKey> where TKey : struct, IKey<TKey>
{
    public abstract void RegisterTypes(TypeRegistryBuilder<TKey> typeRegistry);
    
    public abstract ExportFactory<TKey> CreateExportFactory();
}