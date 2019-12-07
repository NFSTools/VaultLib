using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports
{
    public abstract class BaseClassLoad : BaseExport, IPointerObject
    {
        public VLTClass Class { get; set; }

        public abstract void ReadPointerData(Vault vault, BinaryReader br);
        public abstract void WritePointerData(Vault vault, BinaryWriter bw);
        public abstract void AddPointers(Vault vault);

        public override uint GetExportID()
        {
            return VLT32Hasher.Hash(Class.Name);
        }

        public override uint GetTypeId()
        {
            return VLT32Hasher.Hash("Attrib::ClassLoadData");
        }
    }
}