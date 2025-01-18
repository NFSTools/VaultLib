// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 8:32 PM.

using CoreLibraries.IO;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VltAttribType : VltBaseType, IPointerObject
    {
        private long _offsetDst;

        private long _offsetSrc;

        public VltAttribType(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        public VltAttribType(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Offset { get; set; } // pointer to bin stream
        public object Data { get; set; }

        public void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            Debug.Assert(Offset != 0);

            br.BaseStream.Position = Offset;
            Data = context.Database.TypeRegistry.ReadFieldValue(Class, Field, Collection, context, br);

            if (!(Data is VltArrayType))
                Debug.Assert(br.BaseStream.Position - Offset == Field.Size,
                    "br.BaseStream.Position - Offset == Field.Size");
        }

        public void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            bw.AlignWriter(Field.Alignment);
            _offsetDst = bw.BaseStream.Position;
            context.Database.TypeRegistry.WriteFieldValue(Field, Data, context, bw);

            if (Data is IPointerObject pointerObject) pointerObject.WritePointerData(context, bw);
        }

        public void AddPointers(VaultWriteContext context)
        {
            Debug.Assert(_offsetSrc != 0 && _offsetDst != 0);

            context.AddPointer(_offsetSrc, _offsetDst, true);

            if (Data is IPointerObject pointerObject) pointerObject.AddPointers(context);
        }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Offset = br.ReadPointer();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            _offsetSrc = bw.BaseStream.Position;
            bw.Write(0);
        }
    }
}