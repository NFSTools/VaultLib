// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:19 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.GameUtilities;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports
{
    public class CollectionLoad : BaseCollectionLoad
    {
        private uint _layoutPointer;
        private uint[] _types;
        private AttribEntry[] _entries;

        private long _srcLayoutPtr;
        private long _dstLayoutPtr;

        public override void Read(Vault vault, BinaryReader br)
        {
            var mKey = br.ReadUInt32();
            var mClass = br.ReadUInt32();
            var mParent = br.ReadUInt32();
            var mTableReserve = br.ReadUInt32();
            br.ReadUInt32();
            var mNumEntries = br.ReadUInt32();
            var mNumTypes = br.ReadUInt32();
            _layoutPointer = br.ReadPointer();

            Debug.Assert(mTableReserve == mNumEntries);

            Collection = new VLTCollection(vault, vault.Database.FindClass(HashManager.ResolveVLT(mClass)), HashManager.ResolveVLT(mKey), mKey);

            _types = new uint[mNumTypes];
            for (var i = 0; i < mNumTypes; i++)
            {
                _types[i] = (br.ReadUInt32());
            }

            _entries = new AttribEntry[mNumEntries];

            for (var i = 0; i < mNumEntries; i++)
            {
                var attribEntry = new AttribEntry(Collection);
                attribEntry.Read(vault, br);
                _entries[i] = attribEntry;
            }

            Collection.ParentKey = mParent;
            vault.Database.RowManager.AddCollection(Collection);
        }

        public override void Prepare()
        {
            List<KeyValuePair<ulong, VLTBaseType>> optionalDataColumns = (from pair in Collection.DataRow
                                                                          where Collection.Class.Fields[pair.Key].IsOptional
                                                                          select pair).ToList();

            _entries = new AttribEntry[optionalDataColumns.Count];
            _types = Collection.Class.BaseFields.Select(f => f.TypeName)
                .Concat(optionalDataColumns.Select(c => Collection.Class.Fields[c.Key].TypeName))
                .Select(s => VLT32Hasher.Hash(s)).Distinct().ToArray();

            for (var index = 0; index < optionalDataColumns.Count; index++)
            {
                var optionalDataColumn = optionalDataColumns[index];
                var entry = new AttribEntry(Collection);

                entry.Key = (uint)optionalDataColumn.Key;
                var vltClassField = Collection.Class.Fields[optionalDataColumn.Key];
                entry.TypeIndex = (ushort)Array.IndexOf(_types,
                    VLT32Hasher.Hash(vltClassField.TypeName));
                entry.NodeFlags = AttribEntry.NodeFlagsEnum.Default;

                if (entry.IsInline())
                {
                    entry.InlineData = optionalDataColumn.Value;
                    entry.NodeFlags |= AttribEntry.NodeFlagsEnum.IsInline;
                }
                else
                {
                    entry.InlineData = new VLTAttribType(Collection.Class, vltClassField, Collection)
                    {
                        Data = optionalDataColumn.Value
                    };
                }

                if (vltClassField.IsArray)
                {
                    entry.NodeFlags |= AttribEntry.NodeFlagsEnum.IsArray;
                }

                _entries[index] = entry;
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint) Collection.Key);
            bw.Write(VLT32Hasher.Hash(Collection.Name));
            bw.Write((uint) (Collection.Parent?.Key ?? 0));
            bw.Write((uint) _entries.Length);
            bw.Write(0);
            bw.Write((uint) _entries.Length);
            bw.Write((uint) _types.Length);
            _srcLayoutPtr = bw.BaseStream.Position;
            bw.Write(0);

            foreach (var type in _types)
            {
                bw.Write(type);
            }

            foreach (var entry in _entries)
            {
                entry.Write(vault, bw);
            }
        }

        public override uint GetExportID()
        {
            return VLT32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
        }

        public override void ReadPointerData(Vault vault, BinaryReader br)
        {
            if (_layoutPointer != 0)
            {
                br.BaseStream.Position = _layoutPointer;

                foreach (var baseField in Collection.Class.BaseFields)
                {
                    br.AlignReader(baseField.Alignment);

                    VLTBaseType data =
                        TypeRegistry.CreateInstance(vault.Database.Options.GameId, Collection.Class, baseField, Collection);
                    long startPos = br.BaseStream.Position;
                    data.Read(vault, br);
                    long endPos = br.BaseStream.Position;
                    if (!(data is VLTArrayType))
                        Debug.Assert(endPos - startPos == baseField.Size);
                    Collection.DataRow[baseField.Key] = data;
                }
            }

            foreach (var entry in _entries)
            {
                var optionalField = Collection.Class.Fields[entry.Key];

                if ((optionalField.Flags & DefinitionFlags.kIsStatic) != 0)
                {
                    throw new Exception("Congratulations. You have successfully broken this library. Please consult with your doctor for further instructions.");
                }

                if (entry.InlineData is VLTAttribType attribType)
                {
                    attribType.ReadPointerData(vault, br);
                    Collection.DataRow[optionalField.Key] = attribType.Data;
                }
                else
                {
                    Collection.DataRow[optionalField.Key] = entry.InlineData;
                }
            }

            foreach (var dataEntry in Collection.DataRow)
            {
                if (dataEntry.Value is IPointerObject pointerObject)
                    pointerObject.ReadPointerData(vault, br);
            }
        }

        public override void WritePointerData(Vault vault, BinaryWriter bw)
        {
            foreach (var baseField in Collection.Class.BaseFields)
            {
                bw.AlignWriter(baseField.Alignment);
                if (_dstLayoutPtr == 0)
                {
                    _dstLayoutPtr = bw.BaseStream.Position;
                }

                if (bw.BaseStream.Position - _dstLayoutPtr != baseField.Offset)
                {
                    throw new Exception("incorrect offset");
                }

                Collection.DataRow[baseField.Key].Write(vault, bw);
            }

            foreach (var dataPair in Collection.DataRow)
            {
                VLTClassField field = Collection.Class.Fields[dataPair.Key];

                if (field.IsOptional)
                {
                    var entry = _entries.First(e => e.Key == field.Key);

                    if (!(entry.InlineData is IPointerObject pointerObject)) continue;

                    bw.AlignWriter(field.Alignment);
                    pointerObject.WritePointerData(vault, bw);
                }
                else
                {
                    if (!(dataPair.Value is IPointerObject pointerObject)) continue;

                    bw.AlignWriter(field.Alignment);
                    pointerObject.WritePointerData(vault, bw);
                }
            }

            bw.AlignWriter(Collection.Class.HasBaseFields ? 4 : 2);
        }

        public override void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_srcLayoutPtr, _dstLayoutPtr, true);

            foreach (var baseField in Collection.Class.BaseFields)
            {
                if (this.Collection.DataRow[baseField.Key] is IPointerObject pointerObject)
                {
                    pointerObject.AddPointers(vault);
                }
            }

            foreach (var entry in _entries)
            {
                if (entry.InlineData is IPointerObject pointerObject)
                {
                    pointerObject.AddPointers(vault);
                }
            }
        }
    }
}