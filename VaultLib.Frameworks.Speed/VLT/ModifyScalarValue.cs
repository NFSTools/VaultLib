using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT.Physics.Upgrades;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(ModifyScalarValue))]
public class ModifyScalarValue : VltBaseType<Key32>
{
    public RefSpecPacked32 ReferencedRow { get; set; } = new();
    public bool IsMember { get; set; }
    public uint MemberIndex { get; set; }
    public bool IsElement { get; set; }
    public uint ElementIndex { get; set; }
    public eModifyValueType ModificationType { get; set; }
    public float Value { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        ReferencedRow.Read(context, fieldContext, br);
        IsMember = br.ReadBoolean();
        br.SafeAlignReader(4);
        MemberIndex = br.ReadUInt32();
        IsElement = br.ReadBoolean();
        br.SafeAlignReader(4);
        ElementIndex = br.ReadUInt32();
        ModificationType = br.ReadEnum<eModifyValueType>();
        Value = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        ReferencedRow.Write(context, fieldContext, bw);
        bw.Write(IsMember);
        bw.AlignWriter(4);
        bw.Write(MemberIndex);
        bw.Write(IsElement);
        bw.AlignWriter(4);
        bw.Write(ElementIndex);
        bw.WriteEnum(ModificationType);
        bw.Write(Value);
    }

    public override object Clone()
    {
        return new ModifyScalarValue
        {
            ElementIndex = this.ElementIndex,
            IsElement = this.IsElement,
            IsMember = this.IsMember,
            MemberIndex = this.MemberIndex,
            ModificationType = this.ModificationType,
            ReferencedRow = (RefSpecPacked32)this.ReferencedRow.Clone(),
            Value = this.Value,
        };
    }
}