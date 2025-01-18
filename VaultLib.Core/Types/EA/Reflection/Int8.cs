// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::Int8")]
    [PrimitiveInfo(typeof(sbyte))]
    public sealed class Int8 : PrimitiveTypeBase
    {
        public Int8(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Int8(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public sbyte Value { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value = br.ReadSByte();
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
            Value = (sbyte)value;
        }
    }
}