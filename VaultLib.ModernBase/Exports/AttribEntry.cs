using CoreLibraries.IO;
using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

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
                    InlineData = TypeRegistry.CreateInstance(vault.Database.Options.GameId, Collection.Class, field, Collection);
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

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint) Key);
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