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

        public override void Read(Vault vault, BinaryReader br)
        {
            SubjectHALId = br.ReadUInt32();
            TextHALId = br.ReadUInt32();
            _pictureText = new Text(Class, Field, Collection);
            _pictureText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(SubjectHALId);
            bw.Write(TextHALId);
            _pictureText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _pictureText.ReadPointerData(vault, br);
            Picture = _pictureText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _pictureText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _pictureText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _pictureText.GetStrings();
        }

        public FEHintsData(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEHintsData(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}