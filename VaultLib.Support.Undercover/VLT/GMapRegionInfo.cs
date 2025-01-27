// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:58 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(GMapRegionInfo))]
public class GMapRegionInfo: VltBaseType<uint>, IReferencesStrings<uint>
{
    public string Name { get; set; } = string.Empty;
    public ushort mCurveStart { get; set; }
    public ushort mCurveCount { get; set; }
    public ushort mTriangleStart { get; set; }
    public ushort mTriangleCount { get; set; }
    public float mBoundsMinX { get; set; }
    public float mBoundsMinY { get; set; }
    public float mBoundsMaxX { get; set; }
    public float mBoundsMaxY { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Name = context.ReadString(br);
        mCurveStart = br.ReadUInt16();
        mCurveCount = br.ReadUInt16();
        mTriangleStart = br.ReadUInt16();
        mTriangleCount = br.ReadUInt16();
        mBoundsMinX = br.ReadSingle();
        mBoundsMinY = br.ReadSingle();
        mBoundsMaxX = br.ReadSingle();
        mBoundsMaxY = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        context.WriteString(Name, fieldContext, bw);
        bw.Write(mCurveStart);
        bw.Write(mCurveCount);
        bw.Write(mTriangleStart);
        bw.Write(mTriangleCount);
        bw.Write(mBoundsMinX);
        bw.Write(mBoundsMinY);
        bw.Write(mBoundsMaxX);
        bw.Write(mBoundsMaxY);
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        //
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        //
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        //
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Name };
    }
}