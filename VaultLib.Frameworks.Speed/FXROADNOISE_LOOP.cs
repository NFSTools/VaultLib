using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(FXROADNOISE_LOOP))]
    public class FXROADNOISE_LOOP : Int32
    {
        public FXROADNOISE_LOOP(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FXROADNOISE_LOOP(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}