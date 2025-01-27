using VaultLib.Core.Exports;

namespace VaultLib.Core;

public abstract class BaseGameModule<TKey>
{
    public abstract void RegisterTypes(TypeRegistry<TKey> typeRegistry);
    
    public abstract ExportFactory<TKey> CreateExportFactory();
}