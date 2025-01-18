// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:52 PM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VltTypeInfo("Attrib::Types::FloatColour")]
    public class FloatColour : VltBaseType
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            R = br.ReadSingle();
            G = br.ReadSingle();
            B = br.ReadSingle();
            A = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(R);
            bw.Write(G);
            bw.Write(B);
            bw.Write(A);
        }

        public override string ToString()
        {
            return $"R: {R} G: {G} B: {B} A: {A}";
        }
    }
}