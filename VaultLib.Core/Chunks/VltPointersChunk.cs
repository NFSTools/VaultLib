// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:12 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Chunks;

public class VltPointersChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    public override uint Id => 0x5074724E;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        var binPointers = new List<IPtrRef<TKey>>();
        var vltPointers = new List<IPtrRef<TKey>>();

        var isVltPointer = false;

        while (br.BaseStream.Position < EndOffset)
        {
            var ptr = context.Database.ExportFactory.CreatePtrRef();
            ptr.Read(context, br);

            switch (ptr.PtrType)
            {
                case EPtrRefType.PtrSetFixupTarget:
                    isVltPointer = ptr.Index == 0;
                    break;
                case EPtrRefType.PtrDepRelative:
                case EPtrRefType.PtrNull:
                    if (isVltPointer)
                    {
                        Debug.Assert(ptr.FixupOffset <= context.VltStream.Length);
                        vltPointers.Add(ptr);
                    }
                    else
                    {
                        Debug.Assert(ptr.FixupOffset <= context.BinStream.Length);
                        binPointers.Add(ptr);
                    }

                    break;
            }

            if (ptr.PtrType == EPtrRefType.PtrEnd) break;
        }

        foreach (var ptrRef in binPointers)
            context.Pointers.Add(new VltPointer
                { Type = VltPointerType.Bin, Destination = ptrRef.Destination, FixUpOffset = ptrRef.FixupOffset });

        foreach (var ptrRef in vltPointers)
            context.Pointers.Add(new VltPointer
                { Type = VltPointerType.Vlt, Destination = ptrRef.Destination, FixUpOffset = ptrRef.FixupOffset });
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        var binPointers = context.Pointers.Where(p => p.Type == VltPointerType.Bin).ToList();
        var vltPointers = context.Pointers.Where(p => p.Type == VltPointerType.Vlt).ToList();

        {
            var targetBin = context.Database.ExportFactory.CreatePtrRef();
            targetBin.PtrType = EPtrRefType.PtrSetFixupTarget;
            targetBin.Index = 1;
            targetBin.Write(context, bw);

            foreach (var binPointer in binPointers)
            {
                var ptr = context.Database.ExportFactory.CreatePtrRef();
                ptr.PtrType = binPointer.Destination == 0 ? EPtrRefType.PtrNull : EPtrRefType.PtrDepRelative;
                ptr.FixupOffset = binPointer.FixUpOffset;
                ptr.Destination = binPointer.Destination;
                ptr.Index = 1;
                ptr.Write(context, bw);
            }
        }

        {
            var targetVlt = context.Database.ExportFactory.CreatePtrRef();

            targetVlt.PtrType = EPtrRefType.PtrSetFixupTarget;
            targetVlt.Index = 0;
            targetVlt.Write(context, bw);

            foreach (var vltPointer in vltPointers)
            {
                var ptr = context.Database.ExportFactory.CreatePtrRef();
                ptr.PtrType = vltPointer.Destination == 0 ? EPtrRefType.PtrNull : EPtrRefType.PtrDepRelative;
                ptr.FixupOffset = vltPointer.FixUpOffset;
                ptr.Destination = vltPointer.Destination;
                ptr.Index = 1;
                ptr.Write(context, bw);
            }
        }

        {
            var end = context.Database.ExportFactory.CreatePtrRef();
            end.PtrType = EPtrRefType.PtrEnd;
            end.Write(context, bw);
        }

        bw.AlignWriter(0x10);
    }
}