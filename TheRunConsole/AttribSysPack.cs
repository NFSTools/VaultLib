using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Pack;

namespace TheRunConsole
{
    public class AttribSysPack : IVaultPack
    {
        public IList<Vault> Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions = null)
        {
            if (br.ReadUInt32() != 0x73795341)
            {
                throw new InvalidDataException("Cannot process this stream: Invalid header magic");
            }

            uint numFiles = br.ReadUInt32();

            if (numFiles != 2)
            {
                throw new InvalidDataException("Cannot process this stream: Invalid file count");
            }

            uint nameTableLength = br.ReadUInt32();

            br.ReadUInt32();

            uint vltSize = br.ReadUInt32();
            br.ReadUInt32();
            uint vltOffset = br.ReadUInt32();
            br.ReadUInt32();

            uint binSize = br.ReadUInt32();
            br.ReadUInt32();
            uint binOffset = br.ReadUInt32();
            br.ReadUInt32();

            var fileNames = new List<string>();

            for (int i = 0; i < numFiles; i++)
            {
                fileNames.Add(NullTerminatedString.Read(br));
            }

            long endOffset = br.BaseStream.Position;

            byte[] vltData = new byte[vltSize];
            br.BaseStream.Position = endOffset + vltOffset;

            if (br.Read(vltData, 0, vltData.Length) != vltData.Length)
            {
                throw new Exception("could not read VLT data");
            }

            byte[] binData = new byte[binSize];
            br.BaseStream.Position = endOffset + binOffset;

            if (br.Read(binData, 0, binData.Length) != binData.Length)
            {
                throw new Exception("could not read BIN data");
            }

            Vault vault = new Vault(fileNames[0].Substring(0, fileNames[0].IndexOf('.')))
            {
                BinStream = new MemoryStream(binData),
                VltStream = new MemoryStream(vltData)
            };

            using (VaultLoadingWrapper loadingWrapper = new VaultLoadingWrapper(vault, loadingOptions?.ByteOrder ?? ByteOrder.Little))
            {
                database.LoadVault(vault, loadingWrapper);
            }

            return new ReadOnlyCollection<Vault>(new List<Vault>(new[] { vault }));
        }

        public void Save(BinaryWriter bw, IList<Vault> vaults, PackSavingOptions savingOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}