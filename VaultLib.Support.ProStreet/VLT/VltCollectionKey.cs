using System.IO;
using VaultLib.Core;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo("DUMMY_VltCollectionKey")]
public class VltCollectionKey : VltBaseType, IStringValue
{
    public string CollectionKey { get; set; } = string.Empty;

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        var collectionKey = br.ReadUInt32();
        CollectionKey = HashManager.ResolveVlt(collectionKey);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
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