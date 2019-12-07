// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:13 PM.

using System.Diagnostics;
using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types
{
    public class VLTUnknown : VLTBaseType
    {
        public VLTUnknown(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field,
            collection)
        {
        }

        public VLTUnknown(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }

        public int Size { get; set; }

        public byte[] Data { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Data = new byte[Size];
            var bytesRead = br.Read(Data, 0, Data.Length);
            Debug.Assert(bytesRead == Data.Length, "br.Read(Data, 0, Data.Length) == Data.Length");
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Data ?? new byte[Size]);
        }
    }
}