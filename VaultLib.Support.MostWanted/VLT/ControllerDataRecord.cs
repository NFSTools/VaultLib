// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:22 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;
using VaultLib.LegacyBase;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(ControllerDataRecord))]
public class ControllerDataRecord : VltBaseType<Core.DataInterfaces.Key32>,
    IReferencesStrings
{
    public string DeviceID { get; set; } = string.Empty;
    public InputUpdateType UpdateType { get; set; }
    public float LowerDZ { get; set; }
    public float UpperDZ { get; set; }

    private StringKey64 _deviceID { get; set; } = new();


    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        _deviceID.Read(context, fieldContext, br);
        DeviceID = _deviceID.Value;
        UpdateType = br.ReadEnum<InputUpdateType>();
        LowerDZ = br.ReadSingle();
        UpperDZ = br.ReadSingle();
        uint unk = br.ReadUInt32();

        Debug.Assert(unk == 0);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        _deviceID.Value = DeviceID;
        _deviceID.Write(context, fieldContext, bw);
        bw.WriteEnum(UpdateType);
        bw.Write(LowerDZ);
        bw.Write(UpperDZ);
        bw.Write(0);
    }

    public override object Clone()
    {
        return new ControllerDataRecord
        {
            DeviceID = DeviceID,
            LowerDZ = LowerDZ,
            UpdateType = UpdateType,
            UpperDZ = UpperDZ,
        };
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { DeviceID };
    }
}