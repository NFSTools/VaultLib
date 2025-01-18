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
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(FEPartData))]
    public class FEPartData : VltBaseType, IVltPointerObject, IReferencesStrings
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

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
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

            AutoSculptCamera1.Read(context, fieldContext, br);
            AutoSculptCamera2.Read(context, fieldContext, br);
            AutoSculptCamera3.Read(context, fieldContext, br);

            DetailHash = br.ReadUInt32();

            PartDetails = new VltPointerContainer<FEPartDetail>(Class, Field, Collection);
            PartDetails.Read(context, fieldContext, br);
            _offerIdText.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
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
            AutoSculptCamera1.Write(context, fieldContext, bw);
            AutoSculptCamera2.Write(context, fieldContext, bw);
            AutoSculptCamera3.Write(context, fieldContext, bw);
            bw.Write(DetailHash);
            PartDetails.Write(context, fieldContext, bw);
            _offerIdText.Value = OfferID;
            _offerIdText.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            AutoSculptCamera1.ReadPointerData(context, fieldContext, br);
            AutoSculptCamera2.ReadPointerData(context, fieldContext, br);
            AutoSculptCamera3.ReadPointerData(context, fieldContext, br);
            PartDetails.ReadPointerData(context, fieldContext, br);
            _offerIdText.ReadPointerData(context, fieldContext, br);

            OfferID = _offerIdText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            AutoSculptCamera1.WritePointerData(context, fieldContext, bw);
            AutoSculptCamera2.WritePointerData(context, fieldContext, bw);
            AutoSculptCamera3.WritePointerData(context, fieldContext, bw);
            PartDetails.WritePointerData(context, fieldContext, bw);
            _offerIdText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            AutoSculptCamera1.AddPointers(context, fieldContext);
            AutoSculptCamera2.AddPointers(context, fieldContext);
            AutoSculptCamera3.AddPointers(context, fieldContext);
            PartDetails.AddPointers(context, fieldContext);
            _offerIdText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _offerIdText.GetStrings();
        }

        public FEPartData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field,
            collection)
        {
            _offerIdText = new Text(Class, Field, Collection);
            OfferID = string.Empty;
        }
    }
}