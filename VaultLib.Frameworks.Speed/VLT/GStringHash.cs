using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(GStringHash))]
    public class GStringHash : UInt32
    {
        public GStringHash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GStringHash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}