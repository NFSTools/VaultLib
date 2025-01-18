// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:56 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(GMapCurveRef))]
    public class GMapCurveRef : VltBaseType
    {
        public enum GMapCurveRefFlags : ushort
        {
            kFlag_Reversed = 1,
        }

        public ushort mCurveIndex { get; set; }
        public GMapCurveRefFlags Flags { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            mCurveIndex = br.ReadUInt16();
            Flags = (GMapCurveRefFlags)br.ReadUInt16();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
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