// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:21 AM.

using System.Collections.Generic;
using System.Linq;

namespace VaultLib.Core.Data
{
    /// <summary>
    ///     A class in VLT is like a table in a SQL database.
    ///     A class has fields, which can each have different properties.
    ///     A class also has collections, which are like rows in a table.
    /// </summary>
    public class VltClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VltClass"/> class.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        public VltClass(string name)
        {
            Name = name;
            Fields = new Dictionary<ulong, VltClassField>();
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of fields that are part of the class.
        /// </summary>
        public Dictionary<ulong, VltClassField> Fields { get; }

        /// <summary>
        /// Finds the field with the given name in the class.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The <see cref="VltClassField"/> instance for the field.</returns>
        public VltClassField this[string name] => FindField(name);

        /// <summary>
        /// Finds the field with the given name in the class.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The <see cref="VltClassField"/> instance for the field.</returns>
        public VltClassField FindField(string name) => Fields.Values.First(f => f.Name == name);

        /// <summary>
        /// Gets a value indicating if there is a field with the given name within the class.
        /// </summary>
        /// <param name="name">The name of the field to search for.</param>
        /// <returns><c>true</c> if the field exists; otherwise, <c>false</c></returns>
        public bool HasField(string name) => Fields.Values.Any(f => f.Name == name);

        /// <summary>
        /// Finds the field with the given key in the class.
        /// </summary>
        /// <param name="key">The key of the field to find.</param>
        /// <returns>The <see cref="VltClassField"/> instance for the field.</returns>
        public VltClassField this[ulong key] => FindField(key);

        /// <summary>
        /// Finds the field with the given key in the class.
        /// </summary>
        /// <param name="key">The key of the field to find.</param>
        /// <returns>The <see cref="VltClassField"/> instance for the field.</returns>
        public VltClassField FindField(ulong key) => Fields[key];

        /// <summary>
        /// Gets a value indicating if there is a field with the given key within the class.
        /// </summary>
        /// <param name="key">The key of the field to search for.</param>
        /// <returns><c>true</c> if the field exists; otherwise, <c>false</c></returns>
        public bool HasField(ulong key) => Fields.ContainsKey(key);

        /// <summary>
        /// Returns the field with the given key, if it exists.
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="field">A reference to a <see cref="VltClassField"/> that will be populated.</param>
        /// <returns><c>true</c> if a field was found; otherwise, <c>false</c></returns>
        public bool TryGetField(ulong key, out VltClassField field) => Fields.TryGetValue(key, out field);

        #region Helpers

        /// <summary>
        /// Gets an enumerator of every required field in the class.
        /// </summary>
        public IEnumerable<VltClassField> BaseFields => from field in Fields.Values where field.IsInLayout orderby field.Offset select field;

        /// <summary>
        /// Gets an enumerator of every static field in the class.
        /// </summary>
        public IEnumerable<VltClassField> StaticFields => from field in Fields.Values where field.IsStatic orderby field.Offset select field;

        /// <summary>
        /// Gets a value indicating whether the class has any base fields.
        /// </summary>
        public bool HasBaseFields => BaseFields.Any();

        /// <summary>
        /// Gets a value indicating whether the class has any static fields.
        /// </summary>
        public bool HasStaticFields => StaticFields.Any();

        #endregion
    }
}