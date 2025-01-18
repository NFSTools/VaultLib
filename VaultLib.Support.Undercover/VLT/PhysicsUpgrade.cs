using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(PhysicsUpgrade))]
    public class PhysicsUpgrade : VltBaseType
    {
        public PhysicsUpgrade(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ReferencedAttribute = new AttributeRefSpec(Class, Field, Collection);
        }

        public AttributeRefSpec ReferencedAttribute { get; set; }
        public float BlendingPower { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            ReferencedAttribute.Read(context, fieldContext, br);
            BlendingPower = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            ReferencedAttribute.Write(context, fieldContext, bw);
            bw.Write(BlendingPower);
        }
    }
}