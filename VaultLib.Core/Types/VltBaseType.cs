// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:12 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public abstract class VltBaseType : IVaultFileAccess
    {
        protected VltBaseType(VltClass @class, VltClassField field, VltCollection collection)
        {
            Class = @class;
            Field = field;
            Collection = collection;
        }

        protected VltBaseType(VltClass @class, VltClassField field)
        {
            Class = @class;
            Field = field;
        }

        protected VltClass Class { get; set; }
        protected VltClassField Field { get; set; }
        protected VltCollection Collection { get; set; }

        protected bool IsInVlt
        {
            get { return !Field.IsInLayout && Field.Size <= 4 && !Field.IsArray; }
        }

        public abstract void Read(VaultLoadContext context, BinaryReader br);
        public abstract void Write(VaultSaveContext context, BinaryWriter bw);
    }
}