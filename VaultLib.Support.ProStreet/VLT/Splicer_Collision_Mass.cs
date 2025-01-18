using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(Splicer_Collision_Mass), MappedTo = typeof(int))]
    public class Splicer_Collision_Mass : Int32
    {
        public Splicer_Collision_Mass(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Splicer_Collision_Mass(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}
