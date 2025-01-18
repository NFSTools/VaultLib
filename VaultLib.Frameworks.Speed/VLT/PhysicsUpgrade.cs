using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PhysicsUpgrade))]
    public class PhysicsUpgrade : VltBaseType
    {
        public PhysicsUpgrade(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ReferencedAttribute = new AttributeRefSpec(Class, Field, Collection);
        }

        public AttributeRefSpec ReferencedAttribute { get; set; }
        public bool IsMember { get; set; }
        public uint MemberIndex { get; set; }
        public float BlendingPower { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            ReferencedAttribute.Read(context, fieldContext, br);
            IsMember = br.ReadBoolean();
            br.AlignReader(4);
            MemberIndex = br.ReadUInt32();
            BlendingPower = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            ReferencedAttribute.Write(context, fieldContext, bw);
            bw.Write(IsMember);
            bw.AlignWriter(4);
            bw.Write(MemberIndex);
            bw.Write(BlendingPower);
        }
    }
}