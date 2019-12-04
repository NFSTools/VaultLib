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

namespace VaultLib.Support.ProStreet.VLT
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

        public override void Read(Vault vault, BinaryReader br)
        {
            Car = new RefSpec(Class, Field, Collection);
            _eventText = new Text(Class, Field, Collection);

            Car.Read(vault, br);
            _eventText.Read(vault, br);
            MinDelta = br.ReadSingle();
            MaxDelta = br.ReadSingle();
            Shift = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Car.Write(vault, bw);
            _eventText.Write(vault, bw);
            bw.Write(MinDelta);
            bw.Write(MaxDelta);
            bw.Write(Shift);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _eventText.ReadPointerData(vault, br);
            Event = _eventText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _eventText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _eventText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _eventText.GetStrings();
        }

        public TargetTimeOverrides(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public TargetTimeOverrides(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}