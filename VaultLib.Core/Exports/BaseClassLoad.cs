using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports;

public abstract class BaseClassLoad : BaseExport, IPointerObject
{
    public VltClass Class { get; set; }

    public abstract void ReadPointerData(VaultReadContext context, BinaryReader br);
    public abstract void WritePointerData(VaultWriteContext context, BinaryWriter bw);
    public abstract void AddPointers(VaultWriteContext context);

    public override ulong GetExportId()
    {
        return Vlt32Hasher.Hash(Class.Name);
    }

    public override string GetTypeId()
    {
        return "Attrib::ClassLoadData";
    }
}