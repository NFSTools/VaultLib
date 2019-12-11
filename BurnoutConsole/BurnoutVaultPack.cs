using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Pack;

namespace BurnoutConsole
{
    public class BurnoutVaultPack : IVaultPack
    {
        private readonly string _vaultName;

        public BurnoutVaultPack(string vaultName)
        {
            _vaultName = vaultName;
        }

        public IList<Vault> Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions = null)
        {
            uint vltOffset = br.ReadUInt32();
            uint vltSize = br.ReadUInt32();
            uint binOffset = br.ReadUInt32();
            uint binSize = br.ReadUInt32();

            if (vltOffset > br.BaseStream.Length)
                throw new InvalidDataException();

            if (binOffset > br.BaseStream.Length)
                throw new InvalidDataException();

            br.BaseStream.Position = vltOffset;
            byte[] vltData = new byte[vltSize];

            if (br.Read(vltData) != vltData.Length)
            {
                throw new InvalidDataException();
            }

            br.BaseStream.Position = binOffset;
            byte[] binData = new byte[binSize];

            if (br.Read(binData) != binData.Length)
            {
                throw new InvalidDataException();
            }

            Vault vault = new Vault(_vaultName)
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