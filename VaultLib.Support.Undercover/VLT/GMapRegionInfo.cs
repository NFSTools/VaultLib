// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:58 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(GMapRegionInfo))]
    public class GMapRegionInfo : VLTBaseType, IReferencesStrings
    {
        public string Name { get; set; }
        public ushort mCurveStart { get; set; }
        public ushort mCurveCount { get; set; }
        public ushort mTriangleStart { get; set; }
        public ushort mTriangleCount { get; set; }
        public float mBoundsMinX { get; set; }
        public float mBoundsMinY { get; set; }
        public float mBoundsMaxX { get; set; }
        public float mBoundsMaxY { get; set; }

        private Text _name;

        public override void Read(Vault vault, BinaryReader br)
        {
            _name = new Text(Class, Field, Collection);
            _name.Read(vault, br);
            mCurveStart = br.ReadUInt16();
            mCurveCount = br.ReadUInt16();
            mTriangleStart = br.ReadUInt16();
            mTriangleCount = br.ReadUInt16();
            mBoundsMinX = br.ReadSingle();
            mBoundsMinY = br.ReadSingle();
            mBoundsMaxX = br.ReadSingle();
            mBoundsMaxY = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _name = new Text(Class, Field, Collection) { Value = Name };
            _name.Write(vault, bw);
            bw.Write(mCurveStart);
            bw.Write(mCurveCount);
            bw.Write(mTriangleStart);
            bw.Write(mTriangleCount);
            bw.Write(mBoundsMinX);
            bw.Write(mBoundsMinY);
            bw.Write(mBoundsMaxX);
            bw.Write(mBoundsMaxY);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _name.ReadPointerData(vault, br);
            Name = _name.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _name.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _name.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { Name };
        }

        public GMapRegionInfo(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GMapRegionInfo(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}