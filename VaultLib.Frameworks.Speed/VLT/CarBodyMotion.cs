// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 9:15 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CarBodyMotion))]
    public class CarBodyMotion : VltBaseType
    {
        public float DegPerG { get; set; }
        public float MaxGs { get; set; }
        public float DegPerSec { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            DegPerG = br.ReadSingle();
            MaxGs = br.ReadSingle();
            DegPerSec = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(DegPerG);
            bw.Write(MaxGs);
            bw.Write(DegPerSec);
        }
    }
}