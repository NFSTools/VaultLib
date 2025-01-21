using System.IO;

namespace VaultLib.Core.Types.Attrib;

// For information about the blob system, go to: https://github.com/NFSTools/VaultLib/issues/1
[VltTypeInfo("Attrib::Blob")]
public class Blob : BaseBlob
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
}