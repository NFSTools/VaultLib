using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::EndTransition")]
public class EndTransition: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public eEndTransitionType TransitionType { get; set; }
    public float TransitionSec { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TransitionType = br.ReadEnum<eEndTransitionType>();
        TransitionSec = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(TransitionType);
        bw.Write(TransitionSec);
    }
}