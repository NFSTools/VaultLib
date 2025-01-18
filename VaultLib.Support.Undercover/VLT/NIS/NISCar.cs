// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:52 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VltTypeInfo("NIS::NISCar")]
    public class NISCar : VltBaseType, IReferencesStrings
    {
        public RefSpec PresetRide { get; set; }
        public string PresetSkinName { get; set; }
        public uint VehicleCategory { get; set; }
        public string ChannelName { get; set; }

        private Text _presetSkinNameText, _channelNameText;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PresetRide.Read(context, fieldContext, br);
            _presetSkinNameText.Read(context, fieldContext, br);
            VehicleCategory = br.ReadUInt32();
            _channelNameText.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            PresetRide.Write(context, fieldContext, bw);
            _presetSkinNameText.Value = PresetSkinName;
            _presetSkinNameText.Write(context, fieldContext, bw);
            bw.Write(VehicleCategory);
            _channelNameText.Value = ChannelName;
            _channelNameText.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _presetSkinNameText.ReadPointerData(context, fieldContext, br);
            _channelNameText.ReadPointerData(context, fieldContext, br);

            PresetSkinName = _presetSkinNameText.Value;
            ChannelName = _channelNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _presetSkinNameText.WritePointerData(context, fieldContext, bw);
            _channelNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _presetSkinNameText.AddPointers(context, fieldContext);
            _channelNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _presetSkinNameText.GetStrings().Concat(_channelNameText.GetStrings());
        }

        public NISCar(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            PresetRide = new RefSpec(Class, Field, Collection);
            _presetSkinNameText = new Text(Class, Field, Collection);
            _channelNameText = new Text(Class, Field, Collection);
            PresetSkinName = ChannelName = string.Empty;
        }
    }
}