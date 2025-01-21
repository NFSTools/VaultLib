// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:52 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS;

[VltTypeInfo("NIS::NISCar")]
public class NISCar : VltBaseType, IReferencesStrings
{
    public RefSpec PresetRide { get; set; } = new();
    public string PresetSkinName { get; set; } = string.Empty;
    public uint VehicleCategory { get; set; }
    public string ChannelName { get; set; } = string.Empty;

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        PresetRide.Read(context, fieldContext, br);
        PresetSkinName = context.ReadString(br);
        VehicleCategory = br.ReadUInt32();
        ChannelName = context.ReadString(br);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        PresetRide.Write(context, fieldContext, bw);
        context.WriteString(PresetSkinName, fieldContext, bw);
        bw.Write(VehicleCategory);
        context.WriteString(ChannelName, fieldContext, bw);
    }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { PresetSkinName, ChannelName };
    }
}