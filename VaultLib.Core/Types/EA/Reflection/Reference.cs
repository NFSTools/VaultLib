using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    // TODO: What is this type?
    [VltTypeInfo("EA::Reflection::Reference")]
    public class Reference : VltBaseType
    {
        public Reference(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Reference(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }
}