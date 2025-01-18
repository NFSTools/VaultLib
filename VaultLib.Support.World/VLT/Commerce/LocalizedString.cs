// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:27 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.Commerce
{
    [VltTypeInfo("Commerce::LocalizedString")]
    public class LocalizedString : VltBaseType, IReferencesStrings, IStringValue
    {
        private Text _text;

        public string Value { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _text.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _text.Value = Value;
            _text.Write(context, bw);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { Value };
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _text.ReadPointerData(context, br);
            Value = _text.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _text.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _text.AddPointers(context);
        }

        public string GetString()
        {
            return Value;
        }

        public void SetString(string str)
        {
            Value = str;
        }

        public LocalizedString(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _text = new Text(Class, Field, Collection);
            Value = string.Empty;
        }
    }
}