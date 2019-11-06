// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:53 PM.

using System.IO;

namespace VaultLib.Core.Exports
{
    public class VaultSlotExport : BaseExport
    {
        public override void Read(Vault vault, BinaryReader br)
        {
            //vault.IsGamePlayVault = true;
            br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(0);
        }

        public override uint GetExportID()
        {
            return 0x4FD71F0B;
        }

        public override uint GetTypeId()
        {
            return 0x1C54EE91;
        }
    }
}