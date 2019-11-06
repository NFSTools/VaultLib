// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:55 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    public class GMapCurvePoint : VLTBaseType
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
        }
    }
}