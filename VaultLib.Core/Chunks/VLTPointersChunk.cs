// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:12 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Structures;

namespace VaultLib.Core.Chunks
{
    public class VltPointersChunk : ChunkBase
    {
        public override uint Id => 0x5074724E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            var binPointers = new List<IPtrRef>();
            var vltPointers = new List<IPtrRef>();

            var isVltPointer = false;

            while (br.BaseStream.Position < EndOffset)
            {
                var ptr = ExportFactory.CreatePtrRef(vault);
                ptr.Read(vault, br);

                switch (ptr.PtrType)
                {
                    case EPtrRefType.PtrSetFixupTarget:
                        isVltPointer = ptr.Index == 0;
                        break;
                    case EPtrRefType.PtrDepRelative:
                    case EPtrRefType.PtrNull:
                        if (isVltPointer)
                        {
                            Debug.Assert(ptr.FixupOffset <= vault.VltStream.Length);
                            vltPointers.Add(ptr);
                        }
                        else
                        {
                            Debug.Assert(ptr.FixupOffset <= vault.BinStream.Length);
                            binPointers.Add(ptr);
                        }

                        break;
                }

                if (ptr.PtrType == EPtrRefType.PtrEnd) break;
            }

            foreach (var ptrRef in binPointers)
                vault.Pointers.Add(new VltPointer
                    {Type = VltPointerType.Bin, Destination = ptrRef.Destination, FixUpOffset = ptrRef.FixupOffset});

            foreach (var ptrRef in vltPointers)
                vault.Pointers.Add(new VltPointer
                    {Type = VltPointerType.Vlt, Destination = ptrRef.Destination, FixUpOffset = ptrRef.FixupOffset});
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            var binPointers = vault.SaveContext.Pointers.Where(p => p.Type == VltPointerType.Bin).ToList();
            var vltPointers = vault.SaveContext.Pointers.Where(p => p.Type == VltPointerType.Vlt).ToList();

            {
                var targetBin = ExportFactory.CreatePtrRef(vault);
                targetBin.PtrType = EPtrRefType.PtrSetFixupTarget;
                targetBin.Index = 1;
                targetBin.Write(vault, bw);

                foreach (var binPointer in binPointers)
                {
                    var ptr = ExportFactory.CreatePtrRef(vault);
                    ptr.PtrType = binPointer.Destination == 0 ? EPtrRefType.PtrNull : EPtrRefType.PtrDepRelative;
                    ptr.FixupOffset = binPointer.FixUpOffset;
                    ptr.Destination = binPointer.Destination;
                    ptr.Index = 1;
                    ptr.Write(vault, bw);
                }
            }

            {
                var targetVlt = ExportFactory.CreatePtrRef(vault);

                targetVlt.PtrType = EPtrRefType.PtrSetFixupTarget;
                targetVlt.Index = 0;
                targetVlt.Write(vault, bw);

                foreach (var vltPointer in vltPointers)
                {
                    var ptr = ExportFactory.CreatePtrRef(vault);
                    ptr.PtrType = vltPointer.Destination == 0 ? EPtrRefType.PtrNull : EPtrRefType.PtrDepRelative;
                    ptr.FixupOffset = vltPointer.FixUpOffset;
                    ptr.Destination = vltPointer.Destination;
                    ptr.Index = 1;
                    ptr.Write(vault, bw);
                }
            }

            {
                var end = ExportFactory.CreatePtrRef(vault);
                end.PtrType = EPtrRefType.PtrEnd;
                end.Write(vault, bw);
            }

            bw.AlignWriter(0x10);
        }
    }
}