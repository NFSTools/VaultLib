// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:06 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound
{
    [VLTTypeInfo("Sound::SirenSequence")]
    public class SirenSequence : VLTBaseType
    {
        public enum SirenMode
        {
            SIREN_OFF = 0x1,
            SIREN_WAIL = 0x2,
            SIREN_YELP = 0x3,
            SIREN_PRIORITY = 0x4,
            SIREN_HORN = 0x5,
            SIREN_DEATH = 0x6,
            SIREN_INIT = 0x7,
            MAX_SIREN_STATES = 0x8,
        }

        public SirenMode mMode { get; set; }
        public float mDuration { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mMode = br.ReadEnum<SirenMode>();
            mDuration = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mMode);
            bw.Write(mDuration);
        }

        public SirenSequence(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SirenSequence(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}