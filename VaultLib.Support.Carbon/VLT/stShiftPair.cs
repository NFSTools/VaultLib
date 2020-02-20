using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(stShiftPair))]
    public class stShiftPair : UInt32
    {
        public stShiftPair(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public stShiftPair(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}