using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cSamplerStateAttribDefinition")]
    public class cSamplerStateAttribDefinition : VLTBaseType, IReferencesStrings
    {
        public cSamplerStateAttribDefinition(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _nameText = new Text(Class, Field, Collection);
        }

        public string Name { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public uint Unknown6 { get; set; }
        public uint Unknown7 { get; set; }
        public uint Unknown8 { get; set; }
        public uint Unknown9 { get; set; }
        public uint Unknown10 { get; set; }
        public uint Unknown11 { get; set; }
        public uint Unknown12 { get; set; }
        public uint Unknown13 { get; set; }
        public uint Unknown14 { get; set; }
        public uint Unknown15 { get; set; }

        private Text _nameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _nameText.Read(vault, br);
            Unknown1 = br.ReadUInt32();
            Unknown2 = br.ReadUInt32();
            Unknown3 = br.ReadUInt32();
            Unknown4 = br.ReadSingle();
            Unknown5 = br.ReadUInt32();
            Unknown6 = br.ReadUInt32();
            Unknown7 = br.ReadUInt32();
            Unknown8 = br.ReadUInt32();
            Unknown9 = br.ReadUInt32();
            Unknown10 = br.ReadUInt32();
            Unknown11 = br.ReadUInt32();
            Unknown12 = br.ReadUInt32();
            Unknown13 = br.ReadUInt32();
            Unknown14 = br.ReadUInt32();
            Unknown15 = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _nameText.Value = Name;
            _nameText.Write(vault, bw);
            bw.Write(Unknown1);
            bw.Write(Unknown2);
            bw.Write(Unknown3);
            bw.Write(Unknown4);
            bw.Write(Unknown5);
            bw.Write(Unknown6);
            bw.Write(Unknown7);
            bw.Write(Unknown8);
            bw.Write(Unknown9);
            bw.Write(Unknown10);
            bw.Write(Unknown11);
            bw.Write(Unknown12);
            bw.Write(Unknown13);
            bw.Write(Unknown14);
            bw.Write(Unknown15);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _nameText.ReadPointerData(vault, br);
            Name = _nameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _nameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _nameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _nameText.GetStrings();
        }
    }
}