using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;
using VLT32Hasher = VaultLib.Core.Hashing.VLT32Hasher;

namespace VaultLib.Core.Exports
{
    public abstract class BaseClassLoad : BaseExport, IPointerObject
    {
        public VltClass Class { get; set; }

        public abstract void ReadPointerData(VaultLoadContext context, BinaryReader br);
        public abstract void WritePointerData(VaultSaveContext context, BinaryWriter bw);
        public abstract void AddPointers(VaultSaveContext context);

        public override ulong GetExportId()
        {
            return VLT32Hasher.Hash(Class.Name);
        }

        public override string GetTypeId()
        {
            return "Attrib::ClassLoadData";
        }
    }
}