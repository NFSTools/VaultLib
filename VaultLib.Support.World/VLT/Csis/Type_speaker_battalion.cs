using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_speaker_battalion")]
    public class Type_speaker_battalion : Int32
    {
        public Type_speaker_battalion(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Type_speaker_battalion(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}
