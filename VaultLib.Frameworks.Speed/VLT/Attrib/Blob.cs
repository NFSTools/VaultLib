using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT.Attrib;

/// <summary>
/// Need for Speed games by EA Black Box have compressed data stored in Attrib::Blob instances.
/// </summary>
[VltTypeInfo("Attrib::Blob")]
public abstract class Blob<TKey> : BaseBlob<TKey> where TKey : IKey<TKey>
{
    private CompressedBlob _blob;

    protected override byte[] ReadData(BinaryReader br)
    {
        CompressedBlob compressedBlob = new CompressedBlob();
        compressedBlob.Read(br);

        return compressedBlob.Data;
    }

    protected override void PrepareData()
    {
        _blob = new CompressedBlob { Data = Data };
        _blob.PrepareCompressedData();
    }

    protected override void WriteData(BinaryWriter bw)
    {
        _blob.Write(bw);
    }

    protected override int GetDataLength()
    {
        return _blob.CompressedData.Length;
    }
}

public class Blob32 : Blob<Key32> {}
public class Blob64 : Blob<Key64> {}