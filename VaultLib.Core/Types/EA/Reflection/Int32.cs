// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:29 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::Int32")]
    [PrimitiveInfo(typeof(int))]
    [Obsolete("EA::Reflection types are deprecated, please use type mappings instead.")]
    public class Int32 : PrimitiveTypeBase
    {
        public Int32(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Int32(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public int Value { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value = br.ReadInt32();
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
            Value = (int)value;
        }
    }
}