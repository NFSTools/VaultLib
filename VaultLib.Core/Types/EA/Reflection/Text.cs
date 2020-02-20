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
    [VLTTypeInfo("EA::Reflection::Text")]
    [PrimitiveInfo(typeof(string))]
    public class Text : PrimitiveTypeBase, IReferencesStrings, IStringValue
    {
        private long _internalPointerDst;

        private long _internalPointerSrc;

        public Text(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Text(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public string Value { get; set; }

        private uint Pointer { get; set; }

        public IEnumerable<string> GetStrings()
        {
            return new List<string>(new[] { Value });
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Debug.Assert(Pointer != 0);
            br.BaseStream.Position = Pointer;
            Value = NullTerminatedString.Read(br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _internalPointerDst = vault.SaveContext.StringOffsets[Value];
        }

        public void AddPointers(Vault vault)
        {
            Debug.Assert(_internalPointerSrc != 0 && _internalPointerDst != 0);

            vault.SaveContext.AddPointer(_internalPointerSrc, _internalPointerDst, IsInVLT);
        }

        public string GetString()
        {
            return Value;
        }

        public void SetString(string str)
        {
            Value = str;
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            Debug.Assert(Class != null, "this.Class != null");

            // NOTE 11.01.19: since Text can be in static data, we cannot require a collection
            //Debug.Assert(this.Collection != null, "this.Collection != null");
            Debug.Assert(Field != null, "this.Field != null");
            Pointer = br.ReadPointer();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _internalPointerSrc = bw.BaseStream.Position;
            bw.Write(0);
        }

        public void Bootstrap()
        {
            Value = string.Empty;
        }

        public override IConvertible GetValue()
        {
            return GetString();
        }

        public override void SetValue(IConvertible value)
        {
            SetString((string)value);
        }
    }
}