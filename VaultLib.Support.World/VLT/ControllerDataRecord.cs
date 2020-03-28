// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:28 AM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(ControllerDataRecord))]
    public class ControllerDataRecord : VLTBaseType, IReferencesStrings
    {
        public string DeviceId { get; set; }
        public InputUpdateType UpdateType { get; set; }
        public float LowerDeadZone { get; set; }
        public float UpperDeadZone { get; set; }

        private StringKey InternalDeviceId { get; set; }


        public override void Read(Vault vault, BinaryReader br)
        {
            InternalDeviceId.Read(vault, br);
            UpdateType = br.ReadEnum<InputUpdateType>();
            LowerDeadZone = br.ReadSingle();
            UpperDeadZone = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            InternalDeviceId.Value = DeviceId;
            InternalDeviceId.Write(vault, bw);
            bw.WriteEnum(UpdateType);
            bw.Write(LowerDeadZone);
            bw.Write(UpperDeadZone);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { DeviceId };
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            InternalDeviceId.ReadPointerData(vault, br);
            DeviceId = InternalDeviceId.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            InternalDeviceId.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            InternalDeviceId.AddPointers(vault);
        }

        public ControllerDataRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            InternalDeviceId = new StringKey(Class, Field, Collection);
        }
    }
}