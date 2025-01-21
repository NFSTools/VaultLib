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

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(ControllerDataRecord))]
public class ControllerDataRecord : VltBaseType, IReferencesStrings
{
    public string DeviceID { get; set; } = string.Empty;
    public InputUpdateType UpdateType { get; set; }
    public float LowerDZ { get; set; }
    public float UpperDZ { get; set; }

    private StringKey _deviceID { get; set; } = new();


    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        _deviceID.Read(context, fieldContext, br);
        UpdateType = br.ReadEnum<InputUpdateType>();
        LowerDZ = br.ReadSingle();
        UpperDZ = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _deviceID.Value = DeviceID;
        _deviceID.Write(context, fieldContext, bw);
        bw.WriteEnum(UpdateType);
        bw.Write(LowerDZ);
        bw.Write(UpperDZ);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { DeviceID };
    }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        _deviceID.ReadPointerData(context, fieldContext, br);
        DeviceID = _deviceID.Value;
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _deviceID.WritePointerData(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
        _deviceID.AddPointers(context, fieldContext);
    }
}