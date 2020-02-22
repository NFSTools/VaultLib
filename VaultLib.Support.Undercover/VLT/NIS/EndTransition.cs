using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::EndTransition")]
    public class EndTransition : VLTBaseType
    {
        public EndTransition(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public EndTransition(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eEndTransitionType TransitionType { get; set; }
        public float TransitionSec { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            TransitionType = br.ReadEnum<eEndTransitionType>();
            TransitionSec = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(TransitionType);
            bw.Write(TransitionSec);
        }
    }
}