// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::Int8")]
    public class Int8 : PrimitiveTypeBase
    {
        public sbyte Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadSByte();
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
            Value = (sbyte)value;
        }
    }
}