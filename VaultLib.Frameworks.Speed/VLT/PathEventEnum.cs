using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(PathEventEnum))]
    public class PathEventEnum : Int32
    {
        public PathEventEnum(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PathEventEnum(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}