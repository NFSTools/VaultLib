using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT
{
    // TODO: determine enum values
    [VltTypeInfo(nameof(eSFXOBJ_BLACKBOARD_MIXINPUT), MappedTo = typeof(int))]
    public class eSFXOBJ_BLACKBOARD_MIXINPUT : Int32
    {
        public eSFXOBJ_BLACKBOARD_MIXINPUT(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public eSFXOBJ_BLACKBOARD_MIXINPUT(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}