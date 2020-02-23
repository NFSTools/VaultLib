using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(PhysicsUpgrade))]
    public class PhysicsUpgrade : VLTBaseType
    {
        public PhysicsUpgrade(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PhysicsUpgrade(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public AttributeRefSpec ReferencedAttribute { get; set; }
        public bool IsMember { get; set; }
        public uint MemberIndex { get; set; }
        public float BlendingPower { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedAttribute = new AttributeRefSpec(Class, Field, Collection);
            ReferencedAttribute.Read(vault, br);
            IsMember = br.ReadBoolean();
            br.AlignReader(4);
            MemberIndex = br.ReadUInt32();
            BlendingPower = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedAttribute.Write(vault, bw);
            bw.Write(IsMember);
            bw.AlignWriter(4);
            bw.Write(MemberIndex);
            bw.Write(BlendingPower);
        }
    }
}