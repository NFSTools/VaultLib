// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:08 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEHintsData))]
    public class FEHintsData : VLTBaseType, IReferencesStrings
    {
        public uint SubjectHALId { get; set; }
        public uint TextHALId { get; set; }
        public string Picture { get; set; }

        private Text _pictureText;

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            SubjectHALId = br.ReadUInt32();
            TextHALId = br.ReadUInt32();
            _pictureText.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(SubjectHALId);
            bw.Write(TextHALId);
            _pictureText.Value = Picture;
            _pictureText.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _pictureText.ReadPointerData(context, br);
            Picture = _pictureText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _pictureText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _pictureText.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return _pictureText.GetStrings();
        }

        public FEHintsData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _pictureText = new Text(Class, Field, Collection);
            Picture = string.Empty;
        }
    }
}