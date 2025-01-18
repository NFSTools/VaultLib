using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    // TODO: What is this type?
    [VltTypeInfo("EA::Reflection::Reference")]
    public class Reference : VltBaseType
    {
        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }
}