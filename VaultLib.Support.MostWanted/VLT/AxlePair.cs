// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:54 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(AxlePair))]
    public class AxlePair : VLTBaseType
    {
        public float Front { get; set; }
        public float Rear { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Front = br.ReadSingle();
            Rear = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Front);
            bw.Write(Rear);
        }

        public override string ToString()
        {
            return $"[{Front}, {Rear}]";
        }

        public AxlePair(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AxlePair(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}