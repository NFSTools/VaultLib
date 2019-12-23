// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:19 PM.

using CoreLibraries.GameUtilities;
using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var mKey = br.ReadUInt32(); // 4
            var mClass = br.ReadUInt32(); // 8
            var mParent = br.ReadUInt32(); // 12
            var mTableReserve = br.ReadUInt32(); // 16
            br.ReadUInt32(); // 20
            var mNumEntries = br.ReadUInt32(); // 24
            var mNumTypes = br.ReadUInt32(); // 28
            _layoutPointer = br.ReadPointer(); // 32

            Debug.Assert(mTableReserve == mNumEntries);

            Collection = new VltCollection(vault, vault.Database.FindClass(HashManager.ResolveVLT(mClass)), HashManager.ResolveVLT(mKey));

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

            // TODO: ParentKey
            //Collection.ParentKey = mParent;
            ParentKey = HashManager.ResolveVLT(mParent);
            vault.Database.RowManager.AddCollection(Collection);
        }

        public override void Prepare(Vault vault)
        {
            List<KeyValuePair<string, VLTBaseType>> optionalDataColumns = (from pair in Collection.GetData()
                                                                           where !Collection.Class[pair.Key].IsInLayout
                                                                           select pair).ToList();

            _entries = new AttribEntry[optionalDataColumns.Count];
            _types = Collection.Class.BaseFields.Select(f => f.TypeName)
                .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeName))
                .Select(s => VLT32Hasher.Hash(s)).Distinct().ToArray();

            for (var index = 0; index < optionalDataColumns.Count; index++)
            {
                var optionalDataColumn = optionalDataColumns[index];
                var entry = new AttribEntry(Collection);

                entry.Key = VLT32Hasher.Hash(optionalDataColumn.Key);
                var vltClassField = Collection.Class[optionalDataColumn.Key];
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
            bw.Write(VLT32Hasher.Hash(Collection.Name));
            bw.Write(VLT32Hasher.Hash(Collection.Class.Name));
            //bw.Write((uint) (Collection.Parent?.Key ?? 0));
            bw.Write(Collection.Parent != null ? VLT32Hasher.Hash(Collection.Parent.Name) : 0u);
            bw.Write((uint)_entries.Length);
            bw.Write(0);
            bw.Write((uint)_entries.Length);
            bw.Write((uint)_types.Length);
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

        public override ulong GetExportID()
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
                    //Collection.Data[baseField.Name] = data;
                    Collection.SetDataValue(baseField.Name, data);
                }
            }

            foreach (var entry in _entries)
            {
                var optionalField = Collection.Class[entry.Key];

                if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
                {
                    throw new Exception("Congratulations. You have successfully broken this library. Please consult with your doctor for further instructions.");
                }

                if (entry.InlineData is VLTAttribType attribType)
                {
                    attribType.ReadPointerData(vault, br);
                    Collection.SetDataValue(optionalField.Name, attribType.Data);
                    //Collection.Data[optionalField.Name] = attribType.Data;
                }
                else
                {
                    Collection.SetDataValue(optionalField.Name, entry.InlineData);
                    //Collection.Data[optionalField.Name] = entry.InlineData;
                }
            }

            foreach (var dataEntry in Collection.GetData())
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

                Collection.GetDataValue(baseField.Name).Write(vault, bw);
                //Collection.Data[baseField.Name].Write(vault, bw);
            }

            foreach (var dataPair in Collection.GetData())
            {
                VltClassField field = Collection.Class[dataPair.Key];

                if (!field.IsInLayout)
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
                if (this.Collection.GetDataValue(baseField.Name) is IPointerObject pointerObject)
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