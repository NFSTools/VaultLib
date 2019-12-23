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

        public override void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt64();
            InlineDataPointer = br.BaseStream.Position;
            br.ReadUInt32(); // skip data for now
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadByte();
            EntryFlags = br.ReadByte();
        }

        public override bool ReadData(Vault vault, BinaryReader br)
        {
            if (Collection.Class.HasField(Key))
            {
                br.BaseStream.Position = InlineDataPointer;

                if (IsInline())
                {
                    InlineData = TypeRegistry.CreateInstance(vault.Database.Options.GameId, Collection.Class, Collection.Class[Key],
                        Collection);
                }
                else
                {
                    InlineData = new VLTAttribType(Collection.Class, Collection.Class[Key], Collection);
                }

                InlineData.Read(vault, br);

                return true;
            }

            return false;
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Key);
            InlineData.Write(vault, bw);
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

        public override void WritePointerData(Vault vault, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void AddPointers(Vault vault)
        {
            throw new NotImplementedException();
        }
    }
}