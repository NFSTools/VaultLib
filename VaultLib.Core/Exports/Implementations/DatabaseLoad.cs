// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 6:03 PM.

using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports.Implementations
{
    public class DatabaseLoad : BaseDatabaseLoad, IPointerObject
    {
        private uint _typeNames;
        private uint _numTypes;

        private long _typeNamesSrc;
        private long _typeNamesDst;

        public override void Read(Vault vault, BinaryReader br)
        {
            uint mNumClasses = br.ReadUInt32();
            uint mDefaultDataSize = br.ReadUInt32();
            _numTypes = br.ReadUInt32();
            _typeNames = br.ReadPointer(vault, true); // Pointer

            Debug.WriteLine(
                "Database Load: {0} classes | DefaultData: {1} bytes | {2} types | type names @ bin+0x{3:X}",
                mNumClasses, mDefaultDataSize, _numTypes, _typeNames);

            for (int i = 0; i < _numTypes; i++)
            {
                DatabaseTypeInfo typeInfo = new DatabaseTypeInfo();
                typeInfo.Size = br.ReadUInt32();
                vault.Database.Types.Add(typeInfo);
            }

            Debug.WriteLine("DefaultDataSize calc = {0}", vault.Database.Types.Max(t => t.Size));
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

            foreach (var databaseType in vault.Database.Types)
            {
                bw.Write(databaseType.Size);
            }
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Debug.WriteLine("DatabaseLoadData: loading type names from {0:X}", _typeNames);

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

            foreach (var type in vault.Database.Types)
            {
                NullTerminatedString.Write(bw, type.Name);
            }

            bw.AlignWriter(8);
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_typeNamesSrc, _typeNamesDst, true);
        }
    }
}