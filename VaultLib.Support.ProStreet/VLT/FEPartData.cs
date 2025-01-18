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
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(FEPartData))]
    public class FEPartData : VltBaseType, IPointerObject, IReferencesStrings
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

        public VltListContainer<RefSpec> AutoSculptCamera1 { get; set; }
        public VltListContainer<RefSpec> AutoSculptCamera2 { get; set; }
        public VltListContainer<RefSpec> AutoSculptCamera3 { get; set; }
        public VltPointerContainer<FEPartDetail> PartDetails { get; set; }
        public uint DetailHash { get; set; }
        public string OfferID { get; set; }

        private Text _offerIdText;

        public override void Read(VaultLoadContext context, BinaryReader br)
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

            AutoSculptCamera1 = new VltListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            AutoSculptCamera2 = new VltListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            AutoSculptCamera3 = new VltListContainer<RefSpec>(Class, Field, Collection, br.ReadByte());
            byte b = br.ReadByte();

            if (b != 0)
                throw new InvalidDataException();

            AutoSculptCamera1.Read(context, br);
            AutoSculptCamera2.Read(context, br);
            AutoSculptCamera3.Read(context, br);

            DetailHash = br.ReadUInt32();

            PartDetails = new VltPointerContainer<FEPartDetail>(Class, Field, Collection);
            PartDetails.Read(context, br);
            _offerIdText.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
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
            AutoSculptCamera1.Write(context, bw);
            AutoSculptCamera2.Write(context, bw);
            AutoSculptCamera3.Write(context, bw);
            bw.Write(DetailHash);
            PartDetails.Write(context, bw);
            _offerIdText.Value = OfferID;
            _offerIdText.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            AutoSculptCamera1.ReadPointerData(context, br);
            AutoSculptCamera2.ReadPointerData(context, br);
            AutoSculptCamera3.ReadPointerData(context, br);
            PartDetails.ReadPointerData(context, br);
            _offerIdText.ReadPointerData(context, br);

            OfferID = _offerIdText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            AutoSculptCamera1.WritePointerData(context, bw);
            AutoSculptCamera2.WritePointerData(context, bw);
            AutoSculptCamera3.WritePointerData(context, bw);
            PartDetails.WritePointerData(context, bw);
            _offerIdText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            AutoSculptCamera1.AddPointers(context);
            AutoSculptCamera2.AddPointers(context);
            AutoSculptCamera3.AddPointers(context);
            PartDetails.AddPointers(context);
            _offerIdText.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return _offerIdText.GetStrings();
        }

        public FEPartData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _offerIdText = new Text(Class, Field, Collection);
            OfferID = string.Empty;
        }
    }
}