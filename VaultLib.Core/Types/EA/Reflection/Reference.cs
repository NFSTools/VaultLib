using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    // TODO: What is this type?
    [VltTypeInfo("EA::Reflection::Reference")]
    public class Reference: VltBaseType<uint>
    {
        public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }
}