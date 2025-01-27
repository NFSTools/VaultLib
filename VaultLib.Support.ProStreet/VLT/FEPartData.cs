// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:27 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;
using VaultLib.ModernBase;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FEPartData))]
public class FEPartData : VltBaseType<uint>, IReferencesStrings<uint>
{
    public uint HAL_ID { get; set; }
    public uint CF_HAL_ID { get; set; }
    public int Price { get; set; }
    public byte Unknown1 { get; set; }
    public byte Unknown2 { get; set; }
    public byte Unknown3 { get; set; }
    public byte Unknown4 { get; set; }
    public uint Unknown5 { get; set; }
    public uint BrandHALId { get; set; }
    public uint LogoTextureId { get; set; }

    public List<VltCollectionKey> AutoSculptCamera1 { get; set; }
    public List<VltCollectionKey> AutoSculptCamera2 { get; set; }
    public List<VltCollectionKey> AutoSculptCamera3 { get; set; }
    public VltPointerContainer<uint, FEPartDetail> PartDetails { get; set; }
    public uint DetailHash { get; set; }
    public string OfferID { get; set; } = string.Empty;

    private VltListContainer<uint, VltCollectionKey> _autoSculptCamera1;
    private VltListContainer<uint, VltCollectionKey> _autoSculptCamera2;
    private VltListContainer<uint, VltCollectionKey> _autoSculptCamera3;

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        HAL_ID = br.ReadUInt32();
        CF_HAL_ID = br.ReadUInt32();
        Price = br.ReadInt32();
        Unknown1 = br.ReadByte();
        Unknown2 = br.ReadByte();
        Unknown3 = br.ReadByte();
        Unknown4 = br.ReadByte();
        Unknown5 = br.ReadUInt32();
        BrandHALId = br.ReadUInt32();
        LogoTextureId = br.ReadUInt32();

        _autoSculptCamera1 = new VltListContainer<uint, VltCollectionKey>(br.ReadByte());
        _autoSculptCamera2 = new VltListContainer<uint, VltCollectionKey>(br.ReadByte());
        _autoSculptCamera3 = new VltListContainer<uint, VltCollectionKey>(br.ReadByte());
        byte b = br.ReadByte();

        if (b != 0)
            throw new InvalidDataException();

        _autoSculptCamera1.Read(context, fieldContext, br);
        _autoSculptCamera2.Read(context, fieldContext, br);
        _autoSculptCamera3.Read(context, fieldContext, br);

        AutoSculptCamera1 = _autoSculptCamera1.Items;
        AutoSculptCamera2 = _autoSculptCamera2.Items;
        AutoSculptCamera3 = _autoSculptCamera3.Items;

        DetailHash = br.ReadUInt32();

        PartDetails = new VltPointerContainer<uint, FEPartDetail>();
        PartDetails.Read(context, fieldContext, br);

        OfferID = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        bw.Write(HAL_ID);
        bw.Write(CF_HAL_ID);
        bw.Write(Price);
        bw.Write(Unknown1);
        bw.Write(Unknown2);
        bw.Write(Unknown3);
        bw.Write(Unknown4);
        bw.Write(Unknown5);
        bw.Write(BrandHALId);
        bw.Write(LogoTextureId);
        bw.Write((byte)AutoSculptCamera1.Count);
        bw.Write((byte)AutoSculptCamera2.Count);
        bw.Write((byte)AutoSculptCamera3.Count);
        bw.Write((byte)0);

        _autoSculptCamera1 = new VltListContainer<uint, VltCollectionKey>(AutoSculptCamera1);
        _autoSculptCamera2 = new VltListContainer<uint, VltCollectionKey>(AutoSculptCamera2);
        _autoSculptCamera3 = new VltListContainer<uint, VltCollectionKey>(AutoSculptCamera3);
        _autoSculptCamera1.Write(context, fieldContext, bw);
        _autoSculptCamera2.Write(context, fieldContext, bw);
        _autoSculptCamera3.Write(context, fieldContext, bw);
        bw.Write(DetailHash);
        PartDetails.Write(context, fieldContext, bw);

        context.WriteString(OfferID, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryReader br)
    {
        _autoSculptCamera1.ReadPointerData(context, fieldContext, br);
        _autoSculptCamera2.ReadPointerData(context, fieldContext, br);
        _autoSculptCamera3.ReadPointerData(context, fieldContext, br);
        PartDetails.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        _autoSculptCamera1.WritePointerData(context, fieldContext, bw);
        _autoSculptCamera2.WritePointerData(context, fieldContext, bw);
        _autoSculptCamera3.WritePointerData(context, fieldContext, bw);
        PartDetails.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        _autoSculptCamera1.AddPointers(context, fieldContext);
        _autoSculptCamera2.AddPointers(context, fieldContext);
        _autoSculptCamera3.AddPointers(context, fieldContext);
        PartDetails.AddPointers(context, fieldContext);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { OfferID };
    }
}