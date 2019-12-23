// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 10:03 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::Int64")]
    [PrimitiveInfo(typeof(long))]
    public class Int64 : PrimitiveTypeBase
    {
        public Int64(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Int64(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public long Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadInt64();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public override IConvertible GetValue()
        {
            return Value;
        }

        public override void SetValue(IConvertible value)
        {
            Value = (long)value;
        }
    }
}