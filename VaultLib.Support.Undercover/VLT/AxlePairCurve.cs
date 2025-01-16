// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:19 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(AxlePairCurve))]
    public class AxlePairCurve : VLTBaseType, IPointerObject
    {
        public Curve Front { get; set; }
        public Curve Rear { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Front.Read(vault, br);
            Rear.Read(vault, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Front.Write(context, bw);
            Rear.Write(context, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Front.ReadPointerData(vault, br);
            Rear.ReadPointerData(vault, br);
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            Front.WritePointerData(context, bw);
            Rear.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            Front.AddPointers(context);
            Rear.AddPointers(context);
        }

        public AxlePairCurve(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Front = new Curve(Class, Field, Collection);
            Rear = new Curve(Class, Field, Collection);
        }
    }
}