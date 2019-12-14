// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:12 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.AI
{
    [VLTTypeInfo("AI::GlueCurve")]
    public class GlueCurve : VLTBaseType, IPointerObject
    {
        public Curve Easy { get; set; }
        public Curve Hard { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Easy = new Curve(Class, Field, Collection);
            Hard = new Curve(Class, Field, Collection);

            Easy.Read(vault, br);
            Hard.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Easy.Write(vault, bw);
            Hard.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Easy.ReadPointerData(vault, br);
            Hard.ReadPointerData(vault, br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            Easy.WritePointerData(vault, bw);
            Hard.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            Easy.AddPointers(vault);
            Hard.AddPointers(vault);
        }

        public GlueCurve(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GlueCurve(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}