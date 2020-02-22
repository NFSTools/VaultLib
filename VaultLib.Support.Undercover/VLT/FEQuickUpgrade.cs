using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEQuickUpgrade))]
    public class FEQuickUpgrade : VLTBaseType, IPointerObject, IReferencesStrings
    {
        public FEQuickUpgrade(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEQuickUpgrade(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float Cost { get; set; }
        public float Tier1_Cost { get; set; }
        public float Tier2_Cost { get; set; }
        public float Tier3_Cost { get; set; }
        public float Tier4_Cost { get; set; }
        public string OfferID { get; set; }
        public List<FEQuickUpgradeEntry> Entries { get; set; }

        private byte _packageLength;
        private uint _ptrPackages;
        private long _ptrPackagesSrc;
        private long _ptrPackagesDst;
        private Text _offerIdText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _offerIdText = new Text(Class, Field, Collection);
            _ptrPackages = br.ReadUInt32();
            Cost = br.ReadSingle();
            Tier1_Cost = br.ReadSingle();
            Tier2_Cost = br.ReadSingle();
            Tier3_Cost = br.ReadSingle();
            Tier4_Cost = br.ReadSingle();
            _offerIdText.Read(vault, br);
            _packageLength = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _ptrPackagesSrc = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(Cost);
            bw.Write(Tier1_Cost);
            bw.Write(Tier2_Cost);
            bw.Write(Tier3_Cost);
            bw.Write(Tier4_Cost);
            _offerIdText = new Text(Class, Field, Collection) { Value = OfferID };
            _offerIdText.Write(vault, bw);
            bw.Write((byte) Entries.Count);
            bw.AlignWriter(4);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _offerIdText.ReadPointerData(vault, br);
            OfferID = _offerIdText.Value;

            br.BaseStream.Position = _ptrPackages;

            Entries = new List<FEQuickUpgradeEntry>();

            for (int i = 0; i < _packageLength; i++)
            {
                FEQuickUpgradeEntry entry = new FEQuickUpgradeEntry(Class, Field, Collection);
                entry.Read(vault, br);

                Entries.Add(entry);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _offerIdText.WritePointerData(vault, bw);
            _ptrPackagesDst = bw.BaseStream.Position;

            foreach (var entry in Entries)
            {
                entry.Write(vault, bw);
            }
        }

        public void AddPointers(Vault vault)
        {
            Debug.Assert(_ptrPackagesSrc != 0 && _ptrPackagesDst != 0);
            vault.SaveContext.AddPointer(_ptrPackagesSrc, _ptrPackagesDst, false);
            _offerIdText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _offerIdText.GetStrings();
        }
    }
}