// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:18 PM.

using VaultLib.Core.Types;

namespace VaultLib.Core.Data
{
    /// <summary>
    ///     A field in VLT is like a column in a SQL table.
    ///     A field has a name (e.g. MODEL), a type (e.g. Attrib::StringKey), flags (e.g. IsArray | InLayout), and other properties.
    ///     Fields can be BASE fields (required) or ATTRIBUTE fields (optional).
    /// </summary>
    /// <remarks>In the version of AttribSys used from 2006 onwards, fields can have "static" values - a static field cannot occur in a collection. It has one value.</remarks>
    public class VltClassField
    {
        /// <summary>
        /// Gets the field's numeric (hashed) key.
        /// </summary>
        /// <remarks>This is the VLT(32 or 64) hash of <see cref="Name"/>.</remarks>
        public ulong Key { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <example>MODEL</example>
        public string Name { get; }

        /// <summary>
        /// Gets the type ID of the field.
        /// </summary>
        /// <example>Attrib::StringKey</example>
        public string TypeName { get; }

        /// <summary>
        /// Gets the field's flags.
        /// </summary>
        public DefinitionFlags Flags { get; }

        /// <summary>
        /// Gets the field's alignment.
        /// </summary>
        public int Alignment { get; }

        /// <summary>
        /// Gets the field's data size.
        /// </summary>
        public ushort Size { get; }

        /// <summary>
        /// Gets the maximum number of instances of this field.
        /// </summary>
        /// <remarks>Only applies to arrays.</remarks>
        public ushort MaxCount { get; }

        /// <summary>
        /// Gets the field's data offset.
        /// </summary>
        /// <remarks>Only applies to required fields.</remarks>
        public ushort Offset { get; }

        /// <summary>
        /// Gets or sets the static value of the field.
        /// </summary>
        public VLTBaseType StaticValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VltClassField"/> class.
        /// </summary>
        /// <param name="key">The field's hashed key.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="typeName">The type ID of the field.</param>
        /// <param name="flags">The field's flags.</param>
        /// <param name="alignment">The field's alignment.</param>
        /// <param name="size">The field's data size.</param>
        /// <param name="maxCount">The maximum number of instances of the field.</param>
        /// <param name="offset">The field's data offset.</param>
        public VltClassField(ulong key, string name, string typeName, DefinitionFlags flags, int alignment, ushort size,
            ushort maxCount, ushort offset)
        {
            Name = name;
            TypeName = typeName;
            Flags = flags;
            Alignment = alignment;
            Size = size;
            MaxCount = maxCount;
            Offset = offset;
            Key = key;
        }

        #region Helpers

        /// <summary>
        /// Gets a value indicating whether the field is a static field.
        /// </summary>
        public bool IsStatic => (Flags & DefinitionFlags.IsStatic) != 0;

        /// <summary>
        /// Gets a value indicating whether the field is an array field.
        /// </summary>
        public bool IsArray => (Flags & DefinitionFlags.Array) != 0;

        /// <summary>
        /// Gets a value indicating whether the field is a required field.
        /// </summary>
        public bool IsInLayout => (Flags & DefinitionFlags.InLayout) != 0;

        #endregion
    }
}