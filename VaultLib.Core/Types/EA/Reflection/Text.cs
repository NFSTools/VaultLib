// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 8:33 PM.

using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.EA.Reflection
{
    [VltTypeInfo("EA::Reflection::Text")]
    [PrimitiveInfo(typeof(string))]
    [Obsolete("EA::Reflection::Text is deprecated, please use ReadString and WriteString helpers instead.")]
    public class Text : VltBaseType, IReferencesStrings, IStringValue
    {
        public IEnumerable<string> GetStrings()
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public string GetString()
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public void SetString(string str)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            throw new NotImplementedException("EA::Reflection::Text is no longer supported.");
        }
    }
}