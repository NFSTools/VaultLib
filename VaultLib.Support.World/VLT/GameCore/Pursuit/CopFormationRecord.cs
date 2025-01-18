using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore.Pursuit
{
    [VltTypeInfo("GameCore::Pursuit::CopFormationRecord")]
    public class CopFormationRecord : Frameworks.Speed.VLT.CopFormationRecord
    {
        public CopFormationRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CopFormationRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}