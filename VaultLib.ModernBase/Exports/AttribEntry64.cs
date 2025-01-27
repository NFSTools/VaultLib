using System;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.ModernBase.Exports;

public class AttribEntry64 : AttribEntryBase<Key64>
{
    public AttribEntry64(VltCollection<Key64> collection) : base(collection)
    {
    }

    public override void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        Key = new Key64(br.ReadUInt64());
        InlineDataPointer = br.BaseStream.Position;
        br.ReadUInt32(); // skip data for now
        TypeIndex = br.ReadUInt16();
        NodeFlags = (NodeFlagsEnum)br.ReadByte();
        EntryFlags = br.ReadByte();
    }

    public virtual bool ReadData(VaultReadContext<Key64> context, BinaryReader br)
    {
        if (Collection.Class.TryGetField(Key, out var field))
        {
            br.BaseStream.Position = InlineDataPointer;

            var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, field, Collection);

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

            return true;
        }

        return false;
    }

    public override void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Key.Hash);

        var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, Collection.Class[Key], Collection);

        if (InlineData is VltAttribType<Key64> attrib)
        {
            attrib.Write(context, fieldContext, bw);
        }
        else
        {
            context.Database.TypeRegistry.WriteFieldValue(InlineData, context, fieldContext,
                bw);
        }

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

    public override void ReadPointerData(VaultReadContext<Key64> context, BinaryReader br)
    {
        throw new NotImplementedException();
    }

    public override void WritePointerData(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        throw new NotImplementedException();
    }

    public override void AddPointers(VaultWriteContext<Key64> context)
    {
        throw new NotImplementedException();
    }
}