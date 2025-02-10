// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 2:46 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(Curve))]
public class Curve : VltBaseType<Key32>, IVltPointerObject<Key32>
{
    public float MinX { get; set; }
    public float MaxX { get; set; }
    public float MinY { get; set; }
    public float MaxY { get; set; }

    public float[] XValues { get; set; }
    public float[] YValues { get; set; }
    public float[] Y2Values { get; set; }

    private VariableArray<Key32> _xArray = new();
    private VariableArray<Key32> _yArray = new();
    private VariableArray<Key32> _y2Array = new();

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        MinX = br.ReadSingle();
        MaxX = br.ReadSingle();
        MinY = br.ReadSingle();
        MaxY = br.ReadSingle();
        _xArray.Read(br);
        _yArray.Read(br);
        _y2Array.Read(br);
        br.ReadUInt32();
        //Debug.Assert(br.ReadUInt32()==0);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        _xArray.Data = XValues;
        _yArray.Data = YValues;
        _y2Array.Data = Y2Values;

        bw.Write(MinX);
        bw.Write(MaxX);
        bw.Write(MinY);
        bw.Write(MaxY);

        _xArray.Write(bw);
        _yArray.Write(bw);
        _y2Array.Write(bw);

        bw.Write(0); // AllocatedMemory (bool1 + 3 align bytes)
    }

    public void ReadPointerData(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        _xArray.ReadPointerData(context, br);
        _yArray.ReadPointerData(context, br);
        _y2Array.ReadPointerData(context, br);

        XValues = _xArray.Data;
        YValues = _yArray.Data;
        Y2Values = _y2Array.Data;
    }

    public void WritePointerData(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        _xArray.WritePointerData(context, bw);
        _yArray.WritePointerData(context, bw);
        _y2Array.WritePointerData(context, bw);
    }

    public void AddPointers(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext)
    {
        _xArray.AddPointers(context);
        _yArray.AddPointers(context);
        _y2Array.AddPointers(context);
    }

    public override object Clone()
    {
        return new Curve
        {
            MinX = MinX,
            MaxX = MaxX,
            MinY = MinY,
            MaxY = MaxY,
            XValues = (float[])XValues.Clone(),
            YValues = (float[])YValues.Clone(),
            Y2Values = (float[])Y2Values.Clone(),
        };
    }
}