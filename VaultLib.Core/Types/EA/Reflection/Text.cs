// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 8:33 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::Text")]
    [PrimitiveInfo(typeof(string))]
    [Obsolete("EA::Reflection types are deprecated, please use type mappings instead.")]
    public class Text : VltBaseType, IReferencesStrings, IStringValue
    {
        public string Value { get; set; } = string.Empty;

        public IEnumerable<string> GetStrings()
        {
            return new List<string>(new[] { Value });
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            //
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            //
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            //
        }

        public string GetString()
        {
            return Value;
        }

        public void SetString(string str)
        {
            Value = str;
        }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value = context.Strings[br.ReadPointer()];
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            context.WriteString(fieldContext, Value, bw);
        }
    }
}