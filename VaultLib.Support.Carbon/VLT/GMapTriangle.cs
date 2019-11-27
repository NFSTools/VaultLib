// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:01 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(GMapTriangle))]
    public class GMapTriangle : VLTBaseType
    {
        public ushort mPoint1 { get; set; }
        public ushort mPoint2 { get; set; }
        public ushort mPoint3 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mPoint1 = br.ReadUInt16();
            mPoint2 = br.ReadUInt16();
            mPoint3 = br.ReadUInt16();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(mPoint1);
            bw.Write(mPoint2);
            bw.Write(mPoint3);
        }
    }
}