// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:21 AM.

using System.Collections.Generic;
using System.Linq;

namespace VaultLib.Core.Data
{
    /// <summary>
    /// A class in VLT is like a table in a SQL database.
    /// A class has fields, which can each have different properties.
    /// A class also has collections, which are like rows in a table.
    /// </summary>
    public class VLTClass
    {
        public string Name { get; }

        public Dictionary<ulong, VLTClassField> Fields { get; }

        public IEnumerable<VLTClassField> BaseFields => from vltClassField in Fields.Values
            where (vltClassField.Flags & DefinitionFlags.kInLayout) != 0
            orderby vltClassField.Offset
            select vltClassField;

        public IEnumerable<VLTClassField> OptionalFields => from vltClassField in Fields.Values
            where (vltClassField.Flags & DefinitionFlags.kInLayout) == 0
            select vltClassField;

        public IEnumerable<VLTClassField> StaticFields => from vltClassField in Fields.Values
            where (vltClassField.Flags & DefinitionFlags.kIsStatic) != 0
            orderby vltClassField.Offset
            select vltClassField;

        public ulong NameHash { get; }
        public uint SizeOfLayout { get; set; }
        public bool HasBaseFields => BaseFields.Any();
        public bool HasOptionalFields => OptionalFields.Any();
        public bool HasArrayFields => Fields.Values.Any(f => f.Flags.HasFlag(DefinitionFlags.kArray));
        public bool HasStaticFields => StaticFields.Any();

        public VLTClass(string name, ulong key)
        {
            this.Name = name;
            this.Fields = new Dictionary<ulong, VLTClassField>();
            this.NameHash = key;
        }

        public bool FieldExists(ulong key) => Fields.Any(f => f.Value.Key == key);
    }
}