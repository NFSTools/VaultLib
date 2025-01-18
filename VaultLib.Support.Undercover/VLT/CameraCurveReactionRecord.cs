// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:35 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(CameraCurveReactionRecord))]
    public class CameraCurveReactionRecord : VltBaseType, IVltPointerObject
    {
        public Curve Curve { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
            Curve.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(0);
            Curve.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Curve.ReadPointerData(context, fieldContext, br);
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Curve.WritePointerData(context, fieldContext, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            Curve.AddPointers(context, fieldContext);
        }
    }
}