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
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports
{
    public class ModernCollectionLoad : BaseCollectionLoad
    {
        private uint _layoutPointer;
        private uint[] _types;
        private List<ModernAttribEntry> _entries;

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
            var mNumTypes = br.ReadUInt16();
            var mTypesLen = br.ReadUInt16();
            _layoutPointer = br.ReadPointer();

            Debug.Assert(mTableReserve == mNumEntries);

            Collection = new VLTCollection(vault, vault.Database.FindClass(mClass), HashManager.ResolveVLT(mKey), mKey);

            Debug.Assert(mTypesLen >= mNumTypes);

            _types = new uint[mNumTypes];
            for (var i = 0; i < mNumTypes; i++)
            {
                _types[i] = (br.ReadUInt32());
            }

            for (var i = 0; i < mTypesLen - mNumTypes; i++)
            {
                br.ReadUInt32();
            }

            _entries = new List<ModernAttribEntry>();

            for (var i = 0; i < mNumEntries; i++)
            {
                var attribEntry = new ModernAttribEntry(Collection);

                attribEntry.Read(vault, br);
                _entries.Add(attribEntry);
            }

            Collection.ParentKey = mParent;
            vault.Database.AddRow(Collection);
        }

        public override void Prepare()
        {
            List<KeyValuePair<ulong, VLTBaseType>> optionalDataColumns = (from pair in Collection.DataRow
                                                                          let field = Collection.Class.Fields[pair.Key]
                                                                          where field.IsOptional
                                                                          orderby field.Name
                                                                          select pair).ToList();

            _entries = new List<ModernAttribEntry>();
            _types = Collection.Class.BaseFields.Select(f => f.TypeName)
                .Concat(optionalDataColumns.Select(c => Collection.Class.Fields[c.Key].TypeName))
                .Select(s => VLT32Hasher.Hash(s)).Distinct().ToArray();

            for (var index = 0; index < optionalDataColumns.Count; index++)
            {
                var optionalDataColumn = optionalDataColumns[index];
                var entry = new ModernAttribEntry(Collection);
                var vltClassField = Collection.Class.Fields[optionalDataColumn.Key];

                entry.Key = (uint)optionalDataColumn.Key;
                entry.TypeIndex = (ushort)Array.IndexOf(_types,
                    VLT32Hasher.Hash(vltClassField.TypeName));
                entry.EntryFlags = 0;
                entry.NodeFlags = ModernAttribEntry.NodeFlagsEnum.Default;

                if (entry.IsInline())
                {
                    entry.InlineData = optionalDataColumn.Value;
                    entry.NodeFlags |= ModernAttribEntry.NodeFlagsEnum.IsInline;
                }
                else
                {
                    entry.InlineData =
                        new VLTAttribType(Collection.Class, Collection.Class.Fields[entry.Key], Collection)
                        { Data = optionalDataColumn.Value };
                }

                if (vltClassField.IsArray)
                {
                    entry.NodeFlags |= ModernAttribEntry.NodeFlagsEnum.IsArray;
                }

                if ((vltClassField.Flags & DefinitionFlags.kHasHandler) != 0)
                {
                    entry.NodeFlags |= ModernAttribEntry.NodeFlagsEnum.HasHandler;
                }

                _entries.Add(entry);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint)Collection.Key);
            bw.Write((uint)Collection.Class.NameHash);
            bw.Write((uint)(Collection.Parent?.Key ?? 0));
            bw.Write(_entries.Count);
            bw.Write(0);
            bw.Write(_entries.Count);

            ushort numTypes = (ushort)_types.Length;
            ushort typesLen = (ushort)(numTypes % 2 == 0 ? numTypes : numTypes + 1);

            bw.Write(numTypes);
            bw.Write(typesLen);
            _srcLayoutPtr = bw.BaseStream.Position;
            bw.Write(0);

            foreach (var type in _types)
            {
                bw.Write(type);
            }

            if (typesLen != numTypes)
                bw.Write(0);

            foreach (var attribEntry in _entries)
            {
                attribEntry.Write(vault, bw);
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

                    if (br.BaseStream.Position - _layoutPointer != baseField.Offset)
                    {
                        throw new Exception($"trying to read field {baseField.Name} at offset {br.BaseStream.Position - _layoutPointer:X}, need to be at {baseField.Offset:X}");
                    }

                    VLTBaseType data = TypeRegistry.CreateInstance(vault.Database.Game, Collection.Class, baseField, Collection);
                    long startPos = br.BaseStream.Position;
                    data.Read(vault, br);
                    long endPos = br.BaseStream.Position;

                    if (data is PrimitiveTypeBase)
                        br.BaseStream.Position = startPos + baseField.Size;

                    if (!(data is VLTArrayType) && !(data is PrimitiveTypeBase))
                    {
                        if (endPos - startPos != baseField.Size)
                        {
                            throw new Exception($"read {endPos - startPos} bytes, needed to read {baseField.Size}");
                        }
                    }
                    Collection.DataRow[baseField.Key] = data;
                }
            }

            foreach (var entry in _entries)
            {
                var optionalField = Collection.Class.Fields[entry.Key];

                if ((optionalField.Flags & DefinitionFlags.kIsStatic) != 0)
                {
                    throw new Exception("Encountered static field as an entry. Processing will not continue.");
                }

                if ((optionalField.Flags & DefinitionFlags.kHasHandler) != 0)
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.HasHandler) ==
                                 ModernAttribEntry.NodeFlagsEnum.HasHandler);
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.HasHandler) == 0);
                }

                if ((optionalField.Flags & DefinitionFlags.kArray) != 0)
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.IsArray) ==
                                 ModernAttribEntry.NodeFlagsEnum.IsArray);
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.IsArray) == 0);
                }

                if (entry.InlineData is VLTAttribType attribType)
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.IsInline) == 0);
                    attribType.ReadPointerData(vault, br);
                    Collection.DataRow[optionalField.Key] = attribType.Data;
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & ModernAttribEntry.NodeFlagsEnum.IsInline) ==
                                 ModernAttribEntry.NodeFlagsEnum.IsInline);
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

            if (Collection.Class.HasBaseFields)
            {
                // align to 4 bytes for layout data
                bw.AlignWriter(4);
            }
            else
            {
                // there is no layout data but we might still be
                // in a bad position, so align to 2 bytes
                bw.AlignWriter(2);
            }
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