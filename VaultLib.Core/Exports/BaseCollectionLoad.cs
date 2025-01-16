using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;
using VLT32Hasher = VaultLib.Core.Hashing.VLT32Hasher;

namespace VaultLib.Core.Exports
{
    public abstract class BaseCollectionLoad : BaseExport, IPointerObject
    {
        /// <summary>
        ///     The collection being described by this export.
        /// </summary>
        public VltCollection Collection { get; set; }

        public ulong ParentKey { get; protected set; }

        public abstract void ReadPointerData(VaultLoadContext context, BinaryReader br);
        public abstract void WritePointerData(VaultSaveContext context, BinaryWriter bw);
        public abstract void AddPointers(VaultSaveContext context);

        public override string GetTypeId()
        {
            return "Attrib::CollectionLoadData";
        }

        public override ulong GetExportId()
        {
            return VLT32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
        }
    }
}