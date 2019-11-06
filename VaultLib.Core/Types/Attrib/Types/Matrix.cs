// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 7:25 AM.

using System.IO;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VLTTypeInfo("Attrib::Types::Matrix")]
    public class Matrix : VLTBaseType
    {
        public float[] Data { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Data = new float[16];

            for (int i = 0; i < 16; i++)
            {
                Data[i] = br.ReadSingle();
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            for (int i = 0; i < 16; i++)
            {
                bw.Write(Data[i]);
            }
        }
    }
}