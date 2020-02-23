using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore.Pursuit
{
    [VLTTypeInfo("GameCore::Pursuit::CopFormationRecord")]
    public class CopFormationRecord : Frameworks.Speed.CopFormationRecord
    {
        public CopFormationRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CopFormationRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}