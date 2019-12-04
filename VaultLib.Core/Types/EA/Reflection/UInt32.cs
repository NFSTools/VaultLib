// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:29 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::UInt32")]
    [PrimitiveInfo(typeof(uint))]
    public class UInt32 : PrimitiveTypeBase
    {
        public uint Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadUInt32();
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
            Value = (uint)value;
        }

        public UInt32(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public UInt32(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}