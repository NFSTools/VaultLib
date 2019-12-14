using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

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

        public override ulong GetTypeId()
        {
            return VLT32Hasher.Hash("Attrib::CollectionLoadData");
        }

        public override ulong GetExportID()
        {
            return VLT32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
        }
    }
}