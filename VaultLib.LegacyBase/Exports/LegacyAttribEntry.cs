using System;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports
{
    public class LegacyAttribEntry : IFileAccess, IPointerObject
    {
        [Flags]
        public enum NodeFlagsEnum : ushort
        {
            Default = 0x00,
            IsArray = 0x02,
            IsInline = 0x20
        }

        public uint Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public VLTBaseType InlineData { get; set; }
        public VLTCollection Collection { get; }

        public LegacyAttribEntry(VLTCollection collection)
        {
            Collection = collection;
        }

        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();
            InlineDataPointer = br.BaseStream.Position;
            if (IsInline())
            {
                InlineData = TypeRegistry.CreateInstance(vault.Database.Game, Collection.Class, Collection.Class.Fields[Key],
                    Collection);
            }
            else
            {
                InlineData = new VLTAttribType
                { Class = Collection.Class, Collection = Collection, Field = Collection.Class.Fields[Key] };
            }
            InlineData.Read(vault, br);
            br.AlignReader(4);
            TypeIndex = br.ReadUInt16();
            NodeFlags = br.ReadEnum<NodeFlagsEnum>();

            if (vault.ByteOrder != ByteOrder.Little)
            {
                // ugh
                ushort us = (ushort)NodeFlags;
                NodeFlags = (NodeFlagsEnum)(((us & 0xff) << 8) | (us >> 8));
            }

            Debug.Assert((ushort)NodeFlags <= 0x20);
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Key);
            InlineData.Write(vault, bw);
            bw.AlignWriter(4);
            bw.Write(TypeIndex);
            bw.WriteEnum(NodeFlags);
        }

        public bool IsInline()
        {
            return Collection.Class.Fields[Key].Size <= 4 && (Collection.Class.Fields[Key].Flags & DefinitionFlags.kArray) == 0;
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public void AddPointers(Vault vault)
        {
            throw new NotImplementedException();
        }
    }
}