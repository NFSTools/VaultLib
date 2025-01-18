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

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Key = br.ReadUInt32();
            InlineDataPointer = br.BaseStream.Position;
            br.ReadUInt32(); // skip data for now
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadByte();
            EntryFlags = br.ReadByte();
        }

        public virtual bool ReadData(VaultReadContext context, BinaryReader br)
        {
            if (Collection.Class.TryGetField(Key, out var field))
            {
                if (HasInlineFlag())
                {
                    InlineData = context.Database.TypeRegistry.CreateInstance(Collection.Class, field, Collection);
                }
                else
                {
                    InlineData = new VltAttribType(Collection.Class, field, Collection);
                }

                br.BaseStream.Position = InlineDataPointer;
                InlineData.Read(context, br);

                return true;
            }

            return false;
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
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

        public override void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public override void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void AddPointers(VaultWriteContext context)
        {
            throw new NotImplementedException();
        }
    }
}