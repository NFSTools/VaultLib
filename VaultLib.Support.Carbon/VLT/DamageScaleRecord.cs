// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:35 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(DamageScaleRecord))]
    public class DamageScaleRecord : VLTBaseType
    {
        public float VisualScale { get; set; }
        public float HitPointScale { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            VisualScale = br.ReadSingle();
            HitPointScale = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(VisualScale);
            bw.Write(HitPointScale);
        }
    }
}