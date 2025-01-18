// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::UInt16")]
    [PrimitiveInfo(typeof(ushort))]
    [Obsolete("EA::Reflection types are deprecated, please use type mappings instead.")]
    public class UInt16 : PrimitiveTypeBase
    {
        public UInt16(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public UInt16(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public ushort Value { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value = br.ReadUInt16();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public override IConvertible GetValue()
        {
            return Value;
        }

        public override void SetValue(IConvertible value)
        {
            Value = (ushort)value;
        }
    }
}