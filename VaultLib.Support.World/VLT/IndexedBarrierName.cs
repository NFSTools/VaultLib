// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 10:00 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.Support.World.VLT
{
    public class IndexedBarrierName : PrimitiveTypeBase
    {
        public int ID { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ID = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(ID);
        }

        public override IConvertible GetValue()
        {
            return ID;
        }

        public override void SetValue(IConvertible value)
        {
            ID = (int) value;
        }
    }
}