using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    // TODO: What is this type?
    [VLTTypeInfo("EA::Reflection::Reference")]
    public class Reference : VLTBaseType
    {
        public Reference(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Reference(VltClass @class, VltClassField field) : base(@class, field)
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