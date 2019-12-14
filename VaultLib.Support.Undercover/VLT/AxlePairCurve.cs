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
            Front = new Curve(Class, Field, Collection);
            Rear = new Curve(Class, Field, Collection);

            Front.Read(vault, br);
            Rear.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Front.Write(vault, bw);
            Rear.Write(vault, bw);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Front.ReadPointerData(vault, br);
            Rear.ReadPointerData(vault, br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            Front.WritePointerData(vault, bw);
            Rear.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            Front.AddPointers(vault);
            Rear.AddPointers(vault);
        }

        public AxlePairCurve(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AxlePairCurve(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}