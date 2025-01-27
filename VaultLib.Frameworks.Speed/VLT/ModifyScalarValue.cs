using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT.Physics.Upgrades;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(ModifyScalarValue))]
public class ModifyScalarValue: VltBaseType<uint>
{
    public RefSpecPacked<uint> ReferencedRow { get; set; } = new();
    public bool IsMember { get; set; }
    public uint MemberIndex { get; set; }
    public bool IsElement { get; set; }
    public uint ElementIndex { get; set; }
    public eModifyValueType ModificationType { get; set; }
    public float Value { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
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

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
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
}