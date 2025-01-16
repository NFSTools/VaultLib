using CoreLibraries.IO;
using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.ModernBase.Exports
{
    public class AttribEntry : AttribEntryBase
    {
        public AttribEntry(VltCollection collection) : base(collection)
        {
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();
            InlineDataPointer = br.BaseStream.Position;
            br.ReadUInt32(); // skip data for now
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadByte();
            EntryFlags = br.ReadByte();
        }

        public override bool ReadData(Vault vault, BinaryReader br)
        {
            if (Collection.Class.TryGetField(Key, out VltClassField field))
            {
                if (HasInlineFlag())
                {
                    InlineData = vault.Database.TypeRegistry.CreateInstance(Collection.Class, field, Collection);
                }
                else
                {
                    InlineData = new VLTAttribType(Collection.Class, field, Collection);
                }

                br.BaseStream.Position = InlineDataPointer;
                InlineData.Read(vault, br);

                return true;
            }

            return false;
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write((uint)Key);
            InlineData.Write(context, bw);
            if (HasInlineFlag())
            {
                bw.AlignWriter(4);
            }

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
            return Collection.Class[Key].Size <= 4 && (Collection.Class[Key].Flags & DefinitionFlags.Array) == 0;
        }

        public override void ReadPointerData(Vault vault, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public override void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void AddPointers(VaultSaveContext context)
        {
            throw new NotImplementedException();
        }
    }
}