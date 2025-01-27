using CoreLibraries.IO;
using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.ModernBase.Exports;

public class AttribEntry32 : AttribEntryBase<uint>
{
    public AttribEntry32(VltCollection<uint> collection) : base(collection)
    {
    }

    public override void Read(VaultReadContext<uint> context, BinaryReader br)
    {
        Key = br.ReadUInt32();
        InlineDataPointer = br.BaseStream.Position;
        br.ReadUInt32(); // skip data for now
        TypeIndex = br.ReadUInt16();
        NodeFlags = (NodeFlagsEnum)br.ReadByte();
        EntryFlags = br.ReadByte();
    }

    public virtual bool ReadData(VaultReadContext<uint> context, BinaryReader br)
    {
        if (Collection.Class.TryGetField(Key, out var field))
        {
            br.BaseStream.Position = InlineDataPointer;

            var fieldContext = new FieldReadWriteContext<uint>(Collection.Class, field, Collection);

            if (HasInlineFlag())
            {
                InlineData =
                    context.Database.TypeRegistry.ReadFieldValue(context,
                        fieldContext, br);
            }
            else
            {
                var attrib = new VltAttribType<uint>();
                attrib.Read(context, fieldContext, br);
                InlineData = attrib;
            }

            return true;
        }

        return false;
    }

    public override void Write(VaultWriteContext<uint> context, BinaryWriter bw)
    {
        bw.Write((uint)Key);

        var fieldContext = new FieldReadWriteContext<uint>(Collection.Class, Collection.Class[Key], Collection);
        if (InlineData is VltAttribType<uint> attrib)
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

    public override void ReadPointerData(VaultReadContext<uint> context, BinaryReader br)
    {
        throw new NotImplementedException();
    }

    public override void WritePointerData(VaultWriteContext<uint> context, BinaryWriter bw)
    {
        throw new NotImplementedException();
    }

    public override void AddPointers(VaultWriteContext<uint> context)
    {
        throw new NotImplementedException();
    }
}