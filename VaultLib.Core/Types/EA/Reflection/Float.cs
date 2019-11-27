// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:32 PM.

using System;
using System.IO;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::Float")]
    [PrimitiveInfo(typeof(float))]
    public class Float : PrimitiveTypeBase
    {
        public float Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadSingle();
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
            Value = (float)value;
        }
    }
}