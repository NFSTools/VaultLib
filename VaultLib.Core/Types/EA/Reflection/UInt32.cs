// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:29 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::UInt32")]
    [PrimitiveInfo(typeof(uint))]
    [Obsolete("EA::Reflection types are deprecated, please use type mappings instead.")]
    public class UInt32 : PrimitiveTypeBase
    {
        public UInt32(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public UInt32(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Value { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Value = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public override IConvertible GetValue()
        {
            return Value;
        }

        public override void SetValue(IConvertible value)
        {
            Value = (uint)value;
        }
    }
}