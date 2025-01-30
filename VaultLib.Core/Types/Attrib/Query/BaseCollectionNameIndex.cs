using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Types.Attrib.Query;

public abstract class BaseCollectionNameIndex : BaseManyToOneIndex<Key32, Key32>
{
    protected override List<Key32> ReadKeys(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br, int count)
    {
        var keys = new List<Key32>();
        for (var i = 0; i < count; i++)
        {
            keys.Add(Key32.Read(br));
        }

        return keys;
    }

    protected override List<Key32> ReadValues(VaultReadContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext, BinaryReader br, int count)
    {
        var values = new List<Key32>();
        for (var i = 0; i < count; i++)
        {
            values.Add(Key32.Read(br));
        }

        return values;
    }

    protected override void WriteKeys(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw, IEnumerable<Key32> keys)
    {
        foreach (var key in keys)
        {
            key.Write(bw);
        }
    }

    protected override void WriteValues(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw, IEnumerable<Key32> values)
    {
        foreach (var value in values)
        {
            value.Write(bw);
        }
    }
}