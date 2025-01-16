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

namespace VaultLib.Frameworks.Speed.VLT
{
    [VLTTypeInfo(nameof(GMapRegionInfo))]
    public class GMapRegionInfo : VLTBaseType, IReferencesStrings
    {
        public string Name { get; set; }
        public ushort mCurveStart { get; set; }
        public ushort mCurveCount { get; set; }
        public ushort mTriangleStart { get; set; }
        public ushort mTriangleCount { get; set; }

        private Text _name;

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _name.Read(context, br);
            mCurveStart = br.ReadUInt16();
            mCurveCount = br.ReadUInt16();
            mTriangleStart = br.ReadUInt16();
            mTriangleCount = br.ReadUInt16();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _name.Value = Name;
            _name.Write(context, bw);
            bw.Write(mCurveStart);
            bw.Write(mCurveCount);
            bw.Write(mTriangleStart);
            bw.Write(mTriangleCount);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _name.ReadPointerData(context, br);
            Name = _name.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _name.Value = Name;
            _name.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _name.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { Name };
        }

        public GMapRegionInfo(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _name = new Text(Class, Field, Collection);
            Name = string.Empty;
        }
    }
}