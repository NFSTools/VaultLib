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
            Easy.Read(vault, br);
            Hard.Read(vault, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Easy.Write(context, bw);
            Hard.Write(context, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Easy.ReadPointerData(vault, br);
            Hard.ReadPointerData(vault, br);
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            Easy.WritePointerData(context, bw);
            Hard.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            Easy.AddPointers(context);
            Hard.AddPointers(context);
        }

        public GlueCurve(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Easy = new Curve(Class, Field, Collection);
            Hard = new Curve(Class, Field, Collection);
        }
    }
}