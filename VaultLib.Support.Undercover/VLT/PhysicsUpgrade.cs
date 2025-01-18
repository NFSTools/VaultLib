using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed;
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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            ReferencedAttribute.Read(context, br);
            BlendingPower = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            ReferencedAttribute.Write(context, bw);
            bw.Write(BlendingPower);
        }
    }
}