// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 6:03 PM.

using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports.Implementations;

public class DatabaseLoad : BaseDatabaseLoad<Key32>, IPointerObject<Key32>
{
    private uint _numTypes;
    private long _typeNames;
    private long _typeNamesDst;

    private long _typeNamesSrc;

    public void ReadPointerData(VaultReadContext<Key32> context, BinaryReader br)
    {
        br.BaseStream.Position = _typeNames;

        foreach (var t in context.Database.Types)
        {
            t.Name = NullTerminatedString.Read(br);
            HashManager.AddVlt(t.Name);
        }
    }

    public void WritePointerData(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        _typeNamesDst = bw.BaseStream.Position;

        foreach (var type in context.Database.Types) NullTerminatedString.Write(bw, type.Name);

        bw.AlignWriter(8);
    }

    public void AddPointers(VaultWriteContext<Key32> context)
    {
        context.AddPointer(_typeNamesSrc, _typeNamesDst, true);
    }

    public override void Read(VaultReadContext<Key32> context, BinaryReader br)
    {
        br.ReadUInt32();
        br.ReadUInt32();
        _numTypes = br.ReadUInt32();
        //br.ReadUInt32();
        _typeNames = br./*ReadPointer*/ReadInt32(); // Pointer

        if (_typeNames == 0) throw new InvalidDataException("NULL pointer to mTypeNames is no good!");

        for (var i = 0; i < _numTypes; i++)
        {
            var typeInfo = new DatabaseTypeInfo { Size = br.ReadUInt32() };
            context.Database.Types.Add(typeInfo);
        }
    }

    public override void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        bw.Write(context.Database.Classes.Count);
        // DefaultDataSize is the size, in bytes, of the largest defined type.
        // Generated AttribSys headers would have a static array of bytes of this length.
        bw.Write(context.Database.Types.Max(t => t.Size));
        bw.Write(context.Database.Types.Count);
        _typeNamesSrc = bw.BaseStream.Position;
        bw.Write(0);

        foreach (var databaseType in context.Database.Types) bw.Write(databaseType.Size);
    }

    public override Key32 GetExportId()
    {
        return new Key32(0xF1DFAC8D);
    }
}