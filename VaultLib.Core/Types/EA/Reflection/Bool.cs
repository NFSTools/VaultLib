﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:27 PM.

using System;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VLTTypeInfo("EA::Reflection::Bool")]
    [PrimitiveInfo(typeof(bool))]
    public class Bool : PrimitiveTypeBase
    {
        public Bool(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Bool(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public bool Value { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadBoolean();
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
            Value = (bool)value;
        }
    }
}