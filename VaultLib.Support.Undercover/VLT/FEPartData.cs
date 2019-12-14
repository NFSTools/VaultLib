// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:27 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEPartData))]
    public class FEPartData : VLTBaseType, IPointerObject, IReferencesStrings
    {
        public uint HAL_ID { get; set; }
        public uint CF_HAL_ID { get; set; }
        public int Price { get; set; }
        public int ShowroomUnlock { get; set; }
        public int Tier1Price { get; set; }
        public int Tier2Price { get; set; }
        public int Tier3Price { get; set; }
        public int Tier4Price { get; set; }
        public byte Tier { get; set; }
        public byte Tier1ShowroomUnlock { get; set; }
        public byte Tier2ShowroomUnlock { get; set; }
        public byte Tier3ShowroomUnlock { get; set; }
        public byte Tier4ShowroomUnlock { get; set; }
        public uint BrandHALId { get; set; }
        public uint LogoTextureId { get; set; }
        public uint DetailHash { get; set; }
        public VLTPointerContainer<FEPartDetail> PartDetails { get; set; }
        public string OfferID { get; set; }
        public bool IsOnlineLockable { get; set; }

        private Text _offerIdText;

        public override void Read(Vault vault, BinaryReader br)
        {
            HAL_ID = br.ReadUInt32();
            CF_HAL_ID = br.ReadUInt32();
            Price = br.ReadInt32();
            ShowroomUnlock = br.ReadInt32();
            Tier1Price = br.ReadInt32();
            Tier2Price = br.ReadInt32();
            Tier3Price = br.ReadInt32();
            Tier4Price = br.ReadInt32();
            Tier = br.ReadByte();
            Tier1ShowroomUnlock = br.ReadByte();
            Tier2ShowroomUnlock = br.ReadByte();
            Tier3ShowroomUnlock = br.ReadByte();
            Tier4ShowroomUnlock = br.ReadByte();
            br.AlignReader(4);
            BrandHALId = br.ReadUInt32();
            LogoTextureId = br.ReadUInt32();
            DetailHash = br.ReadUInt32();
            PartDetails = new VLTPointerContainer<FEPartDetail>(Class, Field, Collection);
            PartDetails.Read(vault, br);
            _offerIdText = new Text(Class, Field, Collection);
            _offerIdText.Read(vault, br);
            IsOnlineLockable = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(HAL_ID);
            bw.Write(CF_HAL_ID);
            bw.Write(Price);
            bw.Write(ShowroomUnlock);
            bw.Write(Tier1Price);
            bw.Write(Tier2Price);
            bw.Write(Tier3Price);
            bw.Write(Tier4Price);
            bw.Write(Tier);
            bw.Write(Tier1ShowroomUnlock);
            bw.Write(Tier2ShowroomUnlock);
            bw.Write(Tier3ShowroomUnlock);
            bw.Write(Tier4ShowroomUnlock);
            bw.AlignWriter(4);
            bw.Write(BrandHALId);
            bw.Write(LogoTextureId);
            bw.Write(DetailHash);
            PartDetails.Write(vault, bw);
            _offerIdText.Write(vault, bw);
            bw.Write(IsOnlineLockable);
            bw.AlignWriter(4);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            PartDetails.ReadPointerData(vault, br);
            _offerIdText.ReadPointerData(vault, br);

            OfferID = _offerIdText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            PartDetails.WritePointerData(vault, bw);
            _offerIdText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            PartDetails.AddPointers(vault);
            _offerIdText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _offerIdText.GetStrings();
        }

        public FEPartData(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEPartData(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}