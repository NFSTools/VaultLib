// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:28 AM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.ModernBase;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(ControllerDataRecord))]
    public class ControllerDataRecord : VLTBaseType, IReferencesStrings
    {
        public string DeviceID { get; set; }
        public InputUpdateType UpdateType { get; set; }
        public float LowerDZ { get; set; }
        public float UpperDZ { get; set; }

        private StringKey _deviceID { get; set; }


        public override void Read(Vault vault, BinaryReader br)
        {
            _deviceID = new StringKey(Class, Field, Collection);
            _deviceID.Read(vault, br);
            UpdateType = br.ReadEnum<InputUpdateType>();
            LowerDZ = br.ReadSingle();
            UpperDZ = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _deviceID = new StringKey(Class, Field, Collection) { Value = DeviceID };
            _deviceID.Write(vault, bw);
            bw.WriteEnum(UpdateType);
            bw.Write(LowerDZ);
            bw.Write(UpperDZ);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { DeviceID };
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

        public ControllerDataRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public ControllerDataRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}