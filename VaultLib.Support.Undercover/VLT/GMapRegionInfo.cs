// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:58 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(GMapRegionInfo))]
    public class GMapRegionInfo : VltBaseType, IReferencesStrings
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

        private Text _name = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _name.Read(context, fieldContext, br);
            mCurveStart = br.ReadUInt16();
            mCurveCount = br.ReadUInt16();
            mTriangleStart = br.ReadUInt16();
            mTriangleCount = br.ReadUInt16();
            mBoundsMinX = br.ReadSingle();
            mBoundsMinY = br.ReadSingle();
            mBoundsMaxX = br.ReadSingle();
            mBoundsMaxY = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _name.Value = Name;
            _name.Write(context, fieldContext, bw);
            bw.Write(mCurveStart);
            bw.Write(mCurveCount);
            bw.Write(mTriangleStart);
            bw.Write(mTriangleCount);
            bw.Write(mBoundsMinX);
            bw.Write(mBoundsMinY);
            bw.Write(mBoundsMaxX);
            bw.Write(mBoundsMaxY);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _name.ReadPointerData(context, fieldContext, br);
            Name = _name.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _name.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _name.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { Name };
        }
    }
}