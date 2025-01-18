// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:00 PM.

using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound
{
    [VltTypeInfo("Sound::BinarySequence")]
    public class BinarySequence : VltBaseType
    {
        public bool Value { get; set; }
        public float Duration { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Value = br.ReadBoolean();
            br.AlignReader(4);
            Duration = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Value);
            bw.AlignWriter(4);
            bw.Write(Duration);
        }

        public BinarySequence(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public BinarySequence(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}