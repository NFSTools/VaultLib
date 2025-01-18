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
    [VltTypeInfo(nameof(Curve))]
    public class Curve : VltBaseType, IVltPointerObject
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

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            MinX = br.ReadSingle();
            MaxX = br.ReadSingle();
            MinY = br.ReadSingle();
            MaxY = br.ReadSingle();
            GraphScale = br.ReadSingle();
            _xArray.Read(br);
            _yArray.Read(br);
            _y2Array.Read(br);
            br.ReadUInt32();
            //Debug.Assert(br.ReadUInt32()==0);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _xArray.Data = XValues;
            _yArray.Data = YValues;
            _y2Array.Data = Y2Values;
            
            bw.Write(MinX);
            bw.Write(MaxX);
            bw.Write(MinY);
            bw.Write(MaxY);
            bw.Write(GraphScale);

            _xArray.Write(bw);
            _yArray.Write(bw);
            _y2Array.Write(bw);

            bw.Write(0); // AllocatedMemory (bool1 + 3 align bytes)
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _xArray.ReadPointerData(context, br);
            _yArray.ReadPointerData(context, br);
            _y2Array.ReadPointerData(context, br);

            XValues = _xArray.Data;
            YValues = _yArray.Data;
            Y2Values = _y2Array.Data;
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _xArray.WritePointerData(context, bw);
            _yArray.WritePointerData(context, bw);
            _y2Array.WritePointerData(context, bw);
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            _xArray.AddPointers(context);
            _yArray.AddPointers(context);
            _y2Array.AddPointers(context);
        }

        public Curve(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            _xArray = new VariableArray();
            _yArray = new VariableArray();
            _y2Array = new VariableArray();
        }
    }
}