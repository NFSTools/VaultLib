// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 8:32 PM.

using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VLTAttribType : VLTBaseType, IPointerObject
    {
        private long _offsetDst;

        private long _offsetSrc;

        public VLTAttribType(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field,
            collection)
        {
        }

        public VLTAttribType(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }

        public uint Offset { get; set; } // pointer to bin stream
        public VLTBaseType Data { get; set; }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Data = TypeRegistry.CreateInstance(vault.Database.Options.GameId, Class, Field, Collection);

            Debug.Assert(Offset != 0);
            br.BaseStream.Position = Offset;

            Data.Read(vault, br);

            if (!(Data is VLTArrayType))
                Debug.Assert(br.BaseStream.Position - Offset == Field.Size);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            bw.AlignWriter(Field.Alignment);
            _offsetDst = bw.BaseStream.Position;
            Data.Write(vault, bw);

            if (Data is IPointerObject pointerObject) pointerObject.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            Debug.Assert(_offsetSrc != 0 && _offsetDst != 0);

            vault.SaveContext.AddPointer(_offsetSrc, _offsetDst, true);

            if (Data is IPointerObject pointerObject) pointerObject.AddPointers(vault);
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            Offset = br.ReadPointer();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _offsetSrc = bw.BaseStream.Position;
            bw.Write(0);
        }
    }
}