// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:13 PM.

using System.Diagnostics;
using System.IO;

namespace VaultLib.Core.Types
{
    public class VLTUnknown : VLTBaseType
    {
        public int Size { get; set; }

        public byte[] Data { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Data = new byte[this.Size];
            var bytesRead = br.Read(Data, 0, Data.Length);
            Debug.Assert(bytesRead == Data.Length, "br.Read(Data, 0, Data.Length) == Data.Length");
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(this.Data ?? new byte[this.Size]);
        }
    }
}