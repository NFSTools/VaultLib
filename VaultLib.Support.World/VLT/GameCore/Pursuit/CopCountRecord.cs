// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:27 AM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore.Pursuit
{
    [VLTTypeInfo("GameCore::Pursuit::CopCountRecord")]
    public class CopCountRecord : Frameworks.Speed.CopCountRecord
    {
        public CopCountRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CopCountRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}