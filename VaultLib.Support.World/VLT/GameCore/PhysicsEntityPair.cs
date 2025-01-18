using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VltTypeInfo("GameCore::PhysicsEntityPair")]
    public class PhysicsEntityPair : VltBaseType
    {
        public PhysicsEntity Entity1 { get; set; }
        public PhysicsEntity Entity2 { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Entity1 = br.ReadEnum<PhysicsEntity>();
            Entity2 = br.ReadEnum<PhysicsEntity>();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(Entity1);
            bw.WriteEnum(Entity2);
        }

        public PhysicsEntityPair(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PhysicsEntityPair(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}