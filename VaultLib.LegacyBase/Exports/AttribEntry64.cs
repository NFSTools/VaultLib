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
    public class AttribEntry64 : IVaultFileAccess, IPointerObject
    {
        public ulong Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public object InlineData { get; set; }
        public VltCollection Collection { get; }

        public AttribEntry64(VltCollection collection)
        {
            Collection = collection;
        }

        public void Read(VaultReadContext context, BinaryReader br)
        {
            Key = br.ReadUInt64();

            InlineDataPointer = br.BaseStream.Position;

            var fieldContext = new FieldReadWriteContext(Collection.Class, Collection.Class[Key], Collection);

            if (IsInline())
            {
                InlineData = context.Database.TypeRegistry.ReadFieldValue(Collection.Class, Collection.Class[Key],
                    Collection, context, fieldContext, br);
            }
            else
            {
                var attrib = new VltAttribType(Collection.Class, Collection.Class[Key], Collection);
                attrib.Read(context, fieldContext, br);
                InlineData = attrib;
            }

            br.AlignReader(4);
            TypeIndex = br.ReadUInt16();
            NodeFlags = (NodeFlagsEnum)br.ReadUInt16();
            Debug.Assert((ushort)NodeFlags <= 0x20);
        }

        public void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(Key);

            var fieldContext = new FieldReadWriteContext(Collection.Class, Collection.Class[Key], Collection);
            if (InlineData is VltAttribType attribType)
            {
                attribType.Write(context, fieldContext, bw);
            }
            else
            {
                context.Database.TypeRegistry.WriteFieldValue(Collection.Class[Key], InlineData, context, fieldContext,
                    bw);
            }

            bw.AlignWriter(4);
            bw.Write(TypeIndex);
            bw.WriteEnum(NodeFlags);
        }

        public bool IsInline()
        {
            return Collection.Class[Key].Size <= 4 && (Collection.Class[Key].Flags & DefinitionFlags.Array) == 0;
        }

        public void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public void AddPointers(VaultWriteContext context)
        {
            throw new NotImplementedException();
        }
    }
}