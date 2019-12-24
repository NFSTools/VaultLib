using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;
using VLT32Hasher = VaultLib.Core.Utils.VLT32Hasher;

namespace VaultLib.Core.Exports
{
    public abstract class BaseCollectionLoad : BaseExport, IPointerObject
    {
        /// <summary>
        ///     The collection being described by this export.
        /// </summary>
        public VltCollection Collection { get; set; }

        public string ParentKey { get; protected set; }

        public abstract void ReadPointerData(Vault vault, BinaryReader br);
        public abstract void WritePointerData(Vault vault, BinaryWriter bw);
        public abstract void AddPointers(Vault vault);

        public override string GetTypeId()
        {
            return "Attrib::CollectionLoadData";
        }

        public override ulong GetExportID()
        {
            return VLT32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
        }
    }
}