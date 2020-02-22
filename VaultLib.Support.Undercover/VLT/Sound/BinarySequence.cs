// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:00 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound
{
    [VLTTypeInfo("Sound::BinarySequence")]
    public class BinarySequence : VLTBaseType
    {
        public bool Value { get; set; }
        public float Duration { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadBoolean();
            Duration = br.ReadSingle();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Value);
            bw.Write(Duration);
            bw.AlignWriter(4);
        }

        public BinarySequence(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public BinarySequence(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}