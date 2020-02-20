using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.Carbon.VLT.GRace
{
    [VLTTypeInfo("GRace::PrimaryRole")]
    public class PrimaryRole : UInt32
    {
        public PrimaryRole(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PrimaryRole(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}