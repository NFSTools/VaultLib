// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:46 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VltTypeInfo("GameCore::StringKey", MappedTo = typeof(uint))]
    public class StringKey : GStringHash
    {
        public StringKey(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public StringKey(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}