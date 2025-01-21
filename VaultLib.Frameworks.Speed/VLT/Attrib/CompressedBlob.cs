using System;
using System.IO;
using CompLib;
using CompLib.Algorithms;

namespace VaultLib.Frameworks.Speed.VLT.Attrib;

public class CompressedBlob
{
    public byte[] CompressedData { get; set; }

    public byte[] Data { get; set; }

    public void Read(BinaryReader br)
    {
        Data = BlobDecompressor.Decompress(br).ToArray();
    }

    public void Write(BinaryWriter bw)
    {
        if (CompressedData == null)
            throw new Exception("compressed data buffer is null");
        bw.Write(CompressedData);
    }

    public void PrepareCompressedData()
    {
        CompressedData = BlobCompressor.Compress(Data, new JdlzAlgorithm()).ToArray();
    }
}