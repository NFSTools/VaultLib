using System;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports;

public class AttribEntry64 : IVaultFileAccess<Key64>, IPointerObject<Key64>
{
    public Key64 Key { get; set; }
    public ushort TypeIndex { get; set; }
    public NodeFlagsEnum NodeFlags { get; set; }
    public long InlineDataPointer { get; set; }
    public object InlineData { get; set; }
    public VltCollection<Key64> Collection { get; }

    public AttribEntry64(VltCollection<Key64> collection)
    {
        Collection = collection;
    }

    public void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        Key = new Key64(br.ReadUInt64());

        InlineDataPointer = br.BaseStream.Position;

        var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, Collection.Class[Key], Collection);

        if (IsInline())
        {
            InlineData = context.Database.TypeRegistry.ReadFieldValue(context, fieldContext, br);
        }
        else
        {
            var attrib = new VltAttribType<Key64>();
            attrib.Read(context, fieldContext, br);
            InlineData = attrib;
        }

        br.SafeAlignReader(4);
        TypeIndex = br.ReadUInt16();
        NodeFlags = (NodeFlagsEnum)br.ReadUInt16();
        Debug.Assert((ushort)NodeFlags <= 0x20);
    }

    public void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Key.Hash);

        var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, Collection.Class[Key], Collection);
        if (InlineData is VltAttribType<Key64> attribType)
        {
            attribType.Write(context, fieldContext, bw);
        }
        else
        {
            context.Database.TypeRegistry.WriteFieldValue(InlineData, context, fieldContext,
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

    public void ReadPointerData(VaultReadContext<Key64> context, BinaryReader br)
    {
        throw new NotImplementedException();
    }

    public void WritePointerData(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        throw new NotImplementedException();
    }

    public void AddPointers(VaultWriteContext<Key64> context)
    {
        throw new NotImplementedException();
    }
}