// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:27 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FEPartData))]
public class FEPartData : VltBaseType<Key32>, IReferencesStrings, IVltPointerObject<Key32>
{
    public BinKey32 HAL_ID { get; set; }
    public BinKey32 CF_HAL_ID { get; set; }
    public int Price { get; set; }
    public byte Unknown1 { get; set; }
    public byte Unknown2 { get; set; }
    public byte Unknown3 { get; set; }
    public byte Unknown4 { get; set; }
    public uint Unknown5 { get; set; }
    public BinKey32 BrandHALId { get; set; }
    public BinKey32 LogoTextureId { get; set; }

    public List<Key32> AutoSculptCamera1 { get; set; }
    public List<Key32> AutoSculptCamera2 { get; set; }
    public List<Key32> AutoSculptCamera3 { get; set; }
    public VltPointerContainer<Key32, FEPartDetail> PartDetails { get; set; }
    public uint DetailHash { get; set; }
    public string OfferID { get; set; } = string.Empty;

    private VltListContainer<Key32, Key32> _autoSculptCamera1;
    private VltListContainer<Key32, Key32> _autoSculptCamera2;
    private VltListContainer<Key32, Key32> _autoSculptCamera3;

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        HAL_ID = BinKey32.Read(br);
        CF_HAL_ID = BinKey32.Read(br);
        Price = br.ReadInt32();
        Unknown1 = br.ReadByte();
        Unknown2 = br.ReadByte();
        Unknown3 = br.ReadByte();
        Unknown4 = br.ReadByte();
        Unknown5 = br.ReadUInt32();
        BrandHALId = BinKey32.Read(br);
        LogoTextureId = BinKey32.Read(br);

        _autoSculptCamera1 = new VltListContainer<Key32, Key32>(br.ReadByte());
        _autoSculptCamera2 = new VltListContainer<Key32, Key32>(br.ReadByte());
        _autoSculptCamera3 = new VltListContainer<Key32, Key32>(br.ReadByte());
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

        PartDetails = new VltPointerContainer<Key32, FEPartDetail>();
        PartDetails.Read(context, fieldContext, br);

        OfferID = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        HAL_ID.Write(bw);
        CF_HAL_ID.Write(bw);
        bw.Write(Price);
        bw.Write(Unknown1);
        bw.Write(Unknown2);
        bw.Write(Unknown3);
        bw.Write(Unknown4);
        bw.Write(Unknown5);
        BrandHALId.Write(bw);
        LogoTextureId.Write(bw);
        bw.Write((byte)AutoSculptCamera1.Count);
        bw.Write((byte)AutoSculptCamera2.Count);
        bw.Write((byte)AutoSculptCamera3.Count);
        bw.Write((byte)0);

        _autoSculptCamera1 = new VltListContainer<Key32, Key32>(AutoSculptCamera1);
        _autoSculptCamera2 = new VltListContainer<Key32, Key32>(AutoSculptCamera2);
        _autoSculptCamera3 = new VltListContainer<Key32, Key32>(AutoSculptCamera3);
        _autoSculptCamera1.Write(context, fieldContext, bw);
        _autoSculptCamera2.Write(context, fieldContext, bw);
        _autoSculptCamera3.Write(context, fieldContext, bw);
        bw.Write(DetailHash);
        PartDetails.Write(context, fieldContext, bw);

        context.WriteString(OfferID, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        _autoSculptCamera1.ReadPointerData(context, fieldContext, br);
        _autoSculptCamera2.ReadPointerData(context, fieldContext, br);
        _autoSculptCamera3.ReadPointerData(context, fieldContext, br);
        PartDetails.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        _autoSculptCamera1.WritePointerData(context, fieldContext, bw);
        _autoSculptCamera2.WritePointerData(context, fieldContext, bw);
        _autoSculptCamera3.WritePointerData(context, fieldContext, bw);
        PartDetails.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext)
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

    public override object Clone()
    {
        return new FEPartData
        {
            AutoSculptCamera1 = new List<Key32>(AutoSculptCamera1),
            AutoSculptCamera2 = new List<Key32>(AutoSculptCamera2),
            AutoSculptCamera3 = new List<Key32>(AutoSculptCamera3),
            PartDetails = (VltPointerContainer<Key32, FEPartDetail>)PartDetails.Clone(),
            DetailHash = DetailHash,
            OfferID = OfferID,
            HAL_ID = HAL_ID,
            CF_HAL_ID = CF_HAL_ID,
            Price = Price,
            Unknown1 = Unknown1,
            Unknown2 = Unknown2,
            Unknown3 = Unknown3,
            Unknown4 = Unknown4,
            Unknown5 = Unknown5,
            BrandHALId = BrandHALId,
            LogoTextureId = LogoTextureId,
        };
    }
}