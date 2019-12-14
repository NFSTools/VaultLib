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

        public override void Read(Vault vault, BinaryReader br)
        {
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
            Curve = new Curve(Class, Field, Collection);
            Curve.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(0);
            Curve.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Curve.ReadPointerData(vault, br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            Curve.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            Curve.AddPointers(vault);
        }

        public CameraCurveReactionRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CameraCurveReactionRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}