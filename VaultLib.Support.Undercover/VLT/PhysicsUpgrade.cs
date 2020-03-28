using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PhysicsUpgrade))]
    public class PhysicsUpgrade : VLTBaseType
    {
        public PhysicsUpgrade(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ReferencedAttribute = new AttributeRefSpec(Class, Field, Collection);
        }

        public AttributeRefSpec ReferencedAttribute { get; set; }
        public float BlendingPower { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedAttribute.Read(vault, br);
            BlendingPower = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedAttribute.Write(vault, bw);
            bw.Write(BlendingPower);
        }
    }
}