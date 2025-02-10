using System.Collections.Generic;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Query;

namespace VaultLib.Support.Undercover.VLT.Attrib.Query;

[VltTypeInfo("DUMMY_Attrib_Query_CollectionNameToChildrenIndex")]
public class CollectionNameToChildrenIndex : BaseCollectionNameIndex
{
    protected override List<IndexEntry> GenerateIndex(VaultWriteContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext)
    {
        var collectionsGroupedByParent = context.Database.RowManager.EnumerateCollections(fieldContext.Class.Key)
            .GroupBy(c => c.Parent?.Key ?? Key32.Zero);

        return collectionsGroupedByParent
            .Select(g => new IndexEntry(g.Key,
                g.OrderBy(c => c.GetRawValue<string>("CollectionName")).Select(c => c.Key).ToList()))
            .ToList();
    }

    public override object Clone()
    {
        throw new System.NotImplementedException();
    }
}