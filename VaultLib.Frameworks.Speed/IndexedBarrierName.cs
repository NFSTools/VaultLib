// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 10:00 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(IndexedBarrierName))]
    public class IndexedBarrierName : Int32
    {
        public IndexedBarrierName(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public IndexedBarrierName(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}