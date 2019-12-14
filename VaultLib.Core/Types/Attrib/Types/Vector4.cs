// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:53 PM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VLTTypeInfo("Attrib::Types::Vector4")]
    public class Vector4 : VLTBaseType
    {
        public Vector4(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Vector4(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
            W = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
            bw.Write(W);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }
    }
}