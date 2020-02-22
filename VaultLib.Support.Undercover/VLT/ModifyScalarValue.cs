using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Support.Undercover.VLT.Physics.Upgrades;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(ModifyScalarValue))]
    public class ModifyScalarValue : VLTBaseType
    {
        public ModifyScalarValue(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public ModifyScalarValue(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpecPacked ReferencedRow { get; set; }
        public bool IsMember { get; set; }
        public uint MemberIndex { get; set; }
        public bool IsElement { get; set; }
        public uint ElementIndex { get; set; }
        public eModifyValueType ModificationType { get; set; }
        public float Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedRow = new RefSpecPacked(Class, Field, Collection);
            ReferencedRow.Read(vault, br);
            IsMember = br.ReadBoolean();
            br.AlignReader(4);
            MemberIndex = br.ReadUInt32();
            IsElement = br.ReadBoolean();
            br.AlignReader(4);
            ElementIndex = br.ReadUInt32();
            ModificationType = br.ReadEnum<eModifyValueType>();
            Value = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedRow.Write(vault, bw);
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