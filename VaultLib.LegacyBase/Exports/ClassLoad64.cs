using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports
{
    public class ClassLoad64 : BaseClassLoad
    {
        private ulong ClassHash { get; set; }
        private int NumDefinitions { get; set; }

        private uint _definitionsPtr;
        private long _srcDefinitionsPtr;
        private long _dstDefinitionsPtr;

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            ClassHash = br.ReadUInt64();
            uint cr = br.ReadUInt32(); // collection reserve
            NumDefinitions = br.ReadInt32();

            _definitionsPtr = br.ReadPointer();
            if (_definitionsPtr == 0)
            {
                throw new InvalidDataException("Definitions pointer is NULL, this is not good!");
            }

            br.ReadUInt32();
            uint u = br.ReadUInt32(); // null
            Debug.Assert(u == 0);

            ushort requiredCount = br.ReadUInt16();
            Debug.Assert(requiredCount <= NumDefinitions);
            br.ReadInt16();
            Class = new VltClass(HashManager.ResolveVlt(ClassHash));
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.Write(Vlt64Hasher.Hash(Class.Name));

            int collReserve = (from collection in context.Database.RowManager.GetFlattenedCollections(Class.Name)
                               select collection).Count();

            if (collReserve == 0)
            {
                throw new InvalidDataException("Cannot serialize legacy ClassLoadData when mCollectionReserve is 0.");
            }

            bw.Write(collReserve);
            bw.Write(Class.Fields.Count);
            _srcDefinitionsPtr = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(ComputeBaseSize());
            bw.Write(0);
            bw.Write((ushort)Class.BaseFields.Count());
            bw.Write((ushort)0);
        }

        public override void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            br.BaseStream.Position = _definitionsPtr;

            for (int i = 0; i < NumDefinitions; i++)
            {
                AttribDefinition64 definition = new AttribDefinition64();
                definition.Read(context, br);

                if ((definition.Flags & DefinitionFlags.IsStatic) != 0)
                {
                    throw new Exception("Legacy format does not support static fields");
                }

                VltClassField field = new VltClassField(
                    definition.Key,
                    HashManager.ResolveVlt(definition.Key),
                    HashManager.ResolveVlt(definition.Type),
                    definition.Flags,
                    definition.Alignment,
                    definition.Size,
                    definition.MaxCount,
                    definition.Offset);

                Class.Fields.Add(definition.Key, field);
            }

            context.Database.AddClass(Class);
        }

        public override void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _dstDefinitionsPtr = bw.BaseStream.Position;

            foreach (var (_, field) in Class.Fields.OrderBy(f => f.Key))
            {
                AttribDefinition64 definition = new AttribDefinition64();
                definition.Key = Vlt64Hasher.Hash(field.Name);
                definition.Type = Vlt64Hasher.Hash(field.TypeName);
                definition.Flags = field.Flags;
                definition.Size = field.Size;
                definition.MaxCount = field.MaxCount;
                definition.Offset = field.Offset;
                definition.Alignment = field.Alignment;

                definition.Write(context, bw);
            }
        }

        public override void AddPointers(VaultSaveContext context)
        {
            context.AddPointer(_srcDefinitionsPtr, _dstDefinitionsPtr, true);
        }

        private int ComputeBaseSize()
        {
            int rfs = 0;
            foreach (var baseField in Class.BaseFields)
            {
                if (rfs % baseField.Alignment != 0)
                {
                    rfs += baseField.Alignment - rfs % baseField.Alignment;
                }

                if (baseField.IsArray)
                {
                    rfs += 8;
                    rfs += baseField.Size * baseField.MaxCount;
                }
                else
                {
                    rfs += baseField.Size;
                }
            }

            return rfs;
        }
    }
}