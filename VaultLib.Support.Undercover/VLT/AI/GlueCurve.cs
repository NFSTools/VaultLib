// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:12 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.AI
{
    [VltTypeInfo("AI::GlueCurve")]
    public class GlueCurve : VltBaseType, IVltPointerObject
    {
        public Curve Easy { get; set; } = new();
        public Curve Hard { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Easy.Read(context, fieldContext, br);
            Hard.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Easy.Write(context, fieldContext, bw);
            Hard.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Easy.ReadPointerData(context, fieldContext, br);
            Hard.ReadPointerData(context, fieldContext, br);
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Easy.WritePointerData(context, fieldContext, bw);
            Hard.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            Easy.AddPointers(context, fieldContext);
            Hard.AddPointers(context, fieldContext);
        }
    }
}