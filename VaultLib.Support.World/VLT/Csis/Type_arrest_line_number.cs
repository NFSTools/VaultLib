using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_arrest_line_number")]
    public class Type_arrest_line_number : UInt32
    {
        public Type_arrest_line_number(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Type_arrest_line_number(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}