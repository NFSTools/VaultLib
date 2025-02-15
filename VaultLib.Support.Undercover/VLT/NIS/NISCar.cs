// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:52 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::NISCar")]
public class NISCar : VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public RefSpec32 PresetRide { get; set; } = new();
    public string PresetSkinName { get; set; } = string.Empty;
    public uint VehicleCategory { get; set; }
    public string ChannelName { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PresetRide.Read(context, fieldContext, br);
        PresetSkinName = context.ReadString(br);
        VehicleCategory = br.ReadUInt32();
        ChannelName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PresetRide.Write(context, fieldContext, bw);
        context.WriteString(PresetSkinName, fieldContext, bw);
        bw.Write(VehicleCategory);
        context.WriteString(ChannelName, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { PresetSkinName, ChannelName };
    }

    public override object Clone()
    {
        return new NISCar
        {
            ChannelName = ChannelName,
            PresetRide = (RefSpec32)PresetRide.Clone(),
            PresetSkinName = PresetSkinName,
            VehicleCategory = VehicleCategory,
        };
    }
}