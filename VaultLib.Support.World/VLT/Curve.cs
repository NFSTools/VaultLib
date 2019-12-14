// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 8:19 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(Curve))]
    public class Curve : VLTBaseType, IPointerObject
    {
        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public float GraphScale { get; set; }

        public float[] XValues { get; set; }
        public float[] YValues { get; set; }
        public float[] Y2Values { get; set; }

        private VariableArray _xArray;
        private VariableArray _yArray;
        private VariableArray _y2Array;

        public override void Read(Vault vault, BinaryReader br)
        {
            MinX = br.ReadSingle();
            MaxX = br.ReadSingle();
            MinY = br.ReadSingle();
            MaxY = br.ReadSingle();
            GraphScale = br.ReadSingle();
            _xArray = new VariableArray();
            _yArray = new VariableArray();
            _y2Array = new VariableArray();
            _xArray.Read(vault, br);
            _yArray.Read(vault, br);
            _y2Array.Read(vault, br);
            br.ReadUInt32();
            //Debug.Assert(br.ReadUInt32()==0);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(MinX);
            bw.Write(MaxX);
            bw.Write(MinY);
            bw.Write(MaxY);
            bw.Write(GraphScale);

            _xArray = new VariableArray();
            _yArray = new VariableArray();
            _y2Array = new VariableArray();
            _xArray.Data = XValues;
            _xArray.Write(vault, bw);
            _yArray.Data = YValues;
            _yArray.Write(vault, bw);
            _y2Array.Data = Y2Values;
            _y2Array.Write(vault, bw);

            bw.Write(0); // AllocatedMemory (bool1 + 3 align bytes)
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _xArray.ReadPointerData(vault, br);
            _yArray.ReadPointerData(vault, br);
            _y2Array.ReadPointerData(vault, br);

            XValues = _xArray.Data;
            YValues = _yArray.Data;
            Y2Values = _y2Array.Data;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _xArray.WritePointerData(vault, bw);
            _yArray.WritePointerData(vault, bw);
            _y2Array.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _xArray.AddPointers(vault);
            _yArray.AddPointers(vault);
            _y2Array.AddPointers(vault);
        }

        public Curve(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Curve(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}