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
    public class CollectionLoad64 : BaseCollectionLoad
    {
        private uint _layoutPointer;
        private ulong[] _types;
        private List<AttribEntry64> _entries;

        private long _srcLayoutPtr;
        private long _dstLayoutPtr;

        public override ulong GetTypeId()
        {
            return VLT64Hasher.Hash("Attrib::CollectionLoadData");
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            var mKey = br.ReadUInt64();
            var mClass = br.ReadUInt64();
            var mParent = br.ReadUInt64();
            var mTableReserve = br.ReadUInt32();
            br.ReadUInt32();
            var mNumEntries = br.ReadUInt32();
            var mNumTypes = br.ReadUInt16();
            var mTypesLen = br.ReadUInt16();
            _layoutPointer = br.ReadPointer();

            Debug.Assert(mTableReserve == mNumEntries);

            Collection = new VltCollection(vault, vault.Database.FindClass(HashManager.ResolveVLT(mClass)), HashManager.ResolveVLT(mKey));

            Debug.Assert(mTypesLen >= mNumTypes);

            _types = new ulong[mNumTypes];
            for (var i = 0; i < mNumTypes; i++)
            {
                _types[i] = (br.ReadUInt64());
            }

            for (var i = 0; i < mTypesLen - mNumTypes; i++)
            {
                br.ReadUInt64();
            }

            _entries = new List<AttribEntry64>();

            for (var i = 0; i < mNumEntries; i++)
            {
                var attribEntry = new AttribEntry64(Collection);

                attribEntry.Read(vault, br);

                // save pos
                long pos = br.BaseStream.Position;
                var readData = attribEntry.ReadData(vault, br);
                br.BaseStream.Position = pos;

                if (!readData)
                {
                    continue;
                }

                _entries.Add(attribEntry);
            }

            // TODO: ParentKey
            //Collection.ParentKey = mParent;
            vault.Database.RowManager.AddCollection(Collection);
        }

        public override void Prepare(Vault vault)
        {
            List<KeyValuePair<string, VLTBaseType>> optionalDataColumns = (from pair in Collection.GetData()
                                                                          let field = Collection.Class[pair.Key]
                                                                          where !field.IsInLayout
                                                                          orderby field.Name
                                                                          select pair).ToList();

            _entries = new List<AttribEntry64>();
            _types = Collection.Class.BaseFields.Select(f => f.TypeName)
                .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeName))
                .Select(s => VLT64Hasher.Hash(s)).Distinct().ToArray();

            for (var index = 0; index < optionalDataColumns.Count; index++)
            {
                var optionalDataColumn = optionalDataColumns[index];
                var entry = new AttribEntry64(Collection);
                var vltClassField = Collection.Class[optionalDataColumn.Key];

                entry.Key = VLT64Hasher.Hash(optionalDataColumn.Key);
                entry.TypeIndex = (ushort)Array.IndexOf(_types,
                    VLT32Hasher.Hash(vltClassField.TypeName));
                entry.EntryFlags = 0;
                entry.NodeFlags = NodeFlagsEnum.Default;

                if (entry.IsInline())
                {
                    entry.InlineData = optionalDataColumn.Value;
                    entry.NodeFlags |= NodeFlagsEnum.IsInline;
                }
                else
                {
                    entry.InlineData =
                        new VLTAttribType(Collection.Class, vltClassField, Collection)
                        { Data = optionalDataColumn.Value };
                }

                if (vltClassField.IsArray)
                {
                    entry.NodeFlags |= NodeFlagsEnum.IsArray;
                }

                if ((vltClassField.Flags & DefinitionFlags.HasHandler) != 0)
                {
                    entry.NodeFlags |= NodeFlagsEnum.HasHandler;
                }

                _entries.Add(entry);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(VLT64Hasher.Hash(Collection.Name));
            bw.Write(VLT64Hasher.Hash(Collection.Class.Name));
            bw.Write(Collection.Parent != null ? VLT64Hasher.Hash(Collection.Parent.Name) : 0L);
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

        public override ulong GetExportID()
        {
            return VLT64Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
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

                    VLTBaseType data = TypeRegistry.CreateInstance(vault.Database.Options.GameId, Collection.Class, baseField, Collection);
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
                    Collection.SetDataValue(baseField.Name, data);
                    //Collection.Data[baseField.Name] = data;
                }
            }

            foreach (var entry in _entries)
            {
                var optionalField = Collection.Class[entry.Key];

                if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
                {
                    throw new Exception("Encountered static field as an entry. Processing will not continue.");
                }

                if ((optionalField.Flags & DefinitionFlags.HasHandler) != 0)
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.HasHandler) ==
                                 NodeFlagsEnum.HasHandler);
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.HasHandler) == 0);
                }

                if ((optionalField.Flags & DefinitionFlags.Array) != 0)
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsArray) ==
                                 NodeFlagsEnum.IsArray);
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsArray) == 0);
                }

                if (entry.InlineData is VLTAttribType attribType)
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) == 0);
                    attribType.ReadPointerData(vault, br);
                    Collection.SetDataValue(optionalField.Name, attribType.Data);
                    //Collection.Data[optionalField.Name] = attribType.Data;
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) ==
                                 NodeFlagsEnum.IsInline);
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