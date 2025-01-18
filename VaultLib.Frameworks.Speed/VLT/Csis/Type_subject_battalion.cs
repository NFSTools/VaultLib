using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed.VLT.Csis
{
    [VltTypeInfo("Csis::Type_subject_battalion", MappedTo = typeof(uint))]
    public class Type_subject_battalion : UInt32
    {
        public Type_subject_battalion(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Type_subject_battalion(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}