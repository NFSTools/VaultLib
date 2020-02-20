using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(DISTORTION_BASIS))]
    public class DISTORTION_BASIS : Int32
    {
        public DISTORTION_BASIS(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public DISTORTION_BASIS(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}