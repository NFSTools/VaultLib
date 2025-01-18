// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:12 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public abstract class VltBaseType
    {
        public abstract void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br);
        public abstract void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw);
    }
}