// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:35 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(CameraCurveReactionRecord))]
    public class CameraCurveReactionRecord : VLTBaseType, IPointerObject
    {
        public Curve Curve { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
            Curve.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(0);
            Curve.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            Curve.ReadPointerData(context, br);
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            Curve.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            Curve.AddPointers(context);
        }

        public CameraCurveReactionRecord(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Curve = new Curve(Class, Field, Collection);
        }
    }
}