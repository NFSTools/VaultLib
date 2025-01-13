using VaultLib.Core.Exports;

namespace VaultLib.Core;

public abstract class BaseGameModule
{
    public abstract void RegisterTypes(TypeRegistry typeRegistry);
    
    public abstract ExportFactory CreateExportFactory();
}