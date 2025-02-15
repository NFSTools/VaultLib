// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:58 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(GMapRegionInfo))]
public class GMapRegionInfo : VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public string Name { get; set; } = string.Empty;
    public ushort mCurveStart { get; set; }
    public ushort mCurveCount { get; set; }
    public ushort mTriangleStart { get; set; }
    public ushort mTriangleCount { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Name = context.ReadString(br);
        mCurveStart = br.ReadUInt16();
        mCurveCount = br.ReadUInt16();
        mTriangleStart = br.ReadUInt16();
        mTriangleCount = br.ReadUInt16();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(Name, fieldContext, bw);
        bw.Write(mCurveStart);
        bw.Write(mCurveCount);
        bw.Write(mTriangleStart);
        bw.Write(mTriangleCount);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Name };
    }

    public override object Clone()
    {
        return new GMapRegionInfo
        {
            Name = this.Name,
            mCurveStart = this.mCurveStart,
            mCurveCount = this.mCurveCount,
            mTriangleStart = this.mTriangleStart,
            mTriangleCount = this.mTriangleCount,
        };
    }
}