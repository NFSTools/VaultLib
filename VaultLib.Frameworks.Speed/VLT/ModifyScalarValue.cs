using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT.Physics.Upgrades;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(ModifyScalarValue))]
    public class ModifyScalarValue : VltBaseType
    {
        public ModifyScalarValue(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ReferencedRow = new RefSpecPacked(Class, Field, Collection);
        }

        public RefSpecPacked ReferencedRow { get; set; }
        public bool IsMember { get; set; }
        public uint MemberIndex { get; set; }
        public bool IsElement { get; set; }
        public uint ElementIndex { get; set; }
        public eModifyValueType ModificationType { get; set; }
        public float Value { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            ReferencedRow.Read(context, br);
            IsMember = br.ReadBoolean();
            br.AlignReader(4);
            MemberIndex = br.ReadUInt32();
            IsElement = br.ReadBoolean();
            br.AlignReader(4);
            ElementIndex = br.ReadUInt32();
            ModificationType = br.ReadEnum<eModifyValueType>();
            Value = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            ReferencedRow.Write(context, bw);
            bw.Write(IsMember);
            bw.AlignWriter(4);
            bw.Write(MemberIndex);
            bw.Write(IsElement);
            bw.AlignWriter(4);
            bw.Write(ElementIndex);
            bw.WriteEnum(ModificationType);
            bw.Write(Value);
        }
    }
}