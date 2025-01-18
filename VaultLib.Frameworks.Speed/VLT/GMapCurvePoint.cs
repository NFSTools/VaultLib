// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:55 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(GMapCurvePoint))]
    public class GMapCurvePoint : VltBaseType
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
        }
    }
}