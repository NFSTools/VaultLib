using System;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.Attrib
{
    /// <summary>
    /// Need for Speed games by EA Black Box have compressed data stored in Attrib::Blob instances.
    /// </summary>
    public class Blob : BaseBlob
    {
        public Blob(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Blob(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        protected override byte[] ReadData(BinaryReader br)
        {
            throw new NotImplementedException();
        }

        protected override byte[] PrepareData()
        {
            throw new System.NotImplementedException();
        }
    }
}