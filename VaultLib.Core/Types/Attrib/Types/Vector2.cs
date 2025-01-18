// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:52 PM.

using System.IO;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VltTypeInfo("Attrib::Types::Vector2")]
    public class Vector2 : VltBaseType
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}