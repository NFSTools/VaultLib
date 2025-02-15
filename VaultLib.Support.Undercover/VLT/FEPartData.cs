// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:27 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEPartData))]
public class FEPartData : VltBaseType<Key32>, IReferencesStrings, IVltPointerObject<Key32>
{
    public BinKey32 HAL_ID { get; set; }
    public BinKey32 CF_HAL_ID { get; set; }
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
    public BinKey32 BrandHALId { get; set; }
    public BinKey32 LogoTextureId { get; set; }
    public uint DetailHash { get; set; }
    public VltPointerContainer<Key32, FEPartDetail> PartDetails { get; set; }
    public string OfferID { get; set; } = string.Empty;
    public bool IsOnlineLockable { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        HAL_ID = BinKey32.Read(br);
        CF_HAL_ID = BinKey32.Read(br);
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
        br.SafeAlignReader(4);
        BrandHALId = BinKey32.Read(br);
        LogoTextureId = BinKey32.Read(br);
        DetailHash = br.ReadUInt32();
        PartDetails = new VltPointerContainer<Key32, FEPartDetail>();
        PartDetails.Read(context, fieldContext, br);
        OfferID = context.ReadString(br);
        IsOnlineLockable = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        HAL_ID.Write(bw);
        CF_HAL_ID.Write(bw);
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
        BrandHALId.Write(bw);
        LogoTextureId.Write(bw);
        bw.Write(DetailHash);
        PartDetails.Write(context, fieldContext, bw);
        context.WriteString(OfferID, fieldContext, bw);
        bw.Write(IsOnlineLockable);
        bw.AlignWriter(4);
    }

    public void ReadPointerData(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        PartDetails.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        PartDetails.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext)
    {
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
            HAL_ID = HAL_ID,
            CF_HAL_ID = CF_HAL_ID,
            Price = Price,
            ShowroomUnlock = ShowroomUnlock,
            Tier1Price = Tier1Price,
            Tier2Price = Tier2Price,
            Tier3Price = Tier3Price,
            Tier4Price = Tier4Price,
            Tier = Tier,
            Tier1ShowroomUnlock = Tier1ShowroomUnlock,
            Tier2ShowroomUnlock = Tier2ShowroomUnlock,
            Tier3ShowroomUnlock = Tier3ShowroomUnlock,
            Tier4ShowroomUnlock = Tier4ShowroomUnlock,
            BrandHALId = BrandHALId,
            LogoTextureId = LogoTextureId,
            DetailHash = DetailHash,
            PartDetails = new VltPointerContainer<Key32, FEPartDetail>
            {
                Value = (FEPartDetail)PartDetails.Value.Clone()
            },
            OfferID = OfferID,
            IsOnlineLockable = IsOnlineLockable
        };
    }
}