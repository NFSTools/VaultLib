// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:46 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::NISActor")]
    public class NISActor : VLTBaseType, IReferencesStrings
    {
        public string ActorName { get; set; }
        public string CarChannelName { get; set; }
        public bool IsDriver { get; set; }
        public float ExitAnimSec { get; set; }
        public bool IsFacePixelation { get; set; }

        private Text _actorNameText, _carChannelNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _actorNameText = new Text(Class, Field, Collection);
            _carChannelNameText = new Text(Class, Field, Collection);

            _actorNameText.Read(vault, br);
            _carChannelNameText.Read(vault, br);

            IsDriver = br.ReadBoolean();
            br.AlignReader(4);
            ExitAnimSec = br.ReadSingle();
            IsFacePixelation = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _actorNameText.Write(vault, bw);
            _carChannelNameText.Write(vault, bw);
            bw.Write(IsDriver);
            bw.AlignWriter(4);
            bw.Write(ExitAnimSec);
            bw.Write(IsFacePixelation);
            bw.AlignWriter(4);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _actorNameText.ReadPointerData(vault, br);
            _carChannelNameText.ReadPointerData(vault, br);

            ActorName = _actorNameText.Value;
            CarChannelName = _carChannelNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _actorNameText.WritePointerData(vault, bw);
            _carChannelNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _actorNameText.AddPointers(vault);
            _carChannelNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _actorNameText.GetStrings().Concat(_carChannelNameText.GetStrings());
        }

        public NISActor(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public NISActor(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}