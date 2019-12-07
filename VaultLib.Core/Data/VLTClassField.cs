// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:18 PM.

using VaultLib.Core.Types;

namespace VaultLib.Core.Data
{
    public class VLTClassField
    {
        public string Name { get; set; }

        public string TypeName { get; set; }
        public DefinitionFlags Flags { get; set; }
        public int Alignment { get; set; }
        public ushort Offset { get; set; }
        public ushort Size { get; set; }
        public ushort MaxCount { get; set; }
        public VLTBaseType StaticValue { get; set; }

        public bool IsRequired => (Flags & DefinitionFlags.kInLayout) != 0;
        public bool IsOptional => (Flags & DefinitionFlags.kInLayout) == 0;
        public bool IsArray => (Flags & DefinitionFlags.kArray) != 0;
        public bool IsStatic => (Flags & DefinitionFlags.kIsStatic) != 0;

        public ulong Key { get; set; }

        public bool CanBeInlined()
        {
            return IsOptional && Size <= 4 && !IsArray;
        }
    }
}