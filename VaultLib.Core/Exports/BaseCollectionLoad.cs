using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports
{
    public abstract class BaseCollectionLoad : BaseExport, IPointerObject
    {
        /// <summary>
        /// The collection being described by this export.
        /// </summary>
        public VLTCollection Collection { get; set; }
        
        public override uint GetTypeId()
        {
            return VLT32Hasher.Hash("Attrib::CollectionLoadData");
        }

        public override uint GetExportID()
        {
            return VLT32Hasher.Hash($"{Collection.ClassName}/{Collection.Name}");
        }

        public abstract void ReadPointerData(Vault vault, BinaryReader br);
        public abstract void WritePointerData(Vault vault, BinaryWriter bw);
        public abstract void AddPointers(Vault vault);
    }
}