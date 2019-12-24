using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;
using VLT32Hasher = VaultLib.Core.Utils.VLT32Hasher;

namespace VaultLib.Core.Exports
{
    public abstract class BaseClassLoad : BaseExport, IPointerObject
    {
        public VltClass Class { get; set; }

        public abstract void ReadPointerData(Vault vault, BinaryReader br);
        public abstract void WritePointerData(Vault vault, BinaryWriter bw);
        public abstract void AddPointers(Vault vault);

        public override ulong GetExportID()
        {
            return VLT32Hasher.Hash(Class.Name);
        }

        public override string GetTypeId()
        {
            return "Attrib::ClassLoadData";
        }
    }
}