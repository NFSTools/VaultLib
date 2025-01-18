// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:53 PM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VltTypeInfo("Attrib::Types::Vector3")]
    public class Vector3 : VltBaseType
    {
        public Vector3(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Vector3(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}