// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:52 PM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VLTTypeInfo("Attrib::Types::Vector2")]
    public class Vector2 : VLTBaseType
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

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public Vector2(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public Vector2(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}