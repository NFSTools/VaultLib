using System.IO;
using System.Linq;
using CoreLibraries.GameUtilities;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Hashing;
using VaultLib.Core.Structures;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports
{
    public class ClassLoad : BaseClassLoad
    {
        private uint ClassHash { get; set; }
        private int NumDefinitions { get; set; }

        private uint _definitionsPtr;
        private uint _staticDataPtr;

        private long _srcDefinitionsPtr;
        private long _srcStaticPtr;
        private long _dstDefinitionsPtr;
        private long _dstStaticPtr;

        public override void Read(Vault vault, BinaryReader br)
        {
            ClassHash = br.ReadUInt32();
            br.ReadUInt32(); // Collection reserve
            int mNumDefinitions = br.ReadInt32(); // Number of fields
            _definitionsPtr = br.ReadPointer();
            br.ReadUInt32(); // static size
            _staticDataPtr = br.ReadPointer();
            uint layoutSize = br.ReadUInt32(); // Total size of required fields
            br.ReadUInt16(); // can be 0
            br.ReadUInt16(); // Number of required fields

            if (_definitionsPtr == 0)
            {
                throw new InvalidDataException("Definitions pointer is NULL, this is not good!");
            }

            NumDefinitions = mNumDefinitions;
            Class = new VLTClass(HashManager.ResolveVLT(ClassHash), ClassHash, vault.Database);
            Class.SizeOfLayout = layoutSize;
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            int collectionReserve = (from collection in vault.SaveContext.Collections
                where collection.Class.Name == Class.Name
                select collection).Count();

            bw.Write((uint) Class.NameHash);
            bw.Write(collectionReserve);
            bw.Write(Class.Fields.Count);
            _srcDefinitionsPtr = bw.BaseStream.Position;
            bw.Write(0);
            int staticSize = ComputeStaticSize();
            bw.Write(staticSize);

            if (staticSize > 0)
            {
                _srcStaticPtr = bw.BaseStream.Position;
            }

            bw.Write(0);

            bw.Write(Class.SizeOfLayout);
            bw.Write((ushort) 0);
            bw.Write((ushort) Class.BaseFields.Count());
        }

        public override void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _definitionsPtr;

            for (int i = 0; i < NumDefinitions; i++)
            {
                AttribDefinition definition = new AttribDefinition();
                definition.Read(vault, br);

                VLTClassField field = new VLTClassField();
                field.Key = definition.Key;
                field.Name = HashManager.ResolveVLT((uint) definition.Key);
                field.TypeName = HashManager.ResolveVLT((uint) definition.Type);
                field.Flags = definition.Flags;
                field.Size = definition.Size;
                field.MaxCount = definition.MaxCount;
                field.Offset = definition.Offset;
                field.Alignment = definition.Alignment;

                Class.Fields.Add(definition.Key, field);
            }

            if (_staticDataPtr != 0)
            {
                br.BaseStream.Position = _staticDataPtr;

                foreach (VLTClassField staticField in Class.StaticFields)
                {
                    br.AlignReader(staticField.Alignment);
                    VLTBaseType staticData = TypeRegistry.CreateInstance(vault.Database.Game, Class, staticField, null);
                    staticData.Read(vault, br);
                    staticField.StaticValue = staticData;
                }
            }

            foreach (var staticField in Class.StaticFields)
            {
                if (staticField.StaticValue is IPointerObject pointerObject)
                    pointerObject.ReadPointerData(vault, br);
            }

            vault.Database.AddClass(Class);
        }

        public override void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _dstDefinitionsPtr = bw.BaseStream.Position;

            foreach (var field in Class.Fields.Values)
            {
                AttribDefinition definition = new AttribDefinition();
                definition.Key = field.Key;
                definition.Alignment = field.Alignment;
                definition.Flags = field.Flags;
                definition.MaxCount = field.MaxCount;
                definition.Offset = field.Offset;
                definition.Size = field.Size;
                definition.Type = VLT32Hasher.Hash(field.TypeName);
                definition.Write(vault, bw);
            }

            if (_srcStaticPtr != 0)
            {
                bw.AlignWriter(0x10);

                _dstStaticPtr = bw.BaseStream.Position;

                foreach (var staticField in Class.StaticFields)
                {
                    bw.AlignWriter(staticField.Alignment);
                    staticField.StaticValue.Write(vault, bw);
                }

                foreach (var staticField in Class.StaticFields)
                {
                    if (staticField.StaticValue is IPointerObject pointerObject)
                        pointerObject.WritePointerData(vault, bw);
                }
            }
        }

        public override void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_srcDefinitionsPtr, _dstDefinitionsPtr, true);

            if (_srcStaticPtr != 0 && _dstStaticPtr != 0)
            {
                vault.SaveContext.AddPointer(_srcStaticPtr, _dstStaticPtr, true);

                foreach (var staticField in Class.StaticFields)
                {
                    if (staticField.StaticValue is IPointerObject pointerObject)
                        pointerObject.AddPointers(vault);
                }
            }
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

                if ((baseField.Flags & DefinitionFlags.kArray) != 0)
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

        private int ComputeStaticSize()
        {
            int staticSize = 0;

            foreach (var vltClassField in Class.StaticFields)
            {
                if (staticSize % vltClassField.Alignment != 0)
                {
                    staticSize += vltClassField.Alignment - staticSize % vltClassField.Alignment;
                }

                staticSize += vltClassField.Size;
            }

            return staticSize;
        }
    }
}