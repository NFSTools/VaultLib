// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VLTTypeInfo(nameof(TargetTimeOverrides))]
    public class TargetTimeOverrides : VLTBaseType, IReferencesStrings
    {
        public RefSpec Car { get; set; }
        public string Event { get; set; }
        public float MinDelta { get; set; }
        public float MaxDelta { get; set; }
        public float Shift { get; set; }

        private Text _eventText;

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            Car.Read(context, br);
            _eventText.Read(context, br);
            MinDelta = br.ReadSingle();
            MaxDelta = br.ReadSingle();
            Shift = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Car.Write(context, bw);
            _eventText.Write(context, bw);
            bw.Write(MinDelta);
            bw.Write(MaxDelta);
            bw.Write(Shift);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            _eventText.ReadPointerData(context, br);
            Event = _eventText.Value;
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _eventText.Value = Event;
            _eventText.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            _eventText.AddPointers(context);
        }

        public IEnumerable<string> GetStrings()
        {
            return _eventText.GetStrings();
        }

        public TargetTimeOverrides(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Car = new RefSpec(Class, Field, Collection);
            _eventText = new Text(Class, Field, Collection);
            Event = string.Empty;
        }
    }
}