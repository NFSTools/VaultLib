// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Core;

/// <summary>
///     Provides a facility for mapping type names to actual types.
/// </summary>
public class TypeRegistry<TKey> where TKey : struct, IKey<TKey>
{
    private readonly Database<TKey> _database;
    private readonly ReadOnlyDictionary<(TKey, TKey), Type> _fieldOverrides;
    private readonly ReadOnlyDictionary<TKey, Type> _typeDictionary;
    private readonly ReadOnlyDictionary<Type, ObjectActivator<object>> _activators;
    private readonly ReadOnlyDictionary<Type, TypeReader<TKey>> _readers;
    private readonly ReadOnlyDictionary<Type, TypeWriter<TKey>> _writers;

    internal TypeRegistry(
        Database<TKey> database,
        ReadOnlyDictionary<(TKey, TKey), Type> fieldOverrides,
        ReadOnlyDictionary<TKey, Type> typeDictionary,
        ReadOnlyDictionary<Type, ObjectActivator<object>> activators,
        ReadOnlyDictionary<Type, TypeReader<TKey>> readers,
        ReadOnlyDictionary<Type, TypeWriter<TKey>> writers)
    {
        _database = database;
        _fieldOverrides = fieldOverrides;
        _typeDictionary = typeDictionary;
        _activators = activators;
        _readers = readers;
        _writers = writers;
    }

    public object ConstructTypeInstance(Type type)
    {
        return _activators[type]();
    }

    public object ReadFieldValue(VaultReadContext<TKey> readContext, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader binaryReader)
    {
        var vltClassField = fieldContext.Field;
        var type = ResolveFieldType(vltClassField);
        if (vltClassField.IsArray)
        {
            var array = new VltArrayType<TKey>(vltClassField, type);
            array.Read(readContext, fieldContext, binaryReader);
            return array;
        }

        return ReadTypeInstance(readContext, fieldContext, binaryReader);
    }

    public object ReadTypeInstance(VaultReadContext<TKey> readContext, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader binaryReader)
    {
        var vltClassField = fieldContext.Field;
        var type = ResolveFieldType(vltClassField);
        return ReadTypeInstance(readContext, fieldContext, binaryReader, type);
    }

    public object ReadTypeInstance(VaultReadContext<TKey> readContext, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader binaryReader, Type type)
    {
        var init = ConstructTypeInstance(type);
        return _readers[type](init, readContext, fieldContext, binaryReader);
    }

    public void WriteFieldValue(object instance,
        VaultWriteContext<TKey> writeContext, FieldReadWriteContext<TKey> fieldContext, BinaryWriter binaryWriter)
    {
        var vltClassField = fieldContext.Field;
        if (vltClassField.IsArray)
        {
            var array = (VltArrayType<TKey>)instance;
            array.Write(writeContext, fieldContext, binaryWriter);
        }
        else
        {
            WriteTypeInstance(vltClassField, instance, writeContext, fieldContext, binaryWriter);
        }
    }

    public void WriteTypeInstance(VltClassField<TKey> vltClassField,
        object instance,
        VaultWriteContext<TKey> writeContext, FieldReadWriteContext<TKey> fieldContext, BinaryWriter binaryWriter)
    {
        var type = ResolveFieldType(vltClassField);

        WriteTypeInstance(instance, writeContext, fieldContext, binaryWriter, type);
    }

    public void WriteTypeInstance(object instance, VaultWriteContext<TKey> writeContext,
        FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter binaryWriter, Type type)
    {
        Debug.Assert(instance.GetType() == type, "instance.GetType() == type");
        _writers[type](instance, writeContext, fieldContext, binaryWriter);
    }

    public Type ResolveType(string typeId)
    {
        if (_typeDictionary.TryGetValue(TKey.FromString(typeId), out var type))
            return type;

        throw new KeyNotFoundException($"Type '{typeId}' is not registered");
    }

    public Type ResolveFieldType(VltClassField<TKey> field)
    {
        return ResolveFieldType(field.Class.Key, field.Key, field.TypeKey);
    }

    public Type ResolveFieldType(TKey classKey, TKey fieldKey, TKey fieldTypeKey)
    {
        return _fieldOverrides.TryGetValue((classKey, fieldKey), out var type)
            ? type
            : ResolveType(fieldTypeKey);
    }

    private Type ResolveType(TKey key)
    {
        if (_typeDictionary.TryGetValue(key, out var type))
            return type;

        var dbType = _database.Types.Find(t => TKey.FromString(t.Name) == key);

        if (dbType != null)
            throw new KeyNotFoundException($"Type {dbType.Name} (key: {key}) is not registered");

        throw new KeyNotFoundException($"Type {key} is not registered");
    }
}