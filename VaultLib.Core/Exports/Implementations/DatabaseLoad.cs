// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 6:03 PM.

using CoreLibraries.IO;
using System.IO;
using System.Linq;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports.Implementations
{
    public class DatabaseLoad : BaseDatabaseLoad, IPointerObject
    {
        private uint _numTypes;
        private long _typeNames;
        private long _typeNamesDst;

        private long _typeNamesSrc;

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _typeNames;

            foreach (var t in vault.Database.Types)
            {
                t.Name = NullTerminatedString.Read(br);
                HashManager.AddVLT(t.Name);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _typeNamesDst = bw.BaseStream.Position;

            foreach (var type in vault.Database.Types) NullTerminatedString.Write(bw, type.Name);

            bw.AlignWriter(8);
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_typeNamesSrc, _typeNamesDst, true);
        }

        public override void Read(Vault vault, BinaryReader br)
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
                vault.Database.Types.Add(typeInfo);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(vault.Database.Classes.Count);
            // DefaultDataSize is the size, in bytes, of the largest defined type.
            // Generated AttribSys headers would have a static array of bytes of this length.
            bw.Write(vault.Database.Types.Max(t => t.Size));
            bw.Write(vault.Database.Types.Count);
            _typeNamesSrc = bw.BaseStream.Position;
            bw.Write(0);

            foreach (var databaseType in vault.Database.Types) bw.Write(databaseType.Size);
        }
    }
}