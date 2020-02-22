using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cRenderTargetAttribDefinition")]
    public class cRenderTargetAttribDefinition : VLTBaseType
    {
        public cRenderTargetAttribDefinition(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public cRenderTargetAttribDefinition(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }
}