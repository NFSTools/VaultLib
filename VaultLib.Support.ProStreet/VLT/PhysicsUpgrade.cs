using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
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

        public RefSpecPacked ReferencedRow { get; set; }
        public bool IsMember { get; set; }
        public uint MemberIndex { get; set; }
        public float BlendingPower { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedRow = new RefSpecPacked(Class, Field, Collection);
            ReferencedRow.Read(vault, br);
            IsMember = br.ReadBoolean();
            br.AlignReader(4);
            MemberIndex = br.ReadUInt32();
            BlendingPower = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedRow.Write(vault, bw);
            bw.Write(IsMember);
            bw.AlignWriter(4);
            bw.Write(MemberIndex);
            bw.Write(BlendingPower);
        }
    }
}