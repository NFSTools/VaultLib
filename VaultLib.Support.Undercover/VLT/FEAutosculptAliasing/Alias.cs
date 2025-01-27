// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:58 AM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.FEAutosculptAliasing;

[VltTypeInfo("FEAutosculptAliasing::Alias")]
public class Alias: VltBaseType<uint>, IVltPointerObject<uint>
{
    public byte Kit { get; set; }
    public uint Region { get; set; }
    public List<Slider> Sliders { get; set; }

    private uint _slidersPointer;

    private long _srcSlidersPtr;
    private long _dstSlidersPtr;

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Kit = br.ReadByte();
        br.SafeAlignReader(4);
        Region = br.ReadUInt32();
        _slidersPointer = br.ReadUInt32();
        Sliders = new List<Slider>(br.ReadByte());
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Kit);
        bw.AlignWriter(4);
        bw.Write(Region);
        _srcSlidersPtr = bw.BaseStream.Position;
        bw.Write(0);
        bw.Write((byte)Sliders.Count);
        bw.AlignWriter(4);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        br.BaseStream.Position = _slidersPointer;

        for (int i = 0; i < Sliders.Capacity; i++)
        {
            Slider slider = new Slider();
            slider.Read(context, fieldContext, br);
            Sliders.Add(slider);
        }
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        _dstSlidersPtr = bw.BaseStream.Position;
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        context.AddPointer(_srcSlidersPtr, _dstSlidersPtr, false);
    }
}