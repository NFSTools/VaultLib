using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase
{
    public abstract class ModernCollectionLoadBase<TAttribEntry> : BaseCollectionLoad
        where TAttribEntry : AttribEntryBase
    {
        protected uint LayoutPointer { get; set; }

        protected uint[] Types { get; set; }

        protected List<TAttribEntry> Entries { get; set; }

        protected long SourceLayoutPointer { get; set; }

        private long DestinationLayoutPointer { get; set; }

        public override void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            if (LayoutPointer != 0)
            {
                br.BaseStream.Position = LayoutPointer;

                foreach (var baseField in Collection.Class.BaseFields)
                {
                    var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
                    br.AlignReader(baseField.Alignment);

                    if (br.BaseStream.Position - LayoutPointer != baseField.Offset)
                    {
                        throw new Exception(
                            $"trying to read field {baseField.Name} at offset {br.BaseStream.Position - LayoutPointer:X}, need to be at {baseField.Offset:X}");
                    }

                    var startPos = br.BaseStream.Position;
                    var data =
                        context.Database.TypeRegistry.ReadFieldValue(context,
                            fieldContext, br);
                    var endPos = br.BaseStream.Position;

                    if (data is PrimitiveTypeBase)
                        br.BaseStream.Position = startPos + baseField.Size;

                    if (!(data is VltArrayType) && !(data is PrimitiveTypeBase))
                    {
                        if (endPos - startPos != baseField.Size)
                        {
                            throw new Exception($"read {endPos - startPos} bytes, needed to read {baseField.Size}");
                        }
                    }

                    Collection.SetRawValue(baseField.Name, data);
                }
            }

            foreach (var entry in Entries)
            {
                var optionalField = Collection.Class[entry.Key];
                var fieldContext = new FieldReadWriteContext(Collection.Class, optionalField, Collection);

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

                if (entry.InlineData is VltAttribType attribType)
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) == 0);
                    attribType.ReadPointerData(context, fieldContext, br);
                    Collection.SetRawValue(optionalField.Name, attribType.Data);
                }
                else
                {
                    Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) ==
                                 NodeFlagsEnum.IsInline);
                    Collection.SetRawValue(optionalField.Name, entry.InlineData);
                }
            }

            foreach (var dataEntry in Collection.GetData())
            {
                var fieldContext =
                    new FieldReadWriteContext(Collection.Class, Collection.Class[dataEntry.Key], Collection);
                if (dataEntry.Value is IVltPointerObject vltPointerObject)
                {
                    vltPointerObject.ReadPointerData(context, fieldContext, br);
                }
            }
        }

        public override void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);

                bw.AlignWriter(baseField.Alignment);
                if (DestinationLayoutPointer == 0)
                {
                    DestinationLayoutPointer = bw.BaseStream.Position;
                }

                if (bw.BaseStream.Position - DestinationLayoutPointer != baseField.Offset)
                {
                    throw new Exception(
                        $"incorrect offset before writing {Collection.ShortPath}[{baseField.Name}]; expected to be at {baseField.Offset} but we are at {bw.BaseStream.Position - DestinationLayoutPointer}");
                }

                var rawValue = Collection.GetRawValue(baseField.Name);
                context.Database.TypeRegistry.WriteFieldValue(rawValue, context, fieldContext, bw);
            }

            foreach (var dataPair in Collection.GetData())
            {
                VltClassField field = Collection.Class[dataPair.Key];

                var fieldContext = new FieldReadWriteContext(Collection.Class, field, Collection);

                if (!field.IsInLayout)
                {
                    var entry = Entries.First(e => e.Key == field.Key);

                    if (entry.InlineData is IVltPointerObject vltPointerObject)
                    {
                        bw.AlignWriter(field.Alignment);
                        vltPointerObject.WritePointerData(context, fieldContext, bw);
                    }
                }
                else
                {
                    if (dataPair.Value is IVltPointerObject vltPointerObject)
                    {
                        bw.AlignWriter(field.Alignment);
                        vltPointerObject.WritePointerData(context, fieldContext, bw);
                    }
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

        public override void AddPointers(VaultWriteContext context)
        {
            context.AddPointer(SourceLayoutPointer, DestinationLayoutPointer, true);

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
                var rawValue = Collection.GetRawValue(baseField.Name);

                if (rawValue is IVltPointerObject vltPointerObject)
                {
                    vltPointerObject.AddPointers(context, fieldContext);
                }
            }

            foreach (var entry in Entries)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, Collection.Class[entry.Key], Collection);
                if (entry.InlineData is IVltPointerObject vltPointerObject)
                {
                    vltPointerObject.AddPointers(context, fieldContext);
                }
            }
        }
    }
}