using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.Attrib
{
    /// <summary>
    /// Need for Speed games by EA Black Box have compressed data stored in Attrib::Blob instances.
    /// </summary>
    [VLTTypeInfo("Attrib::Blob")]
    public class Blob : BaseBlob
    {
        public Blob(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Blob(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        private CompressedBlob _blob;

        protected override byte[] ReadData(Vault vault, BinaryReader br)
        {
            CompressedBlob compressedBlob = new CompressedBlob();
            compressedBlob.Read(vault, br);

            return compressedBlob.Data;
        }

        protected override void PrepareData()
        {
            _blob = new CompressedBlob { Type = CompressedBlob.CompressionId.LempelZiv, Data = Data };
            _blob.PrepareCompressedData();
        }

        protected override void WriteData(Vault vault, BinaryWriter bw)
        {
            _blob.Write(vault, bw);
        }

        protected override int GetDataLength()
        {
            return _blob.CompressedData.Length;
        }
    }
}