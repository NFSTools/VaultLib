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
    [VLTTypeInfo("NIS::NISCar")]
    public class NISCar : VLTBaseType, IReferencesStrings
    {
        public RefSpec PresetRide { get; set; }
        public string PresetSkinName { get; set; }
        public uint VehicleCategory { get; set; }
        public string ChannelName { get; set; }

        private Text _presetSkinNameText, _channelNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            PresetRide = new RefSpec(Class, Field, Collection);
            PresetRide.Read(vault, br);

            _presetSkinNameText = new Text(Class, Field, Collection);
            _presetSkinNameText.Read(vault, br);

            VehicleCategory = br.ReadUInt32();

            _channelNameText = new Text(Class, Field, Collection);
            _channelNameText.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            PresetRide.Write(vault, bw);
            _presetSkinNameText.Write(vault, bw);
            bw.Write(VehicleCategory);
            _channelNameText.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _presetSkinNameText.ReadPointerData(vault, br);
            _channelNameText.ReadPointerData(vault, br);

            PresetSkinName = _presetSkinNameText.Value;
            ChannelName = _channelNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _presetSkinNameText.WritePointerData(vault, bw);
            _channelNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _presetSkinNameText.AddPointers(vault);
            _channelNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _presetSkinNameText.GetStrings().Concat(_channelNameText.GetStrings());
        }

        public NISCar(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public NISCar(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}