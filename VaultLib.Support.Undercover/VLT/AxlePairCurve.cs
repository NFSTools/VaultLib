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

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            Front.Read(context, br);
            Rear.Read(context, br);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            Front.Write(context, bw);
            Rear.Write(context, bw);
        }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            Front.ReadPointerData(context, br);
            Rear.ReadPointerData(context, br);
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