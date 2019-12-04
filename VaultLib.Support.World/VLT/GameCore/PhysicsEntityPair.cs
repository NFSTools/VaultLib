using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::PhysicsEntityPair")]
    public class PhysicsEntityPair : VLTBaseType
    {
        public PhysicsEntity Entity1 { get; set; }
        public PhysicsEntity Entity2 { get; set; }
        
        public override void Read(Vault vault, BinaryReader br)
        {
            Entity1 = br.ReadEnum<PhysicsEntity>();
            Entity2 = br.ReadEnum<PhysicsEntity>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Entity1);
            bw.WriteEnum(Entity2);
        }

        public PhysicsEntityPair(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public PhysicsEntityPair(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}