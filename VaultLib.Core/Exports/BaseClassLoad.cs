using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports;

public abstract class BaseClassLoad<TKey> : BaseExport<TKey>, IPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    public VltClass<TKey> Class { get; set; }

    public abstract void ReadPointerData(VaultReadContext<TKey> context, BinaryReader br);
    public abstract void WritePointerData(VaultWriteContext<TKey> context, BinaryWriter bw);
    public abstract void AddPointers(VaultWriteContext<TKey> context);

    // public override ulong GetExportId()
    // {
    //     return Vlt32Hasher.Hash(Class.Name);
    // }

    public abstract override TKey GetExportId();

    public override string GetTypeId()
    {
        return "Attrib::ClassLoadData";
    }
}