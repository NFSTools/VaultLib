// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:46 AM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VltTypeInfo("NIS::NISActor")]
    public class NISActor : VltBaseType, IReferencesStrings
    {
        public string ActorName { get; set; }
        public string CarChannelName { get; set; }
        public bool IsDriver { get; set; }
        public float ExitAnimSec { get; set; }
        public bool IsFacePixelation { get; set; }

        private Text _actorNameText, _carChannelNameText;

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _actorNameText.Read(context, fieldContext, br);
            _carChannelNameText.Read(context, fieldContext, br);

            IsDriver = br.ReadBoolean();
            br.AlignReader(4);
            ExitAnimSec = br.ReadSingle();
            IsFacePixelation = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _actorNameText.Value = ActorName;
            _carChannelNameText.Value = CarChannelName;
            _actorNameText.Write(context, fieldContext, bw);
            _carChannelNameText.Write(context, fieldContext, bw);
            bw.Write(IsDriver);
            bw.AlignWriter(4);
            bw.Write(ExitAnimSec);
            bw.Write(IsFacePixelation);
            bw.AlignWriter(4);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _actorNameText.ReadPointerData(context, fieldContext, br);
            _carChannelNameText.ReadPointerData(context, fieldContext, br);

            ActorName = _actorNameText.Value;
            CarChannelName = _carChannelNameText.Value;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _actorNameText.WritePointerData(context, fieldContext, bw);
            _carChannelNameText.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _actorNameText.AddPointers(context, fieldContext);
            _carChannelNameText.AddPointers(context, fieldContext);
        }

        public IEnumerable<string> GetStrings()
        {
            return _actorNameText.GetStrings().Concat(_carChannelNameText.GetStrings());
        }

        public NISActor(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _actorNameText = new Text(Class, Field, Collection);
            _carChannelNameText = new Text(Class, Field, Collection);
            ActorName = string.Empty;
            CarChannelName = string.Empty;
        }
    }
}