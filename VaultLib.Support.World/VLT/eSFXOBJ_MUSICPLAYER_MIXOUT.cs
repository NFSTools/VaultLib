using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT
{
    // TODO: determine enum values
    [VltTypeInfo(nameof(eSFXOBJ_MUSICPLAYER_MIXOUT), MappedTo = typeof(int))]
    public class eSFXOBJ_MUSICPLAYER_MIXOUT : Int32
    {
        public eSFXOBJ_MUSICPLAYER_MIXOUT(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public eSFXOBJ_MUSICPLAYER_MIXOUT(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}