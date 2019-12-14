// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:52 PM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VLTTypeInfo("Attrib::Types::FloatColour")]
    public class FloatColour : VLTBaseType
    {
        public FloatColour(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        public FloatColour(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            R = br.ReadSingle();
            G = br.ReadSingle();
            B = br.ReadSingle();
            A = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
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