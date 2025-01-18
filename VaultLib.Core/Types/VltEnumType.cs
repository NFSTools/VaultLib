// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:23 PM.

using CoreLibraries.IO;
using System;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Core.Types
{
    public class VltEnumType<T> : PrimitiveTypeBase where T : IConvertible
    {
        public VltEnumType(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        public VltEnumType(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public T Value { get; set; }

        public sealed override void Read(VaultReadContext context, BinaryReader br)
        {
            Value = br.ReadEnum<T>();
        }

        public sealed override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.WriteEnum(Value);
        }

        public override IConvertible GetValue()
        {
            return Value;
        }

        public override void SetValue(IConvertible value)
        {
            Value = (T)value;
        }
    }
}