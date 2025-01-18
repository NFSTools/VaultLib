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

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
        }

        public GMapCurvePoint(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GMapCurvePoint(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}