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
using VaultLib.Frameworks.Speed.VLT;
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


        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            InternalDeviceId.Read(context, br);
            UpdateType = br.ReadEnum<InputUpdateType>();
            LowerDeadZone = br.ReadSingle();
            UpperDeadZone = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            InternalDeviceId.Value = DeviceId;
            InternalDeviceId.Write(context, bw);
            bw.WriteEnum(UpdateType);
            bw.Write(LowerDeadZone);
            bw.Write(UpperDeadZone);
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { DeviceId };
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            InternalDeviceId.ReadPointerData(context, br);
            DeviceId = InternalDeviceId.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            InternalDeviceId.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            InternalDeviceId.AddPointers(context);
        }

        public ControllerDataRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            InternalDeviceId = new StringKey(Class, Field, Collection);
            DeviceId = string.Empty;
        }
    }
}