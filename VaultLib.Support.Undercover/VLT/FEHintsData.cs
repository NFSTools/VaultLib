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
    [VltTypeInfo(nameof(FEHintsData))]
    public class FEHintsData : VltBaseType, IReferencesStrings
    {
        public uint SubjectHALId { get; set; }
        public uint TextHALId { get; set; }
        public string Picture { get; set; }

        private Text _pictureText;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SubjectHALId = br.ReadUInt32();
            TextHALId = br.ReadUInt32();
            _pictureText.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(SubjectHALId);
            bw.Write(TextHALId);
            _pictureText.Value = Picture;
            _pictureText.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _pictureText.ReadPointerData(context, fieldContext, br);
            Picture = _pictureText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _pictureText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _pictureText.AddPointers(context, fieldContext);
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