using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::EndTransition")]
public class EndTransition: VltBaseType<uint>
{
    public eEndTransitionType TransitionType { get; set; }
    public float TransitionSec { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        TransitionType = br.ReadEnum<eEndTransitionType>();
        TransitionSec = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(TransitionType);
        bw.Write(TransitionSec);
    }
}