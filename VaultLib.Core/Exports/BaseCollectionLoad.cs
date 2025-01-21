using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports;

public abstract class BaseCollectionLoad : BaseExport, IPointerObject
{
    /// <summary>
    ///     The collection being described by this export.
    /// </summary>
    public VltCollection Collection { get; set; }

    public ulong ParentKey { get; protected set; }

    public abstract void ReadPointerData(VaultReadContext context, BinaryReader br);
    public abstract void WritePointerData(VaultWriteContext context, BinaryWriter bw);
    public abstract void AddPointers(VaultWriteContext context);

    public override string GetTypeId()
    {
        return "Attrib::CollectionLoadData";
    }

    public override ulong GetExportId()
    {
        return Vlt32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
    }
}