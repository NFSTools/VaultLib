using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib
{
    // For information about the blob system, go to: https://github.com/NFSTools/VaultLib/issues/1
    [VLTTypeInfo("Attrib::Blob")]
    public class Blob : BaseBlob
    {
        public Blob(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Blob(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        protected override byte[] PrepareData()
        {
            return Data;
        }
    }
}