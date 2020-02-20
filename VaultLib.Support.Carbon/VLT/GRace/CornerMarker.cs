using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.Carbon.VLT.GRace
{
    [VLTTypeInfo("GRace::CornerMarker")]
    public class CornerMarker : Int32
    {
        public CornerMarker(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CornerMarker(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}