using CoreLibraries.IO;
using System;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports
{
    public class AttribEntry : IFileAccess, IPointerObject
    {
        public uint Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public VLTBaseType InlineData { get; set; }
        public VltCollection Collection { get; }

        public AttribEntry(VltCollection collection)
        {
            Collection = collection;
        }

        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();

            InlineDataPointer = br.BaseStream.Position;
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
            br.AlignReader(4);
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadUInt16();
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
            return Collection.Class[Key].Size <= 4 && (Collection.Class[Key].Flags & DefinitionFlags.Array) == 0;
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