// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:29 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::Int32")]
    [PrimitiveInfo(typeof(int))]
    public class Int32 : PrimitiveTypeBase
    {
        public int Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadInt32();
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
            Value = (int)value;
        }

        public Int32(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public Int32(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}