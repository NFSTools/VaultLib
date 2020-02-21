using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(FXROADNOISE_TRANSITION))]
    public class FXROADNOISE_TRANSITION : Int32
    {
        public FXROADNOISE_TRANSITION(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FXROADNOISE_TRANSITION(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}