// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 7:25 AM.

using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VltTypeInfo("Attrib::Types::Matrix")]
    public class Matrix : VltBaseType
    {
        public float[] Data { get; set; } = new float[16];

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            for (var i = 0; i < 16; i++) Data[i] = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            for (var i = 0; i < 16; i++) bw.Write(Data[i]);
        }
    }
}