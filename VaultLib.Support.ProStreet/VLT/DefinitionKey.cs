using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(DefinitionKey), MappedTo = typeof(uint))]
    public class DefinitionKey : UInt32
    {
        public DefinitionKey(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public DefinitionKey(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}