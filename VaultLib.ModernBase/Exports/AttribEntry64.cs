using CoreLibraries.IO;
using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.ModernBase.Exports
{
    public class AttribEntry64 : AttribEntryBase
    {
        public AttribEntry64(VltCollection collection) : base(collection)
        {
        }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            Key = br.ReadUInt64();
            InlineDataPointer = br.BaseStream.Position;
            br.ReadUInt32(); // skip data for now
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadByte();
            EntryFlags = br.ReadByte();
        }

        public virtual bool ReadData(VaultLoadContext context, BinaryReader br)
        {
            if (Collection.Class.HasField(Key))
            {
                br.BaseStream.Position = InlineDataPointer;

                if (IsInline())
                {
                    InlineData = context.Database.TypeRegistry.CreateInstance(Collection.Class, Collection.Class[Key],
                        Collection);
                }
                else
                {
                    InlineData = new VltAttribType(Collection.Class, Collection.Class[Key], Collection);
                }

                InlineData.Read(context, br);

                return true;
            }

            return false;
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(Key);
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

        public override void ReadPointerData(VaultLoadContext context, BinaryReader br)
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