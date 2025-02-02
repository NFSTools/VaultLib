// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:21 AM.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Data;

/// <summary>
///     A class in VLT is like a table in a SQL database.
///     A class has fields, which can each have different properties.
///     A class also has collections, which are like rows in a table.
/// </summary>
public class VltClass<TKey> where TKey : struct, IKey<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VltClass{TKey}"/> class.
    /// </summary>
    public VltClass(TKey key)
    {
        Fields = new Dictionary<TKey, VltClassField<TKey>>();
        Key = key;
    }

    public TKey Key { get; set; }

    /// <summary>
    /// Gets the list of fields that are part of the class.
    /// </summary>
    public Dictionary<TKey, VltClassField<TKey>> Fields { get; }

    /// <summary>
    /// Gets or sets the size of the collection layout of the class.
    /// </summary>
    public uint LayoutSize { get; set; }

    /// <summary>
    /// Gets or sets the size of the static layout of the class.
    /// </summary>
    public uint StaticSize { get; set; }

    /// <summary>
    /// Finds the field with a particular name.
    /// </summary>
    /// <param name="name">The name of the field to search for.</param>
    /// <returns>The field with the given name.</returns>
    public VltClassField<TKey> this[string name] => FindField(name);

    /// <summary>
    /// Finds the field with a particular name.
    /// </summary>
    /// <param name="name">The name of the field to search for.</param>
    /// <returns>The field with the given name.</returns>
    public VltClassField<TKey> FindField(string name) => FindField(TKey.FromString(name));

    /// <summary>
    /// Gets a value indicating if there is a field with the given name within the class.
    /// </summary>
    /// <param name="name">The name of the field to search for.</param>
    /// <returns><c>true</c> if the field exists; otherwise, <c>false</c></returns>
    public bool HasField(string name) => HasField(TKey.FromString(name));

    /// <summary>
    /// Finds the field with a particular key.
    /// </summary>
    /// <param name="key">The key of the field to find.</param>
    /// <returns>The field with the given key.</returns>
    public VltClassField<TKey> this[TKey key] => FindField(key);

    /// <summary>
    /// Finds the field with a particular key.
    /// </summary>
    /// <param name="key">The key of the field to find.</param>
    /// <returns>The field with the given key.</returns>
    public VltClassField<TKey> FindField(TKey key) => Fields[key];

    /// <summary>
    /// Gets a value indicating if there is a field with the given key within the class.
    /// </summary>
    /// <param name="key">The key of the field to search for.</param>
    /// <returns><c>true</c> if the field exists; otherwise, <c>false</c></returns>
    public bool HasField(TKey key) => Fields.ContainsKey(key);

    /// <summary>
    /// Returns the field with the given key, if it exists.
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="field">A reference to a <see cref="VltClassField{TKey}"/> that will be populated.</param>
    /// <returns><c>true</c> if a field was found; otherwise, <c>false</c></returns>
    public bool TryGetField(TKey key, [NotNullWhen(true)] out VltClassField<TKey>? field) =>
        Fields.TryGetValue(key, out field);

    /// <summary>
    /// Returns the field with the given name, if it exists.
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="field">A reference to a <see cref="VltClassField{TKey}"/> that will be populated.</param>
    /// <returns><c>true</c> if a field was found; otherwise, <c>false</c></returns>
    public bool TryGetField(string name, [NotNullWhen(true)] out VltClassField<TKey>? field)
    {
        return TryGetField(TKey.FromString(name), out field);
    }

    #region Helpers

    /// <summary>
    /// Gets an enumerator of every required field in the class.
    /// </summary>
    public IEnumerable<VltClassField<TKey>> BaseFields =>
        from field in Fields.Values where field.IsInLayout orderby field.Offset select field;

    /// <summary>
    /// Gets an enumerator of every static field in the class.
    /// </summary>
    public IEnumerable<VltClassField<TKey>> StaticFields =>
        from field in Fields.Values where field.IsStatic orderby field.Offset select field;

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