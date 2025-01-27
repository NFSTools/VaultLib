// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:28 AM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;
using VaultLib.ModernBase;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(ControllerDataRecord))]
public class ControllerDataRecord: VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IReferencesStrings<VaultLib.Core.DataInterfaces.Key32>
{
    public string DeviceId { get; set; } = string.Empty;
    public InputUpdateType UpdateType { get; set; }
    public float LowerDeadZone { get; set; }
    public float UpperDeadZone { get; set; }

    private StringKey32 InternalDeviceId { get; set; } = new();


    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        InternalDeviceId.Read(context, fieldContext, br);
        UpdateType = br.ReadEnum<InputUpdateType>();
        LowerDeadZone = br.ReadSingle();
        UpperDeadZone = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        InternalDeviceId.Value = DeviceId;
        InternalDeviceId.Write(context, fieldContext, bw);
        bw.WriteEnum(UpdateType);
        bw.Write(LowerDeadZone);
        bw.Write(UpperDeadZone);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { DeviceId };
    }

    public void ReadPointerData(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        InternalDeviceId.ReadPointerData(context, fieldContext, br);
        DeviceId = InternalDeviceId.Value;
    }

    public void WritePointerData(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        InternalDeviceId.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext)
    {
        InternalDeviceId.AddPointers(context, fieldContext);
    }
}