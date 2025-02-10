using System.Collections.Generic;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Query;

namespace VaultLib.Support.Undercover.VLT.Attrib.Query;

[VltTypeInfo("DUMMY_Attrib_Query_CollectionNameToParentIndex")]
public class CollectionNameToParentIndex : BaseCollectionNameIndex
{
    protected override List<IndexEntry> GenerateIndex(VaultWriteContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext)
    {
        return context.Database.RowManager.EnumerateCollections(fieldContext.Class.Key)
            .Select(c => new IndexEntry(c.Key, new List<Key32>
            {
                c.Parent?.Key ?? Key32.Zero,
            }))
            .ToList();
    }

    public override object Clone()
    {
        throw new System.NotImplementedException();
    }
}