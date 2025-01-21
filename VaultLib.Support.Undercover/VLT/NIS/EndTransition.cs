using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::EndTransition")]
public class EndTransition : VltBaseType
{
    public eEndTransitionType TransitionType { get; set; }
    public float TransitionSec { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        TransitionType = br.ReadEnum<eEndTransitionType>();
        TransitionSec = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(TransitionType);
        bw.Write(TransitionSec);
    }
}