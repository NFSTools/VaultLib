// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::UInt8")]
    [PrimitiveInfo(typeof(byte))]
    public class UInt8 : PrimitiveTypeBase
    {
        public UInt8(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public UInt8(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }

        public byte Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadByte();
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
            Value = (byte) value;
        }
    }
}