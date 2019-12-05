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
        protected VLTClass Class { get; set; }
        protected VLTClassField Field { get; set; }
        protected VLTCollection Collection { get; set; }


        protected VLTBaseType(VLTClass @class, VLTClassField field, VLTCollection collection)
        {
            Class = @class;
            Field = field;
            Collection = collection;
        }

        protected VLTBaseType(VLTClass @class, VLTClassField field)
        {
            Class = @class;
            Field = field;
        }

        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void Write(Vault vault, BinaryWriter bw);
    }
}