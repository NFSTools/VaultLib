// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 9:15 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(CarBodyMotion))]
    public class CarBodyMotion : VLTBaseType
    {
        public float DegPerG { get; set; }
        public float MaxGs { get; set; }
        public float DegPerSec { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            DegPerG = br.ReadSingle();
            MaxGs = br.ReadSingle();
            DegPerSec = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(DegPerG);
            bw.Write(MaxGs);
            bw.Write(DegPerSec);
        }

        public CarBodyMotion(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public CarBodyMotion(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}