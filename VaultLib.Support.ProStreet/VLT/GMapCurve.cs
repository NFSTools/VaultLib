// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:54 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(GMapCurve))]
    public class GMapCurve : VLTBaseType
    {
        public ushort mPointStart { get; set; }
        public ushort mPointCount { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mPointStart = br.ReadUInt16();
            mPointCount = br.ReadUInt16();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(mPointStart);
            bw.Write(mPointCount);
        }

        public GMapCurve(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GMapCurve(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}