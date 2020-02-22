using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(type_bStringHash))]
    public class type_bStringHash : UInt32
    {
        public type_bStringHash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public type_bStringHash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}