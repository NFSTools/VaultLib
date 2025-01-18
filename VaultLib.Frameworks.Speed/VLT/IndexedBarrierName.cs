// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 10:00 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(IndexedBarrierName), MappedTo = typeof(short))]
    public class IndexedBarrierName : Int16
    {
        public IndexedBarrierName(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public IndexedBarrierName(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}