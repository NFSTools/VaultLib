using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo("DUMMY_VltCollectionKey")]
public class VltCollectionKey: VltBaseType<Key32>, IStringValue
{
    public string CollectionKey { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryReader br)
    {
        var collectionKey = br.ReadUInt32();
        CollectionKey = HashManager.ResolveVlt(collectionKey);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Vlt32Hasher.Hash(CollectionKey));
    }

    public string GetString()
    {
        return CollectionKey;
    }

    public void SetString(string str)
    {
        CollectionKey = str;
    }
}