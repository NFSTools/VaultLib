// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:27 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(FEPartData))]
    public class FEPartData : VLTBaseType, IPointerObject, IReferencesStrings
    {
        public uint HAL_ID { get; set; }
        public uint CF_HAL_ID { get; set; }
        public int Price { get; set; }
        public bool Drift { get; set; }
        public bool Drag { get; set; }
        public bool Grip { get; set; }
        public bool Speed { get; set; }
        public uint Tier { get; set; }
        public uint BrandHALId { get; set; }
        public uint LogoTextureId { get; set; }

        public VLTListContainer<RefSpec> AutoSculptCamera1 { get; set; }
        public VLTListContainer<RefSpec> AutoSculptCamera2 { get; set; }
        public VLTListContainer<RefSpec> AutoSculptCamera3 { get; set; }
        public VLTPointerContainer<FEPartDetail> PartDetails { get; set; }
        public uint DetailHash { get; set; }
        public string OfferID { get; set; }

        private Text _offerIdText;

        public override void Read(Vault vault, BinaryReader br)
        {
            HAL_ID = br.ReadUInt32();
            CF_HAL_ID = br.ReadUInt32();
            Price = br.ReadInt32();
            Drift = br.ReadBoolean();
            Drag = br.ReadBoolean();
            Grip = br.ReadBoolean();
            Speed = br.ReadBoolean();
            Tier = br.ReadUInt32();
            BrandHALId = br.ReadUInt32();
            LogoTextureId = br.ReadUInt32();

            AutoSculptCamera1 = new VLTListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            AutoSculptCamera2 = new VLTListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            AutoSculptCamera3 = new VLTListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            byte b = br.ReadByte();

            if (b != 0)
                throw new InvalidDataException();

            AutoSculptCamera1.Read(vault, br);
            AutoSculptCamera2.Read(vault, br);
            AutoSculptCamera3.Read(vault, br);

            DetailHash = br.ReadUInt32();

            PartDetails = new VLTPointerContainer<FEPartDetail>(Class, Field, Collection);
            PartDetails.Read(vault, br);
            _offerIdText = new Text(Class, Field, Collection);
            _offerIdText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(HAL_ID);
            bw.Write(CF_HAL_ID);
            bw.Write(Price);
            bw.Write(Drift);
            bw.Write(Drag);
            bw.Write(Grip);
            bw.Write(Speed);
            bw.Write(Tier);
            bw.Write(BrandHALId);
            bw.Write(LogoTextureId);
            bw.Write((byte)AutoSculptCamera1.Items.Count);
            bw.Write((byte)AutoSculptCamera2.Items.Count);
            bw.Write((byte)AutoSculptCamera3.Items.Count);
            bw.Write((byte)0);
            AutoSculptCamera1.Write(vault, bw);
            AutoSculptCamera2.Write(vault, bw);
            AutoSculptCamera3.Write(vault, bw);
            bw.Write(DetailHash);
            PartDetails.Write(vault, bw);
            _offerIdText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            AutoSculptCamera1.ReadPointerData(vault, br);
            AutoSculptCamera2.ReadPointerData(vault, br);
            AutoSculptCamera3.ReadPointerData(vault, br);
            PartDetails.ReadPointerData(vault, br);
            _offerIdText.ReadPointerData(vault, br);

            OfferID = _offerIdText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            AutoSculptCamera1.WritePointerData(vault, bw);
            AutoSculptCamera2.WritePointerData(vault, bw);
            AutoSculptCamera3.WritePointerData(vault, bw);
            PartDetails.WritePointerData(vault, bw);
            _offerIdText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            AutoSculptCamera1.AddPointers(vault);
            AutoSculptCamera2.AddPointers(vault);
            AutoSculptCamera3.AddPointers(vault);
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