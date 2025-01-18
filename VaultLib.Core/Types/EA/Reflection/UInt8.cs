// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::UInt8")]
    [PrimitiveInfo(typeof(byte))]
    [Obsolete("EA::Reflection types are deprecated, please use type mappings instead.")]
    public class UInt8 : PrimitiveTypeBase
    {
        public UInt8(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public UInt8(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public byte Value { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Value = br.ReadByte();
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
            Value = (byte)value;
        }
    }
}