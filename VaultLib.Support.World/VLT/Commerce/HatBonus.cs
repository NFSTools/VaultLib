// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:26 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Commerce
{
    [VLTTypeInfo("Commerce::HatBonus")]
    public class HatBonus : VLTBaseType
    {
        public int Handling { get; set; }
        public int Acceleration { get; set; }
        public int TopSpeed { get; set; }
        public int RequiredPartCount { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Handling = br.ReadInt32();
            Acceleration = br.ReadInt32();
            TopSpeed = br.ReadInt32();
            RequiredPartCount = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Handling);
            bw.Write(Acceleration);
            bw.Write(TopSpeed);
            bw.Write(RequiredPartCount);
        }
    }
}