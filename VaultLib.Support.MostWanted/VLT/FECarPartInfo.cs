﻿using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(FECarPartInfo))]
    public class FECarPartInfo : VLTBaseType
    {
        public FECarPartInfo(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FECarPartInfo(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eFEPartUpgradeLevels Level { get; set; }
        public float Unknown { get; set; }
        public float Cost { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Level = br.ReadEnum<eFEPartUpgradeLevels>();
            Unknown = br.ReadSingle();
            Cost = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Level);
            bw.Write(Unknown);
            bw.Write(Cost);
        }
    }
}