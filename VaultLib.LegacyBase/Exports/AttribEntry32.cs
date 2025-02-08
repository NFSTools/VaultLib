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

public class AttribEntry32 : IVaultFileAccess<Key32>, IPointerObject<Key32>
{
    public Key32 Key { get; set; }
    public ushort TypeIndex { get; set; }
    public NodeFlagsEnum NodeFlags { get; set; }
    public long InlineDataPointer { get; set; }
    public object InlineData { get; set; }
    public VltCollection<Key32> Collection { get; }

    public AttribEntry32(VltCollection<Key32> collection)
    {
        Collection = collection;
    }

    public void Read(VaultReadContext<Key32> context, BinaryReader br)
    {
        Key = new Key32(br.ReadUInt32());

        InlineDataPointer = br.BaseStream.Position;

        var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, Collection.Class[Key], Collection);

        if (IsInline())
        {
            InlineData = context.Database.TypeRegistry.ReadFieldValue(context, fieldContext, br);
        }
        else
        {
            var attrib = new VltAttribType<Key32>();
            attrib.Read(context, fieldContext, br);
            InlineData = attrib;
        }

        br.SafeAlignReader(4);
        TypeIndex = br.ReadUInt16();

        var nodeFlags = (ushort)(br.ReadByte() | (ushort)(br.ReadByte() << 8));

        NodeFlags = (NodeFlagsEnum)nodeFlags;
        // NodeFlags = (NodeFlagsEnum)br.ReadUInt16();
        Debug.Assert((ushort)NodeFlags <= 0x20);
    }

    public void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        bw.Write(Key.Hash);

        var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, Collection.Class[Key], Collection);
        if (InlineData is VltAttribType<Key32> attribType)
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

    public void ReadPointerData(VaultReadContext<Key32> context, BinaryReader br)
    {
        throw new NotImplementedException();
    }

    public void WritePointerData(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        throw new NotImplementedException();
    }

    public void AddPointers(VaultWriteContext<Key32> context)
    {
        throw new NotImplementedException();
    }
}