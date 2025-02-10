using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Types.Attrib;

// For information about the blob system, go to: https://github.com/NFSTools/VaultLib/issues/1
[VltTypeInfo("Attrib::Blob")]
public class Blob<TKey> : BaseBlob<TKey> where TKey : struct, IKey<TKey>
{
    protected override void PrepareData()
    {
        //
    }

    protected override int GetDataLength()
    {
        return Data.Length;
    }

    protected override void WriteData(BinaryWriter bw)
    {
        bw.Write(Data);
    }

    public override object Clone()
    {
        return new Blob<TKey> { Data = (byte[])Data.Clone() };
    }
}