// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:56 PM.

using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    public enum GMapCurveRefFlags : ushort
    {
        kFlag_Reversed = 1,
    }

    [VLTTypeInfo(nameof(GMapCurveRef))]
    public class GMapCurveRef : VLTBaseType
    {
        public ushort mCurveIndex { get; set; }
        public GMapCurveRefFlags Flags { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mCurveIndex = br.ReadUInt16();
            Flags = (GMapCurveRefFlags)br.ReadUInt16();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(mCurveIndex);
            bw.WriteEnum(Flags);
        }

        public GMapCurveRef(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GMapCurveRef(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}