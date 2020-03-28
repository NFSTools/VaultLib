// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:22 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed;
using VaultLib.LegacyBase;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(ControllerDataRecord))]
    public class ControllerDataRecord : VLTBaseType, IReferencesStrings
    {
        public string DeviceID { get; set; }
        public InputUpdateType UpdateType { get; set; }
        public float LowerDZ { get; set; }
        public float UpperDZ { get; set; }

        private StringKey64 _deviceID { get; set; }


        public override void Read(Vault vault, BinaryReader br)
        {
            _deviceID.Read(vault, br);
            UpdateType = br.ReadEnum<InputUpdateType>();
            LowerDZ = br.ReadSingle();
            UpperDZ = br.ReadSingle();
            uint unk = br.ReadUInt32();

            Debug.Assert(unk == 0);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _deviceID.Value = DeviceID;
            _deviceID.Write(vault, bw);
            bw.WriteEnum(UpdateType);
            bw.Write(LowerDZ);
            bw.Write(UpperDZ);
            bw.Write(0);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _deviceID.ReadPointerData(vault, br);
            DeviceID = _deviceID.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _deviceID.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _deviceID.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { DeviceID };
        }

        public ControllerDataRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _deviceID = new StringKey64(Class, Field, Collection);
        }
    }
}