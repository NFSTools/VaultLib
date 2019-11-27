// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 10:00 PM.

using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using Int32 = VaultLib.Core.Types.EA.Reflection.Int32;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(IndexedBarrierName))]
    [PrimitiveInfo(typeof(int))]
    public class IndexedBarrierName : Int32
    {
    }
}