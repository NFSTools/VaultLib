using CoreLibraries.GameUtilities;

namespace VaultLib.Core.Exports
{
    public abstract class BaseDatabaseLoad : BaseExport
    {
        public override uint GetExportID()
        {
            return 0xF1DFAC8D; // constant, probably doesn't matter but I don't know the text it comes from
        }

        public override uint GetTypeId()
        {
            return VLT32Hasher.Hash("Attrib::DatabaseLoadData");
        }
    }
}