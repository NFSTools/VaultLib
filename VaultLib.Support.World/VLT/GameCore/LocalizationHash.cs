using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::LocalizationHash")]
    public class LocalizationHash : UInt32
    {
        public LocalizationHash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public LocalizationHash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}