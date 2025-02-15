// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:58 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(GMapRegionInfo))]
public class GMapRegionInfo: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
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

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
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

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
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

    public IEnumerable<string> GetStrings()
    {
        return new[] { Name };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}