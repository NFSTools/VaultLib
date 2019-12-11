using System;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports
{
    public class AttribEntry64 : IFileAccess, IPointerObject
    {
        public ulong Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public byte EntryFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public VLTBaseType InlineData { get; set; }
        public VLTCollection Collection { get; }

        public AttribEntry64(VLTCollection collection)
        {
            Collection = collection;
        }

        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt64();
            InlineDataPointer = br.BaseStream.Position;
            br.ReadUInt32(); // skip data for now
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum) br.ReadByte();
            EntryFlags = br.ReadByte();
        }

        public bool ReadData(Vault vault, BinaryReader br)
        {
            if (Collection.Class.FieldExists(Key))
            {
                br.BaseStream.Position = InlineDataPointer;

                if (IsInline())
                {
                    InlineData = TypeRegistry.CreateInstance(vault.Database.Options.GameId, Collection.Class, Collection.Class.Fields[Key],
                        Collection);
                }
                else
                {
                    InlineData = new VLTAttribType(Collection.Class, Collection.Class.Fields[Key], Collection);
                }

                InlineData.Read(vault, br);

                return true;
            }

            return false;
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