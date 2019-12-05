using System;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exceptions;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports
{
    public class AttribEntry : IFileAccess, IPointerObject
    {
        [Flags]
        public enum NodeFlagsEnum : byte
        {
            Default = 0x00,
            IsArray = 0x02,
            HasHandler = 0x08,
            IsInline = 0x40
        }

        public uint Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public byte EntryFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public VLTBaseType InlineData { get; set; }
        public VLTCollection Collection { get; }

        public AttribEntry(VLTCollection collection)
        {
            Collection = collection;
        }

        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();
            InlineDataPointer = br.BaseStream.Position;

            if (!Collection.Class.FieldExists(Key))
            {
                br.BaseStream.Position = InlineDataPointer - 4 + 0xC;
                throw new CollectionLoadingException(
                    $"Attempted to process entry for field {Key:X8} which does not exist",
                    Collection);
            }

            if (IsInline())
            {
                InlineData = TypeRegistry.CreateInstance(vault.Database.Game, Collection.Class, Collection.Class.Fields[Key],
                    Collection);
            }
            else
            {
                InlineData = new VLTAttribType(Collection.Class, Collection.Class.Fields[Key], Collection);
            }
            InlineData.Read(vault, br);
            br.AlignReader(4);
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadByte();

            if ((byte)NodeFlags > 0x40)
            {
                throw new CollectionLoadingException(
                    $"Invalid NodeFlags value in entry for field {Key:X8}: {((byte)NodeFlags):X2}", Collection);
            }

            EntryFlags = br.ReadByte();
            if (EntryFlags != 0)
                throw new Exception();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Key);
            InlineData.Write(vault, bw);
            if (HasInlineFlag())
                bw.AlignWriter(4);
            bw.Write(TypeIndex);
            bw.Write((byte)NodeFlags);
            bw.Write(EntryFlags);
        }

        private bool HasInlineFlag()
        {
            return (NodeFlags & NodeFlagsEnum.IsInline) == NodeFlagsEnum.IsInline;
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