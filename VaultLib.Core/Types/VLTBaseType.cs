// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:12 PM.

using System.ComponentModel;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public abstract class VLTBaseType : IFileAccess
    {
        [Browsable(false)]
        public VLTCollection Collection { get; set; }
        [Browsable(false)]
        public VLTClassField Field { get; set; }
        [Browsable(false)]
        public VLTClass Class { get; set; }

        [Browsable(false)]
        public bool IsInVLT
        {
            get
            {
                return
                    (Field.Flags & DefinitionFlags.kInLayout) == 0
                    && (Field.Flags & DefinitionFlags.kArray) == 0
                    && Field.Size <= 4;
            }
        }

        [Browsable(false)]
        public int ArrayIndex { get; set; }

        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void Write(Vault vault, BinaryWriter bw);
    }
}