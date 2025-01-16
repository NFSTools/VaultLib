using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT.Attrib
{
    /// <summary>
    /// Need for Speed games by EA Black Box have compressed data stored in Attrib::Blob instances.
    /// </summary>
    [VLTTypeInfo("Attrib::Blob")]
    public class Blob : BaseBlob
    {
        private CompressedBlob _blob;

        public Blob(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Blob(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

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
}