using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.Carbon.VLT.GRace
{
    [VLTTypeInfo("GRace::SecondaryRole")]
    public class SecondaryRole : UInt32
    {
        public SecondaryRole(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SecondaryRole(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}